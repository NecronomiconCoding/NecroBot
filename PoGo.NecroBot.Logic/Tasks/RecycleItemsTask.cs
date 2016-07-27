#region using directives

using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Utils;
using System;
using System.Linq;
using System.Threading.Tasks;
#endregion

namespace PoGo.NecroBot.Logic.Tasks
{
    public class RecycleItemsTask
    {
        private static DateTime _lastRecycleTime = DateTime.Now.AddDays(-1);

        public static async Task Execute(Context ctx, StateMachine machine)
        {
            var limit = Randomizer.GetNamed($"{nameof(RecycleItemsTask)}_limit", 5, 10, 300);
            if (_lastRecycleTime.AddMinutes(limit).Ticks > DateTime.Now.Ticks)
                return;

            var items = (await ctx.Inventory.GetItemsToRecycle(ctx.Settings)).ToList();
            var sum = items.Sum(x => x.Count);
            if ((ctx.LogicSettings.RecycleAboveItemCount > 0) && (sum < ctx.LogicSettings.RecycleAboveItemCount)) return;

            if (items.Any())
            {
                _lastRecycleTime = DateTime.Now;
                foreach (var item in items)
                {
                    await ctx.Client.Inventory.RecycleItem(item.ItemId, item.Count);

                    machine.Fire(new ItemRecycledEvent { Id = item.ItemId, Count = item.Count });

                    await Randomizer.Sleep(2000, 0.5);
                }
            }

            await ctx.Inventory.RefreshCachedInventory();
        }
    }
}