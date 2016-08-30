using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GeoCoordinatePortable;
using PoGo.NecroBot.Logic.Model.Google;
using PoGo.NecroBot.Logic.Service;
using PoGo.NecroBot.Logic.State;
using PokemonGo.RocketAPI;
using POGOProtos.Networking.Responses;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.Utils;

namespace PoGo.NecroBot.Logic.Strategies.Walk
{
    class GoogleStrategy : BaseWalkStrategy, IWalkStrategy
    {
        private GoogleDirectionsService _googleDirectionsService;

        public GoogleStrategy(Client client) : base(client)
        {
        }

        public override async Task<PlayerUpdateResponse> Walk(GeoCoordinate targetLocation, Func<Task<bool>> functionExecutedWhileWalking, ISession session, CancellationToken cancellationToken, double walkSpeed = 0.0)
        {
            GetGoogleInstance(session);

            _minStepLengthInMeters = session.LogicSettings.DefaultStepLength;
            var currentLocation = new GeoCoordinate(_client.CurrentLatitude, _client.CurrentLongitude, _client.CurrentAltitude);
            var googleResult = _googleDirectionsService.GetDirections(currentLocation, new List<GeoCoordinate>(), targetLocation);

            if (googleResult.Directions.status.Equals("OVER_QUERY_LIMIT"))
            {
                return await RedirectToNextFallbackStrategy(session.LogicSettings, targetLocation, functionExecutedWhileWalking, session, cancellationToken);
            }

            var googleWalk = GoogleWalk.Get(googleResult);
            session.EventDispatcher.Send(new FortTargetEvent { Name = FortInfo.Name, Distance = googleResult.GetDistance(), Route = "GoogleWalk" });
            List <GeoCoordinate> points = googleWalk.Waypoints;
            return await DoWalk(points, session, functionExecutedWhileWalking, currentLocation, targetLocation, cancellationToken);
        }

        private void GetGoogleInstance(ISession session)
        {
            if (_googleDirectionsService == null)
                _googleDirectionsService = new GoogleDirectionsService(session);
        }
        public async Task<double> CalculateDistance(double sourceLat, double sourceLng, double destinationLat, double destinationLng)
        {
            //need to implement API call to calculate real distance, that will impact on perfomance.

            return 1.5 * LocationUtils.CalculateDistanceInMeters(sourceLat, sourceLng, destinationLat, destinationLng);
        }
    }
}