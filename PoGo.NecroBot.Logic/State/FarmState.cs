#region using directives

using System.Threading;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Tasks;
using System;

#endregion

namespace PoGo.NecroBot.Logic.State
{
    public class FarmState : IState
    {
        private Random NearRandom = new Random();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Await.Warning", "CS4014:Await.Warning")]

        public async Task<IState> Execute(ISession session, CancellationToken cancellationToken)
        {
            if (session.LogicSettings.EvolveAllPokemonAboveIv || session.LogicSettings.EvolveAllPokemonWithEnoughCandy 
               || session.LogicSettings.UseLuckyEggsWhileEvolving || session.LogicSettings.KeepPokemonsThatCanEvolve)
            {
                if (session.LogicSettings.UseNearActionRandom)
                {
                    if (NearRandom.Next(1, 10) > 4)
                        await EvolvePokemonTask.Execute(session, cancellationToken);
                }
                else
                    await EvolvePokemonTask.Execute(session, cancellationToken);
            }

            if (session.LogicSettings.UseEggIncubators)
            {
                if (session.LogicSettings.UseNearActionRandom)
                {
                    if (NearRandom.Next(1, 10) > 4)
                        await UseIncubatorsTask.Execute(session, cancellationToken);
                }
                else
                    await UseIncubatorsTask.Execute(session, cancellationToken);
            }

            if (session.LogicSettings.TransferDuplicatePokemon)
            {
                if (session.LogicSettings.UseNearActionRandom)
                {
                    if (NearRandom.Next(1, 10) > 4)
                        await TransferDuplicatePokemonTask.Execute(session, cancellationToken);
                }
                else
                    await TransferDuplicatePokemonTask.Execute(session, cancellationToken);
            }

            if (session.LogicSettings.UseLuckyEggConstantly)
            {
                if (session.LogicSettings.UseNearActionRandom)
                {
                    if (NearRandom.Next(1, 10) > 4)
                        await UseLuckyEggConstantlyTask.Execute(session, cancellationToken);
                }
                else
                    await UseLuckyEggConstantlyTask.Execute(session, cancellationToken);
            }

            if (session.LogicSettings.UseIncenseConstantly)
            {
                if (session.LogicSettings.UseNearActionRandom)
                {
                    if (NearRandom.Next(1, 10) > 4)
                        await UseIncenseConstantlyTask.Execute(session, cancellationToken);
                }
                else
                    await UseIncenseConstantlyTask.Execute(session, cancellationToken);
            }

            await GetPokeDexCount.Execute(session, cancellationToken);

            if (session.LogicSettings.RenamePokemon)
            {
                if (session.LogicSettings.UseNearActionRandom)
                {
                    if (NearRandom.Next(1, 10) > 4)
                        await RenamePokemonTask.Execute(session, cancellationToken);
                }
                else
                    await RenamePokemonTask.Execute(session, cancellationToken);
            }

            if (session.LogicSettings.AutoFavoritePokemon)
            {
                if (session.LogicSettings.UseNearActionRandom)
                {
                    if (NearRandom.Next(1, 10) > 4)
                        await FavoritePokemonTask.Execute(session, cancellationToken);
                }
                else
                    await FavoritePokemonTask.Execute(session, cancellationToken);
            }

            if (session.LogicSettings.UseNearActionRandom)
            {
                if (NearRandom.Next(1, 10) > 4)
                    await RecycleItemsTask.Execute(session, cancellationToken);
            }
            else
                await RecycleItemsTask.Execute(session, cancellationToken);

            if (session.LogicSettings.AutomaticallyLevelUpPokemon)
            {
                if (session.LogicSettings.UseNearActionRandom)
                {
                    if (NearRandom.Next(1, 10) > 4)
                        await LevelUpPokemonTask.Execute(session, cancellationToken);
                }
                else
                    await LevelUpPokemonTask.Execute(session, cancellationToken);
            }

            if (session.LogicSettings.UseGpxPathing)
            {
                await FarmPokestopsGpxTask.Execute(session, cancellationToken);
            }
            else
            {
                await FarmPokestopsTask.Execute(session, cancellationToken);
            }

            return this;
        }
    }
}
