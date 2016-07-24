using PokemonGo.RocketAPI.Logic.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PokemonGo.RocketAPI.Logic.State
{
    public delegate void StateMachineEventDeletate(IEvent evt);
    public class StateMachine
    {
        public event StateMachineEventDeletate EventListener;
        private IState _initialState;
        private int _delay;
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
            EventListener?.Invoke(evt);
        }

        public Task AsyncStart(IState initialState, Context ctx)
        {
            return Task.Run(() => Start(initialState, ctx));
        }

        public void Start(IState initialState, Context ctx)
        {
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
