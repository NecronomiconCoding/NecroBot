#region using directives

using System;
using System.IO;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Common;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.Utils;
using PoGo.NecroBot.Logic.Tasks;

#endregion

namespace PoGo.NecroBot.Logic.State
{
    public class PauseState : IState
    {
        public async Task<IState> Execute(ISession session)
        {

            // Pauses so user has a chance to exit program if location is incorrect
            // this will help if/when the config/auth.json files are not present
            // and are being re-created.

            session.EventDispatcher.Send(new WarnEvent
            {
                Message = "Press any key twice to continue or press 'q' to quit."
            });
            // I honestly don't know why it's twice.  
            // Pressing it once does nothing most of the time.

            PauseTask.Execute(session).Wait();
            
            return new InfoState();
        }
    }
}
