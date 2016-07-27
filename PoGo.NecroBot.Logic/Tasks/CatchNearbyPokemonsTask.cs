#region using directives

using System.Linq;
using System.Threading.Tasks;
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
        public static async Task Execute(Context ctx, StateMachine machine)
        {
            Logger.Write("Looking for pokemon..", LogLevel.Debug);

            var pokemons = await GetNearbyPokemons(ctx);
            foreach (var pokemon in pokemons)
            {
                if (ctx.LogicSettings.UsePokemonToNotCatchFilter &&
                    ctx.LogicSettings.PokemonsNotToCatch.Contains(pokemon.PokemonId))
                {
                    Logger.Write("Skipped " + pokemon.PokemonId);
                    continue;
                }

                var distance = LocationUtils.CalculateDistanceInMeters(ctx.Client.CurrentLatitude,
                    ctx.Client.CurrentLongitude, pokemon.Latitude, pokemon.Longitude);
                distance = distance > 41 ? (distance - LogicClient.variation("25,40")) : distance;
                double walking_speed = ctx.LogicSettings.WalkingSpeedInKilometerPerHour;

                double cws_in_kps = (((walking_speed / 60) / 60));
                double time_to_next = (distance / cws_in_kps);
                //TODO:update to propper Event Fire with translate...
                Logger.Write($"Encounter {pokemon.PokemonId} in ({System.Math.Round(distance)}m), ~{System.Math.Round(time_to_next / 1000)} seconds. Current walking speed is {System.Math.Round((cws_in_kps * 1000), 3)}m/s.", LogLevel.Info);
                await Task.Delay((int)time_to_next);

                var encounter = await ctx.Client.Encounter.EncounterPokemon(pokemon.EncounterId, pokemon.SpawnPointId);

                if (encounter.Status == EncounterResponse.Types.Status.EncounterSuccess)
                {
                    await CatchPokemonTask.Execute(ctx, machine, encounter, pokemon);
                }
                else if (encounter.Status == EncounterResponse.Types.Status.PokemonInventoryFull)
                {
                    if (ctx.LogicClient.Settings.TransferDuplicatePokemon)
                    {
                        machine.Fire(new WarnEvent {Message = "PokemonInventory is Full.Transferring pokemons..."});
                        await TransferDuplicatePokemonTask.Execute(ctx, machine);
                    }
                    else
                        machine.Fire(new WarnEvent
                        {
                            Message =
                                "PokemonInventory is Full.Please Transfer pokemon manually or set TransferDuplicatePokemon to true in settings..."
                        });
                }
                else
                {
                    machine.Fire(new WarnEvent {Message = $"Encounter problem: {encounter.Status}"});
                }
            }
        }

        private static async Task<IOrderedEnumerable<MapPokemon>> GetNearbyPokemons(Context ctx)
        {
            var mapObjects = await ctx.Client.Map.GetMapObjects();

            var pokemons = mapObjects.MapCells.SelectMany(i => i.CatchablePokemons)
                .OrderBy(
                    i =>
                        LocationUtils.CalculateDistanceInMeters(ctx.Client.CurrentLatitude, ctx.Client.CurrentLongitude,
                            i.Latitude, i.Longitude));

            return pokemons;
        }
    }
}