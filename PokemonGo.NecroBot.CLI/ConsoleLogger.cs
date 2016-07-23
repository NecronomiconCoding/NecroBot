#region

using System;
using PokemonGo.RocketAPI;
using PokemonGo.RocketAPI.Logging;

#endregion

namespace PokemonGo.NecroBot.CLI
{
    /// <summary>
    ///     The ConsoleLogger is a simple logger which writes all logs to the Console.
    /// </summary>
    public class ConsoleLogger : ILogger
    {
        private readonly LogLevel _maxLogLevel;

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
        /// <param name="level">Optional. Default <see cref="System.LogLevel.Info" />.</param>
        /// <param name="color">Optional. Default is auotmatic</param>
        public void Write(string message, LogLevel level = LogLevel.Info, ConsoleColor color = ConsoleColor.Black)
        {
            if (level > _maxLogLevel)
                return;

            switch (level)
            {
                case LogLevel.Error:
                    System.Console.ForegroundColor = ConsoleColor.Red;
                    System.Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] (ERROR) {message}");
                    break;
                case LogLevel.Warning:
                    System.Console.ForegroundColor = ConsoleColor.DarkYellow;
                    System.Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] (ATTENTION) {message}");
                    break;
                case LogLevel.Info:
                    System.Console.ForegroundColor = ConsoleColor.DarkCyan;
                    System.Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] (INFO) {message}");
                    break;
                case LogLevel.Pokestop:
                    System.Console.ForegroundColor = ConsoleColor.Cyan;
                    System.Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] (POKESTOP) {message}");
                    break;
                case LogLevel.Farming:
                    System.Console.ForegroundColor = ConsoleColor.Magenta;
                    System.Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] (FARMING) {message}");
                    break;
                case LogLevel.Recycling:
                    System.Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    System.Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] (RECYCLING) {message}");
                    break;
                case LogLevel.Caught:
                    System.Console.ForegroundColor = ConsoleColor.Green;
                    System.Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] (PKMN) {message}");
                    break;
                case LogLevel.Transfer:
                    System.Console.ForegroundColor = ConsoleColor.DarkGreen;
                    System.Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] (TRANSFERED) {message}");
                    break;
                case LogLevel.Evolve:
                    System.Console.ForegroundColor = ConsoleColor.Yellow;
                    System.Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] (EVOLVED) {message}");
                    break;
                case LogLevel.Berry:
                    System.Console.ForegroundColor = ConsoleColor.DarkYellow;
                    System.Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] (BERRY) {message}");
                    break;
                case LogLevel.Debug:
                    System.Console.ForegroundColor = ConsoleColor.Gray;
                    System.Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] (DEBUG) {message}");
                    break;
                default:
                    System.Console.ForegroundColor = ConsoleColor.White;
                    System.Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] (ERROR) {message}");
                    break;
            }
        }
    }
}
