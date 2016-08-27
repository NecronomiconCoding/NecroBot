using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using PokemonGo.RocketAPI.Enums;

namespace PoGo.NecroBot.Logic.Model.Settings
{
    [JsonObject(MemberSerialization.OptOut)]
    public class AuthConfig
    {
        [DefaultValue("google")]
        [JsonProperty("AuthType", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public AuthType AuthType;

        [DefaultValue(null)]
        [RegularExpression(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?")]
        [JsonProperty("GoogleUsername", Required = Required.AllowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string GoogleUsername;

        [DefaultValue(null)]
        [RegularExpression(@"^.{0,32}$")]
        [JsonProperty("GooglePassword", Required = Required.AllowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string GooglePassword;

        [DefaultValue(null)]
        [RegularExpression(@"^.{0,32}$")]
        [JsonProperty("PtcUsername", Required = Required.AllowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string PtcUsername;

        [DefaultValue(null)]
        [RegularExpression(@"^.{0,32}$")]
        [JsonProperty("PtcPassword", Required = Required.AllowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string PtcPassword;
    }
}