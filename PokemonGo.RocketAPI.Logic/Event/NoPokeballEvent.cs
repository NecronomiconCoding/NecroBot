using PokemonGo.RocketAPI.GeneratedCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGo.RocketAPI.Logic.Event
{
    public class NoPokeballEvent : IEvent
    {
        public PokemonId Id;
        public int Cp;
    }
}
