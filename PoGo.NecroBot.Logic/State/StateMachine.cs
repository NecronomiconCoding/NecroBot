#region using directives

using System;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Event;

#endregion

namespace PoGo.NecroBot.Logic.State
{
    public delegate void StateMachineEventDeletate(IEvent evt, Context ctx);

    public class StateMachine
    {
        private Context _ctx;
        private IState _initialState;

        public Task AsyncStart(IState initialState, Context ctx)
        {
            return Task.Run(() => Start(initialState, ctx));
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

        public async Task Start(IState initialState, Context ctx)
        {
            _ctx = ctx;
            var state = initialState;
            do
            {
                try
                {
                    state = await state.Execute(ctx, this);
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