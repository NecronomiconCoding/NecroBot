#region using directives

using System.Threading;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Utils;
using POGOProtos.Inventory.Item;

#endregion

namespace PoGo.NecroBot.Logic.Tasks
{
    public class RecycleItemsTask
    {
        public static async Task Execute(ISession session, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var items = await session.Inventory.GetItemsToRecycle(session);

            foreach (var item in items)
            {
                cancellationToken.ThrowIfCancellationRequested();

                await session.Client.Inventory.RecycleItem(item.ItemId, item.Count);

                session.EventDispatcher.Send(new ItemRecycledEvent {Id = item.ItemId, Count = item.Count});

                DelayingUtils.Delay(session.LogicSettings.DelayBetweenPlayerActions, 500);
            }

            if (session.LogicSettings.TotalAmountOfPokebalsToKeep != 0)
            {
                await OptimizedRecycleBalls(session, cancellationToken);
            }

            await session.Inventory.RefreshCachedInventory();
        }

        private static async Task OptimizedRecycleBalls(ISession session, CancellationToken cancellationToken)
        {
            var pokeBallsCount = await session.Inventory.GetItemAmountByType(ItemId.ItemPokeBall);
            var greatBallsCount = await session.Inventory.GetItemAmountByType(ItemId.ItemGreatBall);
            var ultraBallsCount = await session.Inventory.GetItemAmountByType(ItemId.ItemUltraBall);
            var masterBallsCount = await session.Inventory.GetItemAmountByType(ItemId.ItemMasterBall);

            int pokeBallsToRecycle = 0;
            int greatBallsToRecycle = 0;
            int ultraBallsToRecycle = 0;
            int masterBallsToRecycle = 0;

            int totalBallsCount = pokeBallsCount + greatBallsCount + ultraBallsCount + masterBallsCount;
            if (totalBallsCount > session.LogicSettings.TotalAmountOfPokebalsToKeep)
            {
                int diff = totalBallsCount - session.LogicSettings.TotalAmountOfPokebalsToKeep;
                if (diff > 0)
                {
                    int pokeBallsToKeep = pokeBallsCount - diff;
                    if (pokeBallsToKeep < 0)
                    {
                        pokeBallsToKeep = 0;
                    }
                    pokeBallsToRecycle = pokeBallsCount - pokeBallsToKeep;
                    diff -= pokeBallsToRecycle;

                    cancellationToken.ThrowIfCancellationRequested();
                    await session.Client.Inventory.RecycleItem(ItemId.ItemPokeBall, pokeBallsToRecycle);
                    session.EventDispatcher.Send(new ItemRecycledEvent { Id = ItemId.ItemPokeBall, Count = pokeBallsToRecycle });
                    DelayingUtils.Delay(session.LogicSettings.DelayBetweenPlayerActions, 500);
                }

                if (diff > 0)
                {
                    int greatBallsToKeep = greatBallsCount - diff;
                    if (greatBallsToKeep < 0)
                    {
                        greatBallsToKeep = 0;
                    }
                    greatBallsToRecycle = greatBallsCount - greatBallsToKeep;
                    diff -= greatBallsToRecycle;
                    cancellationToken.ThrowIfCancellationRequested();
                    await session.Client.Inventory.RecycleItem(ItemId.ItemGreatBall, greatBallsToRecycle);
                    session.EventDispatcher.Send(new ItemRecycledEvent { Id = ItemId.ItemGreatBall, Count = greatBallsToRecycle });
                    DelayingUtils.Delay(session.LogicSettings.DelayBetweenPlayerActions, 500);
                }

                // Don't Recycle Ultra Balls
                /*
                if (diff > 0)
                {
                    int ultraBallsToKeep = ultraBallsCount - diff;
                    if (ultraBallsToKeep < 0)
                    {
                        ultraBallsToKeep = 0;
                    }
                    ultraBallsToRecycle = ultraBallsCount - ultraBallsToKeep;
                    diff -= ultraBallsToRecycle;
                    cancellationToken.ThrowIfCancellationRequested();
                    await session.Client.Inventory.RecycleItem(ItemId.ItemUltraBall, ultraBallsToRecycle);
                    session.EventDispatcher.Send(new ItemRecycledEvent { Id = ItemId.ItemUltraBall, Count = ultraBallsToRecycle });
                    DelayingUtils.Delay(session.LogicSettings.DelayBetweenPlayerActions, 500);
                }
                */

                // No Master Balls in Game, so far
                /*
                if (diff > 0)
                {
                    int masterBallsToKeep = masterBallsCount - diff;
                    if (masterBallsToKeep < 0)
                    {
                        masterBallsToKeep = 0;
                    }
                    masterBallsToRecycle = masterBallsCount - masterBallsToKeep;
                    diff -= masterBallsToRecycle;
                    cancellationToken.ThrowIfCancellationRequested();
                    await session.Client.Inventory.RecycleItem(ItemId.ItemMasterBall, masterBallsToRecycle);
                    session.EventDispatcher.Send(new ItemRecycledEvent { Id = ItemId.ItemMasterBall, Count = masterBallsToRecycle });
                    DelayingUtils.Delay(session.LogicSettings.DelayBetweenPlayerActions, 500);
                }
                */
            }
        }
    }
}