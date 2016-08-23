using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using GeoCoordinatePortable;
using PoGo.NecroBot.Logic.Model.Google.GoogleObjects;
using Newtonsoft.Json;

namespace PoGo.NecroBot.Logic.Model.Yours
{
    public class RoutingResponse
    {
        public string type { get; set; }
        public Crs crs { get; set; }
        public List<List<double>> coordinates { get; set; }
        public Properties2 properties { get; set; }
    }

    public class Properties
    {
        public string name { get; set; }
    }

    public class Crs
    {
        public string type { get; set; }
        public Properties properties { get; set; }
    }

    public class Properties2
    {
        public string distance { get; set; }
        public string description { get; set; }
        public string traveltime { get; set; }
    }

    public class YoursWalk
    {
        public List<GeoCoordinate> Waypoints { get; set; }
        public double Distance { get; set; }

        public YoursWalk(string yoursResponse)
        {
            RoutingResponse yoursResponseParsed = JsonConvert.DeserializeObject<RoutingResponse>(yoursResponse);

            Distance = double.Parse(yoursResponseParsed.properties.distance) * 1000;

            Waypoints = new List<GeoCoordinate>();
            foreach (List<double> coordinate in yoursResponseParsed.coordinates)
            {
                Waypoints.Add(new GeoCoordinate(coordinate.ToArray()[1], coordinate.ToArray()[0]));
            }
        }
        
        public static YoursWalk Get(string yoursResponse)
        {
            return new YoursWalk(yoursResponse);
        }
    }
}
