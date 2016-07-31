#region using directives

using System.Threading;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Tasks;

#endregion

namespace PoGo.NecroBot.Logic.Service
{
    public interface IFarm
    {
        void Run(CancellationToken cancellationToken);
    }

    public class Farm : IFarm
    {
        private readonly ISession _session;

        public Farm(ISession session)
        {
            _session = session;
        }

        public void Run(CancellationToken cancellationToken)
        {
            if (_session.LogicSettings.EvolveAllPokemonAboveIv || _session.LogicSettings.EvolveAllPokemonWithEnoughCandy)
            {
                EvolvePokemonTask.Execute(_session, cancellationToken).Wait(cancellationToken);
            }
            if (_session.LogicSettings.AutomaticallyLevelUpPokemon)
            {
                LevelUpPokemonTask.Execute(_session, cancellationToken).Wait(cancellationToken);
            }
            if (_session.LogicSettings.UseLuckyEggConstantly)
            {
                UseLuckyEggConstantlyTask.Execute(_session, cancellationToken).Wait(cancellationToken);
            }
            if (_session.LogicSettings.UseIncenseConstantly)
            {
                UseIncenseConstantlyTask.Execute(_session, cancellationToken).Wait(cancellationToken);
            }
            GetPokeDexCount.Execute(_session, cancellationToken).Wait(cancellationToken);
            if (_session.LogicSettings.TransferDuplicatePokemon)
            {
                TransferDuplicatePokemonTask.Execute(_session, cancellationToken).Wait(cancellationToken);
            }

            if (_session.LogicSettings.RenamePokemon)
            {
                RenamePokemonTask.Execute(_session, cancellationToken).Wait(cancellationToken);
            }

            if (_session.LogicSettings.AutoFavoritePokemon)
            {
                FavoritePokemonTask.Execute(_session, cancellationToken).Wait(cancellationToken);
            }

            RecycleItemsTask.Execute(_session, cancellationToken).Wait(cancellationToken);

            if (_session.LogicSettings.UseEggIncubators)
            {
                UseIncubatorsTask.Execute(_session, cancellationToken).Wait(cancellationToken);
            }

            if (_session.LogicSettings.UseGpxPathing)
            {
                FarmPokestopsGpxTask.Execute(_session, cancellationToken).Wait(cancellationToken);
            }
            else
            {
                FarmPokestopsTask.Execute(_session, cancellationToken).Wait(cancellationToken);
            }
        }
    }
}