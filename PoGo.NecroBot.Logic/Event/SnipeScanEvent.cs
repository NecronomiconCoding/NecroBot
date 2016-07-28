using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoGo.NecroBot.Logic.Event
{
    public class SnipeScanEvent : IEvent
    {
        public string Pokemon { get; set; }
        public Location Bounds { get; set; }
    }
}
