#region using directives

using System;
using System.Text;
using PoGo.NecroBot.Logic.Common;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.State;

#endregion

namespace PoGo.NecroBot.CLI
{
    /// <summary>
    ///     The ConsoleLogger is a simple logger which writes all logs to the Console.
    /// </summary>
    public class ConsoleLogger : ILogger
    {
        private readonly LogLevel _maxLogLevel;
        private ISession _session;

        /// <summary>
        ///     To create a ConsoleLogger, we must define a maximum log level.
        ///     All levels above won't be logged.
        /// </summary>
        /// <param name="maxLogLevel"></param>
        public ConsoleLogger(LogLevel maxLogLevel)
        {
            _maxLogLevel = maxLogLevel;
        }

        /// <summary>
        ///     Log a specific message by LogLevel. Won't log if the LogLevel is greater than the maxLogLevel set.
        /// </summary>
        /// <param name="message">The message to log. The current time will be prepended.</param>
        /// <param name="level">Optional. Default <see cref="LogLevel.Info" />.</param>
        /// <param name="color">Optional. Default is auotmatic</param>
        public void Write(string message, LogLevel level = LogLevel.Info, ConsoleColor color = ConsoleColor.Black)
        {
            //Remember to change to a font that supports your language, otherwise it'll still show as ???
            Console.OutputEncoding = Encoding.UTF8;
            if (level > _maxLogLevel)
                return;

            var strError = "ERROR";
            var strAttention = "ATTENTION";
            var strInfo = "INFO";
            var strPokestop = "POKESTOP";
            var strFarming = "FARMING";
            var strSniper = "SNIPER";
            var strRecycling = "RECYCLING";
            var strPkmn = "PKMN";
            var strTransfered = "TRANSFERED";
            var strEvolved = "EVOLVED";
            var strBerry = "BERRY";
            var strEgg = "EGG";
            var strDebug = "DEBUG";
            var strUpdate = "UPDATE";

            if (_session != null)
            {
                strError = _session.Translation.GetTranslation(TranslationString.LogEntryError);
                strAttention = _session.Translation.GetTranslation(TranslationString.LogEntryAttention);
                strInfo = _session.Translation.GetTranslation(TranslationString.LogEntryInfo);
                strPokestop = _session.Translation.GetTranslation(TranslationString.LogEntryPokestop);
                strFarming = _session.Translation.GetTranslation(TranslationString.LogEntryFarming);
                strSniper = _session.Translation.GetTranslation(TranslationString.LogEntrySniper);
                strRecycling = _session.Translation.GetTranslation(TranslationString.LogEntryRecycling);
                strPkmn = _session.Translation.GetTranslation(TranslationString.LogEntryPkmn);
                strTransfered = _session.Translation.GetTranslation(TranslationString.LogEntryTransfered);
                strEvolved = _session.Translation.GetTranslation(TranslationString.LogEntryEvolved);
                strBerry = _session.Translation.GetTranslation(TranslationString.LogEntryBerry);
                strEgg = _session.Translation.GetTranslation(TranslationString.LogEntryEgg);
                strDebug = _session.Translation.GetTranslation(TranslationString.LogEntryDebug);
                strUpdate = _session.Translation.GetTranslation(TranslationString.LogEntryUpdate);
            }

            switch (level)
            {
                case LogLevel.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] ({strError}) {message}");
                    break;
                case LogLevel.Warning:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] ({strAttention}) {message}");
                    break;
                case LogLevel.Info:
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] ({strInfo}) {message}");
                    break;
                case LogLevel.Pokestop:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] ({strPokestop}) {message}");
                    break;
                case LogLevel.Farming:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] ({strFarming}) {message}");
                    break;
                case LogLevel.Sniper:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] ({strSniper}) {message}");
                    break;
                case LogLevel.Recycling:
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] ({strRecycling}) {message}");
                    break;
                case LogLevel.Caught:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] ({strPkmn}) {message}");
                    break;
                case LogLevel.Transfer:
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] ({strTransfered}) {message}");
                    break;
                case LogLevel.Evolve:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] ({strEvolved}) {message}");
                    break;
                case LogLevel.Berry:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] ({strBerry}) {message}");
                    break;
                case LogLevel.Egg:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] ({strEgg}) {message}");
                    break;
                case LogLevel.Debug:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] ({strDebug}) {message}");
                    break;
                case LogLevel.Update:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] ({strUpdate}) {message}");
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] ({strError}) {message}");
                    break;
            }
        }

        public void SetSession(ISession session)
        {
            _session = session;
        }
    }
}