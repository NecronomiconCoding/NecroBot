using System;
using System.Threading;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Event;

namespace PoGo.NecroBot.Logic.State
{
    public delegate void StateMachineEventDeletate(IEvent evt, Context ctx);
    public class StateMachine
    {
        public event StateMachineEventDeletate EventListener;
        private IState _initialState;
        private int _delay;
        private Context _ctx;

        public StateMachine()
        {
        }

        public void SetFailureState(IState state)
        {
            _initialState = state;
        }

        public void RequestDelay(int delay)
        {
            _delay = delay;
        }

        public void Fire(IEvent evt)
        {
            EventListener?.Invoke(evt, _ctx);
        }

        public Task AsyncStart(IState initialState, Context ctx)
        {
            return Task.Run(() => Start(initialState, ctx));
        }

        public void Start(IState initialState, Context ctx)
        {
            _ctx = ctx;
            IState state = initialState;
            do
            {
                try
                {
                    state = state.Execute(ctx, this);
                    Thread.Sleep(_delay);
                    _delay = 0;
                }
                catch(Exception ex)
                {
                    Fire(new ErrorEvent { Message = ex.ToString() });
                    state = _initialState;
                }
            } while (state != null);
        }
    }
}
