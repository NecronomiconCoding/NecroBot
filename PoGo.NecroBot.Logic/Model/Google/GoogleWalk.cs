using System;
using System.Collections.Generic;
using System.Linq;
using GeoCoordinatePortable;
using PoGo.NecroBot.Logic.Model.Google.GoogleObjects;

namespace PoGo.NecroBot.Logic.Model.Google
{
    public class GoogleWalk
    {
        public List<GeoCoordinate> Waypoints { get; set; }

        public GoogleWalk(Route route)
        {
            if (route == null)
                throw new ArgumentException("Invalid google route.");

            Waypoints = new List<GeoCoordinate>() ;
            Waypoints.Add(new GeoCoordinate(route.legs.First().start_location.lat, route.legs.First().start_location.lng));
            Waypoints.AddRange(route.overview_polyline.DecodePoly());
            
            Waypoints.Add(new GeoCoordinate(route.legs.Last().end_location.lat, route.legs.Last().end_location.lng));

            var draw = GetTextFlyPath(Waypoints);
        }

        /// <summary>
        /// Used for test purpose
        /// </summary>
        /// <param name="poly"></param>
        /// <returns></returns>
        private string GetTextFlyPath(IEnumerable<GeoCoordinate> poly) => poly.Aggregate(string.Empty, (current, geoCoordinate) => current + $"{{lat: {geoCoordinate.Latitude}, lng: {geoCoordinate.Longitude}}},{Environment.NewLine}");

        private GeoCoordinate _lastNextStep;
        public GeoCoordinate NextStep(GeoCoordinate actualLocation)
        {
            if (!Waypoints.Any())
            {
                return _lastNextStep ?? (_lastNextStep = actualLocation);
            }

            do
            {
                _lastNextStep = Waypoints.FirstOrDefault();
                Waypoints.Remove(_lastNextStep);

            } while (actualLocation.GetDistanceTo(_lastNextStep) < 20 ||
                    Waypoints.Any());

            return _lastNextStep;
        }

        public static GoogleWalk Get(GoogleResult googleResult)
        {
            return new GoogleWalk(googleResult.Directions.routes.FirstOrDefault());
        }
    }
}
