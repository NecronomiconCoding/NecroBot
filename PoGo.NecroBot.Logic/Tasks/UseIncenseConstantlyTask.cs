using PoGo.NecroBot.Logic.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using POGOProtos.Inventory.Item;

namespace PoGo.NecroBot.Logic.Tasks
{
    class UseIncenseConstantlyTask
    {
        public static async Task Execute(ISession session, CancellationToken cancellationToken)
        {
            if (!session.LogicSettings.UseIncenseConstantly)
                return;

            var currentAmountOfIncense = await session.Inventory.GetItemAmountByType(ItemId.ItemIncenseOrdinary);
            if (currentAmountOfIncense == 0)
            {
                Logging.Logger.Write("No Incense Available");
                return;
            }

            var UseIncense = await session.Inventory.UseIncenseConstantly();

            if (UseIncense.Result.ToString().Contains("Success"))
            {
                Logging.Logger.Write("Used an Incense");
            }
            else if (UseIncense.Result.ToString().ToLower().Contains("errornoitemsremaining") ||
                UseIncense.Result.ToString().ToLower().Contains("noneininventory"))
            {
                Logging.Logger.Write("No Incense Available");
            }
            else if (UseIncense.Result.ToString().Contains("AlreadyActive") || (UseIncense.AppliedIncense == null))
            {
                Logging.Logger.Write("Incense Already Active");
            }
        }
    }
}
