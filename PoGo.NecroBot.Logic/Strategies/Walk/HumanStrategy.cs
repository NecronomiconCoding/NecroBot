using System;
using System.Threading;
using System.Threading.Tasks;
using GeoCoordinatePortable;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Utils;
using PokemonGo.RocketAPI;
using POGOProtos.Networking.Responses;

namespace PoGo.NecroBot.Logic.Strategies.Walk
{
    class HumanStrategy : IWalkStrategy
    {
        private readonly Client _client;
        public event UpdatePositionDelegate UpdatePositionEvent;
        private double CurrentWalkingSpeed = 0;

        public HumanStrategy(Client client)
        {
            _client = client;
        }

        private const double SpeedDownTo = 10 / 3.6;
        public async Task<PlayerUpdateResponse> Walk(GeoCoordinate targetLocation, Func<Task<bool>> functionExecutedWhileWalking, ISession session, CancellationToken cancellationToken, double walkSpeed = 0.0)
        {
            if (CurrentWalkingSpeed <= 0)
                CurrentWalkingSpeed = session.LogicSettings.WalkingSpeedInKilometerPerHour;
            if (session.LogicSettings.UseWalkingSpeedVariant)
                CurrentWalkingSpeed = session.Navigation.VariantRandom(session, CurrentWalkingSpeed);

            var rw = new Random();
            var speedInMetersPerSecond = CurrentWalkingSpeed / 3.6;
            if(walkSpeed !=0)
            {
                speedInMetersPerSecond = walkSpeed / 3.6;
            }
            var sourceLocation = new GeoCoordinate(_client.CurrentLatitude, _client.CurrentLongitude);

            var nextWaypointBearing = LocationUtils.DegreeBearing(sourceLocation, targetLocation);

           
            var nextWaypointDistance = speedInMetersPerSecond;
            var waypoint = LocationUtils.CreateWaypoint(sourceLocation, nextWaypointDistance, nextWaypointBearing);
            var requestSendDateTime = DateTime.Now;
            var requestVariantDateTime = DateTime.Now;

            var result = await LocationUtils.UpdatePlayerLocationWithAltitude(session, waypoint);

            double SpeedVariantSec = rw.Next(1000, 10000);
            UpdatePositionEvent?.Invoke(waypoint.Latitude, waypoint.Longitude);

            do
            {
                cancellationToken.ThrowIfCancellationRequested();

                var millisecondsUntilGetUpdatePlayerLocationResponse = (DateTime.Now - requestSendDateTime).TotalMilliseconds;
                var millisecondsUntilVariant = (DateTime.Now - requestVariantDateTime).TotalMilliseconds;

                sourceLocation = new GeoCoordinate(_client.CurrentLatitude, _client.CurrentLongitude);
                var currentDistanceToTarget = LocationUtils.CalculateDistanceInMeters(sourceLocation, targetLocation);

                if (currentDistanceToTarget < 40)
                    if (speedInMetersPerSecond > SpeedDownTo)
                        speedInMetersPerSecond = SpeedDownTo;

                if (session.LogicSettings.UseWalkingSpeedVariant)
                {
                    CurrentWalkingSpeed = session.Navigation.VariantRandom(session, CurrentWalkingSpeed);
                    speedInMetersPerSecond = CurrentWalkingSpeed / 3.6;
                }

                if (walkSpeed != 0)
                {
                    speedInMetersPerSecond = walkSpeed / 3.6;
                }

                nextWaypointDistance = Math.Min(currentDistanceToTarget, millisecondsUntilGetUpdatePlayerLocationResponse / 1000 * speedInMetersPerSecond);
                nextWaypointBearing = LocationUtils.DegreeBearing(sourceLocation, targetLocation);
                var testeBear = LocationUtils.DegreeBearing(sourceLocation, new GeoCoordinate(40.780396, -73.974844));
                waypoint = LocationUtils.CreateWaypoint(sourceLocation, nextWaypointDistance, nextWaypointBearing);

                requestSendDateTime = DateTime.Now;
                result = await LocationUtils.UpdatePlayerLocationWithAltitude(session, waypoint);

                UpdatePositionEvent?.Invoke(waypoint.Latitude, waypoint.Longitude);

                if (functionExecutedWhileWalking != null)
                    await functionExecutedWhileWalking(); // look for pokemon
               
            } while (LocationUtils.CalculateDistanceInMeters(sourceLocation, targetLocation) >= (new Random()).Next(1, 10));

            return result;
        }

        public double CalculateDistance(double sourceLat, double sourceLng, double destinationLat, double destinationLng, ISession session = null)
        {
            return LocationUtils.CalculateDistanceInMeters(sourceLat, sourceLng, destinationLat, destinationLng);
        }
    }
}
