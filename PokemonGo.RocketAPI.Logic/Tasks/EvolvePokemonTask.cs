using PokemonGo.RocketAPI.GeneratedCode;
using PokemonGo.RocketAPI.Logic.Event;
using PokemonGo.RocketAPI.Logic.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PokemonGo.RocketAPI.Logic.Tasks
{
    public class EvolvePokemonTask
    {
        public static void UseLuckyEgg(Client client, Inventory inventory, StateMachine machine)
        {
            var inventoryContent = inventory.GetItems().Result;

            var luckyEggs = inventoryContent.Where(p => (ItemId)p.Item_ == ItemId.ItemLuckyEgg);
            var luckyEgg = luckyEggs.FirstOrDefault();

            if (luckyEgg == null || luckyEgg.Count <= 0)
                return;

            client.UseItemXpBoost(ItemId.ItemLuckyEgg).Wait();

            luckyEgg.Count -= 1;
            machine.Fire(new UseLuckyEggEvent { Count = luckyEgg.Count });

            Thread.Sleep(2000);
        }

        public static void Execute(Context ctx, StateMachine machine)
        {
            if(ctx.Settings.useLuckyEggsWhileEvolving)
            {
                UseLuckyEgg(ctx.Client, ctx.Inventory, machine);
            }

            var pokemonToEvolveTask = ctx.Inventory.GetPokemonToEvolve(ctx.Settings.PokemonsToEvolve);
            pokemonToEvolveTask.Wait();

            var pokemonToEvolve = pokemonToEvolveTask.Result;
            foreach (var pokemon in pokemonToEvolve)
            {
                var evolveTask = ctx.Client.EvolvePokemon(pokemon.Id);
                evolveTask.Wait();

                var evolvePokemonOutProto = evolveTask.Result;

                machine.Fire(new PokemonEvolveEvent { Id = pokemon.PokemonId, Exp = evolvePokemonOutProto.ExpAwarded, Result = evolvePokemonOutProto.Result });

                Thread.Sleep(3000);
            }
        }
    }
}
