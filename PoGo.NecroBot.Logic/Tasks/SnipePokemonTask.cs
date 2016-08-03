#region using directives

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PoGo.NecroBot.Logic.Common;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.PoGoUtils;
using PoGo.NecroBot.Logic.State;
using POGOProtos.Enums;
using POGOProtos.Inventory.Item;
using POGOProtos.Map.Pokemon;
using POGOProtos.Networking.Responses;

#endregion

namespace PoGo.NecroBot.Logic.Tasks
{
    public class SniperInfo
    {
        public ulong EncounterId { get; set; }
        public DateTime ExpirationTimestamp { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public PokemonId Id { get; set; }
        public string SpawnPointId { get; set; }
        public PokemonMove Move1 { get; set; }
        public PokemonMove Move2 { get; set; }
        public double IV { get; set; }

        [JsonIgnore]
        public DateTime TimeStampAdded { get; set; } = DateTime.Now;
    }

    public class PokemonLocation
    {
        public PokemonLocation(double lat, double lon)
        {
            latitude = lat;
            longitude = lon;
        }

        public long Id { get; set; }
        public double expires { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public int pokemon_id { get; set; }
        public PokemonId pokemon_name { get; set; }

        public bool Equals(PokemonLocation obj)
        {
            return Math.Abs(latitude - obj.latitude) < 0.0001 && Math.Abs(longitude - obj.longitude) < 0.0001;
        }

        public override bool Equals(object obj) // contains calls this here
        {
            var p = obj as PokemonLocation;
            if (p == null) // no cast available
            {
                return false;
            }

            return Math.Abs(latitude - p.latitude) < 0.0001 && Math.Abs(longitude - p.longitude) < 0.0001;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            return latitude.ToString("0.0000") + ", " + longitude.ToString("0.0000");
        }
    }

    public class PokemonLocation_pokezz
    {

        public double time { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
        public int id { get; set; }
        public string iv { get; set; }
        public double _iv
        {
            get
            {
                try
                {
                    return Convert.ToDouble(iv);
                }
                catch 
                {
                    return 0;
                }
            }
        }
        public PokemonId name { get; set; }
        public string[] skills { get; set; }
        public Boolean verified { get; set; }

    }

    public class ScanResult
    {
        public string Status { get; set; }
        public List<PokemonLocation> pokemons { get; set; }
    }
    public class ScanResult_pokezz
    {
        public string Status { get; set; }
        public List<PokemonLocation_pokezz> pokemons { get; set; }
    }

    public static class SnipePokemonTask
    {
        public static List<PokemonLocation> LocsVisited = new List<PokemonLocation>();
        private static readonly List<SniperInfo> SnipeLocations = new List<SniperInfo>();
        private static DateTime _lastSnipe = DateTime.MinValue;

        public static Task AsyncStart(Session session, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Run(() => Start(session, cancellationToken), cancellationToken);
        }

        public static async Task<bool> CheckPokeballsToSnipe(int minPokeballs, ISession session,
            CancellationToken cancellationToken)
        {
            var pokeBallsCount = await session.Inventory.GetItemAmountByType(ItemId.ItemPokeBall);
            pokeBallsCount += await session.Inventory.GetItemAmountByType(ItemId.ItemGreatBall);
            pokeBallsCount += await session.Inventory.GetItemAmountByType(ItemId.ItemUltraBall);
            pokeBallsCount += await session.Inventory.GetItemAmountByType(ItemId.ItemMasterBall);

            if (pokeBallsCount < minPokeballs)
            {
                session.EventDispatcher.Send(new SnipeEvent
                {
                    Message =
                        session.Translation.GetTranslation(TranslationString.NotEnoughPokeballsToSnipe, pokeBallsCount,
                            minPokeballs)
                });
                return false;
            }

            return true;
        }

        public static async Task Execute(ISession session, CancellationToken cancellationToken)
        {
            if (_lastSnipe.AddMilliseconds(session.LogicSettings.MinDelayBetweenSnipes) > DateTime.Now)
                return;

            if (await CheckPokeballsToSnipe(session.LogicSettings.MinPokeballsToSnipe, session, cancellationToken))
            {
                if (session.LogicSettings.PokemonToSnipe != null)
                {
                    var st = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                    var t = DateTime.Now.ToUniversalTime() - st;
                    var currentTimestamp = t.TotalMilliseconds;

                    var pokemonIds = session.LogicSettings.PokemonToSnipe.Pokemon;

                    if (session.LogicSettings.UseSnipeLocationServer)
                    {
                       var locationsToSnipe = SnipeLocations?.Where(q =>
                            (!session.LogicSettings.UseTransferIvForSnipe ||
                             (q.IV == 0 && !session.LogicSettings.SnipeIgnoreUnknownIv) ||
                             (q.IV >= session.Inventory.GetPokemonTransferFilter(q.Id).KeepMinIvPercentage)) &&
                            !LocsVisited.Contains(new PokemonLocation(q.Latitude, q.Longitude))
                            && !(q.ExpirationTimestamp != default(DateTime) &&
                                 q.ExpirationTimestamp > new DateTime(2016) &&
                                 // make absolutely sure that the server sent a correct datetime
                                 q.ExpirationTimestamp < DateTime.Now) &&
                            (q.Id == PokemonId.Missingno || pokemonIds.Contains(q.Id))).ToList() ??
                                               new List<SniperInfo>();

                        if (locationsToSnipe.Any())
                        {
                            _lastSnipe = DateTime.Now;
                            foreach (var location in locationsToSnipe)
                            {
                                session.EventDispatcher.Send(new SnipeScanEvent
                                {
                                    Bounds = new Location(location.Latitude, location.Longitude),
                                    PokemonId = location.Id,
                                    Iv = location.IV
                                });

                                if (!await CheckPokeballsToSnipe(session.LogicSettings.MinPokeballsWhileSnipe + 1, 
                                    session, cancellationToken))
                                    return;

                                await Snipe(session, pokemonIds, location.Latitude, location.Longitude, cancellationToken);
                                LocsVisited.Add(new PokemonLocation(location.Latitude, location.Longitude));
                            }
                        }
                    }
                    else
                    {


                        if (session.LogicSettings.GetSniperInfoFromPokezz)
                        {
                            var _locationsToSnipe =  GetSniperInfoFrom_pokezz(session, pokemonIds);
                            if (_locationsToSnipe.Any())
                            {
                                _lastSnipe = DateTime.Now;
                                foreach (var location in _locationsToSnipe)
                                {
                                    session.EventDispatcher.Send(new SnipeScanEvent
                                    {
                                        Bounds = new Location(location.Latitude, location.Longitude),
                                        PokemonId = location.Id,
                                        //Iv = location.IV
                                    });

                                    if (!await CheckPokeballsToSnipe(session.LogicSettings.MinPokeballsWhileSnipe + 1,
                                        session, cancellationToken))
                                        return;

                                    await Snipe(session, pokemonIds, location.Latitude, location.Longitude, cancellationToken);
                                    LocsVisited.Add(new PokemonLocation(location.Latitude, location.Longitude));
                                }
                            }
                        }

                           

                        foreach (var location in session.LogicSettings.PokemonToSnipe.Locations)
                        {
                            session.EventDispatcher.Send(new SnipeScanEvent
                            {
                                Bounds = location,
                                PokemonId = PokemonId.Missingno
                            });

                            var scanResult = SnipeScanForPokemon(session, location);

                            var locationsToSnipe = new List<PokemonLocation>();
                            if (scanResult.pokemons != null)
                            {
                                var filteredPokemon = scanResult.pokemons.Where(q => pokemonIds.Contains(q.pokemon_name));
                                var notVisitedPokemon = filteredPokemon.Where(q => !LocsVisited.Contains(q));
                                var notExpiredPokemon = notVisitedPokemon.Where(q => q.expires < currentTimestamp);

                                if (notExpiredPokemon.Count() > 0)
                                {
                                    locationsToSnipe.AddRange(notExpiredPokemon);
                                }
                            }

                            if (locationsToSnipe.Any())
                            {
                                foreach (var pokemonLocation in locationsToSnipe)
                                {
                                    if (
                                        !await
                                            CheckPokeballsToSnipe(session.LogicSettings.MinPokeballsWhileSnipe + 1,
                                                session, cancellationToken))
                                        return;

                                    LocsVisited.Add(pokemonLocation);

                                    await
                                        Snipe(session, pokemonIds, pokemonLocation.latitude, pokemonLocation.longitude,
                                            cancellationToken);
                                }
                            }
                            else if (!string.IsNullOrEmpty(scanResult.Status) && scanResult.Status.Contains("fail"))
                            {
                                session.EventDispatcher.Send(new SnipeEvent
                                {
                                    Message = session.Translation.GetTranslation(TranslationString.SnipeServerOffline)
                                });
                            }
                            else
                            {
                                session.EventDispatcher.Send(new SnipeEvent
                                {
                                    Message = session.Translation.GetTranslation(TranslationString.NoPokemonToSnipe)
                                });
                            }

                            _lastSnipe = DateTime.Now;
                        }
                    }
                }
            }
        }

        private static async Task Snipe(ISession session, IEnumerable<PokemonId> pokemonIds, double latitude,
            double longitude, CancellationToken cancellationToken)
        {
            var CurrentLatitude = session.Client.CurrentLatitude;
            var CurrentLongitude = session.Client.CurrentLongitude;

            session.EventDispatcher.Send(new SnipeModeEvent {Active = true});

            List<MapPokemon> catchablePokemon;
            try
            {
                await 
                    session.Client.Player.UpdatePlayerLocation(latitude, longitude, session.Client.CurrentAltitude);

                session.EventDispatcher.Send(new UpdatePositionEvent
                {
                    Longitude = longitude,
                    Latitude = latitude
                });

                var mapObjects = session.Client.Map.GetMapObjects().Result;
                catchablePokemon =
                    mapObjects.Item1.MapCells.SelectMany(q => q.CatchablePokemons)
                        .Where(q => pokemonIds.Contains(q.PokemonId))
                        .OrderByDescending(pokemon => PokemonInfo.CalculateMaxCpMultiplier(pokemon.PokemonId))
                        .ToList();
            }
            finally
            {
                await 
                    session.Client.Player.UpdatePlayerLocation(CurrentLatitude, CurrentLongitude, session.Client.CurrentAltitude);
            }

            foreach (var pokemon in catchablePokemon)
            {
                EncounterResponse encounter;
                try
                {
                    await
                        session.Client.Player.UpdatePlayerLocation(latitude, longitude, session.Client.CurrentAltitude);

                    encounter =
                        session.Client.Encounter.EncounterPokemon(pokemon.EncounterId, pokemon.SpawnPointId).Result;
                }
                finally
                {
                    await
                        session.Client.Player.UpdatePlayerLocation(CurrentLatitude, CurrentLongitude,
                            session.Client.CurrentAltitude);
                }

                if (encounter.Status == EncounterResponse.Types.Status.EncounterSuccess)
                {
                    session.EventDispatcher.Send(new UpdatePositionEvent
                    {
                        Latitude = CurrentLatitude,
                        Longitude = CurrentLongitude
                    });

                    await CatchPokemonTask.Execute(session, cancellationToken, encounter, pokemon);
                }
                else if (encounter.Status == EncounterResponse.Types.Status.PokemonInventoryFull)
                {
                    if (session.LogicSettings.EvolveAllPokemonAboveIv ||
                        session.LogicSettings.EvolveAllPokemonWithEnoughCandy)
                    {
                        await EvolvePokemonTask.Execute(session, cancellationToken);
                    }

                    if (session.LogicSettings.TransferDuplicatePokemon)
                    {
                        await TransferDuplicatePokemonTask.Execute(session, cancellationToken);
                    }
                    else
                    {
                        session.EventDispatcher.Send(new WarnEvent
                        {
                            Message = session.Translation.GetTranslation(TranslationString.InvFullTransferManually)
                        });
                    }
                }
                else
                {
                    session.EventDispatcher.Send(new WarnEvent
                    {
                        Message =
                            session.Translation.GetTranslation(
                                TranslationString.EncounterProblem, encounter.Status)
                    });
                }

                if (
                    !Equals(catchablePokemon.ElementAtOrDefault(catchablePokemon.Count - 1),
                        pokemon))
                {
                    await Task.Delay(session.LogicSettings.DelayBetweenPokemonCatch, cancellationToken);
                }
            }

            session.EventDispatcher.Send(new SnipeModeEvent {Active = false});
            await Task.Delay(session.LogicSettings.DelayBetweenPlayerActions, cancellationToken);
        }

        private static ScanResult SnipeScanForPokemon(ISession session, Location location)
        {
            var formatter = new NumberFormatInfo {NumberDecimalSeparator = "."};

            var offset = session.LogicSettings.SnipingScanOffset;
            // 0.003 = half a mile; maximum 0.06 is 10 miles
            if (offset < 0.001) offset = 0.003;
            if (offset > 0.06) offset = 0.06;

            var boundLowerLeftLat = location.Latitude - offset;
            var boundLowerLeftLng = location.Longitude - offset;
            var boundUpperRightLat = location.Latitude + offset;
            var boundUpperRightLng = location.Longitude + offset;

            var uri =
                $"http://skiplagged.com/api/pokemon.php?bounds={boundLowerLeftLat.ToString(formatter)},{boundLowerLeftLng.ToString(formatter)},{boundUpperRightLat.ToString(formatter)},{boundUpperRightLng.ToString(formatter)}";

            ScanResult scanResult;
            try
            {
                var request = WebRequest.CreateHttp(uri);
                request.Accept = "application/json";
                request.UserAgent =
                    "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36\r\n";
                request.Method = "GET";
                request.Timeout = 15000;
                request.ReadWriteTimeout = 32000;

                var resp = request.GetResponse();
                var reader = new StreamReader(resp.GetResponseStream());
                var fullresp = reader.ReadToEnd().Replace(" M", "Male").Replace(" F", "Female").Replace("Farfetch'd", "Farfetchd").Replace("Mr.Maleime", "MrMime");

                scanResult = JsonConvert.DeserializeObject<ScanResult>(fullresp);
            }
            catch (Exception ex)
            {
                // most likely System.IO.IOException
                session.EventDispatcher.Send(new ErrorEvent {Message = ex.Message});
                scanResult = new ScanResult
                {
                    Status = "fail",
                    pokemons = new List<PokemonLocation>()
                };
            }
            return scanResult;
        }


        private static List<SniperInfo> GetSniperInfoFrom_pokezz(ISession session, List<PokemonId> pokemonIds)
        {

            var uri = $"http://pokezz.com/pokemons.json";

            ScanResult_pokezz scanResult_pokezz;
            try
            {
                var request = WebRequest.CreateHttp(uri);
                request.Accept = "application/json";
                request.UserAgent =
                    "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36\r\n";
                request.Method = "GET";
                request.Timeout = 15000;
                request.ReadWriteTimeout = 32000;

                var resp = request.GetResponse();
                var reader = new StreamReader(resp.GetResponseStream());
                var fullresp = "{\"pokemons\": " + reader.ReadToEnd().Replace(" M", "Male").Replace(" F", "Female").Replace("Farfetch'd", "Farfetchd").Replace("Mr.Maleime", "MrMime") +"}";
    
                scanResult_pokezz = JsonConvert.DeserializeObject<ScanResult_pokezz>(fullresp);
            }
            catch (Exception ex)
            {
                // most likely System.IO.IOException
                session.EventDispatcher.Send(new ErrorEvent { Message = ex.Message });
                scanResult_pokezz = new ScanResult_pokezz
                {
                    Status = "fail",
                    pokemons = new List<PokemonLocation_pokezz>()
                };
                return new List<SniperInfo>();
            }
            if (scanResult_pokezz.pokemons != null)
            {

                SnipeLocations.RemoveAll(x => DateTime.Now > x.TimeStampAdded.AddMinutes(15));

                foreach (var pokemon in scanResult_pokezz.pokemons)
                {
                    var SnipInfo = new SniperInfo();
                    SnipInfo.Id = pokemon.name;
                    SnipInfo.Latitude = pokemon.lat;
                    SnipInfo.Longitude = pokemon.lng;
                    SnipInfo.TimeStampAdded = DateTime.Now;
                    SnipInfo.ExpirationTimestamp = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(pokemon.time / 1000d)).ToLocalTime();
                    SnipInfo.IV = pokemon._iv;
                    if (pokemon.verified || !session.LogicSettings.GetOnlyVerifiedSniperInfoFromPokezz)
                    {
                        SnipeLocations.Add(SnipInfo);
                    }
                }

                var locationsToSnipe = SnipeLocations?.Where(q =>
                (!session.LogicSettings.UseTransferIvForSnipe ||
                 (q.IV == 0 && !session.LogicSettings.SnipeIgnoreUnknownIv) ||
                 (q.IV >= session.Inventory.GetPokemonTransferFilter(q.Id).KeepMinIvPercentage)) &&
                !LocsVisited.Contains(new PokemonLocation(q.Latitude, q.Longitude))
                && !(q.ExpirationTimestamp != default(DateTime) &&
                     q.ExpirationTimestamp > new DateTime(2016) &&
                     // make absolutely sure that the server sent a correct datetime
                     q.ExpirationTimestamp < DateTime.Now) &&
                (q.Id == PokemonId.Missingno || pokemonIds.Contains(q.Id))).ToList() ??
                                   new List<SniperInfo>();
                return locationsToSnipe;
            }
            return new List<SniperInfo>();
        }

        public static async Task Start(Session session, CancellationToken cancellationToken)
        {
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                try
                {
                    var lClient = new TcpClient();
                    lClient.Connect(session.LogicSettings.SnipeLocationServer,
                        session.LogicSettings.SnipeLocationServerPort);

                    var sr = new StreamReader(lClient.GetStream());

                    while (lClient.Connected)
                    {
                        try
                        {
                            var line = sr.ReadLine();
                            if (line == null)
                                throw new Exception("Unable to ReadLine from sniper socket");

                            var info = JsonConvert.DeserializeObject<SniperInfo>(line);

                            if (SnipeLocations.Any(x =>
                                Math.Abs(x.Latitude - info.Latitude) < 0.0001 &&
                                Math.Abs(x.Longitude - info.Longitude) < 0.0001))
                                // we might have different precisions from other sources
                                continue;

                            SnipeLocations.RemoveAll(x => DateTime.Now > x.TimeStampAdded.AddMinutes(15));
                            SnipeLocations.Add(info);
                        }
                        catch (System.IO.IOException)
                        {
                            session.EventDispatcher.Send(new ErrorEvent { Message = "The connection to the sniping location server was lost." });
                        }
                    }
                }
                catch (SocketException)
                {
                    // this is spammed to often. Maybe add it to debug log later
                }
                catch (Exception ex)
                {
                    // most likely System.IO.IOException
                    session.EventDispatcher.Send(new ErrorEvent {Message = ex.ToString()});
                }
                await Task.Delay(100, cancellationToken);
            }
        }
    }
}