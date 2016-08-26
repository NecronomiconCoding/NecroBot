using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using PokemonGo.RocketAPI.Enums;

namespace PoGo.NecroBot.Logic.Model.Settings
{
    [JsonObject(MemberSerialization.OptOut)]
    public class AuthConfig
    {
        [DefaultValue(AuthType.Google)]
        [JsonProperty("AuthType", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public AuthType AuthType;
        [DefaultValue(null)]
        [JsonProperty("GoogleUsername", Required = Required.AllowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string GoogleUsername;
        [DefaultValue(null)]
        [JsonProperty("GooglePassword", Required = Required.AllowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string GooglePassword;
        [DefaultValue(null)]
        [JsonProperty("PtcUsername", Required = Required.AllowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string PtcUsername;
        [DefaultValue(null)]
        [JsonProperty("PtcPassword", Required = Required.AllowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string PtcPassword;
    }
}