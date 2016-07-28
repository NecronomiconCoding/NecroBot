using System;
using System.Threading;
using System.Threading.Tasks;

namespace PoGo.NecroBot.Logic.Utils
{
    public static class DelayingUtils
    {
        private static readonly Random RandomDevice = new Random();

        public static async Task Delay(int delay, int defdelay)
        {
            if (delay > 0)
            {
                float randomFactor = 0.3f;
                int randomMin = (int)(delay * (1 - randomFactor));
                int randomMax = (int)(delay * (1 + randomFactor));
                int randomizedDelay = RandomDevice.Next(randomMin, randomMax);

                await Task.Delay(randomizedDelay);
            }
            else if (defdelay > 0)
            {
                await Task.Delay(defdelay);
            }

        }
    }
}
