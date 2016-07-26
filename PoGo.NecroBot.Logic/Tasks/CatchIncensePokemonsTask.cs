using System.Threading;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Utils;
using POGOProtos.Enums;
using POGOProtos.Map.Pokemon;
using POGOProtos.Networking.Responses;

namespace PoGo.NecroBot.Logic.Tasks
{
    public static class CatchIncensePokemonsTask
    {
        public static void Execute(Context ctx, StateMachine machine)
        {
            Logger.Write("Looking for incense pokemon..", LogLevel.Debug);


            var incensePokemon = ctx.Client.Map.GetIncensePokemons().Result;
            if (incensePokemon.Result == GetIncensePokemonResponse.Types.Result.IncenseEncounterAvailable)
            {
                var pokemon = new MapPokemon
                {
                    EncounterId = incensePokemon.EncounterId,
                    ExpirationTimestampMs = incensePokemon.DisappearTimestampMs,
                    Latitude = incensePokemon.Latitude,
                    Longitude = incensePokemon.Longitude,
                    PokemonId = (PokemonId) incensePokemon.PokemonTypeId,
                    SpawnPointId = incensePokemon.EncounterLocation
                };

                if (ctx.LogicSettings.UsePokemonToNotCatchFilter &&
                    ctx.LogicSettings.PokemonsNotToCatch.Contains(pokemon.PokemonId))
                {
                    Logger.Write("Skipped " + pokemon.PokemonId);
                }
                else
                {
                    var distance = LocationUtils.CalculateDistanceInMeters(ctx.Client.CurrentLatitude,
                        ctx.Client.CurrentLongitude, pokemon.Latitude, pokemon.Longitude);
                    Thread.Sleep(distance > 100 ? 15000 : 500);

                    var encounter =
                        ctx.Client.Encounter.EncounterIncensePokemon((long) pokemon.EncounterId, pokemon.SpawnPointId)
                            .Result;

                    if (encounter.Result == IncenseEncounterResponse.Types.Result.IncenseEncounterSuccess)
                    {
                        CatchPokemonTask.Execute(ctx, machine, encounter, pokemon);
                    }
                    else
                    {
                        machine.Fire(new WarnEvent {Message = $"Encounter problem: {encounter.Result}"});
                    }
                }
            }
        }
    }
}