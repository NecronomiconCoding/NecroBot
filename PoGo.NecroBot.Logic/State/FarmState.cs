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
            if (session.LogicSettings.EvolveAllPokemonAboveIv || session.LogicSettings.EvolveAllPokemonWithEnoughCandy)
            {
                await EvolvePokemonTask.Execute(session);
            }

            if (session.LogicSettings.TransferDuplicatePokemon)
            {
                await TransferDuplicatePokemonTask.Execute(session);
            }
            await UseLuckyEgg.Execute(session);
            await UseIncense.Execute(session);
            if (session.LogicSettings.RenameAboveIv)
            {
                await RenamePokemonTask.Execute(session);
            }

            await RecycleItemsTask.Execute(session);

            if (session.LogicSettings.UseEggIncubators)
            {
                await UseIncubatorsTask.Execute(session);
            }

            if (session.LogicSettings.UseGpxPathing)
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