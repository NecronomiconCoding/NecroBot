using System.ComponentModel;
using Newtonsoft.Json;

namespace PoGo.NecroBot.Logic.Model.Settings
{
    [JsonObject(Description = "")]
    public class UpdateConfig
    {
        [DefaultValue(true)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool CheckForUpdates = true;

        [DefaultValue(true)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool AutoUpdate = true;

        [DefaultValue(true)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool TransferConfigAndAuthOnUpdate = true;
    }
}