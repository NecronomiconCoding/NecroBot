#region using directives

using System;
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

        public Task AsyncStart(IState initialState, Session session)
        {
            return Task.Run(() => Start(initialState, session));
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
                    state = await state.Execute(session);
                }
                catch(InvalidResponseException)
                {
                    session.EventDispatcher.Send(new ErrorEvent { Message = "The PokemonGo servers are having a bad time, chill." });
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
