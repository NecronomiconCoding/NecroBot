#region using directives

using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Utils;
using POGOProtos.Enums;
using POGOProtos.Map.Pokemon;
using POGOProtos.Networking.Responses;

#endregion

namespace PoGo.NecroBot.Logic.Tasks
{
    public static class CatchIncensePokemonsTask
    {
        public static async Task Execute(Context ctx, StateMachine machine)
        {
            Logger.Write("Looking for incense pokemon..", LogLevel.Debug);


            var incensePokemon = await ctx.Client.Map.GetIncensePokemons();
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
                    await Task.Delay(distance > 100 ? 3000 : 500);

                    var encounter =
                        await
                            ctx.Client.Encounter.EncounterIncensePokemon((long) pokemon.EncounterId,
                                pokemon.SpawnPointId);

                    if (encounter.Result == IncenseEncounterResponse.Types.Result.IncenseEncounterSuccess)
                    {
                        await CatchPokemonTask.Execute(ctx, machine, encounter, pokemon);
                    }
                    else if (encounter.Result == IncenseEncounterResponse.Types.Result.PokemonInventoryFull)
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
                        machine.Fire(new WarnEvent {Message = $"Encounter problem: {encounter.Result}"});
                    }
                }
            }
        }
    }
}