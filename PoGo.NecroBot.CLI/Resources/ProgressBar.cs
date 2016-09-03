#region using directives

using System;
using System.IO;

#endregion

namespace PoGo.NecroBot.CLI.Resources
{
    internal class ProgressBar
    {
        public static int Total = 100;
        private static int _leftOffset;

        public static void Start(string startText, int startAmt)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(startText);

            _leftOffset = startText.Length + 1;
            Fill(startAmt);
        }

        public static void Fill(int amt, ConsoleColor barColor = ConsoleColor.Red)
        {
            try
            {
                // Window width has be be larger than what Console.CursorLeft is set to
                // or System.ArgumentOutOfRangeException is thrown.
                if (Console.WindowWidth < 50 + _leftOffset)
                {
                    Console.WindowWidth = 51 + _leftOffset;
                }

                Console.ForegroundColor = barColor;
                Console.CursorLeft = 0 + _leftOffset;
                Console.Write(@"[");
                Console.CursorLeft = 47 + _leftOffset;
                Console.Write(@"]");
                Console.CursorLeft = 1 + _leftOffset;
                var segment = 45.5f/Total;

                var pos = 1 + _leftOffset;
                for (var i = 0; i < segment*amt; i++)
                {
                    Console.BackgroundColor = barColor;
                    Console.CursorLeft = pos++;
                    Console.Write(@" ");
                }

                for (var i = pos; i <= 46 + _leftOffset - 2; i++)
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.CursorLeft = pos++;
                    Console.Write(@" ");
                }

                Console.CursorLeft = 50 + _leftOffset;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(amt + @"%");

                if (amt == Total)
                    Console.Write(Environment.NewLine);
            }
            catch (IOException)
            {
            }
        }
    }
}