#region

using System;

#endregion

namespace PokemonGo.RocketAPI.Extensions
{
    public static class DateTimeExtensions
    {
        public static long ToUnixTime(this DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date - epoch).TotalMilliseconds);
        }
    }
}