using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace PoGo.NecroBot.Logic.Model.Settings
{
    [JsonObject(Description = "")]
    public class GpxConfig
    {
        [DefaultValue(false)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool UseGpxPathing;

        [DefaultValue("GPXPath.GPX")]
        [MinLength(0)]
        [MaxLength(32)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string GpxFile = "GPXPath.GPX";
    }
}