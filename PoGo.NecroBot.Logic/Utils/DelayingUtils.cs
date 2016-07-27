using System;
using System.Threading;
using PoGo.NecroBot.Logic.Logging;

namespace PoGo.NecroBot.Logic.Utils
{
    public static class DelayingUtils
    {
        public static void Delay(int delay, int defdelay)
        {
            if (delay > 0)
            {
                Logger.Write($"Waiting (inside of DelayingUtils) for {delay / 1000} seconds using global delay");
                Thread.Sleep(delay);
            }
            else if (defdelay > 0)
            {
                Logger.Write($"Waiting (inside of DelayingUtils) for {defdelay / 1000} seconds using default delay");
                Thread.Sleep(defdelay);
            }
        }
    }
}