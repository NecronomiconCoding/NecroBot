using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace PoGo.NecroBot.Logic.Model.Settings
{
    [JsonObject(Description = "")]
    public class PokeStopConfig
    {
        [DefaultValue(true)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool UsePokeStopLimit = true;

        [DefaultValue(1998)]
        [Range(0, 9999)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public int PokeStopLimit = 1998;

        [DefaultValue(60 * 24 + 30)]
        [Range(0, 9999)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public int PokeStopLimitMinutes = 60 * 24 + 30;
    }
}
