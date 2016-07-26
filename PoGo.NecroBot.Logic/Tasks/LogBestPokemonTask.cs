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
            var highestsPokemonCp = ctx.Inventory.GetHighestsCp(ctx.LogicSettings.AmountOfPokemonToDisplayOnStart).Result;
            List<Tuple<PokemonData, int, double,double>> pokemonPairedWithStatsCP = new List<Tuple<PokemonData, int, double,double>>() ;
           
            foreach (var pokemon in highestsPokemonCp)
                pokemonPairedWithStatsCP.Add( Tuple.Create(pokemon, PokemonInfo.CalculateMaxCp(pokemon), PokemonInfo.CalculatePokemonPerfection(pokemon), PokemonInfo.GetLevel(pokemon)));

            var highestsPokemonPerfect = ctx.Inventory.GetHighestsPerfect(ctx.LogicSettings.AmountOfPokemonToDisplayOnStart).Result;

            List<Tuple<PokemonData, int, double, double>> pokemonPairedWithStatsIV = new List<Tuple<PokemonData, int, double, double>>();
            foreach (var pokemon in highestsPokemonPerfect)
                pokemonPairedWithStatsIV.Add(Tuple.Create(pokemon, PokemonInfo.CalculateMaxCp(pokemon), PokemonInfo.CalculatePokemonPerfection(pokemon), PokemonInfo.GetLevel(pokemon)));



            // --------------I know this is ugly AF will refactor this asap-----------------
            try
            {

                machine.Fire(               
                    new DisplayHighestsPokemonEvent
                    {
                        sortetBy = "Cp",
                        pokemonList = pokemonPairedWithStatsCP
                    });
            }
            catch (RuntimeBinderException e)
            {
                Thread.Sleep(500);
            }

            try
            {
                machine.Fire(
                       new DisplayHighestsPokemonEvent
                       {
                           sortetBy = "Iv",
                           pokemonList = pokemonPairedWithStatsIV
                       });
            }
            catch (RuntimeBinderException e)
            {
                Thread.Sleep(500);
            }


        }
    }
}
