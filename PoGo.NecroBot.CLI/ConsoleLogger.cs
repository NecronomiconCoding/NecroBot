#region using directives

using System;
using System.Text;
using PoGo.NecroBot.CLI.Models;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.State;

#endregion

namespace PoGo.NecroBot.CLI
{
    /// <summary>
    ///     The ConsoleLogger is a simple logger which writes all logs to the Console.
    /// </summary>
    internal class ConsoleLogger : ILogger
    {
        private readonly LogLevel _maxLogLevel;
        private ISession _session;

        /// <summary>
        ///     To create a ConsoleLogger, we must define a maximum log level.
        ///     All levels above won't be logged.
        /// </summary>
        /// <param name="maxLogLevel"></param>
        internal ConsoleLogger(LogLevel maxLogLevel)
        {
            _maxLogLevel = maxLogLevel;
        }

        public void SetSession(ISession session)
        {
            _session = session;
            LoggingStrings.SetStrings(_session);
        }

        /// <summary>
        ///     Log a specific message by LogLevel. Won't log if the LogLevel is greater than the maxLogLevel set.
        /// </summary>
        /// <param name="message">The message to log. The current time will be prepended.</param>
        /// <param name="level">Optional. Default <see cref="LogLevel.Info" />.</param>
        /// <param name="color">Optional. Default is auotmatic</param>
        public void Write(string message, LogLevel level = LogLevel.Info, ConsoleColor color = ConsoleColor.Black)
        {
            // Remember to change to a font that supports your language, otherwise it'll still show as ???.
            Console.OutputEncoding = Encoding.UTF8;
            if (level > _maxLogLevel)
                return;

            // ReSharper disable once SwitchStatementMissingSomeCases
            string finalMessage;

            switch (level)
            {
                case LogLevel.Error:
                    Console.ForegroundColor = color == ConsoleColor.Black ? ConsoleColor.Red : color;
                    finalMessage = $"[{DateTime.Now.ToString("HH:mm:ss")}] ({LoggingStrings.Error}) {message}";
                    break;
                case LogLevel.Warning:
                    Console.ForegroundColor = color == ConsoleColor.Black ? ConsoleColor.DarkYellow : color;
                    finalMessage = $"[{DateTime.Now.ToString("HH:mm:ss")}] ({LoggingStrings.Attention}) {message}";
                    break;
                case LogLevel.Info:
                    Console.ForegroundColor = color == ConsoleColor.Black ? ConsoleColor.DarkCyan : color;
                    finalMessage = $"[{DateTime.Now.ToString("HH:mm:ss")}] ({LoggingStrings.Info}) {message}";
                    break;
                case LogLevel.Pokestop:
                    Console.ForegroundColor = color == ConsoleColor.Black ? ConsoleColor.Cyan : color;
                    finalMessage = $"[{DateTime.Now.ToString("HH:mm:ss")}] ({LoggingStrings.Pokestop}) {message}";
                    break;
                case LogLevel.Farming:
                    Console.ForegroundColor = color == ConsoleColor.Black ? ConsoleColor.Magenta : color;
                    finalMessage = $"[{DateTime.Now.ToString("HH:mm:ss")}] ({LoggingStrings.Farming}) {message}";
                    break;
                case LogLevel.Sniper:
                    Console.ForegroundColor = color == ConsoleColor.Black ? ConsoleColor.White : color;
                    finalMessage = $"[{DateTime.Now.ToString("HH:mm:ss")}] ({LoggingStrings.Sniper}) {message}";
                    break;
                case LogLevel.Recycling:
                    Console.ForegroundColor = color == ConsoleColor.Black ? ConsoleColor.DarkMagenta : color;
                    finalMessage = $"[{DateTime.Now.ToString("HH:mm:ss")}] ({LoggingStrings.Recycling}) {message}";
                    break;
                case LogLevel.Caught:
                    Console.ForegroundColor = color == ConsoleColor.Black ? ConsoleColor.Green : color;
                    finalMessage = $"[{DateTime.Now.ToString("HH:mm:ss")}] ({LoggingStrings.Pkmn}) {message}";
                    break;
                case LogLevel.Flee:
                    Console.ForegroundColor = color == ConsoleColor.Black ? ConsoleColor.DarkYellow : color;
                    finalMessage = $"[{DateTime.Now.ToString("HH:mm:ss")}] ({LoggingStrings.Pkmn}) {message}";
                    break;
                case LogLevel.Transfer:
                    Console.ForegroundColor = color == ConsoleColor.Black ? ConsoleColor.DarkGreen : color;
                    finalMessage = $"[{DateTime.Now.ToString("HH:mm:ss")}] ({LoggingStrings.Transferred}) {message}";
                    break;
                case LogLevel.Evolve:
                    Console.ForegroundColor = color == ConsoleColor.Black ? ConsoleColor.DarkGreen : color;
                    finalMessage = $"[{DateTime.Now.ToString("HH:mm:ss")}] ({LoggingStrings.Evolved}) {message}";
                    break;
                case LogLevel.Berry:
                    Console.ForegroundColor = color == ConsoleColor.Black ? ConsoleColor.DarkYellow : color;
                    finalMessage = $"[{DateTime.Now.ToString("HH:mm:ss")}] ({LoggingStrings.Berry}) {message}";
                    break;
                case LogLevel.Egg:
                    Console.ForegroundColor = color == ConsoleColor.Black ? ConsoleColor.DarkYellow : color;
                    finalMessage = $"[{DateTime.Now.ToString("HH:mm:ss")}] ({LoggingStrings.Egg}) {message}";
                    break;
                case LogLevel.Debug:
                    Console.ForegroundColor = color == ConsoleColor.Black ? ConsoleColor.Gray : color;
                    finalMessage = $"[{DateTime.Now.ToString("HH:mm:ss")}] ({LoggingStrings.Debug}) {message}";
                    break;
                case LogLevel.Update:
                    Console.ForegroundColor = color == ConsoleColor.Black ? ConsoleColor.White : color;
                    finalMessage = $"[{DateTime.Now.ToString("HH:mm:ss")}] ({LoggingStrings.Update}) {message}";
                    break;
                case LogLevel.New:
                    Console.ForegroundColor = color == ConsoleColor.Black ? ConsoleColor.Green : color;
                    finalMessage = $"[{DateTime.Now.ToString("HH:mm:ss")}] ({LoggingStrings.New}) {message}";
                    break;
                case LogLevel.SoftBan:
                    Console.ForegroundColor = color == ConsoleColor.Black ? ConsoleColor.Red : color;
                    finalMessage = $"[{DateTime.Now.ToString("HH:mm:ss")}] ({LoggingStrings.SoftBan}) {message}";
                    break;
                case LogLevel.LevelUp:
                    Console.ForegroundColor = color == ConsoleColor.Black ? ConsoleColor.Magenta : color;
                    finalMessage = $"[{DateTime.Now.ToString("HH:mm:ss")}] ({LoggingStrings.Pkmn}) {message}";
                    break;
                default:
                    Console.ForegroundColor = color == ConsoleColor.Black ? ConsoleColor.White : color;
                    finalMessage = $"[{DateTime.Now.ToString("HH:mm:ss")}] ({LoggingStrings.Error}) {message}";
                    break;
            }

            Console.WriteLine(finalMessage);
            //fix of null reference exception during (e.g.) the first start of the bot (session for logger not initialized in time)
            _session?.EventDispatcher.Send(new LogEvent
            {
                Message = finalMessage,
                Color = GetHexColor(Console.ForegroundColor)
            });
        }

        public void lineSelect(int lineChar = 0, int linesUp = 1)
        {
            Console.SetCursorPosition(lineChar, Console.CursorTop - linesUp);
        }

        public string GetHexColor(ConsoleColor color)
        {
            switch (color)
            {
                case ConsoleColor.Black:
                    return "#000000";

                case ConsoleColor.Blue:
                    return "#0000FF";

                case ConsoleColor.Cyan:
                    return "#00FFFF";

                case ConsoleColor.DarkBlue:
                    return "#000080";

                case ConsoleColor.DarkCyan:
                    return "#008B8B";

                case ConsoleColor.DarkGray:
                    return "#808080";

                case ConsoleColor.DarkGreen:
                    return "#008000";

                case ConsoleColor.DarkMagenta:
                    return "#800080";

                case ConsoleColor.DarkRed:
                    return "#800000";

                case ConsoleColor.DarkYellow:
                    return "#808000";

                case ConsoleColor.Gray:
                    return "#C0C0C0";

                case ConsoleColor.Green:
                    return "#00FF00";

                case ConsoleColor.Magenta:
                    return "#FF00FF";

                case ConsoleColor.Red:
                    return "#FF0000";

                case ConsoleColor.White:
                    return "#FFFFFF";

                case ConsoleColor.Yellow:
                    return "#FFFF00";

                default:
                    // Grey
                    return "#C0C0C0";
            }
        }
    }
}