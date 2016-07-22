#region

using System;
using PokemonGo.RocketAPI.Logging;

#endregion

namespace PokemonGo.RocketAPI.Console
{
    /// <summary>
    ///     The ConsoleLogger is a simple logger which writes all logs to the Console.
    /// </summary>
    public class ConsoleLogger : ILogger
    {
        private readonly LogLevel maxLogLevel;

        /// <summary>
        ///     To create a ConsoleLogger, we must define a maximum log level.
        ///     All levels above won't be logged.
        /// </summary>
        /// <param name="maxLogLevel"></param>
        public ConsoleLogger(LogLevel maxLogLevel)
        {
            this.maxLogLevel = maxLogLevel;
        }

        /// <summary>
        ///     Log a specific message by LogLevel. Won't log if the LogLevel is greater than the maxLogLevel set.
        /// </summary>
        /// <param name="message">The message to log. The current time will be prepended.</param>
        /// <param name="level">Optional. Default <see cref="LogLevel.Info" />.</param>
        /// <param name="color">Optional. Default is auotmatic</param>
        public void Write(string message, LogLevel level = LogLevel.Info, ConsoleColor color = ConsoleColor.Black)
        {
            if (level > maxLogLevel)
                return;

            switch (level)
            {
                case LogLevel.Error:
                    System.Console.ForegroundColor = color != ConsoleColor.Black ? color : ConsoleColor.Red;
                    System.Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] (ERROR) {message}");
                    break;
                case LogLevel.Warning:
                    System.Console.ForegroundColor = color != ConsoleColor.Black ? color : ConsoleColor.Yellow;
                    System.Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] (WARNING) {message}");
                    break;
                case LogLevel.Info:
                    System.Console.ForegroundColor = color != ConsoleColor.Black ? color : ConsoleColor.Gray;
                    System.Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] (INFO) {message}");
                    break;
                case LogLevel.Debug:
                    System.Console.ForegroundColor = color != ConsoleColor.Black ? color : ConsoleColor.Gray;
                    System.Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] (DEBUG) {message}");
                    break;
                case LogLevel.None:
                    System.Console.ForegroundColor = color != ConsoleColor.Black ? color : ConsoleColor.White;
                    System.Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] (ERROR) {message}");
                    break;
                default:
                    System.Console.ForegroundColor = color != ConsoleColor.Black ? color : ConsoleColor.White;
                    System.Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] (ERROR) {message}");
                    break;
            }
        }
    }
}