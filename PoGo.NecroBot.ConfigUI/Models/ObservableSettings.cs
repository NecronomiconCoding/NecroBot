using System.Windows;
using PoGo.NecroBot.Logic;
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

        public int MaxTravelDistanceInMeters
        {
            get { return (int)GetValue(MaxTravelDistanceInMetersProperty); }
            set { SetValue(MaxTravelDistanceInMetersProperty, value); }
        }
        public static readonly DependencyProperty MaxTravelDistanceInMetersProperty =
            DependencyProperty.Register("MaxTravelDistanceInMeters", typeof(int), typeof(ObservableSettings), new PropertyMetadata(0));

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


        #region DEVICE Properties

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

        #endregion DEVICE Properties


        #region INVENTORY Properties

        public bool VerboseRecycling
        {
            get { return (bool)GetValue(VerboseRecyclingProperty); }
            set { SetValue(VerboseRecyclingProperty, value); }
        }
        public static readonly DependencyProperty VerboseRecyclingProperty =
            DependencyProperty.Register("VerboseRecycling", typeof(bool), typeof(ObservableSettings), new PropertyMetadata(false));

        public double RecycleInventoryAtUsagePercentage
        {
            get { return (double)GetValue(RecycleInventoryAtUsagePercentageProperty); }
            set { SetValue(RecycleInventoryAtUsagePercentageProperty, value); }
        }
        public static readonly DependencyProperty RecycleInventoryAtUsagePercentageProperty =
            DependencyProperty.Register("RecycleInventoryAtUsagePercentage", typeof(double), typeof(ObservableSettings), new PropertyMetadata(0.0));

        public int TotalAmountOfPokeballsToKeep
        {
            get { return (int)GetValue(TotalAmountOfPokeballsToKeepProperty); }
            set { SetValue(TotalAmountOfPokeballsToKeepProperty, value); }
        }
        public static readonly DependencyProperty TotalAmountOfPokeballsToKeepProperty =
            DependencyProperty.Register("TotalAmountOfPokeballsToKeep", typeof(int), typeof(ObservableSettings), new PropertyMetadata(0));

        public int TotalAmountOfPotionsToKeep
        {
            get { return (int)GetValue(TotalAmountOfPotionsToKeepProperty); }
            set { SetValue(TotalAmountOfPotionsToKeepProperty, value); }
        }
        public static readonly DependencyProperty TotalAmountOfPotionsToKeepProperty =
            DependencyProperty.Register("TotalAmountOfPotionsToKeep", typeof(int), typeof(ObservableSettings), new PropertyMetadata(0));

        public int TotalAmountOfRevivesToKeep
        {
            get { return (int)GetValue(TotalAmountOfRevivesToKeepProperty); }
            set { SetValue(TotalAmountOfRevivesToKeepProperty, value); }
        }
        public static readonly DependencyProperty TotalAmountOfRevivesToKeepProperty =
            DependencyProperty.Register("TotalAmountOfRevivesToKeep", typeof(int), typeof(ObservableSettings), new PropertyMetadata(0));

        public int TotalAmountOfBerriesToKeep
        {
            get { return (int)GetValue(TotalAmountOfBerriesToKeepProperty); }
            set { SetValue(TotalAmountOfBerriesToKeepProperty, value); }
        }
        public static readonly DependencyProperty TotalAmountOfBerriesToKeepProperty =
            DependencyProperty.Register("TotalAmountOfBerriesToKeep", typeof(int), typeof(ObservableSettings), new FrameworkPropertyMetadata(0, new PropertyChangedCallback(ItemsKept_PropertyChanged)));
        
        static void ItemsKept_PropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            ObservableSettings os = obj as ObservableSettings;
            if (null != os) os.TotalItemsBeingKept = 1;
        }

        public int TotalItemsBeingKept
        {
            get { return (int)GetValue(TotalAmountOfPokeballsToKeepProperty) + (int)GetValue(TotalAmountOfPotionsToKeepProperty) + (int)GetValue(TotalAmountOfRevivesToKeepProperty) + (int)GetValue(TotalAmountOfBerriesToKeepProperty); }
            set { SetValue(TotalItemsBeingKeptProperty, (int)GetValue(TotalAmountOfPokeballsToKeepProperty) + (int)GetValue(TotalAmountOfPotionsToKeepProperty) + (int)GetValue(TotalAmountOfRevivesToKeepProperty) + (int)GetValue(TotalAmountOfBerriesToKeepProperty)); }
        }
        public static readonly DependencyProperty TotalItemsBeingKeptProperty =
            DependencyProperty.Register("TotalItemsBeingKept", typeof(int), typeof(ObservableSettings), new PropertyMetadata(0));

        public bool UseEggIncubators
        {
            get { return (bool)GetValue(UseEggIncubatorsProperty); }
            set { SetValue(UseEggIncubatorsProperty, value); }
        }
        public static readonly DependencyProperty UseEggIncubatorsProperty =
            DependencyProperty.Register("UseEggIncubators", typeof(bool), typeof(ObservableSettings), new PropertyMetadata(false));

        public bool UseLuckyEggConstantly
        {
            get { return (bool)GetValue(UseLuckyEggConstantlyProperty); }
            set { SetValue(UseLuckyEggConstantlyProperty, value); }
        }
        public static readonly DependencyProperty UseLuckyEggConstantlyProperty =
            DependencyProperty.Register("UseLuckyEggConstantly", typeof(bool), typeof(ObservableSettings), new PropertyMetadata(false));

        public int UseLuckyEggsMinPokemonAmount
        {
            get { return (int)GetValue(UseLuckyEggsMinPokemonAmountProperty); }
            set { SetValue(UseLuckyEggsMinPokemonAmountProperty, value); }
        }
        public static readonly DependencyProperty UseLuckyEggsMinPokemonAmountProperty =
            DependencyProperty.Register("UseLuckyEggsMinPokemonAmount", typeof(int), typeof(ObservableSettings), new PropertyMetadata(0));

        public bool UseLuckyEggsWhileEvolving
        {
            get { return (bool)GetValue(UseLuckyEggsWhileEvolvingProperty); }
            set { SetValue(UseLuckyEggsWhileEvolvingProperty, value); }
        }
        public static readonly DependencyProperty UseLuckyEggsWhileEvolvingProperty =
            DependencyProperty.Register("UseLuckyEggsWhileEvolving", typeof(bool), typeof(ObservableSettings), new PropertyMetadata(false));

        public bool UseIncenseConstantly
        {
            get { return (bool)GetValue(UseIncenseConstantlyProperty); }
            set { SetValue(UseIncenseConstantlyProperty, value); }
        }
        public static readonly DependencyProperty UseIncenseConstantlyProperty =
            DependencyProperty.Register("UseIncenseConstantly", typeof(bool), typeof(ObservableSettings), new PropertyMetadata(false));

        #endregion INVENTORY Properties


        #region POKEMON Properties

        public bool RenamePokemon
        {
            get { return (bool)GetValue(RenamePokemonProperty); }
            set { SetValue(RenamePokemonProperty, value); }
        }
        public static readonly DependencyProperty RenamePokemonProperty =
            DependencyProperty.Register("RenamePokemon", typeof(bool), typeof(ObservableSettings), new PropertyMetadata(false));

        public bool RenameOnlyAboveIv
        {
            get { return (bool)GetValue(RenameOnlyAboveIvProperty); }
            set { SetValue(RenameOnlyAboveIvProperty, value); }
        }
        public static readonly DependencyProperty RenameOnlyAboveIvProperty =
            DependencyProperty.Register("RenameOnlyAboveIv", typeof(bool), typeof(ObservableSettings), new PropertyMetadata(false));

        public string RenameTemplate
        {
            get { return (string)GetValue(RenameTemplateProperty); }
            set { SetValue(RenameTemplateProperty, value); }
        }
        public static readonly DependencyProperty RenameTemplateProperty =
            DependencyProperty.Register("RenameTemplate", typeof(string), typeof(ObservableSettings), new PropertyMetadata(string.Empty));

        public bool AutomaticallyLevelUpPokemon
        {
            get { return (bool)GetValue(AutomaticallyLevelUpPokemonProperty); }
            set { SetValue(AutomaticallyLevelUpPokemonProperty, value); }
        }
        public static readonly DependencyProperty AutomaticallyLevelUpPokemonProperty =
            DependencyProperty.Register("AutomaticallyLevelUpPokemon", typeof(bool), typeof(ObservableSettings), new PropertyMetadata(false));

        public int AmountOfTimesToUpgradeLoop
        {
            get { return (int)GetValue(AmountOfTimesToUpgradeLoopProperty); }
            set { SetValue(AmountOfTimesToUpgradeLoopProperty, value); }
        }
        public static readonly DependencyProperty AmountOfTimesToUpgradeLoopProperty =
            DependencyProperty.Register("AmountOfTimesToUpgradeLoop", typeof(int), typeof(ObservableSettings), new PropertyMetadata(0));

        public int GetMinStarDustForLevelUp
        {
            get { return (int)GetValue(GetMinStarDustForLevelUpProperty); }
            set { SetValue(GetMinStarDustForLevelUpProperty, value); }
        }
        public static readonly DependencyProperty GetMinStarDustForLevelUpProperty =
            DependencyProperty.Register("GetMinStarDustForLevelUp", typeof(int), typeof(ObservableSettings), new PropertyMetadata(0));

        public string LevelUpByCPorIv
        {
            get { return (string)GetValue(LevelUpByCPorIvProperty); }
            set { SetValue(LevelUpByCPorIvProperty, value); }
        }
        public static readonly DependencyProperty LevelUpByCPorIvProperty =
            DependencyProperty.Register("LevelUpByCPorIv", typeof(string), typeof(ObservableSettings), new PropertyMetadata(string.Empty));

        public float UpgradePokemonCpMinimum
        {
            get { return (float)GetValue(UpgradePokemonCpMinimumProperty); }
            set { SetValue(UpgradePokemonCpMinimumProperty, value); }
        }
        public static readonly DependencyProperty UpgradePokemonCpMinimumProperty =
            DependencyProperty.Register("UpgradePokemonCpMinimum", typeof(float), typeof(ObservableSettings), new PropertyMetadata(0.0f));

        public float UpgradePokemonIvMinimum
        {
            get { return (float)GetValue(UpgradePokemonIvMinimumProperty); }
            set { SetValue(UpgradePokemonIvMinimumProperty, value); }
        }
        public static readonly DependencyProperty UpgradePokemonIvMinimumProperty =
            DependencyProperty.Register("UpgradePokemonIvMinimum", typeof(float), typeof(ObservableSettings), new PropertyMetadata(0.0f));

        public string UpgradePokemonMinimumStatsOperator
        {
            get { return (string)GetValue(UpgradePokemonMinimumStatsOperatorProperty); }
            set { SetValue(UpgradePokemonMinimumStatsOperatorProperty, value); }
        }
        public static readonly DependencyProperty UpgradePokemonMinimumStatsOperatorProperty =
            DependencyProperty.Register("UpgradePokemonMinimumStatsOperator", typeof(string), typeof(ObservableSettings), new PropertyMetadata(string.Empty));

        public float EvolveAboveIvValue
        {
            get { return (float)GetValue(EvolveAboveIvValueProperty); }
            set { SetValue(EvolveAboveIvValueProperty, value); }
        }
        public static readonly DependencyProperty EvolveAboveIvValueProperty =
            DependencyProperty.Register("EvolveAboveIvValue", typeof(float), typeof(ObservableSettings), new PropertyMetadata(0.0f));

        public bool EvolveAllPokemonAboveIv
        {
            get { return (bool)GetValue(EvolveAllPokemonAboveIvProperty); }
            set { SetValue(EvolveAllPokemonAboveIvProperty, value); }
        }
        public static readonly DependencyProperty EvolveAllPokemonAboveIvProperty =
            DependencyProperty.Register("EvolveAllPokemonAboveIv", typeof(bool), typeof(ObservableSettings), new PropertyMetadata(false));

        public bool EvolveAllPokemonWithEnoughCandy
        {
            get { return (bool)GetValue(EvolveAllPokemonWithEnoughCandyProperty); }
            set { SetValue(EvolveAllPokemonWithEnoughCandyProperty, value); }
        }
        public static readonly DependencyProperty EvolveAllPokemonWithEnoughCandyProperty =
            DependencyProperty.Register("EvolveAllPokemonWithEnoughCandy", typeof(bool), typeof(ObservableSettings), new PropertyMetadata(false));

        public double EvolveKeptPokemonsAtStorageUsagePercentage
        {
            get { return (double)GetValue(EvolveKeptPokemonsAtStorageUsagePercentageProperty); }
            set { SetValue(EvolveKeptPokemonsAtStorageUsagePercentageProperty, value); }
        }
        public static readonly DependencyProperty EvolveKeptPokemonsAtStorageUsagePercentageProperty =
            DependencyProperty.Register("EvolveKeptPokemonsAtStorageUsagePercentage", typeof(double), typeof(ObservableSettings), new PropertyMetadata(0.0));

        public bool KeepPokemonsThatCanEvolve
        {
            get { return (bool)GetValue(KeepPokemonsThatCanEvolveProperty); }
            set { SetValue(KeepPokemonsThatCanEvolveProperty, value); }
        }
        public static readonly DependencyProperty KeepPokemonsThatCanEvolveProperty =
            DependencyProperty.Register("KeepPokemonsThatCanEvolve", typeof(bool), typeof(ObservableSettings), new PropertyMetadata(false));

        public float FavoriteMinIvPercentage
        {
            get { return (float)GetValue(FavoriteMinIvPercentageProperty); }
            set { SetValue(FavoriteMinIvPercentageProperty, value); }
        }
        public static readonly DependencyProperty FavoriteMinIvPercentageProperty =
            DependencyProperty.Register("FavoriteMinIvPercentage", typeof(float), typeof(ObservableSettings), new PropertyMetadata(0.0f));

        public bool AutoFavoritePokemon
        {
            get { return (bool)GetValue(AutoFavoritePokemonProperty); }
            set { SetValue(AutoFavoritePokemonProperty, value); }
        }
        public static readonly DependencyProperty AutoFavoritePokemonProperty =
            DependencyProperty.Register("AutoFavoritePokemon", typeof(bool), typeof(ObservableSettings), new PropertyMetadata(false));

        #endregion POKEMON Properties


        #region CAPTURE Properties

        public bool CatchPokemon
        {
            get { return (bool)GetValue(CatchPokemonProperty); }
            set { SetValue(CatchPokemonProperty, value); }
        }
        public static readonly DependencyProperty CatchPokemonProperty =
            DependencyProperty.Register("CatchPokemon", typeof(bool), typeof(ObservableSettings), new PropertyMetadata(false));

        public bool UsePokemonToNotCatchFilter
        {
            get { return (bool)GetValue(UsePokemonToNotCatchFilterProperty); }
            set { SetValue(UsePokemonToNotCatchFilterProperty, value); }
        }
        public static readonly DependencyProperty UsePokemonToNotCatchFilterProperty =
            DependencyProperty.Register("UsePokemonToNotCatchFilter", typeof(bool), typeof(ObservableSettings), new PropertyMetadata(false));

        public int MaxPokeballsPerPokemon
        {
            get { return (int)GetValue(MaxPokeballsPerPokemonProperty); }
            set { SetValue(MaxPokeballsPerPokemonProperty, value); }
        }
        public static readonly DependencyProperty MaxPokeballsPerPokemonProperty =
            DependencyProperty.Register("MaxPokeballsPerPokemon", typeof(int), typeof(ObservableSettings), new PropertyMetadata(0));

        public int MaxBerriesToUsePerPokemon
        {
            get { return (int)GetValue(MaxBerriesToUsePerPokemonProperty); }
            set { SetValue(MaxBerriesToUsePerPokemonProperty, value); }
        }
        public static readonly DependencyProperty MaxBerriesToUsePerPokemonProperty =
            DependencyProperty.Register("MaxBerriesToUsePerPokemon", typeof(int), typeof(ObservableSettings), new PropertyMetadata(0));

        public int UseGreatBallAboveCp
        {
            get { return (int)GetValue(UseGreatBallAboveCpProperty); }
            set { SetValue(UseGreatBallAboveCpProperty, value); }
        }
        public static readonly DependencyProperty UseGreatBallAboveCpProperty =
            DependencyProperty.Register("UseGreatBallAboveCp", typeof(int), typeof(ObservableSettings), new PropertyMetadata(0));

        public int UseUltraBallAboveCp
        {
            get { return (int)GetValue(UseUltraBallAboveCpProperty); }
            set { SetValue(UseUltraBallAboveCpProperty, value); }
        }
        public static readonly DependencyProperty UseUltraBallAboveCpProperty =
            DependencyProperty.Register("UseUltraBallAboveCp", typeof(int), typeof(ObservableSettings), new PropertyMetadata(0));

        public int UseMasterBallAboveCp
        {
            get { return (int)GetValue(UseMasterBallAboveCpProperty); }
            set { SetValue(UseMasterBallAboveCpProperty, value); }
        }
        public static readonly DependencyProperty UseMasterBallAboveCpProperty =
            DependencyProperty.Register("UseMasterBallAboveCp", typeof(int), typeof(ObservableSettings), new PropertyMetadata(0));

        public double UseGreatBallAboveIv
        {
            get { return (double)GetValue(UseGreatBallAboveIvProperty); }
            set { SetValue(UseGreatBallAboveIvProperty, value); }
        }
        public static readonly DependencyProperty UseGreatBallAboveIvProperty =
            DependencyProperty.Register("UseGreatBallAboveIv", typeof(double), typeof(ObservableSettings), new PropertyMetadata(0.0));

        public double UseUltraBallAboveIv
        {
            get { return (double)GetValue(UseUltraBallAboveIvProperty); }
            set { SetValue(UseUltraBallAboveIvProperty, value); }
        }
        public static readonly DependencyProperty UseUltraBallAboveIvProperty =
            DependencyProperty.Register("UseUltraBallAboveIv", typeof(double), typeof(ObservableSettings), new PropertyMetadata(0.0));

        public double UseGreatBallBelowCatchProbability
        {
            get { return (double)GetValue(UseGreatBallBelowCatchProbabilityProperty); }
            set { SetValue(UseGreatBallBelowCatchProbabilityProperty, value); }
        }
        public static readonly DependencyProperty UseGreatBallBelowCatchProbabilityProperty =
            DependencyProperty.Register("UseGreatBallBelowCatchProbability", typeof(double), typeof(ObservableSettings), new PropertyMetadata(0.0));

        public double UseUltraBallBelowCatchProbability
        {
            get { return (double)GetValue(UseUltraBallBelowCatchProbabilityProperty); }
            set { SetValue(UseUltraBallBelowCatchProbabilityProperty, value); }
        }
        public static readonly DependencyProperty UseUltraBallBelowCatchProbabilityProperty =
            DependencyProperty.Register("UseUltraBallBelowCatchProbability", typeof(double), typeof(ObservableSettings), new PropertyMetadata(0.0));

        public double UseMasterBallBelowCatchProbability
        {
            get { return (double)GetValue(UseMasterBallBelowCatchProbabilityProperty); }
            set { SetValue(UseMasterBallBelowCatchProbabilityProperty, value); }
        }
        public static readonly DependencyProperty UseMasterBallBelowCatchProbabilityProperty =
            DependencyProperty.Register("UseMasterBallBelowCatchProbability", typeof(double), typeof(ObservableSettings), new PropertyMetadata(0.0));

        public int UseBerriesMinCp
        {
            get { return (int)GetValue(UseBerriesMinCpProperty); }
            set { SetValue(UseBerriesMinCpProperty, value); }
        }
        public static readonly DependencyProperty UseBerriesMinCpProperty =
            DependencyProperty.Register("UseBerriesMinCp", typeof(int), typeof(ObservableSettings), new PropertyMetadata(0));

        public float UseBerriesMinIv
        {
            get { return (float)GetValue(UseBerriesMinIvProperty); }
            set { SetValue(UseBerriesMinIvProperty, value); }
        }
        public static readonly DependencyProperty UseBerriesMinIvProperty =
            DependencyProperty.Register("UseBerriesMinIv", typeof(float), typeof(ObservableSettings), new PropertyMetadata(0.0f));

        public double UseBerriesBelowCatchProbability
        {
            get { return (double)GetValue(UseBerriesBelowCatchProbabilityProperty); }
            set { SetValue(UseBerriesBelowCatchProbabilityProperty, value); }
        }
        public static readonly DependencyProperty UseBerriesBelowCatchProbabilityProperty =
            DependencyProperty.Register("UseBerriesBelowCatchProbability", typeof(double), typeof(ObservableSettings), new PropertyMetadata(0.0));

        public string UseBerriesOperator
        {
            get { return (string)GetValue(UseBerriesOperatorProperty); }
            set { SetValue(UseBerriesOperatorProperty, value); }
        }
        public static readonly DependencyProperty UseBerriesOperatorProperty =
            DependencyProperty.Register("UseBerriesOperator", typeof(string), typeof(ObservableSettings), new PropertyMetadata(string.Empty));

        public bool EnableHumanizedThrows
        {
            get { return (bool)GetValue(EnableHumanizedThrowsProperty); }
            set { SetValue(EnableHumanizedThrowsProperty, value); }
        }
        public static readonly DependencyProperty EnableHumanizedThrowsProperty =
            DependencyProperty.Register("EnableHumanizedThrows", typeof(bool), typeof(ObservableSettings), new PropertyMetadata(false));

        public int NiceThrowChance
        {
            get { return (int)GetValue(NiceThrowChanceProperty); }
            set { SetValue(NiceThrowChanceProperty, value); }
        }
        public static readonly DependencyProperty NiceThrowChanceProperty =
            DependencyProperty.Register("NiceThrowChance", typeof(int), typeof(ObservableSettings), new FrameworkPropertyMetadata(0, new PropertyChangedCallback(ThrowChance_PropertyChanged)));

        public int GreatThrowChance
        {
            get { return (int)GetValue(GreatThrowChanceProperty); }
            set { SetValue(GreatThrowChanceProperty, value); }
        }
        public static readonly DependencyProperty GreatThrowChanceProperty =
            DependencyProperty.Register("GreatThrowChance", typeof(int), typeof(ObservableSettings), new FrameworkPropertyMetadata(0, new PropertyChangedCallback(ThrowChance_PropertyChanged)));

        public int ExcellentThrowChance
        {
            get { return (int)GetValue(ExcellentThrowChanceProperty); }
            set { SetValue(ExcellentThrowChanceProperty, value); }
        }
        public static readonly DependencyProperty ExcellentThrowChanceProperty =
            DependencyProperty.Register("ExcellentThrowChance", typeof(int), typeof(ObservableSettings), new FrameworkPropertyMetadata(0, new PropertyChangedCallback(ThrowChance_PropertyChanged)));

        static void ThrowChance_PropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            ObservableSettings os = obj as ObservableSettings;
            if (null != os) os.NormalThrowChance = 1;
        }

        public int NormalThrowChance
        {
            get { return 100 - (int)GetValue(ExcellentThrowChanceProperty) - (int)GetValue(GreatThrowChanceProperty) - (int)GetValue(NiceThrowChanceProperty); }
            set { SetValue(NormalThrowChanceProperty, 100 - (int)GetValue(ExcellentThrowChanceProperty) - (int)GetValue(GreatThrowChanceProperty) - (int)GetValue(NiceThrowChanceProperty)); }
        }
        public static readonly DependencyProperty NormalThrowChanceProperty =
            DependencyProperty.Register("NormalThrowChance", typeof(int), typeof(ObservableSettings), new PropertyMetadata(0));

        public int CurveThrowChance
        {
            get { return (int)GetValue(CurveThrowChanceProperty); }
            set { SetValue(CurveThrowChanceProperty, value); }
        }
        public static readonly DependencyProperty CurveThrowChanceProperty =
            DependencyProperty.Register("CurveThrowChance", typeof(int), typeof(ObservableSettings), new PropertyMetadata(0));

        public double ForceGreatThrowOverIv
        {
            get { return (double)GetValue(ForceGreatThrowOverIvProperty); }
            set { SetValue(ForceGreatThrowOverIvProperty, value); }
        }
        public static readonly DependencyProperty ForceGreatThrowOverIvProperty =
            DependencyProperty.Register("ForceGreatThrowOverIv", typeof(double), typeof(ObservableSettings), new PropertyMetadata(0.0));

        public double ForceExcellentThrowOverIv
        {
            get { return (double)GetValue(ForceExcellentThrowOverIvProperty); }
            set { SetValue(ForceExcellentThrowOverIvProperty, value); }
        }
        public static readonly DependencyProperty ForceExcellentThrowOverIvProperty =
            DependencyProperty.Register("ForceExcellentThrowOverIv", typeof(double), typeof(ObservableSettings), new PropertyMetadata(0.0));

        public int ForceGreatThrowOverCp
        {
            get { return (int)GetValue(ForceGreatThrowOverCpProperty); }
            set { SetValue(ForceGreatThrowOverCpProperty, value); }
        }
        public static readonly DependencyProperty ForceGreatThrowOverCpProperty =
            DependencyProperty.Register("ForceGreatThrowOverCp", typeof(int), typeof(ObservableSettings), new PropertyMetadata(0));

        public int ForceExcellentThrowOverCp
        {
            get { return (int)GetValue(ForceExcellentThrowOverCpProperty); }
            set { SetValue(ForceExcellentThrowOverCpProperty, value); }
        }
        public static readonly DependencyProperty ForceExcellentThrowOverCpProperty =
            DependencyProperty.Register("ForceExcellentThrowOverCp", typeof(int), typeof(ObservableSettings), new PropertyMetadata(0));

        #endregion CAPTURE Properties


        #region TRANSFER Properties
        #endregion TRANSFER Properties


        #region SNIPING Properties
        #endregion SNIPING Properties


        #region MISC Properties
        #endregion MISC Properties


        public ObservableSettings() { }


        public static ObservableSettings CreateFromGlobalSettings(GlobalSettings set)
        {
            ObservableSettings res = new ObservableSettings();
            // BASIC
            res.TranslationLanguageCode = set.TranslationLanguageCode;
            res.AutoUpdate = set.AutoUpdate;
            res.TransferConfigAndAuthOnUpdate = set.TransferConfigAndAuthOnUpdate;
            res.DisableHumanWalking = set.DisableHumanWalking;
            res.DefaultLatitude = set.DefaultLatitude;
            res.DefaultLongitude = set.DefaultLongitude;
            res.MaxTravelDistanceInMeters = set.MaxTravelDistanceInMeters;
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
            res.VerboseRecycling = set.VerboseRecycling;
            res.RecycleInventoryAtUsagePercentage = set.RecycleInventoryAtUsagePercentage;
            res.TotalAmountOfPokeballsToKeep = set.TotalAmountOfPokeballsToKeep;
            res.TotalAmountOfPotionsToKeep = set.TotalAmountOfPotionsToKeep;
            res.TotalAmountOfRevivesToKeep = set.TotalAmountOfRevivesToKeep;
            res.TotalAmountOfBerriesToKeep = set.TotalAmountOfBerriesToKeep;
            res.UseEggIncubators = set.UseEggIncubators;
            res.UseLuckyEggConstantly = set.UseLuckyEggConstantly;
            res.UseLuckyEggsMinPokemonAmount = set.UseLuckyEggsMinPokemonAmount;
            res.UseLuckyEggsWhileEvolving = set.UseLuckyEggsWhileEvolving;
            res.UseIncenseConstantly = set.UseIncenseConstantly;
            // POKEMON
            res.RenamePokemon = set.RenamePokemon;
            res.RenameOnlyAboveIv = set.RenameOnlyAboveIv;
            res.RenameTemplate = set.RenameTemplate;
            res.AutomaticallyLevelUpPokemon = set.AutomaticallyLevelUpPokemon;
            res.AmountOfTimesToUpgradeLoop = set.AmountOfTimesToUpgradeLoop;
            res.GetMinStarDustForLevelUp = set.GetMinStarDustForLevelUp;
            res.LevelUpByCPorIv = set.LevelUpByCPorIv;
            res.UpgradePokemonCpMinimum = set.UpgradePokemonCpMinimum;
            res.UpgradePokemonIvMinimum = set.UpgradePokemonIvMinimum;
            res.UpgradePokemonMinimumStatsOperator = set.UpgradePokemonMinimumStatsOperator;
            res.EvolveAboveIvValue = set.EvolveAboveIvValue;
            res.EvolveAllPokemonAboveIv = set.EvolveAllPokemonAboveIv;
            res.EvolveAllPokemonWithEnoughCandy = set.EvolveAllPokemonWithEnoughCandy;
            res.EvolveKeptPokemonsAtStorageUsagePercentage = set.EvolveKeptPokemonsAtStorageUsagePercentage;
            res.KeepPokemonsThatCanEvolve = set.KeepPokemonsThatCanEvolve;
            res.AutoFavoritePokemon = set.AutoFavoritePokemon;
            res.FavoriteMinIvPercentage = set.FavoriteMinIvPercentage;
            // CAPTURE
            res.CatchPokemon = set.CatchPokemon;
            res.UsePokemonToNotCatchFilter = set.UsePokemonToNotCatchFilter;
            res.MaxPokeballsPerPokemon = set.MaxPokeballsPerPokemon;
            res.MaxBerriesToUsePerPokemon = set.MaxBerriesToUsePerPokemon;
            res.UseGreatBallAboveCp = set.UseGreatBallAboveCp;
            res.UseUltraBallAboveCp = set.UseUltraBallAboveCp;
            res.UseMasterBallAboveCp = set.UseMasterBallAboveCp;
            res.UseGreatBallAboveIv = set.UseGreatBallAboveIv;
            res.UseUltraBallAboveIv = set.UseUltraBallAboveIv;
            res.UseGreatBallBelowCatchProbability = set.UseGreatBallBelowCatchProbability;
            res.UseUltraBallBelowCatchProbability = set.UseUltraBallBelowCatchProbability;
            res.UseMasterBallBelowCatchProbability = set.UseMasterBallBelowCatchProbability;
            res.UseBerriesMinCp = set.UseBerriesMinCp;
            res.UseBerriesMinIv = set.UseBerriesMinIv;
            res.UseBerriesBelowCatchProbability = set.UseBerriesBelowCatchProbability;
            res.UseBerriesOperator = set.UseBerriesOperator;
            res.EnableHumanizedThrows = set.EnableHumanizedThrows;
            res.NiceThrowChance = set.NiceThrowChance;
            res.GreatThrowChance = set.GreatThrowChance;
            res.ExcellentThrowChance = set.ExcellentThrowChance;
            res.CurveThrowChance = set.CurveThrowChance;
            res.ForceGreatThrowOverIv = set.ForceGreatThrowOverIv;
            res.ForceExcellentThrowOverIv = set.ForceExcellentThrowOverIv;
            res.ForceGreatThrowOverCp = set.ForceGreatThrowOverCp;
            res.ForceExcellentThrowOverCp = set.ForceExcellentThrowOverCp;
            // TRANSFER
            // SNIPING
            // MISC
            return res;
        }

        /*
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

            CAPTURE


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
            gs.MaxTravelDistanceInMeters = MaxTravelDistanceInMeters;
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
            gs.VerboseRecycling = VerboseRecycling;
            gs.RecycleInventoryAtUsagePercentage = RecycleInventoryAtUsagePercentage;
            gs.TotalAmountOfPokeballsToKeep = TotalAmountOfPokeballsToKeep;
            gs.TotalAmountOfPotionsToKeep = TotalAmountOfPotionsToKeep;
            gs.TotalAmountOfRevivesToKeep = TotalAmountOfRevivesToKeep;
            gs.TotalAmountOfBerriesToKeep = TotalAmountOfBerriesToKeep;
            gs.UseEggIncubators = UseEggIncubators;
            gs.UseLuckyEggConstantly = UseLuckyEggConstantly;
            gs.UseLuckyEggsMinPokemonAmount = UseLuckyEggsMinPokemonAmount;
            gs.UseLuckyEggsWhileEvolving = UseLuckyEggsWhileEvolving;
            gs.UseIncenseConstantly = UseIncenseConstantly;
            // POKEMON
            gs.RenamePokemon = RenamePokemon;
            gs.RenameOnlyAboveIv = RenameOnlyAboveIv;
            gs.RenameTemplate = RenameTemplate;
            gs.AutomaticallyLevelUpPokemon = AutomaticallyLevelUpPokemon;
            gs.AmountOfTimesToUpgradeLoop = AmountOfTimesToUpgradeLoop;
            gs.GetMinStarDustForLevelUp = GetMinStarDustForLevelUp;
            gs.LevelUpByCPorIv = LevelUpByCPorIv;
            gs.UpgradePokemonCpMinimum = UpgradePokemonCpMinimum;
            gs.UpgradePokemonIvMinimum = UpgradePokemonIvMinimum;
            gs.UpgradePokemonMinimumStatsOperator = UpgradePokemonMinimumStatsOperator;
            gs.EvolveAboveIvValue = EvolveAboveIvValue;
            gs.EvolveAllPokemonAboveIv = EvolveAllPokemonAboveIv;
            gs.EvolveAllPokemonWithEnoughCandy = EvolveAllPokemonWithEnoughCandy;
            gs.EvolveKeptPokemonsAtStorageUsagePercentage = EvolveKeptPokemonsAtStorageUsagePercentage;
            gs.KeepPokemonsThatCanEvolve = KeepPokemonsThatCanEvolve;
            gs.AutoFavoritePokemon = AutoFavoritePokemon;
            gs.FavoriteMinIvPercentage = FavoriteMinIvPercentage;
            // CAPTURE
            gs.CatchPokemon = CatchPokemon;
            gs.UsePokemonToNotCatchFilter = UsePokemonToNotCatchFilter;
            gs.MaxPokeballsPerPokemon = MaxPokeballsPerPokemon;
            gs.MaxBerriesToUsePerPokemon = MaxBerriesToUsePerPokemon;
            gs.UseGreatBallAboveCp = UseGreatBallAboveCp;
            gs.UseUltraBallAboveCp = UseUltraBallAboveCp;
            gs.UseMasterBallAboveCp = UseMasterBallAboveCp;
            gs.UseGreatBallAboveIv = UseGreatBallAboveIv;
            gs.UseUltraBallAboveIv = UseUltraBallAboveIv;
            gs.UseGreatBallBelowCatchProbability = UseGreatBallBelowCatchProbability;
            gs.UseUltraBallBelowCatchProbability = UseUltraBallBelowCatchProbability;
            gs.UseMasterBallBelowCatchProbability = UseMasterBallBelowCatchProbability;
            gs.UseBerriesMinCp = UseBerriesMinCp;
            gs.UseBerriesMinIv = UseBerriesMinIv;
            gs.UseBerriesBelowCatchProbability = UseBerriesBelowCatchProbability;
            gs.UseBerriesOperator = UseBerriesOperator;
            gs.EnableHumanizedThrows = EnableHumanizedThrows;
            gs.NiceThrowChance = NiceThrowChance;
            gs.GreatThrowChance = GreatThrowChance;
            gs.ExcellentThrowChance = ExcellentThrowChance;
            gs.CurveThrowChance = CurveThrowChance;
            gs.ForceGreatThrowOverIv = ForceGreatThrowOverIv;
            gs.ForceExcellentThrowOverIv = ForceExcellentThrowOverIv;
            gs.ForceGreatThrowOverCp = ForceGreatThrowOverCp;
            gs.ForceExcellentThrowOverCp = ForceExcellentThrowOverCp;
            // TRANSFER
            // SNIPING
            // MISC
            return gs;
        }

    }

}
