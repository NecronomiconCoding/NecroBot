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
    public class TransferDuplicatePokemonTask
    {
        public static void Execute(Context ctx, StateMachine machine)
        {
            var duplicatePokemons =  ctx.Inventory.GetDuplicatePokemonToTransfer(false, ctx.Settings.PrioritizeIVOverCP, ctx.Settings.PokemonsNotToTransfer).Result;

            foreach (var duplicatePokemon in duplicatePokemons)
            {
                if (duplicatePokemon.Cp >= ctx.Settings.KeepMinCP ||
                    PokemonInfo.CalculatePokemonPerfection(duplicatePokemon) > ctx.Settings.KeepMinIVPercentage)
                {
                    continue;
                }

                ctx.Client.TransferPokemon(duplicatePokemon.Id).Wait();
                ctx.Inventory.DeletePokemonFromInvById(duplicatePokemon.Id);

                var bestPokemonOfType = ctx.Settings.PrioritizeIVOverCP
                    ? ctx.Inventory.GetHighestPokemonOfTypeByIv(duplicatePokemon).Result
                    : ctx.Inventory.GetHighestPokemonOfTypeByCp(duplicatePokemon).Result;

                machine.Fire(new TransferPokemonEvent
                {
                    Id = duplicatePokemon.PokemonId,
                    Perfection = PokemonInfo.CalculatePokemonPerfection(duplicatePokemon),
                    Cp = duplicatePokemon.Cp,
                    BestCp = bestPokemonOfType.Cp,
                    BestPerfection = PokemonInfo.CalculatePokemonPerfection(bestPokemonOfType)
                });
                
                
               
            }
        }
    }
}
