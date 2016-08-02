#region using directives

using System;
using System.Collections.Generic;
using System.IO;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Common;

#endregion

namespace PoGo.NecroBot.Logic.Logging
{
    public enum LogLevel
    {
        None = 0,
        Error = 1,
        Warning = 2,
        Pokestop = 3,
        Farming = 4,
        Sniper = 5,
        Recycling = 6,
        Berry = 7,
        Caught = 8,
        Transfer = 9,
        Evolve = 10,
        Egg = 11,
        Update = 12,
        Info = 13,
        Debug = 14
    }

    public static class Logger
    {
        private static ISession _session;
        private static NLog.Logger _defaultLogger = NLog.LogManager.GetCurrentClassLogger();
        private static Dictionary<LogLevel, KeyValuePair<NLog.Logger, string>> _logLevelTranslations;

        /// <summary>
        ///     Sets Context for the logger
        /// </summary>
        /// <param name="session">Context</param>
        public static void SetLoggerContext(ISession session)
        {
            _session = session;

            _logLevelTranslations = new Dictionary<LogLevel, KeyValuePair<NLog.Logger, string>>();

            var Attention =
                session?.Translation.GetTranslation(
                    TranslationString.LogEntryAttention) ?? "ATTENTION";
            _logLevelTranslations.Add(LogLevel.Warning, new KeyValuePair<NLog.Logger, string>(NLog.LogManager.GetLogger(LogLevel.Warning.ToString()), Attention));

            var Berry =
                session?.Translation.GetTranslation(
                    TranslationString.LogEntryBerry) ?? "BERRY";
            _logLevelTranslations.Add(LogLevel.Berry, new KeyValuePair<NLog.Logger, string>(NLog.LogManager.GetLogger(LogLevel.Berry.ToString()), Berry));

            var Debug =
                session?.Translation.GetTranslation(
                    TranslationString.LogEntryDebug) ?? "DEBUG";
            _logLevelTranslations.Add(LogLevel.Debug, new KeyValuePair<NLog.Logger, string>(NLog.LogManager.GetLogger(LogLevel.Debug.ToString()), Debug));

            var Egg =
                session?.Translation.GetTranslation(
                    TranslationString.LogEntryEgg) ?? "EGG";
            _logLevelTranslations.Add(LogLevel.Egg, new KeyValuePair<NLog.Logger, string>(NLog.LogManager.GetLogger(LogLevel.Egg.ToString()), Egg));

            var Error =
                session?.Translation.GetTranslation(
                    TranslationString.LogEntryError) ?? "ERROR";
            _logLevelTranslations.Add(LogLevel.Error, new KeyValuePair<NLog.Logger, string>(NLog.LogManager.GetLogger(LogLevel.Error.ToString()), Error));

            var Evolved =
                session?.Translation.GetTranslation(
                    TranslationString.LogEntryEvolved) ?? "EVOLVED";
            _logLevelTranslations.Add(LogLevel.Evolve, new KeyValuePair<NLog.Logger, string>(NLog.LogManager.GetLogger(LogLevel.Evolve.ToString()), Evolved));

            var Farming =
                session?.Translation.GetTranslation(
                    TranslationString.LogEntryFarming) ?? "FARMING";
            _logLevelTranslations.Add(LogLevel.Farming, new KeyValuePair<NLog.Logger, string>(NLog.LogManager.GetLogger(LogLevel.Farming.ToString()), Farming));

            var Info =
                session?.Translation.GetTranslation(
                    TranslationString.LogEntryInfo) ?? "INFO";
            _logLevelTranslations.Add(LogLevel.Info, new KeyValuePair<NLog.Logger, string>(NLog.LogManager.GetLogger(LogLevel.Info.ToString()), Info));

            var Pkmn =
                session?.Translation.GetTranslation(
                    TranslationString.LogEntryPkmn) ?? "PKMN";
            _logLevelTranslations.Add(LogLevel.Caught, new KeyValuePair<NLog.Logger, string>(NLog.LogManager.GetLogger(LogLevel.Caught.ToString()), Pkmn));

            var Pokestop =
                session?.Translation.GetTranslation(
                    TranslationString.LogEntryPokestop) ?? "POKESTOP";
            _logLevelTranslations.Add(LogLevel.Pokestop, new KeyValuePair<NLog.Logger, string>(NLog.LogManager.GetLogger(LogLevel.Pokestop.ToString()), Pokestop));

            var Recycling =
                session?.Translation.GetTranslation(
                    TranslationString.LogEntryRecycling) ?? "RECYCLING";
            _logLevelTranslations.Add(LogLevel.Recycling, new KeyValuePair<NLog.Logger, string>(NLog.LogManager.GetLogger(LogLevel.Recycling.ToString()), Recycling));

            var Sniper =
                session?.Translation.GetTranslation(
                    TranslationString.LogEntrySniper) ?? "SNIPER";
            _logLevelTranslations.Add(LogLevel.Sniper, new KeyValuePair<NLog.Logger, string>(NLog.LogManager.GetLogger(LogLevel.Sniper.ToString()), Sniper));

            var Transferred =
                session?.Translation.GetTranslation(
                    TranslationString.LogEntryTransfered) ?? "TRANSFERED";
            _logLevelTranslations.Add(LogLevel.Transfer, new KeyValuePair<NLog.Logger, string>(NLog.LogManager.GetLogger(LogLevel.Transfer.ToString()), Transferred));

            var Update =
                session?.Translation.GetTranslation(
                    TranslationString.LogEntryUpdate) ?? "UPDATE";
            _logLevelTranslations.Add(LogLevel.Update, new KeyValuePair<NLog.Logger, string>(NLog.LogManager.GetLogger(LogLevel.Update.ToString()), Update));
        }

        /// <summary>
        ///     Log a specific message to the logger setup by <see cref="SetLogger(ILogger)" /> .
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="level">Optional level to log. Default <see cref="LogLevel.Info" />.</param>
        public static void Write(string message, LogLevel level = LogLevel.Info)
        {
            var logger = _logLevelTranslations?[level].Key ?? _defaultLogger;
            var loggerLevelName = _logLevelTranslations?[level].Value ?? "LOG";

            switch (level)
            {
                case LogLevel.Warning:
                    logger.Warn("{0} {1}", loggerLevelName, message);
                    break;

                case LogLevel.Error:
                    logger.Error("{0} {1}", loggerLevelName, message);
                    break;

                default:
                    logger.Info("{0} {1}", loggerLevelName, message);
                    break;
            }
        }
    }
}