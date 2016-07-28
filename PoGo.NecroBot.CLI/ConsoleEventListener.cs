#region using directives

using System;
using PoGo.NecroBot.Logic.Common;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.State;
using POGOProtos.Inventory.Item;
using POGOProtos.Networking.Responses;
using System.Threading;

#endregion

namespace PoGo.NecroBot.CLI
{
    public class ConsoleEventListener
    {
        public void HandleEvent(ProfileEvent evt, Session session)
        {
            Logger.Write(session.Translations.GetTranslation(TranslationString.EventProfileLogin,
                evt.Profile.PlayerData.Username ?? ""));
        }

        public void HandleEvent(ErrorEvent evt, Session session)
        {
            Logger.Write(evt.ToString(), LogLevel.Error);
        }

        public void HandleEvent(NoticeEvent evt, Session session)
        {
            Logger.Write(evt.ToString());
        }

        public void HandleEvent(WarnEvent evt, Session session)
        {
            Logger.Write(evt.ToString(), LogLevel.Warning);

            if (evt.RequireInput)
            {
                Logger.Write(session.Translations.GetTranslation(TranslationString.RequireInputText));
                Console.ReadKey();
            }

        }

        public void HandleEvent(UseLuckyEggEvent evt, Session session)
        {
            Logger.Write(session.Translations.GetTranslation(TranslationString.EventUsedLuckyEgg, evt.Count), LogLevel.Egg);
        }

        public void HandleEvent(PokemonEvolveEvent evt, Session session)
        {
            Logger.Write(evt.Result == EvolvePokemonResponse.Types.Result.Success
                ? session.Translations.GetTranslation(TranslationString.EventPokemonEvolvedSuccess, evt.Id, evt.Exp)
                : session.Translations.GetTranslation(TranslationString.EventPokemonEvolvedFailed, evt.Id, evt.Result,
                    evt.Id),
                LogLevel.Evolve);
        }

        public void HandleEvent(TransferPokemonEvent evt, Session session)
        {
            Logger.Write(
                session.Translations.GetTranslation(TranslationString.EventPokemonTransferred, evt.Id, evt.Cp,
                    evt.Perfection.ToString("0.00"), evt.BestCp, evt.BestPerfection.ToString("0.00"), evt.FamilyCandies),
                LogLevel.Transfer);
        }

        public void HandleEvent(ItemRecycledEvent evt, Session session)
        {
            Logger.Write(session.Translations.GetTranslation(TranslationString.EventItemRecycled, evt.Count, evt.Id),
                LogLevel.Recycling);
        }

        public void HandleEvent(EggIncubatorStatusEvent evt, Session session)
        {
            Logger.Write(evt.WasAddedNow
                ? session.Translations.GetTranslation(TranslationString.IncubatorPuttingEgg, evt.KmRemaining)
                : session.Translations.GetTranslation(TranslationString.IncubatorStatusUpdate, evt.KmRemaining));
        }

        public void HandleEvent(EggHatchedEvent evt, Session session)
        {
            Logger.Write(session.Translations.GetTranslation(TranslationString.IncubatorEggHatched, evt.PokemonId.ToString()));
        }

        public void HandleEvent(FortUsedEvent evt, Session session)
        {
            Logger.Write(
                session.Translations.GetTranslation(TranslationString.EventFortUsed, evt.Name, evt.Exp, evt.Gems, evt.Items),
                LogLevel.Pokestop);
        }

        public void HandleEvent(FortFailedEvent evt, Session session)
        {
            Logger.Write(session.Translations.GetTranslation(TranslationString.EventFortFailed, evt.Name, evt.Try, evt.Max),
                LogLevel.Pokestop, ConsoleColor.DarkRed);
        }

        public void HandleEvent(FortTargetEvent evt, Session session)
        {
            Logger.Write(
                session.Translations.GetTranslation(TranslationString.EventFortTargeted, evt.Name, Math.Round(evt.Distance)),
                LogLevel.Info, ConsoleColor.DarkRed);
        }

        public void HandleEvent(PokemonCaptureEvent evt, Session session)
        {
            Func<ItemId, string> returnRealBallName = a =>
            {
                switch (a)
                {
                    case ItemId.ItemPokeBall:
                        return session.Translations.GetTranslation(TranslationString.Pokeball);
                    case ItemId.ItemGreatBall:
                        return session.Translations.GetTranslation(TranslationString.GreatPokeball);
                    case ItemId.ItemUltraBall:
                        return session.Translations.GetTranslation(TranslationString.UltraPokeball);
                    case ItemId.ItemMasterBall:
                        return session.Translations.GetTranslation(TranslationString.MasterPokeball);
                    default:
                        return session.Translations.GetTranslation(TranslationString.CommonWordUnknown);
                }
            };

            var catchType = evt.CatchType;

            string strStatus;
            switch (evt.Status)
            {
                case CatchPokemonResponse.Types.CatchStatus.CatchError:
                    strStatus = session.Translations.GetTranslation(TranslationString.CatchStatusError);
                    break;
                case CatchPokemonResponse.Types.CatchStatus.CatchEscape:
                    strStatus = session.Translations.GetTranslation(TranslationString.CatchStatusEscape);
                    break;
                case CatchPokemonResponse.Types.CatchStatus.CatchFlee:
                    strStatus = session.Translations.GetTranslation(TranslationString.CatchStatusFlee);
                    break;
                case CatchPokemonResponse.Types.CatchStatus.CatchMissed:
                    strStatus = session.Translations.GetTranslation(TranslationString.CatchStatusMissed);
                    break;
                case CatchPokemonResponse.Types.CatchStatus.CatchSuccess:
                    strStatus = session.Translations.GetTranslation(TranslationString.CatchStatusSuccess);
                    break;
                default:
                    strStatus = evt.Status.ToString();
                    break;
            }

            var catchStatus = evt.Attempt > 1
                ? session.Translations.GetTranslation(TranslationString.CatchStatusAttempt, strStatus, evt.Attempt)
                : session.Translations.GetTranslation(TranslationString.CatchStatus, strStatus);

            var familyCandies = evt.FamilyCandies > 0
                ? session.Translations.GetTranslation(TranslationString.Candies, evt.FamilyCandies)
                : "";

            Logger.Write(
                session.Translations.GetTranslation(TranslationString.EventPokemonCapture, catchStatus, catchType, evt.Id,
                    evt.Level, evt.Cp, evt.MaxCp, evt.Perfection.ToString("0.00"), evt.Probability,
                    evt.Distance.ToString("F2"),
                    returnRealBallName(evt.Pokeball), evt.BallAmount, familyCandies), LogLevel.Caught);
        }

        public void HandleEvent(NoPokeballEvent evt, Session session)
        {
            Logger.Write(session.Translations.GetTranslation(TranslationString.EventNoPokeballs, evt.Id, evt.Cp),
                LogLevel.Caught);
        }

        public void HandleEvent(UseBerryEvent evt, Session session)
        {
            Logger.Write(session.Translations.GetTranslation(TranslationString.EventNoPokeballs, evt.Count), LogLevel.Berry);
        }

        public void HandleEvent(DisplayHighestsPokemonEvent evt, Session session)
        {
            string strHeader;
            //PokemonData | CP | IV | Level
            switch (evt.SortedBy)
            {
                case "Level":
                    strHeader = session.Translations.GetTranslation(TranslationString.DisplayHighestsLevelHeader);
                    break;
                case "IV":
                    strHeader = session.Translations.GetTranslation(TranslationString.DisplayHighestsPerfectHeader);
                    break;
                case "CP":
                    strHeader = session.Translations.GetTranslation(TranslationString.DisplayHighestsCpHeader);
                    break;
                default:
                    strHeader = session.Translations.GetTranslation(TranslationString.DisplayHighestsHeader);
                    break;
            }
            var strPerfect = session.Translations.GetTranslation(TranslationString.CommonWordPerfect);
            var strName = session.Translations.GetTranslation(TranslationString.CommonWordName).ToUpper();

            Logger.Write($"====== {strHeader} ======", LogLevel.Info, ConsoleColor.Yellow);
            foreach (var pokemon in evt.PokemonList)
                Logger.Write(
                    $"# CP {pokemon.Item1.Cp.ToString().PadLeft(4, ' ')}/{pokemon.Item2.ToString().PadLeft(4, ' ')} | ({pokemon.Item3.ToString("0.00")}% {strPerfect})\t| Lvl {pokemon.Item4.ToString("00")}\t {strName}: '{pokemon.Item1.PokemonId}'",
                    LogLevel.Info, ConsoleColor.Yellow);
        }

        public void HandleEvent(UpdateEvent evt, Session session)
        {
            Logger.Write(evt.ToString(), LogLevel.Update);
        }

        public void Listen(IEvent evt, Session session)
        {
            dynamic eve = evt;

            try
            {
                HandleEvent(eve, session);
            }
                // ReSharper disable once EmptyGeneralCatchClause
            catch
            {
            }
        }
    }
}