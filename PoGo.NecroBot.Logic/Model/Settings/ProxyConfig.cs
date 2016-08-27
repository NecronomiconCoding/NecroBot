using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace PoGo.NecroBot.Logic.Model.Settings
{
    [JsonObject(MemberSerialization.OptOut)]
    public class ProxyConfig
    {
        [DefaultValue(false)]
        [JsonProperty("UseProxy", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool UseProxy;

        [DefaultValue(null)]
        [RegularExpression(@"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$")]
        [JsonProperty("UseProxyHost", Required = Required.AllowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string UseProxyHost;

        [DefaultValue(null)]
        [RegularExpression(@"^([0-9]{1,4}|[1-5][0-9]{4}|6[0-4][0-9]{3}|65[0-4][0-9]{2}|655[0-2][0-9]|6553[0-5])$")]
        [JsonProperty("UseProxyPort", Required = Required.AllowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string UseProxyPort;

        [DefaultValue(false)]
        [JsonProperty("UseProxyAuthentication", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool UseProxyAuthentication;

        [DefaultValue(null)]
        [RegularExpression(@"^.{0,32}$")]
        [JsonProperty("UseProxyUsername", Required = Required.AllowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string UseProxyUsername;

        [DefaultValue(null)]
        [RegularExpression(@"^.{0,32}$")]
        [JsonProperty("UseProxyPassword", Required = Required.AllowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string UseProxyPassword;
    }
}