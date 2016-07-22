using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokemonGo.RocketAPI.Logging;

namespace PokemonGo.RocketAPI.Console
{

    /// <summary>
    /// The ConsoleLogger is a simple logger which writes all logs to the Console.
    /// </summary>
    public class ConsoleLogger : ILogger
    {
        private LogLevel maxLogLevel;

        /// <summary>
        /// To create a ConsoleLogger, we must define a maximum log level.
        /// All levels above won't be logged.
        /// </summary>
        /// <param name="maxLogLevel"></param>
        public ConsoleLogger(LogLevel maxLogLevel)
        {
            this.maxLogLevel = maxLogLevel;
        }

        /// <summary>
        /// Log a specific message by LogLevel. Won't log if the LogLevel is greater than the maxLogLevel set.
        /// </summary>
        /// <param name="message">The message to log. The current time will be prepended.</param>
        /// <param name="level">Optional. Default <see cref="LogLevel.Info"/>.</param>
        public void Write(string message, LogLevel level = LogLevel.Info)
        {
            if (level > maxLogLevel)
                return;

            switch (level)
            {
                case LogLevel.Error:
                    System.Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogLevel.Warning:
                    System.Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogLevel.Info:
                    System.Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case LogLevel.Debug:
                    System.Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                default:
                    System.Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
            
            System.Console.WriteLine($"[{ DateTime.Now.ToString("HH:mm:ss")}] { message}");
        }
    }
}
