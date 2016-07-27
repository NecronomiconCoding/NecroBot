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
        public static async Task Execute(Session session, StateMachine machine)
        {
            var tracks = GetGpxTracks(session);
            var curTrkPt = 0;
            var curTrk = 0;
            var maxTrk = tracks.Count - 1;
            var curTrkSeg = 0;
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
                            machine.Fire(new ErrorEvent
                            {
                                Message = session.Translations.GetTranslation(Common.TranslationString.DesiredDestTooFar, nextPoint.Lat, nextPoint.Lon, session.Client.CurrentLatitude, session.Client.CurrentLongitude)
                            });
                            break;
                        }

                        var pokestopList = await GetPokeStops(session);
                        machine.Fire(new PokeStopListEvent {Forts = pokestopList});

                        while (pokestopList.Any())
                        {
                            pokestopList =
                                pokestopList.OrderBy(
                                    i =>
                                        LocationUtils.CalculateDistanceInMeters(session.Client.CurrentLatitude,
                                            session.Client.CurrentLongitude, i.Latitude, i.Longitude)).ToList();
                            var pokeStop = pokestopList[0];
                            pokestopList.RemoveAt(0);

                            await session.Client.Fort.GetFort(pokeStop.Id, pokeStop.Latitude, pokeStop.Longitude);

                            if (pokeStop.LureInfo != null)
                            {
                                await CatchLurePokemonsTask.Execute(session, machine, pokeStop);
                            }

                            var fortSearch =
                                await session.Client.Fort.SearchFort(pokeStop.Id, pokeStop.Latitude, pokeStop.Longitude);

                            if (fortSearch.ExperienceAwarded > 0)
                            {
                                machine.Fire(new FortUsedEvent
                                {
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

                            await RecycleItemsTask.Execute(session, machine);

                            if (session.LogicSettings.UseEggIncubators)
                            {
                                await UseIncubatorsTask.Execute(session, machine);
                            }

                            if (session.LogicSettings.EvolveAllPokemonWithEnoughCandy ||
                                session.LogicSettings.EvolveAllPokemonAboveIv)
                            {
                                await EvolvePokemonTask.Execute(session, machine);
                            }

                            if (session.LogicSettings.TransferDuplicatePokemon)
                            {
                                await TransferDuplicatePokemonTask.Execute(session, machine);
                            }

                            if (session.LogicSettings.RenameAboveIv)
                            {
                                await RenamePokemonTask.Execute(session, machine);
                            }
                        }

                        await session.Navigation.HumanPathWalking(trackPoints.ElementAt(curTrkPt),
                            session.LogicSettings.WalkingSpeedInKilometerPerHour, async () =>
                            {
                                await CatchNearbyPokemonsTask.Execute(session, machine);
                                //Catch Incense Pokemon
                                await CatchIncensePokemonsTask.Execute(session, machine);
                                await UseNearbyPokestopsTask.Execute(session, machine);
                                return true;
                            }
                            );

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

        private static List<GpxReader.Trk> GetGpxTracks(Session session)
        {
            var xmlString = File.ReadAllText(session.LogicSettings.GpxFile);
            var readgpx = new GpxReader(xmlString, session);
            return readgpx.Tracks;
        }

        //Please do not change GetPokeStops() in this file, it's specifically set
        //to only find stops within 40 meters
        //this is for gpx pathing, we are not going to the pokestops,
        //so do not make it more than 40 because it will never get close to those stops.
        private static async Task<List<FortData>> GetPokeStops(Session session)
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