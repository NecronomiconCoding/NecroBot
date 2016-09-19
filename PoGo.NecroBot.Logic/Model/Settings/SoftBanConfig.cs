using System.ComponentModel;
using Newtonsoft.Json;

namespace PoGo.NecroBot.Logic.Model.Settings
{
    [JsonObject(Title = "Soft Ban Config", Description = "Set your soft ban settings.", ItemRequired = Required.DisallowNull)]
    public class SoftBanConfig
    {
        [DefaultValue(false)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 1)]
        public bool FastSoftBanBypass;
    }
}