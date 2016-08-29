#region using directives

using System;
using System.Collections.Generic;
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
using POGOProtos.Networking.Responses;
using PoGo.NecroBot.Logic.Strategies.Walk;
using PoGo.NecroBot.Logic.Logging;

#endregion

namespace PoGo.NecroBot.Logic.Tasks
{
    public class UseNearbyPokestopsTask
    {
        private static int stopsHit;
        private static int RandomStop;
        private static Random rc; //initialize pokestop random cleanup counter first time
        private static int storeRI;
        private static int RandomNumber;
        private static List<FortData> pokestopList;

        internal static void Initialize()
        {
            stopsHit = 0;
            RandomStop = 0;
            rc = new Random();
            storeRI = rc.Next(8, 15);
            RandomNumber = rc.Next(4, 11);
            pokestopList = new List<FortData>();
        }

        private static bool SearchThresholdExceeds(ISession session)
        {
            if (!session.LogicSettings.UsePokeStopLimit) return false;
            if (session.Stats.PokeStopTimestamps.Count >= session.LogicSettings.PokeStopLimit)
            {
                // delete uesless data
                int toRemove = session.Stats.PokeStopTimestamps.Count - session.LogicSettings.PokeStopLimit;
                if (toRemove > 0)
                {
                    session.Stats.PokeStopTimestamps.RemoveRange(0, toRemove);
                }
                var sec = (DateTime.Now - new DateTime(session.Stats.PokeStopTimestamps.First())).TotalSeconds;
                var limit = session.LogicSettings.PokeStopLimitMinutes * 60;
                if (sec < limit)
                {
                    session.EventDispatcher.Send(new ErrorEvent { Message = session.Translation.GetTranslation(TranslationString.PokeStopExceeds, Math.Round(limit - sec)) });
                    return true;
                }
            }

            Logger.Write($"(POKESTOP LIMIT) {session.Stats.PokeStopTimestamps.Count}/{session.LogicSettings.PokeStopLimit}",
                LogLevel.Info, ConsoleColor.Yellow);
            return false;
        }

        public static async Task Execute(ISession session, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var pokestopsTuple = await GetPokeStops(session);
            pokestopList = pokestopsTuple.Item2;

            while (pokestopList.Any())
            {
                cancellationToken.ThrowIfCancellationRequested();
                await SnipeMSniperTask.CheckMSniperLocation(session, cancellationToken);

                pokestopList =
                    pokestopList.OrderBy(
                        i =>
                            LocationUtils.CalculateDistanceInMeters(session.Client.CurrentLatitude,
                                session.Client.CurrentLongitude, i.Latitude, i.Longitude)).ToList();

                var pokeStop = pokestopList[0];
                pokestopList.RemoveAt(0);

                // this logic should only be called when we reach a pokestop either via GPX path or normal walking
                // as when walk-sniping, we want to get to the snipe ASAP rather than stop for lured pokemon upon
                // calling FarmPokestop; in that situation we are also always within 40m of the pokestop, so no
                // need to walk to it
                var fortInfo = await session.Client.Fort.GetFort(pokeStop.Id, pokeStop.Latitude, pokeStop.Longitude);

                // we only move to the PokeStop, and send the associated FortTargetEvent, when not using GPX
                // also, GPX pathing uses its own EggWalker and calls the CatchPokemon tasks internally.
                if (!session.LogicSettings.UseGpxPathing)
                {
                    var eggWalker = new EggWalker(1000, session);

                    var distance = LocationUtils.CalculateDistanceInMeters(session.Client.CurrentLatitude,
                        session.Client.CurrentLongitude, pokeStop.Latitude, pokeStop.Longitude);
                    cancellationToken.ThrowIfCancellationRequested();

                    if (!session.LogicSettings.UseGoogleWalk && !session.LogicSettings.UseYoursWalk)
                        session.EventDispatcher.Send(new FortTargetEvent { Name = fortInfo.Name, Distance = distance, Route = "NecroBot" });
                    else
                        BaseWalkStrategy.FortInfo = fortInfo;

                    await session.Navigation.Move(new GeoCoordinate(pokeStop.Latitude, pokeStop.Longitude,
                        LocationUtils.getElevation(session, pokeStop.Latitude, pokeStop.Longitude)),
                    async () =>
                    {
                        // Catch normal map Pokemon
                        await CatchNearbyPokemonsTask.Execute(session, cancellationToken);
                        //Catch Incense Pokemon
                        await CatchIncensePokemonsTask.Execute(session, cancellationToken);
                        // Minor fix google route ignore pokestop
                        await LookPokestops(session, pokeStop, cancellationToken);
                        return true;
                    },
                    session,
                    cancellationToken);

                    // we have moved this distance, so apply it immediately to the egg walker.
                    await eggWalker.ApplyDistance(distance, cancellationToken);
                }

                await FortAction(session, pokeStop, fortInfo, cancellationToken);

                if (session.LogicSettings.SnipeAtPokestops || session.LogicSettings.UseSnipeLocationServer)
                    await SnipePokemonTask.Execute(session, cancellationToken);

                if (session.LogicSettings.EnableHumanWalkingSnipe)
                {
                    //refactore to move this code inside the task later.
                    await HumanWalkSnipeTask.Execute(session, cancellationToken,
                        async (double lat, double lng) =>
                        {
                            //idea of this function is to spin pokestop on way. maybe risky.
                            var reachablePokestops = pokestopList.Where(i =>
                                LocationUtils.CalculateDistanceInMeters(session.Client.CurrentLatitude,
                                    session.Client.CurrentLongitude, i.Latitude, i.Longitude) < 40
                                    && i.CooldownCompleteTimestampMs == 0
                                    ).ToList();
                            reachablePokestops = reachablePokestops.OrderBy(i =>
                            LocationUtils.CalculateDistanceInMeters(session.Client.CurrentLatitude,
                            session.Client.CurrentLongitude, i.Latitude, i.Longitude)).ToList();

                            foreach (var ps in reachablePokestops)
                            {
                                if (!session.LogicSettings.UseGpxPathing)
                                    pokestopList.Remove(ps);
                                var fi = await session.Client.Fort.GetFort(ps.Id, ps.Latitude, ps.Longitude);
                                await FarmPokestop(session, ps, fi, cancellationToken, true);
                                await Task.Delay(1000);
                            }
                        },
                        async () =>
                        {
                            // if using GPX we have to move back to the original pokestop, to resume the path.
                            // we do not try to use pokest;ops on the way back, as we will have used them getting
                            // here.
                            if (session.LogicSettings.UseGpxPathing)
                            {
                                var eggWalker = new EggWalker(1000, session);

                                var distance = LocationUtils.CalculateDistanceInMeters(session.Client.CurrentLatitude,
                                    session.Client.CurrentLongitude, pokeStop.Latitude, pokeStop.Longitude);
                                var geo = new GeoCoordinate(pokeStop.Latitude, pokeStop.Longitude);

                                await session.Navigation.Move(geo,
                                    async () =>
                                    {
                                        await CatchNearbyPokemonsTask.Execute(session, cancellationToken);
                                        //Catch Incense Pokemon
                                        await CatchIncensePokemonsTask.Execute(session, cancellationToken);
                                        return true;
                                    },
                                    session,
                                    cancellationToken);

                                await eggWalker.ApplyDistance(distance, cancellationToken);
                                return;
                            }

                            var nearestStop = pokestopList.OrderBy(i =>
                                LocationUtils.CalculateDistanceInMeters(session.Client.CurrentLatitude,
                                    session.Client.CurrentLongitude, i.Latitude, i.Longitude)).FirstOrDefault();

                            var walkedDistance = LocationUtils.CalculateDistanceInMeters(nearestStop.Latitude, nearestStop.Longitude, session.Client.CurrentLatitude, session.Client.CurrentLongitude);
                            if (walkedDistance > session.LogicSettings.HumanWalkingSnipeWalkbackDistanceLimit)
                            {
                                await Task.Delay(3000);
                                var nearbyPokeStops = await UpdateFortsData(session);
                                var notexists = nearbyPokeStops.Where(p => !pokestopList.Any(x => x.Id == p.Id)).ToList();
                                pokestopList.AddRange(notexists);
                                session.EventDispatcher.Send(new PokeStopListEvent { Forts = pokestopList });
                                session.EventDispatcher.Send(new HumanWalkSnipeEvent()
                                {
                                    Type = HumanWalkSnipeEventTypes.PokestopUpdated,
                                    Pokestops = notexists,
                                    NearestDistance = walkedDistance
                                });
                            }
                        });
                }
            }
        }

        private static async Task LookPokestops(ISession session, FortData currentPokestop, CancellationToken cancellationToken)
        {
            if (!session.LogicSettings.UseGoogleWalk && !session.LogicSettings.UseYoursWalk)
                return;

            if (pokestopList.Count > 1)
            {
                var currentPokestopDistance = LocationUtils.CalculateDistanceInMeters(
                                session.Client.CurrentLatitude, session.Client.CurrentLongitude,
                                currentPokestop.Latitude, currentPokestop.Longitude);
                var _pokeStopList = pokestopList.Where(
                    i =>
                        (
                            LocationUtils.CalculateDistanceInMeters(
                                session.Client.CurrentLatitude, session.Client.CurrentLongitude,
                                i.Latitude, i.Longitude) < 40 && currentPokestopDistance >= 40) ||
                        session.LogicSettings.MaxTravelDistanceInMeters == 0
                ).ToList();

                if (_pokeStopList.Count >= 1)
                {
                    foreach (var pokeStop in _pokeStopList)
                    {
                        var fortInfo = await session.Client.Fort.GetFort(pokeStop.Id, pokeStop.Latitude, pokeStop.Longitude);
                        await FortAction(session, pokeStop, fortInfo, cancellationToken);
                        pokestopList.Remove(pokeStop);
                    }
                }
            }
        }

        private static async Task FortAction(ISession session, FortData pokeStop, FortDetailsResponse fortInfo, CancellationToken cancellationToken)
        {
            //Catch Lure Pokemon
            if (pokeStop.LureInfo != null)
            {
                // added for cooldowns
                await Task.Delay(Math.Min(session.LogicSettings.DelayBetweenPlayerActions, 3000));
                await CatchLurePokemonsTask.Execute(session, pokeStop, cancellationToken);
            }

            await FarmPokestop(session, pokeStop, fortInfo, cancellationToken);

            if (++stopsHit >= storeRI) //TODO: OR item/pokemon bag is full //check stopsHit against storeRI random without dividing.
            {
                storeRI = rc.Next(6, 12); //set new storeRI for new random value
                stopsHit = 0;

                if (session.LogicSettings.UseNearActionRandom)
                {
                    await HumanRandomActionTask.Execute(session, cancellationToken);
                }
                else
                {
                    await RecycleItemsTask.Execute(session, cancellationToken);

                    if (session.LogicSettings.EvolveAllPokemonWithEnoughCandy ||
                        session.LogicSettings.EvolveAllPokemonAboveIv ||
                        session.LogicSettings.UseLuckyEggsWhileEvolving ||
                        session.LogicSettings.KeepPokemonsThatCanEvolve)
                        await EvolvePokemonTask.Execute(session, cancellationToken);
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
            }
        }

        private static async Task FarmPokestop(ISession session, FortData pokeStop, FortDetailsResponse fortInfo, CancellationToken cancellationToken, bool doNotRetry = false)
        {
            FortSearchResponse fortSearch;
            var timesZeroXPawarded = 0;
            var fortTry = 0; //Current check
            const int retryNumber = 50; //How many times it needs to check to clear softban
            const int zeroCheck = 5; //How many times it checks fort before it thinks it's softban
            do
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (SearchThresholdExceeds(session))
                {
                    break;
                }

                fortSearch =
                    await session.Client.Fort.SearchFort(pokeStop.Id, pokeStop.Latitude, pokeStop.Longitude);
                if (fortSearch.ExperienceAwarded > 0 && timesZeroXPawarded > 0) timesZeroXPawarded = 0;
                if (fortSearch.ExperienceAwarded == 0)
                {
                    timesZeroXPawarded++;

                    if (timesZeroXPawarded > zeroCheck)
                    {
                        if ((int)fortSearch.CooldownCompleteTimestampMs != 0)
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
                        if (doNotRetry)
                        {
                            break;
                        }
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
                        Altitude = session.Client.CurrentAltitude,
                        InventoryFull = fortSearch.Result == FortSearchResponse.Types.Result.InventoryFull
                    });

                    if (fortSearch.Result == FortSearchResponse.Types.Result.InventoryFull)
                        storeRI = 1;

                    session.Stats.PokeStopTimestamps.Add(DateTime.Now.Ticks);
                    break; //Continue with program as loot was succesfull.
                }
            } while (fortTry < retryNumber - zeroCheck);
            //Stop trying if softban is cleaned earlier or if 40 times fort looting failed.

            if (session.LogicSettings.RandomlyPauseAtStops)
            {
                if (++RandomStop >= RandomNumber)
                {
                    RandomNumber = rc.Next(4, 11);
                    RandomStop = 0;
                    int RandomWaitTime = rc.Next(30, 120);
                    await Task.Delay(RandomWaitTime);
                }
            }

        }

        //Please do not change GetPokeStops() in this file, it's specifically set
        //to only find stops within 40 meters for GPX pathing, as we are not going to the pokestops,
        //so do not make it more than 40 because it will never get close to those stops.
        //For non GPX pathing, it returns all pokestops in range.
        private static async Task<Tuple<List<FortData>, List<FortData>>> GetPokeStops(ISession session)
        {
            List<FortData> pokeStops = await UpdateFortsData(session);
            if (!session.LogicSettings.UseGpxPathing)
            {
                if (pokeStops.Count <= 0)
                {
                    // only send this for non GPX because otherwise we generate false positives
                    session.EventDispatcher.Send(new WarnEvent
                    {
                        Message = session.Translation.GetTranslation(TranslationString.FarmPokestopsNoUsableFound)
                    });
                }

                session.EventDispatcher.Send(new PokeStopListEvent { Forts = pokeStops });
                return Tuple.Create(pokeStops, pokeStops);
            }

            if (pokeStops.Count > 0)
            {
                // only send when there are stops for GPX because otherwise we send empty arrays often
                session.EventDispatcher.Send(new PokeStopListEvent { Forts = pokeStops });
            }
            // Wasn't sure how to make this pretty. Edit as needed.
            return Tuple.Create(
                pokeStops,
                pokeStops.Where(
                    i =>
                        ( // Make sure PokeStop is within 40 meters or else it is pointless to hit it
                            LocationUtils.CalculateDistanceInMeters(
                                session.Client.CurrentLatitude, session.Client.CurrentLongitude,
                                i.Latitude, i.Longitude) < 40) ||
                        session.LogicSettings.MaxTravelDistanceInMeters == 0
                ).ToList());
        }

        private static async Task<List<FortData>> UpdateFortsData(ISession session)
        {
            var mapObjects = await session.Client.Map.GetMapObjects();

            var pokeStops = mapObjects.Item1.MapCells.SelectMany(i => i.Forts)
                .Where(
                    i =>
                        i.Type == FortType.Checkpoint &&
                        i.CooldownCompleteTimestampMs < DateTime.UtcNow.ToUnixTime() &&
                        (
                            LocationUtils.CalculateDistanceInMeters(
                                session.Client.CurrentLatitude, session.Client.CurrentLongitude,
                                i.Latitude, i.Longitude) < session.LogicSettings.MaxTravelDistanceInMeters) ||
                        session.LogicSettings.MaxTravelDistanceInMeters == 0
                );

            return pokeStops.ToList();
        }
    }
}