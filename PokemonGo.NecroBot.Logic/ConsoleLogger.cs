using System;
using System.IO;

namespace PokemonGo.NecroBot.Logic
{
    public static class ConsoleLogger
    {

        public static void WriteConsole(string message, LogLevel level = LogLevel.Info, ConsoleColor color = ConsoleColor.Black)
        {
            ConsoleColor formatColor;
            string formatMessage;

            switch (level)
            {
                case LogLevel.Error:
                    formatColor = ConsoleColor.Red;
                    formatMessage = $"[{DateTime.Now.ToString("HH:mm:ss")}] (ERROR) {message}";
                    break;
                case LogLevel.Warning:
                    formatColor = ConsoleColor.DarkYellow;
                    formatMessage = $"[{DateTime.Now.ToString("HH:mm:ss")}] (ATTENTION) {message}";
                    break;
                case LogLevel.Info:
                    formatColor = ConsoleColor.DarkCyan;
                    formatMessage = $"[{DateTime.Now.ToString("HH:mm:ss")}] (INFO) {message}";
                    break;
                case LogLevel.Pokestop:
                    formatColor = ConsoleColor.Cyan;
                    formatMessage = $"[{DateTime.Now.ToString("HH:mm:ss")}] (POKESTOP) {message}";
                    break;
                case LogLevel.Farming:
                    formatColor = ConsoleColor.Magenta;
                    formatMessage = $"[{DateTime.Now.ToString("HH:mm:ss")}] (FARMING) {message}";
                    break;
                case LogLevel.Recycling:
                    formatColor = ConsoleColor.DarkMagenta;
                    formatMessage = $"[{DateTime.Now.ToString("HH:mm:ss")}] (RECYCLING) {message}";
                    break;
                case LogLevel.Caught:
                    formatColor = ConsoleColor.Green;
                    formatMessage = $"[{DateTime.Now.ToString("HH:mm:ss")}] (PKMN) {message}";
                    break;
                case LogLevel.Transfer:
                    formatColor = ConsoleColor.DarkGreen;
                    formatMessage = $"[{DateTime.Now.ToString("HH:mm:ss")}] (TRANSFERED) {message}";
                    break;
                case LogLevel.Evolve:
                    formatColor = ConsoleColor.Yellow;
                    formatMessage = $"[{DateTime.Now.ToString("HH:mm:ss")}] (EVOLVED) {message}";
                    break;
                case LogLevel.Berry:
                    formatColor = ConsoleColor.DarkYellow;
                    formatMessage = $"[{DateTime.Now.ToString("HH:mm:ss")}] (BERRY) {message}";
                    break;
                case LogLevel.Debug:
                    formatColor = ConsoleColor.Gray;
                    formatMessage = $"[{DateTime.Now.ToString("HH:mm:ss")}] (DEBUG) {message}";
                    break;
                case LogLevel.None:
                    formatColor = ConsoleColor.White;
                    formatMessage = $"[{DateTime.Now.ToString("HH:mm:ss")}] {message}";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }

            if (level != LogLevel.Debug)
                ConsoleWrite(formatMessage, formatColor);
            else
                FileWrite(message);
        }

        public static void ConsoleWrite(string message, ConsoleColor color = ConsoleColor.Black)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
            FileWrite(message);
        }

        public static void FileWrite(string message)
        {
            var streamWriter = File.AppendText("log.txt");
            try
            {
                streamWriter.WriteLine(message);
            }
            finally
            {
                streamWriter.Close();
            }
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
        Info = 10,
        Debug = 11
    }
}