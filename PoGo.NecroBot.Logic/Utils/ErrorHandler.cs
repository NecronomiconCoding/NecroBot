using PoGo.NecroBot.Logic.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoGo.NecroBot.Logic.Utils
{
    class ErrorHandler
    {
        /// <summary>
        /// Alerts that a fatal error has occurred, displaying a message and exiting the application
        /// </summary>
        /// <param name="strMessage">Optional message to display - Leave NULL to exclude message</param>
        /// <param name="timeout">The total seconds the messag will display before shutting down</param>
        public static void ThrowFatalError( string strMessage, int timeout, LogLevel level )
        {
            if( strMessage != null)
                Logger.Write( strMessage, level );

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
