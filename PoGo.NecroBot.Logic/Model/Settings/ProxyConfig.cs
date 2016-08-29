using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace PoGo.NecroBot.Logic.Model.Settings
{
    [JsonObject(Description = "")]
    public class ProxyConfig
    {
        [DefaultValue(false)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool UseProxy;

        [DefaultValue(null)]
        [MinLength(0)]
        [MaxLength(32)]
        //[RegularExpression(@"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$")] //Ip Only
        [JsonProperty(Required = Required.Default, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string UseProxyHost;

        [DefaultValue(null)]
        [RegularExpression(@"^([0-9]{1,4}|[1-5][0-9]{4}|6[0-4][0-9]{3}|65[0-4][0-9]{2}|655[0-2][0-9]|6553[0-5])$")]
        [JsonProperty(Required = Required.Default, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string UseProxyPort;

        [DefaultValue(false)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool UseProxyAuthentication;

        [DefaultValue(null)]
        [MinLength(0)]
        [MaxLength(32)]
        [JsonProperty(Required = Required.Default, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string UseProxyUsername;

        [DefaultValue(null)]
        [MinLength(0)]
        [MaxLength(32)]
        [JsonProperty(Required = Required.Default, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string UseProxyPassword;
    }
}