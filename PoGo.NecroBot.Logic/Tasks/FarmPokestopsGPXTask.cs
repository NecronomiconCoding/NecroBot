#region using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Utils;
using PokemonGo.RocketAPI.Extensions;
using POGOProtos.Map.Fort;
using System.Globalization;

#endregion

namespace PoGo.NecroBot.Logic.Tasks
{
    public static class FarmPokestopsGpxTask
    {
        public static void Execute(Context ctx, StateMachine machine)
        {
            var tracks = GetGpxTracks(ctx);
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
                        var distance = LocationUtils.CalculateDistanceInMeters(ctx.Client.CurrentLatitude,
                            ctx.Client.CurrentLongitude, Convert.ToDouble(nextPoint.Lat, CultureInfo.InvariantCulture),
                            Convert.ToDouble(nextPoint.Lon, CultureInfo.InvariantCulture));

                        if (distance > 5000)
                        {
                            machine.Fire(new ErrorEvent
                            {
                                Message =
                                    $"Your desired destination of {nextPoint.Lat}, {nextPoint.Lon} is too far from your current position of {ctx.Client.CurrentLatitude}, {ctx.Client.CurrentLongitude}"
                            });
                            break;
                        }
                        var pokestopList = GetPokeStops(ctx);

                        while (pokestopList.Any())
                        {
                            pokestopList =
                                pokestopList.OrderBy(
                                    i =>
                                        LocationUtils.CalculateDistanceInMeters(ctx.Client.CurrentLatitude,
                                            ctx.Client.CurrentLongitude, i.Latitude, i.Longitude)).ToList();
                            var pokeStop = pokestopList[0];
                            pokestopList.RemoveAt(0);

                            ctx.Client.Fort.GetFort(pokeStop.Id, pokeStop.Latitude, pokeStop.Longitude).Wait();

                            var fortSearch =
                                ctx.Client.Fort.SearchFort(pokeStop.Id, pokeStop.Latitude, pokeStop.Longitude).Result;

                            if (fortSearch.ExperienceAwarded > 0)
                            {
                                machine.Fire(new FortUsedEvent
                                {
                                    Exp = fortSearch.ExperienceAwarded,
                                    Gems = fortSearch.GemsAwarded,
                                    Items = StringUtils.GetSummedFriendlyNameOfItemAwardList(fortSearch.ItemsAwarded)
                                });
                            }
                            if (fortSearch.ItemsAwarded.Count > 0)
                            {
                                var refreshCachedInventory = ctx.Inventory.RefreshCachedInventory();
                            }

                            Thread.Sleep(1000);

                            RecycleItemsTask.Execute(ctx, machine);

                            if (ctx.LogicSettings.EvolveAllPokemonWithEnoughCandy || ctx.LogicSettings.EvolveAllPokemonAboveIv)
                            {
                                EvolvePokemonTask.Execute(ctx, machine);
                            }

                            if (ctx.LogicSettings.TransferDuplicatePokemon)
                            {
                                TransferDuplicatePokemonTask.Execute(ctx, machine);
                            }
                        }

                        ctx.Navigation.HumanPathWalking(trackPoints.ElementAt(curTrkPt),
                            ctx.LogicSettings.WalkingSpeedInKilometerPerHour, () =>
                            {

                                CatchNearbyPokemonsTask.Execute(ctx, machine);
                                UseNearbyPokestopsTask.Execute(ctx, machine);
                                return true;
                            }
                            ).Wait();

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

        private static List<GpxReader.Trk> GetGpxTracks(Context ctx)
        {
            var xmlString = File.ReadAllText(ctx.LogicSettings.GpxFile);
            var readgpx = new GpxReader(xmlString);
            return readgpx.Tracks;
        }
        //Please do not change GetPokeStops() in this file, it's specifically set
        //to only find stops within 40 meters
        //this is for gpx pathing, we are not going to the pokestops,
        //so do not make it more than 40 because it will never get close to those stops.
        private static List<FortData> GetPokeStops(Context ctx)
        {
            var mapObjects = ctx.Client.Map.GetMapObjects().Result;

            // Wasn't sure how to make this pretty. Edit as needed.
            var pokeStops = mapObjects.MapCells.SelectMany(i => i.Forts)
                .Where(
                    i =>
                        i.Type == FortType.Checkpoint &&
                        i.CooldownCompleteTimestampMs < DateTime.UtcNow.ToUnixTime() &&
                        ( // Make sure PokeStop is within 40 meters or else it is pointless to hit it
                            LocationUtils.CalculateDistanceInMeters(
                                ctx.Client.CurrentLatitude, ctx.Client.CurrentLongitude,
                                i.Latitude, i.Longitude) < 40) ||
                        ctx.LogicSettings.MaxTravelDistanceInMeters == 0
                );

            return pokeStops.ToList();
        }
    }
}
