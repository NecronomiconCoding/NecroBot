#region using directives

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using PoGo.NecroBot.Logic;
using PoGo.NecroBot.Logic.Common;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Tasks;
using PoGo.NecroBot.Logic.Utils;

#endregion

namespace PoGo.NecroBot.CLI
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var culture = CultureInfo.CreateSpecificCulture("en-US");

            CultureInfo.DefaultThreadCurrentCulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;
            var subPath = "";
            if (args.Length > 0)
                subPath = args[0];

            Logger.SetLogger(new ConsoleLogger(LogLevel.Info), subPath);

            var settings = GlobalSettings.Load(subPath);

            if (settings == null)
            {
                var configPath = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), subPath), "config");

                Logger.Write("This is your first start and the bot has generated the default config!", LogLevel.Warning);
                Logger.Write("check it/provide your specific data", LogLevel.Warning);

                Process.Start(configPath);

                Logger.Write("Press a Key to continue...", LogLevel.Warning);

                Console.ReadKey();

                settings = GlobalSettings.Load(subPath);

                if (settings == null)
                    return;
            }
            var session = new Session(new ClientSettings(settings), new LogicSettings(settings));
            session.Client.ApiFailure = new ApiFailureStrategy(session);


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
            stats.DirtyEvent +=
                () =>
                    Console.Title =
                        stats.GetTemplatedStats(
                            session.Translation.GetTranslation(TranslationString.StatsTemplateString),
                            session.Translation.GetTranslation(TranslationString.StatsXpTemplateString));

            var aggregator = new StatisticsAggregator(stats);
            var listener = new ConsoleEventListener();
            var websocket = new WebSocketInterface(settings.WebSocketPort, session);

            session.EventDispatcher.EventReceived += evt => listener.Listen(evt, session);
            session.EventDispatcher.EventReceived += evt => aggregator.Listen(evt, session);
            session.EventDispatcher.EventReceived += evt => websocket.Listen(evt, session);

            machine.SetFailureState(new LoginState());

            Logger.SetLoggerContext(session);

            session.Navigation.UpdatePositionEvent +=
                (lat, lng) => session.EventDispatcher.Send(new UpdatePositionEvent {Latitude = lat, Longitude = lng});

            machine.AsyncStart(new VersionCheckState(), session);
            if (session.LogicSettings.UseSnipeLocationServer)
                SnipePokemonTask.AsyncStart(session);

            //Non-blocking key reader
            //This will allow to process console key presses in another code parts
            while (true)
            {
                if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Enter)
                {
                    break;
                }
                Thread.Sleep(5);
            }
        }
    }
}