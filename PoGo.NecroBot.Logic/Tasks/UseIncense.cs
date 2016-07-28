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
    class UseIncense
    {
        private static ISession ctx2;

        public static async Task Execute(ISession Session)
        {

            Timer t = new Timer(TimerCallback, null, 0, 300000);
            ctx2 = Session;
        }
        private static void TimerCallback(Object o)
        {
           
           
            var UseEgg = ctx2.Inventory.UseIncenseConstantly();
        
            if (UseEgg.Result.Result.ToString().ToLower().Contains("errornoitemsremaining"))
            {
                Logging.Logger.Write("No Incense Available");

            }
            else if (UseEgg.Result.Result.ToString().Contains("IncenseAlreadyActive"))
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
