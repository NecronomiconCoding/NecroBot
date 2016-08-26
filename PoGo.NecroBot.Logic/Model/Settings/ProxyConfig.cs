using System.ComponentModel;
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
        [JsonProperty("UseProxyHost", Required = Required.AllowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string UseProxyHost;
        [DefaultValue(null)]
        [JsonProperty("UseProxyPort", Required = Required.AllowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string UseProxyPort;
        [DefaultValue(false)]
        [JsonProperty("UseProxyAuthentication", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool UseProxyAuthentication;
        [DefaultValue(null)]
        [JsonProperty("UseProxyUsername", Required = Required.AllowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string UseProxyUsername;
        [DefaultValue(null)]
        [JsonProperty("UseProxyPassword", Required = Required.AllowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string UseProxyPassword;
    }
}