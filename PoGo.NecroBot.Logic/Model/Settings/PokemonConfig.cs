using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace PoGo.NecroBot.Logic.Model.Settings
{
    [JsonObject(Title = "Pokemon Config", Description = "Set your pokemon settings.", ItemRequired = Required.DisallowNull)]
    public class PokemonConfig
    {
        internal enum Operator
        {
            or,
            and
        }

        internal enum CpIv
        {
            cp,
            iv
        }

        /*Catch*/
        [DefaultValue(true)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 1)]
        public bool CatchPokemon = true;

        [DefaultValue(2000)]
        [Range(0, 99999)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 2)]
        public int DelayBetweenPokemonCatch = 2000;
        
        /*CatchLimit*/
        [DefaultValue(true)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 3)]
        public bool UseCatchLimit = true;

        [DefaultValue(998)]
        [Range(0, 9999)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 4)]
        public int CatchPokemonLimit = 998;

        [DefaultValue(60 * 24 + 30)]
        [Range(0, 9999)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 5)]
        public int CatchPokemonLimitMinutes = 60 * 24 + 30;

        /*Incense*/
        [DefaultValue(false)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 6)]
        public bool UseIncenseConstantly;

        /*Egg*/
        [DefaultValue(true)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 7)]
        public bool UseEggIncubators = true;

        [DefaultValue(true)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 8)]
        public bool UseLimitedEggIncubators = true;

        [DefaultValue(false)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 9)]
        public bool UseLuckyEggConstantly;

        [DefaultValue(30)]
        [Range(0, 999)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 10)]
        public int UseLuckyEggsMinPokemonAmount = 30;

        [DefaultValue(false)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 11)]
        public bool UseLuckyEggsWhileEvolving;

        /*Berries*/
        [DefaultValue(1000)]
        [Range(0, 9999)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 12)]
        public int UseBerriesMinCp = 1000;

        [DefaultValue(90)]
        [Range(0, 100)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 13)]
        public float UseBerriesMinIv = 90;

        [DefaultValue(0.20)]
        [Range(0, 1)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 14)]
        public double UseBerriesBelowCatchProbability = 0.20;

        [DefaultValue("or")]
        [EnumDataType(typeof(Operator))]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 15)]
        public string UseBerriesOperator = "or";

        [DefaultValue(30)]
        [Range(0, 999)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 16)]
        public int MaxBerriesToUsePerPokemon = 1;

        /*Transfer*/
        [DefaultValue(false)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 17)]
        public bool TransferWeakPokemon;

        [DefaultValue(true)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 18)]
        public bool TransferDuplicatePokemon = true;

        [DefaultValue(true)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 19)]
        public bool TransferDuplicatePokemonOnCapture = true;

        /*Rename*/
        [DefaultValue(false)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 20)]
        public bool RenamePokemon;

        [DefaultValue(false)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 21)]
        public bool RenameOnlyAboveIv;

        [DefaultValue("{1}_{0}")]
        [MinLength(0)]
        [MaxLength(32)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 22)]
        public string RenameTemplate = "{1}_{0}";

        /*Favorite*/
        [DefaultValue(95)]
        [Range(0, 100)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 23)]
        public float FavoriteMinIvPercentage = 95;

        [DefaultValue(false)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 24)]
        public bool AutoFavoritePokemon;

        /*PokeBalls*/
        [DefaultValue(6)]
        [Range(0, 999)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 25)]
        public int MaxPokeballsPerPokemon = 6;

        [DefaultValue(1000)]
        [Range(0, 9999)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 26)]
        public int UseGreatBallAboveCp = 1000;

        [DefaultValue(1250)]
        [Range(0, 9999)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 27)]
        public int UseUltraBallAboveCp = 1250;

        [DefaultValue(1500)]
        [Range(0, 9999)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 28)]
        public int UseMasterBallAboveCp = 1500;

        [DefaultValue(85.0)]
        [Range(0, 100)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 29)]
        public double UseGreatBallAboveIv = 85.0;

        [DefaultValue(95.0)]
        [Range(0, 100)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 30)]
        public double UseUltraBallAboveIv = 95.0;

        [DefaultValue(0.2)]
        [Range(0, 1)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 31)]
        public double UseGreatBallBelowCatchProbability = 0.2;

        [DefaultValue(0.1)]
        [Range(0, 1)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 32)]
        public double UseUltraBallBelowCatchProbability = 0.1;

        [DefaultValue(0.05)]
        [Range(0, 999)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 33)]
        public double UseMasterBallBelowCatchProbability = 0.05;

        /*PoweUp*/
        [DefaultValue(false)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 34)]
        public bool AutomaticallyLevelUpPokemon;

        [DefaultValue(true)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 35)]
        public bool OnlyUpgradeFavorites = true;

        [DefaultValue(true)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 36)]
        public bool UseLevelUpList = true;

        [DefaultValue(5)]
        [Range(0, 99)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 37)]
        public int AmountOfTimesToUpgradeLoop = 5;

        [DefaultValue(5000)]
        [Range(0, 999999)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 38)]
        public int GetMinStarDustForLevelUp = 5000;

        [DefaultValue("iv")]
        [EnumDataType(typeof(CpIv))]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 39)]
        public string LevelUpByCPorIv = "iv";

        [DefaultValue(1000)]
        [Range(0, 9999)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 40)]
        public float UpgradePokemonCpMinimum = 1000;

        [DefaultValue(95)]
        [Range(0, 100)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 41)]
        public float UpgradePokemonIvMinimum = 95;

        [DefaultValue("and")]
        [EnumDataType(typeof(Operator))]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 42)]
        public string UpgradePokemonMinimumStatsOperator = "and";

        /*Evolve*/
        [DefaultValue(95)]
        [Range(0, 100)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 43)]
        public float EvolveAboveIvValue = 95;

        [DefaultValue(false)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 44)]
        public bool EvolveAllPokemonAboveIv;

        [DefaultValue(true)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 45)]
        public bool EvolveAllPokemonWithEnoughCandy = true;

        [DefaultValue(90)]
        [Range(0, 100)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 46)]
        public double EvolveKeptPokemonsAtStorageUsagePercentage = 90.0;

        /*Keep*/
        [DefaultValue(false)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 47)]
        public bool KeepPokemonsThatCanEvolve;

        [DefaultValue(1250)]
        [Range(0, 9999)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 48)]
        public int KeepMinCp = 1250;

        [DefaultValue(90)]
        [Range(0, 100)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 49)]
        public float KeepMinIvPercentage = 90;

        [DefaultValue(6)]
        [Range(0, 100)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 50)]
        public int KeepMinLvl = 6;

        [DefaultValue("or")]
        [EnumDataType(typeof(Operator))]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 51)]
        public string KeepMinOperator = "or";

        [DefaultValue(false)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 52)]
        public bool UseKeepMinLvl;

        [DefaultValue(false)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 53)]
        public bool PrioritizeIvOverCp;

        [DefaultValue(1)]
        [Range(0, 999)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 54)]
        public int KeepMinDuplicatePokemon = 1;

        /*NotCatch*/
        [DefaultValue(false)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 55)]
        public bool UsePokemonToNotCatchFilter;

        [DefaultValue(false)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 56)]
        public bool UsePokemonSniperFilterOnly;

        /*Dump Stats*/
        [DefaultValue(false)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 57)]
        public bool DumpPokemonStats;
    }
}