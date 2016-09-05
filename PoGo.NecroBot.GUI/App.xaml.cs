#region using directives

using System;
using System.IO;
using System.Reflection;
using System.Windows;
using Awesomium.Core;
using PoGo.NecroBot.GUI.WebUiClient;

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
            var settings = new WebUiClientConfig();
            var settingsPath = Path.Combine(Directory.GetCurrentDirectory(),
                "Config" + Path.DirectorySeparatorChar + "WebUiClient.json");
            settings.Load(settingsPath);
            var webUi = settings.WebUiClients[settings.CurrentWebUiClient];
            if (!webUi.IsUpToDate())
                webUi.DonwloadAndInstall();
            if (!webUi.IsInstalled())
                return;

            // Initialization must be performed here,
            // before creating a WebControl.
            if (!WebCore.IsInitialized)
            {
                var baseUri = new Uri(Assembly.GetEntryAssembly().Location);
                WebCore.Initialize(new WebConfig
                {
                    HomeURL = new Uri(baseUri, webUi.HomeUri),
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