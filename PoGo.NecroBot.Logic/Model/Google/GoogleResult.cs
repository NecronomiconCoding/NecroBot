using System;
using System.Collections.Generic;
using System.Linq;
using GeoCoordinatePortable;
using PoGo.NecroBot.Logic.Model.Google.GoogleObjects;

namespace PoGo.NecroBot.Logic.Model.Google
{
    public class GoogleResult
    {
        public DirectionsResponse Directions { get; set; }
        public DateTime RequestDate { get; set; }
        public GeoCoordinate Origin { get; set; }
        public GeoCoordinate Destiny { get; set; }
        public List<GeoCoordinate> Waypoints { get; set; }
        public bool FromCache { get; set; }

        /// <summary>
        /// Google time to reach destiny. If car, consider traffic data.
        /// </summary>
        /// <returns></returns>
        public float TravelTime()
        {
            float tempo = 0;

            foreach (var legs in Directions.routes.SelectMany(route => route.legs))
            {
                tempo += legs.duration.value;
            }
            return tempo;
        }


        public double GetDistance()
        {
            float distance = 0;

            foreach (var legs in Directions.routes.SelectMany(route => route.legs))
            {
                distance += legs.distance.value;
            }
            return distance;
        }
        
    }
}
