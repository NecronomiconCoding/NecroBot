using PoGo.NecroBot.CLI;
using PoGo.NecroBot.Logic;
using PoGo.NecroBot.Logic.Common;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Utils;
using System;
using System.Threading;
using System.Windows.Forms;

namespace PoGo.NecroBot.GUI
{
    static class Program
    {
        public static NecroGUI gui;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var subPath = "";
            gui = new NecroGUI();
            if (args.Length > 0)
                subPath = args[0];

            Logger.SetLogger(new FormLogger(LogLevel.Info), subPath);

            var settings = GlobalSettings.Load(subPath);


            if (settings == null)
            {
                Logger.Write("This is your first start and the bot has generated the default config!", LogLevel.Warning);
                Logger.Write("Please close the application and restart once you've set it up.", LogLevel.Warning);
                return;
            }
            var session = new Session(new ClientSettings(settings), new LogicSettings(settings));
            session.Client.ApiFailure = new ApiFailureStrategy(session);

            var machine = new StateMachine();
            var stats = new Statistics();
            stats.DirtyEvent += () => Console.Title = stats.GetTemplatedStats(session.Translation.GetTranslation(Logic.Common.TranslationString.StatsTemplateString),
                session.Translation.GetTranslation(Logic.Common.TranslationString.StatsXpTemplateString));

            var aggregator = new StatisticsAggregator(stats);
            var listener = new FormEventListener();
            var websocket = new WebSocketInterface(settings.WebSocketPort, session.Translation);

            session.EventDispatcher.EventReceived += (IEvent evt) => listener.Listen(evt, session);
            session.EventDispatcher.EventReceived += (IEvent evt) => aggregator.Listen(evt, session);
            session.EventDispatcher.EventReceived += (IEvent evt) => websocket.Listen(evt, session);

            machine.SetFailureState(new LoginState());

            Logger.SetLoggerContext(session);

            session.Navigation.UpdatePositionEvent +=
                (lat, lng) => session.EventDispatcher.Send(new UpdatePositionEvent { Latitude = lat, Longitude = lng });

            machine.AsyncStart(new VersionCheckState(), session);
            Application.Run(gui);
        }
    }
}
