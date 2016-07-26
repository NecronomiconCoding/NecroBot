using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using PoGo.NecroBot.Logic;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Utils;

namespace PoGo.NecroBot.UI
{
    public partial class MainForm : Form
    {
        private void LoginWithGoogle(string usercode, string uri)
        {
            Logger.Write($"Goto: {uri} & enter {usercode}", LogLevel.Error);
            rtfLog_LinkClicked(rtfLog, new LinkClickedEventArgs(uri));
            try
            {
                // Must be STAThread in order to use OLE like Clipboard
                var thread = new Thread(() => Clipboard.SetText(usercode));
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
                thread.Join();
                Logger.Write("The code has been copied to your clipboard.");
            }
            catch
            {
                // ignored
            }
        }

        public MainForm()
        {
            InitializeComponent();
        }

        private delegate void stats_DirtyCallback(Statistics stats);

        private void stats_Dirty(Statistics stats)
        {
            if (InvokeRequired)
            {
                stats_DirtyCallback d = stats_Dirty;
                Invoke(d, stats);
                return;
            }
            lblUser.Text = stats.PlayerName;
            lblRuntime.Text = stats.GetFormattedRuntime();
            lblLevel.Text = $"Level {stats.CurrentLevel:N0}";
            progress.Value = (int)((double)stats.CurrentLevelExperience / stats.NextLevelExperience * 1000); // out of 1000
            lblXp.Text = $"{stats.CurrentLevelExperience:N0}/{stats.NextLevelExperience:N0} XP";
            lblEta.Text = stats.NextLevelEta == TimeSpan.MaxValue ? "ETA" : stats.NextLevelEta.ToString();
            var runtime = stats.GetRuntime();
            lblXph.Text = (stats.TotalExperience / runtime).ToString("N0");
            lblPph.Text = (stats.TotalPokemons / runtime).ToString("N0");
            lblStardust.Text = stats.TotalStardust.ToString("N0");
            lblTransferred.Text = stats.TotalPokemonsTransfered.ToString("N0");
            lblRecycled.Text = stats.TotalItemsRemoved.ToString("N0");
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Logger.SetLogger(new RichTextLogger(rtfLog, LogLevel.Info));

            var machine = new StateMachine();
            var stats = new Statistics();
            // TODO: Only update the UI elements that need it (but need more fine-grained events than just DirtyEvent then)
            stats.DirtyEvent += () => stats_Dirty(stats);

            var aggregator = new StatisticsAggregator(stats);
            var listener = new EventListener();

            machine.EventListener += listener.Listen;
            machine.EventListener += aggregator.Listen;

            machine.SetFailureState(new LoginState());

            SettingsUtil.Load();

            var context = new Context(new ClientSettings(), new LogicSettings());
            context.Client.Login.GoogleDeviceCodeEvent += LoginWithGoogle;

            webBrowser.DocumentText = Properties.Resources.map;

            machine.AsyncStart(new VersionCheckState(), context);
        }

        private void rtfLog_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            Process.Start(e.LinkText);
        }
    }
}
