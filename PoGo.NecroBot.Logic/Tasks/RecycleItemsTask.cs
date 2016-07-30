#region using directives
using System;
using System.Threading;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Utils;

#endregion

namespace PoGo.NecroBot.Logic.Tasks
{
    public class RecycleItemsTask
    {
        private static DateTime lastRecycle = DateTime.Now;
        public static async Task Execute(ISession session, CancellationToken cancellationToken)
        {
            if (lastRecycle.AddMilliseconds(session.LogicSettings.MinDelayBetweenRecycle) > DateTime.Now)
                return;
            cancellationToken.ThrowIfCancellationRequested();

            var items = await session.Inventory.GetItemsToRecycle(session.Settings);

            foreach (var item in items)
            {
                cancellationToken.ThrowIfCancellationRequested();

                await session.Client.Inventory.RecycleItem(item.ItemId, item.Count);

                session.EventDispatcher.Send(new ItemRecycledEvent {Id = item.ItemId, Count = item.Count});

                DelayingUtils.Delay(session.LogicSettings.DelayBetweenPlayerActions, 500);
            }
            lastRecycle = DateTime.Now;
            await session.Inventory.RefreshCachedInventory();
        }
    }
}