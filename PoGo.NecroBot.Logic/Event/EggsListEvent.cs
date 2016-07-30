#region using directives

using System.Collections.Generic;
using POGOProtos.Inventory;

#endregion

namespace PoGo.NecroBot.Logic.Event
{
    public class EggsListEvent : IEvent
    {
        public List<EggIncubator> Incubators { get; set; }
        public object UnusedEggs { get; set; }
    }
}