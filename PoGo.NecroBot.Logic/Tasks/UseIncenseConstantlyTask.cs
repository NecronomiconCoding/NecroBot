using System.Threading;
using System.Threading.Tasks;
using POGOProtos.Inventory.Item;
using PoGo.NecroBot.Logic.Common;
using POGOProtos.Networking.Responses;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Event;

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
                session.EventDispatcher.Send(new NoticeEvent()
                {
                    Message = session.Translation.GetTranslation(TranslationString.NoIncenseAvailable)
                });
                return;
            }
            else
            {
                session.EventDispatcher.Send(new NoticeEvent()
                {
                    Message = session.Translation.GetTranslation(TranslationString.UseIncenseAmount, currentAmountOfIncense)
                });
            }

            var UseIncense = await session.Inventory.UseIncenseConstantly();

            if (UseIncense.Result == UseIncenseResponse.Types.Result.Success)
            {
                session.EventDispatcher.Send(new NoticeEvent()
                {
                    Message = session.Translation.GetTranslation(TranslationString.UsedIncense)
                });
            }
            else if (UseIncense.Result == UseIncenseResponse.Types.Result.NoneInInventory)
            {
                session.EventDispatcher.Send(new NoticeEvent()
                {
                    Message = session.Translation.GetTranslation(TranslationString.NoIncenseAvailable)
                });
            }
            else if (UseIncense.Result == UseIncenseResponse.Types.Result.IncenseAlreadyActive || (UseIncense.AppliedIncense == null))
            {
                session.EventDispatcher.Send(new NoticeEvent()
                {
                    Message = session.Translation.GetTranslation(TranslationString.UseIncenseActive)
                });
            }
        }
    }
}
