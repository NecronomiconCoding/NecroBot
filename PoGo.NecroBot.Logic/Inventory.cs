#region using directives

using PoGo.NecroBot.Logic.PoGoUtils;
using PoGo.NecroBot.Logic.State;
using POGOProtos.Data;
using POGOProtos.Data.Player;
using POGOProtos.Enums;
using POGOProtos.Inventory;
using POGOProtos.Inventory.Item;
using POGOProtos.Networking.Responses;
using POGOProtos.Settings.Master;
using PokemonGo.RocketAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Utils;

#endregion

namespace PoGo.NecroBot.Logic
{
   
    public class Inventory
    {
        private readonly Client _client;
        private readonly ILogicSettings _logicSettings;

        private readonly List<ItemId> _pokeballs = new List<ItemId>
        {
            ItemId.ItemPokeBall,
            ItemId.ItemGreatBall,
            ItemId.ItemUltraBall,
            ItemId.ItemMasterBall
        };

        private readonly List<ItemId> _potions = new List<ItemId>
        {
            ItemId.ItemPotion,
            ItemId.ItemSuperPotion,
            ItemId.ItemHyperPotion,
            ItemId.ItemMaxPotion
        };

        private readonly List<ItemId> _revives = new List<ItemId> {ItemId.ItemRevive, ItemId.ItemMaxRevive};
        private GetInventoryResponse _cachedInventory;
        private DateTime _lastRefresh;

        public Inventory(Client client, ILogicSettings logicSettings)
        {
            _client = client;
            _logicSettings = logicSettings;
        }

        public async Task DeletePokemonFromInvById(ulong id)
        {
            var inventory = await GetCachedInventory();
            var pokemon =
                inventory.InventoryDelta.InventoryItems.FirstOrDefault(
                    i => i.InventoryItemData.PokemonData != null && i.InventoryItemData.PokemonData.Id == id);
            if (pokemon != null)
                inventory.InventoryDelta.InventoryItems.Remove(pokemon);
        }

        public async Task<LevelUpRewardsResponse> GetLevelUpRewards(Inventory inv )
        {
            var GetData = await _client.Player.GetPlayer(); 

            var ClientLevel = await _client.Player.GetPlayerProfile( GetData.PlayerData.Username );
            var Rewards = await _client.Player.GetLevelUpRewards( inv.GetPlayerStats().Result.FirstOrDefault().Level );

            return Rewards;

        }
        private async Task<GetInventoryResponse> GetCachedInventory()
        {
            var now = DateTime.UtcNow;

            if (_lastRefresh.AddSeconds(30).Ticks > now.Ticks)
            {
                return _cachedInventory;
            }
            return await RefreshCachedInventory();
        }





        public async Task<IEnumerable<PokemonData>> GetDuplicatePokemonToTransfer( 
                IEnumerable<PokemonId> pokemonsNotToTransfer, IEnumerable<PokemonId> pokemonsToEvolve,
                bool keepPokemonsThatCanEvolve = false, bool prioritizeIVoverCp = false
             )
        {
            var myPokemon = await GetPokemons();

            var myPokemonList = myPokemon.ToList();

            var pokemonToTransfer = myPokemonList.Where(p => !pokemonsNotToTransfer.Contains(p.PokemonId) && p.DeployedFortId == string.Empty && p.Favorite == 0).ToList();

            pokemonToTransfer = 
                pokemonToTransfer.Where(
                    p =>
                    {
                        var pokemonTransferFilter = GetPokemonTransferFilter(p.PokemonId);

                        return
                            !pokemonTransferFilter.MovesOperator.BoolFunc(
                                pokemonTransferFilter.Moves.Intersect(new[] { p.Move1, p.Move2 }).Any(),
                                pokemonTransferFilter.KeepMinOperator.BoolFunc(
                                    p.Cp >= pokemonTransferFilter.KeepMinCp,
                                    PokemonInfo.CalculatePokemonPerfection(p) >= pokemonTransferFilter.KeepMinIvPercentage,
                                    pokemonTransferFilter.KeepMinOperator.ReverseBoolFunc(
                                        pokemonTransferFilter.KeepMinOperator.InverseBool(pokemonTransferFilter.UseKeepMinLvl),
                                        PokemonInfo.GetLevel(p) >= pokemonTransferFilter.KeepMinLvl)));
                    }).ToList();


            var myPokemonSettings = await GetPokemonSettings();
            var pokemonSettings = myPokemonSettings.ToList();

            var myPokemonFamilies = await GetPokemonFamilies();
            var pokemonFamilies = myPokemonFamilies.ToArray();


            var results = new List<PokemonData>();

            foreach (var pokemonGroupToTransfer in pokemonToTransfer.GroupBy(p => p.PokemonId).ToList())
            {
                var amountToKeepInStorage = Math.Max(GetPokemonTransferFilter(pokemonGroupToTransfer.Key).KeepMinDuplicatePokemon, 0);

                var settings = pokemonSettings.Single(x => x.PokemonId == pokemonGroupToTransfer.Key);
                var familyCandy = pokemonFamilies.Single(x => settings.FamilyId == x.FamilyId);

                int? modFromEvolve = null;

                if (keepPokemonsThatCanEvolve &&
                    pokemonsToEvolve.Contains(pokemonGroupToTransfer.Key) &&
                    settings.CandyToEvolve > 0 &&
                    settings.EvolutionIds.Count != 0)
                {
                    var possibleCountToEvolve = familyCandy.Candy_ / settings.CandyToEvolve;
                    amountToKeepInStorage = Math.Max(amountToKeepInStorage, possibleCountToEvolve);

                    //remain candy
                    modFromEvolve = familyCandy.Candy_%settings.CandyToEvolve;
                }

                var inStorage = myPokemonList.Count(data => data.PokemonId == pokemonGroupToTransfer.Key);

                var weakPokemonCount = pokemonGroupToTransfer.Count();

                var needToRemove = inStorage - amountToKeepInStorage;
                if (needToRemove <= 0)
                    continue;

                var canBeRemoved = Math.Min(needToRemove, weakPokemonCount);

                if (canBeRemoved <= 0)
                    continue;

                //Lets calc new canBeRemoved pokemons according to transferring some of them and use it as future candy to keepPokemonsThatCanEvolve
                if (modFromEvolve.HasValue)
                {
                    // its an solution in fixed numbers of equations with two variables 
                    // (N = X + Y, X + C >= Y * E) -> (X >= (N * E - C / 1 + E)
                    // where N - current canBeRemoved,  X - new canBeRemoved, Y - possible to keep more, E - CandyToEvolve, C - modFromEvolve(remain candy) ) )
                    canBeRemoved = (settings.CandyToEvolve * canBeRemoved - modFromEvolve.Value) / (1 + settings.CandyToEvolve) + 
                                    Math.Sign((settings.CandyToEvolve * canBeRemoved - modFromEvolve.Value) % (1 + settings.CandyToEvolve));
                }

                var skipCount = weakPokemonCount - canBeRemoved;
                
                if (prioritizeIVoverCp)
                {
                    results.AddRange(pokemonGroupToTransfer
                        .OrderByDescending(PokemonInfo.CalculatePokemonPerfection)
                        .ThenByDescending(n => n.Cp)
                        .Skip(skipCount));
                }
                else
                {
                    results.AddRange(pokemonGroupToTransfer
                        .OrderByDescending(x => x.Cp)
                        .ThenByDescending(PokemonInfo.CalculatePokemonPerfection)
                        .Skip(skipCount));
                }
            }

            #region For testing
            /*
            results.ForEach(data =>
            {
                var bestPokemonOfType = (_logicSettings.PrioritizeIvOverCp
                 ? myPokemonList.Where(x => x.PokemonId == data.PokemonId)
                .OrderByDescending(x => x.Cp)
                .FirstOrDefault()
                 : myPokemonList.Where(x => x.PokemonId == data.PokemonId)
                .OrderByDescending(PokemonInfo.CalculatePokemonPerfection)
                .FirstOrDefault()) ?? data;

                var perfection = PokemonInfo.CalculatePokemonPerfection(data);
                var cp = data.Cp;

                var bestPerfection = PokemonInfo.CalculatePokemonPerfection(bestPokemonOfType);
                var bestCp = bestPokemonOfType.Cp;
            });
            */
            #endregion

            return results;
        }

        public async Task<IEnumerable<EggIncubator>> GetEggIncubators()
        {
            var inventory = await GetCachedInventory();
            return
                inventory.InventoryDelta.InventoryItems
                    .Where(x => x.InventoryItemData.EggIncubators != null)
                    .SelectMany(i => i.InventoryItemData.EggIncubators.EggIncubator)
                    .Where(i => i != null);
        }

        public async Task<IEnumerable<PokemonData>> GetEggs()
        {
            var inventory = await GetCachedInventory();
            return
                inventory.InventoryDelta.InventoryItems.Select(i => i.InventoryItemData?.PokemonData)
                    .Where(p => p != null && p.IsEgg);
        }

        public async Task<PokemonData> GetHighestPokemonOfTypeByCp(PokemonData pokemon)
        {
            var myPokemon = await GetPokemons();
            var pokemons = myPokemon.ToList();
            return pokemons.Where(x => x.PokemonId == pokemon.PokemonId)
                .OrderByDescending(x => x.Cp)
                .FirstOrDefault();
        }
        public async Task<int> GetStarDust()
        {
            var StarDust =await  _client.Player.GetPlayer();
            var gdrfds = StarDust.PlayerData.Currencies;
            var SplitStar = gdrfds[1].Amount;
            return SplitStar;

        }

        public async Task<PokemonData> GetHighestPokemonOfTypeByIv(PokemonData pokemon)
        {
            var myPokemon = await GetPokemons();
            var pokemons = myPokemon.ToList();
            return pokemons.Where(x => x.PokemonId == pokemon.PokemonId)
                .OrderByDescending(PokemonInfo.CalculatePokemonPerfection)
                .FirstOrDefault();
        }

        public async Task<IEnumerable<PokemonData>> GetHighestsCp(int limit)
        {
            var myPokemon = await GetPokemons();
            var pokemons = myPokemon.ToList();
            return pokemons.OrderByDescending(x => x.Cp).ThenBy(n => n.StaminaMax).Take(limit);
        }
     
        public async Task<IEnumerable<PokemonData>> GetHighestsPerfect(int limit)
        {
            var myPokemon = await GetPokemons();
            var pokemons = myPokemon.ToList();
            return pokemons.OrderByDescending(PokemonInfo.CalculatePokemonPerfection).Take(limit);
        }


        public async Task<int> GetItemAmountByType(ItemId type)
        {
            var pokeballs = await GetItems();
            return pokeballs.FirstOrDefault(i => i.ItemId == type)?.Count ?? 0;
        }

        public async Task<IEnumerable<ItemData>> GetItems()
        {
            var inventory = await GetCachedInventory();
            return inventory.InventoryDelta.InventoryItems
                .Select(i => i.InventoryItemData?.Item)
                .Where(p => p != null);
        }

        public async Task<int> GetTotalItemCount()
        {
            var myItems = (await GetItems()).ToList();
            int myItemCount = 0;
            foreach (var myItem in myItems) myItemCount += myItem.Count;
            return myItemCount;
        }

        public async Task<IEnumerable<ItemData>> GetItemsToRecycle(ISession session)
        {
            var itemsToRecycle = new List<ItemData>();
            var myItems = (await GetItems()).ToList();

            var otherItemsToRecycle = myItems
                .Where(x => _logicSettings.ItemRecycleFilter.Any(f => f.Key == x.ItemId && x.Count > f.Value))
                .Select(
                    x =>
                        new ItemData
                        {
                            ItemId = x.ItemId,
                            Count = x.Count - _logicSettings.ItemRecycleFilter.Single(f => f.Key == x.ItemId).Value,
                            Unseen = x.Unseen
                        });

            itemsToRecycle.AddRange(otherItemsToRecycle);

            return itemsToRecycle;
        }

        public double GetPerfect(PokemonData poke)
        {
            var result = PokemonInfo.CalculatePokemonPerfection(poke);
            return result;
        }

        public async Task<IEnumerable<PlayerStats>> GetPlayerStats()
        {
            var inventory = await GetCachedInventory();
            return inventory.InventoryDelta.InventoryItems
                .Select(i => i.InventoryItemData?.PlayerStats)
                .Where(p => p != null);
        }

        public async Task<UseItemXpBoostResponse> UseLuckyEggConstantly()
        {
            var UseLuckyEgg = await _client.Inventory.UseItemXpBoost();
            return UseLuckyEgg;
        }
        public async Task<UseIncenseResponse> UseIncenseConstantly()
        {
            var UseIncense = await _client.Inventory.UseIncense(ItemId.ItemIncenseOrdinary);
            return UseIncense;
        }

        public async Task<List<InventoryItem>> GetPokeDexItems()
        {
            List<InventoryItem> PokeDex = new List<InventoryItem>();
            var inventory = await _client.Inventory.GetInventory();

            return (from items in inventory.InventoryDelta.InventoryItems
                   where items.InventoryItemData?.PokedexEntry != null
                   select items).ToList();
        }

        public async Task<List<Candy>> GetPokemonFamilies()
        {
            var inventory = await GetCachedInventory();

            var families = from item in inventory.InventoryDelta.InventoryItems
                            where item.InventoryItemData?.Candy != null
                            where item.InventoryItemData?.Candy.FamilyId != PokemonFamilyId.FamilyUnset
                            group item by item.InventoryItemData?.Candy.FamilyId into family
                            select new Candy
                            {
                                FamilyId = family.First().InventoryItemData.Candy.FamilyId,
                                Candy_ = family.First().InventoryItemData.Candy.Candy_
                            };


            return families.ToList();
        }

        public async Task<IEnumerable<PokemonData>> GetPokemons()
        {
            var inventory = await GetCachedInventory();
            return
                inventory.InventoryDelta.InventoryItems.Select(i => i.InventoryItemData?.PokemonData)
                    .Where(p => p != null && p.PokemonId > 0);
        }

        public async Task<IEnumerable<PokemonSettings>> GetPokemonSettings()
        {
            var templates = await _client.Download.GetItemTemplates();
            return
                templates.ItemTemplates.Select(i => i.PokemonSettings)
                    .Where(p => p != null && p.FamilyId != PokemonFamilyId.FamilyUnset);
        }

        public async Task<IEnumerable<PokemonData>> GetPokemonToEvolve(IEnumerable<PokemonId> filter = null)
        {
            var myPokemon = await GetPokemons();
            myPokemon = myPokemon.Where(p => p.DeployedFortId == string.Empty).OrderByDescending(p => p.Cp);
            //Don't evolve pokemon in gyms
            IEnumerable<PokemonId> pokemonIds = filter as PokemonId[] ?? filter.ToArray();
            if (pokemonIds.Any())
            {
                myPokemon =
                    myPokemon.Where(
                        p => (_logicSettings.EvolveAllPokemonWithEnoughCandy && pokemonIds.Contains(p.PokemonId)) ||
                             (_logicSettings.EvolveAllPokemonAboveIv &&
                              (PokemonInfo.CalculatePokemonPerfection(p) >= _logicSettings.EvolveAboveIvValue)));
            }
            else if (_logicSettings.EvolveAllPokemonAboveIv)
            {
                myPokemon =
                    myPokemon.Where(
                        p => PokemonInfo.CalculatePokemonPerfection(p) >= _logicSettings.EvolveAboveIvValue);
            }
            var pokemons = myPokemon.ToList();

            var myPokemonSettings = await GetPokemonSettings();
            var pokemonSettings = myPokemonSettings.ToList();

            var myPokemonFamilies = await GetPokemonFamilies();
            var pokemonFamilies = myPokemonFamilies.ToArray();

            var pokemonToEvolve = new List<PokemonData>();
            foreach (var pokemon in pokemons)
            {
                var settings = pokemonSettings.SingleOrDefault(x => x.PokemonId == pokemon.PokemonId);
                var familyCandy = pokemonFamilies.SingleOrDefault(x => settings.FamilyId == x.FamilyId);

                //Don't evolve if we can't evolve it
                if (settings.EvolutionIds.Count == 0)
                    continue;

                var pokemonCandyNeededAlready =
                    pokemonToEvolve.Count(
                        p => pokemonSettings.Single(x => x.PokemonId == p.PokemonId).FamilyId == settings.FamilyId)*
                    settings.CandyToEvolve;

                if (familyCandy.Candy_ - pokemonCandyNeededAlready > settings.CandyToEvolve)
                {
                    pokemonToEvolve.Add(pokemon);
                }
            }

            return pokemonToEvolve;
        }

        public async Task<List<PokemonData>> GetPokemonToUpgrade()
        {
            var upgradePokemon = new List<PokemonData>();

            if (!_logicSettings.AutomaticallyLevelUpPokemon)
                return upgradePokemon;

            var myPokemon = await GetPokemons();
            myPokemon = myPokemon.Where(p => p.DeployedFortId == string.Empty);
            
            IEnumerable<PokemonData> highestPokemonForUpgrade = (_logicSettings.UpgradePokemonMinimumStatsOperator.ToLower().Equals("and")) ?
                myPokemon.Where(
                        p => (p.Cp >= _logicSettings.UpgradePokemonCpMinimum &&
                            PokemonInfo.CalculatePokemonPerfection(p) >= _logicSettings.UpgradePokemonIvMinimum)).OrderByDescending(p => p.Cp).ToList() :
                myPokemon.Where(
                    p => (p.Cp >= _logicSettings.UpgradePokemonCpMinimum ||
                        PokemonInfo.CalculatePokemonPerfection(p) >= _logicSettings.UpgradePokemonIvMinimum)).OrderByDescending(p => p.Cp).ToList();

            return upgradePokemon = (_logicSettings.LevelUpByCPorIv.ToLower().Equals("iv")) ?
                    highestPokemonForUpgrade.OrderByDescending(PokemonInfo.CalculatePokemonPerfection).ToList() :
                    highestPokemonForUpgrade.OrderByDescending(p => p.Cp).ToList();
        }

        public TransferFilter GetPokemonTransferFilter(PokemonId pokemon)
        {
            if (_logicSettings.PokemonsTransferFilter != null &&
                _logicSettings.PokemonsTransferFilter.ContainsKey(pokemon))
            {
                return _logicSettings.PokemonsTransferFilter[pokemon];
            }
            return new TransferFilter(_logicSettings.KeepMinCp, _logicSettings.KeepMinLvl, _logicSettings.UseKeepMinLvl, _logicSettings.KeepMinIvPercentage,
                _logicSettings.KeepMinOperator, _logicSettings.KeepMinDuplicatePokemon);
        }

        public async Task<GetInventoryResponse> RefreshCachedInventory()
        {
            var now = DateTime.UtcNow;
            var ss = new SemaphoreSlim(10);

            await ss.WaitAsync();
            try
            {
                _lastRefresh = now;
                _cachedInventory = await _client.Inventory.GetInventory();
                return _cachedInventory;
            }
            finally
            {
                ss.Release();
            }
        }

        public async Task<UpgradePokemonResponse> UpgradePokemon(ulong pokemonid)
        {
            var upgradeResult = await _client.Inventory.UpgradePokemon(pokemonid);
            return upgradeResult;
        }
    }
}
