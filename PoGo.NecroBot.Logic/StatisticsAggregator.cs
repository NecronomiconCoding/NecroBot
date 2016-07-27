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
            if (evt.Status == CatchPokemonResponse.Types.CatchStatus.CatchSuccess)
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

        public void HandleEvent(UseBerryEvent evt, Context ctx)
        {
        }

        public void HandleEvent(DisplayHighestsPokemonEvent evt, Context ctx)
        {
        }

        public void Listen(IEvent evt, Context ctx)
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