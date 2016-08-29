using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace PoGo.NecroBot.Logic.Model.Settings
{
    [JsonObject(Description = "")]
    public class TelegramConfig
    {
        [DefaultValue(false)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool UseTelegramAPI;

        [DefaultValue(null)]
        [MinLength(0)]
        [MaxLength(32)]
        [JsonProperty(Required = Required.Default, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string TelegramAPIKey;

        [DefaultValue(null)]
        [MinLength(0)]
        [MaxLength(32)]
        [JsonProperty(Required = Required.Default, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string TelegramPassword;
    }
}