#region using directives

using System;
using System.Text;
using PoGo.NecroBot.CLI.Models;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.State;

#endregion

namespace PoGo.NecroBot.CLI
{
    /// <summary>
    /// The ConsoleLogger is a simple logger which writes all logs to the Console.
    /// </summary>
    internal class ConsoleLogger : ILogger
    {
        private readonly LogLevel _maxLogLevel;
        private ISession _session;
        
        /// <summary>
        /// To create a ConsoleLogger, we must define a maximum log level.
        /// All levels above won't be logged.
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
        /// Log a specific message by LogLevel. Won't log if the LogLevel is greater than the maxLogLevel set.
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
            switch (level)
            {
                case LogLevel.Error:
                    Console.ForegroundColor = color == ConsoleColor.Black ? ConsoleColor.(session.LogicSettings.ErrorColor) : color;
                    Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] ({LoggingStrings.Error}) {message}");
                    break;
                case LogLevel.Warning:
                    Console.ForegroundColor = color == ConsoleColor.Black ? ConsoleColor.(session.LogicSettings.WarningColor) : color;
                    Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] ({LoggingStrings.Attention}) {message}");
                    break;
                case LogLevel.Info:
                    Console.ForegroundColor = color == ConsoleColor.Black ? ConsoleColor.(session.LogicSettings.Info) : color;
                    Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] ({LoggingStrings.Info}) {message}");
                    break;
                case LogLevel.Pokestop:
                    Console.ForegroundColor = color == ConsoleColor.Black ? ConsoleColor.(session.LogicSettings.PokestopColor) : color;
                    Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] ({LoggingStrings.Pokestop}) {message}");
                    break;
                case LogLevel.Farming:
                    Console.ForegroundColor = color == ConsoleColor.Black ? ConsoleColor.(session.LogicSettings.FarmingColor) : color;
                    Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] ({LoggingStrings.Farming}) {message}");
                    break;
                case LogLevel.Sniper:
                    Console.ForegroundColor = color == ConsoleColor.Black ? ConsoleColor.(session.LogicSettings.SniperColor) : color;
                    Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] ({LoggingStrings.Sniper}) {message}");
                    break;
                case LogLevel.Recycling:
                    Console.ForegroundColor = color == ConsoleColor.Black ? ConsoleColor.(session.LogicSettings.RecyclingColor) : color;
                    Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] ({LoggingStrings.Recycling}) {message}");
                    break;
                case LogLevel.Caught:
                    Console.ForegroundColor = color == ConsoleColor.Black ? ConsoleColor.(session.LogicSettings.CaughtColor) : color;
                    Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] ({LoggingStrings.Pkmn}) {message}");
                    break;
                case LogLevel.Flee:
                    Console.ForegroundColor = color == ConsoleColor.Black ? ConsoleColor.(session.LogicSettings.FleeColor) : color;
                    Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] ({LoggingStrings.Pkmn}) {message}");
                    break;
                case LogLevel.Transfer:
                    Console.ForegroundColor = color == ConsoleColor.Black ? ConsoleColor.(session.LogicSettings.TransferColor) : color;
                    Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] ({LoggingStrings.Transferred}) {message}");
                    break;
                case LogLevel.Evolve:
                    Console.ForegroundColor = color == ConsoleColor.Black ? ConsoleColor.(session.LogicSettings.EvolveColor) : color;
                    Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] ({LoggingStrings.Evolved}) {message}");
                    break;
                case LogLevel.Berry:
                    Console.ForegroundColor = color == ConsoleColor.Black ? ConsoleColor.(session.LogicSettings.BerryColor) : color;
                    Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] ({LoggingStrings.Berry}) {message}");
                    break;
                case LogLevel.Egg:
                    Console.ForegroundColor = color == ConsoleColor.Black ? ConsoleColor.(session.LogicSettings.EggColor) : color;
                    Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] ({LoggingStrings.Egg}) {message}");
                    break;
                case LogLevel.Debug:
                    Console.ForegroundColor = color == ConsoleColor.Black ? ConsoleColor.(session.LogicSettings.DebugColor) : color;
                    Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] ({LoggingStrings.Debug}) {message}");
                    break;
                case LogLevel.Update:
                    Console.ForegroundColor = color == ConsoleColor.Black ? ConsoleColor.(session.LogicSettings.UpdateColor) : color;
                    Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] ({LoggingStrings.Update}) {message}");
                    break;
                case LogLevel.New:
                    Console.ForegroundColor = color == ConsoleColor.Black ? ConsoleColor.(session.LogicSettings.NewColor) : color;
                    Console.WriteLine( $"[{DateTime.Now.ToString( "HH:mm:ss" )}] ({LoggingStrings.New}) {message}" );
                    break;
                case LogLevel.SoftBan:
                    Console.ForegroundColor = color == ConsoleColor.Black ? ConsoleColor.(session.LogicSettings.SoftBanColor) : color;
                    Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] ({LoggingStrings.SoftBan}) {message}");
                    break;
                case LogLevel.LevelUp:
                    Console.ForegroundColor = color == ConsoleColor.Black ? ConsoleColor.(session.LogicSettings.LevelUpColor) : color;
                    Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] ({LoggingStrings.Pkmn}) {message}");
                    break;
                default:
                    Console.ForegroundColor = color == ConsoleColor.Black ? ConsoleColor.White : color;
                    Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] ({LoggingStrings.Error}) {message}");
                    break;
            }
        }

        public void lineSelect(int lineChar = 0, int linesUp = 1)
        {
            Console.SetCursorPosition(lineChar, Console.CursorTop - linesUp);
        }
    }
}
