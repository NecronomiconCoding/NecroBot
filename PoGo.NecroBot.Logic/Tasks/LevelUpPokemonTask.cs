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
            if (DisplayPokemonStatsTask.PokemonId.Count == 0 || DisplayPokemonStatsTask.PokemonIdcp.Count == 0)
            {
                return;
            }
            if (await session.Inventory.GetStarDust() <= session.LogicSettings.GetMinStarDustForLevelUp)
            {
                return;
            }
            if (session.LogicSettings.LevelUpByCPorIv.ToLower().Contains("iv"))
            {
                for (int i = 0; i < session.LogicSettings.AmountOfTimesToUpgradeLoop; i++)
                {
                    var rand = new Random();
                    var randomNumber = rand.Next(0, DisplayPokemonStatsTask.PokemonId.Count - 1);

                    var upgradeResult =
                        await session.Inventory.UpgradePokemon(DisplayPokemonStatsTask.PokemonId[randomNumber]);
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
                    else if (upgradeResult.Result.ToString().Contains("ErrorUpgradeNotAvailable"))
                    {
                        Logger.Write("Pokemon Is At Max Level For Your Level");
                        break;
                    }
                    else
                    {
                        Logger.Write(
                            "Pokemon Upgrade Failed Unknown Error");
                        break;
                    }
                }
               
            }
            else if (session.LogicSettings.LevelUpByCPorIv.ToLower().Contains("cp"))
            {
               
                for (int i = 0; i < session.LogicSettings.AmountOfTimesToUpgradeLoop; i++)
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
                            upgradeResult.UpgradedPokemon.PokemonId);
                        break;
                    }
                }
                
            }
        }
    }
}