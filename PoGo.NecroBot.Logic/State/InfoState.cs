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
        public IState Execute(Context ctx, StateMachine machine)
        {

            LogBestPokemonTask.Execute(ctx,machine);

            return new PositionCheckState();
        }
    }
}
