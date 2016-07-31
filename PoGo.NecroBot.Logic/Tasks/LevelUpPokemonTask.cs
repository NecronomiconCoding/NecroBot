#region using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using POGOProtos.Inventory;
using POGOProtos.Settings.Master;
using POGOProtos.Data;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.PoGoUtils;

#endregion

namespace PoGo.NecroBot.Logic.Tasks
{
    internal class LevelUpPokemonTask
    {
        public static async Task Execute(ISession session, CancellationToken cancellationToken)
        {
            // get the families and the pokemons settings to do some actual smart stuff like checking if you have enough candy in the first place
            var pokemonFamilies = await session.Inventory.GetPokemonFamilies();
            var pokemonSettings = await session.Inventory.GetPokemonSettings();
            var pokemonUpgradeSettings = await session.Inventory.GetPokemonUpgradeSettings();
            var playerLevel = await session.Inventory.GetPlayerStats();

            if (session.LogicSettings.LevelUpByCPorIv?.ToLower() == "iv")
            {
                var allPokemon = await session.Inventory.GetHighestsPerfect(session.Profile.PlayerData.MaxPokemonStorage);

                // get everything that is higher than the iv min
                foreach (var pokemon in allPokemon.Where(p => session.Inventory.GetPerfect(p) >= session.LogicSettings.UpgradePokemonIvMinimum))
                {
                    int pokeLevel = (int)PokemonInfo.GetLevel(pokemon);
                    var currentPokemonSettings = pokemonSettings.FirstOrDefault(q => pokemon != null && q.PokemonId.Equals(pokemon.PokemonId));
                    var family = pokemonFamilies.FirstOrDefault(q => currentPokemonSettings != null && q.FamilyId.Equals(currentPokemonSettings.FamilyId));
                    int candyToEvolveTotal = GetCandyMinToKeep(pokemonSettings, currentPokemonSettings);

                    // you can upgrade up to player level+2 right now
                    // may need translation for stardust???
                    if (pokeLevel < playerLevel?.FirstOrDefault().Level + pokemonUpgradeSettings.FirstOrDefault().AllowedLevelsAbovePlayer
                        && family.Candy_ > pokemonUpgradeSettings.FirstOrDefault()?.CandyCost[pokeLevel] 
                        && family.Candy_ >= candyToEvolveTotal 
                        && session.Profile.PlayerData.Currencies.FirstOrDefault(c => c.Name.ToLower().Contains("stardust")).Amount >= pokemonUpgradeSettings.FirstOrDefault()?.StardustCost[pokeLevel])
                    {
                        await DoUpgrade(session, pokemon);
                    }
                }
            }
            else if (session.LogicSettings.LevelUpByCPorIv?.ToLower() == "cp")
            {
                var allPokemon = await session.Inventory.GetHighestsPerfect(session.Profile.PlayerData.MaxPokemonStorage);

                // get everything that is higher than the cp min
                foreach (var pokemon in allPokemon.Where(p => session.Inventory.GetPerfect(p) >= session.LogicSettings.UpgradePokemonCpMinimum))
                {
                    int pokeLevel = (int)PokemonInfo.GetLevel(pokemon);
                    var currentPokemonSettings = pokemonSettings.FirstOrDefault(q => pokemon != null && q.PokemonId.Equals(pokemon.PokemonId));
                    var family = pokemonFamilies.FirstOrDefault(q => currentPokemonSettings != null && q.FamilyId.Equals(currentPokemonSettings.FamilyId));
                    int candyToEvolveTotal = GetCandyMinToKeep(pokemonSettings, currentPokemonSettings);

                    if (pokeLevel < playerLevel?.FirstOrDefault().Level + pokemonUpgradeSettings.FirstOrDefault().AllowedLevelsAbovePlayer
                        && family.Candy_ > pokemonUpgradeSettings.FirstOrDefault()?.CandyCost[pokeLevel]
                        && family.Candy_ >= candyToEvolveTotal
                        && session.Profile.PlayerData.Currencies.FirstOrDefault(c => c.Name.ToLower().Contains("stardust")).Amount >= pokemonUpgradeSettings.FirstOrDefault()?.StardustCost[pokeLevel])
                    {
                        await DoUpgrade(session, pokemon);
                    }
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

        private static async Task DoUpgrade(ISession session, PokemonData pokemon)
        {
            var upgradeResult = await session.Inventory.UpgradePokemon(pokemon.Id);

            if (upgradeResult.Result == POGOProtos.Networking.Responses.UpgradePokemonResponse.Types.Result.Success)
            {
                Logger.Write("Pokemon Upgraded:" + upgradeResult.UpgradedPokemon.PokemonId + ":" +
                                upgradeResult.UpgradedPokemon.Cp);
            }
            else if (upgradeResult.Result == POGOProtos.Networking.Responses.UpgradePokemonResponse.Types.Result.ErrorInsufficientResources)
            {
                Logger.Write("Pokemon Upgrade Failed Not Enough Resources");
            }
            // pokemon max level
            else if (upgradeResult.Result == POGOProtos.Networking.Responses.UpgradePokemonResponse.Types.Result.ErrorUpgradeNotAvailable)
            {
                Logger.Write("Pokemon upgrade unavailable for: " + pokemon.PokemonId + ":" + pokemon.Cp + "/" + PokemonInfo.CalculateMaxCp(pokemon));
            }
            else
            {
                Logger.Write(
                    "Pokemon Upgrade Failed Unknown Error, Pokemon Could Be Max Level For Your Level The Pokemon That Caused Issue Was:" +
                    pokemon.PokemonId);
            }
        }
    }
}