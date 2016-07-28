#region using directives

using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Tasks;

#endregion

namespace PoGo.NecroBot.Logic.State
{
    public class FarmState : IState
    {
        public async Task<IState> Execute(ISession session)
        {
            if (session.BotProfile.Settings.Bot.EvolveAllPokemonAboveIv || session.BotProfile.Settings.Bot.EvolveAllPokemonWithEnoughCandy)
            {
                await EvolvePokemonTask.Execute(session);
            }

            if (session.BotProfile.Settings.Bot.TransferDuplicatePokemon)
            {
                await TransferDuplicatePokemonTask.Execute(session);
            }

            if (session.BotProfile.Settings.Bot.RenameAboveIv)
            {
                await RenamePokemonTask.Execute(session);
            }

            await RecycleItemsTask.Execute(session);

            if (session.BotProfile.Settings.Bot.UseEggIncubators)
            {
                await UseIncubatorsTask.Execute(session);
            }

            if (session.BotProfile.Settings.Bot.UseGpxPathing)
            {
                await FarmPokestopsGpxTask.Execute(session);
            }
            else
            {
                await FarmPokestopsTask.Execute(session);
            }

            return this;
        }
    }
}