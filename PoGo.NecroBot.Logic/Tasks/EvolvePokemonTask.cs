#region using directives

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Utils;
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

            session.EventDispatcher.Send( new EvolveCountEvent
            {
                Evolves = pokemonToEvolve.Count
            } );

            if (pokemonToEvolve.Any())
            {
                if (session.LogicSettings.KeepPokemonsThatCanEvolve)
                {
                    var myPokemons = await session.Inventory.GetPokemons();
                    var needPokemonToStartEvolve = Math.Max(0,
                        Math.Min(session.Profile.PlayerData.MaxPokemonStorage*
                                 session.LogicSettings.EvolveKeptPokemonsAtStorageUsagePercentage,
                            session.Profile.PlayerData.MaxPokemonStorage));

                    var deltaCount = needPokemonToStartEvolve - myPokemons.Count();

                    if (deltaCount > 0)
                    {
                        session.EventDispatcher.Send(new NoticeEvent()
                        {
                            Message = $"Not enough Pokemons to start evlove. Waiting for {deltaCount} " +
                                      $"more ({ pokemonToEvolve.Count}/{deltaCount + pokemonToEvolve.Count})"
                        });

                        return;
                    }
                }

                var inventoryContent = await session.Inventory.GetItems();

                var luckyEggs = inventoryContent.Where(p => p.ItemId == ItemId.ItemLuckyEgg);
                var luckyEgg = luckyEggs.FirstOrDefault();

                if (session.LogicSettings.UseLuckyEggsWhileEvolving && luckyEgg != null && luckyEgg.Count > 0)
                {
                    if (pokemonToEvolve.Count >= session.LogicSettings.UseLuckyEggsMinPokemonAmount)
                    {
                        await UseLuckyEgg(session);
                    }
                    else
                    {
                        var myPokemons = await session.Inventory.GetPokemons();

                        var deltaPokemonsToUseLuckyEgg = session.LogicSettings.UseLuckyEggsMinPokemonAmount -
                                                                   pokemonToEvolve.Count;

                        var availableSpace = session.Profile.PlayerData.MaxPokemonStorage - myPokemons.Count();

                        if (deltaPokemonsToUseLuckyEgg > availableSpace)
                        {
                            var possibleLimitInThisIteration = pokemonToEvolve.Count + availableSpace;

                            session.EventDispatcher.Send(new NoticeEvent()
                            {
                                Message = $"Use lucky egg impossible with UseLuckyEggsMinPokemonAmount = {session.LogicSettings.UseLuckyEggsMinPokemonAmount} " +
                                          Environment.NewLine +
                                          $"set value <= {possibleLimitInThisIteration} instead"
                            });
                        }
                        else
                        {
                            session.EventDispatcher.Send(new NoticeEvent()
                            {
                                Message = $"Not enough Pokemons to trigger a lucky egg. Were needed {deltaPokemonsToUseLuckyEgg} more"
                            });
                        }
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
            if (luckyEgg != null) session.EventDispatcher.Send(new UseLuckyEggEvent {Count = luckyEgg.Count});
            DelayingUtils.Delay(session.LogicSettings.DelayBetweenPokemonCatch, 2000);
        }
    }
}
