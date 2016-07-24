using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGo.RocketAPI.Logic.Event
{
    public class FortTargetEvent : IEvent
    {
        public string Name;
        public double Distance;
    }
}
