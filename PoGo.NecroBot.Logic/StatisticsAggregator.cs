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

        public void HandleEvent(ProfileEvent evt, ISession ISession)
        {
            _stats.SetUsername(evt.Profile);
            _stats.Dirty(ISession.Inventory,ISession);
        }

        public void HandleEvent(ErrorEvent evt, ISession ISession)
        {
        }

        public void HandleEvent(NoticeEvent evt, ISession ISession)
        {
        }

        public void HandleEvent(WarnEvent evt, ISession ISession)
        {
        }

        public void HandleEvent(UseLuckyEggEvent evt, ISession ISession)
        {
        }

        public void HandleEvent(PokemonEvolveEvent evt, ISession ISession)
        {
            _stats.TotalExperience += evt.Exp;
            _stats.Dirty(ISession.Inventory, ISession);
        }

        public void HandleEvent(TransferPokemonEvent evt, ISession ISession)
        {
            _stats.TotalPokemonsTransfered++;
            _stats.Dirty(ISession.Inventory, ISession);
        }

        public void HandleEvent(ItemRecycledEvent evt, ISession ISession)
        {
            _stats.TotalItemsRemoved++;
            _stats.Dirty(ISession.Inventory, ISession);
        }

        public void HandleEvent(FortUsedEvent evt, ISession ISession)
        {
            _stats.TotalExperience += evt.Exp;
            _stats.Dirty(ISession.Inventory, ISession);
        }

        public void HandleEvent(FortTargetEvent evt, ISession ISession)
        {
        }

        public void HandleEvent(PokemonCaptureEvent evt, ISession ISession)
        {
            if (evt.Status == CatchPokemonResponse.Types.CatchStatus.CatchSuccess)
            {
                _stats.TotalExperience += evt.Exp;
                _stats.TotalPokemons++;
                _stats.TotalStardust = evt.Stardust;
                _stats.Dirty(ISession.Inventory, ISession);
            }
        }

        public void HandleEvent(NoPokeballEvent evt, ISession ISession)
        {
        }

        public void HandleEvent(UseBerryEvent evt, ISession ISession)
        {
        }

        public void HandleEvent(DisplayHighestsPokemonEvent evt, ISession ISession)
        {
        }

        public void Listen(IEvent evt, ISession ISession)
        {
            dynamic eve = evt;

            try
            {
                HandleEvent(eve, ISession);
            }
            catch
            {
                // ignored
            }
        }
    }
}