#region using directives

using System;
using PoGo.NecroBot.Logic.Common;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.State;
using POGOProtos.Inventory.Item;
using POGOProtos.Networking.Responses;

#endregion

namespace PoGo.NecroBot.CLI
{
    public class ConsoleEventListener
    {
        public void HandleEvent(ProfileEvent evt, Context ctx)
        {
            Logger.Write(ctx.Translations.GetTranslation(TranslationString.EventFortUsed,
                evt.Profile.PlayerData.Username ?? ""));
        }

        public void HandleEvent(ErrorEvent evt, Context ctx)
        {
            Logger.Write(evt.ToString(), LogLevel.Error);
        }

        public void HandleEvent(NoticeEvent evt, Context ctx)
        {
            Logger.Write(evt.ToString());
        }

        public void HandleEvent(WarnEvent evt, Context ctx)
        {
            Logger.Write(evt.ToString(), LogLevel.Warning);
        }

        public void HandleEvent(UseLuckyEggEvent evt, Context ctx)
        {
            Logger.Write(ctx.Translations.GetTranslation(TranslationString.EventUsedLuckyEgg, evt.Count), LogLevel.Egg);
        }

        public void HandleEvent(PokemonEvolveEvent evt, Context ctx)
        {
            Logger.Write(evt.Result == EvolvePokemonResponse.Types.Result.Success
                ? ctx.Translations.GetTranslation(TranslationString.EventPokemonEvolvedSuccess, evt.Id, evt.Exp)
                : ctx.Translations.GetTranslation(TranslationString.EventPokemonEvolvedFailed, evt.Id, evt.Result,
                    evt.Id),
                LogLevel.Evolve);
        }

        public void HandleEvent(TransferPokemonEvent evt, Context ctx)
        {
            Logger.Write(
                ctx.Translations.GetTranslation(TranslationString.EventPokemonTransferred, evt.Id, evt.Cp,
                    evt.Perfection.ToString("0.00"), evt.BestCp, evt.BestPerfection.ToString("0.00"), evt.FamilyCandies),
                LogLevel.Transfer);
        }

        public void HandleEvent(ItemRecycledEvent evt, Context ctx)
        {
            Logger.Write(ctx.Translations.GetTranslation(TranslationString.EventItemRecycled, evt.Count, evt.Id),
                LogLevel.Recycling);
        }

        public void HandleEvent(EggIncubatorStatusEvent evt, Context ctx)
        {
            Logger.Write(evt.WasAddedNow
                ? ctx.Translations.GetTranslation(TranslationString.IncubatorPuttingEgg, evt.KmRemaining)
                : ctx.Translations.GetTranslation(TranslationString.IncubatorStatusUpdate, evt.KmRemaining));
        }

        public void HandleEvent(EggHatchedEvent evt, Context ctx)
        {
            Logger.Write(ctx.Translations.GetTranslation(TranslationString.IncubatorEggHatched, evt.PokemonId.ToString()));
        }

        public void HandleEvent(FortUsedEvent evt, Context ctx)
        {
            Logger.Write(
                ctx.Translations.GetTranslation(TranslationString.EventFortUsed, evt.Exp, evt.Gems, evt.Items),
                LogLevel.Pokestop);
        }

        public void HandleEvent(FortFailedEvent evt, Context ctx)
        {
            Logger.Write(ctx.Translations.GetTranslation(TranslationString.EventFortFailed, evt.Retry, evt.Max),
                LogLevel.Pokestop, ConsoleColor.DarkRed);
        }

        public void HandleEvent(FortTargetEvent evt, Context ctx)
        {
            Logger.Write(
                ctx.Translations.GetTranslation(TranslationString.EventFortTargeted, evt.Name, Math.Round(evt.Distance)),
                LogLevel.Info, ConsoleColor.DarkRed);
        }

        public void HandleEvent(PokemonCaptureEvent evt, Context ctx)
        {
            Func<ItemId, string> returnRealBallName = a =>
            {
                switch (a)
                {
                    case ItemId.ItemPokeBall:
                        return ctx.Translations.GetTranslation(TranslationString.Pokeball);
                    case ItemId.ItemGreatBall:
                        return ctx.Translations.GetTranslation(TranslationString.GreatPokeball);
                    case ItemId.ItemUltraBall:
                        return ctx.Translations.GetTranslation(TranslationString.UltraPokeball);
                    case ItemId.ItemMasterBall:
                        return ctx.Translations.GetTranslation(TranslationString.MasterPokeball);
                    default:
                        return "Unknown";
                }
            };

            var catchType = evt.CatchType;

            var catchStatus = evt.Attempt > 1
                ? ctx.Translations.GetTranslation(TranslationString.CatchStatusAttempt, evt.Status, evt.Attempt)
                : ctx.Translations.GetTranslation(TranslationString.CatchStatus, evt.Status);

            var familyCandies = evt.FamilyCandies > 0
                ? ctx.Translations.GetTranslation(TranslationString.Candies, evt.FamilyCandies)
                : "";

            Logger.Write(
                ctx.Translations.GetTranslation(TranslationString.EventPokemonCapture, catchStatus, catchType, evt.Id,
                    evt.Level, evt.Cp, evt.MaxCp, evt.Perfection.ToString("0.00"), evt.Probability,
                    evt.Distance.ToString("F2"),
                    returnRealBallName(evt.Pokeball), evt.BallAmount, familyCandies), LogLevel.Caught);
        }

        public void HandleEvent(NoPokeballEvent evt, Context ctx)
        {
            Logger.Write(ctx.Translations.GetTranslation(TranslationString.EventNoPokeballs, evt.Id, evt.Cp),
                LogLevel.Caught);
        }

        public void HandleEvent(UseBerryEvent evt, Context ctx)
        {
            Logger.Write(ctx.Translations.GetTranslation(TranslationString.EventNoPokeballs, evt.Count), LogLevel.Berry);
        }

        public void HandleEvent(DisplayHighestsPokemonEvent evt, Context ctx)
        {
            string strHeader;
            //PokemonData | CP | IV | Level
            switch (evt.SortedBy)
            {
                case "Level":
                    strHeader = ctx.Translations.GetTranslation(TranslationString.DisplayHighestsLevelHeader);
                    break;
                case "IV":
                    strHeader = ctx.Translations.GetTranslation(TranslationString.DisplayHighestsPerfectHeader);
                    break;
                case "CP":
                    strHeader = ctx.Translations.GetTranslation(TranslationString.DisplayHighestsCpHeader);
                    break;
                default:
                    strHeader = ctx.Translations.GetTranslation(TranslationString.DisplayHighestsHeader);
                    break;
            }
            var strPerfect = ctx.Translations.GetTranslation(TranslationString.CommonWordPerfect);
            var strName = ctx.Translations.GetTranslation(TranslationString.CommonWordName).ToUpper();

            Logger.Write($"====== {strHeader} ======", LogLevel.Info, ConsoleColor.Yellow);
            foreach (var pokemon in evt.PokemonList)
                Logger.Write(
                    $"# CP {pokemon.Item1.Cp.ToString().PadLeft(4, ' ')}/{pokemon.Item2.ToString().PadLeft(4, ' ')} | ({pokemon.Item3.ToString("0.00")}% {strPerfect})\t| Lvl {pokemon.Item4.ToString("00")}\t {strName}: '{pokemon.Item1.PokemonId}'",
                    LogLevel.Info, ConsoleColor.Yellow);
        }

        public void HandleEvent(UpdateEvent evt, Context ctx)
        {
            Logger.Write(evt.ToString(), LogLevel.Update);
        }

        public void Listen(IEvent evt, Context ctx)
        {
            dynamic eve = evt;

            try
            {
                HandleEvent(eve, ctx);
            }
                // ReSharper disable once EmptyGeneralCatchClause
            catch
            {
            }
        }
    }
}