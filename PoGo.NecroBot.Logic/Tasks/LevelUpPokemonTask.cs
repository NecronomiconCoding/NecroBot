#region using directives

using System;
using System.Threading;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.State;

#endregion

namespace PoGo.NecroBot.Logic.Tasks
{
    internal class LevelUpPokemonTask
    {
        public static async Task Execute(ISession session, CancellationToken cancellationToken)
        {
            if (session.LogicSettings.LevelUpByCPorIv.ToLower().Contains("iv"))
            {
                foreach (var pokeId in session.Inventory.GetPokemons().Result.Where(pokemonData => PokemonInfo.CalculatePokemonPerfection(pokemonData) >= session.LogicSettings.KeepMinIvPercentage).Select(p => p.Id))
                {
                    while (true)
                    {
                        var upgradeResult = await session.Inventory.UpgradePokemon(pokeId);
                        if (upgradeResult.Result.ToString().ToLower().Contains("success"))
                        {
                            Logger.Write("Pokemon Upgraded:" + upgradeResult.UpgradedPokemon.PokemonId + ":" +
                                         upgradeResult.UpgradedPokemon.Cp);
                        }
                        else if (upgradeResult.Result.ToString().ToLower().Contains("insufficient"))
                        {
                            Logger.Write("Pokemon Upgrade Failed Not Enough Resources");
                            break;
                        }
                        else
                        {
//                            Logger.Write("Pokemon Upgrade Failed Unknown Error, Pokemon Could Be Max Level For Your Level The Pokemon That Caused Issue Was:" + upgradeResult.UpgradedPokemon.PokemonId);
                            break;
                        }
                    }
                }
            }
            else if (session.LogicSettings.LevelUpByCPorIv.ToLower().Contains("cp"))
            {
                var rand = new Random();
                var randomNumber = rand.Next(0, DisplayPokemonStatsTask.PokemonIdcp.Count - 1);
                var upgradeResult =
                    await session.Inventory.UpgradePokemon(DisplayPokemonStatsTask.PokemonIdcp[randomNumber]);
                if (upgradeResult.Result.ToString().ToLower().Contains("success"))
                {
                    Logger.Write("Pokemon Upgraded:" + upgradeResult.UpgradedPokemon.PokemonId + ":" +
                                 upgradeResult.UpgradedPokemon.Cp);
                }
                else if (upgradeResult.Result.ToString().ToLower().Contains("insufficient"))
                {
                    Logger.Write("Pokemon Upgrade Failed Not Enough Resources");
                }
                else
                {
                    Logger.Write(
                        "Pokemon Upgrade Failed Unknown Error, Pokemon Could Be Max Level For Your Level The Pokemon That Caused Issue Was:" +
                        upgradeResult.UpgradedPokemon.PokemonId);
                }
            }
        }
    }
}
