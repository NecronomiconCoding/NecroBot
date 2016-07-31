using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.State;
using POGOProtos.Inventory.Item;

namespace PoGo.NecroBot.Logic.Tasks
{
    class UseLuckyEggConstantlyTask
    {
        public static async Task Execute(ISession session, CancellationToken cancellationToken)
        {
            if (!session.LogicSettings.UseLuckyEggConstantly)
                return;

            var currentAmountOfLuckyEggs = await session.Inventory.GetItemAmountByType(ItemId.ItemLuckyEgg);
            if (currentAmountOfLuckyEggs == 0)
            {
                Logging.Logger.Write("No Eggs Available");
                return;
            }

            var UseEgg = await session.Inventory.UseLuckyEggConstantly();

            if (UseEgg.Result.ToString().Contains("Success"))
            {
                Logging.Logger.Write("Used a Lucky Egg");
            }
            else if (UseEgg.Result.ToString().ToLower().Contains("errornoitemsremaining"))
            {
                Logging.Logger.Write("No Eggs Available");
            }
            else if (UseEgg.Result.ToString().Contains("AlreadyActive") || (UseEgg.AppliedItems == null))
            {
                Logging.Logger.Write("Lucky Egg Already Active");
            }
        }
       
    }
}
