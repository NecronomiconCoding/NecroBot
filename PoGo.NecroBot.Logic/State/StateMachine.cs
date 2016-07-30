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
        private ISession _ctx;
        private IState _initialState;

        public Task AsyncStart(IState initialState, Session session, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Run(() => Start(initialState, session, cancellationToken), cancellationToken);
        }

        public void SetFailureState(IState state)
        {
            _initialState = state;
        }

        public async Task Start(IState initialState, Session session, CancellationToken cancellationToken = default(CancellationToken))
        {
            _ctx = session;
            var state = initialState;
            do
            {
                try
                {
                    state = await state.Execute(session, cancellationToken);
                }
                catch (InvalidResponseException)
                {

                    session.EventDispatcher.Send(new ErrorEvent { Message = "The PokemonGo servers are having a bad time, chill." });
                }
                catch (OperationCanceledException)
                {
                    session.EventDispatcher.Send(new ErrorEvent { Message = "The bot was stopped." });
                    return;
                }
                catch (Exception ex)
                {
                    session.EventDispatcher.Send(new ErrorEvent { Message = ex.ToString() });
                    state = _initialState;
                }
            } while (state != null);
        }
    }
}