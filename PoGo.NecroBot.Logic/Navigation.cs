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
using PoGo.NecroBot.Logic.Event;

#endregion

namespace PoGo.NecroBot.Logic
{
    public delegate void UpdatePositionDelegate(double lat, double lng);

    public class Navigation
    {
        public IWalkStrategy WalkStrategy { get; set; }
        private readonly Client _client;
        private Random WalkingRandom = new Random();

        public Navigation(Client client, ILogicSettings logicSettings)
        {
            _client = client;
            WalkStrategy = GetStrategy(logicSettings);
        }

        public double VariantRandom(ISession session, double currentSpeed)
        {
            if (WalkingRandom.Next(1, 10) > 5)
            {
                if (WalkingRandom.Next(1, 10) > 5)
                {
                    var randomicSpeed = currentSpeed;
                    var max = session.LogicSettings.WalkingSpeedInKilometerPerHour + session.LogicSettings.WalkingSpeedVariant;
                    randomicSpeed += WalkingRandom.NextDouble() * (0.02 - 0.001) + 0.001;

                    if (randomicSpeed > max)
                        randomicSpeed = max;

                    if (Math.Round(randomicSpeed, 2) != Math.Round(currentSpeed, 2))
                    {
                        session.EventDispatcher.Send(new HumanWalkingEvent
                        {
                            OldWalkingSpeed = currentSpeed,
                            CurrentWalkingSpeed = randomicSpeed
                        });
                    }

                    return randomicSpeed;
                }
                else
                {
                    var randomicSpeed = currentSpeed;
                    var min = session.LogicSettings.WalkingSpeedInKilometerPerHour - session.LogicSettings.WalkingSpeedVariant;
                    randomicSpeed -= WalkingRandom.NextDouble() * (0.02 - 0.001) + 0.001;                    

                    if (randomicSpeed < min)
                        randomicSpeed = min;

                    if (Math.Round(randomicSpeed, 2) != Math.Round(currentSpeed, 2))
                    {
                        session.EventDispatcher.Send(new HumanWalkingEvent
                        {
                            OldWalkingSpeed = currentSpeed,
                            CurrentWalkingSpeed = randomicSpeed
                        });
                    }

                    return randomicSpeed;
                }
            }

            return currentSpeed;
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

            if (logicSettings.UseYoursWalk)
            {
                return new YoursNavigationStrategy(_client);
            }

            return new HumanStrategy(_client);
        }
    }
}