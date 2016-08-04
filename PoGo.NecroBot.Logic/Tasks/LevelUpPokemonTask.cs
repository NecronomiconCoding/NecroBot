#region using directives

using System.Threading;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.PoGoUtils;
using System.Linq;

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

            var upgradedNumber = 0;
            foreach (var pokemon in upgradablePokemon)
            {
                if (PokemonInfo.CalculateMaxCp(pokemon) == pokemon.Cp) continue;

                var settings = pokemonSettings.Single(x => x.PokemonId == pokemon.PokemonId);
                var familyCandy = pokemonFamilies.Single(x => settings.FamilyId == x.FamilyId);

                if (familyCandy.Candy_ <= 0) continue;
                
                var upgradeResult = await session.Inventory.UpgradePokemon(pokemon.Id);
                if (upgradeResult.Result.ToString().ToLower().Contains("success"))
                {
                    Logger.Write("Pokemon Upgraded:" + session.Translation.GetPokemonTranslation(upgradeResult.UpgradedPokemon.PokemonId) + ":" +
                                    upgradeResult.UpgradedPokemon.Cp);
                    upgradedNumber++;
                }

                if (upgradedNumber >= session.LogicSettings.AmountOfTimesToUpgradeLoop)
                        break;
            }
        }
    }
}