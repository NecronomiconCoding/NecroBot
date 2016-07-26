#region using directives

using System.Linq;
using System.Threading;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Utils;
using POGOProtos.Inventory.Item;
using POGOProtos.Map.Pokemon;
using POGOProtos.Networking.Responses;

#endregion

namespace PoGo.NecroBot.Logic.Tasks
{
    public static class CatchNearbyPokemonsTask
    {
        public static void Execute(Context ctx, StateMachine machine)
        {
            var invResp = ctx.Client.Inventory.GetInventory();
            var invRespRes = invResp.Result;
            if (invRespRes.Success)
            {
                var items = invResp.Result.InventoryDelta.InventoryItems;
                if (items.Select(rawItem => rawItem.InventoryItemData.Item).Any(item => (item.ItemId == ItemId.ItemPokeBall || item.ItemId == ItemId.ItemGreatBall || item.ItemId == ItemId.ItemUltraBall || item.ItemId == ItemId.ItemMasterBall) && item.Count == 0))
                {
                    machine.Fire(new WarnEvent {Message = "NO pokeballs, ignoring pokemon encounters"});
                    return;
                }
            }

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