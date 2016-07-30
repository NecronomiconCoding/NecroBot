using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic;
using PokemonGo.RocketAPI;
using PokemonGo.RocketAPI.Enums;
using System.Windows.Forms;

namespace PoGo.NecroBot.CLI.Forms
{
    public partial class FormNecroSettings : Form
    {
        private String _path;
        private GlobalSettings _globalSettings;

        public FormNecroSettings(String path)
        {
            InitializeComponent();
            _path = path;
            initialize();
        }

        private void initialize()
        {
            _globalSettings = GlobalSettings.Load(_path);
        }

        private void loadSettings()
        {
            initialize();

            altitude.Text = _globalSettings.DefaultAltitude.ToString();
            latitude.Text = _globalSettings.DefaultLatitude.ToString();
            longitude.Text = _globalSettings.DefaultLongitude.ToString();
            speed.Text = _globalSettings.WalkingSpeedInKilometerPerHour.ToString();
            traveldistance.Text = _globalSettings.MaxTravelDistanceInMeters.ToString();
            gpxPath.Checked = _globalSettings.UseGpxPathing;
            gpxFilename.Text = _globalSettings.GpxFile;
            maxPokeballs.Text = _globalSettings.TotalAmountOfPokebalsToKeep.ToString();
            maxRevives.Text = _globalSettings.TotalAmountOfRevivesToKeep.ToString();
            maxPotions.Text = _globalSettings.TotalAmountOfPotionsToKeep.ToString();
            delayCatch.Text = _globalSettings.DelayBetweenPokemonCatch.ToString();
            delayActions.Text = _globalSettings.DelayBetweenPlayerActions.ToString();
            evolveaboveiv.Text = _globalSettings.EvolveAboveIvValue.ToString();
            evolvealliv.Checked = _globalSettings.EvolveAllPokemonAboveIv;
            evolvecandy.Checked = _globalSettings.EvolveAllPokemonWithEnoughCandy;
            keepPokemonCanEvolve.Checked = _globalSettings.KeepPokemonsThatCanEvolve;
            luckyeggEvolve.Checked = _globalSettings.UseLuckyEggsWhileEvolving;
            luckyeggPokemon.Text = _globalSettings.UseLuckyEggsMinPokemonAmount.ToString();
            keepMinCP.Text = _globalSettings.KeepMinCp.ToString();
            keepMinDuplicate.Text = _globalSettings.KeepMinDuplicatePokemon.ToString();
            keepMinIVPerc.Text = _globalSettings.KeepMinIvPercentage.ToString();
            transferDuplicate.Checked = _globalSettings.TransferDuplicatePokemon;
            IVPrio.Checked = _globalSettings.PrioritizeIvOverCp;
            renameAboveIV.Checked = _globalSettings.RenameAboveIv;
            renameTemplate.Text = _globalSettings.RenameTemplate;
            notCatchFilter.Checked = _globalSettings.UsePokemonToNotCatchFilter;
            useEggIncubators.Checked = _globalSettings.UseEggIncubators;

            amountStartPokemon.Text = _globalSettings.AmountOfPokemonToDisplayOnStart.ToString();
            transferConfig.Checked = _globalSettings.TransferConfigAndAuthOnUpdate;
            autoUpdate.Checked = _globalSettings.AutoUpdate;
            dumpPokemon.Checked = _globalSettings.DumpPokemonStats;
            startDelay.Checked = _globalSettings.StartupWelcomeDelay;
            language.Text = _globalSettings.TranslationLanguageCode;
            webPort.Text = _globalSettings.WebSocketPort.ToString();


            pokeballstoSnipe.Text = _globalSettings.MinPokeballsToSnipe.ToString();
            snipePokestops.Checked = _globalSettings.SnipeAtPokestops;
            transferIVSnipe.Checked = _globalSettings.UseTransferIVForSnipe;
            ignoreUnknownIV.Checked = _globalSettings.SnipeIgnoreUnknownIV;
            snipeDelay.Text = _globalSettings.MinDelayBetweenSnipes.ToString();
            useSnipeServer.Checked = _globalSettings.UseSnipeLocationServer;
            srvSnipe.Text = _globalSettings.SnipeLocationServer;
            snipeSrvPort.Text = _globalSettings.SnipeLocationServerPort.ToString();

            if (_globalSettings.Auth.AuthType.Equals(AuthType.Ptc))
            {
                radioPTC.Checked = true;
                username.Text = _globalSettings.Auth.PtcUsername;
                password.Text = _globalSettings.Auth.PtcPassword;
            }
            else
            {
                radioGoogle.Checked = true;
                username.Text = _globalSettings.Auth.GoogleUsername;
                password.Text = _globalSettings.Auth.GooglePassword;
            }
        }

        private void FormNecroSettings_Load(object sender, EventArgs e)
        {
            loadSettings();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _globalSettings.DefaultAltitude  = Convert.ToDouble(altitude.Text);
            _globalSettings.DefaultLatitude  = Convert.ToDouble(latitude.Text);
            _globalSettings.DefaultLongitude = Convert.ToDouble(longitude.Text);
            _globalSettings.WalkingSpeedInKilometerPerHour = Convert.ToDouble(speed.Text);
            _globalSettings.MaxTravelDistanceInMeters      = Convert.ToInt32(traveldistance.Text);
            _globalSettings.UseGpxPathing    = gpxPath.Checked;
            _globalSettings.GpxFile          = gpxFilename.Text;

            _globalSettings.TotalAmountOfPokebalsToKeep = Convert.ToInt32(maxPokeballs.Text);
            _globalSettings.TotalAmountOfPotionsToKeep = Convert.ToInt32(maxPotions.Text);
            _globalSettings.TotalAmountOfRevivesToKeep = Convert.ToInt32(maxRevives.Text);


            _globalSettings.DelayBetweenPokemonCatch  = Convert.ToInt32(delayCatch.Text);
            _globalSettings.DelayBetweenPlayerActions = Convert.ToInt32(delayActions.Text);
            _globalSettings.EvolveAboveIvValue = Convert.ToSingle(evolveaboveiv.Text);
            _globalSettings.EvolveAllPokemonAboveIv = evolvealliv.Checked;
            _globalSettings.EvolveAllPokemonWithEnoughCandy = evolvecandy.Checked;
            _globalSettings.KeepPokemonsThatCanEvolve = keepPokemonCanEvolve.Checked;
            _globalSettings.UseLuckyEggsWhileEvolving = luckyeggEvolve.Checked;
            _globalSettings.UseLuckyEggsMinPokemonAmount = Convert.ToInt32(luckyeggPokemon.Text);
            _globalSettings.KeepMinCp = Convert.ToInt32(keepMinCP.Text);
            _globalSettings.KeepMinDuplicatePokemon = Convert.ToInt32(keepMinDuplicate.Text);
            _globalSettings.KeepMinIvPercentage = Convert.ToSingle(keepMinIVPerc.Text);
            _globalSettings.TransferDuplicatePokemon = transferDuplicate.Checked;
            _globalSettings.PrioritizeIvOverCp = IVPrio.Checked;
            _globalSettings.RenameAboveIv = renameAboveIV.Checked;
            _globalSettings.RenameTemplate = renameTemplate.Text;
            _globalSettings.UsePokemonToNotCatchFilter = notCatchFilter.Checked;
            _globalSettings.UseEggIncubators = useEggIncubators.Checked;
            _globalSettings.AmountOfPokemonToDisplayOnStart = Convert.ToInt32(amountStartPokemon.Text);
            _globalSettings.TransferConfigAndAuthOnUpdate = transferConfig.Checked;
            _globalSettings.AutoUpdate = autoUpdate.Checked;
            _globalSettings.DumpPokemonStats = dumpPokemon.Checked;
            _globalSettings.StartupWelcomeDelay = startDelay.Checked;
            _globalSettings.TranslationLanguageCode = language.Text;
            _globalSettings.WebSocketPort = Convert.ToInt32(webPort.Text);


            _globalSettings.MinPokeballsToSnipe = Convert.ToInt32(pokeballstoSnipe.Text);
            _globalSettings.SnipeAtPokestops = snipePokestops.Checked;
            _globalSettings.UseTransferIVForSnipe = transferIVSnipe.Checked;
            _globalSettings.SnipeIgnoreUnknownIV = ignoreUnknownIV.Checked;
            _globalSettings.MinDelayBetweenSnipes = Convert.ToInt32(snipeDelay.Text);
            _globalSettings.UseSnipeLocationServer = useSnipeServer.Checked;
            _globalSettings.SnipeLocationServer = srvSnipe.Text;
            _globalSettings.SnipeLocationServerPort = Convert.ToInt32(snipeSrvPort.Text);


            if (radioPTC.Checked == true)
            {
                _globalSettings.Auth.AuthType = AuthType.Ptc;
                _globalSettings.Auth.PtcUsername = username.Text;
                _globalSettings.Auth.PtcPassword = password.Text;
            }
            else
            {
                _globalSettings.Auth.AuthType = AuthType.Google;
                _globalSettings.Auth.GoogleUsername = username.Text;
                _globalSettings.Auth.GooglePassword = password.Text;
            }

            _globalSettings.Save(_path);
        }

        private void btnReloadSettings_Click(object sender, EventArgs e)
        {
            loadSettings();
        }
    }
}
