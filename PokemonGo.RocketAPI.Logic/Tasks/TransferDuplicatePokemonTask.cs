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
            var duplicatePokemons =  ctx.Inventory.GetDuplicatePokemonToTransfer(false, ctx.LogicSettings.PrioritizeIVOverCP, ctx.LogicSettings.PokemonsNotToTransfer).Result;

            foreach (var duplicatePokemon in duplicatePokemons)
            {
                if (duplicatePokemon.Cp >= ctx.LogicSettings.KeepMinCP ||
                    PokemonInfo.CalculatePokemonPerfection(duplicatePokemon) > ctx.LogicSettings.KeepMinIVPercentage)
                {
                    continue;
                }

                ctx.Client.Inventory.TransferPokemon(duplicatePokemon.Id).Wait();
                ctx.Inventory.DeletePokemonFromInvById(duplicatePokemon.Id);

                var bestPokemonOfType = ctx.LogicSettings.PrioritizeIVOverCP
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
