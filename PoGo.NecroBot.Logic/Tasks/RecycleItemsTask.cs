#region using directives

using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Utils;

#endregion

namespace PoGo.NecroBot.Logic.Tasks
{
    public class RecycleItemsTask
    {
        public static async Task Execute(ISession session)
        {
            var items = await session.Inventory.GetItemsToRecycle(session.Settings);

            foreach (var item in items)
            {
                await session.Client.Inventory.RecycleItem(item.ItemId, item.Count);

                session.EventDispatcher.Send(new ItemRecycledEvent {Id = item.ItemId, Count = item.Count});

                DelayingUtils.Delay(session.LogicSettings.DelayBetweenPlayerActions, 500);
            }

            await session.Inventory.RefreshCachedInventory();
            await Statistics.LogInventory(session);
        }
    }
}