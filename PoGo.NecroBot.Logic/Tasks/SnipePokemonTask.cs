using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Utils;
using POGOProtos.Enums;
using POGOProtos.Networking.Responses;
using System.Net.Sockets;
using System.Threading;

namespace PoGo.NecroBot.Logic.Tasks
{
    public class SniperInfo
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
        public double iv { get; set; }
        public DateTime timeStamp { get; set; }
        public PokemonId id { get; set; }
        [JsonIgnore]
        public DateTime timeStampAdded { get; set; } = DateTime.Now;
    }

    public class PokemonLocation
    {
        public long id { get; set; }
        public double expiration_time { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string uid { get; set; }
        public bool is_alive { get; set; }
        public int pokemonId { get; set; }

        public PokemonLocation(double _latitude, double _longitude)
        {
            latitude = _latitude;
            longitude = _longitude;
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public override string ToString()
        {
            return latitude.ToString("0.0000") + ", " + latitude.ToString("0.0000");
        }

        public bool Equals(PokemonLocation obj)
        {
            return Math.Abs(latitude - obj.latitude) < 0.0001 && Math.Abs(longitude - obj.longitude) < 0.0001;
        }

        public override bool Equals(Object obj) // contains calls this here
        {
            if (obj == null)
                return false;

            PokemonLocation p = obj as PokemonLocation;
            if ((System.Object)p == null) // no cast available
            {
                return false;
            }

            return Math.Abs(latitude - p.latitude) < 0.0001 && Math.Abs(longitude - p.longitude) < 0.0001;
        }
    }

    public class ScanResult
    {
        public string status { get; set; }
        public List<PokemonLocation> pokemon { get; set; }
    }

    public static class SnipePokemonTask
    {
        public static List<PokemonLocation> locsVisited = new List<PokemonLocation>();
        private static List<SniperInfo> snipeLocations = new List<SniperInfo>();
        private static DateTime lastSnipe = DateTime.Now;

        public static Task AsyncStart(Session session, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Run(() => Start(session, cancellationToken), cancellationToken);
        }
        public static async Task Start(Session session, CancellationToken cancellationToken)
        {
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                try
                {
                    TcpClient lClient = new TcpClient();
                    lClient.Connect(session.LogicSettings.SnipeLocationServer, session.LogicSettings.SnipeLocationServerPort);

                    var sr = new StreamReader(lClient.GetStream());

                    while (lClient.Connected)
                    {
                        var line = sr.ReadLine();
                        if (line == null)
                            throw new Exception("Unable to ReadLine from sniper socket");

                        var info = JsonConvert.DeserializeObject<SniperInfo>(line);

                        if (snipeLocations.Any(x =>
                                Math.Abs(x.latitude - info.latitude) < 0.0001 &&
                                Math.Abs(x.longitude - info.longitude) < 0.0001)) // we might have different precisions from other sources
                            continue;

                        snipeLocations.RemoveAll(x => DateTime.Now > x.timeStampAdded.AddMinutes(15));
                        snipeLocations.Add(info);
                    }
                }
                catch (SocketException)
                {
                    // this is spammed to often. Maybe add it to debug log later
                }
                catch (Exception ex)
                {
                    // most likely System.IO.IOException
                    session.EventDispatcher.Send(new ErrorEvent { Message = ex.ToString() });
                }

                await Task.Delay(5000, cancellationToken);
            }
        }

        private static async Task snipe(ISession session, IEnumerable<PokemonId> pokemonIds, double latitude, double longitude, CancellationToken cancellationToken)
        {
            var currentLatitude = session.Client.CurrentLatitude;
            var currentLongitude = session.Client.CurrentLongitude;

            await
                session.Client.Player.UpdatePlayerLocation(latitude,
                    longitude, session.Client.CurrentAltitude);

            session.EventDispatcher.Send(new UpdatePositionEvent()
            {
                Longitude = longitude,
                Latitude = latitude
            });

            var mapObjects = session.Client.Map.GetMapObjects().Result;
            var catchablePokemon =
                mapObjects.MapCells.SelectMany(q => q.CatchablePokemons)
                    .Where(q => pokemonIds.Contains(q.PokemonId))
                    .ToList();

            await session.Client.Player.UpdatePlayerLocation(currentLatitude, currentLongitude,
                        session.Client.CurrentAltitude);

            foreach (var pokemon in catchablePokemon)
            {
                cancellationToken.ThrowIfCancellationRequested();

                await session.Client.Player.UpdatePlayerLocation(latitude, longitude, session.Client.CurrentAltitude);

                var encounter = session.Client.Encounter.EncounterPokemon(pokemon.EncounterId, pokemon.SpawnPointId).Result;

                await session.Client.Player.UpdatePlayerLocation(currentLatitude, currentLongitude, session.Client.CurrentAltitude);

                if (encounter.Status == EncounterResponse.Types.Status.EncounterSuccess)
                {
                    session.EventDispatcher.Send(new UpdatePositionEvent()
                    {
                        Latitude = currentLatitude,
                        Longitude = currentLongitude
                    });

                    await CatchPokemonTask.Execute(session, encounter, pokemon);
                }
                else if (encounter.Status == EncounterResponse.Types.Status.PokemonInventoryFull)
                {
                    session.EventDispatcher.Send(new WarnEvent
                    {
                        Message =
                            session.Translation.GetTranslation(
                                Common.TranslationString.InvFullTransferManually)
                    });
                }
                else
                {
                    session.EventDispatcher.Send(new WarnEvent
                    {
                        Message =
                            session.Translation.GetTranslation(
                                Common.TranslationString.EncounterProblem, encounter.Status)
                    });
                }

                if (
                    !Equals(catchablePokemon.ElementAtOrDefault(catchablePokemon.Count() - 1),
                        pokemon))
                {
                    await Task.Delay(session.LogicSettings.DelayBetweenPokemonCatch, cancellationToken);
                }
            }

            await Task.Delay(session.LogicSettings.DelayBetweenPlayerActions, cancellationToken);
        }

        public static async Task Execute(ISession session, CancellationToken cancellationToken)
        {
            if (lastSnipe.AddMilliseconds(session.LogicSettings.MinDelayBetweenSnipes) > DateTime.Now)
                return;

            if (session.LogicSettings.PokemonToSnipe != null)
            {
                DateTime st = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                TimeSpan t = (DateTime.Now.ToUniversalTime() - st);
                var currentTimestamp = t.TotalMilliseconds;

                var pokemonIds = session.LogicSettings.PokemonToSnipe.Pokemon;

                if (session.LogicSettings.UseSnipeLocationServer)
                {
                    var locationsToSnipe = snipeLocations == null ? new List<SniperInfo>() : snipeLocations.Where(q =>
                        // when UseTransferIVForSnipe=true skip pokemons with unknown iv or if they don't match the TransferFilter/default MinIV
                        (!session.LogicSettings.UseTransferIVForSnipe || (q.iv > 0 && q.iv < session.Inventory.GetPokemonTransferFilter(q.id).KeepMinIvPercentage)) &&
                        !locsVisited.Contains(new PokemonLocation(q.latitude, q.longitude))
                        && !(q.timeStamp != default(DateTime) &&
                                q.timeStamp > new DateTime(2016) && // make absolutely sure that the server sent a correct datetime
                                q.timeStamp < DateTime.Now) && (q.id == PokemonId.Missingno || pokemonIds.Contains(q.id))).ToList();

                    if (locationsToSnipe.Any())
                    {
                        lastSnipe = DateTime.Now;
                        foreach (var location in locationsToSnipe)
                        {
                            var pokeBallsCount = await session.Inventory.GetItemAmountByType(POGOProtos.Inventory.Item.ItemId.ItemPokeBall);
                            var greatBallsCount = await session.Inventory.GetItemAmountByType(POGOProtos.Inventory.Item.ItemId.ItemGreatBall);
                            var ultraBallsCount = await session.Inventory.GetItemAmountByType(POGOProtos.Inventory.Item.ItemId.ItemUltraBall);
                            var masterBallsCount = await session.Inventory.GetItemAmountByType(POGOProtos.Inventory.Item.ItemId.ItemMasterBall);

                            if (pokeBallsCount + greatBallsCount + ultraBallsCount + masterBallsCount < session.LogicSettings.MinPokeballsToSnipe)
                                return;
                                    
                            session.EventDispatcher.Send(new SnipeScanEvent() { Bounds = new Location(location.latitude, location.longitude) });

                            await snipe(session, pokemonIds, location.latitude, location.longitude, cancellationToken);
                            locsVisited.Add(new PokemonLocation(location.latitude, location.longitude));
                        }
                    }
                }
                else

                    foreach (var location in session.LogicSettings.PokemonToSnipe.Locations)
                    {
                        session.EventDispatcher.Send(new SnipeScanEvent() { Bounds = location });

                        var scanResult = SnipeScanForPokemon(location);

                        var locationsToSnipe = scanResult.pokemon == null ? new List<PokemonLocation>() : scanResult.pokemon.Where(q =>
                            pokemonIds.Contains((PokemonId)q.pokemonId)
                            && !locsVisited.Contains(q)
                            && q.expiration_time < currentTimestamp
                            && q.is_alive).ToList();

                        if (locationsToSnipe.Any())
                        {
                            lastSnipe = DateTime.Now;
                            foreach (var pokemonLocation in locationsToSnipe)
                            {
                                var pokeBallsCount = await session.Inventory.GetItemAmountByType(POGOProtos.Inventory.Item.ItemId.ItemPokeBall);
                                var greatBallsCount = await session.Inventory.GetItemAmountByType(POGOProtos.Inventory.Item.ItemId.ItemGreatBall);
                                var ultraBallsCount = await session.Inventory.GetItemAmountByType(POGOProtos.Inventory.Item.ItemId.ItemUltraBall);
                                var masterBallsCount = await session.Inventory.GetItemAmountByType(POGOProtos.Inventory.Item.ItemId.ItemMasterBall);

                                if (pokeBallsCount + greatBallsCount + ultraBallsCount + masterBallsCount < session.LogicSettings.MinPokeballsToSnipe)
                                    return;

                                locsVisited.Add(pokemonLocation);

                                await snipe(session, pokemonIds, pokemonLocation.latitude, pokemonLocation.longitude, cancellationToken);
                            }
                        }
                        else
                        {
                            session.EventDispatcher.Send(new NoticeEvent()
                            {
                                Message = session.Translation.GetTranslation(Common.TranslationString.NoPokemonToSnipe)
                            });
                        }
                    }
            }

        }

        private static ScanResult SnipeScanForPokemon(Location location)
        {
            var formatter = new System.Globalization.NumberFormatInfo() { NumberDecimalSeparator = "." };
            var uri = $"https://pokevision.com/map/data/{location.Latitude.ToString(formatter)}/{location.Longitude.ToString(formatter)}";

            ScanResult scanResult;
            try
            {
                var request = WebRequest.CreateHttp(uri);
                request.Accept = "application/json";
                request.Method = "GET";
                request.Timeout = 1000;

                var resp = request.GetResponse();
                var reader = new StreamReader(resp.GetResponseStream());

                scanResult = JsonConvert.DeserializeObject<ScanResult>(reader.ReadToEnd());
            }
            catch (Exception)
            {
                scanResult = new ScanResult()
                {
                    status = "fail",
                    pokemon = new List<PokemonLocation>()
                };
            }
            return scanResult;
        }
    }
}
