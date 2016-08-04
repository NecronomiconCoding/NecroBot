#region using directives

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Utils;
using POGOProtos.Inventory.Item;
using PoGo.NecroBot.Logic.Common;
using System.Collections.Generic;
using POGOProtos.Data;

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
                    var totalPokemon = await session.Inventory.GetPokemons();

                    var pokemonNeededInInventory = session.Profile.PlayerData.MaxPokemonStorage * session.LogicSettings.EvolveKeptPokemonsAtStorageUsagePercentage / 100.0f;
                    var needPokemonToStartEvolve = Math.Round(
                        Math.Max(0,
                            Math.Min(pokemonNeededInInventory, session.Profile.PlayerData.MaxPokemonStorage)));

                    var deltaCount = needPokemonToStartEvolve - totalPokemon.Count();

                    if (deltaCount > 0)
                    {
                        session.EventDispatcher.Send(new NoticeEvent()
                        {
                            Message = session.Translation.GetTranslation(TranslationString.WaitingForMorePokemonToEvolve,
                                pokemonToEvolve.Count, deltaCount, totalPokemon.Count(), needPokemonToStartEvolve, session.LogicSettings.EvolveKeptPokemonsAtStorageUsagePercentage)
                        });
                        return;
                    }
                }

                if (await shouldUseLuckyEgg(session, pokemonToEvolve))
                {
                    await UseLuckyEgg(session);
                }
                await evolve(session, pokemonToEvolve);
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
            DelayingUtils.Delay(session.LogicSettings.DelayBetweenPlayerActions, 2000);
        }

        private static async Task evolve(ISession session, List<PokemonData> pokemonToEvolve)
        {
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
                if(!pokemonToEvolve.Last().Equals(pokemon))
                {
                    DelayingUtils.Delay(session.LogicSettings.DelayBetweenPlayerActions, 2000);
                }
            }
        }

        private static async Task<Boolean> shouldUseLuckyEgg(ISession session, List<PokemonData> pokemonToEvolve)
        {
            var inventoryContent = await session.Inventory.GetItems();

            var luckyEggs = inventoryContent.Where(p => p.ItemId == ItemId.ItemLuckyEgg);
            var luckyEgg = luckyEggs.FirstOrDefault();

            if (session.LogicSettings.UseLuckyEggsWhileEvolving && luckyEgg != null && luckyEgg.Count > 0)
            {
                if (pokemonToEvolve.Count >= session.LogicSettings.UseLuckyEggsMinPokemonAmount)
                {
                    return true;
                }
                else
                {
                    var evolvablePokemon = await session.Inventory.GetPokemons();

                    var deltaPokemonToUseLuckyEgg = session.LogicSettings.UseLuckyEggsMinPokemonAmount -
                                                               pokemonToEvolve.Count;

                    var availableSpace = session.Profile.PlayerData.MaxPokemonStorage - evolvablePokemon.Count();

                    if (deltaPokemonToUseLuckyEgg > availableSpace)
                    {
                        var possibleLimitInThisIteration = pokemonToEvolve.Count + availableSpace;

                        session.EventDispatcher.Send(new NoticeEvent()
                        {
                            Message = session.Translation.GetTranslation(TranslationString.UseLuckyEggsMinPokemonAmountTooHigh,
                                session.LogicSettings.UseLuckyEggsMinPokemonAmount, possibleLimitInThisIteration)
                        });
                    }
                    else
                    {
                        session.EventDispatcher.Send(new NoticeEvent()
                        {
                            Message = session.Translation.GetTranslation(TranslationString.CatchMorePokemonToUseLuckyEgg,
                                deltaPokemonToUseLuckyEgg)
                        });
                    }
                }
            }
            return false;
        }
    }
}
