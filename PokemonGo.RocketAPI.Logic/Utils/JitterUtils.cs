using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGo.RocketAPI.Logic.Utils
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
