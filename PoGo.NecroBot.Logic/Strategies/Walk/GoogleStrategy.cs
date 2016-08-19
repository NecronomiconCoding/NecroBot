using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GeoCoordinatePortable;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.Model.Google;
using PoGo.NecroBot.Logic.Service;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Utils;
using PokemonGo.RocketAPI;
using POGOProtos.Networking.Responses;

namespace PoGo.NecroBot.Logic.Strategies.Walk
{
    class GoogleStrategy : IWalkStrategy
    {
        private readonly Client _client;
        public event UpdatePositionDelegate UpdatePositionEvent;

        private double _currentWalkingSpeed;
        private const double SpeedDownTo = 10 / 3.6;
        private DirectionsService _googleDirectionsService;
        private HumanStrategy _humanStraightLine;
        private DateTime _lastMajorVariantWalkingSpeed;
        private DateTime _nextMajorVariantWalkingSpeed;
        private readonly Random _randWalking = new Random();

        public GoogleStrategy(Client client)
        {
            _client = client;
        }
        
        public async Task<PlayerUpdateResponse> Walk(GeoCoordinate targetLocation, Func<Task<bool>> functionExecutedWhileWalking, ISession session, CancellationToken cancellationToken)
        {
            GetGoogleInstance(session);
            var sourceLocation = new GeoCoordinate(_client.CurrentLatitude, _client.CurrentLongitude, _client.CurrentAltitude);
            var googleResult = _googleDirectionsService.GetDirections(sourceLocation, new List<GeoCoordinate>(), targetLocation);

            if (googleResult.Directions.status.Equals("OVER_QUERY_LIMIT"))
            {
                return await RedirectToHumanStrategy(targetLocation, functionExecutedWhileWalking, session, cancellationToken);
            }
            session.EventDispatcher.Send(new NewPathToDestinyEvent { GoogleData = googleResult });

            var googleWalk = GoogleWalk.Get(googleResult);

            PlayerUpdateResponse result = null;
            List<GeoCoordinate> points = googleWalk.Waypoints;

            foreach (var nextStep in points)
            {
                var speedInMetersPerSecond = session.LogicSettings.UseWalkingSpeedVariant ? MajorWalkingSpeedVariant(session) :
                session.LogicSettings.WalkingSpeedInKilometerPerHour / 3.6;
                sourceLocation = new GeoCoordinate(_client.CurrentLatitude, _client.CurrentLongitude);

                var nextWaypointBearing = LocationUtils.DegreeBearing(sourceLocation, nextStep);
                var nextWaypointDistance = speedInMetersPerSecond;
                var waypoint = LocationUtils.CreateWaypoint(sourceLocation, nextWaypointDistance, nextWaypointBearing);

                var requestSendDateTime = DateTime.Now;
                result = await _client.Player.UpdatePlayerLocation(waypoint.Latitude, waypoint.Longitude, waypoint.Altitude);

                UpdatePositionEvent?.Invoke(waypoint.Latitude, waypoint.Longitude);

                var realDistanceToTarget = LocationUtils.CalculateDistanceInMeters(sourceLocation, targetLocation);
                if (realDistanceToTarget < 10)
                    break;

                do
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var millisecondsUntilGetUpdatePlayerLocationResponse = (DateTime.Now - requestSendDateTime).TotalMilliseconds;

                    sourceLocation = new GeoCoordinate(_client.CurrentLatitude, _client.CurrentLongitude);
                    var currentDistanceToTarget = LocationUtils.CalculateDistanceInMeters(sourceLocation, nextStep);

                    var realDistanceToTargetSpeedDown = LocationUtils.CalculateDistanceInMeters(sourceLocation, targetLocation);
                    if (realDistanceToTargetSpeedDown < 40)
                        if (speedInMetersPerSecond > SpeedDownTo)
                            speedInMetersPerSecond = SpeedDownTo;

                    if (session.LogicSettings.UseWalkingSpeedVariant)
                        speedInMetersPerSecond = MinorWalkingSpeedVariant(session);

                    nextWaypointDistance = Math.Min(currentDistanceToTarget, millisecondsUntilGetUpdatePlayerLocationResponse / 1000 * speedInMetersPerSecond);
                    nextWaypointBearing = LocationUtils.DegreeBearing(sourceLocation, nextStep);
                    waypoint = LocationUtils.CreateWaypoint(sourceLocation, nextWaypointDistance, nextWaypointBearing);

                    requestSendDateTime = DateTime.Now;
                    result = await _client.Player.UpdatePlayerLocation(waypoint.Latitude, waypoint.Longitude, waypoint.Altitude);

                    UpdatePositionEvent?.Invoke(waypoint.Latitude, waypoint.Longitude);

                    if (functionExecutedWhileWalking != null)
                        await functionExecutedWhileWalking(); // look for pokemon
                } while (LocationUtils.CalculateDistanceInMeters(sourceLocation, nextStep) >= 2);

                UpdatePositionEvent?.Invoke(nextStep.Latitude, nextStep.Longitude);
            }

            return result;
        }

        private async Task<PlayerUpdateResponse> GoToWaypoint(GeoCoordinate targetLocation, Func<Task<bool>> functionExecutedWhileWalking, ISession session, CancellationToken cancellationToken, double distanceToTarget)
        {
            PlayerUpdateResponse result;

            var speedInMetersPerSecond = session.LogicSettings.UseWalkingSpeedVariant ? MajorWalkingSpeedVariant(session) :
                    session.LogicSettings.WalkingSpeedInKilometerPerHour / 3.6;
            var sourceLocation = new GeoCoordinate(_client.CurrentLatitude, _client.CurrentLongitude);

            var requestSendDateTime = DateTime.Now;

            do
            {
                cancellationToken.ThrowIfCancellationRequested();

                var millisecondsUntilGetUpdatePlayerLocationResponse = (DateTime.Now - requestSendDateTime).TotalMilliseconds;

                sourceLocation = new GeoCoordinate(_client.CurrentLatitude, _client.CurrentLongitude);
                var currentDistanceToTarget = LocationUtils.CalculateDistanceInMeters(sourceLocation, targetLocation);

                if (distanceToTarget < 40)
                    if (speedInMetersPerSecond > SpeedDownTo)
                        speedInMetersPerSecond = SpeedDownTo;

                if (session.LogicSettings.UseWalkingSpeedVariant)
                    speedInMetersPerSecond = MinorWalkingSpeedVariant(session);

                var nextWaypointDistance = Math.Min(currentDistanceToTarget, millisecondsUntilGetUpdatePlayerLocationResponse / 1000 * speedInMetersPerSecond);
                var nextWaypointBearing = LocationUtils.DegreeBearing(sourceLocation, targetLocation);
                var waypoint = LocationUtils.CreateWaypoint(sourceLocation, nextWaypointDistance, nextWaypointBearing);

                requestSendDateTime = DateTime.Now;
                result = await _client.Player.UpdatePlayerLocation(waypoint.Latitude, waypoint.Longitude, waypoint.Altitude);

                UpdatePositionEvent?.Invoke(waypoint.Latitude, waypoint.Longitude);

                if (functionExecutedWhileWalking != null)
                    await functionExecutedWhileWalking(); // look for pokemon
            } while (LocationUtils.CalculateDistanceInMeters(sourceLocation, targetLocation) >= 2);

            return result;
        }

        private double MinorWalkingSpeedVariant(ISession session)
        {
            if (_randWalking.Next(1, 10) > 5) //Random change or no variant speed
            {
                var oldWalkingSpeed = _currentWalkingSpeed;

                if (_randWalking.Next(1, 10) > 5) //Random change upper or lower variant speed
                {
                    var randomMax = session.LogicSettings.WalkingSpeedInKilometerPerHour + session.LogicSettings.WalkingSpeedVariant + 0.5;

                    _currentWalkingSpeed += _randWalking.NextDouble() * (0.01 - 0.09) + 0.01;
                    if (_currentWalkingSpeed > randomMax)
                        _currentWalkingSpeed = randomMax;
                }
                else
                {
                    var randomMin = session.LogicSettings.WalkingSpeedInKilometerPerHour - session.LogicSettings.WalkingSpeedVariant - 0.5;

                    _currentWalkingSpeed -= _randWalking.NextDouble() * (0.01 - 0.09) + 0.01;
                    if (_currentWalkingSpeed < randomMin)
                        _currentWalkingSpeed = randomMin;
                }

                if (oldWalkingSpeed != _currentWalkingSpeed)
                {
                    session.EventDispatcher.Send(new HumanWalkingEvent
                    {
                        OldWalkingSpeed = oldWalkingSpeed,
                        CurrentWalkingSpeed = _currentWalkingSpeed
                    });
                }
            }

            return _currentWalkingSpeed / 3.6;
        }
        private double MajorWalkingSpeedVariant(ISession session)
        {
            if (_lastMajorVariantWalkingSpeed == DateTime.MinValue && _nextMajorVariantWalkingSpeed == DateTime.MinValue)
            {
                var minutes = _randWalking.NextDouble() * (2 - 6) + 2;
                _lastMajorVariantWalkingSpeed = DateTime.Now;
                _nextMajorVariantWalkingSpeed = _lastMajorVariantWalkingSpeed.AddMinutes(minutes);
                _currentWalkingSpeed = session.LogicSettings.WalkingSpeedInKilometerPerHour;
            }
            else if (_nextMajorVariantWalkingSpeed.Ticks < DateTime.Now.Ticks)
            {
                var oldWalkingSpeed = _currentWalkingSpeed;

                var randomMin = session.LogicSettings.WalkingSpeedInKilometerPerHour - session.LogicSettings.WalkingSpeedVariant;
                var randomMax = session.LogicSettings.WalkingSpeedInKilometerPerHour + session.LogicSettings.WalkingSpeedVariant;
                _currentWalkingSpeed = _randWalking.NextDouble() * (randomMax - randomMin) + randomMin;

                var minutes = _randWalking.NextDouble() * (2 - 6) + 2;
                _lastMajorVariantWalkingSpeed = DateTime.Now;
                _nextMajorVariantWalkingSpeed = _lastMajorVariantWalkingSpeed.AddMinutes(minutes);

                session.EventDispatcher.Send(new HumanWalkingEvent
                {
                    OldWalkingSpeed = oldWalkingSpeed,
                    CurrentWalkingSpeed = _currentWalkingSpeed
                });
            }

            return _currentWalkingSpeed / 3.6;
        }

        private Task<PlayerUpdateResponse> RedirectToHumanStrategy(GeoCoordinate targetLocation, Func<Task<bool>> functionExecutedWhileWalking, ISession session, CancellationToken cancellationToken)
        {
            if (_humanStraightLine == null)
                _humanStraightLine = new HumanStrategy(_client);

            return _humanStraightLine.Walk(targetLocation, functionExecutedWhileWalking, session, cancellationToken);
        }

        private void GetGoogleInstance(ISession session)
        {
            if (_googleDirectionsService == null)
                _googleDirectionsService = new DirectionsService(session);
        }
    }
}
