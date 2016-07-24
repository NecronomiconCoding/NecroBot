using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGo.RocketAPI.Logic.Event
{
    public class FortUsedEvent : IEvent
    {
        public int Exp;
        public int Gems;
        public string Items;
    }
}
