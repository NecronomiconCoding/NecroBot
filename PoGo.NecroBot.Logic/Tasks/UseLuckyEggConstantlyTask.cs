using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.State;

namespace PoGo.NecroBot.Logic.Tasks
{
    class UseLuckyEggConstantlyTask
    {
        public static async Task Execute(ISession session, CancellationToken cancellationToken)
        {
            var UseEgg =await session.Inventory.UseLuckyEggConstantly();
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
