using System.Collections.Generic;
using GeoCoordinatePortable;

namespace PoGo.NecroBot.Logic.Model.Google.GoogleObjects
{
    public class DirectionsResponse
    {
        public GeocodedWaypoints[] geocoded_waypoints { get; set; }
        public Route[] routes { get; set; }
        public string status { get; set; }
        
    }
}