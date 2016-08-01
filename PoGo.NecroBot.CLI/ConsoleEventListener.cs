#region using directives

using System;
using System.Globalization;
using PoGo.NecroBot.Logic.Common;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.State;
using POGOProtos.Enums;
using POGOProtos.Inventory.Item;
using POGOProtos.Networking.Responses;

#endregion

namespace PoGo.NecroBot.CLI
{
    public class ConsoleEventListener
    {
        public void HandleEvent(ProfileEvent evt, ISession session)
        {
            Logger.Write(session.Translation.GetTranslation(TranslationString.EventProfileLogin,
                evt.Profile.PlayerData.Username ?? ""));
        }

        public void HandleEvent(ErrorEvent evt, ISession session)
        {
            Logger.Write(evt.ToString(), LogLevel.Error);
        }

        public void HandleEvent(NoticeEvent evt, ISession session)
        {
            Logger.Write(evt.ToString());
        }

        public void HandleEvent(WarnEvent evt, ISession session)
        {
            Logger.Write(evt.ToString(), LogLevel.Warning);

            if (evt.RequireInput)
            {
                Logger.Write(session.Translation.GetTranslation(TranslationString.RequireInputText));
                Console.ReadKey();
            }
        }

        public void HandleEvent(UseLuckyEggEvent evt, ISession session)
        {
            Logger.Write(session.Translation.GetTranslation(TranslationString.EventUsedLuckyEgg, evt.Count),
                LogLevel.Egg);
        }

        public void HandleEvent(PokemonEvolveEvent evt, ISession session)
        {
            Logger.Write(evt.Result == EvolvePokemonResponse.Types.Result.Success
                ? session.Translation.GetTranslation(TranslationString.EventPokemonEvolvedSuccess, evt.Id, evt.Exp)
                : session.Translation.GetTranslation(TranslationString.EventPokemonEvolvedFailed, evt.Id, evt.Result,
                    evt.Id),
                LogLevel.Evolve);
        }

        public void HandleEvent(TransferPokemonEvent evt, ISession session)
        {
            Logger.Write(
                session.Translation.GetTranslation(TranslationString.EventPokemonTransferred, evt.Id, evt.Cp,
                    evt.Perfection.ToString("0.00"), evt.BestCp, evt.BestPerfection.ToString("0.00"), evt.FamilyCandies),
                LogLevel.Transfer);
        }

        public void HandleEvent(ItemRecycledEvent evt, ISession session)
        {
            Logger.Write(session.Translation.GetTranslation(TranslationString.EventItemRecycled, evt.Count, evt.Id),
                LogLevel.Recycling);
        }

        public void HandleEvent(EggIncubatorStatusEvent evt, ISession session)
        {
            Logger.Write(evt.WasAddedNow
                ? session.Translation.GetTranslation(TranslationString.IncubatorPuttingEgg, evt.KmRemaining)
                : session.Translation.GetTranslation(TranslationString.IncubatorStatusUpdate, evt.KmRemaining),
                LogLevel.Egg);
        }

        public void HandleEvent(EggHatchedEvent evt, ISession session)
        {
            Logger.Write(session.Translation.GetTranslation(TranslationString.IncubatorEggHatched,
                evt.PokemonId.ToString(), evt.Level, evt.Cp, evt.MaxCp, evt.Perfection),
                LogLevel.Egg);
        }

        public void HandleEvent(FortUsedEvent evt, ISession session)
        {
            var itemString = evt.InventoryFull
                ? session.Translation.GetTranslation(TranslationString.InvFullPokestopLooting)
                : evt.Items;
            Logger.Write(
                session.Translation.GetTranslation(TranslationString.EventFortUsed, evt.Name, evt.Exp, evt.Gems,
                    itemString),
                LogLevel.Pokestop);
        }

        public void HandleEvent(FortFailedEvent evt, ISession session)
        {
            Logger.Write(
                session.Translation.GetTranslation(TranslationString.EventFortFailed, evt.Name, evt.Try, evt.Max),
                LogLevel.Pokestop, ConsoleColor.DarkRed);
        }

        public void HandleEvent(FortTargetEvent evt, ISession session)
        {
            Logger.Write(
                session.Translation.GetTranslation(TranslationString.EventFortTargeted, evt.Name,
                    Math.Round(evt.Distance)),
                LogLevel.Info, ConsoleColor.DarkRed);
        }

        public void HandleEvent(PokemonCaptureEvent evt, ISession session)
        {
            Func<ItemId, string> returnRealBallName = a =>
            {
                switch (a)
                {
                    case ItemId.ItemPokeBall:
                        return session.Translation.GetTranslation(TranslationString.Pokeball);
                    case ItemId.ItemGreatBall:
                        return session.Translation.GetTranslation(TranslationString.GreatPokeball);
                    case ItemId.ItemUltraBall:
                        return session.Translation.GetTranslation(TranslationString.UltraPokeball);
                    case ItemId.ItemMasterBall:
                        return session.Translation.GetTranslation(TranslationString.MasterPokeball);
                    default:
                        return session.Translation.GetTranslation(TranslationString.CommonWordUnknown);
                }
            };

            var catchType = evt.CatchType;

            string strStatus;
            switch (evt.Status)
            {
                case CatchPokemonResponse.Types.CatchStatus.CatchError:
                    strStatus = session.Translation.GetTranslation(TranslationString.CatchStatusError);
                    break;
                case CatchPokemonResponse.Types.CatchStatus.CatchEscape:
                    strStatus = session.Translation.GetTranslation(TranslationString.CatchStatusEscape);
                    break;
                case CatchPokemonResponse.Types.CatchStatus.CatchFlee:
                    strStatus = session.Translation.GetTranslation(TranslationString.CatchStatusFlee);
                    break;
                case CatchPokemonResponse.Types.CatchStatus.CatchMissed:
                    strStatus = session.Translation.GetTranslation(TranslationString.CatchStatusMissed);
                    break;
                case CatchPokemonResponse.Types.CatchStatus.CatchSuccess:
                    strStatus = session.Translation.GetTranslation(TranslationString.CatchStatusSuccess);
                    break;
                default:
                    strStatus = evt.Status.ToString();
                    break;
            }

            var catchStatus = evt.Attempt > 1
                ? session.Translation.GetTranslation(TranslationString.CatchStatusAttempt, strStatus, evt.Attempt)
                : session.Translation.GetTranslation(TranslationString.CatchStatus, strStatus);

            var familyCandies = evt.FamilyCandies > 0
                ? session.Translation.GetTranslation(TranslationString.Candies, evt.FamilyCandies)
                : "";

            Logger.Write(
                session.Translation.GetTranslation(TranslationString.EventPokemonCapture, catchStatus, catchType, evt.Id,
                    evt.Level, evt.Cp, evt.MaxCp, evt.Perfection.ToString("0.00"), evt.Probability,
                    evt.Distance.ToString("F2"),
                    returnRealBallName(evt.Pokeball), evt.BallAmount, familyCandies), LogLevel.Caught);
        }

        public void HandleEvent(NoPokeballEvent evt, ISession session)
        {
            Logger.Write(session.Translation.GetTranslation(TranslationString.EventNoPokeballs, evt.Id, evt.Cp),
                LogLevel.Caught);
        }

        public void HandleEvent(UseBerryEvent evt, ISession session)
        {
            string strBerry;
            switch (evt.BerryType)
            {
                case ItemId.ItemRazzBerry:
                    strBerry = session.Translation.GetTranslation(TranslationString.ItemRazzBerry);
                    break;
                default:
                    strBerry = evt.BerryType.ToString();
                    break;
            }

            Logger.Write(session.Translation.GetTranslation(TranslationString.EventUseBerry, strBerry, evt.Count),
                LogLevel.Berry);
        }

        public void HandleEvent(SnipeEvent evt, ISession session)
        {
            Logger.Write(evt.ToString(), LogLevel.Sniper);
        }

        public void HandleEvent(SnipeScanEvent evt, ISession session)
        {
            Logger.Write(evt.PokemonId == PokemonId.Missingno
                ? session.Translation.GetTranslation(TranslationString.SnipeScan,
                    $"{evt.Bounds.Latitude},{evt.Bounds.Longitude}")
                : session.Translation.GetTranslation(TranslationString.SnipeScanEx, evt.PokemonId,
                    evt.Iv > 0 ? evt.Iv.ToString(CultureInfo.InvariantCulture) : "unknown",
                    $"{evt.Bounds.Latitude},{evt.Bounds.Longitude}"), LogLevel.Sniper);
        }

        public void HandleEvent(DisplayHighestsPokemonEvent evt, ISession session)
        {
            string strHeader;
            //PokemonData | CP | IV | Level | MOVE1 | MOVE2 | Candy
            switch (evt.SortedBy)
            {
                case "Level":
                    strHeader = session.Translation.GetTranslation(TranslationString.DisplayHighestsLevelHeader);
                    break;
                case "IV":
                    strHeader = session.Translation.GetTranslation(TranslationString.DisplayHighestsPerfectHeader);
                    break;
                case "CP":
                    strHeader = session.Translation.GetTranslation(TranslationString.DisplayHighestsCpHeader);
                    break;
                case "MOVE1":
                    strHeader = session.Translation.GetTranslation(TranslationString.DisplayHighestMove1Header);
                    break;
                case "MOVE2":
                    strHeader = session.Translation.GetTranslation(TranslationString.DisplayHighestMove2Header);
                    break;
                case "Candy":
                    strHeader = session.Translation.GetTranslation(TranslationString.DisplayHighestCandy);
                    break;
                default:
                    strHeader = session.Translation.GetTranslation(TranslationString.DisplayHighestsHeader);
                    break;
            }
            var strPerfect = session.Translation.GetTranslation(TranslationString.CommonWordPerfect);
            var strName = session.Translation.GetTranslation(TranslationString.CommonWordName).ToUpper();

            Logger.Write($"====== {strHeader} ======", LogLevel.Info, ConsoleColor.Yellow);
            foreach (var pokemon in evt.PokemonList)
                Logger.Write(
                    $"# CP {pokemon.Item1.Cp.ToString().PadLeft(4, ' ')}/{pokemon.Item2.ToString().PadLeft(4, ' ')} | ({pokemon.Item3.ToString("0.00")}% {strPerfect})\t| Lvl {pokemon.Item4.ToString("00")}\t {strName}: {pokemon.Item1.PokemonId.ToString().PadRight(10, ' ')}\t MOVE1: {pokemon.Item5.ToString().PadRight(20, ' ')} MOVE2: {pokemon.Item6.ToString().PadRight(20, ' ')} Candy: {pokemon.Item7}",
                    LogLevel.Info, ConsoleColor.Yellow);
        }

        public void HandleEvent( EvolveCountEvent evt, ISession session )
        {
            Logger.Write( $"[Evolves] Potential Evolves: {evt.Evolves}" + 
                ( session.LogicSettings.UseLuckyEggsWhileEvolving ? 
                    $" | {session.LogicSettings.UseLuckyEggsMinPokemonAmount} required for mass evolving" 
                    : "" ), LogLevel.Update, ConsoleColor.White );
        }

        public void HandleEvent(UpdateEvent evt, ISession session)
        {
            Logger.Write(evt.ToString(), LogLevel.Update);
        }

        public void Listen(IEvent evt, ISession session)
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