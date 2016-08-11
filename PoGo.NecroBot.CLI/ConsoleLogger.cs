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
                    ConsoleColor color;
                    if (!Enum.TryParse(session.LogicSettings.ErrorColor, out color))
                    {
                    // this is the fallback color in case an invalid value was entered.
                    color = ConsoleColor.Red;
                    }

                    Console.ForegroundColor = color;
                    
                    Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] ({LoggingStrings.Error}) {message}");
                    break;
                case LogLevel.Warning:
                    ConsoleColor color;
                    if (!Enum.TryParse(session.LogicSettings.WarningColor, out color))
                    {
                    // this is the fallback color in case an invalid value was entered.
                    color = ConsoleColor.Red;
                    }

                    Console.ForegroundColor = color;
                    Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] ({LoggingStrings.Attention}) {message}");
                    break;
                case LogLevel.Info:
                    ConsoleColor color;
                    if (!Enum.TryParse(session.LogicSettings.InfoColor, out color))
                    {
                    // this is the fallback color in case an invalid value was entered.
                    color = ConsoleColor.Red;
                    }

                    Console.ForegroundColor = color;
                    Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] ({LoggingStrings.Info}) {message}");
                    break;
                case LogLevel.Pokestop:
                    ConsoleColor color;
                    if (!Enum.TryParse(session.LogicSettings.PokestopColor, out color))
                    {
                    // this is the fallback color in case an invalid value was entered.
                    color = ConsoleColor.Red;
                    }

                    Console.ForegroundColor = color;
                    
                    Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] ({LoggingStrings.Pokestop}) {message}");
                    break;
                case LogLevel.Farming:
                    ConsoleColor color;
                    if (!Enum.TryParse(session.LogicSettings.FarmingColor, out color))
                    {
                    // this is the fallback color in case an invalid value was entered.
                    color = ConsoleColor.Red;
                    }

                    Console.ForegroundColor = color;
                    
                    Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] ({LoggingStrings.Farming}) {message}");
                    break;
                case LogLevel.Sniper:
                    ConsoleColor color;
                    if (!Enum.TryParse(session.LogicSettings.SniperColor, out color))
                    {
                    // this is the fallback color in case an invalid value was entered.
                    color = ConsoleColor.Red;
                    }

                    Console.ForegroundColor = color;
                    
                    Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] ({LoggingStrings.Sniper}) {message}");
                    break;
                case LogLevel.Recycling:
                    ConsoleColor color;
                    if (!Enum.TryParse(session.LogicSettings.RecyclingColor, out color))
                    {
                    // this is the fallback color in case an invalid value was entered.
                    color = ConsoleColor.Red;
                    }

                    Console.ForegroundColor = color;
                    
                    Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] ({LoggingStrings.Recycling}) {message}");
                    break;
                case LogLevel.Caught:
                    ConsoleColor color;
                    if (!Enum.TryParse(session.LogicSettings.CaughtColor, out color))
                    {
                    // this is the fallback color in case an invalid value was entered.
                    color = ConsoleColor.Red;
                    }

                    Console.ForegroundColor = color;
                    
                    Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] ({LoggingStrings.Pkmn}) {message}");
                    break;
                case LogLevel.Flee:
                    ConsoleColor color;
                    if (!Enum.TryParse(session.LogicSettings.FleeColor, out color))
                    {
                    // this is the fallback color in case an invalid value was entered.
                    color = ConsoleColor.Red;
                    }

                    Console.ForegroundColor = color;
                    
                    Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] ({LoggingStrings.Pkmn}) {message}");
                    break;
                case LogLevel.Transfer:
                    ConsoleColor color;
                    if (!Enum.TryParse(session.LogicSettings.TransferColor, out color))
                    {
                    // this is the fallback color in case an invalid value was entered.
                    color = ConsoleColor.Red;
                    }

                    Console.ForegroundColor = color;
                    
                    Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] ({LoggingStrings.Transferred}) {message}");
                    break;
                case LogLevel.Evolve:
                    ConsoleColor color;
                    if (!Enum.TryParse(session.LogicSettings.EvolveColor, out color))
                    {
                    // this is the fallback color in case an invalid value was entered.
                    color = ConsoleColor.Red;
                    }

                    Console.ForegroundColor = color;
                    
                    Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] ({LoggingStrings.Evolved}) {message}");
                    break;
                case LogLevel.Berry:
                    ConsoleColor color;
                    if (!Enum.TryParse(session.LogicSettings.BerryColor, out color))
                    {
                    // this is the fallback color in case an invalid value was entered.
                    color = ConsoleColor.Red;
                    }

                    Console.ForegroundColor = color;
                    
                    Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] ({LoggingStrings.Berry}) {message}");
                    break;
                case LogLevel.Egg:
                    ConsoleColor color;
                    if (!Enum.TryParse(session.LogicSettings.EggColor, out color))
                    {
                    // this is the fallback color in case an invalid value was entered.
                    color = ConsoleColor.Red;
                    }

                    Console.ForegroundColor = color;
                    
                    Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] ({LoggingStrings.Egg}) {message}");
                    break;
                case LogLevel.Debug:
                    ConsoleColor color;
                    if (!Enum.TryParse(session.LogicSettings.DebugColor, out color))
                    {
                    // this is the fallback color in case an invalid value was entered.
                    color = ConsoleColor.Red;
                    }

                    Console.ForegroundColor = color;
                    
                    Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] ({LoggingStrings.Debug}) {message}");
                    break;
                case LogLevel.Update:
                    ConsoleColor color;
                    if (!Enum.TryParse(session.LogicSettings.UpdateColor, out color))
                    {
                    // this is the fallback color in case an invalid value was entered.
                    color = ConsoleColor.Red;
                    }

                    Console.ForegroundColor = color;
                    
                    Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] ({LoggingStrings.Update}) {message}");
                    break;
                case LogLevel.New:
                    ConsoleColor color;
                    if (!Enum.TryParse(session.LogicSettings.NewColor, out color))
                    {
                    // this is the fallback color in case an invalid value was entered.
                    color = ConsoleColor.Red;
                    }

                    Console.ForegroundColor = color;
                    
                    Console.WriteLine( $"[{DateTime.Now.ToString( "HH:mm:ss" )}] ({LoggingStrings.New}) {message}" );
                    break;
                case LogLevel.SoftBan:
                    ConsoleColor color;
                    if (!Enum.TryParse(session.LogicSettings.SoftBanColor, out color))
                    {
                    // this is the fallback color in case an invalid value was entered.
                    color = ConsoleColor.Red;
                    }

                    Console.ForegroundColor = color;
                    
                    Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] ({LoggingStrings.SoftBan}) {message}");
                    break;
                case LogLevel.LevelUp:
                    ConsoleColor color;
                    if (!Enum.TryParse(session.LogicSettings.LevelUpColor, out color))
                    {
                    // this is the fallback color in case an invalid value was entered.
                    color = ConsoleColor.Red;
                    }

                    Console.ForegroundColor = color;
                    
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
