#region using directives

using System;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Event;

#endregion

namespace PoGo.NecroBot.Logic.State
{
    public delegate void StateMachineEventDeletate(IEvent evt, Session session);

    public class StateMachine
    {
        private Session _ctx;
        private IState _initialState;

        public Task AsyncStart(IState initialState, Session session)
        {
            return Task.Run(() => Start(initialState, session));
        }

        public event StateMachineEventDeletate EventListener;

        public void Fire(IEvent evt)
        {
            EventListener?.Invoke(evt, _ctx);
        }

        public void SetFailureState(IState state)
        {
            _initialState = state;
        }

        public async Task Start(IState initialState, Session session)
        {
            _ctx = session;
            var state = initialState;
            do
            {
                try
                {
                    state = await state.Execute(session, this);
                }
                catch (Exception ex)
                {
                    Fire(new ErrorEvent {Message = ex.ToString()});
                    state = _initialState;
                }
            } while (state != null);
        }
    }
}