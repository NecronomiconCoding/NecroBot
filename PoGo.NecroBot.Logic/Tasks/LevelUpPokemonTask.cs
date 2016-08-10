#region using directives

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.PoGoUtils;
using System.Linq;
using POGOProtos.Data;

#endregion

namespace PoGo.NecroBot.Logic.Tasks
{
    internal class LevelUpPokemonTask
    {
        public static List<PokemonData> Upgrade = new List<PokemonData>();
        private static IEnumerable<PokemonData> upgradablePokemon;

        public static async Task Execute(ISession session, CancellationToken cancellationToken)
        {
           
           
            if (session.Inventory.GetStarDust() <= session.LogicSettings.GetMinStarDustForLevelUp)
                return;
            upgradablePokemon = await session.Inventory.GetPokemonToUpgrade();
            if (session.LogicSettings.OnlyUpgradeFavorites)
            {
                var fave = upgradablePokemon.Where(i => i.Favorite == 1);
                upgradablePokemon = fave;
            }
           
           
            if (upgradablePokemon.Count() == 0)
                return;

            var myPokemonSettings = await session.Inventory.GetPokemonSettings();
            var pokemonSettings = myPokemonSettings.ToList();

            var myPokemonFamilies = await session.Inventory.GetPokemonFamilies();
            var pokemonFamilies = myPokemonFamilies.ToArray();

            var upgradedNumber = 0;
            var PokemonToLevel = session.LogicSettings.PokemonsToLevelUp;

            foreach (var pokemon in upgradablePokemon)
            {
                if (session.LogicSettings.UseLevelUpList && PokemonToLevel!=null)
                {
                    for (int i = 0; i < PokemonToLevel.Count; i++)
                    {
                        if (PokemonToLevel.Contains(pokemon.PokemonId))
                        {
                            if (PokemonInfo.GetLevel(pokemon) >=
                                session.Inventory.GetPlayerStats().Result.FirstOrDefault().Level + 1) break;

                            var settings = pokemonSettings.Single(x => x.PokemonId == pokemon.PokemonId);
                            var familyCandy = pokemonFamilies.Single(x => settings.FamilyId == x.FamilyId);

                            if (familyCandy.Candy_ <= 0) continue;

                            var upgradeResult = await session.Inventory.UpgradePokemon(pokemon.Id);
                            if (upgradeResult.Result.ToString().ToLower().Contains("success"))
                            {
                                Logger.Write("Pokemon Upgraded:" +
                                             session.Translation.GetPokemonTranslation(
                                                 upgradeResult.UpgradedPokemon.PokemonId) + ":" +
                                             upgradeResult.UpgradedPokemon.Cp,LogLevel.LevelUp);
                                upgradedNumber++;
                            }

                            if (upgradedNumber >= session.LogicSettings.AmountOfTimesToUpgradeLoop)
                                break;
                        }
                        else
                        {
                            break;
                        }
                    }

                }
                else
                {
                    if (PokemonInfo.GetLevel(pokemon) >= session.Inventory.GetPlayerStats().Result.FirstOrDefault().Level + 1) break;

                    var settings = pokemonSettings.Single(x => x.PokemonId == pokemon.PokemonId);
                    var familyCandy = pokemonFamilies.Single(x => settings.FamilyId == x.FamilyId);

                    if (familyCandy.Candy_ <= 0) continue;

                    var upgradeResult = await session.Inventory.UpgradePokemon(pokemon.Id);
                    if (upgradeResult.Result.ToString().ToLower().Contains("success"))
                    {
                        Logger.Write("Pokemon Upgraded:" + session.Translation.GetPokemonTranslation(upgradeResult.UpgradedPokemon.PokemonId) + ":" +
                                        upgradeResult.UpgradedPokemon.Cp, LogLevel.LevelUp);
                        upgradedNumber++;
                    }

                    if (upgradedNumber >= session.LogicSettings.AmountOfTimesToUpgradeLoop)
                        break;
                }
               
                
            }
        }
    }
}
