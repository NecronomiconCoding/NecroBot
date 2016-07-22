using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PokemonGo.RocketAPI.Logic.Navigation;

namespace PokemonGo.RocketAPI.Logic.Utils
{
    public static class LocationUtils
    {
        public static Location CreateWaypoint(Location sourceLocation, double distanceInMeters, double bearingDegrees) //from http://stackoverflow.com/a/17545955
        {
            double distanceKm = distanceInMeters / 1000.0;
            double distanceRadians = distanceKm / 6371; //6371 = Earth's radius in km

            double bearingRadians = ToRad(bearingDegrees);
            double sourceLatitudeRadians = ToRad(sourceLocation.Latitude);
            double sourceLongitudeRadians = ToRad(sourceLocation.Longitude);

            double targetLatitudeRadians = Math.Asin(Math.Sin(sourceLatitudeRadians) * Math.Cos(distanceRadians)
                    + Math.Cos(sourceLatitudeRadians) * Math.Sin(distanceRadians) * Math.Cos(bearingRadians));

            double targetLongitudeRadians = sourceLongitudeRadians + Math.Atan2(Math.Sin(bearingRadians)
                    * Math.Sin(distanceRadians) * Math.Cos(sourceLatitudeRadians), Math.Cos(distanceRadians)
                    - Math.Sin(sourceLatitudeRadians) * Math.Sin(targetLatitudeRadians));

            // adjust toLonRadians to be in the range -180 to +180...
            targetLongitudeRadians = ((targetLongitudeRadians + 3 * Math.PI) % (2 * Math.PI)) - Math.PI;

            return new Location(ToDegrees(targetLatitudeRadians), ToDegrees(targetLongitudeRadians));
        }

        public static double CalculateDistanceInMeters(Location sourceLocation, Location targetLocation) // from http://stackoverflow.com/questions/6366408/calculating-distance-between-two-latitude-and-longitude-geocoordinates
        {
            var baseRad = Math.PI * sourceLocation.Latitude / 180;
            var targetRad = Math.PI * targetLocation.Latitude / 180;
            var theta = sourceLocation.Longitude - targetLocation.Longitude;
            var thetaRad = Math.PI * theta / 180;

            double dist =
                Math.Sin(baseRad) * Math.Sin(targetRad) + Math.Cos(baseRad) *
                Math.Cos(targetRad) * Math.Cos(thetaRad);
            dist = Math.Acos(dist);

            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515 * 1.609344 * 1000;

            return dist;
        }

        public static double DegreeBearing(Location sourceLocation, Location targetLocation) // from http://stackoverflow.com/questions/2042599/direction-between-2-latitude-longitude-points-in-c-sharp
        {
            var dLon = ToRad(targetLocation.Longitude - sourceLocation.Longitude);
            var dPhi = Math.Log(
                Math.Tan(ToRad(targetLocation.Latitude) / 2 + Math.PI / 4) / Math.Tan(ToRad(sourceLocation.Latitude) / 2 + Math.PI / 4));
            if (Math.Abs(dLon) > Math.PI)
                dLon = dLon > 0 ? -(2 * Math.PI - dLon) : (2 * Math.PI + dLon);
            return ToBearing(Math.Atan2(dLon, dPhi));
        }

        public static double ToRad(double degrees)
        {
            return degrees * (Math.PI / 180);
        }

        public static double ToDegrees(double radians)
        {
            return radians * 180 / Math.PI;
        }

        public static double ToBearing(double radians)
        {
            // convert radians to degrees (as bearing: 0...360)
            return (ToDegrees(radians) + 360) % 360;
        }
    }
}
