#region using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using PoGo.NecroBot.Logic;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Utils;

#endregion

namespace PoGo.NecroBot.CLI
{
    internal class Program
    {
        public static void LoginWithGoogle(string usercode, string uri)
        {
            try
            {
                Logger.Write("Opening Google Device page. Please paste the code using CTRL+V", LogLevel.Warning);
                Thread.Sleep(5000);
                Process.Start(uri);
                var thread = new Thread(() => Clipboard.SetText(usercode)); //Copy device code
                thread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
                thread.Start();
                thread.Join();
            }
            catch (Exception)
            {
                Logger.Write("Couldnt copy to clipboard, do it manually", LogLevel.Error);
                Logger.Write($"Goto: {uri} & enter {usercode}", LogLevel.Error);
            }
        }

        private static void Main(string[] args)
        {
            var subPath = "";
            if (args.Length > 0)
                subPath = Path.DirectorySeparatorChar + args[0];

            Logger.SetLogger(new ConsoleLogger(LogLevel.Info), subPath);

            var settings = GlobalSettings.Load(subPath);

            var machine = new StateMachine();
            var stats = new Statistics();
            stats.DirtyEvent += () => Console.Title = stats.ToString();

            var aggregator = new StatisticsAggregator(stats);
            var listener = new ConsoleEventListener();
            var websocket = new WebSocketInterface(settings.WebSocketPort);

            machine.EventListener += listener.Listen;
            machine.EventListener += aggregator.Listen;
            machine.EventListener += websocket.Listen;

            machine.SetFailureState(new LoginState());

            var context = new Context(new ClientSettings(settings), new LogicSettings(settings));

            context.Navigation.UpdatePositionEvent +=
                (lat, lng) => machine.Fire(new UpdatePositionEvent {Latitude = lat, Longitude = lng});

            context.Client.Login.GoogleDeviceCodeEvent += LoginWithGoogle;

            machine.AsyncStart(new VersionCheckState(), context);

            Console.ReadLine();
        }
    }
}