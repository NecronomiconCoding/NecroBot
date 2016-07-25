using POGOProtos.Map.Fort;
using PokemonGo.RocketAPI.Extensions;
using PokemonGo.RocketAPI.Logic.Event;
using PokemonGo.RocketAPI.Logic.State;
using PokemonGo.RocketAPI.Logic.Utils;
using PokemonGo.RocketAPI.Logic.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace PokemonGo.RocketAPI.Logic.Tasks
{
    public static class FarmPokestopsGPXTask
    {
        private static List<FortData> GetPokeStops(Context ctx)
        {
            var mapObjects = ctx.Client.Map.GetMapObjects().Result;

            // Wasn't sure how to make this pretty. Edit as needed.
            var pokeStops = mapObjects.MapCells.SelectMany(i => i.Forts)
                    .Where(
                        i =>
                            i.Type == FortType.Checkpoint &&
                            i.CooldownCompleteTimestampMs < DateTime.UtcNow.ToUnixTime() &&
                            ( // Make sure PokeStop is within max travel distance, unless it's set to 0.
                                LocationUtils.CalculateDistanceInMeters(
                                    ctx.Settings.DefaultLatitude, ctx.Settings.DefaultLongitude,
                                    i.Latitude, i.Longitude) < ctx.Settings.MaxTravelDistanceInMeters) ||
                            ctx.Settings.MaxTravelDistanceInMeters == 0
                    );

            return pokeStops.ToList();
        }

        private static List<GpxReader.Trk> GetGpxTracks(Context ctx)
        {
            var xmlString = File.ReadAllText(ctx.Settings.GPXFile);
            var readgpx = new GpxReader(xmlString);
            return readgpx.Tracks;
        }

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
                        var distance = LocationUtils.CalculateDistanceInMeters(ctx.Client.CurrentLatitude, ctx.Client.CurrentLongitude, Convert.ToDouble(nextPoint.Lat), Convert.ToDouble(nextPoint.Lon));

                        if (distance > 5000)
                        {
                            Logger.Write($"Your desired destination of {nextPoint.Lat}, {nextPoint.Lon} is too far from your current position of {ctx.Client.CurrentLatitude}, {ctx.Client.CurrentLongitude}", LogLevel.Error);
                            break;
                        }

                        Logger.Write($"Your desired destination is {nextPoint.Lat}, {nextPoint.Lon} your location is {ctx.Client.CurrentLatitude}, {ctx.Client.CurrentLongitude}", LogLevel.Warning);

                        var pokestopList = GetPokeStops(ctx);

                        while (pokestopList.Any())
                        {
                            pokestopList = pokestopList.OrderBy(i => LocationUtils.CalculateDistanceInMeters(ctx.Client.CurrentLatitude, ctx.Client.CurrentLongitude, i.Latitude, i.Longitude)).ToList();
                            var pokeStop = pokestopList[0];
                            pokestopList.RemoveAt(0);

                            ctx.Client.Fort.GetFort(pokeStop.Id, pokeStop.Latitude, pokeStop.Longitude).Wait();

                            var fortSearch = ctx.Client.Fort.SearchFort(pokeStop.Id, pokeStop.Latitude, pokeStop.Longitude).Result;
                            if (fortSearch.ExperienceAwarded > 0)
                            {
                                machine.Fire(new FortUsedEvent { Exp = fortSearch.ExperienceAwarded, Gems = fortSearch.GemsAwarded, Items = StringUtils.GetSummedFriendlyNameOfItemAwardList(fortSearch.ItemsAwarded) });

                            }

                            Thread.Sleep(1000);

                            RecycleItemsTask.Execute(ctx, machine);

                            if (ctx.Settings.TransferDuplicatePokemon)
                            {
                                TransferDuplicatePokemonTask.Execute(ctx, machine);
                            }
                        }

                        ctx.Navigation.HumanPathWalking(trackPoints.ElementAt(curTrkPt), ctx.Settings.WalkingSpeedInKilometerPerHour, () =>
                        {
                            CatchNearbyPokemonsTask.Execute(ctx, machine);
                            return true;
                        }).Wait();

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
    }
}
