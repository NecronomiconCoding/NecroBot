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
      

        public static async Task Execute(ISession Session)
        {
            var UseEgg =await Session.Inventory.UseIncenseConstantly();

            if (UseEgg.Result.ToString().Contains("NoneInInventory"))
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
