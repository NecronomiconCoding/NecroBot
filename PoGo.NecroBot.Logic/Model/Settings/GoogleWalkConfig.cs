using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace PoGo.NecroBot.Logic.Model.Settings
{
    /// <summary>
    /// Google has some limitations for free use
    /// 2,500 free requests per day, calculated as the sum of client-side and server-side queries; enable billing to access higher daily quotas, billed at $0.50 USD / 1000 additional requests, up to 100,000 requests daily.
    /// With cache enabled, we can optimize the use.
    ///  </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class GoogleWalkConfig
    {
        [DefaultValue(true)]
        [JsonProperty("UseGoogleWalk", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool UseGoogleWalk = true;

        [DefaultValue(1.3d)]
        [JsonProperty("DefaultStepLength", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public double DefaultStepLength = 1.3d;

        [DefaultValue("walking")]
        [RegularExpression(@"^.{0,32}$")]
        [JsonProperty("GoogleHeuristic", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        //https://developers.google.com/maps/documentation/directions/intro?hl=pt-br#TravelModes
        public string GoogleHeuristic = "walking";

        [DefaultValue(null)]
        [RegularExpression(@"^.{0,64}$")]
        [JsonProperty("GoogleAPIKey", Required = Required.AllowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        // If you have a key, nowadays a single contract is $16.000,00 USD. With a key you can deactivate Cache
        public string GoogleAPIKey = "";

        [DefaultValue(true)]
        [JsonProperty("Cache", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool Cache = true;
    }
    
}