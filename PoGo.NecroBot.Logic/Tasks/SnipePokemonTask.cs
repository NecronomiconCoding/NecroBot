﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.State;
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

                foreach (var snipeSettings in session.LogicSettings.PokemonToSnipe)
                {
                    var pokemonName = snipeSettings.Pokemon;
                    PokemonId pokemonId = Enum.Parse(typeof(PokemonId), pokemonName) as PokemonId? ??
                                          PokemonId.Missingno;

                    foreach (var location in snipeSettings.Locations)
                    {
                        session.EventDispatcher.Send(new SnipeScanEvent()
                        {
                            Bounds = location,
                            Pokemon = pokemonName
                        });

                        var scanResult = SnipeScanForPokemon(location);

                        var locationsToSnipe = scanResult.pokemon.Where(q =>
                            q.pokemonId == (int)pokemonId
                            && !locsVisited.Contains(q)
                            && q.expiration_time < currentTimestamp
                            && q.is_alive).ToList();

                        if (locationsToSnipe.Any())
                        {
                            foreach (var pokemonLocation in locationsToSnipe)
                            {
                                locsVisited.Add(pokemonLocation);

                                await
                                    session.Client.Player.UpdatePlayerLocation(pokemonLocation.latitude,
                                        pokemonLocation.longitude, session.Settings.DefaultAltitude);

                                session.EventDispatcher.Send(new UpdatePositionEvent()
                                {
                                    Longitude = pokemonLocation.longitude,
                                    Latitude = pokemonLocation.latitude
                                });

                                var mapObjects = session.Client.Map.GetMapObjects().Result;
                                var catchablePokemon =
                                    mapObjects.MapCells.SelectMany(q => q.CatchablePokemons)
                                        .Where(q => q.PokemonId == pokemonId)
                                        .ToList();

                                foreach (var pokemon in catchablePokemon)
                                {
                                    var encounter =
                                        session.Client.Encounter.EncounterPokemon(pokemon.EncounterId,
                                            pokemon.SpawnPointId).Result;

                                    if (encounter.Status == EncounterResponse.Types.Status.EncounterSuccess)
                                    {
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
                                        await Task.Delay(5000);
                                    }
                                }

                                await
                                    session.Client.Player.UpdatePlayerLocation(session.Settings.DefaultLatitude,
                                        session.Settings.DefaultLongitude, session.Settings.DefaultAltitude);

                                session.EventDispatcher.Send(new UpdatePositionEvent()
                                {
                                    Latitude = session.Settings.DefaultLatitude,
                                    Longitude = session.Settings.DefaultLongitude
                                });

                                await Task.Delay(1000);
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

        }

        private static ScanResult SnipeScanForPokemon(Location location)
        {
            var uri = $"https://pokevision.com/map/data/{location.Latitude}/{location.Longitude}";

            var request = WebRequest.CreateHttp(uri);
            request.Accept = "application/json";
            request.Method = "GET";

            var resp = request.GetResponse();
            var reader = new StreamReader(resp.GetResponseStream());

            var scanResult = JsonConvert.DeserializeObject<ScanResult>(reader.ReadToEnd());

            return scanResult;
        } 
    }
}
