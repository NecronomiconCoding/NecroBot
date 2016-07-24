using PokemonGo.RocketAPI.GeneratedCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGo.RocketAPI.Logic.Event
{
    public class PokemonEvolveEvent : IEvent
    {
        public PokemonId Id;
        public int Exp;
        public EvolvePokemonOut.Types.EvolvePokemonStatus Result;
    }
}
