using PokemonGo.RocketAPI.Callbacks;
using PokemonGo.RocketAPI.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using PokemonGo.RocketAPI.GeneratedCode;
using PokemonGo.RocketAPI.Console.Server;
using PokemonGo.RocketAPI.Console.Server.Models;
using PokemonGo.RocketAPI.Console.Server.Models.Constants;

namespace PokemonGo.RocketAPI.Console.Forms
{
    public partial class MainForm : Form, IPokeCallBack
    {
        private Settings settings = null;
        private IPokeCallBack callback;
        private PokeServer pokeServer = new PokeServer(4711);
        public MainForm()
        {
            InitializeComponent();
            Logger.SetLogger(new ConsoleLogger(LogLevel.Info));
            settings = new Settings();
            AuthUserNameTxtBox.Text = settings.PtcUsername;
            AuthPasswordTxtBox.Text = settings.PtcPassword;
            switch (settings.AuthType)
            {
                case Enums.AuthType.Google:
                    AuthTypeComboBox.SelectedIndex = 0;
                    break;
                case Enums.AuthType.Ptc:
                    AuthTypeComboBox.SelectedIndex = 1;
                    break;
            }
            this.callback = this;
        }

        public void OnStart()
        {
            Task.Run(() =>
            {
                try
                {
                    new Logic.Logic(settings, callback).Execute().Wait();
                }
                catch (PtcOfflineException ptc)
                {
                    Logger.Write("PTC Servers are probably down OR your credentials are wrong. Try google",
                        LogLevel.Error);
                    Logger.Write("Trying again in 20 seconds...");
                    Thread.Sleep(20000);
                    new Logic.Logic(settings,callback).Execute().Wait();
                }
                catch (AccountNotVerifiedException anv)
                {
                    Logger.Write("Account not verified. - Exiting");
                    Environment.Exit(0);
                }
                catch (Exception ex)
                {
                    Logger.Write($"Unhandled exception: {ex}", LogLevel.Error);
                    new Logic.Logic(settings, callback).Execute().Wait();
                }
            });

        }



        public void OnWrite(string date, string message, LogLevel level)
        {
            switch (level)
            {

                case LogLevel.Error:
                    break;
                case LogLevel.Warning:
                    break;
                case LogLevel.Info:
                    break;
                case LogLevel.Pokestop:
                    break;
                case LogLevel.Farming:
                    break;
                case LogLevel.Recycling:
                    break;
                case LogLevel.Caught:
                    break;
                case LogLevel.CaughtGreat:
                    break;
                case LogLevel.CaughtUltra:
                    break;
                case LogLevel.CaughtMaster:
                    break;
                case LogLevel.CatchFail:
                    break;
                case LogLevel.Transfer:
                    break;
                case LogLevel.Evolve:
                    break;
                case LogLevel.Debug:
                    break;
                default:

                    break;
            }
        }

        private void AuthLoginBtn_Click(object sender, EventArgs e)
        {
            UserSettings.Default.PtcUsername = AuthUserNameTxtBox.Text;
            if (AuthTypeComboBox.SelectedIndex == 0)
            {
                UserSettings.Default.AuthType = "Google";
            }
            else if (AuthTypeComboBox.SelectedIndex == 1)
            {
                UserSettings.Default.AuthType = "Ptc";
                UserSettings.Default.PtcPassword = AuthPasswordTxtBox.Text;
            }
            OnStart();
        }

        void IPokeCallBack.OnBerryUsed(Item item)
        {
            ResponseModel<Item> model = ResponseModel<Item>.Factory(item);
            model.message = "Berry Used";
            model.status = Constants.BERRY_USED;
            pokeServer.sendData(model);
        }

        void IPokeCallBack.OnCatchFailed(int counter, MapPokemon pkmnId, PokemonData data, string attempts)
        {

        }

        void IPokeCallBack.OnCaught(int counter, MapPokemon pkmnId, PokemonData data, string attempts)
        {

        }

        void IPokeCallBack.OnEggFound()
        {

        }

        void IPokeCallBack.OnEvolved(PokemonData data, EvolvePokemonOut evolvedPokemon)
        {

        }

        void IPokeCallBack.OnEvolvedFailed(PokemonData data, EvolvePokemonOut evolvedPokemon)
        {

        }

        void IPokeCallBack.OnNoPokeBalls(MapPokemon pkmnId, PokemonData data)
        {

        }

        void IPokeCallBack.OnRecycled(Item item)
        {
            
        }

        void IPokeCallBack.OnTransfer(PokemonData pokemonData)
        {

        }

        private void AuthTypeComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            switch (AuthTypeComboBox.SelectedIndex)
            {
                case 0:
                    AuthPasswordTxtBox.Enabled = false;
                    AuthUserNameTxtBox.Enabled = false;
                    break;
                case 1:
                    AuthPasswordTxtBox.Enabled = true;
                    AuthUserNameTxtBox.Enabled = true;
                    break;
            }
            
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            pokeServer.endServer();
        }
    }
}