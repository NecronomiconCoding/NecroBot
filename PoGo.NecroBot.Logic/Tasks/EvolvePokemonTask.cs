#region using directives

using System;
using System.Linq;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.State;
using PokemonGo.RocketAPI;
using POGOProtos.Inventory.Item;
using PoGo.NecroBot.Logic.Utils;

#endregion

namespace PoGo.NecroBot.Logic.Tasks
{
    public class EvolvePokemonTask
    {
        private static DateTime _lastLuckyEggTime;

        public static async Task Execute(ISession session)
        {
            var pokemonToEvolveTask = await session.Inventory.GetPokemonToEvolve(session.LogicSettings.PokemonsToEvolve);
            var pokemonToEvolve = pokemonToEvolveTask.ToList();

            if (pokemonToEvolve.Any())
            {
                if (session.LogicSettings.UseLuckyEggsWhileEvolving)
                {
                    if (pokemonToEvolve.Count >= session.LogicSettings.UseLuckyEggsMinPokemonAmount)
                    {
                        await UseLuckyEgg(session);

                        await Randomizer.Sleep(2500, 0.2);
                    }
                    else
                    {
                        // Wait until we have enough pokemon
                        return;
                    }
                }

                foreach (var pokemon in pokemonToEvolve)
                {
                    var evolveResponse = await session.Client.Inventory.EvolvePokemon(pokemon.Id);

                    session.EventDispatcher.Send(new PokemonEvolveEvent
                    {
                        Id = pokemon.PokemonId,
                        Exp = evolveResponse.ExperienceAwarded,
                        Result = evolveResponse.Result
                    });
                    await Randomizer.Sleep(4000, 0.3);
                }
            }
        }

        public static async Task UseLuckyEgg(ISession session)
        {
            var inventoryContent = await session.Inventory.GetItems();

            var luckyEggs = inventoryContent.Where(p => p.ItemId == ItemId.ItemLuckyEgg);
            var luckyEgg = luckyEggs.FirstOrDefault();

            if (luckyEgg == null || luckyEgg.Count <= 0 || _lastLuckyEggTime.AddMinutes(30).Ticks > DateTime.Now.Ticks)
                return;

            _lastLuckyEggTime = DateTime.Now;
            await session.Client.Inventory.UseItemXpBoost();
            await session.Inventory.RefreshCachedInventory();
            session.EventDispatcher.Send(new UseLuckyEggEvent {Count = luckyEgg.Count});
        }
    }
}