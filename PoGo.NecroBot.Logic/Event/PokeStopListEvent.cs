using POGOProtos.Map.Fort;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoGo.NecroBot.Logic.Event
{
    class PokeStopListEvent : IEvent
    {
        public List<FortData> Forts;
    }
}
