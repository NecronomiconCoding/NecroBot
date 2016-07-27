using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.PoGoUtils;
using PoGo.NecroBot.Logic.State;
using POGOProtos.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PoGo.NecroBot.Logic.Tasks
{
    public static class LogBestPokemonTask
    {
        public static async Task Execute(Context ctx, StateMachine machine)
        {
            var highestsPokemonCp = await ctx.Inventory.GetHighestsCp(ctx.LogicSettings.AmountOfPokemonToDisplayOnStart);
            List<Tuple<PokemonData, int, double,double>> pokemonPairedWithStatsCP = new List<Tuple<PokemonData, int, double,double>>() ;
           
            foreach (var pokemon in highestsPokemonCp)
                pokemonPairedWithStatsCP.Add( Tuple.Create(pokemon, PokemonInfo.CalculateMaxCp(pokemon), PokemonInfo.CalculatePokemonPerfection(pokemon), PokemonInfo.GetLevel(pokemon)));

            var highestsPokemonPerfect = await ctx.Inventory.GetHighestsPerfect(ctx.LogicSettings.AmountOfPokemonToDisplayOnStart);

            List<Tuple<PokemonData, int, double, double>> pokemonPairedWithStatsIV = new List<Tuple<PokemonData, int, double, double>>();
            foreach (var pokemon in highestsPokemonPerfect)
                pokemonPairedWithStatsIV.Add(Tuple.Create(pokemon, PokemonInfo.CalculateMaxCp(pokemon), PokemonInfo.CalculatePokemonPerfection(pokemon), PokemonInfo.GetLevel(pokemon)));

            machine.Fire(               
                new DisplayHighestsPokemonEvent
                {
                    SortedBy = "Cp",
                    PokemonList = pokemonPairedWithStatsCP
                });

            await Utils.Statistics.RandomDelay(500);

            machine.Fire(
                    new DisplayHighestsPokemonEvent
                    {
                        SortedBy = "Iv",
                        PokemonList = pokemonPairedWithStatsIV
                    });
 
            await Utils.Statistics.RandomDelay(500);
        }
    }
}
