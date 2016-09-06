using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GeoCoordinatePortable;
using PoGo.NecroBot.Logic.Service;
using PoGo.NecroBot.Logic.State;
using PokemonGo.RocketAPI;
using POGOProtos.Networking.Responses;
using PoGo.NecroBot.Logic.Model.Yours;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.Utils;

namespace PoGo.NecroBot.Logic.Strategies.Walk
{
    class YoursNavigationStrategy : BaseWalkStrategy, IWalkStrategy
    {
        private YoursDirectionsService _yoursDirectionsService;

        public YoursNavigationStrategy(Client client) : base(client)
        {
            _yoursDirectionsService = null;
        }

        public override async Task<PlayerUpdateResponse> Walk(GeoCoordinate targetLocation, Func<Task<bool>> functionExecutedWhileWalking, ISession session, CancellationToken cancellationToken, double walkSpeed = 0.0)
        {
            GetYoursInstance(session);
            var sourceLocation = new GeoCoordinate(_client.CurrentLatitude, _client.CurrentLongitude, _client.CurrentAltitude);
            var yoursResult = _yoursDirectionsService.GetDirections(sourceLocation, targetLocation);

            if (string.IsNullOrEmpty(yoursResult) || yoursResult.StartsWith("<?xml version=\"1.0\"") || yoursResult.Contains("error"))
            {
                return await RedirectToNextFallbackStrategy(session.LogicSettings, targetLocation, functionExecutedWhileWalking, session, cancellationToken);
            }
            
            var yoursWalk = YoursWalk.Get(yoursResult);
            session.EventDispatcher.Send(new FortTargetEvent { Name = FortInfo.Name, Distance = yoursWalk.Distance, Route = "YoursWalk" });
            List<GeoCoordinate> points = yoursWalk.Waypoints;
            return await DoWalk(points, session, functionExecutedWhileWalking, sourceLocation, targetLocation, cancellationToken);
        }

        private void GetYoursInstance(ISession session)
        {
            if (_yoursDirectionsService == null)
                _yoursDirectionsService = new YoursDirectionsService(session);
        }

        public override double CalculateDistance(double sourceLat, double sourceLng, double destinationLat, double destinationLng, ISession session = null)
        {
            // Too expensive to calculate true distance.
            return 1.5 * base.CalculateDistance(sourceLat, sourceLng, destinationLat, destinationLng);

            /*
            if (session != null)
                GetYoursInstance(session);

            if (_yoursDirectionsService != null)
            {
                var yoursResult = _yoursDirectionsService.GetDirections(new GeoCoordinate(sourceLat, sourceLng), new GeoCoordinate(destinationLat, destinationLng));
                if (string.IsNullOrEmpty(yoursResult) || yoursResult.StartsWith("<?xml version=\"1.0\"") || yoursResult.Contains("error"))
                {
                    return 1.5 * base.CalculateDistance(sourceLat, sourceLng, destinationLat, destinationLng);
                }
                else
                {
                    var yoursWalk = YoursWalk.Get(yoursResult);
                    return yoursWalk.Distance;
                }
            }
            else
            {
                return 1.5 * base.CalculateDistance(sourceLat, sourceLng, destinationLat, destinationLng);
            }
            */
        }
    }
}
