using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic;
using System.Windows;
using PokemonGo.RocketAPI.Enums;

namespace PoGo.NecroBot.ConfigUI.Models
{

    public class ObservableSettings : DependencyObject
    {

        #region BASIC Properties

        public string TranslationLanguageCode
        {
            get { return (string)GetValue(TranslationLanguageCodeProperty); }
            set { SetValue(TranslationLanguageCodeProperty, value); }
        }
        public static readonly DependencyProperty TranslationLanguageCodeProperty =
            DependencyProperty.Register("TranslationLanguageCode", typeof(string), typeof(ObservableSettings), new PropertyMetadata(string.Empty));

        public bool AutoUpdate
        {
            get { return (bool)GetValue(AutoUpdateProperty); }
            set { SetValue(AutoUpdateProperty, value); }
        }
        public static readonly DependencyProperty AutoUpdateProperty =
            DependencyProperty.Register("AutoUpdate", typeof(bool), typeof(ObservableSettings), new PropertyMetadata(false));

        public bool TransferConfigAndAuthOnUpdate
        {
            get { return (bool)GetValue(TransferConfigAndAuthOnUpdateProperty); }
            set { SetValue(TransferConfigAndAuthOnUpdateProperty, value); }
        }
        public static readonly DependencyProperty TransferConfigAndAuthOnUpdateProperty =
            DependencyProperty.Register("TransferConfigAndAuthOnUpdate", typeof(bool), typeof(ObservableSettings), new PropertyMetadata(false));

        public bool DisableHumanWalking
        {
            get { return (bool)GetValue(DisableHumanWalkingProperty); }
            set { SetValue(DisableHumanWalkingProperty, value); }
        }
        public static readonly DependencyProperty DisableHumanWalkingProperty =
            DependencyProperty.Register("DisableHumanWalking", typeof(bool), typeof(ObservableSettings), new PropertyMetadata(false));

        public double DefaultLatitude
        {
            get { return (double)GetValue(DefaultLatitudeProperty); }
            set { SetValue(DefaultLatitudeProperty, value); }
        }
        public static readonly DependencyProperty DefaultLatitudeProperty =
            DependencyProperty.Register("DefaultLatitude", typeof(double), typeof(ObservableSettings), new PropertyMetadata(0.0));

        public double DefaultLongitude
        {
            get { return (double)GetValue(DefaultLongitudeProperty); }
            set { SetValue(DefaultLongitudeProperty, value); }
        }
        public static readonly DependencyProperty DefaultLongitudeProperty =
            DependencyProperty.Register("DefaultLongitude", typeof(double), typeof(ObservableSettings), new PropertyMetadata(0.0));

        public double WalkingSpeedInKilometerPerHour
        {
            get { return (double)GetValue(WalkingSpeedInKilometerPerHourProperty); }
            set { SetValue(WalkingSpeedInKilometerPerHourProperty, value); }
        }
        public static readonly DependencyProperty WalkingSpeedInKilometerPerHourProperty =
            DependencyProperty.Register("WalkingSpeedInKilometerPerHour", typeof(double), typeof(ObservableSettings), new PropertyMetadata(0.0));

        public int MaxSpawnLocationOffset
        {
            get { return (int)GetValue(MaxSpawnLocationOffsetProperty); }
            set { SetValue(MaxSpawnLocationOffsetProperty, value); }
        }
        public static readonly DependencyProperty MaxSpawnLocationOffsetProperty =
            DependencyProperty.Register("MaxSpawnLocationOffset", typeof(int), typeof(ObservableSettings), new PropertyMetadata(0));

        public int DelayBetweenPlayerActions
        {
            get { return (int)GetValue(DelayBetweenPlayerActionsProperty); }
            set { SetValue(DelayBetweenPlayerActionsProperty, value); }
        }
        public static readonly DependencyProperty DelayBetweenPlayerActionsProperty =
            DependencyProperty.Register("DelayBetweenPlayerActions", typeof(int), typeof(ObservableSettings), new PropertyMetadata(0));

        public int DelayBetweenPokemonCatch
        {
            get { return (int)GetValue(DelayBetweenPokemonCatchProperty); }
            set { SetValue(DelayBetweenPokemonCatchProperty, value); }
        }
        public static readonly DependencyProperty DelayBetweenPokemonCatchProperty =
            DependencyProperty.Register("DelayBetweenPokemonCatch", typeof(int), typeof(ObservableSettings), new PropertyMetadata(0));

        public double Latitude
        {
            get { return (double)GetValue(LatitudeProperty); }
            set { SetValue(LatitudeProperty, value); }
        }
        public static readonly DependencyProperty LatitudeProperty =
            DependencyProperty.Register("Latitude", typeof(double), typeof(ObservableSettings), new PropertyMetadata(0.0));

        public double Longitude
        {
            get { return (double)GetValue(LongitudeProperty); }
            set { SetValue(LongitudeProperty, value); }
        }
        public static readonly DependencyProperty LongitudeProperty =
            DependencyProperty.Register("Longitude", typeof(double), typeof(ObservableSettings), new PropertyMetadata(0.0));

        #endregion BASIC Properties


        #region AUTH Properties

        public AuthType AuthType
        {
            get { return (AuthType)GetValue(AuthTypeProperty); }
            set { SetValue(AuthTypeProperty, value); }
        }
        public static readonly DependencyProperty AuthTypeProperty =
            DependencyProperty.Register("AuthType", typeof(AuthType), typeof(ObservableSettings), new PropertyMetadata(AuthType.Google));

        public string GoogleUsername
        {
            get { return (string)GetValue(GoogleUsernameProperty); }
            set { SetValue(GoogleUsernameProperty, value); }
        }
        public static readonly DependencyProperty GoogleUsernameProperty =
            DependencyProperty.Register("GoogleUsername", typeof(string), typeof(ObservableSettings), new PropertyMetadata(string.Empty));

        public string GooglePassword
        {
            get { return (string)GetValue(GooglePasswordProperty); }
            set { SetValue(GooglePasswordProperty, value); }
        }
        public static readonly DependencyProperty GooglePasswordProperty =
            DependencyProperty.Register("GooglePassword", typeof(string), typeof(ObservableSettings), new PropertyMetadata(string.Empty));

        public string PtcUsername
        {
            get { return (string)GetValue(PtcUsernameProperty); }
            set { SetValue(PtcUsernameProperty, value); }
        }
        public static readonly DependencyProperty PtcUsernameProperty =
            DependencyProperty.Register("PtcUsername", typeof(string), typeof(ObservableSettings), new PropertyMetadata(string.Empty));

        public string PtcPassword
        {
            get { return (string)GetValue(PtcPasswordProperty); }
            set { SetValue(PtcPasswordProperty, value); }
        }
        public static readonly DependencyProperty PtcPasswordProperty =
            DependencyProperty.Register("PtcPassword", typeof(string), typeof(ObservableSettings), new PropertyMetadata(string.Empty));

        public bool UseProxy
        {
            get { return (bool)GetValue(UseProxyProperty); }
            set { SetValue(UseProxyProperty, value); }
        }
        public static readonly DependencyProperty UseProxyProperty =
            DependencyProperty.Register("UseProxy", typeof(bool), typeof(ObservableSettings), new PropertyMetadata(false));

        public string UseProxyHost
        {
            get { return (string)GetValue(UseProxyHostProperty); }
            set { SetValue(UseProxyHostProperty, value); }
        }
        public static readonly DependencyProperty UseProxyHostProperty =
            DependencyProperty.Register("UseProxyHost", typeof(string), typeof(ObservableSettings), new PropertyMetadata(string.Empty));

        public string UseProxyPort
        {
            get { return (string)GetValue(UseProxyPortProperty); }
            set { SetValue(UseProxyPortProperty, value); }
        }
        public static readonly DependencyProperty UseProxyPortProperty =
            DependencyProperty.Register("UseProxyPort", typeof(string), typeof(ObservableSettings), new PropertyMetadata(string.Empty));

        public bool UseProxyAuthentication
        {
            get { return (bool)GetValue(UseProxyAuthenticationProperty); }
            set { SetValue(UseProxyAuthenticationProperty, value); }
        }
        public static readonly DependencyProperty UseProxyAuthenticationProperty =
            DependencyProperty.Register("UseProxyAuthentication", typeof(bool), typeof(ObservableSettings), new PropertyMetadata(false));

        public string UseProxyUsername
        {
            get { return (string)GetValue(UseProxyUsernameProperty); }
            set { SetValue(UseProxyUsernameProperty, value); }
        }
        public static readonly DependencyProperty UseProxyUsernameProperty =
            DependencyProperty.Register("UseProxyUsername", typeof(string), typeof(ObservableSettings), new PropertyMetadata(string.Empty));

        public string UseProxyPassword
        {
            get { return (string)GetValue(UseProxyPasswordProperty); }
            set { SetValue(UseProxyPasswordProperty, value); }
        }
        public static readonly DependencyProperty UseProxyPasswordProperty =
            DependencyProperty.Register("UseProxyPassword", typeof(string), typeof(ObservableSettings), new PropertyMetadata(string.Empty));

        #endregion AUTH Properties





        public string DevicePackageName
        {
            get { return (string)GetValue(DevicePackageNameProperty); }
            set { SetValue(DevicePackageNameProperty, value); }
        }
        public static readonly DependencyProperty DevicePackageNameProperty =
            DependencyProperty.Register("DevicePackageName", typeof(string), typeof(ObservableSettings), new PropertyMetadata("random"));

        public string DeviceId
        {
            get { return (string)GetValue(DeviceIdProperty); }
            set { SetValue(DeviceIdProperty, value); }
        }
        public static readonly DependencyProperty DeviceIdProperty =
            DependencyProperty.Register("DeviceId", typeof(string), typeof(ObservableSettings), new PropertyMetadata(string.Empty));

        public string AndroidBoardName
        {
            get { return (string)GetValue(AndroidBoardNameProperty); }
            set { SetValue(AndroidBoardNameProperty, value); }
        }
        public static readonly DependencyProperty AndroidBoardNameProperty =
            DependencyProperty.Register("AndroidBoardName", typeof(string), typeof(ObservableSettings), new PropertyMetadata(string.Empty));

        public string AndroidBootloader
        {
            get { return (string)GetValue(AndroidBootloaderProperty); }
            set { SetValue(AndroidBootloaderProperty, value); }
        }
        public static readonly DependencyProperty AndroidBootloaderProperty =
            DependencyProperty.Register("AndroidBootloader", typeof(string), typeof(ObservableSettings), new PropertyMetadata(string.Empty));

        public string DeviceBrand
        {
            get { return (string)GetValue(DeviceBrandProperty); }
            set { SetValue(DeviceBrandProperty, value); }
        }
        public static readonly DependencyProperty DeviceBrandProperty =
            DependencyProperty.Register("DeviceBrand", typeof(string), typeof(ObservableSettings), new PropertyMetadata(string.Empty));

        public string DeviceModel
        {
            get { return (string)GetValue(DeviceModelProperty); }
            set { SetValue(DeviceModelProperty, value); }
        }
        public static readonly DependencyProperty DeviceModelProperty =
            DependencyProperty.Register("DeviceModel", typeof(string), typeof(ObservableSettings), new PropertyMetadata(string.Empty));

        public string DeviceModelIdentifier
        {
            get { return (string)GetValue(DeviceModelIdentifierProperty); }
            set { SetValue(DeviceModelIdentifierProperty, value); }
        }
        public static readonly DependencyProperty DeviceModelIdentifierProperty =
            DependencyProperty.Register("DeviceModelIdentifier", typeof(string), typeof(ObservableSettings), new PropertyMetadata(string.Empty));

        public string DeviceModelBoot
        {
            get { return (string)GetValue(DeviceModelBootProperty); }
            set { SetValue(DeviceModelBootProperty, value); }
        }
        public static readonly DependencyProperty DeviceModelBootProperty =
            DependencyProperty.Register("DeviceModelBoot", typeof(string), typeof(ObservableSettings), new PropertyMetadata(string.Empty));

        public string HardwareManufacturer
        {
            get { return (string)GetValue(HardwareManufacturerProperty); }
            set { SetValue(HardwareManufacturerProperty, value); }
        }
        public static readonly DependencyProperty HardwareManufacturerProperty =
            DependencyProperty.Register("HardwareManufacturer", typeof(string), typeof(ObservableSettings), new PropertyMetadata(string.Empty));

        public string HardwareModel
        {
            get { return (string)GetValue(HardwareModelProperty); }
            set { SetValue(HardwareModelProperty, value); }
        }
        public static readonly DependencyProperty HardwareModelProperty =
            DependencyProperty.Register("HardwareModel", typeof(string), typeof(ObservableSettings), new PropertyMetadata(string.Empty));

        public string FirmwareBrand
        {
            get { return (string)GetValue(FirmwareBrandProperty); }
            set { SetValue(FirmwareBrandProperty, value); }
        }
        public static readonly DependencyProperty FirmwareBrandProperty =
            DependencyProperty.Register("FirmwareBrand", typeof(string), typeof(ObservableSettings), new PropertyMetadata(string.Empty));

        public string FirmwareTags
        {
            get { return (string)GetValue(FirmwareTagsProperty); }
            set { SetValue(FirmwareTagsProperty, value); }
        }
        public static readonly DependencyProperty FirmwareTagsProperty =
            DependencyProperty.Register("FirmwareTags", typeof(string), typeof(ObservableSettings), new PropertyMetadata(string.Empty));

        public string FirmwareType
        {
            get { return (string)GetValue(FirmwareTypeProperty); }
            set { SetValue(FirmwareTypeProperty, value); }
        }
        public static readonly DependencyProperty FirmwareTypeProperty =
            DependencyProperty.Register("FirmwareType", typeof(string), typeof(ObservableSettings), new PropertyMetadata(string.Empty));

        public string FirmwareFingerprint
        {
            get { return (string)GetValue(FirmwareFingerprintProperty); }
            set { SetValue(FirmwareFingerprintProperty, value); }
        }
        public static readonly DependencyProperty FirmwareFingerprintProperty =
            DependencyProperty.Register("FirmwareFingerprint", typeof(string), typeof(ObservableSettings), new PropertyMetadata(string.Empty));




        public ObservableSettings() { }


        public static ObservableSettings CreateFromGlobalSettings(GlobalSettings set)
        {
            ObservableSettings res = new ObservableSettings();
            // BASIC
            res.TranslationLanguageCode = set.TranslationLanguageCode;
            res.AutoUpdate = set.AutoUpdate;
            res.TransferConfigAndAuthOnUpdate = set.TransferConfigAndAuthOnUpdate;
            res.DisableHumanWalking = set.DisableHumanWalking;
            res.Latitude = set.DefaultLatitude;
            res.Longitude = set.DefaultLongitude;
            res.WalkingSpeedInKilometerPerHour = set.WalkingSpeedInKilometerPerHour;
            res.MaxSpawnLocationOffset = set.MaxSpawnLocationOffset;
            res.DelayBetweenPlayerActions = set.DelayBetweenPlayerActions;
            res.DelayBetweenPokemonCatch = set.DelayBetweenPokemonCatch;
            // AUTH
            res.AuthType = set.Auth.AuthType;
            res.GoogleUsername = set.Auth.GoogleUsername;
            res.GooglePassword = set.Auth.GooglePassword;
            res.PtcUsername = set.Auth.PtcUsername;
            res.PtcPassword = set.Auth.PtcPassword;
            res.UseProxy = set.Auth.UseProxy;
            res.UseProxyHost = set.Auth.UseProxyHost;
            res.UseProxyPort = set.Auth.UseProxyPort;
            res.UseProxyAuthentication = set.Auth.UseProxyAuthentication;
            res.UseProxyUsername = set.Auth.UseProxyUsername;
            res.UseProxyPassword = set.Auth.UseProxyPassword;
            // DEVICE
            res.DevicePackageName = set.Auth.DevicePackageName;
            res.DeviceId = set.Auth.DeviceId;
            res.AndroidBoardName = set.Auth.AndroidBoardName;
            res.AndroidBootloader = set.Auth.AndroidBootloader;
            res.DeviceBrand = set.Auth.DeviceBrand;
            res.DeviceModel = set.Auth.DeviceModel;
            res.DeviceModelIdentifier = set.Auth.DeviceModelIdentifier;
            res.DeviceModelBoot = set.Auth.DeviceModelBoot;
            res.HardwareManufacturer = set.Auth.HardwareManufacturer;
            res.HardwareModel = set.Auth.HardwareModel;
            res.FirmwareBrand = set.Auth.FirmwareBrand;
            res.FirmwareTags = set.Auth.FirmwareTags;
            res.FirmwareType = set.Auth.FirmwareType;
            res.FirmwareFingerprint = set.Auth.FirmwareFingerprint;
            // INVENTORY
            // POKEMON
            // CAPTURE
            // TRANSFER
            // SNIPING
            // MISC
            return res;
        }

        /*
                 AUTH

            // auth data
                public AuthType AuthType;
                public string GoogleUsername;
                public string GooglePassword;
                public string PtcUsername;
                public string PtcPassword;
                public bool UseProxy;
                public string UseProxyHost;
                public string UseProxyPort;
                public bool UseProxyAuthentication;
                public string UseProxyUsername;
                public string UseProxyPassword;

                DEVICE

            // device data
                public string DevicePackageName;
                public string DeviceId;
                public string AndroidBoardName;
                public string AndroidBootloader;
                public string DeviceBrand;
                public string DeviceModel;
                public string DeviceModelIdentifier;
                public string DeviceModelBoot;
                public string HardwareManufacturer;
                public string HardwareModel;
                public string FirmwareBrand;
                public string FirmwareTags;
                public string FirmwareType;
                public string FirmwareFingerprint;

                BASIC

            // general / basic data

            public string TranslationLanguageCode;
        public bool AutoUpdate;
        public bool TransferConfigAndAuthOnUpdate;
        public bool DisableHumanWalking;
        public double DefaultLatitude;
        public double DefaultLongitude;
        public double WalkingSpeedInKilometerPerHour;
        public int MaxSpawnLocationOffset;

            public int DelayBetweenPlayerActions;
        public int DelayBetweenPokemonCatch;


            POKEMONS

            //pokemon
        public bool RenamePokemon;
        public bool RenameOnlyAboveIv;
        public string RenameTemplate;

            //powerup
        public bool AutomaticallyLevelUpPokemon;
        public int AmountOfTimesToUpgradeLoop;
        public int GetMinStarDustForLevelUp;
        public string LevelUpByCPorIv;
        public float UpgradePokemonCpMinimum;
        public float UpgradePokemonIvMinimum;
        public string UpgradePokemonMinimumStatsOperator;

            //evolve
        public float EvolveAboveIvValue;
        public bool EvolveAllPokemonAboveIv;
        public bool EvolveAllPokemonWithEnoughCandy;
        public double EvolveKeptPokemonsAtStorageUsagePercentage;
        public bool KeepPokemonsThatCanEvolve;

            //favorite
        public float FavoriteMinIvPercentage;
        public bool AutoFavoritePokemon;

            MISC

            //gpx
        public bool UseGpxPathing;
        public string GpxFile;

            //websockets
        public bool UseWebsocket;
        public int WebSocketPort;

            //Telegram
        public bool UseTelegramAPI;
        public string TelegramAPIKey;

        //console options
        public bool StartupWelcomeDelay;
        public int AmountOfPokemonToDisplayOnStart;
        public bool DetailedCountsBeforeRecycling;
        public bool DumpPokemonStats;



            SNIPING

            //snipe
        public bool UseSnipeLocationServer;
        public string SnipeLocationServer;
        public int SnipeLocationServerPort;
        public bool GetSniperInfoFromPokezz;
        public bool GetOnlyVerifiedSniperInfoFromPokezz;
        public int MinPokeballsToSnipe;
        public int MinPokeballsWhileSnipe;
        public int MinDelayBetweenSnipes;
        public double SnipingScanOffset;
        public bool SnipeAtPokestops;
        public bool SnipeIgnoreUnknownIv;
        public bool UseTransferIvForSnipe;
        public bool SnipePokemonNotInPokedex;

        public bool UsePokemonSniperFilterOnly;

            INVENTORY

            //recycle
        public bool VerboseRecycling;
        public double RecycleInventoryAtUsagePercentage;

            //amounts
        public int MaxPokeballsPerPokemon;
        public int MaxTravelDistanceInMeters;
        public int TotalAmountOfPokeballsToKeep;
        public int TotalAmountOfPotionsToKeep;
        public int TotalAmountOfRevivesToKeep;
        public int TotalAmountOfBerriesToKeep;

            //lucky, incense and berries
        public bool UseEggIncubators;
        public bool UseLuckyEggConstantly;
        public int UseLuckyEggsMinPokemonAmount;
        public bool UseLuckyEggsWhileEvolving;
        public bool UseIncenseConstantly;

            CAPTURE

        public bool CatchPokemon;
        public int MaxBerriesToUsePerPokemon;

            //balls
        public int UseGreatBallAboveCp;
        public int UseUltraBallAboveCp;
        public int UseMasterBallAboveCp;
        public double UseGreatBallAboveIv;
        public double UseUltraBallAboveIv;
        public double UseGreatBallBelowCatchProbability;
        public double UseUltraBallBelowCatchProbability;
        public double UseMasterBallBelowCatchProbability;
        public bool EnableHumanizedThrows;
        public int NiceThrowChance;
        public int GreatThrowChance;
        public int ExcellentThrowChance;
        public int CurveThrowChance;
        public double ForceGreatThrowOverIv;
        public double ForceExcellentThrowOverIv;
        public int ForceGreatThrowOverCp;
        public int ForceExcellentThrowOverCp;
        public bool UsePokemonToNotCatchFilter;

        public int UseBerriesMinCp;
        public float UseBerriesMinIv;
        public double UseBerriesBelowCatchProbability;
        public string UseBerriesOperator;

            TRANSFER

            //transfer
        public bool TransferWeakPokemon;
        public bool TransferDuplicatePokemon;
        public bool TransferDuplicatePokemonOnCapture;

            //keeping
        public int KeepMinCp;
        public float KeepMinIvPercentage;
        public int KeepMinLvl;
        public string KeepMinOperator;
        public bool UseKeepMinLvl;
        public bool PrioritizeIvOverCp;
        public int KeepMinDuplicatePokemon;

           */

        internal GlobalSettings GetGlobalSettingsObject()
        {
            GlobalSettings gs = new GlobalSettings();
            // BASIC
            gs.TranslationLanguageCode = TranslationLanguageCode;
            gs.AutoUpdate = AutoUpdate;
            gs.TransferConfigAndAuthOnUpdate = TransferConfigAndAuthOnUpdate;
            gs.DisableHumanWalking = DisableHumanWalking;
            gs.DefaultLatitude = DefaultLatitude;
            gs.DefaultLongitude = DefaultLongitude;
            gs.WalkingSpeedInKilometerPerHour = WalkingSpeedInKilometerPerHour;
            gs.MaxSpawnLocationOffset = MaxSpawnLocationOffset;
            gs.DelayBetweenPlayerActions = DelayBetweenPlayerActions;
            gs.DelayBetweenPokemonCatch = DelayBetweenPokemonCatch;
            // AUTH
            gs.Auth.AuthType = AuthType;
            gs.Auth.GoogleUsername = GoogleUsername;
            gs.Auth.GooglePassword = GooglePassword;
            gs.Auth.PtcUsername = PtcUsername;
            gs.Auth.PtcPassword = PtcPassword;
            gs.Auth.UseProxy = UseProxy;
            gs.Auth.UseProxyHost = UseProxyHost;
            gs.Auth.UseProxyPort = UseProxyPort;
            gs.Auth.UseProxyAuthentication = UseProxyAuthentication;
            gs.Auth.UseProxyUsername = UseProxyUsername;
            gs.Auth.UseProxyPassword = UseProxyPassword;
            // DEVICE
            gs.Auth.DevicePackageName = DevicePackageName;
            gs.Auth.DeviceId = DeviceId;
            gs.Auth.AndroidBoardName = AndroidBoardName;
            gs.Auth.AndroidBootloader = AndroidBootloader;
            gs.Auth.DeviceBrand = DeviceBrand;
            gs.Auth.DeviceModel = DeviceModel;
            gs.Auth.DeviceModelIdentifier = DeviceModelIdentifier;
            gs.Auth.DeviceModelBoot = DeviceModelBoot;
            gs.Auth.HardwareManufacturer = HardwareManufacturer;
            gs.Auth.HardwareModel = HardwareModel;
            gs.Auth.FirmwareBrand = FirmwareBrand;
            gs.Auth.FirmwareTags = FirmwareTags;
            gs.Auth.FirmwareType = FirmwareType;
            gs.Auth.FirmwareFingerprint = FirmwareFingerprint;
            // INVENTORY
            // POKEMON
            // CAPTURE
            // TRANSFER
            // SNIPING
            // MISC
            return gs;
        }

    }

}
