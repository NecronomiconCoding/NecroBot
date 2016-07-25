using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGo.RocketAPI.Logic.State
{
    public class FarmState : IState
    {
        public IState Execute(Context ctx, StateMachine machine)
        {
            if(ctx.LogicSettings.EvolveAllPokemonAboveIV || ctx.LogicSettings.EvolveAllPokemonWithEnoughCandy)
            {
                Tasks.EvolvePokemonTask.Execute(ctx, machine);
            }

            if(ctx.LogicSettings.TransferDuplicatePokemon)
            {
                Tasks.TransferDuplicatePokemonTask.Execute(ctx, machine);
            }

            Tasks.RecycleItemsTask.Execute(ctx, machine);

            if(ctx.LogicSettings.UseGPXPathing)
            {
                Tasks.FarmPokestopsGPXTask.Execute(ctx, machine);
            }
            else
            {
                Tasks.FarmPokestopsTask.Execute(ctx, machine);
            }

            machine.RequestDelay(10000);

            return this;
        }
    }
}
