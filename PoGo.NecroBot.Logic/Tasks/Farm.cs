using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Tasks;
using System.Threading;

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
                EvolvePokemonTask.Execute(_session, cancellationToken).Wait();
            }
            if (_session.LogicSettings.LevelUpPokemonFromIV)
            {
                LevelUpPokemon.Execute(_session, cancellationToken).Wait();
            }
            if (_session.LogicSettings.TransferDuplicatePokemon)
            {
                TransferDuplicatePokemonTask.Execute(_session, cancellationToken).Wait();
            }

            if (_session.LogicSettings.RenamePokemon)
            {
                RenamePokemonTask.Execute(_session, cancellationToken).Wait();
            }

            RecycleItemsTask.Execute(_session, cancellationToken).Wait();

            if (_session.LogicSettings.UseEggIncubators)
            {
                UseIncubatorsTask.Execute(_session, cancellationToken).Wait();
            }

            if (_session.LogicSettings.UseGpxPathing)
            {
                FarmPokestopsGpxTask.Execute(_session, cancellationToken).Wait();
            }
            else
            {
                FarmPokestopsTask.Execute(_session, cancellationToken).Wait();
            }
        }
    }
}
