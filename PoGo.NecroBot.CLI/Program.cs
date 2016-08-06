#region using directives

using System;
using System.Globalization;
using System.Threading;
using PoGo.NecroBot.Logic;
using PoGo.NecroBot.Logic.Common;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Tasks;
using PoGo.NecroBot.Logic.Utils;
using System.IO;
using PoGo.NecroBot.CLI.Resources;

#endregion

namespace PoGo.NecroBot.CLI
{
    internal class Program
    {
        private static readonly ManualResetEvent QuitEvent = new ManualResetEvent(false);
        private static string subPath = "";
        private static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionEventHandler;
            Console.Title = "NecroBot";
            Console.CancelKeyPress += (sender, eArgs) =>
            {
                QuitEvent.Set();
                eArgs.Cancel = true;
            };
            var culture = CultureInfo.CreateSpecificCulture("en-US");

            CultureInfo.DefaultThreadCurrentCulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;
            if (args.Length > 0)
                subPath = args[0];

            Logger.SetLogger(new ConsoleLogger(LogLevel.Info), subPath);
            
            var settings = GlobalSettings.Load(subPath);

            if (settings == null)
            {
                Logger.Write("Press a Key to continue...",
                    LogLevel.Warning);
                Console.ReadKey();
                return;
            }
            ProgressBar.start("NecroBot is starting up", 10);

            var session = new Session(new ClientSettings(settings), new LogicSettings(settings));
            session.Client.ApiFailure = new ApiFailureStrategy(session);
            ProgressBar.fill(20);

            /*SimpleSession session = new SimpleSession
            {
                _client = new PokemonGo.RocketAPI.Client(new ClientSettings(settings)),
                _dispatcher = new EventDispatcher(),
                _localizer = new Localizer()
            };

            BotService service = new BotService
            {
                _session = session,
                _loginTask = new Login(session)
            };

            service.Run();
            */

            var machine = new StateMachine();
            var stats = new Statistics();
            ProgressBar.fill(30);
            stats.DirtyEvent +=
                () =>
                    Console.Title =
                        stats.GetTemplatedStats(
                            session.Translation.GetTranslation(TranslationString.StatsTemplateString),
                            session.Translation.GetTranslation(TranslationString.StatsXpTemplateString));
            ProgressBar.fill(40);

            var aggregator = new StatisticsAggregator(stats);
            ProgressBar.fill(50);
            var listener = new ConsoleEventListener();

            session.EventDispatcher.EventReceived += evt => listener.Listen(evt, session);
            session.EventDispatcher.EventReceived += evt => aggregator.Listen(evt, session);
            if(settings.UseWebsocket)
            session.EventDispatcher.EventReceived += evt => new WebSocketInterface(settings.WebSocketPort, session).Listen(evt, session);

            machine.SetFailureState(new LoginState());
            ProgressBar.fill(90);

            Logger.SetLoggerContext(session);

            session.Navigation.UpdatePositionEvent +=
                (lat, lng) => session.EventDispatcher.Send(new UpdatePositionEvent {Latitude = lat, Longitude = lng});
            session.Navigation.UpdatePositionEvent += Navigation_UpdatePositionEvent;
            ProgressBar.fill(100);

            machine.AsyncStart(new VersionCheckState(), session);
            if (session.LogicSettings.UseSnipeLocationServer)
                SnipePokemonTask.AsyncStart(session);
            Console.Clear();

            QuitEvent.WaitOne();
        }

        private static void Navigation_UpdatePositionEvent(double lat, double lng)
        {
            SaveLocationToDisk(lat, lng);
        }

        private static void SaveLocationToDisk(double lat, double lng)
        {
            var coordsPath = Path.Combine(Directory.GetCurrentDirectory(), subPath, "Config", "LastPos.ini");

            File.WriteAllText(coordsPath, $"{lat}:{lng}");
        }

        private static void UnhandledExceptionEventHandler(object obj, UnhandledExceptionEventArgs args)
        {
            Logger.Write("Exceptiion caught, writing LogBuffer.", force: true);
            throw new Exception();
        }
    }
}
