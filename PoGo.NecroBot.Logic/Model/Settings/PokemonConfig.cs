namespace PoGo.NecroBot.Logic.Model.Settings
{
    public class PokemonConfig
    {
        /*Catch*/
        public bool CatchPokemon = true;
        public int DelayBetweenPokemonCatch = 2000;
        /*CatchLimit*/
        public bool UseCatchLimit = true;
        public int CatchPokemonLimit = 998;
        public int CatchPokemonLimitMinutes = 60 * 24 + 30;
        /*Incense*/
        public bool UseIncenseConstantly;
        /*Egg*/
        public bool UseEggIncubators = true;
        public int UseEggIncubatorMinKm = 2;
        public bool UseLuckyEggConstantly;
        public int UseLuckyEggsMinPokemonAmount = 30;
        public bool UseLuckyEggsWhileEvolving;
        /*Berries*/
        public int UseBerriesMinCp = 1000;
        public float UseBerriesMinIv = 90;
        public double UseBerriesBelowCatchProbability = 0.20;
        public string UseBerriesOperator = "or";
        public int MaxBerriesToUsePerPokemon = 3;
        /*Transfer*/
        public bool TransferWeakPokemon;
        public bool TransferDuplicatePokemon = true;
        public bool TransferDuplicatePokemonOnCapture = true;
        /*Rename*/
        public bool RenamePokemon;
        public bool RenameOnlyAboveIv = true;
        public string RenameTemplate = "{1}_{0}";
        /*Favorite*/
        public float FavoriteMinIvPercentage = 95;
        public bool AutoFavoritePokemon;
        /*PokeBalls*/
        public int MaxPokeballsPerPokemon = 6;
        public int UseGreatBallAboveCp = 1000;
        public int UseUltraBallAboveCp = 1250;
        public int UseMasterBallAboveCp = 1500;
        public double UseGreatBallAboveIv = 85.0;
        public double UseUltraBallAboveIv = 95.0;
        public double UseGreatBallBelowCatchProbability = 0.2;
        public double UseUltraBallBelowCatchProbability = 0.1;
        public double UseMasterBallBelowCatchProbability = 0.05;
        /*PoweUp*/
        public bool AutomaticallyLevelUpPokemon;
        public bool OnlyUpgradeFavorites = true;
        public bool UseLevelUpList = true;
        public int AmountOfTimesToUpgradeLoop = 5;
        public int GetMinStarDustForLevelUp = 5000;
        public string LevelUpByCPorIv = "iv";
        public float UpgradePokemonCpMinimum = 1000;
        public float UpgradePokemonIvMinimum = 95;
        public string UpgradePokemonMinimumStatsOperator = "and";
        /*Evolve*/
        public float EvolveAboveIvValue = 95;
        public bool EvolveAllPokemonAboveIv;
        public bool EvolveAllPokemonWithEnoughCandy = true;
        public double EvolveKeptPokemonsAtStorageUsagePercentage = 90.0;
        /*Keep*/
        public bool KeepPokemonsThatCanEvolve;
        public int KeepMinCp = 1250;
        public float KeepMinIvPercentage = 90;
        public int KeepMinLvl = 6;
        public string KeepMinOperator = "or";
        public bool UseKeepMinLvl;
        public bool PrioritizeIvOverCp;
        public int KeepMinDuplicatePokemon = 1;
        /*NotCatch*/
        public bool UsePokemonToNotCatchFilter;
        public bool UsePokemonSniperFilterOnly;
        /*Dump Stats*/
        public bool DumpPokemonStats;
    }
}