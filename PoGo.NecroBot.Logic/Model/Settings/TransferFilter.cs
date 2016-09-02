using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using POGOProtos.Enums;

namespace PoGo.NecroBot.Logic.Model.Settings
{
    [JsonObject(Description = "", ItemRequired = Required.DisallowNull)] //Dont set Title
    public class TransferFilter
    {
        internal enum Operator
        {
            or,
            and
        }

        public TransferFilter()
        {
        }

        public TransferFilter(int keepMinCp, int keepMinLvl, bool useKeepMinLvl, float keepMinIvPercentage, string keepMinOperator, int keepMinDuplicatePokemon,
            List<List<PokemonMove>> moves = null, List<PokemonMove> deprecatedMoves = null, string movesOperator = "or")
        {
            KeepMinCp = keepMinCp;
            KeepMinLvl = keepMinLvl;
            UseKeepMinLvl = useKeepMinLvl;
            KeepMinIvPercentage = keepMinIvPercentage;
            KeepMinDuplicatePokemon = keepMinDuplicatePokemon;
            KeepMinOperator = keepMinOperator;
            Moves = (moves == null && deprecatedMoves != null)
                ? new List<List<PokemonMove>> { deprecatedMoves }
                : moves ?? new List<List<PokemonMove>>();
            MovesOperator = movesOperator;
        }

        [DefaultValue(1250)]
        [Range(0, 9999)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 1)]
        public int KeepMinCp { get; set; }

        [DefaultValue(6)]
        [Range(0, 99)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 2)]
        public int KeepMinLvl { get; set; }

        [DefaultValue(false)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 3)]
        public bool UseKeepMinLvl { get; set; }

        [DefaultValue(90)]
        [Range(0, 100)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 4)]
        public float KeepMinIvPercentage { get; set; }

        [DefaultValue(1)]
        [Range(0, 999)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 5)]
        public int KeepMinDuplicatePokemon { get; set; }

        [DefaultValue(null)]
        [JsonProperty(Required = Required.Default, DefaultValueHandling = DefaultValueHandling.Populate, Order = 6)]
        public List<List<PokemonMove>> Moves { get; set; }

        [DefaultValue(null)]
        [JsonProperty(Required = Required.Default, DefaultValueHandling = DefaultValueHandling.Populate, Order = 7)]
        public List<PokemonMove> DeprecatedMoves { get; set; }

        [DefaultValue("or")]
        [EnumDataType(typeof(Operator))]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 8)]
        public string KeepMinOperator { get; set; }

        [DefaultValue("and")]
        [EnumDataType(typeof(Operator))]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 9)]
        public string MovesOperator { get; set; }
    }
}