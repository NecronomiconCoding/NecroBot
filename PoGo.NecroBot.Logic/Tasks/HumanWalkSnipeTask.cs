using GeoCoordinatePortable;
using Newtonsoft.Json;
using PoGo.NecroBot.Logic.Common;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.Interfaces.Configuration;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.Model.Settings;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Utils;
using POGOProtos.Enums;
using POGOProtos.Inventory.Item;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PoGo.NecroBot.Logic.Tasks
{
    //need refactor this class, move list snipping pokemon to session and split function out to smaller class.
    public class HumanWalkSnipeTask
    {
        public class Wrapper
        {
            public List<RarePokemonInfo> data { get; set; }
        }
        public class RarePokemonInfo
        {
            public double distance;
            public double estimateTime;
            public double created { get; set; }
            public double latitude { get; set; }
            public double longitude { get; set; }
            public int pokemonId { get; set; }
            public string id
            {
                get
                {
                    return $"{pokemonId}-{latitude}-{longitude}";
                }
            }
            public bool visited { get; set; }
            public HumanWalkSnipeFilter FilterSetting { get; set; }

            public PokemonId Id
            {
                get
                {
                    return (PokemonId)(pokemonId);
                }
            }
            public DateTime expired
            {
                get
                {
                    System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                    dtDateTime = dtDateTime.AddSeconds(created).ToLocalTime();
                    return dtDateTime.AddMinutes(15);
                }
            }
        }
        private static List<RarePokemonInfo> rarePokemons = new List<RarePokemonInfo>();

        private static ISession _session;
        private static ILogicSettings _setting;

        private static int pokestopCount = 0;
        private static List<PokemonId> pokemonToBeSnipedIds = null;

        public static async Task<bool> CheckPokeballsToSnipe(int minPokeballs, ISession session,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            // Refresh inventory so that the player stats are fresh
            await session.Inventory.RefreshCachedInventory();
            var pokeBallsCount = await session.Inventory.GetItemAmountByType(ItemId.ItemPokeBall);
            pokeBallsCount += await session.Inventory.GetItemAmountByType(ItemId.ItemGreatBall);
            pokeBallsCount += await session.Inventory.GetItemAmountByType(ItemId.ItemUltraBall);
            pokeBallsCount += await session.Inventory.GetItemAmountByType(ItemId.ItemMasterBall);

            if (pokeBallsCount < minPokeballs)
            {
                session.EventDispatcher.Send(new HumanWalkSnipeEvent
                {
                    Type = HumanWalkSnipeEventTypes.NotEnoughtPalls,
                    CurrentBalls = pokeBallsCount,
                    MinBallsToSnipe = minPokeballs,
                });
                return false;
            }
            return true;
        }

        public static Task ExecuteFetchData(Session session)
        {
            InitSession(session);

            return Task.Run(() =>
            {
                FetchData(_session.Client.CurrentLatitude, _session.Client.CurrentLongitude, false);
            });
        }

        private static void InitSession(ISession session)
        {
            if (_session == null)
            {
                _session = session;
                _setting = _session.LogicSettings;
                pokemonToBeSnipedIds = _setting.HumanWalkingSnipeUseSnipePokemonList ? _setting.PokemonToSnipe.Pokemon : new List<PokemonId>();
                pokemonToBeSnipedIds.AddRange(_setting.HumanWalkSnipeFilters.Where(x => !pokemonToBeSnipedIds.Any(t => t == x.Key)).Select(x => x.Key).ToList());      //this will combine with pokemon snipe filter

            }
        }
        static bool prioritySnipeFlag = false;
        public static async Task Execute(ISession session, CancellationToken cancellationToken, Func<double, double, Task> actionWhenWalking = null, Func<Task> afterCatchFunc = null)
        {
            pokestopCount++;
            pokestopCount = pokestopCount % 3;

            if (pokestopCount > 0 && !prioritySnipeFlag) return;

            InitSession(session);
            if (!_setting.CatchPokemon && !prioritySnipeFlag) return;

            cancellationToken.ThrowIfCancellationRequested();

            if (_setting.HumanWalkingSnipeTryCatchEmAll)
            {
                var checkBall = await CheckPokeballsToSnipe(_setting.HumanWalkingSnipeCatchEmAllMinBalls, session, cancellationToken);
                if (!checkBall && !prioritySnipeFlag) return;
            }

            bool keepWalking = true;
            bool caughtAnyPokemonInThisWalk = false;

            while (keepWalking)
            {
                prioritySnipeFlag = false;
                var pokemons = GetRarePokemons(session.Client.CurrentLatitude, session.Client.CurrentLongitude, !caughtAnyPokemonInThisWalk);

                foreach (var pokemon in pokemons)
                {
                    caughtAnyPokemonInThisWalk = true;
                    CalculateDistanceAndEstTime(pokemon);
                    var remainTimes = (pokemon.expired - DateTime.Now).TotalSeconds * 0.95; //just use 90% times
                    var catchPokemonTimeEST = (pokemon.distance / 100) * 10;  //assume that 100m we catch 1 pokemon and it took 10 second for each.
                    string strPokemon = session.Translation.GetPokemonTranslation(pokemon.Id);
                    var spinPokestopEST = (pokemon.distance / 100) * 5;

                    bool catchPokemon = (pokemon.estimateTime + catchPokemonTimeEST) < remainTimes && pokemon.FilterSetting.CatchPokemonWhileWalking;
                    bool spinPokestop = pokemon.FilterSetting.SpinPokestopWhileWalking && (pokemon.estimateTime + catchPokemonTimeEST + spinPokestopEST) < remainTimes;

                    session.EventDispatcher.Send(new HumanWalkSnipeEvent()
                    {
                        PokemonId = pokemon.Id,
                        Latitude = pokemon.latitude,
                        Longitude = pokemon.longitude,
                        Distance = pokemon.distance,
                        Expires = (pokemon.expired - DateTime.Now).TotalSeconds,
                        Estimate = (int)pokemon.estimateTime,
                        Setting = pokemon.FilterSetting,
                        CatchPokemon = catchPokemon,
                        SpinPokeStop = pokemon.FilterSetting.SpinPokestopWhileWalking,
                        WalkSpeedApplied = pokemon.FilterSetting.AllowSpeedUp ? pokemon.FilterSetting.MaxSpeedUpSpeed : _session.LogicSettings.WalkingSpeedInKilometerPerHour,
                        Type = HumanWalkSnipeEventTypes.StartWalking
                    });

                    await session.Navigation.Move(new GeoCoordinate(pokemon.latitude, pokemon.longitude,
                           LocationUtils.getElevation(pokemon.latitude, pokemon.longitude)),
                       async () =>
                       {
                           var distance = LocationUtils.CalculateDistanceInMeters(pokemon.latitude, pokemon.longitude, session.Client.CurrentLatitude, session.Client.CurrentLongitude);

                           if (catchPokemon && distance > 80.0)
                           {
                               // Catch normal map Pokemon
                               await CatchNearbyPokemonsTask.Execute(session, cancellationToken);
                           }
                           if (actionWhenWalking != null && spinPokestop)
                           {
                               await actionWhenWalking(session.Client.CurrentLatitude, session.Client.CurrentLongitude);
                           }
                           return await Task.FromResult<bool>(true);
                       },
                       session,
                       cancellationToken, pokemon.FilterSetting.AllowSpeedUp ? pokemon.FilterSetting.MaxSpeedUpSpeed : 0);
                    session.EventDispatcher.Send(new HumanWalkSnipeEvent()
                    {
                        Latitude = pokemon.latitude,
                        Longitude = pokemon.longitude,
                        Type = HumanWalkSnipeEventTypes.DestinationReached
                    });
                    await Task.Delay(2000);
                    await CatchNearbyPokemonsTask.Execute(session, cancellationToken, pokemon.Id);
                    await CatchIncensePokemonsTask.Execute(session, cancellationToken);
                    pokemon.visited = true;
                }
                keepWalking = _setting.HumanWalkingSnipeTryCatchEmAll && pokemons.Count > 0;
            }

            if (caughtAnyPokemonInThisWalk && !_setting.HumanWalkingSnipeAlwaysWalkBack)
            {
                await afterCatchFunc?.Invoke();
            }
        }

        static void CalculateDistanceAndEstTime(RarePokemonInfo p)
        {
            double speed = p.FilterSetting.AllowSpeedUp ? p.FilterSetting.MaxSpeedUpSpeed : _setting.WalkingSpeedInKilometerPerHour;
            var speedInMetersPerSecond = speed / 3.6;

            p.distance = LocationUtils.CalculateDistanceInMeters(_session.Client.CurrentLatitude, _session.Client.CurrentLongitude, p.latitude, p.longitude);
            p.estimateTime = p.distance / speedInMetersPerSecond + 15; //margin 30 second

        }
        private static List<RarePokemonInfo> GetRarePokemons(double lat, double lng, bool refreshData = true)
        {
            if (refreshData)
            {
                FetchData(lat, lng);
            }

            rarePokemons.RemoveAll(p => p.expired < DateTime.Now);

            rarePokemons.ForEach((p) =>
            {
                CalculateDistanceAndEstTime(p);
            });

            //remove list not reach able (expired)
            if (rarePokemons.Count > 0)
            {
                var ordered = rarePokemons.Where(p => !p.visited &&
                    (p.FilterSetting.Priority == 0 || (
                    p.distance < p.FilterSetting.MaxDistance &&
                    p.estimateTime < p.FilterSetting.MaxWalkTimes)
                    && p.expired > DateTime.Now.AddSeconds(p.estimateTime)

                    )
                )
                .OrderBy(p => p.FilterSetting.Priority)
                .ThenBy(p => p.distance);
                if (ordered != null && ordered.Count() > 0)
                {
                    var first = ordered.First();
                    first.visited = true;
                    var results = new List<RarePokemonInfo>() { first };
                    return results;
                }
            }
            return new List<RarePokemonInfo>();
        }

        private static void FetchData(double lat, double lng, bool silent = false)
        {
            if (lastUpdated > DateTime.Now.AddSeconds(-30) && !silent) return;

            if (lastUpdated < DateTime.Now.AddSeconds(-30) && silent && rarePokemons != null && rarePokemons.Count > 0)
            {
                rarePokemons.ForEach(CalculateDistanceAndEstTime);
                rarePokemons = rarePokemons.OrderBy(p => p.FilterSetting.Priority).ThenBy(p => p.distance).ToList();
                _session.EventDispatcher.Send(new HumanWalkSnipeEvent()
                {
                    Type = HumanWalkSnipeEventTypes.ClientRequestUpdate,
                    Pokemons = rarePokemons,
                });
            }

            List<Task<List<RarePokemonInfo>>> allTasks = new List<Task<List<RarePokemonInfo>>>()
            {
                FetchFromPokeradar(lat, lng),
                FetchFromSkiplagged(lat, lng)
            };
            if (_setting.HumanWalkingSnipeIncludeDefaultLocation &&
                LocationUtils.CalculateDistanceInMeters(lat, lng, _session.Settings.DefaultLatitude, _session.Settings.DefaultLongitude) > 1000)
            {
                allTasks.Add(FetchFromPokeradar(_session.Settings.DefaultLatitude, _session.Settings.DefaultLongitude));
                allTasks.Add(FetchFromSkiplagged(_session.Settings.DefaultLatitude, _session.Settings.DefaultLongitude));
            }

            Task.WaitAll(allTasks.ToArray());
            lastUpdated = DateTime.Now;
            var fetchedPokemons = allTasks.SelectMany(p => p.Result);

            PostProcessDataFetched(fetchedPokemons, !silent);
        }
        public static T Clone<T>(object item)
        {
            if (item != null)
            {

                string json = JsonConvert.SerializeObject(item);
                return JsonConvert.DeserializeObject<T>(json);
            }
            else
                return default(T);
        }
        private static void PostProcessDataFetched(IEnumerable<RarePokemonInfo> pokemons, bool displayList = true)
        {
            var rw = new Random();
            var speedInMetersPerSecond = _setting.WalkingSpeedInKilometerPerHour / 3.6;
            int count = 0;

            foreach (var item in pokemons)
            {
                //the pokemon data already in the list
                if (rarePokemons.Any(x => x.id == item.id ||
                (LocationUtils.CalculateDistanceInMeters(x.latitude, x.longitude, item.latitude, item.longitude) < 10 && item.pokemonId == x.pokemonId)))
                {
                    continue;
                }
                //check if pokemon in the snip list
                if (!pokemonToBeSnipedIds.Any(x => x == item.Id)) continue;

                count++;
                var snipeSetting = _setting.HumanWalkSnipeFilters.FirstOrDefault(x => x.Key == item.Id);

                HumanWalkSnipeFilter config = new HumanWalkSnipeFilter(_setting.HumanWalkingSnipeMaxDistance,
                    _setting.HumanWalkingSnipeMaxEstimateTime,
                    3, //default priority
                    _setting.HumanWalkingSnipeTryCatchEmAll,
                    _setting.HumanWalkingSnipeSpinWhileWalking,
                    _setting.HumanWalkingSnipeAllowSpeedUp,
                    _setting.HumanWalkingSnipeMaxSpeedUpSpeed);

                if (_setting.HumanWalkSnipeFilters.Any(x => x.Key == item.Id))
                {
                    config = _setting.HumanWalkSnipeFilters.First(x => x.Key == item.Id).Value;
                }
                item.FilterSetting = Clone<HumanWalkSnipeFilter>(config);

                CalculateDistanceAndEstTime(item);

                rarePokemons.Add(item);
                rarePokemons = rarePokemons.OrderBy(p => p.FilterSetting.Priority).ThenBy(p => p.distance).ToList();
            }

            if (count > 0)
            {
                _session.EventDispatcher.Send(new HumanWalkSnipeEvent()
                {
                    Type = HumanWalkSnipeEventTypes.PokemonScanned,
                    Pokemons = rarePokemons,
                });

                if (_setting.HumanWalkingSnipeDisplayList)
                {
                    var ordered = rarePokemons.Where(p => p.expired > DateTime.Now.AddSeconds(p.estimateTime) && !p.visited).ToList();

                    if (ordered.Count > 0 && displayList)
                    {
                        Logger.Write(string.Format("          |  Name                   |    Distance     |  Expires        |  Travel times   | Catchable"));
                        foreach (var pokemon in ordered)
                        {
                            string name = _session.Translation.GetPokemonTranslation(pokemon.Id);
                            name += "".PadLeft(20 - name.Length, ' ');
                            Logger.Write(string.Format("SNIPPING  |  {0}  |  {1:0.00}m  \t|  {2:mm} min {2:ss} sec  |  {3:00} min {4:00} sec  | {5}",
                                name,
                                pokemon.distance,
                                pokemon.expired - DateTime.Now,
                                pokemon.estimateTime / 60,
                                pokemon.estimateTime % 60,
                                pokemon.expired > DateTime.Now.AddSeconds(pokemon.estimateTime) ? "Possible" : "Missied"
                                ));
                        }
                    }
                }
            }
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        private static DateTime lastUpdated = DateTime.Now.AddMinutes(-10);
        private static async Task<List<RarePokemonInfo>> FetchFromPokeradar(double lat, double lng)
        {
            List<RarePokemonInfo> results = new List<RarePokemonInfo>();
            if (!_setting.HumanWalkingSnipeUsePokeRadar) return results;
            try
            {

                HttpClient client = new HttpClient();
                double offset = _setting.HumanWalkingSnipeSnipingScanOffset; //0.015 
                string url = $"https://www.pokeradar.io/api/v1/submissions?deviceId=1fd29370661111e6b850a13a2bdc4ebf&minLatitude={lat - offset}&maxLatitude={lat + offset}&minLongitude={lng - offset}&maxLongitude={lng + offset}&pokemonId=0";

                var task = await client.GetStringAsync(url);

                var data = JsonConvert.DeserializeObject<Wrapper>(task);
                results = data.data;
            }
            catch (Exception ex)
            {
                Logger.Write("Error loading data", LogLevel.Error, ConsoleColor.DarkRed);
            }
            return results;
        }

        private static async Task<List<RarePokemonInfo>> FetchFromSkiplagged(double lat, double lng)
        {
            List<RarePokemonInfo> results = new List<RarePokemonInfo>();
            if (!_setting.HumanWalkingSnipeUseSkiplagged) return results;

            var lat1 = lat - _setting.HumanWalkingSnipeSnipingScanOffset;
            var lat2 = lat + _setting.HumanWalkingSnipeSnipingScanOffset;
            var lng1 = lng - _setting.HumanWalkingSnipeSnipingScanOffset;
            var lng2 = lng + _setting.HumanWalkingSnipeSnipingScanOffset;

            string url = $"https://skiplagged.com/api/pokemon.php?bounds={lat1},{lng1},{lat2},{lng2}";

            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.TryParseAdd("text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                client.DefaultRequestHeaders.AcceptEncoding.ParseAdd("gzip, deflate, sdch, br");
                client.DefaultRequestHeaders.Host = "skiplagged.com";
                client.DefaultRequestHeaders.UserAgent.TryParseAdd("Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.116 Safari/537.36");

                var json = await client.GetStringAsync(url);

                results = GetJsonList(json);
            }
            catch (Exception ex)
            {

            }
            return results;
        }
        private static List<RarePokemonInfo> GetJsonList(string reader)
        {
            var wrapper = JsonConvert.DeserializeObject<SkippedLaggedWrap>(reader);
            var list = new List<RarePokemonInfo>();
            foreach (var result in wrapper.pokemons)
            {
                var sniperInfo = Map(result);
                if (sniperInfo != null)
                {
                    list.Add(sniperInfo);
                }
            }
            return list;
        }
        private static RarePokemonInfo Map(pokemon result)
        {
            return new RarePokemonInfo()
            {
                latitude = result.latitude,
                longitude = result.longitude,
                pokemonId = result.pokemon_id,
                created = result.expires - 15 * 60
            };
        }

        public static Task PriorityPokemon(ISession session, string id)
        {
            return Task.Run(() =>
            {
                var pokemonItem = rarePokemons.FirstOrDefault(p => p.id == id);
                if (pokemonItem != null)
                {
                    pokemonItem.FilterSetting.Priority = 0;//will be going to catch next check. TODO  add code to trigger catch now
                }
            });
        }

        public static Task<List<RarePokemonInfo>> GetCurrentQueueItems(ISession session)
        {
            return Task.FromResult(rarePokemons);
        }

        public static Task TargetPokemonSnip(ISession session, string id)
        {
            return Task.Run(() =>
            {
                var ele = rarePokemons.FirstOrDefault(p => p.id == id);
                if (ele != null)
                {
                    ele.FilterSetting.Priority = 0;
                    rarePokemons = rarePokemons.OrderBy(p => p.FilterSetting.Priority).ThenBy(p => p.distance).ToList();
                    _session.EventDispatcher.Send(new HumanWalkSnipeEvent()
                    {
                        Type = HumanWalkSnipeEventTypes.TargetedPokemon,
                        Pokemons = rarePokemons,
                    });
                }
                prioritySnipeFlag = true;
            });
        }

        public static void UpdateCatchPokemon(double latitude, double longitude, PokemonId id)
        {
            var find = rarePokemons.FirstOrDefault(p => Math.Abs(p.latitude - latitude) < 10 &&
            Math.Abs(p.longitude - longitude) < 10.0 &&
            p.Id == id && 
            !p.visited);
            if(find!= null)
            {
                _session.EventDispatcher.Send(new HumanWalkSnipeEvent()
                {
                    UniqueId = find.id,
                    Type = HumanWalkSnipeEventTypes.EncounterSnipePokemon,
                    PokemonId = id,
                    Latitude = latitude, Longitude = longitude
                });

                find.visited = true;
            }
        }
    }

    public class SkippedLaggedWrap
    {
        public double duration { get; set; }
        public List<pokemon> pokemons { get; set; }
        public SkippedLaggedWrap()
        {
            pokemons = new List<pokemon>();
        }
    }
    public class pokemon
    {
        public DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public DateTime expires_date
        {
            get
            {
                return UnixTimeStampToDateTime(expires);
            }
        }

        public double expires { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public int pokemon_id { get; set; }
        public string pokemon_name { get; set; }
    }

}
