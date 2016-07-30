using PoGo.NecroBot.Logic.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using POGOProtos.Enums;

namespace PoGo.NecroBot.Logic.Tasks
{
    class LevelUpPokemon
    {
        public static async Task Execute(ISession session, CancellationToken cancellationToken)
        {
            if (session.LogicSettings.LevelUpByCPorIV.ToLower().Contains(("iv")))
            {
                Random rand = new Random();
                int RandomNumber = rand.Next(0, DisplayPokemonStatsTask.PokemonID.Count);
                
                var UpgradeResult = await session.Inventory.UpgradePokemon(DisplayPokemonStatsTask.PokemonID[RandomNumber]);
                if (UpgradeResult.Result.ToString().ToLower().Contains("success"))
                {
                    Logging.Logger.Write("Pokemon Upgraded:" + UpgradeResult.UpgradedPokemon.PokemonId + ":" + UpgradeResult.UpgradedPokemon.Cp);
                }
                else if (UpgradeResult.Result.ToString().ToLower().Contains("insufficient"))
                {
                    Logging.Logger.Write("Pokemon Upgrade Failed Not Enough Resources");

                }
                else
                {
                    Logging.Logger.Write("Pokemon Upgrade Failed Unknown Error, Pokemon Could Be Max Level For Your Level The Pokemon That Caused Issue Was:"+UpgradeResult.UpgradedPokemon.PokemonId);
                }
            }
            else if (session.LogicSettings.LevelUpByCPorIV.ToLower().Contains(("cp")))
            {

                Random rand = new Random();
                int RandomNumber = rand.Next(0, DisplayPokemonStatsTask.PokemonIDCP.Count);
                var UpgradeResult = await session.Inventory.UpgradePokemon(DisplayPokemonStatsTask.PokemonIDCP[RandomNumber]);
                if (UpgradeResult.Result.ToString().ToLower().Contains("success"))
                {
                    Logging.Logger.Write("Pokemon Upgraded:" + UpgradeResult.UpgradedPokemon.PokemonId + ":" + UpgradeResult.UpgradedPokemon.Cp);
                }
                else if (UpgradeResult.Result.ToString().ToLower().Contains("insufficient"))
                {
                    Logging.Logger.Write("Pokemon Upgrade Failed Not Enough Resources");

                }
                else
                {
                    Logging.Logger.Write("Pokemon Upgrade Failed Unknown Error, Pokemon Could Be Max Level For Your Level The Pokemon That Caused Issue Was:" + UpgradeResult.UpgradedPokemon.PokemonId);
                }
            }


        }
    }
}