using Microsoft.CSharp.RuntimeBinder;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.PoGoUtils;
using PoGo.NecroBot.Logic.State;
using POGOProtos.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PoGo.NecroBot.Logic.Tasks
{
    public static class LogBestPokemonTask
    {
        public static void Execute(Context ctx, StateMachine machine)
        {
            var stats = ctx.Inventory.GetPlayerStats().Result;
            var stat = stats.FirstOrDefault();
          
            var trainerLevel = stats != null ? stat.Level : 30;

            var highestsPokemonCp = ctx.Inventory.GetHighestsCp(ctx.LogicSettings.AmountOfPokemonToDisplayOnStart).Result;
            List<Tuple<PokemonData, int, double,double, double>> pokemonPairedWithStatsCP = new List<Tuple<PokemonData, int, double,double, double>>() ;
           
            foreach (var pokemon in highestsPokemonCp)
                pokemonPairedWithStatsCP.Add(Tuple.Create(pokemon, PokemonInfo.CalculateMaxCp(pokemon), PokemonInfo.CalculatePokemonPerfection(pokemon), PokemonInfo.CalculatePokemonBattleRating(pokemon, ctx.LogicSettings.PokemonBrPrioritizationIVWeightPercentage), PokemonInfo.GetLevel(pokemon)));

            var highestsPokemonPerfect = ctx.Inventory.GetHighestsPerfect(ctx.LogicSettings.AmountOfPokemonToDisplayOnStart).Result;

            List<Tuple<PokemonData, int, double, double, double>> pokemonPairedWithStatsIV = new List<Tuple<PokemonData, int, double, double, double>>();
            foreach (var pokemon in highestsPokemonPerfect)
                pokemonPairedWithStatsIV.Add(Tuple.Create(pokemon, PokemonInfo.CalculateMaxCp(pokemon), PokemonInfo.CalculatePokemonPerfection(pokemon), PokemonInfo.CalculatePokemonBattleRating(pokemon, ctx.LogicSettings.PokemonBrPrioritizationIVWeightPercentage), PokemonInfo.GetLevel(pokemon)));

            var highestsPokemonBr = ctx.Inventory.GetHighestsBR(ctx.LogicSettings.AmountOfPokemonToDisplayOnStart).Result;

            List<Tuple<PokemonData, int, double, double,double>> pokemonPairedWithStatsBR = new List<Tuple<PokemonData, int, double, double,double>>();
            foreach (var pokemon in highestsPokemonBr)
                pokemonPairedWithStatsBR.Add(Tuple.Create(pokemon, PokemonInfo.CalculateMaxCp(pokemon), PokemonInfo.CalculatePokemonPerfection(pokemon), PokemonInfo.CalculatePokemonBattleRating(pokemon, ctx.LogicSettings.PokemonBrPrioritizationIVWeightPercentage,trainerLevel), PokemonInfo.GetLevel(pokemon)));



            machine.Fire(               
                    new DisplayHighestsPokemonEvent
                    {
                        SortedBy = "Cp",
                        PokemonList = pokemonPairedWithStatsCP
                    });
 
                Thread.Sleep(500);
     

                machine.Fire(
                       new DisplayHighestsPokemonEvent
                       {
                           SortedBy = "Iv",
                           PokemonList = pokemonPairedWithStatsIV
                       });
 
                Thread.Sleep(500);

            machine.Fire(
                   new DisplayHighestsPokemonEvent
                   {
                       SortedBy = "Br",
                       PokemonList = pokemonPairedWithStatsBR
                   });

            Thread.Sleep(500);

        }
    }
}
