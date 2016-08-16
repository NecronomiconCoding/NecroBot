using System.Windows;
using PoGo.NecroBot.Logic;
using PokemonGo.RocketAPI.Enums;
using System.Collections.Generic;
using POGOProtos.Inventory.Item;
using POGOProtos.Enums;
using System.Collections.ObjectModel;
using System;
using System.IO;

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

        public bool StartupWelcomeDelay
        {
            get { return (bool)GetValue(StartupWelcomeDelayProperty); }
            set { SetValue(StartupWelcomeDelayProperty, value); }
        }
        public static readonly DependencyProperty StartupWelcomeDelayProperty =
            DependencyProperty.Register("StartupWelcomeDelay", typeof(bool), typeof(ObservableSettings), new PropertyMetadata(false));

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

        public bool UseWalkingSpeedVariant
        {
            get { return (bool)GetValue(UseWalkingSpeedVariantProperty); }
            set { SetValue(UseWalkingSpeedVariantProperty, value); }
        }
        public static readonly DependencyProperty UseWalkingSpeedVariantProperty =
            DependencyProperty.Register("UseWalkingSpeedVariant", typeof(bool), typeof(ObservableSettings), new PropertyMetadata(true));

        public bool ShowVariantWalking
        {
            get { return (bool)GetValue(ShowVariantWalkingProperty); }
            set { SetValue(ShowVariantWalkingProperty, value); }
        }
        public static readonly DependencyProperty ShowVariantWalkingProperty =
            DependencyProperty.Register("ShowVariantWalking", typeof(bool), typeof(ObservableSettings), new PropertyMetadata(true));

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

        public bool RandomizeRecycle
        {
            get { return (bool)GetValue(RandomizeRecycleProperty); }
            set { SetValue(RandomizeRecycleProperty, value); }
        }
        public static readonly DependencyProperty RandomizeRecycleProperty =
            DependencyProperty.Register("RandomizeRecycle", typeof(bool), typeof(ObservableSettings), new PropertyMetadata(false));

        public int RandomRecycleValue
        {
            get { return (int)GetValue(RandomRecycleValueProperty); }
            set { SetValue(RandomRecycleValueProperty, value); }
        }
        public static readonly DependencyProperty RandomRecycleValueProperty =
            DependencyProperty.Register("RandomRecycleValue", typeof(int), typeof(ObservableSettings), new PropertyMetadata(0));

        public bool DelayBetweenRecycleActions
        {
            get { return (bool)GetValue(DelayBetweenRecycleActionsProperty); }
            set { SetValue(DelayBetweenRecycleActionsProperty, value); }
        }
        public static readonly DependencyProperty DelayBetweenRecycleActionsProperty =
            DependencyProperty.Register("DelayBetweenRecycleActions", typeof(bool), typeof(ObservableSettings), new PropertyMetadata(false));

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

        public bool OnlyUpgradeFavorites
        {
            get { return (bool)GetValue(OnlyUpgradeFavoritesProperty); }
            set { SetValue(OnlyUpgradeFavoritesProperty, value); }
        }
        public static readonly DependencyProperty OnlyUpgradeFavoritesProperty =
            DependencyProperty.Register("OnlyUpgradeFavorites", typeof(bool), typeof(ObservableSettings), new PropertyMetadata(false));

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

        public bool TransferWeakPokemon
        {
            get { return (bool)GetValue(TransferWeakPokemonProperty); }
            set { SetValue(TransferWeakPokemonProperty, value); }
        }
        public static readonly DependencyProperty TransferWeakPokemonProperty =
            DependencyProperty.Register("TransferWeakPokemon", typeof(bool), typeof(ObservableSettings), new PropertyMetadata(false));

        public bool TransferDuplicatePokemon
        {
            get { return (bool)GetValue(TransferDuplicatePokemonProperty); }
            set { SetValue(TransferDuplicatePokemonProperty, value); }
        }
        public static readonly DependencyProperty TransferDuplicatePokemonProperty =
            DependencyProperty.Register("TransferDuplicatePokemon", typeof(bool), typeof(ObservableSettings), new PropertyMetadata(false));

        public bool TransferDuplicatePokemonOnCapture
        {
            get { return (bool)GetValue(TransferDuplicatePokemonOnCaptureProperty); }
            set { SetValue(TransferDuplicatePokemonOnCaptureProperty, value); }
        }
        public static readonly DependencyProperty TransferDuplicatePokemonOnCaptureProperty =
            DependencyProperty.Register("TransferDuplicatePokemonOnCapture", typeof(bool), typeof(ObservableSettings), new PropertyMetadata(false));

        public int KeepMinCp
        {
            get { return (int)GetValue(KeepMinCpProperty); }
            set { SetValue(KeepMinCpProperty, value); }
        }
        public static readonly DependencyProperty KeepMinCpProperty =
            DependencyProperty.Register("KeepMinCp", typeof(int), typeof(ObservableSettings), new PropertyMetadata(0));

        public float KeepMinIvPercentage
        {
            get { return (float)GetValue(KeepMinIvPercentageProperty); }
            set { SetValue(KeepMinIvPercentageProperty, value); }
        }
        public static readonly DependencyProperty KeepMinIvPercentageProperty =
            DependencyProperty.Register("KeepMinIvPercentage", typeof(float), typeof(ObservableSettings), new PropertyMetadata(0.0f));

        public int KeepMinLvl
        {
            get { return (int)GetValue(KeepMinLvlProperty); }
            set { SetValue(KeepMinLvlProperty, value); }
        }
        public static readonly DependencyProperty KeepMinLvlProperty =
            DependencyProperty.Register("KeepMinLvl", typeof(int), typeof(ObservableSettings), new PropertyMetadata(0));

        public string KeepMinOperator
        {
            get { return (string)GetValue(KeepMinOperatorProperty); }
            set { SetValue(KeepMinOperatorProperty, value); }
        }
        public static readonly DependencyProperty KeepMinOperatorProperty =
            DependencyProperty.Register("KeepMinOperator", typeof(string), typeof(ObservableSettings), new PropertyMetadata(string.Empty));

        public bool UseKeepMinLevel
        {
            get { return (bool)GetValue(UseKeepMinLevelProperty); }
            set { SetValue(UseKeepMinLevelProperty, value); }
        }
        public static readonly DependencyProperty UseKeepMinLevelProperty =
            DependencyProperty.Register("UseKeepMinLevel", typeof(bool), typeof(ObservableSettings), new PropertyMetadata(false));

        public bool PrioritizeIvOverCp
        {
            get { return (bool)GetValue(PrioritizeIvOverCpProperty); }
            set { SetValue(PrioritizeIvOverCpProperty, value); }
        }
        public static readonly DependencyProperty PrioritizeIvOverCpProperty =
            DependencyProperty.Register("PrioritizeIvOverCp", typeof(bool), typeof(ObservableSettings), new PropertyMetadata(false));

        public int KeepMinDuplicatePokemon
        {
            get { return (int)GetValue(KeepMinDuplicatePokemonProperty); }
            set { SetValue(KeepMinDuplicatePokemonProperty, value); }
        }
        public static readonly DependencyProperty KeepMinDuplicatePokemonProperty =
            DependencyProperty.Register("KeepMinDuplicatePokemon", typeof(int), typeof(ObservableSettings), new PropertyMetadata(0));

        #endregion TRANSFER Properties


        #region SNIPING Properties

        public bool UseSnipeLocationServer
        {
            get { return (bool)GetValue(UseSnipeLocationServerProperty); }
            set { SetValue(UseSnipeLocationServerProperty, value); }
        }
        public static readonly DependencyProperty UseSnipeLocationServerProperty =
            DependencyProperty.Register("UseSnipeLocationServer", typeof(bool), typeof(ObservableSettings), new PropertyMetadata(false));

        public string SnipeLocationServer
        {
            get { return (string)GetValue(SnipeLocationServerProperty); }
            set { SetValue(SnipeLocationServerProperty, value); }
        }
        public static readonly DependencyProperty SnipeLocationServerProperty =
            DependencyProperty.Register("SnipeLocationServer", typeof(string), typeof(ObservableSettings), new PropertyMetadata(string.Empty));

        public int SnipeLocationServerPort
        {
            get { return (int)GetValue(SnipeLocationServerPortProperty); }
            set { SetValue(SnipeLocationServerPortProperty, value); }
        }
        public static readonly DependencyProperty SnipeLocationServerPortProperty =
            DependencyProperty.Register("SnipeLocationServerPort", typeof(int), typeof(ObservableSettings), new PropertyMetadata(0));

        public bool GetSniperInfoFromPokezz
        {
            get { return (bool)GetValue(GetSniperInfoFromPokezzProperty); }
            set { SetValue(GetSniperInfoFromPokezzProperty, value); }
        }
        public static readonly DependencyProperty GetSniperInfoFromPokezzProperty =
            DependencyProperty.Register("GetSniperInfoFromPokezz", typeof(bool), typeof(ObservableSettings), new PropertyMetadata(false));

        public bool GetOnlyVerifiedSniperInfoFromPokezz
        {
            get { return (bool)GetValue(GetOnlyVerifiedSniperInfoFromPokezzProperty); }
            set { SetValue(GetOnlyVerifiedSniperInfoFromPokezzProperty, value); }
        }
        public static readonly DependencyProperty GetOnlyVerifiedSniperInfoFromPokezzProperty =
            DependencyProperty.Register("GetOnlyVerifiedSniperInfoFromPokezz", typeof(bool), typeof(ObservableSettings), new PropertyMetadata(false));

        public bool GetSniperInfoFromPokeSnipers
        {
            get { return (bool)GetValue(GetSniperInfoFromPokeSnipersProperty); }
            set { SetValue(GetSniperInfoFromPokeSnipersProperty, value); }
        }
        public static readonly DependencyProperty GetSniperInfoFromPokeSnipersProperty =
            DependencyProperty.Register("GetSniperInfoFromPokeSnipers", typeof(bool), typeof(ObservableSettings), new PropertyMetadata(false));

        public bool GetSniperInfoFromPokeWatchers
        {
            get { return (bool)GetValue(GetSniperInfoFromPokeWatchersProperty); }
            set { SetValue(GetSniperInfoFromPokeWatchersProperty, value); }
        }
        public static readonly DependencyProperty GetSniperInfoFromPokeWatchersProperty =
            DependencyProperty.Register("GetSniperInfoFromPokeWatchers", typeof(bool), typeof(ObservableSettings), new PropertyMetadata(false));
        
        public bool GetSniperInfoFromSkiplagged
        {
            get { return (bool)GetValue(GetSniperInfoFromSkiplaggedProperty); }
            set { SetValue(GetSniperInfoFromSkiplaggedProperty, value); }
        }
        public static readonly DependencyProperty GetSniperInfoFromSkiplaggedProperty =
            DependencyProperty.Register("GetSniperInfoFromSkiplagged", typeof(bool), typeof(ObservableSettings), new PropertyMetadata(false));

        public int MinPokeballsToSnipe
        {
            get { return (int)GetValue(MinPokeballsToSnipeProperty); }
            set { SetValue(MinPokeballsToSnipeProperty, value); }
        }
        public static readonly DependencyProperty MinPokeballsToSnipeProperty =
            DependencyProperty.Register("MinPokeballsToSnipe", typeof(int), typeof(ObservableSettings), new PropertyMetadata(0));

        public int MinPokeballsWhileSnipe
        {
            get { return (int)GetValue(MinPokeballsWhileSnipeProperty); }
            set { SetValue(MinPokeballsWhileSnipeProperty, value); }
        }
        public static readonly DependencyProperty MinPokeballsWhileSnipeProperty =
            DependencyProperty.Register("MinPokeballsWhileSnipe", typeof(int), typeof(ObservableSettings), new PropertyMetadata(0));

        public int MinDelayBetweenSnipes
        {
            get { return (int)GetValue(MinDelayBetweenSnipesProperty); }
            set { SetValue(MinDelayBetweenSnipesProperty, value); }
        }
        public static readonly DependencyProperty MinDelayBetweenSnipesProperty =
            DependencyProperty.Register("MinDelayBetweenSnipes", typeof(int), typeof(ObservableSettings), new PropertyMetadata(0));

        public double SnipingScanOffset
        {
            get { return (double)GetValue(SnipingScanOffsetProperty); }
            set { SetValue(SnipingScanOffsetProperty, value); }
        }
        public static readonly DependencyProperty SnipingScanOffsetProperty =
            DependencyProperty.Register("SnipingScanOffset", typeof(double), typeof(ObservableSettings), new PropertyMetadata(0.0));

        public bool SnipeAtPokestops
        {
            get { return (bool)GetValue(SnipeAtPokestopsProperty); }
            set { SetValue(SnipeAtPokestopsProperty, value); }
        }
        public static readonly DependencyProperty SnipeAtPokestopsProperty =
            DependencyProperty.Register("SnipeAtPokestops", typeof(bool), typeof(ObservableSettings), new PropertyMetadata(false));

        public bool SnipeIgnoreUnknownIv
        {
            get { return (bool)GetValue(SnipeIgnoreUnknownIvProperty); }
            set { SetValue(SnipeIgnoreUnknownIvProperty, value); }
        }
        public static readonly DependencyProperty SnipeIgnoreUnknownIvProperty =
            DependencyProperty.Register("SnipeIgnoreUnknownIv", typeof(bool), typeof(ObservableSettings), new PropertyMetadata(false));

        public bool UseTransferIvForSnipe
        {
            get { return (bool)GetValue(UseTransferIvForSnipeProperty); }
            set { SetValue(UseTransferIvForSnipeProperty, value); }
        }
        public static readonly DependencyProperty UseTransferIvForSnipeProperty =
            DependencyProperty.Register("UseTransferIvForSnipe", typeof(bool), typeof(ObservableSettings), new PropertyMetadata(false));

        public bool SnipePokemonNotInPokedex
        {
            get { return (bool)GetValue(SnipePokemonNotInPokedexProperty); }
            set { SetValue(SnipePokemonNotInPokedexProperty, value); }
        }
        public static readonly DependencyProperty SnipePokemonNotInPokedexProperty =
            DependencyProperty.Register("SnipePokemonNotInPokedex", typeof(bool), typeof(ObservableSettings), new PropertyMetadata(false));

        #endregion SNIPING Properties


        #region MISC Properties

        public bool FastSoftBanBypass
        {
            get { return (bool)GetValue(FastSoftBanBypassProperty); }
            set { SetValue(FastSoftBanBypassProperty, value); }
        }
        public static readonly DependencyProperty FastSoftBanBypassProperty =
            DependencyProperty.Register("FastSoftBanBypass", typeof(bool), typeof(ObservableSettings), new PropertyMetadata(false));

        public bool UseGpxPathing
        {
            get { return (bool)GetValue(UseGpxPathingProperty); }
            set { SetValue(UseGpxPathingProperty, value); }
        }
        public static readonly DependencyProperty UseGpxPathingProperty =
            DependencyProperty.Register("UseGpxPathing", typeof(bool), typeof(ObservableSettings), new PropertyMetadata(false));

        public string GpxFile
        {
            get { return (string)GetValue(GpxFileProperty); }
            set { SetValue(GpxFileProperty, value); }
        }
        public static readonly DependencyProperty GpxFileProperty =
            DependencyProperty.Register("GpxFile", typeof(string), typeof(ObservableSettings), new PropertyMetadata(string.Empty));

        public bool UseWebsocket
        {
            get { return (bool)GetValue(UseWebsocketProperty); }
            set { SetValue(UseWebsocketProperty, value); }
        }
        public static readonly DependencyProperty UseWebsocketProperty =
            DependencyProperty.Register("UseWebsocket", typeof(bool), typeof(ObservableSettings), new PropertyMetadata(false));

        public int WebSocketPort
        {
            get { return (int)GetValue(WebSocketPortProperty); }
            set { SetValue(WebSocketPortProperty, value); }
        }
        public static readonly DependencyProperty WebSocketPortProperty =
            DependencyProperty.Register("WebSocketPort", typeof(int), typeof(ObservableSettings), new PropertyMetadata(0));

        public bool UseTelegramAPI
        {
            get { return (bool)GetValue(UseTelegramAPIProperty); }
            set { SetValue(UseTelegramAPIProperty, value); }
        }
        public static readonly DependencyProperty UseTelegramAPIProperty =
            DependencyProperty.Register("UseTelegramAPI", typeof(bool), typeof(ObservableSettings), new PropertyMetadata(false));

        public string TelegramAPIKey
        {
            get { return (string)GetValue(TelegramAPIKeyProperty); }
            set { SetValue(TelegramAPIKeyProperty, value); }
        }
        public static readonly DependencyProperty TelegramAPIKeyProperty =
            DependencyProperty.Register("TelegramAPIKey", typeof(string), typeof(ObservableSettings), new PropertyMetadata(string.Empty));

        public string TelegramPassword
        {
            get { return (string)GetValue(TelegramPasswordProperty); }
            set { SetValue(TelegramPasswordProperty, value); }
        }
        public static readonly DependencyProperty TelegramPasswordProperty =
            DependencyProperty.Register("TelegramPassword", typeof(string), typeof(ObservableSettings), new PropertyMetadata(string.Empty));

        public int AmountOfPokemonToDisplayOnStart
        {
            get { return (int)GetValue(AmountOfPokemonToDisplayOnStartProperty); }
            set { SetValue(AmountOfPokemonToDisplayOnStartProperty, value); }
        }
        public static readonly DependencyProperty AmountOfPokemonToDisplayOnStartProperty =
            DependencyProperty.Register("AmountOfPokemonToDisplayOnStart", typeof(int), typeof(ObservableSettings), new PropertyMetadata(0));

        public bool DetailedCountsBeforeRecycling
        {
            get { return (bool)GetValue(DetailedCountsBeforeRecyclingProperty); }
            set { SetValue(DetailedCountsBeforeRecyclingProperty, value); }
        }
        public static readonly DependencyProperty DetailedCountsBeforeRecyclingProperty =
            DependencyProperty.Register("DetailedCountsBeforeRecycling", typeof(bool), typeof(ObservableSettings), new PropertyMetadata(false));

        public bool DumpPokemonStats
        {
            get { return (bool)GetValue(DumpPokemonStatsProperty); }
            set { SetValue(DumpPokemonStatsProperty, value); }
        }
        public static readonly DependencyProperty DumpPokemonStatsProperty =
            DependencyProperty.Register("DumpPokemonStats", typeof(bool), typeof(ObservableSettings), new PropertyMetadata(false));

        #endregion MISC Properties


        public List<KeyValuePair<ItemId, int>> ItemRecycleFilter
        {
            get { return (List<KeyValuePair<ItemId, int>>)GetValue(ItemRecycleFilterProperty); }
            set { SetValue(ItemRecycleFilterProperty, value); }
        }
        public static readonly DependencyProperty ItemRecycleFilterProperty =
            DependencyProperty.Register("ItemRecycleFilter", typeof(List<KeyValuePair<ItemId, int>>), typeof(ObservableSettings), new PropertyMetadata(null));

        public ObservableCollection<PokemonToggle> NoTransferCollection
        {
            get { return (ObservableCollection<PokemonToggle>)GetValue(NoTransferCollectionProperty); }
            set { SetValue(NoTransferCollectionProperty, value); }
        }
        public static readonly DependencyProperty NoTransferCollectionProperty =
            DependencyProperty.Register("NoTransferCollection", typeof(ObservableCollection<PokemonToggle>), typeof(ObservableSettings), new PropertyMetadata(null));

        public ObservableCollection<PokemonToggle> EvolveCollection
        {
            get { return (ObservableCollection<PokemonToggle>)GetValue(EvolveCollectionProperty); }
            set { SetValue(EvolveCollectionProperty, value); }
        }
        public static readonly DependencyProperty EvolveCollectionProperty =
            DependencyProperty.Register("EvolveCollection", typeof(ObservableCollection<PokemonToggle>), typeof(ObservableSettings), new PropertyMetadata(null));

        public ObservableCollection<PokemonToggle> UpgradeCollection
        {
            get { return (ObservableCollection<PokemonToggle>)GetValue(UpgradeCollectionProperty); }
            set { SetValue(UpgradeCollectionProperty, value); }
        }
        public static readonly DependencyProperty UpgradeCollectionProperty =
            DependencyProperty.Register("UpgradeCollection", typeof(ObservableCollection<PokemonToggle>), typeof(ObservableSettings), new PropertyMetadata(null));

        public ObservableCollection<PokemonToggle> IgnoreCollection
        {
            get { return (ObservableCollection<PokemonToggle>)GetValue(IgnoreCollectionProperty); }
            set { SetValue(IgnoreCollectionProperty, value); }
        }
        public static readonly DependencyProperty IgnoreCollectionProperty =
            DependencyProperty.Register("IgnoreCollection", typeof(ObservableCollection<PokemonToggle>), typeof(ObservableSettings), new PropertyMetadata(null));

        public ObservableCollection<PokemonToggle> MasterballCollection
        {
            get { return (ObservableCollection<PokemonToggle>)GetValue(MasterballCollectionProperty); }
            set { SetValue(MasterballCollectionProperty, value); }
        }
        public static readonly DependencyProperty MasterballCollectionProperty =
            DependencyProperty.Register("MasterballCollection", typeof(ObservableCollection<PokemonToggle>), typeof(ObservableSettings), new PropertyMetadata(null));

        public Dictionary<PokemonId, TransferFilter> PokemonsTransferFilter
        {
            get { return (Dictionary<PokemonId, TransferFilter>)GetValue(PokemonsTransferFilterProperty); }
            set { SetValue(PokemonsTransferFilterProperty, value); }
        }
        public static readonly DependencyProperty PokemonsTransferFilterProperty =
            DependencyProperty.Register("PokemonsTransferFilter", typeof(Dictionary<PokemonId, TransferFilter>), typeof(ObservableSettings), new PropertyMetadata(null));

        public SnipeSettings PokemonToSnipe
        {
            get { return (SnipeSettings)GetValue(PokemonToSnipeProperty); }
            set { SetValue(PokemonToSnipeProperty, value); }
        }
        public static readonly DependencyProperty PokemonToSnipeProperty =
            DependencyProperty.Register("PokemonToSnipe", typeof(SnipeSettings), typeof(ObservableSettings), new PropertyMetadata(null));



        public ObservableSettings()
        {
            NoTransferCollection = new ObservableCollection<PokemonToggle>();
            EvolveCollection = new ObservableCollection<PokemonToggle>();
            UpgradeCollection = new ObservableCollection<PokemonToggle>();
            IgnoreCollection = new ObservableCollection<PokemonToggle>();
            MasterballCollection = new ObservableCollection<PokemonToggle>();
            PokemonsTransferFilter = new Dictionary<PokemonId, TransferFilter>();
            ItemRecycleFilter = new List<KeyValuePair<ItemId, int>>();
        }

        private void LoadCurrentCoords()
        {
            Latitude = DefaultLatitude;
            Longitude = DefaultLongitude;
            string coordFile = Path.Combine(Directory.GetCurrentDirectory(), "Config", "LastPos.ini");
            if (File.Exists(coordFile))
            {
                var latlngFromFile = File.ReadAllText(coordFile);
                var latlng = latlngFromFile.Split(':');
                if (latlng[0].Length != 0 && latlng[1].Length != 0)
                {
                    try
                    {
                        Latitude = Convert.ToDouble(latlng[0]);
                        Longitude = Convert.ToDouble(latlng[1]);
                    }
                    catch (FormatException) { }
                }
            }
        }

        private void SaveCurrentCoords()
        {
            var coordsPath = Path.Combine(Directory.GetCurrentDirectory(), "Config", "LastPos.ini");
            File.WriteAllText(coordsPath, $"{Latitude}:{Longitude}");
        }

        public static ObservableSettings CreateFromGlobalSettings(GlobalSettings set)
        {
            ObservableSettings res = new ObservableSettings();
            // BASIC
            res.TranslationLanguageCode = set.TranslationLanguageCode;
            res.AutoUpdate = set.AutoUpdate;
            res.TransferConfigAndAuthOnUpdate = set.TransferConfigAndAuthOnUpdate;
            res.StartupWelcomeDelay = set.StartupWelcomeDelay;
            res.DisableHumanWalking = set.DisableHumanWalking;
            res.DefaultLatitude = set.DefaultLatitude;
            res.DefaultLongitude = set.DefaultLongitude;
            res.MaxTravelDistanceInMeters = set.MaxTravelDistanceInMeters;
            res.WalkingSpeedInKilometerPerHour = set.WalkingSpeedInKilometerPerHour;
            res.UseWalkingSpeedVariant = set.UseWalkingSpeedVariant;
            res.ShowVariantWalking = set.ShowVariantWalking;
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
            res.OnlyUpgradeFavorites = set.OnlyUpgradeFavorites;
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
            res.TransferWeakPokemon = set.TransferWeakPokemon;
            res.TransferDuplicatePokemon = set.TransferDuplicatePokemon;
            res.TransferDuplicatePokemonOnCapture = set.TransferDuplicatePokemonOnCapture;
            res.KeepMinCp = set.KeepMinCp;
            res.KeepMinIvPercentage = set.KeepMinIvPercentage;
            res.KeepMinLvl = set.KeepMinLvl;
            res.KeepMinOperator = set.KeepMinOperator;
            res.UseKeepMinLevel = set.UseKeepMinLvl;
            res.PrioritizeIvOverCp = set.PrioritizeIvOverCp;
            res.KeepMinDuplicatePokemon = set.KeepMinDuplicatePokemon;
            // SNIPING
            res.UseSnipeLocationServer = set.UseSnipeLocationServer;
            res.SnipeLocationServer = set.SnipeLocationServer;
            res.SnipeLocationServerPort = set.SnipeLocationServerPort;
            res.GetSniperInfoFromPokezz = set.GetSniperInfoFromPokezz;
            res.GetOnlyVerifiedSniperInfoFromPokezz = set.GetOnlyVerifiedSniperInfoFromPokezz;
            res.GetSniperInfoFromPokeSnipers = set.GetSniperInfoFromPokeSnipers;
            res.GetSniperInfoFromPokeWatchers = set.GetSniperInfoFromPokeWatchers;
            res.GetSniperInfoFromSkiplagged = set.GetSniperInfoFromSkiplagged;
            res.MinPokeballsToSnipe = set.MinPokeballsToSnipe;
            res.MinPokeballsWhileSnipe = set.MinPokeballsWhileSnipe;
            res.MinDelayBetweenSnipes = set.MinDelayBetweenSnipes;
            res.SnipingScanOffset = set.SnipingScanOffset;
            res.SnipeAtPokestops = set.SnipeAtPokestops;
            res.SnipeIgnoreUnknownIv = set.SnipeIgnoreUnknownIv;
            res.UseTransferIvForSnipe = set.UseTransferIvForSnipe;
            res.SnipePokemonNotInPokedex = set.SnipePokemonNotInPokedex;
            // MISC
            res.FastSoftBanBypass = set.FastSoftBanBypass;
            res.UseGpxPathing = set.UseGpxPathing;
            res.GpxFile = set.GpxFile;
            res.UseWebsocket = set.UseWebsocket;
            res.WebSocketPort = set.WebSocketPort;
            res.UseTelegramAPI = set.UseTelegramAPI;
            res.TelegramAPIKey = set.TelegramAPIKey;
            res.TelegramPassword = set.TelegramPassword;
            res.AmountOfPokemonToDisplayOnStart = set.AmountOfPokemonToDisplayOnStart;
            res.DetailedCountsBeforeRecycling = set.DetailedCountsBeforeRecycling;
            res.DumpPokemonStats = set.DumpPokemonStats;
            // OBJECTS & ITERATORS
            res.PokemonToSnipe = set.PokemonToSnipe;
            foreach (PokemonId pid in Enum.GetValues(typeof(PokemonId)))
            {
                res.NoTransferCollection.Add(new PokemonToggle(pid, (null != set.PokemonsNotToTransfer  && set.PokemonsNotToTransfer.Contains(pid)) ));
                res.EvolveCollection.Add(    new PokemonToggle(pid, (null != set.PokemonsToEvolve       && set.PokemonsToEvolve.Contains(pid)) ));
                res.UpgradeCollection.Add(   new PokemonToggle(pid, (null != set.PokemonsToLevelUp      && set.PokemonsToLevelUp.Contains(pid)) ));
                res.IgnoreCollection.Add(    new PokemonToggle(pid, (null != set.PokemonsToIgnore       && set.PokemonsToIgnore.Contains(pid)) ));
                res.MasterballCollection.Add(new PokemonToggle(pid, (null != set.PokemonToUseMasterball && set.PokemonToUseMasterball.Contains(pid)) ));
            }
            foreach (PokemonId key in set.PokemonsTransferFilter.Keys)
                res.PokemonsTransferFilter.Add(key, set.PokemonsTransferFilter[key]);
            foreach (KeyValuePair<ItemId, int> kvp in set.ItemRecycleFilter)
                res.ItemRecycleFilter.Add(kvp);

            res.LoadCurrentCoords();

            return res;
        }

        internal GlobalSettings GetGlobalSettingsObject()
        {
            GlobalSettings gs = new GlobalSettings();
            // BASIC
            gs.TranslationLanguageCode = TranslationLanguageCode;
            gs.AutoUpdate = AutoUpdate;
            gs.TransferConfigAndAuthOnUpdate = TransferConfigAndAuthOnUpdate;
            gs.StartupWelcomeDelay = StartupWelcomeDelay;
            gs.DisableHumanWalking = DisableHumanWalking;
            gs.DefaultLatitude = DefaultLatitude;
            gs.DefaultLongitude = DefaultLongitude;
            gs.MaxTravelDistanceInMeters = MaxTravelDistanceInMeters;
            gs.WalkingSpeedInKilometerPerHour = WalkingSpeedInKilometerPerHour;
            gs.UseWalkingSpeedVariant = UseWalkingSpeedVariant;
            gs.ShowVariantWalking = ShowVariantWalking;
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
            gs.OnlyUpgradeFavorites = OnlyUpgradeFavorites;
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
            gs.TransferWeakPokemon = TransferWeakPokemon;
            gs.TransferDuplicatePokemon = TransferDuplicatePokemon;
            gs.TransferDuplicatePokemonOnCapture = TransferDuplicatePokemonOnCapture;
            gs.KeepMinCp = KeepMinCp;
            gs.KeepMinIvPercentage = KeepMinIvPercentage;
            gs.KeepMinLvl = KeepMinLvl;
            gs.KeepMinOperator = KeepMinOperator;
            gs.UseKeepMinLvl = UseKeepMinLevel;
            gs.PrioritizeIvOverCp = PrioritizeIvOverCp;
            gs.KeepMinDuplicatePokemon = KeepMinDuplicatePokemon;
            // SNIPING
            gs.UseSnipeLocationServer = UseSnipeLocationServer;
            gs.SnipeLocationServer = SnipeLocationServer;
            gs.SnipeLocationServerPort = SnipeLocationServerPort;
            gs.GetSniperInfoFromPokezz = GetSniperInfoFromPokezz;
            gs.GetOnlyVerifiedSniperInfoFromPokezz = GetOnlyVerifiedSniperInfoFromPokezz;
            gs.GetSniperInfoFromPokeSnipers = GetSniperInfoFromPokeSnipers;
            gs.GetSniperInfoFromPokeWatchers = GetSniperInfoFromPokeWatchers;
            gs.GetSniperInfoFromSkiplagged = GetSniperInfoFromSkiplagged;
            gs.MinPokeballsToSnipe = MinPokeballsToSnipe;
            gs.MinPokeballsWhileSnipe = MinPokeballsWhileSnipe;
            gs.MinDelayBetweenSnipes = MinDelayBetweenSnipes;
            gs.SnipingScanOffset = SnipingScanOffset;
            gs.SnipeAtPokestops = SnipeAtPokestops;
            gs.SnipeIgnoreUnknownIv = SnipeIgnoreUnknownIv;
            gs.UseTransferIvForSnipe = UseTransferIvForSnipe;
            gs.SnipePokemonNotInPokedex = SnipePokemonNotInPokedex;
            // MISC
            gs.FastSoftBanBypass = FastSoftBanBypass;
            gs.UseGpxPathing = UseGpxPathing;
            gs.GpxFile = GpxFile;
            gs.UseWebsocket = UseWebsocket;
            gs.WebSocketPort = WebSocketPort;
            gs.UseTelegramAPI = UseTelegramAPI;
            gs.TelegramAPIKey = TelegramAPIKey;
            gs.TelegramPassword = TelegramPassword;
            gs.AmountOfPokemonToDisplayOnStart = AmountOfPokemonToDisplayOnStart;
            gs.DetailedCountsBeforeRecycling = DetailedCountsBeforeRecycling;
            gs.DumpPokemonStats = DumpPokemonStats;
            // OBJECTS & ITERATORS
            gs.PokemonToSnipe = PokemonToSnipe;
            gs.PokemonsNotToTransfer.Clear();
            foreach (PokemonToggle pt in NoTransferCollection) if (pt.IsChecked) gs.PokemonsNotToTransfer.Add(pt.Id);
            gs.PokemonsToEvolve.Clear();
            foreach (PokemonToggle pt in EvolveCollection) if (pt.IsChecked) gs.PokemonsToEvolve.Add(pt.Id);
            gs.PokemonsToLevelUp.Clear();
            foreach (PokemonToggle pt in UpgradeCollection) if (pt.IsChecked) gs.PokemonsToLevelUp.Add(pt.Id);
            gs.PokemonsToIgnore.Clear();
            foreach (PokemonToggle pt in IgnoreCollection) if (pt.IsChecked) gs.PokemonsToIgnore.Add(pt.Id);
            gs.PokemonToUseMasterball.Clear();
            foreach (PokemonToggle pt in MasterballCollection) if (pt.IsChecked) gs.PokemonToUseMasterball.Add(pt.Id);
            gs.PokemonsTransferFilter.Clear();
            foreach (PokemonId key in PokemonsTransferFilter.Keys)
                gs.PokemonsTransferFilter.Add(key, PokemonsTransferFilter[key]);
            foreach (KeyValuePair<ItemId, int> kvp in ItemRecycleFilter)
                gs.ItemRecycleFilter.Add(kvp);

            SaveCurrentCoords();

            return gs;
        }

    } // END class ObservableSettings

    public class PokemonToggle : DependencyObject
    {
        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }
        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register("Name", typeof(string), typeof(PokemonToggle), new PropertyMetadata(string.Empty));

        public PokemonId Id
        {
            get { return (PokemonId)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }
        public static readonly DependencyProperty IdProperty =
            DependencyProperty.Register("Id", typeof(PokemonId), typeof(PokemonToggle), new PropertyMetadata(PokemonId.Abra));

        public int Numeric
        {
            get { return (int)GetValue(NumericProperty); }
            set { SetValue(NumericProperty, value); }
        }
        public static readonly DependencyProperty NumericProperty =
            DependencyProperty.Register("Numeric", typeof(int), typeof(PokemonToggle), new PropertyMetadata(0));

        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }
        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register("IsChecked", typeof(bool), typeof(PokemonToggle), new PropertyMetadata(false));

        public PokemonToggle() { }
        public PokemonToggle(PokemonId id, bool isChecked)
        {
            Name = id.ToString();
            Id = id;
            Numeric = (int)id;
            IsChecked = isChecked;
        }

    }

}
