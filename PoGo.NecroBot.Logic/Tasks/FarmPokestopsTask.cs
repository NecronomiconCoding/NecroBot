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
        private static readonly Random RandomDevice = new Random();
        public static int TimesZeroXPawarded;
        private static int storeRI;
        private static int RandomNumber;

        public static async Task Execute(ISession session, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var distanceFromStart = LocationUtils.CalculateDistanceInMeters(
                session.Settings.DefaultLatitude, session.Settings.DefaultLongitude,
                session.Client.CurrentLatitude, session.Client.CurrentLongitude);

            // Edge case for when the client somehow ends up outside the defined radius
            if (session.LogicSettings.MaxTravelDistanceInMeters != 0 &&
                distanceFromStart > session.LogicSettings.MaxTravelDistanceInMeters)
            {
                
                Logger.Write(
                    session.Translation.GetTranslation(TranslationString.FarmPokestopsOutsideRadius, distanceFromStart),
                    LogLevel.Warning);
                
                await session.Navigation.Move(
                    new GeoCoordinate(session.Settings.DefaultLatitude, session.Settings.DefaultLongitude, LocationUtils.getElevation(session.Settings.DefaultLatitude, session.Settings.DefaultLongitude)),
                    session.LogicSettings.WalkingSpeedInKilometerPerHour, null, cancellationToken, session.LogicSettings.DisableHumanWalking,
                    session.LogicSettings.UseWalkingSpeedVariant);
            }

            var pokestopList = await GetPokeStops(session);
            var stopsHit = 0;
            var RandomStop = 0;
            var rc = new Random(); //initialize pokestop random cleanup counter first time
            storeRI = rc.Next(8, 15);
            RandomNumber = rc.Next(4, 11);
            
            var eggWalker = new EggWalker(1000, session);

            if (pokestopList.Count <= 0)
            {
                session.EventDispatcher.Send(new WarnEvent
                {
                    Message = session.Translation.GetTranslation(TranslationString.FarmPokestopsNoUsableFound)
                });
            }

            session.EventDispatcher.Send(new PokeStopListEvent {Forts = pokestopList});

            while (pokestopList.Any())
            {
                cancellationToken.ThrowIfCancellationRequested();

                //resort
                pokestopList =
                    pokestopList.OrderBy(
                        i =>
                            LocationUtils.CalculateDistanceInMeters(session.Client.CurrentLatitude,
                                session.Client.CurrentLongitude, i.Latitude, i.Longitude)).ToList();

                // randomize next pokestop between first and second by distance
                var pokestopListNum = 0;
                if (pokestopList.Count >= 1)
                {
                    pokestopListNum = rc.Next(0, 2);
                }
                var pokeStop = pokestopList[pokestopListNum];
                pokestopList.RemoveAt(pokestopListNum);

                var distance = LocationUtils.CalculateDistanceInMeters(session.Client.CurrentLatitude,
                    session.Client.CurrentLongitude, pokeStop.Latitude, pokeStop.Longitude);
                var fortInfo = await session.Client.Fort.GetFort(pokeStop.Id, pokeStop.Latitude, pokeStop.Longitude);
                double WalkingSpeed = session.LogicSettings.WalkingSpeedInKilometerPerHour;
                var randomFactor = 0.5f;
                var randomMin = (int)(WalkingSpeed * (1 - randomFactor));
                var randomMax = (int)(WalkingSpeed * (1 + randomFactor));
                var RandomWalkSpeed = RandomDevice.Next(randomMin, randomMax);
                cancellationToken.ThrowIfCancellationRequested();
                session.EventDispatcher.Send(new FortTargetEvent {Name = fortInfo.Name, Distance = distance});

                    await session.Navigation.Move(new GeoCoordinate(pokeStop.Latitude, pokeStop.Longitude, LocationUtils.getElevation(pokeStop.Latitude, pokeStop.Longitude)),
                    RandomWalkSpeed,
                    async () =>
                    {
                        // Catch normal map Pokemon
                        await CatchNearbyPokemonsTask.Execute(session, cancellationToken);
                        //Catch Incense Pokemon
                        await CatchIncensePokemonsTask.Execute(session, cancellationToken);
                        return true;
                    }, cancellationToken, session.LogicSettings.DisableHumanWalking,
                    session.LogicSettings.UseWalkingSpeedVariant);

                //Catch Lure Pokemon
                if (pokeStop.LureInfo != null)
                {
                    await CatchLurePokemonsTask.Execute(session, pokeStop, cancellationToken);
                }

                FortSearchResponse fortSearch;
                var timesZeroXPawarded = 0;
                var fortTry = 0; //Current check
                const int retryNumber = 50; //How many times it needs to check to clear softban
                const int zeroCheck = 5; //How many times it checks fort before it thinks it's softban
                do
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    fortSearch =
                        await session.Client.Fort.SearchFort(pokeStop.Id, pokeStop.Latitude, pokeStop.Longitude);
                    if (fortSearch.ExperienceAwarded > 0 && timesZeroXPawarded > 0) timesZeroXPawarded = 0;
                    if (fortSearch.ExperienceAwarded == 0)
                    {
                        timesZeroXPawarded++;

                        if (timesZeroXPawarded > zeroCheck)
                        {
                            if ((int) fortSearch.CooldownCompleteTimestampMs != 0)
                            {
                                break; // Check if successfully looted, if so program can continue as this was "false alarm".
                            }

                            fortTry += 1;

                            session.EventDispatcher.Send(new FortFailedEvent
                            {
                                Name = fortInfo.Name,
                                Try = fortTry,
                                Max = retryNumber - zeroCheck,
                                Looted = false
                            });

                            if (!session.LogicSettings.FastSoftBanBypass)
                            {
                                DelayingUtils.Delay(session.LogicSettings.DelayBetweenPlayerActions, 0);
                            }
                        }
                    }
                    else
                    {
                        if (fortTry != 0)
                        {
                            session.EventDispatcher.Send(new FortFailedEvent
                            {
                                Name = fortInfo.Name,
                                Try = fortTry + 1,
                                Max = retryNumber - zeroCheck,
                                Looted = true
                            });
                        }

                        session.EventDispatcher.Send(new FortUsedEvent
                        {
                            Id = pokeStop.Id,
                            Name = fortInfo.Name,
                            Exp = fortSearch.ExperienceAwarded,
                            Gems = fortSearch.GemsAwarded,
                            Items = StringUtils.GetSummedFriendlyNameOfItemAwardList(fortSearch.ItemsAwarded),
                            Latitude = pokeStop.Latitude,
                            Longitude = pokeStop.Longitude,
                            InventoryFull = fortSearch.Result == FortSearchResponse.Types.Result.InventoryFull
                        });

                        if ( fortSearch.Result == FortSearchResponse.Types.Result.InventoryFull )
                            storeRI = 1;

                        break; //Continue with program as loot was succesfull.
                    }
                } while (fortTry < retryNumber - zeroCheck);
                //Stop trying if softban is cleaned earlier or if 40 times fort looting failed.

                await eggWalker.ApplyDistance(distance, cancellationToken);
                if (session.LogicSettings.RandomlyPauseAtStops)
                {
                    if (++RandomStop >= RandomNumber)
                    {
                        RandomNumber = rc.Next(4, 11);
                        RandomStop = 0;
                        int RandomWaitTime = rc.Next(30, 120);
                        Thread.Sleep(RandomWaitTime);
                    }
                }
              
                if (++stopsHit >= storeRI) //TODO: OR item/pokemon bag is full //check stopsHit against storeRI random without dividing.
                {
                    storeRI = rc.Next(6, 12); //set new storeRI for new random value
                    stopsHit = 0;

                    await RecycleItemsTask.Execute(session, cancellationToken);

                    if (session.LogicSettings.EvolveAllPokemonWithEnoughCandy ||
                        session.LogicSettings.EvolveAllPokemonAboveIv ||
                        session.LogicSettings.UseLuckyEggsWhileEvolving ||
                        session.LogicSettings.KeepPokemonsThatCanEvolve)
                    {
                        await EvolvePokemonTask.Execute(session, cancellationToken);
                    }

                    if (session.LogicSettings.UseLuckyEggConstantly)
                        await UseLuckyEggConstantlyTask.Execute(session, cancellationToken);

                    if (session.LogicSettings.UseIncenseConstantly)
                        await UseIncenseConstantlyTask.Execute(session, cancellationToken);

                    if (session.LogicSettings.TransferDuplicatePokemon)
                        await TransferDuplicatePokemonTask.Execute(session, cancellationToken);

                    if (session.LogicSettings.TransferWeakPokemon)
                        await TransferWeakPokemonTask.Execute(session, cancellationToken);

                    if (session.LogicSettings.RenamePokemon)
                        await RenamePokemonTask.Execute(session, cancellationToken);

                    if (session.LogicSettings.AutoFavoritePokemon)
                        await FavoritePokemonTask.Execute(session, cancellationToken);

                    if (session.LogicSettings.AutomaticallyLevelUpPokemon)
                        await LevelUpPokemonTask.Execute(session, cancellationToken);

                    await GetPokeDexCount.Execute(session, cancellationToken);
                }

                if (session.LogicSettings.SnipeAtPokestops || session.LogicSettings.UseSnipeLocationServer)
                    await SnipePokemonTask.Execute(session, cancellationToken);
            }
        }

        private static async Task<List<FortData>> GetPokeStops(ISession session)
        {
            var mapObjects = await session.Client.Map.GetMapObjects();

            // Wasn't sure how to make this pretty. Edit as needed.
            var pokeStops = mapObjects.Item1.MapCells.SelectMany(i => i.Forts)
                .Where(
                    i =>
                        i.Type == FortType.Checkpoint &&
                        i.CooldownCompleteTimestampMs < DateTime.UtcNow.ToUnixTime() &&
                        ( // Make sure PokeStop is within max travel distance, unless it's set to 0.
                            LocationUtils.CalculateDistanceInMeters(
                                session.Settings.DefaultLatitude, session.Settings.DefaultLongitude,
                                i.Latitude, i.Longitude) < session.LogicSettings.MaxTravelDistanceInMeters ||
                        session.LogicSettings.MaxTravelDistanceInMeters == 0) 
                );

            return pokeStops.ToList();
        }
    }
}
