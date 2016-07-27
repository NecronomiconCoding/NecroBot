#region using directives

using System;
using System.Linq;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.State;
using PokemonGo.RocketAPI;
using POGOProtos.Inventory.Item;

#endregion

namespace PoGo.NecroBot.Logic.Tasks
{
    public class EvolvePokemonTask
    {
        private static DateTime _lastLuckyEggTime;

        public static async Task Execute(Context ctx, StateMachine machine)
        {
            var pokemonToEvolveTask = await ctx.Inventory.GetPokemonToEvolve(ctx.LogicSettings.PokemonsToEvolve);
            var pokemonToEvolve = pokemonToEvolveTask.ToList();

            if (pokemonToEvolve.Any())
            {
                if (ctx.LogicSettings.UseLuckyEggsWhileEvolving)
                {
                    if (pokemonToEvolve.Count >= ctx.LogicSettings.UseLuckyEggsMinPokemonAmount)
                    {
                        await UseLuckyEgg(ctx.Client, ctx.Inventory, machine);
                    }
                    else
                    {
                        // Wait until we have enough pokemon
                        return;
                    }
                }

                foreach (var pokemon in pokemonToEvolve)
                {
                    var evolveResponse = await ctx.Client.Inventory.EvolvePokemon(pokemon.Id);

                    machine.Fire(new PokemonEvolveEvent
                    {
                        Id = pokemon.PokemonId,
                        Exp = evolveResponse.ExperienceAwarded,
                        Result = evolveResponse.Result
                    });
                }
            }
        }

        public static async Task UseLuckyEgg(Client client, Inventory inventory, StateMachine machine)
        {
            var inventoryContent = await inventory.GetItems();

            var luckyEggs = inventoryContent.Where(p => p.ItemId == ItemId.ItemLuckyEgg);
            var luckyEgg = luckyEggs.FirstOrDefault();

            if (luckyEgg == null || luckyEgg.Count <= 0 || _lastLuckyEggTime.AddMinutes(30).Ticks > DateTime.Now.Ticks)
                return;

            _lastLuckyEggTime = DateTime.Now;
            await client.Inventory.UseItemXpBoost();
            await inventory.RefreshCachedInventory();
            machine.Fire(new UseLuckyEggEvent {Count = luckyEgg.Count});
            await Task.Delay(2000);
        }
    }
}