using PoGo.NecroBot.Logic.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PoGo.NecroBot.Logic.Tasks
{
    public class HumanRandomActionTask
    {
        private static Random ActionRandom = new Random();

        public static async Task Execute(ISession session, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var randomCommand = Enumerable.Range(1, 9).OrderBy(x => ActionRandom.Next()).Take(9).ToList();
            for (int i = 0; i < 9; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                switch (randomCommand[i])
                {
                    case 1:
                        if (session.LogicSettings.EvolveAllPokemonAboveIv || session.LogicSettings.EvolveAllPokemonWithEnoughCandy
                            || session.LogicSettings.UseLuckyEggsWhileEvolving || session.LogicSettings.KeepPokemonsThatCanEvolve)
                            if (ActionRandom.Next(1, 10) > 4)
                                await EvolvePokemonTask.Execute(session, cancellationToken);
                        break;
                    case 2:
                        if (session.LogicSettings.UseEggIncubators)
                            if (ActionRandom.Next(1, 10) > 4)
                                await UseIncubatorsTask.Execute(session, cancellationToken);
                        break;
                    case 3:
                        if (session.LogicSettings.TransferDuplicatePokemon)
                            if (ActionRandom.Next(1, 10) > 4)
                                await TransferDuplicatePokemonTask.Execute(session, cancellationToken);
                        break;
                    case 4:
                        if (session.LogicSettings.UseLuckyEggConstantly)
                            if (ActionRandom.Next(1, 10) > 4)
                                await UseLuckyEggConstantlyTask.Execute(session, cancellationToken);
                        break;
                    case 5:
                        if (session.LogicSettings.UseIncenseConstantly)
                            if (ActionRandom.Next(1, 10) > 4)
                                await UseIncenseConstantlyTask.Execute(session, cancellationToken);
                        break;
                    case 6:
                        if (session.LogicSettings.RenamePokemon)
                            if (ActionRandom.Next(1, 10) > 4)
                                await RenamePokemonTask.Execute(session, cancellationToken);
                        break;
                    case 7:
                        if (session.LogicSettings.AutoFavoritePokemon)
                            if (ActionRandom.Next(1, 10) > 4)
                                await FavoritePokemonTask.Execute(session, cancellationToken);
                        break;
                    case 8:
                        if (ActionRandom.Next(1, 10) > 4)
                            await RecycleItemsTask.Execute(session, cancellationToken);
                        break;
                    case 9:
                        if (session.LogicSettings.AutomaticallyLevelUpPokemon)
                            if (ActionRandom.Next(1, 10) > 4)
                                await LevelUpPokemonTask.Execute(session, cancellationToken);
                        break;
                }
            }

            await GetPokeDexCount.Execute(session, cancellationToken);
        }

        public async Task TransferRandom(ISession session, CancellationToken cancellationToken)
        {
            if (ActionRandom.Next(1, 10) > 4)
                await TransferDuplicatePokemonTask.Execute(session, cancellationToken);
        }
    }
}
