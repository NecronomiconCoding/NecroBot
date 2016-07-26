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
<<<<<<< HEAD
            if (ctx.LogicSettings.AmountOfPokemonToDisplayOnStart > 0)
                LogBestPokemonTask.Execute(ctx,machine);
            if(ctx.LogicSettings.ExportPokemonToCSV) ExportListPokemonTask.Execute(ctx, machine);
=======
            if(ctx.LogicSettings.AmountOfPokemonToDisplayOnStart > 0)
                await LogBestPokemonTask.Execute(ctx,machine);
>>>>>>> refs/remotes/NecronomiconCoding/master

            return new PositionCheckState();
        }
    }
}
