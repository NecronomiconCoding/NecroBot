#region using directives

using System.Linq;
using System.Threading;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Utils;
using POGOProtos.Map.Pokemon;
using POGOProtos.Networking.Responses;

#endregion

namespace PoGo.NecroBot.Logic.Tasks
{
    public static class CatchNearbyPokemonsTask
    {
        public static void Execute(Context ctx, StateMachine machine)
        {
            Logger.Write("Looking for pokemon..", LogLevel.Debug);

            var pokemons = GetNearbyPokemons(ctx);
            foreach (var pokemon in pokemons)
            {
                if (ctx.LogicSettings.UsePokemonToNotCatchFilter &&
                    ctx.LogicSettings.PokemonsNotToCatch.Contains(pokemon.PokemonId))
                {
                    Logger.Write("Skipped " + pokemon.PokemonId);
                    continue;
                }

                if (ctx.Inventory.GetPokemons().Result.Any(p => p.PokemonId == pokemon.PokemonId) && ctx.LogicSettings.DontCatchDuplicatedPokemon)
                {
                    Logger.Write("Already Have " + pokemon.PokemonId + " Skipped");
                    continue;
                }

                var distance = LocationUtils.CalculateDistanceInMeters(ctx.Client.CurrentLatitude,
                    ctx.Client.CurrentLongitude, pokemon.Latitude, pokemon.Longitude);
                Thread.Sleep(distance > 100 ? 15000 : 500);

                var encounter = ctx.Client.Encounter.EncounterPokemon(pokemon.EncounterId, pokemon.SpawnPointId).Result;

                if (encounter.Status == EncounterResponse.Types.Status.EncounterSuccess)
                {
                    CatchPokemonTask.Execute(ctx, machine, encounter, pokemon);
                }
                else
                {
                    machine.Fire(new WarnEvent {Message = $"Encounter problem: {encounter.Status}"});
                }

                // If pokemon is not last pokemon in list, create delay between catches, else keep moving.
                if (!Equals(pokemons.ElementAtOrDefault(pokemons.Count() - 1), pokemon))
                {
                    Thread.Sleep(ctx.LogicSettings.DelayBetweenPokemonCatch);
                }
            }
        }

        private static IOrderedEnumerable<MapPokemon> GetNearbyPokemons(Context ctx)
        {
            var mapObjects = ctx.Client.Map.GetMapObjects().Result;

            var pokemons = mapObjects.MapCells.SelectMany(i => i.CatchablePokemons)
                .OrderBy(
                    i =>
                        LocationUtils.CalculateDistanceInMeters(ctx.Client.CurrentLatitude, ctx.Client.CurrentLongitude,
                            i.Latitude, i.Longitude));

            return pokemons;
        }
    }
}