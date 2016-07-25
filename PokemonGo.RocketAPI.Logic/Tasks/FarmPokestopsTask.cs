using POGOProtos.Map.Fort;
using PokemonGo.RocketAPI.Extensions;
using PokemonGo.RocketAPI.Logic.Event;
using PokemonGo.RocketAPI.Logic.State;
using PokemonGo.RocketAPI.Logic.Utils;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Threading;

namespace PokemonGo.RocketAPI.Logic.Tasks
{
    public static class FarmPokestopsTask
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

        public static void Execute(Context ctx, StateMachine machine)
        {
            var distanceFromStart = LocationUtils.CalculateDistanceInMeters(
                ctx.Settings.DefaultLatitude, ctx.Settings.DefaultLongitude,
                ctx.Client.CurrentLat, ctx.Client.CurrentLng);

            // Edge case for when the client somehow ends up outside the defined radius
            if (ctx.Settings.MaxTravelDistanceInMeters != 0 &&
                distanceFromStart > ctx.Settings.MaxTravelDistanceInMeters)
            {
                Logger.Write(
                    $"You're outside of your defined radius! Walking to start ({distanceFromStart}m away) in 5 seconds. Is your Coords.ini file correct?",
                    LogLevel.Warning);

                Thread.Sleep(5000);

                Logger.Write("Moving to start location now.");
                ctx.Navigation.HumanLikeWalking(new GeoCoordinate(ctx.Settings.DefaultLatitude, ctx.Settings.DefaultLongitude),
                    ctx.Settings.WalkingSpeedInKilometerPerHour, null).Wait();
            }

            var pokestopList = GetPokeStops(ctx);
            var stopsHit = 0;

            if (pokestopList.Count <= 0)
            {
                Logger.Write("No usable PokeStops found in your area. Is your maximum distance too small?", LogLevel.Warning);
            }

            while (pokestopList.Any())
            {
                //resort
                pokestopList = pokestopList.OrderBy(i => LocationUtils.CalculateDistanceInMeters(ctx.Client.CurrentLat, ctx.Client.CurrentLng, i.Latitude, i.Longitude)).ToList();
                var pokeStop = pokestopList[0];
                pokestopList.RemoveAt(0);

                var distance = LocationUtils.CalculateDistanceInMeters(ctx.Client.CurrentLat, ctx.Client.CurrentLng, pokeStop.Latitude, pokeStop.Longitude);
                var fortInfo = ctx.Client.Fort.GetFort(pokeStop.Id, pokeStop.Latitude, pokeStop.Longitude).Result;

                machine.Fire(new FortTargetEvent { Name= fortInfo.Name, Distance = distance });

                ctx.Navigation.HumanLikeWalking(new GeoCoordinate(pokeStop.Latitude, pokeStop.Longitude), ctx.Settings.WalkingSpeedInKilometerPerHour, 
                    () =>
                    {
                        CatchNearbyPokemonsTask.Execute(ctx, machine);
                        return true;
                    }).Wait();

                var fortSearch = ctx.Client.Fort.SearchFort(pokeStop.Id, pokeStop.Latitude, pokeStop.Longitude).Result;
                if (fortSearch.ExperienceAwarded > 0)
                {
                    machine.Fire(new FortUsedEvent { Exp = fortSearch.ExperienceAwarded, Gems = fortSearch.GemsAwarded, Items = StringUtils.GetSummedFriendlyNameOfItemAwardList(fortSearch.ItemsAwarded) });
                }

                Thread.Sleep(1000);
                if (++stopsHit % 5 == 0) //TODO: OR item/pokemon bag is full
                {
                    stopsHit = 0;
                    RecycleItemsTask.Execute(ctx, machine);
                    if (ctx.Settings.EvolveAllPokemonWithEnoughCandy || ctx.Settings.EvolveAllPokemonAboveIV)
                    {
                        EvolvePokemonTask.Execute(ctx, machine);
                    }
                    if (ctx.Settings.TransferDuplicatePokemon)
                    {
                        TransferDuplicatePokemonTask.Execute(ctx, machine);
                    }
                }
            }
        }
    }
}
