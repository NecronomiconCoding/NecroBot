using System;
using System.Threading;
using System.Threading.Tasks;
using GeoCoordinatePortable;
using PoGo.NecroBot.Logic.State;
using POGOProtos.Networking.Responses;

namespace PoGo.NecroBot.Logic.Strategies.Walk
{
    public interface IWalkStrategy
    {
        event UpdatePositionDelegate UpdatePositionEvent;
        Task<PlayerUpdateResponse> Walk(GeoCoordinate targetLocation, Func<Task<bool>> functionExecutedWhileWalking, ISession session, CancellationToken cancellationToken, double customWalkingSpeed = 0.0);
        double CalculateDistance(double sourceLat, double sourceLng, double destinationLat, double destinationLng, ISession session = null);
    }
}