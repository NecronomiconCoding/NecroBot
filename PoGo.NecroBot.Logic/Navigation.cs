#region using directives

using System;
using System.Threading;
using System.Threading.Tasks;
using GeoCoordinatePortable;
using PoGo.NecroBot.Logic.Interfaces.Configuration;
using PokemonGo.RocketAPI;
using POGOProtos.Networking.Responses;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Strategies.Walk;

#endregion

namespace PoGo.NecroBot.Logic
{
    public delegate void UpdatePositionDelegate(double lat, double lng);

    public class Navigation
    {

        public IWalkStrategy WalkStrategy { get; set; }
        private readonly Client _client;

        public Navigation(Client client, ILogicSettings logicSettings)
        {
            _client = client;
            WalkStrategy = GetStrategy(logicSettings);
        }

        public async Task<PlayerUpdateResponse> Move(GeoCoordinate targetLocation,
            Func<Task<bool>> functionExecutedWhileWalking,
            ISession session,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            // If the stretegies become bigger, create a factory for easy management

            return await WalkStrategy.Walk(targetLocation, functionExecutedWhileWalking, session, cancellationToken);
        }

        private IWalkStrategy GetStrategy(ILogicSettings logicSettings)
        {
            // Maybe change configuration for a Navigation Type.
            if (logicSettings.DisableHumanWalking)
            {
                return new FlyStrategy(_client);
            }

            if (logicSettings.UseGpxPathing)
            {
                return new HumanPathWalkingStrategy(_client);
            }

            if (logicSettings.UseGoogleWalk)
            {
                return new GoogleStrategy(_client);
            }

            return new HumanStrategy(_client);
        }
    }
}