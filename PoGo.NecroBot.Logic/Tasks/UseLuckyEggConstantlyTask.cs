using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.State;
using POGOProtos.Inventory.Item;
using POGOProtos.Networking.Responses;

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
                Logging.Logger.Write(session.Translation.GetTranslation(Common.TranslationString.NoEggsAvailable));
                return;
            }
            else
            {
                Logging.Logger.Write(session.Translation.GetTranslation(Common.TranslationString.UseLuckyEggAmount, currentAmountOfLuckyEggs));
            }

            var UseEgg = await session.Inventory.UseLuckyEggConstantly();

            if (UseEgg.Result == UseItemXpBoostResponse.Types.Result.Success)
            {
                Logging.Logger.Write(session.Translation.GetTranslation(Common.TranslationString.UsedLuckyEgg));
            }
            else if (UseEgg.Result == UseItemXpBoostResponse.Types.Result.ErrorNoItemsRemaining)
            {
                Logging.Logger.Write(session.Translation.GetTranslation(Common.TranslationString.NoEggsAvailable));
            }
            else if (UseEgg.Result == UseItemXpBoostResponse.Types.Result.ErrorXpBoostAlreadyActive || (UseEgg.AppliedItems == null))
            {
                Logging.Logger.Write(session.Translation.GetTranslation(Common.TranslationString.UseLuckyEggActive));
            }
        }
       
    }
}
