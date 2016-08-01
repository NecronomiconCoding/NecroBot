using System.Threading;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.State;
using POGOProtos.Inventory.Item;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.Common;

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
                Logger.Write(session.Translation.GetTranslation(TranslationString.NoEggsAvailable));
                return;
            }

            var UseEgg = await session.Inventory.UseLuckyEggConstantly();

            if (UseEgg.Result.ToString().Contains("Success"))
            {
                Logger.Write(session.Translation.GetTranslation(TranslationString.EventUsedLuckyEgg, currentAmountOfLuckyEggs));
            }
            else if (UseEgg.Result.ToString().ToLower().Contains("errornoitemsremaining"))
            {
                Logger.Write(session.Translation.GetTranslation(TranslationString.NoEggsAvailable));
            }
            else if (UseEgg.Result.ToString().Contains("AlreadyActive") || (UseEgg.AppliedItems == null))
            {
                Logger.Write(session.Translation.GetTranslation(TranslationString.UseLuckyEggActive));
            }
        }
       
    }
}
