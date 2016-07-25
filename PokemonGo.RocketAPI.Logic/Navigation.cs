#region using directives

using System;
using System.Device.Location;
using System.Threading.Tasks;
using PokemonGo.RocketAPI.Logic.Utils;
using PokemonGoDesktop.API.Proto;
// ReSharper disable RedundantAssignment

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

        public async Task<PlayerUpdateResponse> HumanLikeWalking(GeoCoordinate targetLocation,
            double walkingSpeedInKilometersPerHour, Func<Task> functionExecutedWhileWalking)
        {
            var speedInMetersPerSecond = walkingSpeedInKilometersPerHour/3.6;

            var sourceLocation = new GeoCoordinate(_client.CurrentLat, _client.CurrentLng);
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

            do
            {
                var millisecondsUntilGetUpdatePlayerLocationResponse =
                    (DateTime.Now - requestSendDateTime).TotalMilliseconds;

                sourceLocation = new GeoCoordinate(_client.CurrentLat, _client.CurrentLng);
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
                if (functionExecutedWhileWalking != null)
                    await functionExecutedWhileWalking(); // look for pokemon
                await Task.Delay(Math.Min((int) (distanceToTarget/speedInMetersPerSecond*1000), 3000));
            } while (LocationUtils.CalculateDistanceInMeters(sourceLocation, targetLocation) >= 30);

            return result;
        }

        public async Task<PlayerUpdateResponse> HumanPathWalking(GpxReader.Trkpt trk,
            double walkingSpeedInKilometersPerHour, Func<Task> functionExecutedWhileWalking)
        {
            //PlayerUpdateResponse result = null;

            var targetLocation = new GeoCoordinate(Convert.ToDouble(trk.Lat), Convert.ToDouble(trk.Lon));

            var speedInMetersPerSecond = walkingSpeedInKilometersPerHour/3.6;

            var sourceLocation = new GeoCoordinate(_client.CurrentLat, _client.CurrentLng);
            var distanceToTarget = LocationUtils.CalculateDistanceInMeters(sourceLocation, targetLocation);
            // Logger.Write($"Distance to target location: {distanceToTarget:0.##} meters. Will take {distanceToTarget/speedInMetersPerSecond:0.##} seconds!", LogLevel.Info);

            var nextWaypointBearing = LocationUtils.DegreeBearing(sourceLocation, targetLocation);
            var nextWaypointDistance = speedInMetersPerSecond;
            var waypoint = LocationUtils.CreateWaypoint(sourceLocation, nextWaypointDistance, nextWaypointBearing,
                Convert.ToDouble(trk.Ele));

            //Initial walking

            var requestSendDateTime = DateTime.Now;
            var result =
                await
                    _client.UpdatePlayerLocation(waypoint.Latitude, waypoint.Longitude, waypoint.Altitude);

            do
            {
                var millisecondsUntilGetUpdatePlayerLocationResponse =
                    (DateTime.Now - requestSendDateTime).TotalMilliseconds;

                sourceLocation = new GeoCoordinate(_client.CurrentLat, _client.CurrentLng);
                var currentDistanceToTarget = LocationUtils.CalculateDistanceInMeters(sourceLocation, targetLocation);

                //if (currentDistanceToTarget < 40)
                //{
                //    if (speedInMetersPerSecond > SpeedDownTo)
                //    {
                //        //Logger.Write("We are within 40 meters of the target. Speeding down to 10 km/h to not pass the target.", LogLevel.Info);
                //        speedInMetersPerSecond = SpeedDownTo;
                //    }
                //}

                nextWaypointDistance = Math.Min(currentDistanceToTarget,
                    millisecondsUntilGetUpdatePlayerLocationResponse/1000*speedInMetersPerSecond);
                nextWaypointBearing = LocationUtils.DegreeBearing(sourceLocation, targetLocation);
                waypoint = LocationUtils.CreateWaypoint(sourceLocation, nextWaypointDistance, nextWaypointBearing);

                requestSendDateTime = DateTime.Now;
                result =
                    await
                        _client.UpdatePlayerLocation(waypoint.Latitude, waypoint.Longitude,
                            waypoint.Altitude);
                if (functionExecutedWhileWalking != null)
                    await functionExecutedWhileWalking(); // look for pokemon
                await Task.Delay(Math.Min((int) (distanceToTarget/speedInMetersPerSecond*1000), 3000));
            } while (LocationUtils.CalculateDistanceInMeters(sourceLocation, targetLocation) >= 30);

            return result;
        }
    }
}