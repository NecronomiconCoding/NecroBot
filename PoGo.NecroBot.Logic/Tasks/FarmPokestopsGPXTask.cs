#region using directives

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GeoCoordinatePortable;
using PoGo.NecroBot.Logic.Common;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Utils;
using PokemonGo.RocketAPI.Extensions;
using POGOProtos.Map.Fort;

#endregion

namespace PoGo.NecroBot.Logic.Tasks
{
    public static class FarmPokestopsGpxTask
    {
        private static int? _resumeTrack = null;
        private static int? _resumeTrackSeg = null;
        private static int? _resumeTrackPt = null;

        public static async Task Execute(ISession session, CancellationToken cancellationToken)
        {
            var tracks = GetGpxTracks(session);
            var eggWalker = new EggWalker(1000, session);

            if (!_resumeTrack.HasValue || !_resumeTrackSeg.HasValue || !_resumeTrackPt.HasValue)
            {
                _resumeTrack = session.LogicSettings.ResumeTrack;
                _resumeTrackSeg = session.LogicSettings.ResumeTrackSeg;
                _resumeTrackPt = session.LogicSettings.ResumeTrackPt;

                // initialize the variables in UseNearbyPokestopsTask here, as this is a fresh start.
                UseNearbyPokestopsTask.Initialize();
            }

            for (var curTrk = _resumeTrack.Value; curTrk < tracks.Count; curTrk++)
            {
                _resumeTrack = curTrk;
                cancellationToken.ThrowIfCancellationRequested();

                var track = tracks.ElementAt(curTrk);
                var trackSegments = track.Segments;

                for (var curTrkSeg = _resumeTrackSeg.Value; curTrkSeg < trackSegments.Count; curTrkSeg++)
                {
                    _resumeTrackSeg = curTrkSeg;
                    cancellationToken.ThrowIfCancellationRequested();

                    var trackPoints = trackSegments.ElementAt(curTrkSeg).TrackPoints;

                    for (var curTrkPt = _resumeTrackPt.Value; curTrkPt < trackPoints.Count; curTrkPt++)
                    {
                        _resumeTrackPt = curTrkPt;
                        cancellationToken.ThrowIfCancellationRequested();

                        var nextPoint = trackPoints.ElementAt(curTrkPt);
                        var distance = LocationUtils.CalculateDistanceInMeters(session.Client.CurrentLatitude,
                            session.Client.CurrentLongitude,
                            Convert.ToDouble(nextPoint.Lat, CultureInfo.InvariantCulture),
                            Convert.ToDouble(nextPoint.Lon, CultureInfo.InvariantCulture));

                        if (distance > 5000)
                        {
                            session.EventDispatcher.Send(new ErrorEvent
                            {
                                Message =
                                    session.Translation.GetTranslation(TranslationString.DesiredDestTooFar,
                                        nextPoint.Lat, nextPoint.Lon, session.Client.CurrentLatitude,
                                        session.Client.CurrentLongitude)
                            });
                            break;
                        }

                        var geo = new GeoCoordinate(Convert.ToDouble(trackPoints.ElementAt(curTrkPt).Lat, CultureInfo.InvariantCulture),
                            Convert.ToDouble(trackPoints.ElementAt(curTrkPt).Lon, CultureInfo.InvariantCulture));

                        await session.Navigation.Move(geo,
                            async () =>
                            {
                                await CatchNearbyPokemonsTask.Execute(session, cancellationToken);
                                //Catch Incense Pokemon
                                await CatchIncensePokemonsTask.Execute(session, cancellationToken);
                                await UseNearbyPokestopsTask.Execute(session, cancellationToken);
                                return true;
                            },
                            session,
                            cancellationToken);

                        await eggWalker.ApplyDistance(distance, cancellationToken);
                    } //end trkpts
                    _resumeTrackPt = 0;
                } //end trksegs
                _resumeTrackSeg = 0;
            } //end tracks
            _resumeTrack = 0;
        }

        private static List<GpxReader.Trk> GetGpxTracks(ISession session)
        {
            var xmlString = File.ReadAllText(session.LogicSettings.GpxFile);
            var readgpx = new GpxReader(xmlString, session);
            return readgpx.Tracks;
        }
    }
}