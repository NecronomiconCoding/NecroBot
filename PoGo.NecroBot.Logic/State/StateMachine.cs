#region using directives

using System;
using System.Threading;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Event;
using PokemonGo.RocketAPI.Exceptions;

#endregion

namespace PoGo.NecroBot.Logic.State
{
    public class StateMachine
    {
        private IState _initialState;
        private int _crashCount;
        private DateTime _crashTime;

        public Task AsyncStart(IState initialState, Session session,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Run(() => Start(initialState, session, cancellationToken), cancellationToken);
        }

        public void SetFailureState(IState state)
        {
            _initialState = state;
            _crashCount = 0;
            _crashTime = DateTime.Now;
        }

        public async Task Start(IState initialState, Session session,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var state = initialState;
            do
            {
                try
                {
                    state = await state.Execute(session, cancellationToken);
                }
                catch (InvalidResponseException)
                {
                    session.EventDispatcher.Send(new ErrorEvent { Message = "Niantic Servers unstable, throttling API Calls." });

                    if(_crashTime.AddSeconds(3).Ticks > DateTime.Now.Ticks)     //if throttling API calls twice in 3s, crash count will increase.
                    {
                        _crashCount++;
                    }

                    if (_crashCount > 20)       //if server throttling API calls too frequently, let bot relogin.
                    {
                        _crashCount = 0;
                        state = _initialState;
                    }

                    _crashTime = DateTime.Now;
                }
                catch (OperationCanceledException)
                {
                    session.EventDispatcher.Send(new ErrorEvent {Message = "Current Operation was canceled."});
                    state = _initialState;
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