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

#endregion

namespace PoGo.NecroBot.CLI
{
    internal class Program
    {
        private static readonly ManualResetEvent QuitEvent = new ManualResetEvent(false);
        private static string subPath = "";
        private static void Main(string[] args)
        {
            string strCulture = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionEventHandler;
            Console.Title = $"NecroBot starting [{strCulture.ToUpper()}]";
            Console.CancelKeyPress += (sender, eArgs) =>
            {
                QuitEvent.Set();
                eArgs.Cancel = true;
            };
            if (args.Length > 0)
                subPath = args[0];

            Logger.SetLogger(new ConsoleLogger(LogLevel.New), subPath);

            var profilePath = Path.Combine( Directory.GetCurrentDirectory(), subPath );
            var profileConfigPath = Path.Combine( profilePath, "config" );
            var configFile = Path.Combine( profileConfigPath, "config.json" );

            GlobalSettings settings;
            Boolean boolNeedsSetup = false;
            
            if( File.Exists( configFile ) )
            {
                if( !VersionCheckState.IsLatest() )
                    settings = GlobalSettings.Load( subPath, true );
                else
                    settings = GlobalSettings.Load( subPath );
            }
            else
            {
                settings = new GlobalSettings();
                settings.ProfilePath = profilePath;
                settings.ProfileConfigPath = profileConfigPath;
                settings.GeneralConfigPath = Path.Combine( Directory.GetCurrentDirectory(), "config" );
                settings.TranslationLanguageCode = strCulture;

                boolNeedsSetup = true;
            }

            var session = new Session(new ClientSettings(settings), new LogicSettings(settings));
            
            if( boolNeedsSetup )
            {
                if( GlobalSettings.PromptForSetup( session.Translation ) )
                    session = GlobalSettings.SetupSettings( session, settings, configFile );
                else
                {
                    GlobalSettings.Load( subPath );

                    Logger.Write( "Press a Key to continue...",
                        LogLevel.Warning );
                    Console.ReadKey();
                    return;
                }

            }

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

            string strVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(3);

            stats.DirtyEvent +=
                () =>
                    Console.Title = $"[Necrobot v{strVersion}] " +
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
            session.Navigation.UpdatePositionEvent += Navigation_UpdatePositionEvent;
            machine.AsyncStart(new VersionCheckState(), session);
            if (session.LogicSettings.UseSnipeLocationServer)
                SnipePokemonTask.AsyncStart(session);

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
