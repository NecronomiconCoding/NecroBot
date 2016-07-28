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

        public static async Task Execute(ISession session)
        {
            // only recycle once in 5..10 minutes (reconsider this decision after 8 minutes)
            var limit = Randomizer.GetNamed($"{nameof(RecycleItemsTask)}_limit", 5 * 60, 10 * 60, 8 * 60);
            if (_lastRecycleTime.AddSeconds(limit).Ticks > DateTime.Now.Ticks)
                return;

            var items = await session.Inventory.GetItemsToRecycle(session.Settings);
            var sum = items.Sum(x => x.Count);
            if ((session.LogicSettings.RecycleAboveItemCount > 0) && (sum < session.LogicSettings.RecycleAboveItemCount)) return;

            if (items.Any())
            {
                _lastRecycleTime = DateTime.Now;
                foreach (var item in items)
                {
                    await session.Client.Inventory.RecycleItem(item.ItemId, item.Count);

                    session.EventDispatcher.Send(new ItemRecycledEvent {Id = item.ItemId, Count = item.Count});

                    await Randomizer.Sleep(2000, 0.5);
                }
            }

            await session.Inventory.RefreshCachedInventory();
        }
    }
}