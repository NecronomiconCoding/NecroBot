#region using directives

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
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
        public static async Task Execute(ISession session)
        {
            var tracks = GetGpxTracks(session);
            var curTrkPt = 0;
            var curTrk = 0;
            var maxTrk = tracks.Count - 1;
            var curTrkSeg = 0;
            var eggWalker = new EggWalker(1000, session);
            while (curTrk <= maxTrk)
            {
                var track = tracks.ElementAt(curTrk);
                var trackSegments = track.Segments;
                var maxTrkSeg = trackSegments.Count - 1;
                while (curTrkSeg <= maxTrkSeg)
                {
                    var trackPoints = track.Segments.ElementAt(0).TrackPoints;
                    var maxTrkPt = trackPoints.Count - 1;

                    while (curTrkPt <= maxTrkPt)
                    {
                        var nextPoint = trackPoints.ElementAt(curTrkPt);
                        var distance = LocationUtils.CalculateDistanceInMeters(session.Client.CurrentLatitude,
                            session.Client.CurrentLongitude, Convert.ToDouble(nextPoint.Lat, CultureInfo.InvariantCulture),
                            Convert.ToDouble(nextPoint.Lon, CultureInfo.InvariantCulture));
                        
                        if (distance > 5000)
                        {
                            session.EventDispatcher.Send(new ErrorEvent
                            {
                                Message = session.Translation.GetTranslation(Common.TranslationString.DesiredDestTooFar, nextPoint.Lat, nextPoint.Lon, session.Client.CurrentLatitude, session.Client.CurrentLongitude)
                            });
                            break;
                        }

                        var pokestopList = await GetPokeStops(session);
                        session.EventDispatcher.Send(new PokeStopListEvent {Forts = pokestopList});

                        while (pokestopList.Any())
                        {
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
                                await CatchLurePokemonsTask.Execute(session, pokeStop);
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

                            await RecycleItemsTask.Execute(session);

                            if (session.LogicSettings.EvolveAllPokemonWithEnoughCandy ||
                                session.LogicSettings.EvolveAllPokemonAboveIv)
                            {
                                await EvolvePokemonTask.Execute(session);
                            }

                            if (session.LogicSettings.TransferDuplicatePokemon)
                            {
                                await TransferDuplicatePokemonTask.Execute(session);
                            }

                            if (session.LogicSettings.RenameAboveIv)
                            {
                                await RenamePokemonTask.Execute(session);
                            }
                        }

                        if (session.LogicSettings.SnipeAtPokestops)
                        {
                            await SnipePokemonTask.Execute(session);
                        }

                        await session.Navigation.HumanPathWalking(trackPoints.ElementAt(curTrkPt),
                            session.LogicSettings.WalkingSpeedInKilometerPerHour, async () =>
                            {
                                await CatchNearbyPokemonsTask.Execute(session);
                                //Catch Incense Pokemon
                                await CatchIncensePokemonsTask.Execute(session);
                                await UseNearbyPokestopsTask.Execute(session);
                                return true;
                            }
                            );

                        await eggWalker.ApplyDistance(distance);

                        if (curTrkPt >= maxTrkPt)
                            curTrkPt = 0;
                        else
                            curTrkPt++;
                    } //end trkpts
                    if (curTrkSeg >= maxTrkSeg)
                        curTrkSeg = 0;
                    else
                        curTrkSeg++;
                } //end trksegs
                if (curTrk >= maxTrkSeg)
                    curTrk = 0;
                else
                    curTrk++;
            } //end tracks
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