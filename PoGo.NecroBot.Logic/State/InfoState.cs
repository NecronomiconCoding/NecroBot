#region using directives

using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Tasks;

#endregion

namespace PoGo.NecroBot.Logic.State
{
    public class InfoState : IState
    {
        public async Task<IState> Execute(Session session, StateMachine machine)
        {
            if (session.LogicSettings.AmountOfPokemonToDisplayOnStart > 0)
                await DisplayPokemonStatsTask.Execute(session, machine);

            return new FarmState();
        }
    }
}