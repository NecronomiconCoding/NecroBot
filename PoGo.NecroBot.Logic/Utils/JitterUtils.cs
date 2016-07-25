using System;
using System.Threading.Tasks;

namespace PoGo.NecroBot.Logic.Utils
{
    public static class JitterUtils
    {
        private static Random _randomDevice = new Random();

        public static Task RandomDelay(int min, int max)
        {
            return Task.Delay(_randomDevice.Next(min, max));
        }
    }
}
