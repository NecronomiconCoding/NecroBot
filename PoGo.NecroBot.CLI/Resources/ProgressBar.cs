#region using directives

using System;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace PoGo.NecroBot.CLI.Resources
{
    class ProgressBar
    {
        public static int total = 100;
        private static int leftOffset;
        private static CancellationTokenSource taskToken;
        private static Thread spinThread;

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
            try
            {
                // Window width has be be larger than what Console.CursorLeft is set to
                // or System.ArgumentOutOfRangeException is thrown.
                if (Console.WindowWidth < 50 + leftOffset)
                {
                    Console.WindowWidth = 51 + leftOffset;
                }

                Console.ForegroundColor = barColor;
                Console.CursorLeft = 0 + leftOffset;
                Console.Write("[");
                Console.CursorLeft = 47 + leftOffset;
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

                for (int i = pos; i <= (46 + leftOffset - 2); i++)
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
            catch (System.IO.IOException) { }
        }

        public static void SpinTurn(string startText)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(startText);

            int marginLeft = startText.Length + 1;

            taskToken = new CancellationTokenSource();
            Task.Factory.StartNew(() =>
            {
                spinThread = Thread.CurrentThread;

                Console.CursorLeft = marginLeft;

                int counter = 0;
                while (!taskToken.IsCancellationRequested)
                {
                    counter++;
                    switch (counter % 4)
                    {
                        case 0: Console.Write("/"); break;
                        case 1: Console.Write("-"); break;
                        case 2: Console.Write("\\"); break;
                        case 3: Console.Write("|"); break;
                    }

                    Console.SetCursorPosition(marginLeft, Console.CursorTop);
                }

                Console.Write(Environment.NewLine);
            }, taskToken.Token);
        }

        public static void SpinOff()
        {
            if (taskToken != null)
                taskToken.Cancel();
        }
    }
}
