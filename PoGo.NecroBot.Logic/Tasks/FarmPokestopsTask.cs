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
using System.Xml.Linq;
using System.Xml.XPath;

#endregion

namespace PoGo.NecroBot.Logic.Tasks
{
    public static class FarmPokestopsTask
    {
        public static int TimesZeroXPawarded;
        private static int storeRI;
        private static readonly System.Globalization.CultureInfo _CultureEnglish = new System.Globalization.CultureInfo("en");
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
                    session.LogicSettings.WalkingSpeedInKilometerPerHour, null, cancellationToken, session.LogicSettings.DisableHumanWalking);
            }

            var pokestopList = await GetPokeStops(session);
            var stopsHit = 0;
            var rc = new Random(); //initialize pokestop random cleanup counter first time
            storeRI = rc.Next(8, 15);
            var eggWalker = new EggWalker(1000, session);

            if (pokestopList.Count <= 0)
            {
                session.EventDispatcher.Send(new WarnEvent
                {
                    Message = session.Translation.GetTranslation(TranslationString.FarmPokestopsNoUsableFound)
                });
            }

            session.EventDispatcher.Send(new PokeStopListEvent { Forts = pokestopList });

            while (pokestopList.Any())
            {
                cancellationToken.ThrowIfCancellationRequested();

                //resort
                var pokestopListWithDetails = pokestopList
                                .Select(p =>
                                {
                                    Boolean useNav = session.LogicSettings.UseOsmNavigation && LocationUtils.CalculateDistanceInMeters(session.Client.CurrentLatitude, session.Client.CurrentLongitude, p.Latitude, p.Longitude) > session.LogicSettings.OsmMinDistanceInMeter;
                                    String uri = useNav ? string.Format(_CultureEnglish, "http://www.yournavigation.org/api/1.0/gosmore.php?flat={0:0.000000}&flon={1:0.000000}&tlat={2:0.000000}&tlon={3:0.000000}&v=foot", session.Client.CurrentLatitude, session.Client.CurrentLongitude, p.Latitude, p.Longitude) : null;
                                    XDocument doc = useNav ? XDocument.Load(uri) : null;
                                    XNamespace kmlns = useNav ? XNamespace.Get("http://earth.google.com/kml/2.0") : null;
                                    var points = !useNav ? null :
                                                 doc.Element(kmlns + "kml")
                                                    .Element(kmlns + "Document")
                                                    .Element(kmlns + "Folder")
                                                    .Element(kmlns + "Placemark")
                                                    .Element(kmlns + "LineString")
                                                    .Element(kmlns + "coordinates")
                                                    .Value
                                                    .Trim()
                                                    .Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                                                    .Select(pp =>
                                                    {
                                                        String[] parts = pp.Split(',');
                                                        return new
                                                        {
                                                            Latitude = double.Parse(parts[1], _CultureEnglish),
                                                            Longitude = double.Parse(parts[0], _CultureEnglish),
                                                        };
                                                    })
                                                    .ToArray();
                                    Double dist = useNav ?
                                                          new Func<double>(() =>
                                                          {
                                                              Double d = 0d;
                                                              for (int i = 1; i < points.Length; i++)
                                                              {
                                                                  d += LocationUtils.CalculateDistanceInMeters
                                                                  (
                                                                      points[i - 1].Latitude,
                                                                      points[i - 1].Longitude,
                                                                      points[i].Latitude,
                                                                      points[i].Longitude
                                                                  );
                                                              }
                                                              return d;
                                                          })() :
                                                          LocationUtils.CalculateDistanceInMeters(session.Client.CurrentLatitude, session.Client.CurrentLongitude, p.Latitude, p.Longitude);
                                    return new
                                    {
                                        PokeStop = p,
                                        UseOSM = useNav,
                                        Distance = dist,
                                        NavigationDocumentUri = uri,
                                        NavigationDocument = doc,
                                        NavigationDocumentNamespace = kmlns,
                                        Points = points
                                    };
                                })
                                .OrderBy(p => p.Distance)
                                .ToList();
                // randomize next pokestop between first and second by distance
                var pokestopListNum = 0;
                if (pokestopList.Count > 1)
                {
                    pokestopListNum = rc.Next(0, 2);
                }

                var pokeStop = pokestopListWithDetails[pokestopListNum];
                pokestopList.Remove(pokeStop.PokeStop);

                var distance = pokeStop.Distance;
                var fortInfo = await session.Client.Fort.GetFort(pokeStop.PokeStop.Id, pokeStop.PokeStop.Latitude, pokeStop.PokeStop.Longitude);

                session.EventDispatcher.Send(new FortTargetEvent { Name = fortInfo.Name, Distance = distance });

                if (pokeStop.UseOSM)
                {
                    var points = pokeStop.Points;
                    if (points.Any())
                    {
                        foreach (var step in points)
                        {
                            await MoveToLocationAsync(session, cancellationToken, step.Latitude, step.Longitude);
                        }
                    }
                }
                //Why no else? Just to be sure =)
                await MoveToLocationAsync(session, cancellationToken, pokeStop.PokeStop.Latitude, pokeStop.PokeStop.Longitude);

                //Catch Lure Pokemon
                if (pokeStop.PokeStop.LureInfo != null)
                {
                    await CatchLurePokemonsTask.Execute(session, pokeStop.PokeStop, cancellationToken);
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
                        await session.Client.Fort.SearchFort(pokeStop.PokeStop.Id, pokeStop.PokeStop.Latitude, pokeStop.PokeStop.Longitude);
                    if (fortSearch.ExperienceAwarded > 0 && timesZeroXPawarded > 0) timesZeroXPawarded = 0;
                    if (fortSearch.ExperienceAwarded == 0)
                    {
                        timesZeroXPawarded++;

                        if (timesZeroXPawarded > zeroCheck)
                        {
                            if ((int)fortSearch.CooldownCompleteTimestampMs != 0)
                            {
                                break;
                                // Check if successfully looted, if so program can continue as this was "false alarm".
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
                            Id = pokeStop.PokeStop.Id,
                            Name = fortInfo.Name,
                            Exp = fortSearch.ExperienceAwarded,
                            Gems = fortSearch.GemsAwarded,
                            Items = StringUtils.GetSummedFriendlyNameOfItemAwardList(fortSearch.ItemsAwarded),
                            Latitude = pokeStop.PokeStop.Latitude,
                            Longitude = pokeStop.PokeStop.Longitude,
                            InventoryFull = fortSearch.Result == FortSearchResponse.Types.Result.InventoryFull
                        });

                        if (fortSearch.Result == FortSearchResponse.Types.Result.InventoryFull)
                            storeRI = 1;

                        break; //Continue with program as loot was succesfull.
                    }
                } while (fortTry < retryNumber - zeroCheck);
                //Stop trying if softban is cleaned earlier or if 40 times fort looting failed.

                await eggWalker.ApplyDistance(distance, cancellationToken);

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

        private static async Task MoveToLocationAsync(ISession session, CancellationToken cancellationToken, double latitude, double longitude)
        {
            await session.Navigation.Move(new GeoCoordinate(latitude, longitude),
                session.LogicSettings.WalkingSpeedInKilometerPerHour,
                async () =>
                {
                    // Catch normal map Pokemon
                    await CatchNearbyPokemonsTask.Execute(session, cancellationToken);
                    //Catch Incense Pokemon
                    await CatchIncensePokemonsTask.Execute(session, cancellationToken);
                    return true;
                }, cancellationToken, session.LogicSettings.DisableHumanWalking);
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
