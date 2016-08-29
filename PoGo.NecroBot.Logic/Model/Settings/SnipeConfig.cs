using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace PoGo.NecroBot.Logic.Model.Settings
{
    [JsonObject(Description = "")]
    public class SnipeConfig
    {
        [DefaultValue(false)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool UseSnipeLocationServer;

        [DefaultValue("localhost")]
        [MinLength(0)]
        [MaxLength(32)]
        //[RegularExpression(@"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$")] //Ip Only
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string SnipeLocationServer = "localhost";

        [DefaultValue(16969)]
        [Range(1, 65535)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public int SnipeLocationServerPort = 16969;

        [DefaultValue(false)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool GetSniperInfoFromPokezz;

        [DefaultValue(true)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool GetOnlyVerifiedSniperInfoFromPokezz = true;

        [DefaultValue(true)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool GetSniperInfoFromPokeSnipers = true;

        [DefaultValue(true)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool GetSniperInfoFromPokeWatchers = true;

        [DefaultValue(true)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool GetSniperInfoFromSkiplagged = true;

        [DefaultValue(20)]
        [Range(0, 999)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public int MinPokeballsToSnipe = 20;

        [DefaultValue(0)]
        [Range(0, 999)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public int MinPokeballsWhileSnipe = 0;

        [DefaultValue(60000)]
        [Range(0, 999999)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public int MinDelayBetweenSnipes = 60000;

        [DefaultValue(0.005)]
        [Range(0, 1)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public double SnipingScanOffset = 0.005;

        [DefaultValue(false)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool SnipeAtPokestops;

        [DefaultValue(false)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool SnipeIgnoreUnknownIv;

        [DefaultValue(false)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool UseTransferIvForSnipe;

        [DefaultValue(false)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool SnipePokemonNotInPokedex;

        /*SnipeLimit*/
        [DefaultValue(true)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool UseSnipeLimit = true;

        [DefaultValue(10 * 60)]
        [Range(0, 9999)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public int SnipeRestSeconds = 10 * 60;

        [DefaultValue(39)]
        [Range(0, 999)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public int SnipeCountLimit = 39;
    }
}