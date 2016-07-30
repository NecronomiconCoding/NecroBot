#region using directives

using System;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.State;

#endregion

namespace PoGo.NecroBot.CLI
{
    /// <summary>
    ///     The ConsoleLogger is a simple logger which writes all logs to the Console.
    /// </summary>
    public class UIConsoleLogger : ILogger
    {
        private readonly LogLevel _maxLogLevel;
        private ISession _session;
        private ListView _logView;

        /// <summary>
        ///     To create a ConsoleLogger, we must define a maximum log level.
        ///     All levels above won't be logged.
        /// </summary>
        /// <param name="maxLogLevel"></param>
        public UIConsoleLogger(LogLevel maxLogLevel, ListView logView)
        {
            _maxLogLevel = maxLogLevel;
            _logView = logView;
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

            if (_session != null)
            {
                strError = _session.Translation.GetTranslation(Logic.Common.TranslationString.LogEntryError);
                strAttention = _session.Translation.GetTranslation(Logic.Common.TranslationString.LogEntryAttention);
                strInfo = _session.Translation.GetTranslation(Logic.Common.TranslationString.LogEntryInfo);
                strPokestop = _session.Translation.GetTranslation(Logic.Common.TranslationString.LogEntryPokestop);
                strFarming = _session.Translation.GetTranslation(Logic.Common.TranslationString.LogEntryFarming);
                strRecycling = _session.Translation.GetTranslation(Logic.Common.TranslationString.LogEntryRecycling);
                strPKMN = _session.Translation.GetTranslation(Logic.Common.TranslationString.LogEntryPKMN);
                strTransfered = _session.Translation.GetTranslation(Logic.Common.TranslationString.LogEntryTransfered);
                strEvolved = _session.Translation.GetTranslation(Logic.Common.TranslationString.LogEntryEvolved);
                strBerry = _session.Translation.GetTranslation(Logic.Common.TranslationString.LogEntryBerry);
                strEgg = _session.Translation.GetTranslation(Logic.Common.TranslationString.LogEntryEgg);
                strDebug = _session.Translation.GetTranslation(Logic.Common.TranslationString.LogEntryDebug);
                strUpdate = _session.Translation.GetTranslation(Logic.Common.TranslationString.LogEntryUpdate);
            }

            switch (level)
            {
                case LogLevel.Error:
                    _logView.Invoke((MethodInvoker)delegate {
                        var errorItem = _logView.Items.Add($"[{DateTime.Now.ToString("HH:mm:ss")}] ({strError}) {message}");
                        errorItem.BackColor = Color.Black;
                        errorItem.ForeColor = Color.Red;
                    });
                    break;
                case LogLevel.Warning:
                    _logView.Invoke((MethodInvoker)delegate {
                    var warningItem = _logView.Items.Add($"[{DateTime.Now.ToString("HH:mm:ss")}] ({strAttention}) {message}");
                    warningItem.ForeColor = Color.Yellow;
                    warningItem.BackColor = Color.Black;
                    });
                    break;
                case LogLevel.Info:
                    _logView.Invoke((MethodInvoker)delegate {
                    var infoItem = _logView.Items.Add($"[{DateTime.Now.ToString("HH:mm:ss")}] ({strInfo}) {message}");
                    infoItem.ForeColor = Color.DarkCyan;
                    infoItem.BackColor = Color.Black;
                    });
                    break;
                case LogLevel.Pokestop:
                    _logView.Invoke((MethodInvoker)delegate {
                    var pokestopItem = _logView.Items.Add($"[{DateTime.Now.ToString("HH:mm:ss")}] ({strPokestop}) {message}");
                    pokestopItem.ForeColor = Color.Cyan;
                    pokestopItem.BackColor = Color.Black;
                    });
                    break;
                case LogLevel.Farming:
                    _logView.Invoke((MethodInvoker)delegate {
                    var farmingItem = _logView.Items.Add($"[{DateTime.Now.ToString("HH:mm:ss")}] ({strFarming}) {message}");
                    farmingItem.BackColor = Color.Black;
                    farmingItem.ForeColor = Color.Magenta;
                    });
                    break;
                case LogLevel.Recycling:
                    _logView.Invoke((MethodInvoker)delegate {
                    var recyItem = _logView.Items.Add($"[{DateTime.Now.ToString("HH:mm:ss")}] ({strRecycling}) {message}");
                    recyItem.BackColor = Color.Black;
                    recyItem.ForeColor = Color.DarkMagenta;
                    });
                    break;
                case LogLevel.Caught:
                    _logView.Invoke((MethodInvoker)delegate {
                    var caughtItem = _logView.Items.Add($"[{DateTime.Now.ToString("HH:mm:ss")}] ({strPKMN}) {message}");
                    caughtItem.ForeColor = Color.Green;
                    caughtItem.BackColor = Color.Black;
                    });
                    break;
                case LogLevel.Transfer:
                    _logView.Invoke((MethodInvoker)delegate {
                    var transferItem = _logView.Items.Add($"[{DateTime.Now.ToString("HH:mm:ss")}] ({strTransfered}) {message}");
                    transferItem.BackColor = Color.Black;
                    transferItem.ForeColor = Color.DarkGreen;
                    });
                    break;
                case LogLevel.Evolve:
                    _logView.Invoke((MethodInvoker)delegate {
                    var evolveItem = _logView.Items.Add($"[{DateTime.Now.ToString("HH:mm:ss")}] ({strEvolved}) {message}");
                    evolveItem.ForeColor = Color.Yellow;
                    evolveItem.BackColor = Color.Black;
                    });
                    break;
                case LogLevel.Berry:
                    _logView.Invoke((MethodInvoker)delegate {
                    var berryItem = _logView.Items.Add($"[{DateTime.Now.ToString("HH:mm:ss")}] ({strBerry}) {message}");
                    berryItem.BackColor = Color.Black;
                    berryItem.ForeColor = Color.Yellow;
                    });
                    break;
                case LogLevel.Egg:
                    _logView.Invoke((MethodInvoker)delegate {
                    var eggItem = _logView.Items.Add($"[{DateTime.Now.ToString("HH:mm:ss")}] ({strEgg}) {message}");
                    eggItem.BackColor = Color.Black;
                    eggItem.ForeColor = Color.Yellow;
                    });
                    break;
                case LogLevel.Debug:
                    _logView.Invoke((MethodInvoker)delegate {
                    var debugItem = _logView.Items.Add($"[{DateTime.Now.ToString("HH:mm:ss")}] ({strDebug}) {message}");
                    debugItem.BackColor = Color.Black;
                    debugItem.ForeColor = Color.Gray;
                    });
                    break;
                case LogLevel.Update:
                    _logView.Invoke((MethodInvoker)delegate {
                        var updateItem = _logView.Items.Add($"[{DateTime.Now.ToString("HH:mm:ss")}] ({strUpdate}) {message}");
                    updateItem.BackColor = Color.Black;
                    updateItem.ForeColor = Color.White;
                    });
                    break;
                default:
                    _logView.Invoke((MethodInvoker)delegate {
                        var defaultItem = _logView.Items.Add($"[{DateTime.Now.ToString("HH:mm:ss")}] ({strError}) {message}");
                    defaultItem.BackColor = Color.Black;
                    defaultItem.ForeColor = Color.White;
                    });
                    break;
            }
        }

        public void SetSession(ISession session)
        {
            _session = session;
        }
    }
}