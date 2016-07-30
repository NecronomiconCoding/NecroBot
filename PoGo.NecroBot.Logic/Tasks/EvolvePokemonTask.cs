#region using directives

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Utils;
using PokemonGo.RocketAPI;
using POGOProtos.Inventory.Item;

#endregion

namespace PoGo.NecroBot.Logic.Tasks
{
    public class EvolvePokemonTask
    {
        private static DateTime _lastLuckyEggTime;

        public static async Task Execute(ISession session, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var pokemonToEvolveTask = await session.Inventory.GetPokemonToEvolve(session.LogicSettings.PokemonsToEvolve);
            var pokemonToEvolve = pokemonToEvolveTask.ToList();
            Random rand = new Random();
            int RandomNumber = rand.Next(1, 20);
            var UpgradeResult = await session.Inventory.UpgradePokemon(DisplayPokemonStatsTask.PokemonID[RandomNumber]);
            if (UpgradeResult.Result.ToString().ToLower().Contains("success"))
            {
                Logging.Logger.Write("Pokemon Upgraded:" + UpgradeResult.UpgradedPokemon.PokemonId + ":" + UpgradeResult.UpgradedPokemon.Cp);
            }
            else if (UpgradeResult.Result.ToString().ToLower().Contains("insufficient"))
            {
                Logging.Logger.Write("Pokemon Upgrade Failed Not Enough Resources");

            }
            else
            {
                string fgds = "";
            }
            Random rand2 = new Random();
            int RandomNumber2 = rand2.Next(1, 20);
            var UpgradeResult2 = await session.Inventory.UpgradePokemon(DisplayPokemonStatsTask.PokemonIDCP[RandomNumber]);
            if (UpgradeResult2.Result.ToString().ToLower().Contains("success"))
            {
                Logging.Logger.Write("Pokemon Upgraded:" + UpgradeResult2.UpgradedPokemon.PokemonId + ":" + UpgradeResult2.UpgradedPokemon.Cp);
            }
            else if (UpgradeResult2.Result.ToString().ToLower().Contains("insufficient"))
            {
                Logging.Logger.Write("Pokemon Upgrade Failed Not Enough Resources");

            }
            else
            {
                string fgds = "";
            }
            var fhfds = await session.Inventory.GetPokedexCount() - 1;
            Logging.Logger.Write(fhfds.ToString() + " Pokemons caught out of 152");





            if (pokemonToEvolve.Any())
            {
                var inventoryContent = await session.Inventory.GetItems();

                var luckyEggs = inventoryContent.Where(p => p.ItemId == ItemId.ItemLuckyEgg);
                var luckyEgg = luckyEggs.FirstOrDefault();
                
                //maybe there can be a warning message as an else condition of luckyEgg checks, like; 
                //"There is no Lucky Egg, so, your UseLuckyEggsMinPokemonAmount setting bypassed."
                if (session.LogicSettings.UseLuckyEggsWhileEvolving && luckyEgg != null && luckyEgg.Count > 0)
                {
                    if (pokemonToEvolve.Count >= session.LogicSettings.UseLuckyEggsMinPokemonAmount)
                    {
                        await UseLuckyEgg(session);
                    }
                    else
                    {
                        // Wait until we have enough pokemon
                        return;
                    }
                }

                foreach (var pokemon in pokemonToEvolve)
                {
                    // no cancellationToken.ThrowIfCancellationRequested here, otherwise the lucky egg would be wasted.
                    var evolveResponse = await session.Client.Inventory.EvolvePokemon(pokemon.Id);

                    session.EventDispatcher.Send(new PokemonEvolveEvent
                    {
                        Id = pokemon.PokemonId,
                        Exp = evolveResponse.ExperienceAwarded,
                        Result = evolveResponse.Result
                    });
                }
            }
        }

        public static async Task UseLuckyEgg(ISession session)
        {
            var inventoryContent = await session.Inventory.GetItems();

            var luckyEggs = inventoryContent.Where(p => p.ItemId == ItemId.ItemLuckyEgg);
            var luckyEgg = luckyEggs.FirstOrDefault();

            if (_lastLuckyEggTime.AddMinutes(30).Ticks > DateTime.Now.Ticks)
                return;

            _lastLuckyEggTime = DateTime.Now;
            await session.Client.Inventory.UseItemXpBoost();
            await session.Inventory.RefreshCachedInventory();
            session.EventDispatcher.Send(new UseLuckyEggEvent {Count = luckyEgg.Count});
            DelayingUtils.Delay(session.LogicSettings.DelayBetweenPokemonCatch, 2000);
        }
    }
}
