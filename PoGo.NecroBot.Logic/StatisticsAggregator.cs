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

        public void HandleEvent(ProfileEvent evt, Session session)
        {
            _stats.SetUsername(evt.Profile);
            _stats.Dirty(session.Inventory);
        }

        public void HandleEvent(ErrorEvent evt, Session session)
        {
        }

        public void HandleEvent(NoticeEvent evt, Session session)
        {
        }

        public void HandleEvent(WarnEvent evt, Session session)
        {
        }

        public void HandleEvent(UseLuckyEggEvent evt, Session session)
        {
        }

        public void HandleEvent(PokemonEvolveEvent evt, Session session)
        {
            _stats.TotalExperience += evt.Exp;
            _stats.Dirty(session.Inventory);
        }

        public void HandleEvent(TransferPokemonEvent evt, Session session)
        {
            _stats.TotalPokemonsTransfered++;
            _stats.Dirty(session.Inventory);
        }

        public void HandleEvent(ItemRecycledEvent evt, Session session)
        {
            _stats.TotalItemsRemoved++;
            _stats.Dirty(session.Inventory);
        }

        public void HandleEvent(FortUsedEvent evt, Session session)
        {
            _stats.TotalExperience += evt.Exp;
            _stats.Dirty(session.Inventory);
        }

        public void HandleEvent(FortTargetEvent evt, Session session)
        {
        }

        public void HandleEvent(PokemonCaptureEvent evt, Session session)
        {
            if (evt.Status == CatchPokemonResponse.Types.CatchStatus.CatchSuccess)
            {
                _stats.TotalExperience += evt.Exp;
                _stats.TotalPokemons++;
                _stats.TotalStardust = evt.Stardust;
                _stats.Dirty(session.Inventory);
            }
        }

        public void HandleEvent(NoPokeballEvent evt, Session session)
        {
        }

        public void HandleEvent(UseBerryEvent evt, Session session)
        {
        }

        public void HandleEvent(DisplayHighestsPokemonEvent evt, Session session)
        {
        }

        public void Listen(IEvent evt, Session session)
        {
            dynamic eve = evt;

            try
            {
                HandleEvent(eve, session);
            }
            catch
            {
                // ignored
            }
        }
    }
}