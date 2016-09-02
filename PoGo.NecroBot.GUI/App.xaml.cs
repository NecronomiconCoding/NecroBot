using System;
using System.Linq;
using Awesomium.Core;
using System.Windows;
using System.Collections.Generic;

namespace PoGo.NecroBot.GUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup( StartupEventArgs e )
        {
            // Initialization must be performed here,
            // before creating a WebControl.
            if ( !WebCore.IsInitialized )
            {
                Uri baseUri = new Uri(System.Reflection.Assembly.GetEntryAssembly().Location);

                WebCore.Initialize( new WebConfig()
                {
                    HomeURL = new Uri(baseUri, "html/index.html"),
                    LogPath = @".\starter.log",
                    LogLevel = LogLevel.Verbose,
                    RemoteDebuggingPort = 9033
                } );
            }

            base.OnStartup( e );
        }

        protected override void OnExit( ExitEventArgs e )
        {
            // Make sure we shutdown the core last.
            if ( WebCore.IsInitialized )
                WebCore.Shutdown();

            base.OnExit( e );
        }
    }
}
