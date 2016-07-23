#region Usings

using System;

#endregion

namespace PokemonGo.RocketAPI.Helpers
{
    public class RandomHelper
    {
        private static readonly Random Random = new Random();

        public static long GetLongRandom(long min, long max)
        {
            var buf = new byte[8];
            Random.NextBytes(buf);
            var longRand = BitConverter.ToInt64(buf, 0);

            return Math.Abs(longRand%(max - min)) + min;
        }
    }
}