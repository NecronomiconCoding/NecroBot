using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace PoGo.NecroBot.Logic.Model.Settings
{
    [JsonObject(MemberSerialization.OptOut)]
    public class GpxConfig
    {
        [DefaultValue(false)]
        [JsonProperty("UseGpxPathing", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool UseGpxPathing;

        [DefaultValue("GPXPath.GPX")]
        [RegularExpression(@"^.{0,32}$")]
        [JsonProperty("GpxFile", Required = Required.AllowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string GpxFile = "GPXPath.GPX";
    }
}