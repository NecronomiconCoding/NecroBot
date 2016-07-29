using System;
using System.Threading;

namespace PoGo.NecroBot.Logic.Utils
{
    public static class DelayingUtils
    {
        private static readonly Random RandomDevice = new Random();

        public static void Delay(int delay, int defdelay)
        {
            if (delay > defdelay)
            {
                float randomFactor = 0.3f;
                int randomMin = (int)(delay * (1 - randomFactor));
                int randomMax = (int)(delay * (1 + randomFactor));
                int randomizedDelay = RandomDevice.Next(randomMin, randomMax);

                Thread.Sleep(randomizedDelay);
            }
            else if (defdelay > 0)
            {
                Thread.Sleep(defdelay);
            }

        }
    }
}
