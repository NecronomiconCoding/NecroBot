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
using PoGo.NecroBot.Logic.Localization;
using PoGo.NecroBot.Logic.Service;
using PoGo.NecroBot.Logic.Tasks;

#endregion

namespace PoGo.NecroBot.CLI
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var subPath = "";
            if (args.Length > 0)
                subPath = args[0];

            Logger.SetLogger(new ConsoleLogger(LogLevel.Info), subPath);

            var settings = GlobalSettings.Load(subPath);
            var session = new Session(new ClientSettings(settings), new LogicSettings(settings));


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

            if (settings == null)
            {
                settings = GlobalSettings.Load(subPath);
                Logger.Write("This is your first start and the bot will use the default config!", LogLevel.Warning);
                Logger.Write("Continue? (y/n)", LogLevel.Warning);

                if (!Console.ReadLine().ToUpper().Equals("Y"))
                    return;
            }


            var machine = new StateMachine();
            var stats = new Statistics();
            stats.DirtyEvent += () => Console.Title = stats.ToString();

            var aggregator = new StatisticsAggregator(stats);
            var listener = new ConsoleEventListener();
            var websocket = new WebSocketInterface(settings.WebSocketPort);

            session.EventDispatcher.EventReceived += (IEvent evt) => listener.Listen(evt, session);
            session.EventDispatcher.EventReceived += (IEvent evt) => aggregator.Listen(evt, session);
            session.EventDispatcher.EventReceived += (IEvent evt) => websocket.Listen(evt, session);

            machine.SetFailureState(new LoginState());

            Logger.SetLoggerContext(session);

            session.Navigation.UpdatePositionEvent +=
                (lat, lng) => session.EventDispatcher.Send(new UpdatePositionEvent {Latitude = lat, Longitude = lng});

            session.Client.Login.GoogleDeviceCodeEvent += (usercode, uri) =>
            {
                try
                {
                    Logger.Write(session.Translations.GetTranslation(Logic.Common.TranslationString.OpeningGoogleDevicePage), LogLevel.Warning);
                    Thread.Sleep(5000);
                    Process.Start(uri);
                    var thread = new Thread(() => Clipboard.SetText(usercode)); //Copy device code
                    thread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
                    thread.Start();
                    thread.Join();
                }
                catch (Exception)
                {
                    Logger.Write(session.Translations.GetTranslation(Logic.Common.TranslationString.CouldntCopyToClipboard), LogLevel.Error);
                    Logger.Write(session.Translations.GetTranslation(Logic.Common.TranslationString.CouldntCopyToClipboard2, uri, usercode), LogLevel.Error);
                }
            };

            machine.AsyncStart(new VersionCheckState(), session);

            Console.ReadLine();
        }
    }
}