using PoGo.NecroBot.Logic.State;
using POGOProtos.Inventory.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PoGo.NecroBot.Logic.Tasks
{
    class UseLuckyEgg
    {
        private static ISession ctx2;

        public static async Task Execute(ISession ctx)
        {

            Timer t = new Timer(TimerCallback, null, 0, 300000);
            ctx2 = ctx;
        }
        private static void TimerCallback(Object o)
        {
            var UseEgg = ctx2.Inventory.UseLuckyEggConstantly();
            if (UseEgg.Result.Result.ToString().ToLower().Contains("errornoitemsremaining"))
            {
                Logging.Logger.Write("No Eggs Available");

            }
            else if (UseEgg.Result.Result.ToString().Contains("AlreadyActive"))
            {
                Logging.Logger.Write("Lucky Egg Already Active");
            }
            else
            {
                Logging.Logger.Write("Used a Lucky Egg");
            }

        }
    }
}
