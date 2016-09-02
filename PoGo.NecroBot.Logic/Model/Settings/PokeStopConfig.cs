using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace PoGo.NecroBot.Logic.Model.Settings
{
    [JsonObject(Title = "Poke Stop Config", Description = "Set your poke stop settings.", ItemRequired = Required.DisallowNull)]
    public class PokeStopConfig
    {
        [DefaultValue(true)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 1)]
        public bool UsePokeStopLimit = true;

        [DefaultValue(1998)]
        [Range(0, 9999)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 2)]
        public int PokeStopLimit = 1998;

        [DefaultValue(60 * 24 + 30)]
        [Range(0, 9999)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 3)]
        public int PokeStopLimitMinutes = 60 * 24 + 30;
    }
}
