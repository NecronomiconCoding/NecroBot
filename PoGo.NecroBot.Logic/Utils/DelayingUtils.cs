using System;
using System.Threading;
using PoGo.NecroBot.Logic.Logging;

namespace PoGo.NecroBot.Logic.Utils
{
    public static class DelayingUtils
    {
        private static readonly Random RandomDevice = new Random();

        public static void Delay(int delay, int defdelay)
        {
            if (delay > 0)
            {
                float randomFactor = 0.3f;
                int randomMin = (int) delay * (1 - randomFactor);
                int randomMax = (int) delay * (1 + randomFactor);
                int randomizedDelay = RandomDevice.Next(randomMin, randomMax);
               
                Logger.Write($"Waiting (inside of DelayingUtils) for {randomizedDelay / 1000:0.#} seconds using global delay");
                Thread.Sleep(randomizedDelay);
            }
            else if (defdelay > 0)
            {
                Logger.Write($"Waiting (inside of DelayingUtils) for {defdelay / 1000:0.#} seconds using default delay");
                Thread.Sleep(defdelay);
            }
        }
    }
}