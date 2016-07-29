#region using directives

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using PoGo.NecroBot.Logic;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Utils;
using PoGo.NecroBot.Logic.Localization;
using PoGo.NecroBot.Logic.Service;
using PoGo.NecroBot.Logic.Tasks;
using PoGo.NecroBot.Logic.Common;

#endregion

namespace PoGo.NecroBot.CLI
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");

            CultureInfo.DefaultThreadCurrentCulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;
			
            var subPath = "";
            if (args.Length > 0)
                subPath = args[0];

            Logger.SetLogger(new ConsoleLogger(LogLevel.Info), subPath);

            var settings = GlobalSettings.Load(subPath);
           

            if (settings == null)
            {
                Logger.Write("This is your first start and the bot has generated the default config!", LogLevel.Warning);
                Logger.Write("We will now shutdown to let you configure the bot and then launch it again.", LogLevel.Warning);
                Thread.Sleep(2000);
                Environment.Exit(0);
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
            stats.DirtyEvent += () => Console.Title = stats.GetTemplatedStats(session.Translation.GetTranslation(Logic.Common.TranslationString.StatsTemplateString),
                session.Translation.GetTranslation(Logic.Common.TranslationString.StatsXpTemplateString));

            var aggregator = new StatisticsAggregator(stats);
            var listener = new ConsoleEventListener();
            var websocket = new WebSocketInterface(settings.WebSocketPort, session);

            session.EventDispatcher.EventReceived += (IEvent evt) => listener.Listen(evt, session);
            session.EventDispatcher.EventReceived += (IEvent evt) => aggregator.Listen(evt, session);
            session.EventDispatcher.EventReceived += (IEvent evt) => websocket.Listen(evt, session);

            machine.SetFailureState(new LoginState());

            Logger.SetLoggerContext(session);

            session.Navigation.UpdatePositionEvent +=
                (lat, lng) => session.EventDispatcher.Send(new UpdatePositionEvent {Latitude = lat, Longitude = lng});

            machine.AsyncStart(new VersionCheckState(), session);
            if(session.LogicSettings.UseSnipeLocationServer)
                SnipePokemonTask.AsyncStart(session);

            //Non-blocking key reader
            //This will allow to process console key presses in another code parts
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    var info = Console.ReadKey();
                    if (info.Key == ConsoleKey.Enter)
                        break;
                }
                Thread.Sleep(5);
            }
        }
    }
}