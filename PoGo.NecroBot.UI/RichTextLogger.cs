using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PoGo.NecroBot.Logic.Logging;

namespace PoGo.NecroBot.UI
{
    internal class RichTextLogger : ILogger
    {
        private readonly RichTextBox _richTextBox;
        private readonly LogLevel _maxLogLevel;

        private delegate void WriteCallback(string message, LogLevel level = LogLevel.Info, ConsoleColor color = ConsoleColor.Black);

        /// <summary>
        ///     To create a RichTextLogger, we must specify the RichTextBox and define a maximum log level.
        ///     All levels above won't be logged.
        /// </summary>
        /// <param name="richTextBox">RichTextBox to log to.</param>
        /// <param name="maxLogLevel">Maximum log level to display.</param>
        public RichTextLogger(RichTextBox richTextBox, LogLevel maxLogLevel)
        {
            _richTextBox = richTextBox;
            _maxLogLevel = maxLogLevel;
        }

        /// <summary>
        ///     Log a specific message by LogLevel. Won't log if the LogLevel is greater than the maxLogLevel set.
        /// </summary>
        /// <param name="message">The message to log. The current time will be prepended.</param>
        /// <param name="level">Optional. Default <see cref="LogLevel.Info" />.</param>
        /// <param name="color">Optional. Default is auotmatic.</param>
        public void Write(string message, LogLevel level = LogLevel.Info, ConsoleColor color = ConsoleColor.Black)
        {
            // Don't log above the set maximum log level
            if (level > _maxLogLevel)
                return;
            // Invoke on the UI thread, if we aren't already
            if (_richTextBox.InvokeRequired)
            {
                WriteCallback d = Write;
                _richTextBox.Invoke(d, message, level, color);
                return;
            }
            // Prepend the time, modify the message, and set the color
            string stamp = $"[{DateTime.Now.ToString("HH:mm:ss")}] ";
            switch (level)
            {
                case LogLevel.Error:     color = ConsoleColor.Red;         message = stamp + " (ERROR) " + message;      break;
                case LogLevel.Warning:   color = ConsoleColor.DarkYellow;  message = stamp + " (ATTENTION) " + message;  break;
                case LogLevel.Info:      color = ConsoleColor.DarkCyan;    message = stamp + " (INFO) " + message;       break;
                case LogLevel.Pokestop:  color = ConsoleColor.Cyan;        message = stamp + " (POKESTOP) " + message;   break;
                case LogLevel.Farming:   color = ConsoleColor.Magenta;     message = stamp + " (FARMING) " + message;    break;
                case LogLevel.Recycling: color = ConsoleColor.DarkMagenta; message = stamp + " (RECYCLING) " + message;  break;
                case LogLevel.Caught:    color = ConsoleColor.Green;       message = stamp + " (PKMN) " + message;       break;
                case LogLevel.Transfer:  color = ConsoleColor.DarkGreen;   message = stamp + " (TRANSFERED) " + message; break;
                case LogLevel.Evolve:    color = ConsoleColor.Yellow;      message = stamp + " (EVOLVED) " + message;    break;
                case LogLevel.Berry:     color = ConsoleColor.DarkYellow;  message = stamp + " (BERRY) " + message;      break;
                case LogLevel.Egg:       color = ConsoleColor.DarkYellow;  message = stamp + " (EGG) " + message;        break;
                case LogLevel.Debug:     color = ConsoleColor.Gray;        message = stamp + " (DEBUG) " + message;      break;
                default:                 color = ConsoleColor.White;       message = stamp + " (ERROR) " + message;      break;
            }
            // Save the cursor position
            int selStart = _richTextBox.SelectionStart;
            int selLength = _richTextBox.SelectionLength;
            int textLength = _richTextBox.TextLength;
            // Append the message
            _richTextBox.AppendText(message + Environment.NewLine);
            // Select and color the message
            _richTextBox.SelectionStart = textLength;
            _richTextBox.SelectionLength = _richTextBox.TextLength - textLength;
            _richTextBox.SelectionColor = color.ToColor();
            if (selStart == textLength)
            {
                // Move cursor to the end if it was already there
                _richTextBox.SelectionStart = _richTextBox.TextLength;
                _richTextBox.ScrollToCaret();
            }
            else
            {
                // Otherwise, restore the previous selection
                _richTextBox.SelectionStart = selStart;
                _richTextBox.SelectionLength = selLength;
            }
        }
    }

    static class ConsoleColorEx
    {
        public static Color ToColor(this ConsoleColor consoleColor)
        {
            switch (consoleColor)
            {
                case ConsoleColor.Black:       return Color.FromArgb(0x00, 0x00, 0x00);
                case ConsoleColor.DarkBlue:    return Color.FromArgb(0x00, 0x00, 0x80);
                case ConsoleColor.DarkGreen:   return Color.FromArgb(0x00, 0x80, 0x00);
                case ConsoleColor.DarkCyan:    return Color.FromArgb(0x00, 0x80, 0x80);
                case ConsoleColor.DarkRed:     return Color.FromArgb(0x80, 0x00, 0x00);
                case ConsoleColor.DarkMagenta: return Color.FromArgb(0x80, 0x00, 0x80);
                case ConsoleColor.DarkYellow:  return Color.FromArgb(0x80, 0x80, 0x00);
                case ConsoleColor.Gray:        return Color.FromArgb(0xC0, 0xC0, 0xC0);
                case ConsoleColor.DarkGray:    return Color.FromArgb(0x80, 0x80, 0x80);
                case ConsoleColor.Blue:        return Color.FromArgb(0x00, 0x00, 0xFF);
                case ConsoleColor.Green:       return Color.FromArgb(0x00, 0xFF, 0x00);
                case ConsoleColor.Cyan:        return Color.FromArgb(0x00, 0xFF, 0xFF);
                case ConsoleColor.Red:         return Color.FromArgb(0xFF, 0x00, 0x00);
                case ConsoleColor.Magenta:     return Color.FromArgb(0xFF, 0x00, 0xFF);
                case ConsoleColor.Yellow:      return Color.FromArgb(0xFF, 0xFF, 0x00);
                case ConsoleColor.White:       return Color.FromArgb(0xFF, 0xFF, 0xFF);
                default:                       throw new ArgumentOutOfRangeException();
            }
        }
    }
}
