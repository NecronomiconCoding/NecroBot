#region using directives

using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Tasks;

#endregion

namespace PoGo.NecroBot.Logic.State
{
    public class FarmState : IState
    {
        public async Task<IState> Execute(Session session, StateMachine machine)
        {
            if (session.LogicSettings.EvolveAllPokemonAboveIv || session.LogicSettings.EvolveAllPokemonWithEnoughCandy)
            {
                await EvolvePokemonTask.Execute(session, machine);
            }

            if (session.LogicSettings.TransferDuplicatePokemon)
            {
                await TransferDuplicatePokemonTask.Execute(session, machine);
            }

            if (session.LogicSettings.RenameAboveIv)
            {
                await RenamePokemonTask.Execute(session, machine);
            }

            await RecycleItemsTask.Execute(session, machine);

            if (session.LogicSettings.UseEggIncubators)
            {
                await UseIncubatorsTask.Execute(session, machine);
            }

            if (session.LogicSettings.UseGpxPathing)
            {
                await FarmPokestopsGpxTask.Execute(session, machine);
            }
            else
            {
                await FarmPokestopsTask.Execute(session, machine);
            }

            return this;
        }
    }
}