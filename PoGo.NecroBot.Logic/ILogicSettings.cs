#region using directives

using POGOProtos.Enums;
using POGOProtos.Inventory.Item;
using System.Collections.Generic;

#endregion

namespace PoGo.NecroBot.Logic
{
    public class Location
    {
        public Location()
        {
        }

        public Location(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class SnipeSettings
    {
        public SnipeSettings()
        {
        }

        public SnipeSettings(List<Location> locations, List<PokemonId> pokemon)
        {
            Locations = locations;
            Pokemon = pokemon;
        }

        public List<Location> Locations { get; set; }
        public List<PokemonId> Pokemon { get; set; }
    }

    public class TransferFilter
    {
        public TransferFilter()
        {
        }

        public TransferFilter(int keepMinCp, int keepMinLvl, bool useKeepMinLvl, float keepMinIvPercentage, string keepMinOperator, int keepMinDuplicatePokemon, 
            List<PokemonMove> moves = null, string movesOperator = "or")
        {
            KeepMinCp = keepMinCp;
            KeepMinLvl = keepMinLvl;
            UseKeepMinLvl = useKeepMinLvl;
            KeepMinIvPercentage = keepMinIvPercentage;
            KeepMinDuplicatePokemon = keepMinDuplicatePokemon;
            KeepMinOperator = keepMinOperator;
            Moves = moves ?? new List<PokemonMove>();
            MovesOperator = movesOperator;
        }

        public int KeepMinCp { get; set; }
        public int KeepMinLvl { get; set; }
        public bool UseKeepMinLvl { get; set; }
        public float KeepMinIvPercentage { get; set; }
        public int KeepMinDuplicatePokemon { get; set; }
        public List<PokemonMove> Moves { get; set; }
        public string KeepMinOperator { get; set; }
        public string MovesOperator { get; set; }
    }

    public interface ILogicSettings
    {
        bool UseWebsocket { get; }
        bool CatchPokemon { get; }
        bool TransferWeakPokemon { get; }
        bool DisableHumanWalking { get; }
        bool AutoUpdate { get; }
        bool TransferConfigAndAuthOnUpdate { get; }
        float KeepMinIvPercentage { get; }
        int KeepMinCp { get; }
        int KeepMinLvl { get; }
        bool UseKeepMinLvl { get; }
        string KeepMinOperator { get; }
        double WalkingSpeedInKilometerPerHour { get; }
        bool EvolveAllPokemonWithEnoughCandy { get; }
        bool KeepPokemonsThatCanEvolve { get; }
        bool TransferDuplicatePokemon { get; }
        bool TransferDuplicatePokemonOnCapture { get; }
        bool UseEggIncubators { get; }
        int UseGreatBallAboveCp { get; }
        int UseUltraBallAboveCp { get; }
        int UseMasterBallAboveCp { get; }
        double UseGreatBallAboveIv { get; }
        double UseUltraBallAboveIv { get; }
        double UseMasterBallBelowCatchProbability { get; }
        double UseUltraBallBelowCatchProbability { get; }
        double UseGreatBallBelowCatchProbability { get; }
        bool EnableHumanizedThrows { get; }
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
        int MinPokeballsToSnipe { get; }
        int MinPokeballsWhileSnipe { get; }
        int MaxPokeballsPerPokemon { get; }
        string SnipeLocationServer { get; }
        int SnipeLocationServerPort { get; }
        bool GetSniperInfoFromPokezz { get; }
        bool GetOnlyVerifiedSniperInfoFromPokezz { get; }
        bool UseSnipeLocationServer { get; }
        bool UseSnipeOnlineLocationServer { get; }
        bool UseTransferIvForSnipe { get; }
        bool SnipeIgnoreUnknownIv { get; }
        int MinDelayBetweenSnipes { get; }
        double SnipingScanOffset { get; }
        int TotalAmountOfPokeballsToKeep { get; }
        int TotalAmountOfPotionsToKeep { get; }
        int TotalAmountOfRevivesToKeep { get; }
        int TotalAmountOfBerriesToKeep { get; }

        bool ShowPokeballCountsBeforeRecycle { get; }
        bool VerboseRecycling { get; }
        double RecycleInventoryAtUsagePercentage { get; }
        double EvolveKeptPokemonsAtStorageUsagePercentage { get; }
        ICollection<KeyValuePair<ItemId, int>> ItemRecycleFilter { get; }

        ICollection<PokemonId> PokemonsToEvolve { get; }

        ICollection<PokemonId> PokemonsNotToTransfer { get; }

        ICollection<PokemonId> PokemonsNotToCatch { get; }

        ICollection<PokemonId> PokemonToUseMasterball { get; }

        Dictionary<PokemonId, TransferFilter> PokemonsTransferFilter { get; }
        SnipeSettings PokemonToSnipe { get; }

        bool StartupWelcomeDelay { get; }
    }
}