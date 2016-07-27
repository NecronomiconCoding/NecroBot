#region using directives

using System;

#endregion

namespace PoGo.NecroBot.Logic.Logging
{
    public interface ILogger
    {
        /// <summary>
        ///     Log a specific message by LogLevel.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="level">Optional. Default <see cref="LogLevel.Info" />.</param>
        /// <param name="color">Optional. Default automatic color.</param>
        void Write(string message, LogLevel level = LogLevel.Info, ConsoleColor color = ConsoleColor.Black);
    }
}