using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using GeoCoordinatePortable;
using PoGo.NecroBot.Logic.Model.Google.GoogleObjects;

namespace PoGo.NecroBot.Logic.Model.Google
{
    public class GoogleWalk
    {
        public List<GeoCoordinate> Waypoints { get; set; }

        public GoogleWalk(GoogleResult googleResult)
        {
            if (googleResult.Directions.routes == null)
                throw new ArgumentException("Invalid google route.");

            var route = googleResult.Directions.routes.First();

            Waypoints = new List<GeoCoordinate>();
            // In some cases, player are inside build
            Waypoints.Add(googleResult.Origin);

            Waypoints.Add(new GeoCoordinate(route.legs.First().start_location.lat, route.legs.First().start_location.lng));
            Waypoints.AddRange(route.overview_polyline.DecodePoly());
            Waypoints.Add(new GeoCoordinate(route.legs.Last().end_location.lat, route.legs.Last().end_location.lng));

            // In some cases, player need to get inside a  build
            Waypoints.Add(googleResult.Destiny);
        }

        /// <summary>
        /// Used for test purpose
        /// </summary>
        /// <returns></returns>
        public string GetTextFlyPath() => "[" + string.Join(",", Waypoints.Select(geoCoordinate => $"{{lat: {geoCoordinate.Latitude.ToString(new CultureInfo("en-US"))}, lng: {geoCoordinate.Longitude.ToString(new CultureInfo("en-US"))}}}").ToList()) + "]";

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
            return new GoogleWalk(googleResult);
        }
    }
}
