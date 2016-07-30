#region using directives

using System;
using System.Threading;

#endregion

namespace PoGo.NecroBot.Logic.Utils
{
    public static class DelayingUtils
    {
        private static readonly Random RandomDevice = new Random();

        public static void Delay(int delay, int defdelay)
        {
            if (delay > defdelay)
            {
                var randomFactor = 0.3f;
                var randomMin = (int) (delay*(1 - randomFactor));
                var randomMax = (int) (delay*(1 + randomFactor));
                var randomizedDelay = RandomDevice.Next(randomMin, randomMax);

                Thread.Sleep(randomizedDelay);
            }
            else if (defdelay > 0)
            {
                Thread.Sleep(defdelay);
            }
        }
    }
}