#region using directives

using System.Threading;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Tasks;

#endregion

namespace PoGo.NecroBot.Logic.State
{
    public class FarmState : IState
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Await.Warning", "CS4014:Await.Warning")]

        public async Task<IState> Execute(ISession session, CancellationToken cancellationToken)
        {
            if (session.LogicSettings.UseNearActionRandom)
            {
                await HumanRandomActionTask.Execute(session, cancellationToken);
            }
            else
            {
                if (session.LogicSettings.EvolveAllPokemonAboveIv || session.LogicSettings.EvolveAllPokemonWithEnoughCandy
                   || session.LogicSettings.UseLuckyEggsWhileEvolving || session.LogicSettings.KeepPokemonsThatCanEvolve)
                    await EvolvePokemonTask.Execute(session, cancellationToken);
                if (session.LogicSettings.UseEggIncubators)
                    await UseIncubatorsTask.Execute(session, cancellationToken);
                if (session.LogicSettings.TransferDuplicatePokemon)
                    await TransferDuplicatePokemonTask.Execute(session, cancellationToken);
                if (session.LogicSettings.UseLuckyEggConstantly)
                    await UseLuckyEggConstantlyTask.Execute(session, cancellationToken);
                if (session.LogicSettings.UseIncenseConstantly)
                    await UseIncenseConstantlyTask.Execute(session, cancellationToken);

                await GetPokeDexCount.Execute(session, cancellationToken);

                if (session.LogicSettings.RenamePokemon)
                    await RenamePokemonTask.Execute(session, cancellationToken);

                await RecycleItemsTask.Execute(session, cancellationToken);

                if (session.LogicSettings.AutomaticallyLevelUpPokemon)
                    await LevelUpPokemonTask.Execute(session, cancellationToken);
            }

            if (session.LogicSettings.UseGpxPathing)
                await FarmPokestopsGpxTask.Execute(session, cancellationToken);
            else
                await FarmPokestopsTask.Execute(session, cancellationToken);

            return this;
        }
    }
}
