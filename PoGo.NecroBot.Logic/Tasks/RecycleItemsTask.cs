#region using directives

using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.State;

#endregion

namespace PoGo.NecroBot.Logic.Tasks
{
    public class RecycleItemsTask
    {
        public static async Task Execute(Context ctx, StateMachine machine)
        {
            var items = await ctx.Inventory.GetItemsToRecycle(ctx.Settings);

            foreach (var item in items)
            {
                await ctx.Client.Inventory.RecycleItem(item.ItemId, item.Count);

                machine.Fire(new ItemRecycledEvent {Id = item.ItemId, Count = item.Count});

                await Task.Delay(500);
            }

            await ctx.Inventory.RefreshCachedInventory();
        }
    }
}