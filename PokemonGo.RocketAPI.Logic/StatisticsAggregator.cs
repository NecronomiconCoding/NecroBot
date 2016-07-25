using PokemonGo.RocketAPI.Logic.Event;
using PokemonGo.RocketAPI.Logic.State;
using PokemonGo.RocketAPI.Logic.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGo.RocketAPI.Logic
{
    public class StatisticsAggregator
    {
        private readonly Statistics _stats;

        public StatisticsAggregator(Statistics stats)
        {
            _stats = stats;
        }

        public void Listen(IEvent evt, Context ctx)
        {
            dynamic eve = evt;

            HandleEvent(eve, ctx);
        }

        public void HandleEvent(ProfileEvent evt, Context ctx)
        {
            _stats.SetUsername(evt.Profile);
            _stats.Dirty(ctx.Inventory);
        }

        public void HandleEvent(ErrorEvent evt, Context ctx)
        {
        }

        public void HandleEvent(NoticeEvent evt, Context ctx)
        {
        }

        public void HandleEvent(WarnEvent evt, Context ctx)
        {
        }

        public void HandleEvent(UseLuckyEggEvent evt, Context ctx)
        {
        }

        public void HandleEvent(PokemonEvolveEvent evt, Context ctx)
        {
            _stats.TotalExperience += evt.Exp;
            _stats.Dirty(ctx.Inventory);
        }

        public void HandleEvent(TransferPokemonEvent evt, Context ctx)
        {
            _stats.TotalPokemonsTransfered++;
            _stats.Dirty(ctx.Inventory);
        }

        public void HandleEvent(ItemRecycledEvent evt, Context ctx)
        {
            _stats.TotalItemsRemoved++;
            _stats.Dirty(ctx.Inventory);
        }

        public void HandleEvent(FortUsedEvent evt, Context ctx)
        {
            _stats.TotalExperience += evt.Exp;
            _stats.Dirty(ctx.Inventory);
        }

        public void HandleEvent(FortTargetEvent evt, Context ctx)
        {
        }

        public void HandleEvent(PokemonCaptureEvent evt, Context ctx)
        {
            if (evt.Status == GeneratedCode.CatchPokemonResponse.Types.CatchStatus.CatchSuccess)
            {
                _stats.TotalExperience += evt.Exp;
                _stats.TotalPokemons++;
                _stats.TotalStardust = evt.Stardust;
                _stats.Dirty(ctx.Inventory);
            }
        }

        public void HandleEvent(NoPokeballEvent evt, Context ctx)
        {
        }
    }
}
