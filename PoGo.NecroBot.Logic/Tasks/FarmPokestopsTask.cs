#region using directives

using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Threading;
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
        public static void Execute(Context ctx, StateMachine machine)
        {
            var distanceFromStart = LocationUtils.CalculateDistanceInMeters(
                ctx.Settings.DefaultLatitude, ctx.Settings.DefaultLongitude,
                ctx.Client.CurrentLatitude, ctx.Client.CurrentLongitude);

            // Edge case for when the client somehow ends up outside the defined radius
            if (ctx.LogicSettings.MaxTravelDistanceInMeters != 0 &&
                distanceFromStart > ctx.LogicSettings.MaxTravelDistanceInMeters)
            {
                Logger.Write(
                    $"You're outside of your defined radius! Walking to start ({distanceFromStart}m away) in 5 seconds. Is your Coords.ini file correct?",
                    LogLevel.Warning);

                Thread.Sleep(5000);

                ctx.Navigation.HumanLikeWalking(
                    new GeoCoordinate(ctx.Settings.DefaultLatitude, ctx.Settings.DefaultLongitude),
                    ctx.LogicSettings.WalkingSpeedInKilometerPerHour, null).Wait();
            }

            var pokestopList = GetPokeStops(ctx);
            var stopsHit = 0;

            if (pokestopList.Count <= 0)
            {
                Logger.Write("No usable PokeStops found in your area. Is your maximum distance too small?",
                    LogLevel.Warning);
            }

            while (pokestopList.Any())
            {
                //resort
                pokestopList =
                    pokestopList.OrderBy(
                        i =>
                            LocationUtils.CalculateDistanceInMeters(ctx.Client.CurrentLatitude,
                                ctx.Client.CurrentLongitude, i.Latitude, i.Longitude)).ToList();
                var pokeStop = pokestopList[0];
                pokestopList.RemoveAt(0);

                var distance = LocationUtils.CalculateDistanceInMeters(ctx.Client.CurrentLatitude,
                    ctx.Client.CurrentLongitude, pokeStop.Latitude, pokeStop.Longitude);
                var fortInfo = ctx.Client.Fort.GetFort(pokeStop.Id, pokeStop.Latitude, pokeStop.Longitude).Result;

                machine.Fire(new FortTargetEvent {Name = fortInfo.Name, Distance = distance});

                ctx.Navigation.HumanLikeWalking(new GeoCoordinate(pokeStop.Latitude, pokeStop.Longitude),
                    ctx.LogicSettings.WalkingSpeedInKilometerPerHour,
                    () =>
                    {
                        CatchNearbyPokemonsTask.Execute(ctx, machine);
                        return true;
                    }).Wait();

                var fortSearch = ctx.Client.Fort.SearchFort(pokeStop.Id, pokeStop.Latitude, pokeStop.Longitude).Result;
                if (fortSearch.ExperienceAwarded > 0)
                {
                    machine.Fire(new FortUsedEvent
                    {
                        Exp = fortSearch.ExperienceAwarded,
                        Gems = fortSearch.GemsAwarded,
                        Items = StringUtils.GetSummedFriendlyNameOfItemAwardList(fortSearch.ItemsAwarded)
                    });
                }


                Thread.Sleep(1000);

                CatchLuredPokemonTask.Execute(ctx, machine, pokeStop);

                if (++stopsHit%5 == 0) //TODO: OR item/pokemon bag is full
                {
                    stopsHit = 0;
                    if (fortSearch.ItemsAwarded.Count > 0)
                    {
                        var refreshCachedInventory = ctx.Inventory.RefreshCachedInventory();
                    }
                    RenamePokemonTask.Execute(ctx, machine);
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
            }
        }

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
                                i.Latitude, i.Longitude) < ctx.LogicSettings.MaxTravelDistanceInMeters) ||
                        ctx.LogicSettings.MaxTravelDistanceInMeters == 0
                );

            return pokeStops.ToList();
        }
    }
}