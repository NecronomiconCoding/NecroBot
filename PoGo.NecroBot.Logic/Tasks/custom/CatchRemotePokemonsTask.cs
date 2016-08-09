using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Utils;
using POGOProtos.Enums;
using POGOProtos.Map.Pokemon;
using POGOProtos.Networking.Responses;
using PokemonGo.RocketAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PoGo.NecroBot.Logic.Tasks.custom
{
    public class CatchRemotePokemonsTask
    {
        private static ICollection<PokemonId> PokemonsFlyToCatch = new PokemonId[] { PokemonId.Snorlax, PokemonId.Paras, PokemonId.Dragonite };
        private static List<PokemonId> rarePokemonIds = RarePokemonsFactory.createRarePokemonList();
        public static async Task Execute(ISession session, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (!FarmControl.stoped || !FarmControl.flying || FarmControl.flyLatitude == 0 || FarmControl.flyLongitude == 0)
            {
                return;
            }
            FarmControl.flying = false;
            if(FarmControl.autoFarm)
            {
                FarmControl.stopping = false;
            }            
            Client _client = session.Client;
            double currentLatitude = session.Client.CurrentLatitude;
            double currentLongitude = session.Client.CurrentLongitude;
            try
            {
                var result = await _client.Player.UpdatePlayerLocation(FarmControl.flyLatitude, FarmControl.flyLongitude, _client.Settings.DefaultAltitude);
                if (result == null)
                {
                    return;
                }
                session.EventDispatcher.Send(new UpdatePositionEvent()
                {
                    Latitude = FarmControl.flyLatitude,
                    Longitude = FarmControl.flyLongitude
                });
                await Task.Delay(500);
                Logger.Write(session.Translation.GetTranslation(Common.TranslationString.LookingForPokemon), LogLevel.Debug);

                var pokemons = await GetNearbyPokemons(session);
                Logger.Write(string.Format("here is {0}, {1}", session.Client.CurrentLatitude, session.Client.CurrentLongitude));
                bool isFound = false;
                string names = "";
                foreach (var pokemon in pokemons)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    names += pokemon.PokemonId.ToString() + " ";
                    if (rarePokemonIds.Contains(pokemon.PokemonId) || PokemonsFlyToCatch.Contains(pokemon.PokemonId)
                        || FarmControl.flyCatchName == pokemon.PokemonId.ToString())
                    {
                        Logger.Write(string.Format("Fly catching {0}", pokemon.PokemonId));
                    }
                    else
                    {
                        continue;
                    }

                    var distance = LocationUtils.CalculateDistanceInMeters(session.Client.CurrentLatitude,
                        session.Client.CurrentLongitude, pokemon.Latitude, pokemon.Longitude);
                    await Task.Delay(distance > 100 ? 3000 : 500);

                    var encounter =
                    await session.Client.Encounter.EncounterPokemon(pokemon.EncounterId, pokemon.SpawnPointId);

                    if (encounter.Status == EncounterResponse.Types.Status.EncounterSuccess)
                    {
                        Logger.Write(string.Format("Fly meeting success {0}", pokemon.PokemonId));
                        result = await _client.Player.UpdatePlayerLocation(currentLatitude, currentLongitude, _client.Settings.DefaultAltitude);
                        if (result == null)
                        {
                            return;
                        }
                        session.EventDispatcher.Send(new UpdatePositionEvent()
                        {
                            Latitude = currentLatitude,
                            Longitude = currentLongitude
                        });
                        await Task.Delay(500);
                        await CatchPokemonTask.Execute(session, cancellationToken, encounter, pokemon);
                    }
                    else
                    {
                        Logger.Write(string.Format("Encounter problem: {0}", encounter.Status), LogLevel.Error);
                    }
                    isFound = true;
                    break;
                }
                if (!isFound)
                {
                    Logger.Write("Just Found :" + names + "  You can type:x,y,name to catch", LogLevel.Error);
                }
            }
            finally
            {
                var result = await _client.Player.UpdatePlayerLocation(currentLatitude, currentLongitude, _client.Settings.DefaultAltitude);
                session.EventDispatcher.Send(new UpdatePositionEvent()
                {
                    Latitude = currentLatitude,
                    Longitude = currentLongitude
                });
                Logger.Write("Fly Over!");
                await Task.Delay(2000);
            }
        }

        private static async Task<IOrderedEnumerable<MapPokemon>> GetNearbyPokemons(ISession session)
        {
            var mapObjects = await session.Client.Map.GetMapObjects();

            var pokemons = mapObjects.Item1.MapCells.SelectMany(i => i.CatchablePokemons)
                .OrderBy(
                    i =>
                        LocationUtils.CalculateDistanceInMeters(session.Client.CurrentLatitude,
                            session.Client.CurrentLongitude,
                            i.Latitude, i.Longitude));

            return pokemons;
        }
    }
}
