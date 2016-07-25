#region

using System;

#endregion

namespace PokemonGo.RocketAPI.Exceptions
{
    public class AccessTokenExpiredException : Exception
    {
        public AccessTokenExpiredException(string message)
            : base(message)
        {

        }
    }
}