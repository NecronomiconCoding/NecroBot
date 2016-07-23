#region

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PokemonGo.RocketAPI.Enums;
using PokemonGo.RocketAPI.GeneratedCode;
using System.Collections.Concurrent;
using System;
using System.Threading;

#endregion

namespace PokemonGo.RocketAPI.Logic
{
    public class Inventory
    {
        private readonly Client _client;
        private DateTime _lastRefresh;
        private GetInventoryResponse _cachedInventory;

        public Inventory(Client client)
        {
            _client = client;
        }

        public async Task<IEnumerable<PokemonData>> GetDuplicatePokemonToTransfer(
            bool keepPokemonsThatCanEvolve = false, bool prioritizeIVoverCP = false, IEnumerable<PokemonId> filter = null)
        {
            var myPokemon = await GetPokemons();

            var pokemonList = myPokemon.Where(p => p.DeployedFortId == 0 && p.Favorite == 0).ToList(); //Don't evolve pokemon in gyms
            if (filter != null)
            {
                pokemonList = pokemonList.Where(p => !filter.Contains(p.PokemonId)).ToList();
            }
            if (keepPokemonsThatCanEvolve)
            {
                var results = new List<PokemonData>();
                var pokemonsThatCanBeTransfered = pokemonList.GroupBy(p => p.PokemonId)
                    .Where(x => x.Count() > 2).ToList();

                var myPokemonSettings = await GetPokemonSettings();
                var pokemonSettings = myPokemonSettings.ToList();

                var myPokemonFamilies = await GetPokemonFamilies();
                var pokemonFamilies = myPokemonFamilies.ToArray();

                foreach (var pokemon in pokemonsThatCanBeTransfered)
                {
                    var settings = pokemonSettings.Single(x => x.PokemonId == pokemon.Key);
                    var familyCandy = pokemonFamilies.Single(x => settings.FamilyId == x.FamilyId);
                    if (settings.CandyToEvolve == 0)
                        continue;

                    var amountToSkip = familyCandy.Candy / settings.CandyToEvolve;
                    if (prioritizeIVoverCP)
                    {
                        results.AddRange(pokemonList.Where(x => x.PokemonId == pokemon.Key)
                            .OrderByDescending(x => PokemonInfo.CalculatePokemonPerfection(x))
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
            if (prioritizeIVoverCP)
            {
                return pokemonList
                .GroupBy(p => p.PokemonId)
                .Where(x => x.Count() > 1)
                .SelectMany(
                    p =>
                        p.OrderByDescending(x => PokemonInfo.CalculatePokemonPerfection(x))
                            .ThenBy(n => n.StaminaMax)
                            .Skip(_client.Settings.KeepMinDuplicatePokemon)
                            .ToList());
            }
            else
            {
                return pokemonList
                .GroupBy(p => p.PokemonId)
                .Where(x => x.Count() > 1)
                .SelectMany(
                    p =>
                        p.OrderByDescending(x => x.Cp)
                            .ThenBy(n => n.StaminaMax)
                            .Skip(_client.Settings.KeepMinDuplicatePokemon)
                            .ToList());
            }
        }

        public async Task<IEnumerable<PokemonData>> GetHighestsCP(int limit)
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

        public async Task<PokemonData> GetHighestPokemonOfTypeByCP(PokemonData pokemon)
        {
            var myPokemon = await GetPokemons();
            var pokemons = myPokemon.ToList();
            return pokemons.Where(x => x.PokemonId == pokemon.PokemonId)
                .OrderByDescending(x => x.Cp)
                .FirstOrDefault();
        }

        public async Task<int> GetItemAmountByType(MiscEnums.Item type)
        {
            var pokeballs = await GetItems();
            return pokeballs.FirstOrDefault(i => (MiscEnums.Item)i.Item_ == type)?.Count ?? 0;
        }

        public async Task<IEnumerable<Item>> GetItems()
        {
            var inventory = await getCachedInventory();
            return inventory.InventoryDelta.InventoryItems
                .Select(i => i.InventoryItemData?.Item)
                .Where(p => p != null);
        }

        public async Task<IEnumerable<Item>> GetItemsToRecycle(ISettings settings)
        {
            var myItems = await GetItems();

            return myItems
                .Where(x => settings.ItemRecycleFilter.Any(f => f.Key == (ItemId)x.Item_ && x.Count > f.Value))
                .Select(
                    x =>
                        new Item
                        {
                            Item_ = x.Item_,
                            Count = x.Count - settings.ItemRecycleFilter.Single(f => f.Key == (ItemId)x.Item_).Value,
                            Unseen = x.Unseen
                        });
        }

        public async Task<IEnumerable<PlayerStats>> GetPlayerStats()
        {
            var inventory = await getCachedInventory();
            return inventory.InventoryDelta.InventoryItems
                .Select(i => i.InventoryItemData?.PlayerStats)
                .Where(p => p != null);
        }

        public async Task<IEnumerable<PokemonFamily>> GetPokemonFamilies()
        {
            var inventory = await getCachedInventory();
            return
                inventory.InventoryDelta.InventoryItems.Select(i => i.InventoryItemData?.PokemonFamily)
                    .Where(p => p != null && p.FamilyId != PokemonFamilyId.FamilyUnset);
        }

        public async Task<IEnumerable<PokemonData>> GetPokemons()
        {
            var inventory = await getCachedInventory();
            return
                inventory.InventoryDelta.InventoryItems.Select(i => i.InventoryItemData?.Pokemon)
                    .Where(p => p != null && p.PokemonId > 0);
        }

        public async Task<IEnumerable<PokemonSettings>> GetPokemonSettings()
        {
            var templates = await _client.GetItemTemplates();
            return
                templates.ItemTemplates.Select(i => i.PokemonSettings)
                    .Where(p => p != null && p.FamilyId != PokemonFamilyId.FamilyUnset);
        }


        public async Task<IEnumerable<PokemonData>> GetPokemonToEvolve(IEnumerable<PokemonId> filter = null)
        {
            var myPokemons = await GetPokemons();
            myPokemons = myPokemons.Where(p => p.DeployedFortId == 0).OrderBy(p => p.Cp); //Don't evolve pokemon in gyms
            if (filter != null)
            {
                myPokemons = myPokemons.Where(p => filter.Contains(p.PokemonId));
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
                        p => pokemonSettings.Single(x => x.PokemonId == p.PokemonId).FamilyId == settings.FamilyId) *
                    settings.CandyToEvolve;
                if (familyCandy.Candy - pokemonCandyNeededAlready > settings.CandyToEvolve)
                    pokemonToEvolve.Add(pokemon);
            }

            return pokemonToEvolve;
        }


        private async Task<GetInventoryResponse> getCachedInventory()
        {
            var now = DateTime.UtcNow;
            SemaphoreSlim ss = new SemaphoreSlim(10);

            if (_lastRefresh != null && _lastRefresh.AddSeconds(30).Ticks > now.Ticks)
            {
                return _cachedInventory;
            }
            else
            {
                await ss.WaitAsync();
                try
                {
                    _lastRefresh = now;
                    _cachedInventory = await _client.GetInventory();
                    return _cachedInventory;
                }
                finally
                {
                    ss.Release();
                }
            }
           
        }
    }
}
