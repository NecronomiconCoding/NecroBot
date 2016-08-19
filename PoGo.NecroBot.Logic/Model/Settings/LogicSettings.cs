
#region using directives

using System.Collections.Generic;
using PoGo.NecroBot.Logic.Interfaces.Configuration;
using POGOProtos.Enums;
using POGOProtos.Inventory.Item;

#endregion

namespace PoGo.NecroBot.Logic.Model.Settings
{
    public class LogicSettings : ILogicSettings
    {
        private readonly GlobalSettings _settings;

        public LogicSettings(GlobalSettings settings)

        {
            _settings = settings;
        }

        public string ProfilePath => _settings.ProfilePath;
        public string ProfileConfigPath => _settings.ProfileConfigPath;
        public string GeneralConfigPath => _settings.GeneralConfigPath;
        public bool CheckForUpdates => _settings.UpdateSettings.CheckForUpdates;
        public bool AutoUpdate => _settings.UpdateSettings.AutoUpdate;
        public bool TransferConfigAndAuthOnUpdate => _settings.UpdateSettings.TransferConfigAndAuthOnUpdate;
        public bool UseWebsocket => _settings.WebsocketsSettings.UseWebsocket;
        public bool CatchPokemon => _settings.PokemonSettings.CatchPokemon;
        public bool TransferWeakPokemon => _settings.PokemonSettings.TransferWeakPokemon;
        public bool DisableHumanWalking => _settings.LocationSettings.DisableHumanWalking;
        public int MaxBerriesToUsePerPokemon => _settings.PokemonSettings.MaxBerriesToUsePerPokemon;
        public float KeepMinIvPercentage => _settings.PokemonSettings.KeepMinIvPercentage;
        public string KeepMinOperator => _settings.PokemonSettings.KeepMinOperator;
        public int KeepMinCp => _settings.PokemonSettings.KeepMinCp;
        public int KeepMinLvl => _settings.PokemonSettings.KeepMinLvl;
        public bool UseKeepMinLvl => _settings.PokemonSettings.UseKeepMinLvl;
        public bool AutomaticallyLevelUpPokemon => _settings.PokemonSettings.AutomaticallyLevelUpPokemon;
        public bool OnlyUpgradeFavorites => _settings.PokemonSettings.OnlyUpgradeFavorites;
        public bool UseLevelUpList => _settings.PokemonSettings.UseLevelUpList;
        public int AmountOfTimesToUpgradeLoop => _settings.PokemonSettings.AmountOfTimesToUpgradeLoop;
        public string LevelUpByCPorIv => _settings.PokemonSettings.LevelUpByCPorIv;
        public int GetMinStarDustForLevelUp => _settings.PokemonSettings.GetMinStarDustForLevelUp;
        public bool UseLuckyEggConstantly => _settings.PokemonSettings.UseLuckyEggConstantly;
        public bool UseIncenseConstantly => _settings.PokemonSettings.UseIncenseConstantly;
        public int UseBerriesMinCp => _settings.PokemonSettings.UseBerriesMinCp;
        public float UseBerriesMinIv => _settings.PokemonSettings.UseBerriesMinIv;
        public double UseBerriesBelowCatchProbability => _settings.PokemonSettings.UseBerriesBelowCatchProbability;
        public string UseBerriesOperator => _settings.PokemonSettings.UseBerriesOperator;
        public float UpgradePokemonIvMinimum => _settings.PokemonSettings.UpgradePokemonIvMinimum;
        public float UpgradePokemonCpMinimum => _settings.PokemonSettings.UpgradePokemonCpMinimum;
        public string UpgradePokemonMinimumStatsOperator => _settings.PokemonSettings.UpgradePokemonMinimumStatsOperator;
        public double WalkingSpeedInKilometerPerHour => _settings.LocationSettings.WalkingSpeedInKilometerPerHour;
        public bool UseWalkingSpeedVariant => _settings.LocationSettings.UseWalkingSpeedVariant;
        public double WalkingSpeedVariant => _settings.LocationSettings.WalkingSpeedVariant;
        public bool ShowVariantWalking => _settings.LocationSettings.ShowVariantWalking;
        public bool FastSoftBanBypass => _settings.SoftBanSettings.FastSoftBanBypass;
        public bool EvolveAllPokemonWithEnoughCandy => _settings.PokemonSettings.EvolveAllPokemonWithEnoughCandy;
        public bool KeepPokemonsThatCanEvolve => _settings.PokemonSettings.KeepPokemonsThatCanEvolve;
        public bool TransferDuplicatePokemon => _settings.PokemonSettings.TransferDuplicatePokemon;
        public bool TransferDuplicatePokemonOnCapture => _settings.PokemonSettings.TransferDuplicatePokemonOnCapture;
        public bool UseEggIncubators => _settings.PokemonSettings.UseEggIncubators;
        public int UseEggIncubatorMinKm => _settings.PokemonSettings.UseEggIncubatorMinKm;
        public int UseGreatBallAboveCp => _settings.PokemonSettings.UseGreatBallAboveCp;
        public int UseUltraBallAboveCp => _settings.PokemonSettings.UseUltraBallAboveCp;
        public int UseMasterBallAboveCp => _settings.PokemonSettings.UseMasterBallAboveCp;
        public double UseGreatBallAboveIv => _settings.PokemonSettings.UseGreatBallAboveIv;
        public double UseUltraBallAboveIv => _settings.PokemonSettings.UseUltraBallAboveIv;
        public double UseMasterBallBelowCatchProbability => _settings.PokemonSettings.UseMasterBallBelowCatchProbability;
        public double UseUltraBallBelowCatchProbability => _settings.PokemonSettings.UseUltraBallBelowCatchProbability;
        public double UseGreatBallBelowCatchProbability => _settings.PokemonSettings.UseGreatBallBelowCatchProbability;
        public bool EnableHumanizedThrows => _settings.CustomCatchSettings.EnableHumanizedThrows;
        public bool EnableMissedThrows => _settings.CustomCatchSettings.EnableMissedThrows;
        public int ThrowMissPercentage => _settings.CustomCatchSettings.ThrowMissPercentage;
        public int NiceThrowChance => _settings.CustomCatchSettings.NiceThrowChance;
        public int GreatThrowChance => _settings.CustomCatchSettings.GreatThrowChance;
        public int ExcellentThrowChance => _settings.CustomCatchSettings.ExcellentThrowChance;
        public int CurveThrowChance => _settings.CustomCatchSettings.CurveThrowChance;
        public double ForceGreatThrowOverIv => _settings.CustomCatchSettings.ForceGreatThrowOverIv;
        public double ForceExcellentThrowOverIv => _settings.CustomCatchSettings.ForceExcellentThrowOverIv;
        public int ForceGreatThrowOverCp => _settings.CustomCatchSettings.ForceGreatThrowOverCp;
        public int ForceExcellentThrowOverCp => _settings.CustomCatchSettings.ForceExcellentThrowOverCp;
        public int DelayBetweenPokemonCatch => _settings.PokemonSettings.DelayBetweenPokemonCatch;
        public int DelayBetweenPlayerActions => _settings.PlayerSettings.DelayBetweenPlayerActions;
        public bool UsePokemonToNotCatchFilter => _settings.PokemonSettings.UsePokemonToNotCatchFilter;
        public bool UsePokemonSniperFilterOnly => _settings.PokemonSettings.UsePokemonSniperFilterOnly;
        public int KeepMinDuplicatePokemon => _settings.PokemonSettings.KeepMinDuplicatePokemon;
        public bool PrioritizeIvOverCp => _settings.PokemonSettings.PrioritizeIvOverCp;
        public int MaxTravelDistanceInMeters => _settings.LocationSettings.MaxTravelDistanceInMeters;
        public string GpxFile => _settings.GPXSettings.GpxFile;
        public bool UseGpxPathing => _settings.GPXSettings.UseGpxPathing;
        public bool UseLuckyEggsWhileEvolving => _settings.PokemonSettings.UseLuckyEggsWhileEvolving;
        public int UseLuckyEggsMinPokemonAmount => _settings.PokemonSettings.UseLuckyEggsMinPokemonAmount;
        public bool EvolveAllPokemonAboveIv => _settings.PokemonSettings.EvolveAllPokemonAboveIv;
        public float EvolveAboveIvValue => _settings.PokemonSettings.EvolveAboveIvValue;
        public bool RenamePokemon => _settings.PokemonSettings.RenamePokemon;
        public bool RenameOnlyAboveIv => _settings.PokemonSettings.RenameOnlyAboveIv;
        public float FavoriteMinIvPercentage => _settings.PokemonSettings.FavoriteMinIvPercentage;
        public bool AutoFavoritePokemon => _settings.PokemonSettings.AutoFavoritePokemon;
        public string RenameTemplate => _settings.PokemonSettings.RenameTemplate;
        public int AmountOfPokemonToDisplayOnStart => _settings.ConsoleSettings.AmountOfPokemonToDisplayOnStart;
        public bool DumpPokemonStats => _settings.PokemonSettings.DumpPokemonStats;
        public string TranslationLanguageCode => _settings.ConsoleSettings.TranslationLanguageCode;
        public bool DetailedCountsBeforeRecycling => _settings.ConsoleSettings.DetailedCountsBeforeRecycling;
        public bool VerboseRecycling => _settings.RecycleSettings.VerboseRecycling;
        public double RecycleInventoryAtUsagePercentage => _settings.RecycleSettings.RecycleInventoryAtUsagePercentage;
        public double EvolveKeptPokemonsAtStorageUsagePercentage => _settings.PokemonSettings.EvolveKeptPokemonsAtStorageUsagePercentage;
        public bool UseKillSwitchCatch => _settings.SoftBanSettings.UseKillSwitchCatch;
        public int CatchErrorPerHours => _settings.SoftBanSettings.CatchErrorPerHours;
        public int CatchEscapePerHours => _settings.SoftBanSettings.CatchEscapePerHours;
        public int CatchFleePerHours => _settings.SoftBanSettings.CatchFleePerHours;
        public int CatchMissedPerHours => _settings.SoftBanSettings.CatchMissedPerHours;
        public int CatchSuccessPerHours => _settings.SoftBanSettings.CatchSuccessPerHours;
        public bool UseKillSwitchPokestops => _settings.SoftBanSettings.UseKillSwitchPokestops;
        public int AmountPokestops => _settings.SoftBanSettings.AmountPokestops;
        public ICollection<KeyValuePair<ItemId, int>> ItemRecycleFilter => _settings.ItemRecycleFilter;
        public ICollection<PokemonId> PokemonsToEvolve => _settings.PokemonsToEvolve;
        public ICollection<PokemonId> PokemonsToLevelUp => _settings.PokemonsToLevelUp;
        public ICollection<PokemonId> PokemonsNotToTransfer => _settings.PokemonsNotToTransfer;
        public ICollection<PokemonId> PokemonsNotToCatch => _settings.PokemonsToIgnore;

        public ICollection<PokemonId> PokemonToUseMasterball => _settings.PokemonToUseMasterball;
        public Dictionary<PokemonId, TransferFilter> PokemonsTransferFilter => _settings.PokemonsTransferFilter;
        public bool StartupWelcomeDelay => _settings.ConsoleSettings.StartupWelcomeDelay;
        public bool UseGoogleWalk => _settings.GoogleWalkConfig.UseGoogleWalk;
        public bool UseGoogleWalkCache => _settings.GoogleWalkConfig.Cache;
        public string GoogleApiKey => _settings.GoogleWalkConfig.GoogleAPIKey;
        public string GoogleHeuristic => _settings.GoogleWalkConfig.GoogleHeuristic;
        public bool SnipeAtPokestops => _settings.SnipingSettings.SnipeAtPokestops;

        public bool UseTelegramAPI => _settings.TelegramSettings.UseTelegramAPI;
        public string TelegramAPIKey => _settings.TelegramSettings.TelegramAPIKey;
        public string TelegramPassword => _settings.TelegramSettings.TelegramPassword;
        public int MinPokeballsToSnipe => _settings.SnipingSettings.MinPokeballsToSnipe;
        public int MinPokeballsWhileSnipe => _settings.SnipingSettings.MinPokeballsWhileSnipe;
        public int MaxPokeballsPerPokemon => _settings.PokemonSettings.MaxPokeballsPerPokemon;
        public bool RandomlyPauseAtStops => _settings.LocationSettings.RandomlyPauseAtStops;
        public SnipeSettings PokemonToSnipe => _settings.PokemonToSnipe;
        public string SnipeLocationServer => _settings.SnipingSettings.SnipeLocationServer;
        public int SnipeLocationServerPort => _settings.SnipingSettings.SnipeLocationServerPort;
        public bool GetSniperInfoFromPokezz => _settings.SnipingSettings.GetSniperInfoFromPokezz;
        public bool GetOnlyVerifiedSniperInfoFromPokezz => _settings.SnipingSettings.GetOnlyVerifiedSniperInfoFromPokezz;
        public bool GetSniperInfoFromPokeSnipers => _settings.SnipingSettings.GetSniperInfoFromPokeSnipers;
        public bool GetSniperInfoFromPokeWatchers => _settings.SnipingSettings.GetSniperInfoFromPokeWatchers;
        public bool GetSniperInfoFromSkiplagged => _settings.SnipingSettings.GetSniperInfoFromSkiplagged;
        public bool UseSnipeLocationServer => _settings.SnipingSettings.UseSnipeLocationServer;
        public bool UseTransferIvForSnipe => _settings.SnipingSettings.UseTransferIvForSnipe;
        public bool SnipeIgnoreUnknownIv => _settings.SnipingSettings.SnipeIgnoreUnknownIv;
        public int MinDelayBetweenSnipes => _settings.SnipingSettings.MinDelayBetweenSnipes;
        public double SnipingScanOffset => _settings.SnipingSettings.SnipingScanOffset;
        public bool SnipePokemonNotInPokedex => _settings.SnipingSettings.SnipePokemonNotInPokedex;
        public bool RandomizeRecycle => _settings.RecycleSettings.RandomizeRecycle;
        public int RandomRecycleValue => _settings.RecycleSettings.RandomRecycleValue;
        public bool DelayBetweenRecycleActions => _settings.RecycleSettings.DelayBetweenRecycleActions;
        public int TotalAmountOfPokeballsToKeep => _settings.RecycleSettings.TotalAmountOfPokeballsToKeep;
        public int TotalAmountOfPotionsToKeep => _settings.RecycleSettings.TotalAmountOfPotionsToKeep;
        public int TotalAmountOfRevivesToKeep => _settings.RecycleSettings.TotalAmountOfRevivesToKeep;
        public int TotalAmountOfBerriesToKeep => _settings.RecycleSettings.TotalAmountOfBerriesToKeep;
    }
}
