#region using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Common;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.PoGoUtils;
using PoGo.NecroBot.Logic.State;
using PokemonGo.RocketAPI;
using POGOProtos.Data;
using POGOProtos.Data.Player;
using POGOProtos.Enums;
using POGOProtos.Inventory;
using POGOProtos.Inventory.Item;
using POGOProtos.Networking.Responses;
using POGOProtos.Settings.Master;

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
            bool keepPokemonsThatCanEvolve = false, bool prioritizeIVoverCp = false,
            IEnumerable<PokemonId> filter = null)
        {
            var myPokemon = await GetPokemons();

            var pokemonList =
                myPokemon.Where(
                    p => p.DeployedFortId == string.Empty &&
                         p.Favorite == 0 && (p.Cp < GetPokemonTransferFilter(p.PokemonId).KeepMinCp ||
                                             PokemonInfo.CalculatePokemonPerfection(p) <
                                             GetPokemonTransferFilter(p.PokemonId).KeepMinIvPercentage))
                    .ToList();
            if (filter != null)
            {
                pokemonList = pokemonList.Where(p => !filter.Contains(p.PokemonId)).ToList();
            }
            if (keepPokemonsThatCanEvolve)
            {
                var results = new List<PokemonData>();
                var pokemonsThatCanBeTransfered = pokemonList.GroupBy(p => p.PokemonId)
                    .Where(x => x.Count() > GetPokemonTransferFilter(x.Key).KeepMinDuplicatePokemon).ToList();

                var myPokemonSettings = await GetPokemonSettings();
                var pokemonSettings = myPokemonSettings.ToList();

                var myPokemonFamilies = await GetPokemonFamilies();
                var pokemonFamilies = myPokemonFamilies.ToArray();

                foreach (var pokemon in pokemonsThatCanBeTransfered)
                {
                    var settings = pokemonSettings.Single(x => x.PokemonId == pokemon.Key);
                    var familyCandy = pokemonFamilies.Single(x => settings.FamilyId == x.FamilyId);
                    var amountToSkip = GetPokemonTransferFilter(pokemon.Key).KeepMinDuplicatePokemon;

                    if (settings.CandyToEvolve > 0 && _logicSettings.PokemonsToEvolve.Contains(pokemon.Key))
                    {
                        var amountPossible = (familyCandy.Candy_ - 1) / (settings.CandyToEvolve - 1);
                        if (amountPossible > amountToSkip)
                            amountToSkip = amountPossible;
                    }

                    if (prioritizeIVoverCp)
                    {
                        results.AddRange(pokemonList.Where(x => x.PokemonId == pokemon.Key)
                            .OrderByDescending(PokemonInfo.CalculatePokemonPerfection)
                            .ThenBy(n => n.StaminaMax)
                            .Skip(amountToSkip)
                            .ToList());
                    }
                    else
                    {
                        results.AddRange(pokemonList.Where(x => x.PokemonId == pokemon.Key)
                            .OrderByDescending(x => x.Cp)
                            .ThenBy(n => n.StaminaMax)
                            .Skip(amountToSkip)
                            .ToList());
                    }
                }

                return results;
            }
            if (prioritizeIVoverCp)
            {
                return pokemonList
                    .GroupBy(p => p.PokemonId)
                    .Where(x => x.Any())
                    .SelectMany(
                        p =>
                            p.OrderByDescending(PokemonInfo.CalculatePokemonPerfection)
                                .ThenBy(n => n.StaminaMax)
                                .Skip(GetPokemonTransferFilter(p.Key).KeepMinDuplicatePokemon)
                                .ToList());
            }
            return pokemonList
                .GroupBy(p => p.PokemonId)
                .Where(x => x.Any())
                .SelectMany(
                    p =>
                        p.OrderByDescending(x => x.Cp)
                            .ThenBy(n => n.StaminaMax)
                            .Skip(GetPokemonTransferFilter(p.Key).KeepMinDuplicatePokemon)
                            .ToList());
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
            var itemsToRecylce = new List<ItemData>();
            var myItems = (await GetItems()).ToList();

            var otherItemsToRecylce = myItems
                .Where(x => _logicSettings.ItemRecycleFilter.Any(f => f.Key == x.ItemId && x.Count > f.Value))
                .Select(
                    x =>
                        new ItemData
                        {
                            ItemId = x.ItemId,
                            Count = x.Count - _logicSettings.ItemRecycleFilter.Single(f => f.Key == x.ItemId).Value,
                            Unseen = x.Unseen
                        });

            itemsToRecylce.AddRange(otherItemsToRecylce);

            return itemsToRecylce;
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

        private List<ItemData> GetPokeballsToRecycle(ISession session, IReadOnlyList<ItemData> myItems)
        {
            var amountOfPokeballsToKeep = _logicSettings.TotalAmountOfPokeballsToKeep;
            if (amountOfPokeballsToKeep < 1)
            {
                Logger.Write(session.Translation.GetTranslation(TranslationString.PokeballsToKeepIncorrect),
                    LogLevel.Error, ConsoleColor.Red);
                return new List<ItemData>();
            }

            var allPokeballs = myItems.Where(s => _pokeballs.Contains(s.ItemId)).ToList();
            allPokeballs.Sort((ball1, ball2) => ((int) ball1.ItemId).CompareTo((int) ball2.ItemId));

            return TakeAmountOfItems(allPokeballs, amountOfPokeballsToKeep).ToList();
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
                group item by item.InventoryItemData?.Candy.FamilyId
                into family
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
            var myPokemons = await GetPokemons();
            myPokemons = myPokemons.Where(p => p.DeployedFortId == string.Empty).OrderByDescending(p => p.Cp);
            //Don't evolve pokemon in gyms
            IEnumerable<PokemonId> pokemonIds = filter as PokemonId[] ?? filter.ToArray();
            if (pokemonIds.Any())
            {
                myPokemons =
                    myPokemons.Where(
                        p => (_logicSettings.EvolveAllPokemonWithEnoughCandy && pokemonIds.Contains(p.PokemonId)) ||
                             (_logicSettings.EvolveAllPokemonAboveIv &&
                              (PokemonInfo.CalculatePokemonPerfection(p) >= _logicSettings.EvolveAboveIvValue)));
            }
            else if (_logicSettings.EvolveAllPokemonAboveIv)
            {
                myPokemons =
                    myPokemons.Where(
                        p => PokemonInfo.CalculatePokemonPerfection(p) >= _logicSettings.EvolveAboveIvValue);
            }
            var pokemons = myPokemons.ToList();

            var myPokemonSettings = await GetPokemonSettings();
            var pokemonSettings = myPokemonSettings.ToList();

            var myPokemonFamilies = await GetPokemonFamilies();
            var pokemonFamilies = myPokemonFamilies.ToArray();

            var pokemonToEvolve = new List<PokemonData>();
            foreach (var pokemon in pokemons)
            {
                var settings = pokemonSettings.Single(x => x.PokemonId == pokemon.PokemonId);
                var familyCandy = pokemonFamilies.Single(x => settings.FamilyId == x.FamilyId);

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

        public TransferFilter GetPokemonTransferFilter(PokemonId pokemon)
        {
            if (_logicSettings.PokemonsTransferFilter != null &&
                _logicSettings.PokemonsTransferFilter.ContainsKey(pokemon))
            {
                return _logicSettings.PokemonsTransferFilter[pokemon];
            }
            return new TransferFilter(_logicSettings.KeepMinCp, _logicSettings.KeepMinIvPercentage,
                _logicSettings.KeepMinDuplicatePokemon);
        }

        private List<ItemData> GetPotionsToRecycle(ISession session, IReadOnlyList<ItemData> myItems)
        {
            var amountOfPotionsToKeep = _logicSettings.TotalAmountOfPotionsToKeep;
            if (amountOfPotionsToKeep < 1)
            {
                Logger.Write(session.Translation.GetTranslation(TranslationString.PotionsToKeepIncorrect),
                    LogLevel.Error, ConsoleColor.Red);
                return new List<ItemData>();
            }

            var allPotions = myItems.Where(s => _potions.Contains(s.ItemId)).ToList();
            allPotions.Sort((i1, i2) => ((int) i1.ItemId).CompareTo((int) i2.ItemId));

            return TakeAmountOfItems(allPotions, amountOfPotionsToKeep).ToList();
        }

        private List<ItemData> GetRevivesToRecycle(ISession session, IReadOnlyList<ItemData> myItems)
        {
            var amountOfRevivesToKeep = _logicSettings.TotalAmountOfRevivesToKeep;
            if (amountOfRevivesToKeep < 1)
            {
                Logger.Write(session.Translation.GetTranslation(TranslationString.RevivesToKeepIncorrect),
                    LogLevel.Error, ConsoleColor.Red);
                return new List<ItemData>();
            }

            var allRevives = myItems.Where(s => _revives.Contains(s.ItemId)).ToList();
            allRevives.Sort((i1, i2) => ((int) i1.ItemId).CompareTo((int) i2.ItemId));

            return TakeAmountOfItems(allRevives, amountOfRevivesToKeep).ToList();
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

        private IEnumerable<ItemData> TakeAmountOfItems(IReadOnlyList<ItemData> items, int ammountToLeave)
        {
            var itemsAvailable = 0;
            foreach (var item in items)
            {
                itemsAvailable += item.Count;
            }

            var itemsToRemove = itemsAvailable - ammountToLeave;

            foreach (var item in items)
            {
                if (itemsToRemove > 0 && item.Count > 0)
                {
                    if (item.Count < itemsToRemove)
                    {
                        // Recylce all of this type
                        itemsToRemove -= item.Count;
                        yield return item;
                    }
                    else
                    {
                        // Recycle remaining amount
                        var count = itemsToRemove;
                        itemsToRemove = 0;
                        yield return new ItemData {ItemId = item.ItemId, Count = count};
                    }
                }
            }
        }


        public async Task<UpgradePokemonResponse> UpgradePokemon(ulong pokemonid)
        {
            var upgradeResult = await _client.Inventory.UpgradePokemon(pokemonid);
            return upgradeResult;
        }
    }
}