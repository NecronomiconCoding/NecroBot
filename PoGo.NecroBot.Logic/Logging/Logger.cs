#region using directives

using System;
using System.IO;

#endregion

namespace PoGo.NecroBot.Logic.Logging
{
    public static class Logger
    {
        private static ILogger _logger;
        private static string _subPath;

        private static void Log(string message)
        {
            // maybe do a new log rather than appending?
            Directory.CreateDirectory(Directory.GetCurrentDirectory() + _subPath + Path.DirectorySeparatorChar + "Logs");


            using (
                var log =
                    File.AppendText(Directory.GetCurrentDirectory() + _subPath +
                                    $"{Path.DirectorySeparatorChar}Logs{Path.DirectorySeparatorChar}NecroBot-{DateTime.Today.ToString("yyyy-MM-dd")}-{DateTime.Now.ToString("HH")}.txt")
                )
            {
                log.WriteLine(message);
                log.Flush();
            }
        }

        /// <summary>
        ///     Set the logger. All future requests to <see cref="Write(string,LogLevel,ConsoleColor)" /> will use that logger, any
        ///     old will be
        ///     unset.
        /// </summary>
        /// <param name="logger"></param>
        public static void SetLogger(ILogger logger, string subPath = "")
        {
            _logger = logger;
            _subPath = subPath;
            Log($"Initializing Rocket logger at time {DateTime.Now}...");
        }

        /// <summary>
        ///     Log a specific message to the logger setup by <see cref="SetLogger(ILogger)" /> .
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="level">Optional level to log. Default <see cref="LogLevel.Info" />.</param>
        /// <param name="color">Optional. Default is automatic color.</param>
        public static void Write(string message, LogLevel level = LogLevel.Info, ConsoleColor color = ConsoleColor.Black)
        {
            if (_logger == null)
                return;
            _logger.Write(message, level, color);
            Log(string.Concat($"[{DateTime.Now.ToString("HH:mm:ss")}] ", message));
        }
    }

    public enum LogLevel
    {
        None = 0,
        Error = 1,
        Warning = 2,
        Pokestop = 3,
        Farming = 4,
        Recycling = 5,
        Berry = 6,
        Caught = 7,
        Transfer = 8,
        Evolve = 9,
        Egg = 10,
        Update = 11,
        Info = 12,
        Debug = 13,
        
    }
}