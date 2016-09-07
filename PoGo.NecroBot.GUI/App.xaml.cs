#region using directives

using System;
using System.Reflection;
using System.Windows;
using Awesomium.Core;

#endregion

namespace PoGo.NecroBot.GUI
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // Initialization must be performed here,
            // before creating a WebControl.
            if (!WebCore.IsInitialized)
            {
                var baseUri = new Uri(Assembly.GetEntryAssembly().Location);
                WebCore.Initialize(new WebConfig
                {
                    HomeURL = new Uri(baseUri, "WebUi/"),
                    LogPath = @".\starter.log",
                    LogLevel = LogLevel.Verbose,
                    RemoteDebuggingPort = 9033
                });
            }

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // Make sure we shutdown the core last.
            if (WebCore.IsInitialized)
                WebCore.Shutdown();

            base.OnExit(e);
        }
    }
}