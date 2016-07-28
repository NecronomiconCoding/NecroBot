using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoGo.NecroBot.Logic.Service
{
    public interface IFarm
    {
        void Run();
    }

    public class Farm : IFarm
    {
        private readonly ISession _session;

        public Farm(ISession session)
        {
            _session = session;
        }

        public void Run()
        {
            if (_session.BotProfile.Settings.Bot.EvolveAllPokemonAboveIv || _session.BotProfile.Settings.Bot.EvolveAllPokemonWithEnoughCandy)
            {
                EvolvePokemonTask.Execute(_session).Wait();
            }

            if (_session.BotProfile.Settings.Bot.TransferDuplicatePokemon)
            {
                TransferDuplicatePokemonTask.Execute(_session).Wait();
            }

            if (_session.BotProfile.Settings.Bot.RenameAboveIv)
            {
                RenamePokemonTask.Execute(_session).Wait();
            }

            RecycleItemsTask.Execute(_session).Wait();

            if (_session.BotProfile.Settings.Bot.UseEggIncubators)
            {
                UseIncubatorsTask.Execute(_session).Wait();
            }

            if (_session.BotProfile.Settings.Bot.UseGpxPathing)
            {
                FarmPokestopsGpxTask.Execute(_session).Wait();
            }
            else
            {
                FarmPokestopsTask.Execute(_session).Wait();
            }
        }
    }
}
