using POGOProtos.Map.Fort;
using System.Collections.Generic;

namespace PoGo.NecroBot.Logic.Event
{
    public class PokeStopListEvent : IEvent
    {
        public List<FortData> Forts;
    }
}
