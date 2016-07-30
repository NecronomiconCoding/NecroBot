using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

namespace PoGo.NecroBot.CLI.Forms
{
    public partial class FormNecroMain : Form
    {
        private String[] args;
        private String   subPath;
        private GlobalSettings globalSettings;

        public FormNecroMain(String[] args)
        {
            InitializeComponent();
            this.args = args;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void FormNecroMain_Load(object sender, EventArgs e)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");

            CultureInfo.DefaultThreadCurrentCulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;

            subPath = "";
            if (args != null && args.Length > 0)
                subPath = args[0];

            Logger.SetLogger(new UIConsoleLogger(LogLevel.Info, consoleView), subPath);

            globalSettings = GlobalSettings.Load(subPath);

            if(globalSettings == null)
            {
                Logger.Write("This is your first start and the bot has generated the default config!", LogLevel.Warning);
            }

        }

        private void startMenuItem_Click(object sender, EventArgs e)
        {
            globalSettings = GlobalSettings.Load(subPath); // Load settings again in case of changes.

            var session = new Session(new ClientSettings(globalSettings), new LogicSettings(globalSettings));
            session.Client.ApiFailure = new ApiFailureStrategy(session);

            var machine = new StateMachine();
            var stats = new Statistics();
         
            stats.DirtyEvent += () => labelBotInfo.Text = stats.GetTemplatedStats(session.Translation.GetTranslation(Logic.Common.TranslationString.StatsTemplateString),
             session.Translation.GetTranslation(Logic.Common.TranslationString.StatsXpTemplateString));

            var aggregator = new StatisticsAggregator(stats);
            var listener = new ConsoleEventListener();
            var websocket = new WebSocketInterface(globalSettings.WebSocketPort, session);

            session.EventDispatcher.EventReceived += (IEvent evt) => listener.Listen(evt, session);
            session.EventDispatcher.EventReceived += (IEvent evt) => aggregator.Listen(evt, session);
            session.EventDispatcher.EventReceived += (IEvent evt) => websocket.Listen(evt, session);

            machine.SetFailureState(new LoginState());

            Logger.SetLoggerContext(session);

            session.Navigation.UpdatePositionEvent +=
                (lat, lng) => session.EventDispatcher.Send(new UpdatePositionEvent { Latitude = lat, Longitude = lng });

            machine.AsyncStart(new VersionCheckState(), session);
            if (session.LogicSettings.UseSnipeLocationServer)
                SnipePokemonTask.AsyncStart(session);
        }

        private void settingsMenuItem_Click(object sender, EventArgs e)
        {
            FormNecroSettings settings = new FormNecroSettings(subPath);
            settings.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
