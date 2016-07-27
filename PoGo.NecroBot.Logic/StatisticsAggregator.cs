#region using directives

using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Utils;
using POGOProtos.Networking.Responses;

#endregion

namespace PoGo.NecroBot.Logic
{
    public class StatisticsAggregator
    {
        private readonly Statistics _stats;

        public StatisticsAggregator(Statistics stats)
        {
            _stats = stats;
        }

        public void HandleEvent(ProfileEvent evt, Session ctx)
        {
            _stats.SetUsername(evt.Profile);
            _stats.Dirty(ctx.Inventory);
        }

        public void HandleEvent(ErrorEvent evt, Session ctx)
        {
        }

        public void HandleEvent(NoticeEvent evt, Session ctx)
        {
        }

        public void HandleEvent(WarnEvent evt, Session ctx)
        {
        }

        public void HandleEvent(UseLuckyEggEvent evt, Session ctx)
        {
        }

        public void HandleEvent(PokemonEvolveEvent evt, Session ctx)
        {
            _stats.TotalExperience += evt.Exp;
            _stats.Dirty(ctx.Inventory);
        }

        public void HandleEvent(TransferPokemonEvent evt, Session ctx)
        {
            _stats.TotalPokemonsTransfered++;
            _stats.Dirty(ctx.Inventory);
        }

        public void HandleEvent(ItemRecycledEvent evt, Session ctx)
        {
            _stats.TotalItemsRemoved++;
            _stats.Dirty(ctx.Inventory);
        }

        public void HandleEvent(FortUsedEvent evt, Session ctx)
        {
            _stats.TotalExperience += evt.Exp;
            _stats.Dirty(ctx.Inventory);
        }

        public void HandleEvent(FortTargetEvent evt, Session ctx)
        {
        }

        public void HandleEvent(PokemonCaptureEvent evt, Session ctx)
        {
            if (evt.Status == CatchPokemonResponse.Types.CatchStatus.CatchSuccess)
            {
                _stats.TotalExperience += evt.Exp;
                _stats.TotalPokemons++;
                _stats.TotalStardust = evt.Stardust;
                _stats.Dirty(ctx.Inventory);
            }
        }

        public void HandleEvent(NoPokeballEvent evt, Session ctx)
        {
        }

        public void HandleEvent(UseBerryEvent evt, Session ctx)
        {
        }

        public void HandleEvent(DisplayHighestsPokemonEvent evt, Session ctx)
        {
        }

        public void Listen(IEvent evt, Session ctx)
        {
            dynamic eve = evt;

            try
            {
                HandleEvent(eve, ctx);
            }
            catch
            {
                // ignored
            }
        }
    }
}