#region using directives

using System.Threading;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Utils;
using System.Linq;

#endregion

namespace PoGo.NecroBot.Logic.Tasks
{
    public class RecycleItemsTask
    {
        public static async Task Execute(ISession session, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var myItems = (await session.Inventory.GetItems()).ToList();

            var maxItemsCount = session.Profile.PlayerData.MaxItemStorage;
            var itemRecycleThreshold = session.LogicSettings.MinimumAvailableInvetorySpaceBeforeRecycling;

            var itemsCount = myItems.Sum(s => s.Count);

            if (maxItemsCount - itemsCount > itemRecycleThreshold)
            {
                Logging.Logger.Write($"We have enough space, postponing recylcing. Items ({itemsCount}/{maxItemsCount} Treshhold: {itemRecycleThreshold})", Logging.LogLevel.Recycling);
                return;
            }

            var items = await session.Inventory.GetItemsToRecycle(session /*, myItems*/);

            foreach (var item in items)
            {
                cancellationToken.ThrowIfCancellationRequested();

                await session.Client.Inventory.RecycleItem(item.ItemId, item.Count);

                session.EventDispatcher.Send(new ItemRecycledEvent { Id = item.ItemId, Count = item.Count });

                DelayingUtils.Delay(session.LogicSettings.DelayBetweenPlayerActions, 500);
            }

            await session.Inventory.RefreshCachedInventory();
            var updatedListOfItems = (await session.Inventory.GetItems()).ToList();

            var newItemsCount = updatedListOfItems.Sum(s => s.Count);
            Logging.Logger.Write($"Inventory space: {newItemsCount}/{maxItemsCount}", Logging.LogLevel.Recycling);
        }
    }
}