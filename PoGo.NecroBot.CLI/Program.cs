using PoGo.NecroBot.Logic;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PoGo.NecroBot.CLI
{
    public partial class Program : Form
    {
        delegate void SetTextCallback(string message, LogLevel level = LogLevel.Info, ConsoleColor color = ConsoleColor.Black);
        delegate void SetLighTextCallback(string message);
        public static Program window;
        public Program()
        {
            InitializeComponent();
        }

        public static void LoginWithGoogle(string usercode, string uri)
        {
            try
            {
                Logger.Write("Google Device Code copied to clipboard");
                Thread.Sleep(2000);
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

        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            window = new Program();
            Logger.SetLogger(new ConsoleLogger(LogLevel.Info));
            var stats = new Statistics();
            stats.DirtyEvent += () => window.UpdatePlayerDetails(stats.ToString());
            var machine = new StateMachine();
            var aggregator = new StatisticsAggregator(stats);
            var listener = new ConsoleEventListener();

            machine.EventListener += listener.Listen;
            machine.EventListener += aggregator.Listen;

            machine.SetFailureState(new LoginState());

            GlobalSettings settings = GlobalSettings.Load();

            var context = new Context(new ClientSettings(settings), new LogicSettings(settings));
            context.Client.Login.GoogleDeviceCodeEvent += LoginWithGoogle;

            machine.AsyncStart(new VersionCheckState(), context);
            Application.Run(window);
        }
    }
}
