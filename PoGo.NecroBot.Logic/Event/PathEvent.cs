using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoCoordinatePortable;

namespace PoGo.NecroBot.Logic.Event
{
    public class PathEvent : IEvent
    {
        /// <summary>
        /// List of path coordinates (waypoints) as a string
        /// </summary>
        public string StringifiedPath;
        /// <summary>
        /// Is the path calculated (true) or is it the real walked path (false)
        /// </summary>
        public bool IsCalculated;
    }
}
