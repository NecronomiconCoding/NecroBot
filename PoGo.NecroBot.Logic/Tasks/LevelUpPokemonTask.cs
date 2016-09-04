#region using directives

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.PoGoUtils;
using PoGo.NecroBot.Logic.State;
using POGOProtos.Data;
using PoGo.NecroBot.Logic.Event;
using POGOProtos.Inventory;
using POGOProtos.Settings.Master;

#endregion

namespace PoGo.NecroBot.Logic.Tasks
{
    internal class LevelUpPokemonTask
    {
        public static List<PokemonData> Upgrade = new List<PokemonData>();
        private static IEnumerable<PokemonData> upgradablePokemon;

        private  static async Task<bool> UpgradeSinglePokemon(ISession session, PokemonData pokemon, List<Candy> pokemonFamilies, List<PokemonSettings> pokemonSettings) {
            if (PokemonInfo.GetLevel(pokemon) >=
                                 session.Inventory.GetPlayerStats().Result.FirstOrDefault().Level + 1) return false;

            var settings = pokemonSettings.Single(x => x.PokemonId == pokemon.PokemonId);
            var familyCandy = pokemonFamilies.Single(x => settings.FamilyId == x.FamilyId);

            if (familyCandy.Candy_ <= 10) return false;

            var upgradeResult = await session.Inventory.UpgradePokemon(pokemon.Id);

            var bestPokemonOfType = (session.LogicSettings.PrioritizeIvOverCp
    ? await session.Inventory.GetHighestPokemonOfTypeByIv(upgradeResult.UpgradedPokemon)
    : await session.Inventory.GetHighestPokemonOfTypeByCp(upgradeResult.UpgradedPokemon)) ?? upgradeResult.UpgradedPokemon;

            if (upgradeResult.Result.ToString().ToLower().Contains("success"))
            {
                session.EventDispatcher.Send(new UpgradePokemonEvent()
                {
                    Id = upgradeResult.UpgradedPokemon.PokemonId,
                    Cp = upgradeResult.UpgradedPokemon.Cp,
                    BestCp = bestPokemonOfType.Cp,
                    BestPerfection = PokemonInfo.CalculatePokemonPerfection(bestPokemonOfType),
                    Perfection = PokemonInfo.CalculatePokemonPerfection(upgradeResult.UpgradedPokemon)
                });
            }
            return true;

        }
        public static async Task Execute(ISession session, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await session.Inventory.RefreshCachedInventory();

            if (session.Inventory.GetStarDust() <= session.LogicSettings.GetMinStarDustForLevelUp)
                return;
            upgradablePokemon = await session.Inventory.GetPokemonToUpgrade();
           
                      
            if (upgradablePokemon.Count() == 0)
                return;

            var myPokemonSettings = await session.Inventory.GetPokemonSettings();
            var pokemonSettings = myPokemonSettings.ToList();

            var myPokemonFamilies = await session.Inventory.GetPokemonFamilies();
            var pokemonFamilies = myPokemonFamilies.ToList();

            var upgradedNumber = 0;
            var PokemonToLevel = session.LogicSettings.PokemonsToLevelUp.ToList();
            PokemonToLevel.AddRange(session.LogicSettings.PokemonUpgradeFilters.Select(p => p.Key));
            foreach (var pokemon in upgradablePokemon)
            {
                //code seem wrong. need need refactore to cleanup code here.
                if (session.LogicSettings.UseLevelUpList && PokemonToLevel!=null)
                {
                    for (int i = 0; i < PokemonToLevel.Count; i++)
                    {
                        //unnessecsarily check, shoudl remove
                        if (PokemonToLevel.Contains(pokemon.PokemonId))
                        {
                            await UpgradeSinglePokemon(session, pokemon, pokemonFamilies, pokemonSettings);
                            if (upgradedNumber >= session.LogicSettings.AmountOfTimesToUpgradeLoop)
                                break;
                            await Task.Delay(session.LogicSettings.DelayBetweenPlayerActions);
                            upgradedNumber++;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else
                {
                    await UpgradeSinglePokemon(session, pokemon, pokemonFamilies, pokemonSettings); ;
                    await Task.Delay(session.LogicSettings.DelayBetweenPlayerActions);
                }
            }
        }
    }
}
