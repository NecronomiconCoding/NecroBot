#region using directives

using System;
using System.Threading;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Event;

#endregion

namespace PoGo.NecroBot.Logic.State
{
    public class StateMachine
    {
        private ISession _ctx;
        private IState _initialState;
        private CancellationTokenSource _tokenSource;
        

        public Task AsyncStart(IState initialState, Session session)
        {
            return Task.Run(() => Start(initialState, session));
        }

        public void SetFailureState(IState state)
        {
            _initialState = state;
        }

        public void Stop() {
            if (_tokenSource != null) {
                _tokenSource.Cancel();
            }

        }

        public async Task Start(IState initialState, Session session)
        {
            _ctx = session;
            var state = initialState;
            do
            {
                try
                {
                    _tokenSource = new CancellationTokenSource();
                    state = await state.Execute(session);
                }
                catch (OperationCanceledException) {
                    state = null;
                    break;
                }
                catch (Exception ex)
                {
                    session.EventDispatcher.Send(new ErrorEvent {Message = ex.ToString()});
                    state = _initialState;
                }
            } while (state != null);
        }
    }
}