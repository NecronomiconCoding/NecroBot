using PokemonGo.RocketAPI.GeneratedCode;
using PokemonGo.RocketAPI.Logic.Event;
using PokemonGo.RocketAPI.Logic.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PokemonGo.RocketAPI.Logic.Tasks
{
    public class RecycleItemsTask
    {
        public static void Execute(Context ctx, StateMachine machine)
        {
            var items = ctx.Inventory.GetItemsToRecycle(ctx.Settings).Result;

            foreach (var item in items)
            {
                ctx.Client.RecycleItem((ItemId)item.Item_, item.Count).Wait();

                machine.Fire(new ItemRecycledEvent { Id = item.Item_, Count = item.Count });

                Thread.Sleep(500);
            }
        }
    }
}
