#region using directives

using System;
using System.Diagnostics.CodeAnalysis;
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
    [SuppressMessage("ReSharper", "UnusedParameter.Local")]
    internal class ConsoleEventListener
    {
        private static void HandleEvent(ProfileEvent profileEvent, ISession session)
        {
            Logger.Write(session.Translation.GetTranslation(TranslationString.EventProfileLogin,
                profileEvent.Profile.PlayerData.Username ?? ""));
        }

        private static void HandleEvent(ErrorEvent errorEvent, ISession session)
        {
            Logger.Write(errorEvent.ToString(), LogLevel.Error, force: true);
        }

        private static void HandleEvent(NoticeEvent noticeEvent, ISession session)
        {
            Logger.Write(noticeEvent.ToString());
        }

        private static void HandleEvent(WarnEvent warnEvent, ISession session)
        {
            Logger.Write(warnEvent.ToString(), LogLevel.Warning);

            if (!warnEvent.RequireInput) return;
            Logger.Write(session.Translation.GetTranslation(TranslationString.RequireInputText), LogLevel.Warning);
        }

        private static void HandleEvent(UseLuckyEggEvent useLuckyEggEvent, ISession session)
        {
            Logger.Write(session.Translation.GetTranslation(TranslationString.EventUsedLuckyEgg, useLuckyEggEvent.Count),
                LogLevel.Egg);
        }

        private static void HandleEvent(PokemonEvolveEvent pokemonEvolveEvent, ISession session)
        {
            string strPokemon = session.Translation.GetPokemonTranslation(pokemonEvolveEvent.Id);
            Logger.Write(pokemonEvolveEvent.Result == EvolvePokemonResponse.Types.Result.Success
                ? session.Translation.GetTranslation(TranslationString.EventPokemonEvolvedSuccess, strPokemon, pokemonEvolveEvent.Exp)
                : session.Translation.GetTranslation(TranslationString.EventPokemonEvolvedFailed, pokemonEvolveEvent.Id, pokemonEvolveEvent.Result,
                    strPokemon),
                LogLevel.Evolve);
        }

        private static void HandleEvent(TransferPokemonEvent transferPokemonEvent, ISession session)
        {
            Logger.Write(
                session.Translation.GetTranslation(TranslationString.EventPokemonTransferred,
                session.Translation.GetPokemonTranslation(transferPokemonEvent.Id),
                transferPokemonEvent.Cp.ToString(),
                transferPokemonEvent.Perfection.ToString("0.00"),
                transferPokemonEvent.BestCp.ToString(),
                transferPokemonEvent.BestPerfection.ToString("0.00"),
                transferPokemonEvent.FamilyCandies),
                LogLevel.Transfer);
        }

        private static void HandleEvent(ItemRecycledEvent itemRecycledEvent, ISession session)
        {
            Logger.Write(session.Translation.GetTranslation(TranslationString.EventItemRecycled, itemRecycledEvent.Count, itemRecycledEvent.Id),
                LogLevel.Recycling);
        }

        private static void HandleEvent(EggIncubatorStatusEvent eggIncubatorStatusEvent, ISession session)
        {
            Logger.Write(eggIncubatorStatusEvent.WasAddedNow
                ? session.Translation.GetTranslation(TranslationString.IncubatorPuttingEgg, eggIncubatorStatusEvent.KmRemaining)
                : session.Translation.GetTranslation(TranslationString.IncubatorStatusUpdate, eggIncubatorStatusEvent.KmRemaining),
                LogLevel.Egg);
        }

        private static void HandleEvent(EggHatchedEvent eggHatchedEvent, ISession session)
        {
            Logger.Write(session.Translation.GetTranslation(TranslationString.IncubatorEggHatched,
                session.Translation.GetPokemonTranslation(eggHatchedEvent.PokemonId), eggHatchedEvent.Level, eggHatchedEvent.Cp, eggHatchedEvent.MaxCp, eggHatchedEvent.Perfection),
                LogLevel.Egg);
        }

        private static void HandleEvent(FortUsedEvent fortUsedEvent, ISession session)
        {
            var itemString = fortUsedEvent.InventoryFull
                ? session.Translation.GetTranslation(TranslationString.InvFullPokestopLooting)
                : fortUsedEvent.Items;
            Logger.Write(
                session.Translation.GetTranslation(TranslationString.EventFortUsed, fortUsedEvent.Name, fortUsedEvent.Exp, fortUsedEvent.Gems,
                    itemString, fortUsedEvent.Latitude, fortUsedEvent.Longitude, fortUsedEvent.Altitude),
                LogLevel.Pokestop);
        }

        private static void HandleEvent(FortFailedEvent fortFailedEvent, ISession session)
        {
            if (fortFailedEvent.Try != 1 && fortFailedEvent.Looted == false)
            {
                Logger.lineSelect(0, 1); // Replaces the last line to prevent spam.
            }

            if (fortFailedEvent.Looted == true)
            {
                Logger.Write(
                session.Translation.GetTranslation(TranslationString.SoftBanBypassed),
                LogLevel.SoftBan, ConsoleColor.Green);
            }
            else
            {
                Logger.Write(
                session.Translation.GetTranslation(TranslationString.EventFortFailed, fortFailedEvent.Name, fortFailedEvent.Try, fortFailedEvent.Max),
                LogLevel.SoftBan);
            }
        }

        private static void HandleEvent(FortTargetEvent fortTargetEvent, ISession session)
        {
            int intTimeForArrival = (int)(fortTargetEvent.Distance / (session.LogicSettings.WalkingSpeedInKilometerPerHour * 0.5));

            Logger.Write(
                session.Translation.GetTranslation(TranslationString.EventFortTargeted, fortTargetEvent.Name,
                     Math.Round(fortTargetEvent.Distance), intTimeForArrival, fortTargetEvent.Route),
                LogLevel.Info, ConsoleColor.Gray);
        }

        private static void HandleEvent(PokemonCaptureEvent pokemonCaptureEvent, ISession session)
        {
            Func<ItemId, string> returnRealBallName = a =>
            {
                // ReSharper disable once SwitchStatementMissingSomeCases
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

            var catchType = pokemonCaptureEvent.CatchType;

            string strStatus;
            switch (pokemonCaptureEvent.Status)
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
                    strStatus = pokemonCaptureEvent.Status.ToString();
                    break;
            }

            var catchStatus = pokemonCaptureEvent.Attempt > 1
                ? session.Translation.GetTranslation(TranslationString.CatchStatusAttempt, strStatus, pokemonCaptureEvent.Attempt)
                : session.Translation.GetTranslation(TranslationString.CatchStatus, strStatus);

            var familyCandies = pokemonCaptureEvent.FamilyCandies > 0
                ? session.Translation.GetTranslation(TranslationString.Candies, pokemonCaptureEvent.FamilyCandies)
                : "";

            string message;

            if (pokemonCaptureEvent.Status == CatchPokemonResponse.Types.CatchStatus.CatchSuccess)
            {
                message = session.Translation.GetTranslation(TranslationString.EventPokemonCaptureSuccess, catchStatus, catchType, session.Translation.GetPokemonTranslation(pokemonCaptureEvent.Id),
                pokemonCaptureEvent.Level, pokemonCaptureEvent.Cp, pokemonCaptureEvent.MaxCp, pokemonCaptureEvent.Perfection.ToString("0.00"), pokemonCaptureEvent.Probability,
                pokemonCaptureEvent.Distance.ToString("F2"),
                returnRealBallName(pokemonCaptureEvent.Pokeball), pokemonCaptureEvent.BallAmount,
                pokemonCaptureEvent.Exp, familyCandies, pokemonCaptureEvent.Latitude.ToString("0.000000"), pokemonCaptureEvent.Longitude.ToString("0.000000"),
                pokemonCaptureEvent.Move1, pokemonCaptureEvent.Move2
               );
                Logger.Write(message, LogLevel.Caught);
            }
            else
            {
                message = session.Translation.GetTranslation(TranslationString.EventPokemonCaptureFailed, catchStatus, catchType, session.Translation.GetPokemonTranslation(pokemonCaptureEvent.Id),
                pokemonCaptureEvent.Level, pokemonCaptureEvent.Cp, pokemonCaptureEvent.MaxCp, pokemonCaptureEvent.Perfection.ToString("0.00"), pokemonCaptureEvent.Probability,
                pokemonCaptureEvent.Distance.ToString("F2"),
                returnRealBallName(pokemonCaptureEvent.Pokeball), pokemonCaptureEvent.BallAmount,
                pokemonCaptureEvent.Latitude.ToString("0.000000"), pokemonCaptureEvent.Longitude.ToString("0.000000"),
                pokemonCaptureEvent.Move1,pokemonCaptureEvent.Move2
               );
                Logger.Write(message, LogLevel.Flee);
            }

        }

        private static void HandleEvent(NoPokeballEvent noPokeballEvent, ISession session)
        {
            Logger.Write(session.Translation.GetTranslation(TranslationString.EventNoPokeballs, noPokeballEvent.Id, noPokeballEvent.Cp),
                LogLevel.Caught);
        }

        private static void HandleEvent(UseBerryEvent useBerryEvent, ISession session)
        {
            string strBerry;
            switch (useBerryEvent.BerryType)
            {
                case ItemId.ItemRazzBerry:
                    strBerry = session.Translation.GetTranslation(TranslationString.ItemRazzBerry);
                    break;
                default:
                    strBerry = useBerryEvent.BerryType.ToString();
                    break;
            }

            Logger.Write(session.Translation.GetTranslation(TranslationString.EventUseBerry, strBerry, useBerryEvent.Count),
                LogLevel.Berry);
        }

        private static void HandleEvent(SnipeEvent snipeEvent, ISession session)
        {
            Logger.Write(snipeEvent.ToString(), LogLevel.Sniper);
        }

        private static void HandleEvent(SnipeScanEvent snipeScanEvent, ISession session)
        {
            Logger.Write(snipeScanEvent.PokemonId == PokemonId.Missingno
                ? ((snipeScanEvent.Source != null) ? "(" + snipeScanEvent.Source + ") " : null) + session.Translation.GetTranslation(TranslationString.SnipeScan,
                    $"{snipeScanEvent.Bounds.Latitude},{snipeScanEvent.Bounds.Longitude}")
                : ((snipeScanEvent.Source != null) ? "(" + snipeScanEvent.Source + ") " : null) + session.Translation.GetTranslation(TranslationString.SnipeScanEx, session.Translation.GetPokemonTranslation(snipeScanEvent.PokemonId),
                    snipeScanEvent.Iv > 0 ? snipeScanEvent.Iv.ToString(CultureInfo.InvariantCulture) : session.Translation.GetTranslation(TranslationString.CommonWordUnknown),
                    $"{snipeScanEvent.Bounds.Latitude},{snipeScanEvent.Bounds.Longitude}"), LogLevel.Sniper);
        }

        private static void HandleEvent(DisplayHighestsPokemonEvent displayHighestsPokemonEvent, ISession session)
        {
            if (session.LogicSettings.AmountOfPokemonToDisplayOnStart <= 0)
            {
                return;
            }

            string strHeader;
            //PokemonData | CP | IV | Level | MOVE1 | MOVE2 | Candy
            switch (displayHighestsPokemonEvent.SortedBy)
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
            var move1 = session.Translation.GetTranslation(TranslationString.DisplayHighestMove1Header);
            var move2 = session.Translation.GetTranslation(TranslationString.DisplayHighestMove2Header);
            var candy = session.Translation.GetTranslation(TranslationString.DisplayHighestCandy);

            Logger.Write(session.Translation.GetTranslation(TranslationString.HighestsPokemoHeader, strHeader), LogLevel.Info, ConsoleColor.Yellow);
            foreach (var pokemon in displayHighestsPokemonEvent.PokemonList)
            {
                string strMove1 = session.Translation.GetPokemonMovesetTranslation(pokemon.Item5);
                string strMove2 = session.Translation.GetPokemonMovesetTranslation(pokemon.Item6);

                Logger.Write(
                    session.Translation.GetTranslation(
                        TranslationString.HighestsPokemoCell,
                        pokemon.Item1.Cp.ToString().PadLeft(4, ' '),
                        pokemon.Item2.ToString().PadLeft(4, ' '),
                        pokemon.Item3.ToString("0.00"),
                        strPerfect,
                        pokemon.Item4.ToString("00"),
                        strName,
                        session.Translation.GetPokemonTranslation(pokemon.Item1.PokemonId).PadRight(10, ' '),
                        move1,
                        strMove1.PadRight(20, ' '),
                        move2,
                        strMove2.PadRight(20, ' '),
                        candy,
                        pokemon.Item7
                    ),
                    LogLevel.Info,
                    ConsoleColor.Yellow
                );
            }
        }

        private static void HandleEvent(EvolveCountEvent evolveCountEvent, ISession session)
        {
            Logger.Write(session.Translation.GetTranslation(TranslationString.PkmPotentialEvolveCount, evolveCountEvent.Evolves), LogLevel.Evolve);
        }

        private static void HandleEvent(UpdateEvent updateEvent, ISession session)
        {
            Logger.Write(updateEvent.ToString(), LogLevel.Update);
        }

        private static void HandleEvent(SnipeModeEvent event1, ISession session) { }
        private static void HandleEvent(PokeStopListEvent event1, ISession session) { }
        private static void HandleEvent(EggsListEvent event1, ISession session) { }
        private static void HandleEvent(InventoryListEvent event1, ISession session) { }
        private static void HandleEvent(PokemonListEvent event1, ISession session) { }
        private static void HandleEvent(UpdatePositionEvent event1, ISession session)
        {
            //uncomment for more info about locations
            //Logger.Write(event1.Latitude.ToString("0.0000000000") + "," + event1.Longitude.ToString("0.0000000000"), LogLevel.Debug, force: true);
        }

        private static void HandleEvent(HumanWalkingEvent humanWalkingEvent, ISession session)
        {
            if (session.LogicSettings.ShowVariantWalking)
                Logger.Write(
                    session.Translation.GetTranslation(TranslationString.HumanWalkingVariant,
                    humanWalkingEvent.OldWalkingSpeed,
                    humanWalkingEvent.CurrentWalkingSpeed),
                    LogLevel.Info, ConsoleColor.DarkCyan);
        }

        private static void HandleEvent(KillSwitchEvent killSwitchEvent, ISession session)
        {
            if (killSwitchEvent.RequireStop)
            {
                Logger.Write(killSwitchEvent.Message, LogLevel.Warning);
                Logger.Write(session.Translation.GetTranslation(TranslationString.RequireInputText), LogLevel.Warning);
            }
            else
                Logger.Write(killSwitchEvent.Message, LogLevel.Info, ConsoleColor.White);
        }

        private static void HandleEvent(HumanWalkSnipeEvent ev, ISession session)
        {
            switch (ev.Type)
            {
                case HumanWalkSnipeEventTypes.StartWalking:
                    var strPokemon = session.Translation.GetPokemonTranslation(ev.PokemonId);
                    Logger.Write(session.Translation.GetTranslation(TranslationString.HumanWalkSnipe,
                        strPokemon,
                        ev.Latitude,
                        ev.Longitude,
                        ev.Distance,
                        ev.Expires / 60,
                        ev.Expires % 60,
                        ev.Estimate / 60,
                        ev.Estimate % 60,
                        ev.SpinPokeStop ? "Yes" : "No",
                        ev.CatchPokemon ? "Yes" : "No",
                        ev.WalkSpeedApplied),
                        LogLevel.Sniper,
                        ConsoleColor.Yellow);
                    break;
                case HumanWalkSnipeEventTypes.DestinationReached:
                    Logger.Write(session.Translation.GetTranslation(TranslationString.HumanWalkSnipeDestinationReached, ev.Latitude, ev.Longitude, ev.PauseDuration), LogLevel.Sniper);

                    break;
                case HumanWalkSnipeEventTypes.PokemonScanned:
                    Logger.Write(session.Translation.GetTranslation(TranslationString.HumanWalkSnipeUpdate, ev.Pokemons.Count, 2, 3), LogLevel.Sniper, ConsoleColor.DarkMagenta);
                    break;
                    case HumanWalkSnipeEventTypes.PokestopUpdated:
                    Logger.Write(session.Translation.GetTranslation(TranslationString.HumanWalkSnipeAddedPokestop, ev.NearestDistance, ev.Pokestops.Count), LogLevel.Sniper, ConsoleColor.Yellow);
                    break;

                case HumanWalkSnipeEventTypes.NotEnoughtPalls:
                    Logger.Write(session.Translation.GetTranslation(TranslationString.HumanWalkSnipeNotEnoughtBalls, ev.CurrentBalls, ev.MinBallsToSnipe), LogLevel.Sniper, ConsoleColor.Yellow);
                    break;
                case HumanWalkSnipeEventTypes.EncounterSnipePokemon:
                    Logger.Write(session.Translation.GetTranslation(TranslationString.HumanWalkSnipePokemonEncountered,
                        session.Translation.GetPokemonTranslation(ev.PokemonId),
                        ev.Latitude,
                        ev.Longitude));
                    break;
                default:
                    break;
            }
        }

        internal void Listen(IEvent evt, ISession session)
        {
            dynamic eve = evt;

            try
            { HandleEvent(eve, session); }
            catch
            { }
        }
    }
}
