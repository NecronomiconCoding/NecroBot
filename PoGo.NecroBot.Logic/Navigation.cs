#region using directives

#region using directives

using System;
using System.Globalization;
using System.Threading.Tasks;
using GeoCoordinatePortable;
using PoGo.NecroBot.Logic.Utils;
using PokemonGo.RocketAPI;
using POGOProtos.Networking.Responses;

#endregion

// ReSharper disable RedundantAssignment

#endregion

namespace PoGo.NecroBot.Logic
{
    public delegate void UpdatePositionDelegate(double lat, double lng);

    public class Navigation
    {
        private const double SpeedDownTo = 10/3.6;
        private readonly Client _client;
        private DateTime _lastRest = DateTime.Now; 
        private DateTime _lastShortStop = DateTime.Now; 

        public Navigation(Client client)
        {
            _client = client;
        }

        private double GetSpeed(double walkingSpeedInKilometersPerHour)
        {
            var speed = walkingSpeedInKilometersPerHour / 3.6d * 100.0d / Randomizer.GetNext(95, 105);
            if (_lastRest.AddMinutes(15) < DateTime.Now) speed = speed * 0.95;
            if (_lastRest.AddMinutes(30) < DateTime.Now) speed = speed * 0.90;
            if (_lastRest.AddMinutes(45) < DateTime.Now) speed = speed * 0.85;
            if (Randomizer.GetNext(1, 100) < 5) speed = speed * 0.1;
            return speed;
        }

        private async Task RestIfNecessary()
        {
            var limit = Randomizer.GetNamed($"{nameof(RestIfNecessary)}", 15 * 60, 45 * 60, 30 * 60);
            if (_lastRest.AddSeconds(limit) < DateTime.Now)
            {
                var amount = Randomizer.GetNext(60, 240);
                // todo: add logging
                // Console.WriteLine("resting " + amount + " seconds");
                await Randomizer.Sleep(amount * 1000);
                _lastRest = DateTime.Now;
            } 
        }

        private async Task ShortStopIfNecessary()
        {
            var limit = Randomizer.GetNamed($"{nameof(ShortStopIfNecessary)}", 2 * 60, 5 * 60, 3 * 60);
            if (_lastShortStop.AddSeconds(limit) < DateTime.Now)
            {
                var amount = Randomizer.GetNext(5, 15);
                // todo: add logging
                // Console.WriteLine("short stop for " + amount + " seconds");
                await Randomizer.Sleep(amount * 1000);
                _lastShortStop = DateTime.Now;
            } 
        }

        public async Task<PlayerUpdateResponse> HumanLikeWalking(GeoCoordinate targetLocation,
            double walkingSpeedInKilometersPerHour, Func<Task<bool>> functionExecutedWhileWalking)
        {
            walkingSpeedInKilometersPerHour = walkingSpeedInKilometersPerHour * 100d / Randomizer.GetNext(80, 120);

            await RestIfNecessary();

            var speedInMetersPerSecond = GetSpeed(walkingSpeedInKilometersPerHour);

            var sourceLocation = new GeoCoordinate(_client.CurrentLatitude, _client.CurrentLongitude);
            var distanceToTarget = LocationUtils.CalculateDistanceInMeters(sourceLocation, targetLocation);
            // Logger.Write($"Distance to target location: {distanceToTarget:0.##} meters. Will take {distanceToTarget/speedInMetersPerSecond:0.##} seconds!", LogLevel.Info);

            var nextWaypointBearing = LocationUtils.DegreeBearing(sourceLocation, targetLocation);
            var nextWaypointDistance = speedInMetersPerSecond;
            var waypoint = LocationUtils.CreateWaypoint(sourceLocation, nextWaypointDistance, nextWaypointBearing);

            //Initial walking
            var requestSendDateTime = DateTime.Now;
            var result =
                await
                    _client.Player.UpdatePlayerLocation(waypoint.Latitude, waypoint.Longitude,
                        _client.Settings.DefaultAltitude);

            UpdatePositionEvent?.Invoke(waypoint.Latitude, waypoint.Longitude);

            do
            {
                var millisecondsUntilGetUpdatePlayerLocationResponse =
                    (DateTime.Now - requestSendDateTime).TotalMilliseconds;

                sourceLocation = new GeoCoordinate(_client.CurrentLatitude, _client.CurrentLongitude);
                var currentDistanceToTarget = LocationUtils.CalculateDistanceInMeters(sourceLocation, targetLocation);

                if (currentDistanceToTarget < 40)
                {
                    if (speedInMetersPerSecond > SpeedDownTo)
                    {
                        //Logger.Write("We are within 40 meters of the target. Speeding down to 10 km/h to not pass the target.", LogLevel.Info);
                        speedInMetersPerSecond = SpeedDownTo;
                    }
                }
                else speedInMetersPerSecond = GetSpeed(walkingSpeedInKilometersPerHour);

                await ShortStopIfNecessary();

                nextWaypointDistance = Math.Min(currentDistanceToTarget,
                    millisecondsUntilGetUpdatePlayerLocationResponse/1000*speedInMetersPerSecond);
                nextWaypointBearing = LocationUtils.DegreeBearing(sourceLocation, targetLocation);
                waypoint = LocationUtils.CreateWaypoint(sourceLocation, nextWaypointDistance, nextWaypointBearing);

                var randomBearing = 0.0d;
                if (currentDistanceToTarget > 50)
                {
                    var n = Convert.ToInt32(currentDistanceToTarget);
                    randomBearing = (double)Randomizer.GetNext(-n, n) / 20.0d;
                    nextWaypointBearing += randomBearing;
                }

                // todo: add logging
                // Console.WriteLine("walking to target with speed: " + speedInMetersPerSecond.ToString("F2", CultureInfo.InvariantCulture) + "m/sec, distance: " + currentDistanceToTarget.ToString("F2", CultureInfo.InvariantCulture) + "m, bearing: " + nextWaypointBearing.ToString("F2", CultureInfo.InvariantCulture) + ", rndBearing: " + randomBearing.ToString("F2", CultureInfo.InvariantCulture));

                requestSendDateTime = DateTime.Now;
                result =
                    await
                        _client.Player.UpdatePlayerLocation(waypoint.Latitude, waypoint.Longitude,
                            _client.Settings.DefaultAltitude);

                UpdatePositionEvent?.Invoke(waypoint.Latitude, waypoint.Longitude);

                if (functionExecutedWhileWalking != null)
                    await functionExecutedWhileWalking(); // look for pokemon

                await Randomizer.Sleep(Math.Min((int)(distanceToTarget / speedInMetersPerSecond * 1000), 3400), 0.1);
                //await Randomizer.Sleep(500); // this is wrong because we should not send too many position updates to the server

            } while (LocationUtils.CalculateDistanceInMeters(sourceLocation, targetLocation) >= 30);

            return result;
        }

        public async Task<PlayerUpdateResponse> HumanPathWalking(GpxReader.Trkpt trk,
            double walkingSpeedInKilometersPerHour, Func<Task<bool>> functionExecutedWhileWalking)
        {
            walkingSpeedInKilometersPerHour = walkingSpeedInKilometersPerHour * 100d / Randomizer.GetNext(80, 120);
            //PlayerUpdateResponse result = null;

            var targetLocation = new GeoCoordinate(Convert.ToDouble(trk.Lat, CultureInfo.InvariantCulture),
                Convert.ToDouble(trk.Lon, CultureInfo.InvariantCulture));

            var speedInMetersPerSecond = GetSpeed(walkingSpeedInKilometersPerHour);

            var sourceLocation = new GeoCoordinate(_client.CurrentLatitude, _client.CurrentLongitude);
            var distanceToTarget = LocationUtils.CalculateDistanceInMeters(sourceLocation, targetLocation);
            // Logger.Write($"Distance to target location: {distanceToTarget:0.##} meters. Will take {distanceToTarget/speedInMetersPerSecond:0.##} seconds!", LogLevel.Info);

            var nextWaypointBearing = LocationUtils.DegreeBearing(sourceLocation, targetLocation);
            var nextWaypointDistance = speedInMetersPerSecond;
            var waypoint = LocationUtils.CreateWaypoint(sourceLocation, nextWaypointDistance, nextWaypointBearing,
                Convert.ToDouble(trk.Ele, CultureInfo.InvariantCulture));

            //Initial walking

            var requestSendDateTime = DateTime.Now;
            var result =
                await
                    _client.Player.UpdatePlayerLocation(waypoint.Latitude, waypoint.Longitude, waypoint.Altitude);

            UpdatePositionEvent?.Invoke(waypoint.Latitude, waypoint.Longitude);

            do
            {
                var millisecondsUntilGetUpdatePlayerLocationResponse =
                    (DateTime.Now - requestSendDateTime).TotalMilliseconds;

                sourceLocation = new GeoCoordinate(_client.CurrentLatitude, _client.CurrentLongitude);
                var currentDistanceToTarget = LocationUtils.CalculateDistanceInMeters(sourceLocation, targetLocation);

                //if (currentDistanceToTarget < 40)
                //{
                //    if (speedInMetersPerSecond > SpeedDownTo)
                //    {
                //        //Logger.Write("We are within 40 meters of the target. Speeding down to 10 km/h to not pass the target.", LogLevel.Info);
                //        speedInMetersPerSecond = SpeedDownTo;
                //    }
                //}
                speedInMetersPerSecond = GetSpeed(walkingSpeedInKilometersPerHour);

                // todo: add logging
                // Console.WriteLine("walking to target with speed: " + speedInMetersPerSecond.ToString("F1", CultureInfo.InvariantCulture) + "m/sec, distance: " + currentDistanceToTarget.ToString("F1", CultureInfo.InvariantCulture) + "m");

                nextWaypointDistance = Math.Min(currentDistanceToTarget,
                    millisecondsUntilGetUpdatePlayerLocationResponse/1000*speedInMetersPerSecond);
                nextWaypointBearing = LocationUtils.DegreeBearing(sourceLocation, targetLocation);
                waypoint = LocationUtils.CreateWaypoint(sourceLocation, nextWaypointDistance, nextWaypointBearing);

                requestSendDateTime = DateTime.Now;
                result =
                    await
                        _client.Player.UpdatePlayerLocation(waypoint.Latitude, waypoint.Longitude,
                            waypoint.Altitude);

                UpdatePositionEvent?.Invoke(waypoint.Latitude, waypoint.Longitude);

                if (functionExecutedWhileWalking != null)
                    await functionExecutedWhileWalking(); // look for pokemon & hit stops

                await Randomizer.Sleep(Math.Min((int)(distanceToTarget / speedInMetersPerSecond * 1000), 3400), 0.1);
                //await Randomizer.Sleep(500); // this is wrong because we should not send too many position updates to the server

            } while (LocationUtils.CalculateDistanceInMeters(sourceLocation, targetLocation) >= 30);

            return result;
        }

        public event UpdatePositionDelegate UpdatePositionEvent;
    }
}