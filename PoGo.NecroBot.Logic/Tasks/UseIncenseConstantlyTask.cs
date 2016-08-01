using System.Threading;
using System.Threading.Tasks;
using POGOProtos.Inventory.Item;
using PoGo.NecroBot.Logic.Common;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.State;

namespace PoGo.NecroBot.Logic.Tasks
{
    class UseIncenseConstantlyTask
    {
        public static async Task Execute(ISession session, CancellationToken cancellationToken)
        {
            if (!session.LogicSettings.UseIncenseConstantly)
                return;

            var currentAmountOfIncense = await session.Inventory.GetItemAmountByType(ItemId.ItemIncenseOrdinary);
            if (currentAmountOfIncense == 0)
            {
                Logger.Write(session.Translation.GetTranslation(TranslationString.NoIncenseAvailable));
                return;
            }

            var UseIncense = await session.Inventory.UseIncenseConstantly();

            if (UseIncense.Result.ToString().Contains("Success"))
            {
                Logger.Write(session.Translation.GetTranslation(TranslationString.EventUsedIncense, currentAmountOfIncense));
            }
            else if (UseIncense.Result.ToString().ToLower().Contains("errornoitemsremaining") ||
                UseIncense.Result.ToString().ToLower().Contains("noneininventory"))
            {
                Logger.Write(session.Translation.GetTranslation(TranslationString.NoIncenseAvailable));
            }
            else if (UseIncense.Result.ToString().Contains("AlreadyActive") || (UseIncense.AppliedIncense == null))
            {
                Logger.Write(session.Translation.GetTranslation(TranslationString.UseIncenseActive));
            }
        }
    }
}
