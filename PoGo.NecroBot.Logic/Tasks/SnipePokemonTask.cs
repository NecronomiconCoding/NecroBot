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

namespace PoGo.NecroBot.Logic.Tasks
{
    public class PokemonLocation
    {
        public long id { get; set; }
        public double expiration_time { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string uid { get; set; }
        public bool is_alive { get; set; }
        public int pokemonId { get; set; }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public override string ToString()
        {
            return uid;
        }

        public override bool Equals(object obj)
        {
            return this.ToString().Equals(obj.ToString());
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

        public static async Task Execute(ISession session)
        {
            if (session.LogicSettings.PokemonToSnipe != null)
            {
                DateTime st = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                TimeSpan t = (DateTime.Now.ToUniversalTime() - st);
                var currentTimestamp = t.TotalMilliseconds;

                foreach (var location in session.LogicSettings.PokemonToSnipe.Locations)
                {
                    session.EventDispatcher.Send(new SnipeScanEvent() { Bounds = location });

                    var scanResult = SnipeScanForPokemon(location);

                    var pokemonIds =
                        session.LogicSettings.PokemonToSnipe.Pokemon.Select(
                            q => Enum.Parse(typeof(PokemonId), q) as PokemonId? ?? PokemonId.Missingno).Select(q => (int)q);


                    var locationsToSnipe = scanResult.pokemon == null ? new List<PokemonLocation>() : scanResult.pokemon.Where(q =>
                        pokemonIds.Contains(q.pokemonId)
                        && !locsVisited.Contains(q)
                        && q.expiration_time < currentTimestamp
                        && q.is_alive).ToList();

                    if (locationsToSnipe.Any())
                    {
                        foreach (var pokemonLocation in locationsToSnipe)
                        {
                            locsVisited.Add(pokemonLocation);

                            var currentLatitude = session.Client.CurrentLatitude;
                            var currentLongitude = session.Client.CurrentLongitude;

                            await
                                session.Client.Player.UpdatePlayerLocation(pokemonLocation.latitude,
                                    pokemonLocation.longitude, session.Client.CurrentAltitude);

                            session.EventDispatcher.Send(new UpdatePositionEvent()
                            {
                                Longitude = pokemonLocation.longitude,
                                Latitude = pokemonLocation.latitude
                            });

                            var mapObjects = session.Client.Map.GetMapObjects().Result;
                            var catchablePokemon =
                                mapObjects.MapCells.SelectMany(q => q.CatchablePokemons)
                                    .Where(q => pokemonIds.Contains((int)q.PokemonId))
                                    .ToList();

                            await session.Client.Player.UpdatePlayerLocation(currentLatitude, currentLongitude,
                                        session.Client.CurrentAltitude);

                            foreach (var pokemon in catchablePokemon)
                            {
                                await session.Client.Player.UpdatePlayerLocation(pokemonLocation.latitude, pokemonLocation.longitude, session.Client.CurrentAltitude);

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
                                    await Task.Delay(session.LogicSettings.DelayBetweenPokemonCatch);
                                }
                            }

                            await Task.Delay(session.LogicSettings.DelayBetweenPlayerActions);
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
            var uri = $"https://pokevision.com/map/data/{location.Latitude}/{location.Longitude}";
            ScanResult scanResult;
            try
            {
                var request = WebRequest.CreateHttp(uri);
                request.Accept = "application/json";
                request.Method = "GET";

                var resp = request.GetResponse();
                var reader = new StreamReader(resp.GetResponseStream());

                scanResult = JsonConvert.DeserializeObject<ScanResult>(reader.ReadToEnd());
            }
            catch (Exception e)
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
