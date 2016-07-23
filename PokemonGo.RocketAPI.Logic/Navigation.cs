#region

using System;
using System.Threading.Tasks;
using PokemonGo.RocketAPI.GeneratedCode;
using PokemonGo.RocketAPI.Logic.Utils;

#endregion

namespace PokemonGo.RocketAPI.Logic
{
    public class Navigation
    {
        private const double SpeedDownTo = 10/3.6;
        private readonly Client _client;

        public Navigation(Client client)
        {
            _client = client;
        }

        public static double DistanceBetween2Coordinates(double lat1, double lng1, double lat2, double lng2)
        {
            const double rEarth = 6378137;
            var dLat = (lat2 - lat1)*Math.PI/180;
            var dLon = (lng2 - lng1)*Math.PI/180;
            var alpha = Math.Sin(dLat/2)*Math.Sin(dLat/2)
                        + Math.Cos(lat1*Math.PI/180)*Math.Cos(lat2*Math.PI/180)
                        *Math.Sin(dLon/2)*Math.Sin(dLon/2);
            var d = 2*rEarth*Math.Atan2(Math.Sqrt(alpha), Math.Sqrt(1 - alpha));
            return d;
        }

        public async Task<PlayerUpdateResponse> HumanLikeWalking(Location targetLocation,
            double walkingSpeedInKilometersPerHour, Func<Task> functionExecutedWhileWalking)
        {
            var speedInMetersPerSecond = walkingSpeedInKilometersPerHour/3.6;

            var sourceLocation = new Location(_client.CurrentLat, _client.CurrentLng);
            var distanceToTarget = LocationUtils.CalculateDistanceInMeters(sourceLocation, targetLocation);
            // Logger.Write($"Distance to target location: {distanceToTarget:0.##} meters. Will take {distanceToTarget/speedInMetersPerSecond:0.##} seconds!", LogLevel.Info);

            var nextWaypointBearing = LocationUtils.DegreeBearing(sourceLocation, targetLocation);
            var nextWaypointDistance = speedInMetersPerSecond;
            var waypoint = LocationUtils.CreateWaypoint(sourceLocation, nextWaypointDistance, nextWaypointBearing);

            //Initial walking
            var requestSendDateTime = DateTime.Now;
            var result =
                await
                    _client.UpdatePlayerLocation(waypoint.Latitude, waypoint.Longitude, _client.Settings.DefaultAltitude);

            if (functionExecutedWhileWalking != null)
                await functionExecutedWhileWalking();
            do
            {
                var millisecondsUntilGetUpdatePlayerLocationResponse =
                    (DateTime.Now - requestSendDateTime).TotalMilliseconds;

                sourceLocation = new Location(_client.CurrentLat, _client.CurrentLng);
                var currentDistanceToTarget = LocationUtils.CalculateDistanceInMeters(sourceLocation, targetLocation);

                if (currentDistanceToTarget < 40)
                {
                    if (speedInMetersPerSecond > SpeedDownTo)
                    {
                        //Logger.Write("We are within 40 meters of the target. Speeding down to 10 km/h to not pass the target.", LogLevel.Info);
                        speedInMetersPerSecond = SpeedDownTo;
                    }
                }

                nextWaypointDistance = Math.Min(currentDistanceToTarget,
                    millisecondsUntilGetUpdatePlayerLocationResponse/1000*speedInMetersPerSecond);
                nextWaypointBearing = LocationUtils.DegreeBearing(sourceLocation, targetLocation);
                waypoint = LocationUtils.CreateWaypoint(sourceLocation, nextWaypointDistance, nextWaypointBearing);

                requestSendDateTime = DateTime.Now;
                result =
                    await
                        _client.UpdatePlayerLocation(waypoint.Latitude, waypoint.Longitude,
                            _client.Settings.DefaultAltitude);
                await Task.Delay(Math.Min((int) (distanceToTarget/speedInMetersPerSecond*1000), 3000));
            } while (LocationUtils.CalculateDistanceInMeters(sourceLocation, targetLocation) >= 30);

            return result;
        }

        public class Location
        {
            public Location(double latitude, double longitude)
            {
                Latitude = latitude;
                Longitude = longitude;
            }

            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }
    }
}