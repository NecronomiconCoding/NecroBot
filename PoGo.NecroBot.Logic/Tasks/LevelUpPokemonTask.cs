#region using directives

using System;
using System.Threading;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.State;
using System.Collections.Generic;
using POGOProtos.Enums;
using PoGo.NecroBot.Logic.PoGoUtils;

#endregion

namespace PoGo.NecroBot.Logic.Tasks
{
    internal class LevelUpPokemonTask
    {
        public static async Task Execute(ISession session, CancellationToken cancellationToken)
        {
            if (await session.Inventory.GetStarDust() <= session.LogicSettings.GetMinStarDustForLevelUp)
                return;

            var upgradablePokemon = await session.Inventory.GetPokemonToUpgrade();
            if (upgradablePokemon.Count == 0)
                return;

            var myPokemonSettings = await session.Inventory.GetPokemonSettings();
            var pokemonSettings = myPokemonSettings.ToList();

            var myPokemonFamilies = await session.Inventory.GetPokemonFamilies();
            var pokemonFamilies = myPokemonFamilies.ToArray();

            foreach (var pokemon in upgradablePokemon)
            {
                if (PokemonInfo.CalculateMaxCp(pokemon) == pokemon.Cp) continue;

                var settings = pokemonSettings.Single(x => x.PokemonId == pokemon.Key);
                var familyCandy = pokemonFamilies.Single(x => settings.FamilyId == x.FamilyId);

                var upgradeResult = await session.Inventory.UpgradePokemon(pokemon.Id);
                if (upgradeResult.Result.ToString().ToLower().Contains("success"))
                {
                    Logger.Write("Pokemon Upgraded:" + session.Translation.GetPokemonTranslation(upgradeResult.UpgradedPokemon.PokemonId) + ":" +
                                    upgradeResult.UpgradedPokemon.Cp);
                }
                else if (upgradeResult.Result.ToString().ToLower().Contains("insufficient"))
                {
                    Logger.Write("Pokemon Upgrade Failed Not Enough Resources");
                    break;
                }
                else if (upgradeResult.Result.ToString().Contains("ErrorUpgradeNotAvailable"))
                {
                    Logger.Write("Pokemon Is At Max Level For Your Level");
                    break;
                }
                else
                {
                    Logger.Write(
                        "Pokemon Upgrade Failed Unknown Error, Pokemon Could Be Max Level For Your Level The Pokemon That Caused Issue Was:" +
                        session.Translation.GetPokemonTranslation(upgradeResult.UpgradedPokemon.PokemonId));
                    break;
                }
            }
        }
    }
}