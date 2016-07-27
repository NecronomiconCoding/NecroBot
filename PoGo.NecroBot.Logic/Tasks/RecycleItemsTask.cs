#region using directives

using System.Threading;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Utils;

#endregion

namespace PoGo.NecroBot.Logic.Tasks
{
    public class RecycleItemsTask
    {
        public static void Execute(Context ctx, StateMachine machine)
        {
            var items = ctx.Inventory.GetItemsToRecycle(ctx.Settings).Result;

            foreach (var item in items)
            {
                ctx.Client.Inventory.RecycleItem(item.ItemId, item.Count).Wait();

                machine.Fire(new ItemRecycledEvent {Id = item.ItemId, Count = item.Count});

                DelayingUtils.Delay(ctx.LogicSettings.DelayBetweenPlayerActions, 500);              
            }

            ctx.Inventory.RefreshCachedInventory().Wait();
        }
    }
}