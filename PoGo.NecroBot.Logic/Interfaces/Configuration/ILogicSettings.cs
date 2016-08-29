#region using directives

using System.Collections.Generic;
using PoGo.NecroBot.Logic.Model.Settings;
using POGOProtos.Enums;
using POGOProtos.Inventory.Item;

#endregion

namespace PoGo.NecroBot.Logic.Interfaces.Configuration
{
    public interface ILogicSettings
    {
        bool UseWebsocket { get; }
        bool CatchPokemon { get; }
        int CatchPokemonLimit { get; }
        int CatchPokemonLimitMinutes { get; }
        int PokeStopLimit { get; }
        int PokeStopLimitMinutes { get; }
        int SnipeCountLimit { get; }
        int SnipeRestSeconds { get; }
        bool TransferWeakPokemon { get; }
        bool DisableHumanWalking { get; }
        bool CheckForUpdates { get; }
        bool AutoUpdate { get; }
        bool TransferConfigAndAuthOnUpdate { get; }
        float KeepMinIvPercentage { get; }
        int KeepMinCp { get; }
        int KeepMinLvl { get; }
        bool UseKeepMinLvl { get; }
        string KeepMinOperator { get; }
        double WalkingSpeedInKilometerPerHour { get; }
        bool UseWalkingSpeedVariant { get; }
        double WalkingSpeedVariant { get; }
        bool ShowVariantWalking { get; }
        bool RandomlyPauseAtStops { get; }
        bool FastSoftBanBypass { get; }
        bool EvolveAllPokemonWithEnoughCandy { get; }
        bool KeepPokemonsThatCanEvolve { get; }
        bool TransferDuplicatePokemon { get; }
        bool TransferDuplicatePokemonOnCapture { get; }
        bool UseEggIncubators { get; }
        bool UseLimitedEggIncubators { get; }
        int UseGreatBallAboveCp { get; }
        int UseUltraBallAboveCp { get; }
        int UseMasterBallAboveCp { get; }
        double UseGreatBallAboveIv { get; }
        double UseUltraBallAboveIv { get; }
        double UseMasterBallBelowCatchProbability { get; }
        double UseUltraBallBelowCatchProbability { get; }
        double UseGreatBallBelowCatchProbability { get; }
        bool EnableHumanizedThrows { get; }
        bool EnableMissedThrows { get; }
        int ThrowMissPercentage { get; }
        int NiceThrowChance { get; }
        int GreatThrowChance { get; }
        int ExcellentThrowChance { get; }
        int CurveThrowChance { get; }
        double ForceGreatThrowOverIv { get; }
        double ForceExcellentThrowOverIv { get; }
        int ForceGreatThrowOverCp { get; }
        int ForceExcellentThrowOverCp { get; }
        int DelayBetweenPokemonCatch { get; }
        bool AutomaticallyLevelUpPokemon { get; }
        bool OnlyUpgradeFavorites { get; }
        bool UseLevelUpList { get; }
        string LevelUpByCPorIv { get; }
        float UpgradePokemonCpMinimum { get; }
        float UpgradePokemonIvMinimum { get; }
        int DelayBetweenPlayerActions { get; }
        bool UsePokemonToNotCatchFilter { get; }
        bool UsePokemonSniperFilterOnly { get; }
        int KeepMinDuplicatePokemon { get; }
        bool PrioritizeIvOverCp { get; }
        int AmountOfTimesToUpgradeLoop { get; }
        int GetMinStarDustForLevelUp { get; }
        bool UseLuckyEggConstantly { get; }
        int MaxBerriesToUsePerPokemon { get; }
        bool UseIncenseConstantly { get; }
        int UseBerriesMinCp { get; }
        float UseBerriesMinIv { get; }
        double UseBerriesBelowCatchProbability { get; }
        string UseBerriesOperator { get; }
        string UpgradePokemonMinimumStatsOperator { get; }
        int MaxTravelDistanceInMeters { get; }
        bool UseGpxPathing { get; }
        string GpxFile { get; }
        bool UseLuckyEggsWhileEvolving { get; }
        int UseLuckyEggsMinPokemonAmount { get; }
        bool EvolveAllPokemonAboveIv { get; }
        float EvolveAboveIvValue { get; }
        bool DumpPokemonStats { get; }
        bool RenamePokemon { get; }
        bool RenameOnlyAboveIv { get; }
        float FavoriteMinIvPercentage { get; }
        bool AutoFavoritePokemon { get; }
        string RenameTemplate { get; }
        int AmountOfPokemonToDisplayOnStart { get; }
        string TranslationLanguageCode { get; }
        string ProfilePath { get; }
        string ProfileConfigPath { get; }
        string GeneralConfigPath { get; }
        bool SnipeAtPokestops { get; }
        bool UseTelegramAPI { get; }
        string TelegramAPIKey { get; }
        string TelegramPassword { get; }
        int MinPokeballsToSnipe { get; }
        int MinPokeballsWhileSnipe { get; }
        int MaxPokeballsPerPokemon { get; }
        string SnipeLocationServer { get; }
        int SnipeLocationServerPort { get; }
        bool GetSniperInfoFromPokezz { get; }
        bool GetOnlyVerifiedSniperInfoFromPokezz { get; }
        bool GetSniperInfoFromPokeSnipers { get; }
        bool GetSniperInfoFromPokeWatchers { get; }
        bool GetSniperInfoFromSkiplagged { get; }
        bool UseSnipeLocationServer { get; }
        bool UseTransferIvForSnipe { get; }
        bool SnipeIgnoreUnknownIv { get; }
        int MinDelayBetweenSnipes { get; }
        double SnipingScanOffset { get; }
        bool SnipePokemonNotInPokedex { get; }
        bool RandomizeRecycle { get; }
        int RandomRecycleValue { get; }
        bool DelayBetweenRecycleActions { get; }
        int TotalAmountOfPokeballsToKeep { get; }
        int TotalAmountOfPotionsToKeep { get; }
        int TotalAmountOfRevivesToKeep { get; }
        int TotalAmountOfBerriesToKeep { get; }
        bool DetailedCountsBeforeRecycling { get; }
        bool VerboseRecycling { get; }
        double RecycleInventoryAtUsagePercentage { get; }
        double EvolveKeptPokemonsAtStorageUsagePercentage { get; }
        bool UseSnipeLimit { get; }
        bool UsePokeStopLimit { get; }
        bool UseCatchLimit { get; }
        bool UseNearActionRandom { get; }
        ICollection<KeyValuePair<ItemId, int>> ItemRecycleFilter { get; }

        ICollection<PokemonId> PokemonsToEvolve { get; }
        ICollection<PokemonId> PokemonsToLevelUp { get; }

        ICollection<PokemonId> PokemonsNotToTransfer { get; }

        ICollection<PokemonId> PokemonsNotToCatch { get; }

        ICollection<PokemonId> PokemonToUseMasterball { get; }

        Dictionary<PokemonId, TransferFilter> PokemonsTransferFilter { get; }
        SnipeSettings PokemonToSnipe { get; }

        bool StartupWelcomeDelay { get; }
        bool UseGoogleWalk { get; }
        double DefaultStepLength { get; }
        bool UseGoogleWalkCache { get; }
        string GoogleApiKey { get; }
        string GoogleHeuristic { get; }

        bool UseYoursWalk { get; }
        string YoursWalkHeuristic { get; }

        int ResumeTrack { get; }
        int ResumeTrackSeg { get; }
        int ResumeTrackPt { get; }

        bool EnableHumanWalkingSnipe { get; }
        bool HumanWalkingSnipeDisplayList { get; }
        bool HumanWalkingSnipeSpinWhileWalking { get; }
        double HumanWalkingSnipeMaxDistance { get; }
        bool HumanWalkingSnipeAlwaysWalkBack { get; }
        double HumanWalkingSnipeMaxEstimateTime { get; }
        bool HumanWalkingSnipeTryCatchEmAll { get; }
        int HumanWalkingSnipeCatchEmAllMinBalls { get; }
        bool HumanWalkingSnipeCatchPokemonWhileWalking { get; }
        double HumanWalkingSnipeSnipingScanOffset { get; }
        double HumanWalkingSnipeWalkbackDistanceLimit { get; }
        bool HumanWalkingSnipeIncludeDefaultLocation { get; }
        bool HumanWalkingSnipeUsePokeRadar { get; }
        bool HumanWalkingSnipeUseSkiplagged { get; }
        bool HumanWalkingSnipeUseSnipePokemonList { get; }
        Dictionary<PokemonId, HumanWalkSnipeFilter> HumanWalkSnipeFilters { get; }
        double HumanWalkingSnipeMaxSpeedUpSpeed { get; }
        int HumanWalkingSnipeDelayTimeAtDestination { get; }
        bool HumanWalkingSnipeAllowSpeedUp { get; }
    }
}
