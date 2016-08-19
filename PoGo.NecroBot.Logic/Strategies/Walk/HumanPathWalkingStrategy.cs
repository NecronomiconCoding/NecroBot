using System;
using System.Threading;
using System.Threading.Tasks;
using GeoCoordinatePortable;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Utils;
using PokemonGo.RocketAPI;
using POGOProtos.Networking.Responses;

namespace PoGo.NecroBot.Logic.Strategies.Walk
{
    class HumanPathWalkingStrategy : IWalkStrategy
    {
        private readonly Client _client;
        public event UpdatePositionDelegate UpdatePositionEvent;
        private double LastWalkingSpeed = 0;

        public HumanPathWalkingStrategy(Client client)
        {
            _client = client;
        }

        public async Task<PlayerUpdateResponse> Walk(GeoCoordinate targetLocation, Func<Task<bool>> functionExecutedWhileWalking, ISession session, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            //PlayerUpdateResponse result = null;

            if (LastWalkingSpeed <= 0)
                LastWalkingSpeed = session.LogicSettings.WalkingSpeedInKilometerPerHour;

            var rw = new Random();
            var speedInMetersPerSecond = LastWalkingSpeed / 3.6;
            var sourceLocation = new GeoCoordinate(_client.CurrentLatitude, _client.CurrentLongitude);
            LocationUtils.CalculateDistanceInMeters(sourceLocation, targetLocation);
            var nextWaypointBearing = LocationUtils.DegreeBearing(sourceLocation, targetLocation);
            var nextWaypointDistance = speedInMetersPerSecond;
            var waypoint = LocationUtils.CreateWaypoint(sourceLocation, nextWaypointDistance, nextWaypointBearing);
            var requestSendDateTime = DateTime.Now;
            var requestVariantDateTime = DateTime.Now;

            var result = await _client.Player.UpdatePlayerLocation(waypoint.Latitude, waypoint.Longitude, waypoint.Altitude);

            double SpeedVariantSec = rw.Next(1000, 10000);
            UpdatePositionEvent?.Invoke(waypoint.Latitude, waypoint.Longitude);

            do
            {
                cancellationToken.ThrowIfCancellationRequested();

                var millisecondsUntilGetUpdatePlayerLocationResponse =
                    (DateTime.Now - requestSendDateTime).TotalMilliseconds;
                var millisecondsUntilVariant =
                        (DateTime.Now - requestVariantDateTime).TotalMilliseconds;

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

                if (session.LogicSettings.UseWalkingSpeedVariant)
                {
                    if (millisecondsUntilVariant >= SpeedVariantSec)
                    {
                        var randomMin = session.LogicSettings.WalkingSpeedInKilometerPerHour - 1.2;
                        var randomMax = session.LogicSettings.WalkingSpeedInKilometerPerHour + 1.2;
                        var RandomWalkSpeed = rw.NextDouble() * (randomMax - randomMin) + randomMin;

                        session.EventDispatcher.Send(new HumanWalkingEvent
                        {
                            OldWalkingSpeed = LastWalkingSpeed,
                            CurrentWalkingSpeed = RandomWalkSpeed
                        });

                        LastWalkingSpeed = RandomWalkSpeed;
                        speedInMetersPerSecond = RandomWalkSpeed / 3.6;
                        SpeedVariantSec += rw.Next(5000, 15000);
                    }
                }

                nextWaypointDistance = Math.Min(currentDistanceToTarget, millisecondsUntilGetUpdatePlayerLocationResponse / 1000 * speedInMetersPerSecond);
                nextWaypointBearing = LocationUtils.DegreeBearing(sourceLocation, targetLocation);
                waypoint = LocationUtils.CreateWaypoint(sourceLocation, nextWaypointDistance, nextWaypointBearing);

                requestSendDateTime = DateTime.Now;
                result = await _client.Player.UpdatePlayerLocation(waypoint.Latitude, waypoint.Longitude, waypoint.Altitude);

                UpdatePositionEvent?.Invoke(waypoint.Latitude, waypoint.Longitude);

                if (functionExecutedWhileWalking != null)
                    await functionExecutedWhileWalking(); // look for pokemon & hit stops
            } while (LocationUtils.CalculateDistanceInMeters(sourceLocation, targetLocation) >= 30);

            return result;
        }
    }
}
