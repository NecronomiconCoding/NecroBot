#region using directives

using System;

#endregion

namespace PoGo.NecroBot.CLI.Resources
{
    class ProgressBar
    {
        public static int total = 100;
        private static int leftOffset;

        public static void start(string startText, int startAmt)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(startText);

            leftOffset = startText.Length+1;
            fill(startAmt);
        }

        public static void fill(int amt, ConsoleColor barColor = ConsoleColor.Red)
        {
            Console.ForegroundColor = barColor;
            Console.CursorLeft = 0+leftOffset;
            Console.Write("[");
            Console.CursorLeft = 47+leftOffset;
            Console.Write("]");
            Console.CursorLeft = 1 + leftOffset;
            float segment = 45.5f / total;

            int pos = 1 + leftOffset;
            for (int i = 0; i < segment * amt; i++)
            {
                Console.BackgroundColor = barColor;
                Console.CursorLeft = pos++;
                Console.Write(" ");
            }
            
            for (int i = pos; i <= (46+leftOffset-2); i++)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.CursorLeft = pos++;
                Console.Write(" ");
            }

            Console.CursorLeft = 50 + leftOffset;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(amt.ToString() + "%");

            if (amt == total)
                Console.Write(Environment.NewLine);
        }
    }
}
