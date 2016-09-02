#region using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GeoCoordinatePortable;
using PoGo.NecroBot.Logic.Common;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Utils;
using PokemonGo.RocketAPI.Extensions;
using POGOProtos.Map.Fort;
using POGOProtos.Networking.Responses;

#endregion

namespace PoGo.NecroBot.Logic.Tasks
{
    public static class FarmPokestopsTask
    {
        private static bool checkForMoveBackToDefault = true;
        public static async Task Execute(ISession session, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var distanceFromStart = LocationUtils.CalculateDistanceInMeters(
                session.Settings.DefaultLatitude, session.Settings.DefaultLongitude,
                session.Client.CurrentLatitude, session.Client.CurrentLongitude);

            // Edge case for when the client somehow ends up outside the defined radius
            if (session.LogicSettings.MaxTravelDistanceInMeters != 0 && checkForMoveBackToDefault &&
                distanceFromStart > session.LogicSettings.MaxTravelDistanceInMeters)
            {
                checkForMoveBackToDefault = false;
                Logger.Write(
                    session.Translation.GetTranslation(TranslationString.FarmPokestopsOutsideRadius, distanceFromStart),
                    LogLevel.Warning);

                var eggWalker = new EggWalker(1000, session);

                await session.Navigation.Move(new GeoCoordinate(
                    session.Settings.DefaultLatitude,
                    session.Settings.DefaultLongitude,
                    LocationUtils.getElevation(session, session.Settings.DefaultLatitude,
                    session.Settings.DefaultLongitude)),
                    null,
                    session,
                    cancellationToken);

                // we have moved this distance, so apply it immediately to the egg walker.
                await eggWalker.ApplyDistance(distanceFromStart, cancellationToken);
            }
            checkForMoveBackToDefault = false;
            // initialize the variables in UseNearbyPokestopsTask here, as this is a fresh start.
            UseNearbyPokestopsTask.Initialize();
            await UseNearbyPokestopsTask.Execute(session, cancellationToken);
        }
    }
}
