#region using directives

using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Tasks;

#endregion

namespace PoGo.NecroBot.Logic.State
{
    public class InfoState : IState
    {
        public async Task<IState> Execute(Context ctx, StateMachine machine)
        {
            if (ctx.LogicSettings.AmountOfPokemonToDisplayOnStart > 0)
                await DisplayPokemonStatsTask.Execute(ctx, machine);

            return new FarmState();
        }
    }
}