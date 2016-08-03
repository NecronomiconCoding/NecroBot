#region using directives

using PoGo.NecroBot.Logic.Common;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Utils;
using POGOProtos.Inventory.Item;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace PoGo.NecroBot.Logic.Tasks
{
    public class RecycleItemsTask
    {
        private static int Diff;

        public static async Task Execute(ISession session, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var currentAmountOfPokeballs = await session.Inventory.GetItemAmountByType(ItemId.ItemPokeBall);
            var currentAmountOfGreatballs = await session.Inventory.GetItemAmountByType(ItemId.ItemGreatBall);
            var currentAmountOfUltraballs = await session.Inventory.GetItemAmountByType(ItemId.ItemUltraBall);
            var currentAmountOfMasterballs = await session.Inventory.GetItemAmountByType(ItemId.ItemMasterBall);

            if (session.LogicSettings.ShowPokeballCountsBeforeRecycle)
                Logger.Write(session.Translation.GetTranslation(TranslationString.CurrentPokeballInv,
                    currentAmountOfPokeballs, currentAmountOfGreatballs, currentAmountOfUltraballs,
                    currentAmountOfMasterballs));

            var currentAmountOfPotions = await session.Inventory.GetItemAmountByType(ItemId.ItemMaxPotion) +
                await session.Inventory.GetItemAmountByType(ItemId.ItemHyperPotion) +
                await session.Inventory.GetItemAmountByType(ItemId.ItemSuperPotion) +
                await session.Inventory.GetItemAmountByType(ItemId.ItemPotion);
            var currentAmountOfRevives = await session.Inventory.GetItemAmountByType(ItemId.ItemMaxRevive) +
                await session.Inventory.GetItemAmountByType(ItemId.ItemRevive);
            var currentAmountOfBerries = await session.Inventory.GetItemAmountByType(ItemId.ItemRazzBerry) +
                await session.Inventory.GetItemAmountByType(ItemId.ItemBlukBerry) +
                await session.Inventory.GetItemAmountByType(ItemId.ItemNanabBerry) +
                await session.Inventory.GetItemAmountByType(ItemId.ItemWeparBerry) +
                await session.Inventory.GetItemAmountByType(ItemId.ItemPinapBerry);
            var currentAmountOfIncense = await session.Inventory.GetItemAmountByType(ItemId.ItemIncenseOrdinary) +
                await session.Inventory.GetItemAmountByType(ItemId.ItemIncenseSpicy) +
                await session.Inventory.GetItemAmountByType(ItemId.ItemIncenseCool) +
                await session.Inventory.GetItemAmountByType(ItemId.ItemIncenseFloral);
            var currentAmountOfLuckyEggs = await session.Inventory.GetItemAmountByType(ItemId.ItemLuckyEgg);
            var currentAmountOfLures = await session.Inventory.GetItemAmountByType(ItemId.ItemTroyDisk);

            if (session.LogicSettings.ShowPokeballCountsBeforeRecycle)
                Logger.Write(session.Translation.GetTranslation(TranslationString.CurrentItemInv,
                    currentAmountOfPotions, currentAmountOfRevives, currentAmountOfBerries, 
                    currentAmountOfIncense, currentAmountOfLuckyEggs, currentAmountOfLures));
            
            var currentTotalItems = await session.Inventory.GetTotalItemCount();
            if ((session.Profile.PlayerData.MaxItemStorage * session.LogicSettings.RecycleInventoryAtUsagePercentage/100.0f) > currentTotalItems)
                return;

            if (session.LogicSettings.TotalAmountOfPokeballsToKeep != 0)
                await OptimizedRecycleBalls(session, cancellationToken);

            if (!session.LogicSettings.VerboseRecycling)
                Logger.Write(session.Translation.GetTranslation(TranslationString.RecyclingQuietly), LogLevel.Recycling);

            if (session.LogicSettings.TotalAmountOfPotionsToKeep>=0)
                await OptimizedRecyclePotions(session, cancellationToken);

            if (session.LogicSettings.TotalAmountOfRevivesToKeep>=0)
                await OptimizedRecycleRevives(session, cancellationToken);

            if (session.LogicSettings.TotalAmountOfBerriesToKeep >= 0)
                await OptimizedRecycleBerries(session, cancellationToken);

            currentTotalItems = await session.Inventory.GetTotalItemCount();
            if ((session.Profile.PlayerData.MaxItemStorage * session.LogicSettings.RecycleInventoryAtUsagePercentage/100.0f) > currentTotalItems)
            {
                await session.Inventory.RefreshCachedInventory();
                return;
            }

            var items = await session.Inventory.GetItemsToRecycle(session);

            foreach (var item in items)
            {
                cancellationToken.ThrowIfCancellationRequested();

                await session.Client.Inventory.RecycleItem(item.ItemId, item.Count);

                if (session.LogicSettings.VerboseRecycling)
                    session.EventDispatcher.Send(new ItemRecycledEvent { Id = item.ItemId, Count = item.Count });

                DelayingUtils.Delay(session.LogicSettings.DelayBetweenPlayerActions, 500);
            }

            await session.Inventory.RefreshCachedInventory();
        }

        private static async Task RecycleItems(ISession session, CancellationToken cancellationToken, int itemCount, ItemId item)
        {
            int itemsToRecycle = 0;
            int itemsToKeep = itemCount - Diff;
            if (itemsToKeep < 0)
                itemsToKeep = 0;
            itemsToRecycle = itemCount - itemsToKeep;
            if (itemsToRecycle != 0)
            {
                Diff -= itemsToRecycle;
                cancellationToken.ThrowIfCancellationRequested();
                await session.Client.Inventory.RecycleItem(item, itemsToRecycle);
                if (session.LogicSettings.VerboseRecycling)
                    session.EventDispatcher.Send(new ItemRecycledEvent { Id = item, Count = itemsToRecycle });
                DelayingUtils.Delay(session.LogicSettings.DelayBetweenPlayerActions, 500);
            }
        }

        private static async Task OptimizedRecycleBalls(ISession session, CancellationToken cancellationToken)
        {

            var pokeBallsCount = await session.Inventory.GetItemAmountByType(ItemId.ItemPokeBall);
            var greatBallsCount = await session.Inventory.GetItemAmountByType(ItemId.ItemGreatBall);
            var ultraBallsCount = await session.Inventory.GetItemAmountByType(ItemId.ItemUltraBall);
            var masterBallsCount = await session.Inventory.GetItemAmountByType(ItemId.ItemMasterBall);

            int totalBallsCount = pokeBallsCount + greatBallsCount + ultraBallsCount + masterBallsCount;

            if (totalBallsCount > session.LogicSettings.TotalAmountOfPokeballsToKeep)
            {
                Diff = totalBallsCount - session.LogicSettings.TotalAmountOfPokeballsToKeep;
                if (Diff > 0)
                {
                    await RecycleItems(session, cancellationToken, pokeBallsCount, ItemId.ItemPokeBall);
                }
                if (Diff > 0)
                {
                    await RecycleItems(session, cancellationToken, greatBallsCount, ItemId.ItemGreatBall); 
                }
                if (Diff > 0)
                {
                    await RecycleItems(session, cancellationToken, ultraBallsCount, ItemId.ItemUltraBall);
                }
                if (Diff > 0)
                {
                    await RecycleItems(session, cancellationToken, masterBallsCount, ItemId.ItemMasterBall);
                }
            }
        }

        private static async Task OptimizedRecyclePotions(ISession session, CancellationToken cancellationToken)
        {
            var potionCount = await session.Inventory.GetItemAmountByType(ItemId.ItemPotion);
            var superPotionCount = await session.Inventory.GetItemAmountByType(ItemId.ItemSuperPotion);
            var hyperPotionsCount = await session.Inventory.GetItemAmountByType(ItemId.ItemHyperPotion);
            var maxPotionCount = await session.Inventory.GetItemAmountByType(ItemId.ItemMaxPotion);

            int totalPotionsCount = potionCount + superPotionCount + hyperPotionsCount + maxPotionCount;
            if (totalPotionsCount > session.LogicSettings.TotalAmountOfPotionsToKeep)
            {
                Diff = totalPotionsCount - session.LogicSettings.TotalAmountOfPotionsToKeep;
                if (Diff > 0)
                {
                    await RecycleItems(session, cancellationToken, potionCount, ItemId.ItemPotion);
                }

                if (Diff > 0)
                {
                    await RecycleItems(session, cancellationToken, superPotionCount, ItemId.ItemSuperPotion);
                }

                if (Diff > 0)
                {
                    await RecycleItems(session, cancellationToken, hyperPotionsCount, ItemId.ItemHyperPotion);
                }

                if (Diff > 0)
                {
                    await RecycleItems(session, cancellationToken, maxPotionCount, ItemId.ItemMaxPotion);
                }
            }
        }

        private static async Task OptimizedRecycleRevives(ISession session, CancellationToken cancellationToken)
        {
            var reviveCount = await session.Inventory.GetItemAmountByType(ItemId.ItemRevive);
            var maxReviveCount = await session.Inventory.GetItemAmountByType(ItemId.ItemMaxRevive);

            int totalRevivesCount = reviveCount + maxReviveCount;
            if (totalRevivesCount > session.LogicSettings.TotalAmountOfRevivesToKeep)
            {
                Diff = totalRevivesCount - session.LogicSettings.TotalAmountOfRevivesToKeep;
                if (Diff > 0)
                {
                    await RecycleItems(session, cancellationToken, reviveCount, ItemId.ItemRevive);
                }

                if (Diff > 0)
                {
                    await RecycleItems(session, cancellationToken, maxReviveCount, ItemId.ItemMaxRevive);
                }
            }
        }

        private static async Task OptimizedRecycleBerries(ISession session, CancellationToken cancellationToken)
        {
            var razz = await session.Inventory.GetItemAmountByType(ItemId.ItemRazzBerry);
            var bluk = await session.Inventory.GetItemAmountByType(ItemId.ItemBlukBerry);
            var nanab = await session.Inventory.GetItemAmountByType(ItemId.ItemNanabBerry);
            var pinap = await session.Inventory.GetItemAmountByType(ItemId.ItemPinapBerry);
            var wepar = await session.Inventory.GetItemAmountByType(ItemId.ItemWeparBerry);

            int totalBerryCount = razz + bluk + nanab + pinap + wepar;
            if (totalBerryCount > session.LogicSettings.TotalAmountOfBerriesToKeep)
            {
                Diff = totalBerryCount - session.LogicSettings.TotalAmountOfBerriesToKeep;
                if (Diff > 0)
                {
                    await RecycleItems(session, cancellationToken, razz, ItemId.ItemRazzBerry);
                }

                if (Diff > 0)
                {
                    await RecycleItems(session, cancellationToken, bluk, ItemId.ItemBlukBerry);
                }

                if (Diff > 0)
                {
                    await RecycleItems(session, cancellationToken, nanab, ItemId.ItemNanabBerry);
                }

                if (Diff > 0)
                {
                    await RecycleItems(session, cancellationToken, pinap, ItemId.ItemPinapBerry);
                }

                if (Diff > 0)
                {
                    await RecycleItems(session, cancellationToken, wepar, ItemId.ItemWeparBerry);
                }
            }
        }
    }
}