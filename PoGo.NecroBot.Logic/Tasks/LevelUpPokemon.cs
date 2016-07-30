using PoGo.NecroBot.Logic.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PoGo.NecroBot.Logic.Tasks
{
    class LevelUpPokemon
    {
        public static async Task Execute(ISession session, CancellationToken cancellationToken)
        {
            Random rand = new Random();
            int RandomNumber = rand.Next(1, 10);
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
                Logging.Logger.Write("Pokemon Upgrade Failed Unknown Error");
            }
        }
    }
}