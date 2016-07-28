#region using directives

using System;
using System.Linq;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.PoGoUtils;
using PoGo.NecroBot.Logic.State;

#endregion

namespace PoGo.NecroBot.Logic.Tasks
{
    public class DisplayPokemonStatsTask
    {
        public static async Task Execute(ISession session)
        {
            var highestsPokemonCp = await session.Inventory.GetHighestsCp(session.LogicSettings.AmountOfPokemonToDisplayOnStart);
            var pokemonPairedWithStatsCp = highestsPokemonCp.Select(pokemon => Tuple.Create(pokemon, PokemonInfo.CalculateMaxCp(pokemon), PokemonInfo.CalculatePokemonPerfection(pokemon), PokemonInfo.GetLevel(pokemon))).ToList();

            var highestsPokemonPerfect =
                await session.Inventory.GetHighestsPerfect(session.LogicSettings.AmountOfPokemonToDisplayOnStart);

            var pokemonPairedWithStatsIv = highestsPokemonPerfect.Select(pokemon => Tuple.Create(pokemon, PokemonInfo.CalculateMaxCp(pokemon), PokemonInfo.CalculatePokemonPerfection(pokemon), PokemonInfo.GetLevel(pokemon))).ToList();

            session.EventDispatcher.Send(
                new DisplayHighestsPokemonEvent
                {
                    SortedBy = "CP",
                    PokemonList = pokemonPairedWithStatsCp
                });

            await Task.Delay(500);

            session.EventDispatcher.Send(
                new DisplayHighestsPokemonEvent
                {
                    SortedBy = "IV",
                    PokemonList = pokemonPairedWithStatsIv
                });

            await Task.Delay(500);
        }
    }
}
