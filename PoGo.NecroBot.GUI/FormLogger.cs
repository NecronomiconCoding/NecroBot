#region using directives

using System;
using System.Text;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.GUI;

#endregion

namespace PoGo.NecroBot.CLI
{
    /// <summary>
    ///     The FormLogger is a simple logger which writes all logs to the Windows Form.
    /// </summary>
    public class FormLogger : ILogger
    {
        private readonly LogLevel _maxLogLevel;
        private ISession _session;

        /// <summary>
        ///     To create a FormLogger, we must define a maximum log level.
        ///     All levels above won't be logged.
        /// </summary>
        /// <param name="maxLogLevel"></param>
        public FormLogger(LogLevel maxLogLevel)
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
            if (level > _maxLogLevel)
                return;

            var strError = "ERROR";
            var strAttention = "ATTENTION";
            var strInfo = "INFO";
            var strPokestop = "POKESTOP";
            var strFarming = "FARMING";
            var strRecycling = "RECYCLING";
            var strPKMN = "PKMN";
            var strTransfered = "TRANSFERED"; 
            var strEvolved = "EVOLVED";
            var strBerry = "BERRY";
            var strEgg = "EGG";
            var strDebug = "DEBUG";
            var strUpdate = "UPDATE";

            if(_session != null)
            {
                strError     = _session.Translation.GetTranslation(Logic.Common.TranslationString.LogEntryError);
                strAttention = _session.Translation.GetTranslation(Logic.Common.TranslationString.LogEntryAttention);
                strInfo      = _session.Translation.GetTranslation(Logic.Common.TranslationString.LogEntryInfo);
                strPokestop  = _session.Translation.GetTranslation(Logic.Common.TranslationString.LogEntryPokestop);
                strFarming   = _session.Translation.GetTranslation(Logic.Common.TranslationString.LogEntryFarming);
                strRecycling = _session.Translation.GetTranslation(Logic.Common.TranslationString.LogEntryRecycling);
                strPKMN      = _session.Translation.GetTranslation(Logic.Common.TranslationString.LogEntryPKMN);
                strTransfered= _session.Translation.GetTranslation(Logic.Common.TranslationString.LogEntryTransfered);
                strEvolved   = _session.Translation.GetTranslation(Logic.Common.TranslationString.LogEntryEvolved);
                strBerry     = _session.Translation.GetTranslation(Logic.Common.TranslationString.LogEntryBerry); 
                strEgg       = _session.Translation.GetTranslation(Logic.Common.TranslationString.LogEntryEgg); 
                strDebug     = _session.Translation.GetTranslation(Logic.Common.TranslationString.LogEntryDebug);
                strUpdate = _session.Translation.GetTranslation(Logic.Common.TranslationString.LogEntryUpdate);
            }

            switch (level)
            {
                case LogLevel.Error:
                    Program.gui.AppendDebugMessage($"[{DateTime.Now.ToString("HH:mm:ss")}] ({strError}) {message}", ConsoleColor.Red);
                    break;
                case LogLevel.Warning:
                    Program.gui.AppendDebugMessage($"[{DateTime.Now.ToString("HH:mm:ss")}] ({strAttention}) {message}", ConsoleColor.DarkYellow);
                    break;
                case LogLevel.Info:
                    Program.gui.AppendDebugMessage($"[{DateTime.Now.ToString("HH:mm:ss")}] ({strInfo}) {message}", ConsoleColor.DarkCyan);
                    break;
                case LogLevel.Pokestop:
                    Program.gui.AppendDebugMessage($"[{DateTime.Now.ToString("HH:mm:ss")}] ({strPokestop}) {message}", ConsoleColor.Cyan);
                    break;
                case LogLevel.Farming:
                    Program.gui.AppendDebugMessage($"[{DateTime.Now.ToString("HH:mm:ss")}] ({strFarming}) {message}", ConsoleColor.Magenta);
                    break;
                case LogLevel.Recycling:
                    Program.gui.AppendDebugMessage($"[{DateTime.Now.ToString("HH:mm:ss")}] ({strRecycling}) {message}", ConsoleColor.DarkMagenta);
                    break;
                case LogLevel.Caught:
                    Program.gui.AppendDebugMessage($"[{DateTime.Now.ToString("HH:mm:ss")}] ({strPKMN}) {message}", ConsoleColor.Green);
                    break;
                case LogLevel.Transfer:
                    Program.gui.AppendDebugMessage($"[{DateTime.Now.ToString("HH:mm:ss")}] ({strTransfered}) {message}", ConsoleColor.DarkGreen);
                    break;
                case LogLevel.Evolve:
                    Program.gui.AppendDebugMessage($"[{DateTime.Now.ToString("HH:mm:ss")}] ({strEvolved}) {message}", ConsoleColor.Yellow);
                    break;
                case LogLevel.Berry:
                    Program.gui.AppendDebugMessage($"[{DateTime.Now.ToString("HH:mm:ss")}] ({strBerry}) {message}", ConsoleColor.DarkYellow);
                    break;
                case LogLevel.Egg:
                    Program.gui.AppendDebugMessage($"[{DateTime.Now.ToString("HH:mm:ss")}] ({strEgg}) {message}", ConsoleColor.DarkYellow);
                    break;
                case LogLevel.Debug:
                    Program.gui.AppendDebugMessage($"[{DateTime.Now.ToString("HH:mm:ss")}] ({strDebug}) {message}", ConsoleColor.Gray);
                    break;
                case LogLevel.Update:
                    Program.gui.AppendDebugMessage($"[{DateTime.Now.ToString("HH:mm:ss")}] ({strUpdate}) {message}", ConsoleColor.White);
                    break;
                default:
                    Program.gui.AppendDebugMessage($"[{DateTime.Now.ToString("HH:mm:ss")}] ({strError}) {message}", ConsoleColor.White);
                    break;
            }
        }

        public void SetSession(ISession session)
        {
            _session = session;
        }
    }
}