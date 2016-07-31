using PoGo.NecroBot.Logic.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PoGo.NecroBot.Logic.Tasks
{
    class UseIncenseConstantlyTask
    {
        public static async Task Execute(ISession session, CancellationToken cancellationToken)
        {
            var UseEgg = await session.Inventory.UseIncenseConstantly();

            if (UseEgg.Result.ToString().ToLower().Contains("noneininventory"))
            {
                Logging.Logger.Write("No Incense Available");

            }
            else if (UseEgg.Result.ToString().Contains("IncenseAlreadyActive"))
            {
                Logging.Logger.Write("Incense Already Active");
            }
            else
            {
                Logging.Logger.Write("Used an Incense");
            }
        }
    }
}
