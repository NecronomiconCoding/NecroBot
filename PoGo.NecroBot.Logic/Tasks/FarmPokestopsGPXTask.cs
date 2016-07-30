#region using directives

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        static DateTime lastTasksCall = DateTime.Now;

        //This function deals with the main gps logic from point to point
        private static async Task GpxTrackPointProcess(ISession session, CancellationToken cancellationToken, EggWalker eggWalker, GpxReader.Trkpt trackPoint)
        {
            var distance = LocationUtils.CalculateDistanceInMeters(session.Client.CurrentLatitude,
                session.Client.CurrentLongitude, Convert.ToDouble(trackPoint.Lat, CultureInfo.InvariantCulture),
                Convert.ToDouble(trackPoint.Lon, CultureInfo.InvariantCulture));

            if (distance > 5000)
            {
                session.EventDispatcher.Send(new ErrorEvent
                {
                    Message = session.Translation.GetTranslation(Common.TranslationString.DesiredDestTooFar, trackPoint.Lat, trackPoint.Lon, session.Client.CurrentLatitude, session.Client.CurrentLongitude)
                });
            }
            else
            {
                var pokestopList = await GetPokeStops(session);
                session.EventDispatcher.Send(new PokeStopListEvent { Forts = pokestopList });

                while (pokestopList.Any()) // warning: this is never entered due to ps cooldowns from UseNearbyPokestopsTask 
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    pokestopList =
                        pokestopList.OrderBy(
                            i =>
                                LocationUtils.CalculateDistanceInMeters(session.Client.CurrentLatitude,
                                    session.Client.CurrentLongitude, i.Latitude, i.Longitude)).ToList();
                    var pokeStop = pokestopList[0];
                    pokestopList.RemoveAt(0);

                    var fortInfo = await session.Client.Fort.GetFort(pokeStop.Id, pokeStop.Latitude, pokeStop.Longitude);

                    if (pokeStop.LureInfo != null)
                    {
                        await CatchLurePokemonsTask.Execute(session, pokeStop, cancellationToken);
                    }

                    var fortSearch =
                        await session.Client.Fort.SearchFort(pokeStop.Id, pokeStop.Latitude, pokeStop.Longitude);

                    if (fortSearch.ExperienceAwarded > 0)
                    {
                        session.EventDispatcher.Send(new FortUsedEvent
                        {
                            Id = pokeStop.Id,
                            Name = fortInfo.Name,
                            Exp = fortSearch.ExperienceAwarded,
                            Gems = fortSearch.GemsAwarded,
                            Items = StringUtils.GetSummedFriendlyNameOfItemAwardList(fortSearch.ItemsAwarded),
                            Latitude = pokeStop.Latitude,
                            Longitude = pokeStop.Longitude
                        });
                    }
                    if (fortSearch.ItemsAwarded.Count > 0)
                    {
                        await session.Inventory.RefreshCachedInventory();
                    }
                }

                if (DateTime.Now > lastTasksCall)
                {
                    lastTasksCall = DateTime.Now.AddMilliseconds(Math.Min(session.LogicSettings.DelayBetweenPlayerActions, 3000));

                    await RecycleItemsTask.Execute(session, cancellationToken);

                    if (session.LogicSettings.SnipeAtPokestops)
                    {
                        await SnipePokemonTask.Execute(session, cancellationToken);
                    }

                    if (session.LogicSettings.EvolveAllPokemonWithEnoughCandy ||
                        session.LogicSettings.EvolveAllPokemonAboveIv)
                    {
                        await EvolvePokemonTask.Execute(session, cancellationToken);
                    }

                    if (session.LogicSettings.TransferDuplicatePokemon)
                    {
                        await TransferDuplicatePokemonTask.Execute(session, cancellationToken);
                    }

                    if (session.LogicSettings.RenameAboveIv)
                    {
                        await RenamePokemonTask.Execute(session, cancellationToken);
                    }
                }

                await session.Navigation.HumanPathWalking(
                        trackPoint,
                        session.LogicSettings.WalkingSpeedInKilometerPerHour,
                        async () =>
                        {
                            await CatchNearbyPokemonsTask.Execute(session, cancellationToken);
                        //Catch Incense Pokemon
                        await CatchIncensePokemonsTask.Execute(session, cancellationToken);
                            await UseNearbyPokestopsTask.Execute(session, cancellationToken);
                            return true;
                        },
                        cancellationToken
                    );

                await eggWalker.ApplyDistance(distance, cancellationToken);
            }
        }
        
        public static async Task Execute(ISession session, CancellationToken cancellationToken)
        {
            //Gets all the gpx track elements
            var tracks = GetGpxTracks(session);     
            
            //Sets up the eggWalker object                   
            var eggWalker = new EggWalker(1000, session);
            
            //Loops over all the track elements
            for(int curTrk = 0; curTrk < tracks.Count; curTrk++)
            {
                //Checks if a cancellation operation has been performed
                cancellationToken.ThrowIfCancellationRequested();

                //Gets the current track
                var track = tracks.ElementAt(curTrk);
                
                //If config is to display gpx track output information
                string trackNameOutput = "";//Used to hold the name of the tracker for output *if any*
                if (session.LogicSettings.GpxSettings.OutputTrackPathData)
                {   
                    //Note: checks if name is populated if not, desc if not uses element count number
                    trackNameOutput = (!string.IsNullOrWhiteSpace(track.Name)) ? track.Name : (!string.IsNullOrWhiteSpace(track.Desc)) ? track.Desc : string.Format("track number: {0}", curTrk);
                    session.EventDispatcher.Send(new NoticeEvent
                    {
                        Message = session.Translation.GetTranslation(Common.TranslationString.GpxStartTrack, trackNameOutput)
                    });
                }

                //Gets the segment elements from the current track
                var trackSegments = track.Segments;
                
                //Loops over the track segment elements
                for(int curTrkSeg = 0; curTrkSeg < trackSegments.Count; curTrkSeg++)
                {
                    //Checks if a cancellation operation has been performed
                    cancellationToken.ThrowIfCancellationRequested();

                    //Gets the track points from the current segment element
                    var segment = track.Segments.ElementAt(0);

                    //Gets the segment track points
                    var trackPoints = segment.TrackPoints;

                    //Loops through the indivudual track points within the segment
                    for (int curTrkPt=0; curTrkPt < trackPoints.Count; curTrkPt++)
                    {
                        //Checks if a cancellation operation has been performed
                        cancellationToken.ThrowIfCancellationRequested();

                        //Gets the current track point element
                        var trackPoint = trackPoints.ElementAt(curTrkPt);

                        //If config is to display gpx track output information
                        string trackPointNameOutput = "";//Used to hold the name of the tracker point for output *if any*
                        if (session.LogicSettings.GpxSettings.OutputTrackPointPathData)
                        {   
                            //Note: checks if name is populated if not, desc if not uses element count number
                            trackPointNameOutput = (!string.IsNullOrWhiteSpace(trackPoint.Name)) ? trackPoint.Name : (!string.IsNullOrWhiteSpace(trackPoint.Desc)) ? trackPoint.Desc : string.Format("element number {0}", curTrkPt);
                            session.EventDispatcher.Send(new NoticeEvent
                            {
                                Message = session.Translation.GetTranslation(Common.TranslationString.GpxStartPoint, curTrkSeg, trackPointNameOutput)
                            });
                        }

                        //Runs the processing logic for actions with the track points, i.e. pokestops, walking etc
                        await GpxTrackPointProcess(session, cancellationToken, eggWalker, trackPoint);

                        //If config is to display gpx track output information
                        if (session.LogicSettings.GpxSettings.OutputTrackPointPathData)
                        {   
                            session.EventDispatcher.Send(new NoticeEvent
                            {
                                Message = session.Translation.GetTranslation(Common.TranslationString.GpxEndPoint, curTrkSeg, trackPointNameOutput)
                            });
                        }
                    }

                }

                //If config is to display gpx track output information
                if (session.LogicSettings.GpxSettings.OutputTrackPathData)
                {   
                    session.EventDispatcher.Send(new NoticeEvent
                    {
                        Message = session.Translation.GetTranslation(Common.TranslationString.GpxEndTrack, trackNameOutput)
                    });
                }

            }
        }

        private static List<GpxReader.Trk> GetGpxTracks(ISession session)
        {
            var xmlString = File.ReadAllText(session.LogicSettings.GpxFile);
            var readgpx = new GpxReader(xmlString, session);
            return readgpx.Tracks;
        }

        //Please do not change GetPokeStops() in this file, it's specifically set
        //to only find stops within 40 meters
        //this is for gpx pathing, we are not going to the pokestops,
        //so do not make it more than 40 because it will never get close to those stops.
        private static async Task<List<FortData>> GetPokeStops(ISession session)
        {
            var mapObjects = await session.Client.Map.GetMapObjects();

            // Wasn't sure how to make this pretty. Edit as needed.
            var pokeStops = mapObjects.MapCells.SelectMany(i => i.Forts)
                .Where(
                    i =>
                        i.Type == FortType.Checkpoint &&
                        i.CooldownCompleteTimestampMs < DateTime.UtcNow.ToUnixTime() &&
                        ( // Make sure PokeStop is within 40 meters or else it is pointless to hit it
                            LocationUtils.CalculateDistanceInMeters(
                                session.Client.CurrentLatitude, session.Client.CurrentLongitude,
                                i.Latitude, i.Longitude) < 40) ||
                        session.LogicSettings.MaxTravelDistanceInMeters == 0
                );

            return pokeStops.ToList();
        }
    }
}
