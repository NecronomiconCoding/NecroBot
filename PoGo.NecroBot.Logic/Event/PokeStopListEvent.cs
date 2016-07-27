#region using directives

using System.Collections.Generic;
using POGOProtos.Map.Fort;

#endregion

namespace PoGo.NecroBot.Logic.Event
{
    public class PokeStopListEvent : IEvent
    {
        public List<FortData> Forts;
    }
}