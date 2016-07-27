using PoGo.NecroBot.Logic.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoGo.NecroBot.Logic.State
{
    public class InfoState : IState
    {
        public async Task<IState> Execute(Context ctx, StateMachine machine)
        {
            if(ctx.LogicSettings.AmountOfPokemonToDisplayOnStart > 0)
                await LogBestPokemonTask.Execute(ctx,machine);

            return new PositionCheckState();
        }
    }
}
