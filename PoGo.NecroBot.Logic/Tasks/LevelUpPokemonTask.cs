#region using directives

using System;
using System.Threading;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.State;
using System.Collections.Generic;
using System.Linq;


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
            if (session.LogicSettings.LevelUpByCPorIv.ToLower().Contains("iv"))
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
                }
                else
                {
                    Logger.Write(
                        "Pokemon Upgrade Failed Unknown Error, Pokemon Could Be Max Level For Your Level The Pokemon That Caused Issue Was:" +
                        upgradeResult.UpgradedPokemon.PokemonId);
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
            else if (session.LogicSettings.LevelUpByCPorIv.ToLower().Contains("both"))
            {
                var PokemonIdcpiv = new List<ulong>();
                PokemonIdcpiv = DisplayPokemonStatsTask.PokemonId.Intersect(DisplayPokemonStatsTask.PokemonIdcp).ToList();
                if(PokemonIdcpiv.Count == 0)
                {
                    return;
                }

                var rand = new Random();
                var randomNumber = rand.Next(0, PokemonIdcpiv.Count - 1);
                var upgradeResult =
                    await session.Inventory.UpgradePokemon(PokemonIdcpiv[randomNumber]);
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