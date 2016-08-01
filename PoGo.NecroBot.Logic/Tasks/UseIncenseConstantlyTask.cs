using PoGo.NecroBot.Logic.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using POGOProtos.Inventory.Item;
using POGOProtos.Networking.Responses;

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
                Logging.Logger.Write(session.Translation.GetTranslation(Common.TranslationString.NoIncenseAvailable));
                return;
            }
            else
            {
                Logging.Logger.Write(session.Translation.GetTranslation(Common.TranslationString.UseIncenseAmount, currentAmountOfIncense));
            }

            var UseIncense = await session.Inventory.UseIncenseConstantly();

            if (UseIncense.Result == UseIncenseResponse.Types.Result.Success)
            {
                Logging.Logger.Write(session.Translation.GetTranslation(Common.TranslationString.UsedIncense));
            }
            else if (UseIncense.Result == UseIncenseResponse.Types.Result.NoneInInventory)
            {
                Logging.Logger.Write(session.Translation.GetTranslation(Common.TranslationString.NoIncenseAvailable));
            }
            else if (UseIncense.Result == UseIncenseResponse.Types.Result.IncenseAlreadyActive || (UseIncense.AppliedIncense == null))
            {
                Logging.Logger.Write(session.Translation.GetTranslation(Common.TranslationString.UseIncenseActive));
            }
        }
    }
}
