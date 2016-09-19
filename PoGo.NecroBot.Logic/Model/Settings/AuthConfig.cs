using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using PokemonGo.RocketAPI.Enums;

namespace PoGo.NecroBot.Logic.Model.Settings
{
    [JsonObject(Title = "Authentication Config", Description = "Set your authentication type (Google or Ptc) and your login informartion.", ItemRequired = Required.DisallowNull)]
    public class AuthConfig
    {
        [DefaultValue("google")]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 1)]
        public AuthType AuthType = AuthType.Google;

        [DefaultValue(null)]
        [RegularExpression(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?")] // Valid email
        [MinLength(0)]
        [MaxLength(64)]
        [JsonProperty(Required = Required.Default, DefaultValueHandling = DefaultValueHandling.Populate, Order = 2)]
        public string GoogleUsername;

        [DefaultValue(null)]
        [MinLength(0)]
        [MaxLength(50)]
        [JsonProperty(Required = Required.Default, DefaultValueHandling = DefaultValueHandling.Populate, Order = 3)]
        public string GooglePassword;

        [DefaultValue(null)]
        [MinLength(0)]
        [MaxLength(32)]
        [JsonProperty(Required = Required.Default, DefaultValueHandling = DefaultValueHandling.Populate, Order = 4)]
        public string PtcUsername;

        [DefaultValue(null)]
        [MinLength(0)]
        [MaxLength(32)]
        [JsonProperty(Required = Required.Default, DefaultValueHandling = DefaultValueHandling.Populate, Order = 5)]
        public string PtcPassword;
    }
}