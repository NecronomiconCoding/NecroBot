#region using directives

using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Tasks;

#endregion

namespace PoGo.NecroBot.Logic.State
{
    public class InfoState : IState
    {
        public async Task<IState> Execute(ISession session)
        {
            if (session.BotProfile.Settings.Bot.AmountOfPokemonToDisplayOnStart > 0)
                await DisplayPokemonStatsTask.Execute(session);

            return new FarmState();
        }
    }
}