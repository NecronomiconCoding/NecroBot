using System;
using System.Threading.Tasks;

namespace PoGo.NecroBot.Logic.Utils
{
    public static class DelayingUtils
    {
        private static readonly Random RandomDevice = new Random();

        public static async void Delay(int delay, int defdelay)
        {
            if (delay > 0)
            {
                float randomFactor = 0.3f;
                int randomMin = delay * (1 - randomFactor);
                int randomMax = delay * (1 + randomFactor);
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
