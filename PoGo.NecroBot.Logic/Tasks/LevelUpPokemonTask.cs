#region using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.PoGoUtils;
using PoGo.NecroBot.Logic.State;
using POGOProtos.Data;
using POGOProtos.Networking.Responses;
using POGOProtos.Settings.Master;

#endregion

namespace PoGo.NecroBot.Logic.Tasks
{
    internal class LevelUpPokemonTask
    {
        public static async Task Execute(ISession session, CancellationToken cancellationToken)
        {

            // get the families and the pokemons settings to do some actual smart stuff like checking if you have enough candy in the first place

            var pokemonIdsToLevelUp = new List<PokemonData>();
            var levelUpByCpOrIv = session.LogicSettings.LevelUpByCPorIv?.ToLower();

            if (levelUpByCpOrIv == "iv" || levelUpByCpOrIv == "both")
            {
                pokemonIdsToLevelUp.AddRange(
                    session.Inventory.GetPokemons().Result
                        .Where(
                            pokemonData =>
                                PokemonInfo.CalculatePokemonPerfection(pokemonData) >=
                                session.LogicSettings.KeepMinIvPercentage &&
                                session.LogicSettings.PokemonsToLevelUp.Contains(pokemonData.PokemonId))
                        .OrderByDescending(PokemonInfo.CalculatePokemonPerfection)
                        .Select(p => p)
                        .ToList());
            }

            if (levelUpByCpOrIv == "cp" || levelUpByCpOrIv == "both")
            {
                pokemonIdsToLevelUp.AddRange(
                    session.Inventory.GetPokemons().Result
                        .Where(pokemonData => pokemonData.Cp > session.LogicSettings.KeepMinCp && session.LogicSettings.PokemonsToLevelUp.Contains(pokemonData.PokemonId))
                        .OrderByDescending(pokemonData => pokemonData.Cp)
                        .Select(p => p)
                        .ToList());
            }

            foreach (var pokemon in pokemonIdsToLevelUp)
            {
                var pokemonFamilies = await session.Inventory.GetPokemonFamilies();
                var pokemonSettings = await session.Inventory.GetPokemonSettings();
                var pokemonUpgradeSettings = await session.Inventory.GetPokemonUpgradeSettings();
                var playerLevel = await session.Inventory.GetPlayerStats();

                var pokeLevel = (int)PokemonInfo.GetLevel(pokemon);
                var currentPokemonSettings = pokemonSettings.FirstOrDefault(q => pokemon != null && q.PokemonId.Equals(pokemon.PokemonId));
                var family = pokemonFamilies.FirstOrDefault(q => currentPokemonSettings != null && q.FamilyId.Equals(currentPokemonSettings.FamilyId));
                var candyToEvolveTotal = GetCandyMinToKeep(pokemonSettings, currentPokemonSettings);

                var maxPokemonLevel = playerLevel?.FirstOrDefault().Level + pokemonUpgradeSettings.FirstOrDefault().AllowedLevelsAbovePlayer;
                var haveEnoughStarDust = session.Profile.PlayerData.Currencies.FirstOrDefault(c => c.Name.ToLower().Contains("stardust")).Amount >= pokemonUpgradeSettings.FirstOrDefault()?.StardustCost[pokeLevel];
                var haveEnoughCandy = family.Candy_ > pokemonUpgradeSettings.FirstOrDefault()?.CandyCost[pokeLevel] && family.Candy_ >= candyToEvolveTotal;
                if (pokeLevel < maxPokemonLevel && haveEnoughCandy && haveEnoughStarDust)
                {
                    await DoUpgrade(session, pokemon.Id);
                }
                    
            }
        }

        private static int GetCandyMinToKeep(IEnumerable<PokemonSettings> pokemonSettings, PokemonSettings currentPokemonSettings)
        {
            // total up required candy for evolution, for yourself and your ancestors to allow for others to be evolved before upgrading
            // always keeps a minimum amount in reserve, should never have 0 except for cases where a pokemon is in both first and final form (ie onix)
            var ancestor = pokemonSettings.FirstOrDefault(q => q.PokemonId == currentPokemonSettings.ParentPokemonId);
            var ancestor2 = pokemonSettings.FirstOrDefault(q => q.PokemonId == ancestor?.ParentPokemonId);

            int candyToEvolveTotal = currentPokemonSettings.CandyToEvolve;
            if (ancestor != null)
            {
                candyToEvolveTotal += ancestor.CandyToEvolve;
            }

            if (ancestor2 != null)
            {
                candyToEvolveTotal += ancestor2.CandyToEvolve;
            }

            return candyToEvolveTotal;
        }

        private static async Task DoUpgrade(ISession session, ulong pokemonId)
        {
            while (true)
            {
                var upgradeResult = await session.Inventory.UpgradePokemon(pokemonId);
                if (upgradeResult.Result == UpgradePokemonResponse.Types.Result.Success)
                {
                    Logger.Write("Pokemon Upgraded:" + upgradeResult.UpgradedPokemon.PokemonId + ":" +
                                 upgradeResult.UpgradedPokemon.Cp);
                }
                else
                {
                    if (upgradeResult.Result == UpgradePokemonResponse.Types.Result.ErrorInsufficientResources)
                        Logger.Write("Pokemon Upgrade Failed: Not enough candies");
                    if (upgradeResult.Result == UpgradePokemonResponse.Types.Result.ErrorUpgradeNotAvailable)
                        Logger.Write("Pokemon Upgrade Failed: Upgrade not available");
                    if (upgradeResult.Result == UpgradePokemonResponse.Types.Result.ErrorPokemonIsDeployed)
                        Logger.Write("Pokemon Upgrade Failed: Pokemon is defending gym");
                    if (upgradeResult.Result == UpgradePokemonResponse.Types.Result.ErrorPokemonNotFound)
                        Logger.Write("Pokemon Upgrade Failed: Something bad happened to your pokemon");
                    break;
                }
            }
        }
    }
}
