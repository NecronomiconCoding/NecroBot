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
        public async Task Start(IState rootState)
        {
            var currentState = rootState;
            var previousState = currentState;
            do
            {
                if(previousState != currentState)
                {
                    await previousState.OnExit();

                    previousState = currentState;
                    currentState = await currentState.OnEnter();

                    if (previousState != currentState)
                    {
                        continue;
                    }
                }

                try
                {
                    currentState = await currentState.Run();
                }
                catch (Exception ex)
                {
                    currentState = await currentState.OnError(ex);
                }
            } while (currentState != null);
        }
    }
}