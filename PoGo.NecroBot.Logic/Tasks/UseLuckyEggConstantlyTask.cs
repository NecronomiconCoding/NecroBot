using System.Threading;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.State;
using POGOProtos.Inventory.Item;
using PoGo.NecroBot.Logic.Common;
using POGOProtos.Networking.Responses;
using PoGo.NecroBot.Logic.Event;

namespace PoGo.NecroBot.Logic.Tasks
{
    class UseLuckyEggConstantlyTask
    {
        public static async Task Execute(ISession session, CancellationToken cancellationToken)
        {
            if (!session.LogicSettings.UseLuckyEggConstantly)
                return;

            var currentAmountOfLuckyEggs = await session.Inventory.GetItemAmountByType(ItemId.ItemLuckyEgg);
            if (currentAmountOfLuckyEggs == 0)
            {
                session.EventDispatcher.Send(new NoticeEvent()
                {
                    Message = session.Translation.GetTranslation(TranslationString.NoEggsAvailable)
                });
                return;
            }
            else
            {
                session.EventDispatcher.Send(new NoticeEvent()
                {
                    Message = session.Translation.GetTranslation(TranslationString.UseLuckyEggAmount, currentAmountOfLuckyEggs)
                });
            }

            var UseEgg = await session.Inventory.UseLuckyEggConstantly();

            if (UseEgg.Result == UseItemXpBoostResponse.Types.Result.Success)
            {
                session.EventDispatcher.Send(new NoticeEvent()
                {
                    Message = session.Translation.GetTranslation(TranslationString.UsedLuckyEgg)
                });
            }
            else if (UseEgg.Result == UseItemXpBoostResponse.Types.Result.ErrorNoItemsRemaining)
            {
                session.EventDispatcher.Send(new NoticeEvent()
                {
                    Message = session.Translation.GetTranslation(TranslationString.NoEggsAvailable)
                });
            }
            else if (UseEgg.Result == UseItemXpBoostResponse.Types.Result.ErrorXpBoostAlreadyActive || (UseEgg.AppliedItems == null))
            {
                session.EventDispatcher.Send(new NoticeEvent()
                {
                    Message = session.Translation.GetTranslation(TranslationString.UseLuckyEggActive)
                });
            }
        }
       
    }
}
