using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.State;
using System;

namespace PoGo.NecroBot.Logic.Utils
{
    class ErrorHandler
    {
        /// <summary>
        /// Alerts that a fatal error has occurred, displaying a message and exiting the application
        /// </summary>
        /// <param name="strMessage">Optional message to display - Leave NULL to exclude message</param>
        /// <param name="timeout">The total seconds the messag will display before shutting down</param>
        public static void ThrowFatalError(ISession session, string strMessage, int timeout )
        {
            session.EventDispatcher.Send(new ErrorEvent()
            {
                Message = "Fatal Error"
            });

            if( strMessage != null )
                session.EventDispatcher.Send(new ErrorEvent()
                {
                    Message = strMessage
                });


            Console.Write( "Ending Application... " );

            for( int i = timeout; i > 0; i-- )
            {
                Console.Write( "\b" + i );
                System.Threading.Thread.Sleep( 1000 );
            }

            Environment.Exit( -1 );
        }
    }
}
