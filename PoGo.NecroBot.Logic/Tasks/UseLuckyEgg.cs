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

        public static async Task Execute(ISession Session)
        {
            var UseEgg =await Session.Inventory.UseLuckyEggConstantly();
            if (UseEgg.Result.ToString().ToLower().Contains("errornoitemsremaining"))
            {
                Logging.Logger.Write("No Eggs Available");

            }
            else if (UseEgg.Result.ToString().Contains("AlreadyActive"))
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
