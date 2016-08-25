#region using directives

using System.Threading;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Tasks;
using System;

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
        private Random NearRandom = new Random();

        public Farm(ISession session)
        {
            _session = session;
        }

        public void Run(CancellationToken cancellationToken)
        {
            if (_session.LogicSettings.EvolveAllPokemonAboveIv || _session.LogicSettings.EvolveAllPokemonWithEnoughCandy
                || _session.LogicSettings.UseLuckyEggsWhileEvolving || _session.LogicSettings.KeepPokemonsThatCanEvolve)
            {
                if (_session.LogicSettings.UseNearActionRandom)
                {
                    if (NearRandom.Next(1, 10) > 4)
                        EvolvePokemonTask.Execute(_session, cancellationToken).Wait(cancellationToken);
                }
                else
                    EvolvePokemonTask.Execute(_session, cancellationToken).Wait(cancellationToken);
            }
            if (_session.LogicSettings.AutomaticallyLevelUpPokemon)
            {
                if (_session.LogicSettings.UseNearActionRandom)
                {
                    if (NearRandom.Next(1, 10) > 4)
                        LevelUpPokemonTask.Execute(_session, cancellationToken).Wait(cancellationToken);
                }
                else
                    LevelUpPokemonTask.Execute(_session, cancellationToken).Wait(cancellationToken);
            }
            if (_session.LogicSettings.UseLuckyEggConstantly)
            {
                if (_session.LogicSettings.UseNearActionRandom)
                {
                    if (NearRandom.Next(1, 10) > 4)
                        UseLuckyEggConstantlyTask.Execute(_session, cancellationToken).Wait(cancellationToken);
                }
                else
                    UseLuckyEggConstantlyTask.Execute(_session, cancellationToken).Wait(cancellationToken);
            }
            if (_session.LogicSettings.UseIncenseConstantly)
            {
                if (_session.LogicSettings.UseNearActionRandom)
                {
                    if (NearRandom.Next(1, 10) > 4)
                        UseIncenseConstantlyTask.Execute(_session, cancellationToken).Wait(cancellationToken);
                }
                else
                    UseIncenseConstantlyTask.Execute(_session, cancellationToken).Wait(cancellationToken);
            }

            if (_session.LogicSettings.TransferDuplicatePokemon)
            {
                if (_session.LogicSettings.UseNearActionRandom)
                {
                    if (NearRandom.Next(1, 10) > 4)
                        TransferDuplicatePokemonTask.Execute(_session, cancellationToken).Wait(cancellationToken);
                }
                else
                    TransferDuplicatePokemonTask.Execute(_session, cancellationToken).Wait(cancellationToken);
            }

            if (_session.LogicSettings.TransferWeakPokemon)
            {
                if (_session.LogicSettings.UseNearActionRandom)
                {
                    if (NearRandom.Next(1, 10) > 4)
                        TransferWeakPokemonTask.Execute(_session, cancellationToken).Wait(cancellationToken);
                }
                else
                    TransferWeakPokemonTask.Execute(_session, cancellationToken).Wait(cancellationToken);
            }

            if (_session.LogicSettings.RenamePokemon)
            {
                if (_session.LogicSettings.UseNearActionRandom)
                {
                    if (NearRandom.Next(1, 10) > 4)
                        RenamePokemonTask.Execute(_session, cancellationToken).Wait(cancellationToken);
                }
                else
                    RenamePokemonTask.Execute(_session, cancellationToken).Wait(cancellationToken);
            }

            if (_session.LogicSettings.AutoFavoritePokemon)
            {
                if (_session.LogicSettings.UseNearActionRandom)
                {
                    if (NearRandom.Next(1, 10) > 4)
                        FavoritePokemonTask.Execute(_session, cancellationToken).Wait(cancellationToken);
                }
                else
                    FavoritePokemonTask.Execute(_session, cancellationToken).Wait(cancellationToken);
            }

            if (_session.LogicSettings.UseNearActionRandom)
            {
                if (NearRandom.Next(1, 10) > 4)
                    RecycleItemsTask.Execute(_session, cancellationToken).Wait(cancellationToken);
            }
            else
                RecycleItemsTask.Execute(_session, cancellationToken).Wait(cancellationToken);

            if (_session.LogicSettings.UseEggIncubators)
            {
                if (_session.LogicSettings.UseNearActionRandom)
                {
                    if (NearRandom.Next(1, 10) > 4)
                        UseIncubatorsTask.Execute(_session, cancellationToken).Wait(cancellationToken);
                }
                else
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

            GetPokeDexCount.Execute(_session, cancellationToken).Wait(cancellationToken);
        }
    }
}