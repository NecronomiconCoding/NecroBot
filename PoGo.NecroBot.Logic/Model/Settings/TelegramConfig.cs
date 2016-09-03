using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace PoGo.NecroBot.Logic.Model.Settings
{
    [JsonObject(Title = "Telegram Messaging Client", Description = "Configure to use with Telegram Messaging.", ItemRequired = Required.DisallowNull)]
    public class TelegramConfig
    {
        [DefaultValue(false)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 1)]
        public bool UseTelegramAPI;

        [DefaultValue(null)]
        [MinLength(0)]
        [MaxLength(64)]
        [JsonProperty(Required = Required.Default, DefaultValueHandling = DefaultValueHandling.Populate, Order = 2)]
        public string TelegramAPIKey = null;

        [DefaultValue(null)]
        [MinLength(0)]
        [MaxLength(32)]
        [JsonProperty(Required = Required.Default, DefaultValueHandling = DefaultValueHandling.Populate, Order = 3)]
        public string TelegramPassword = null;
    }
}