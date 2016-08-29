using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using PokemonGo.RocketAPI.Enums;

namespace PoGo.NecroBot.Logic.Model.Settings
{
    [JsonObject(Description = "")]
    public class AuthConfig
    {
        [DefaultValue("google")]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public AuthType AuthType;

        [DefaultValue(null)]
        [RegularExpression(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?")]
        [MinLength(0)]
        [MaxLength(64)]
        [JsonProperty(Required = Required.Default, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string GoogleUsername;

        [DefaultValue(null)]
        [MinLength(0)]
        [MaxLength(32)]
        [JsonProperty(Required = Required.Default, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string GooglePassword;

        [DefaultValue(null)]
        [MinLength(0)]
        [MaxLength(32)]
        [JsonProperty(Required = Required.Default, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string PtcUsername;

        [DefaultValue(null)]
        [MinLength(0)]
        [MaxLength(32)]
        [JsonProperty(Required = Required.Default, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string PtcPassword;
    }
}