using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace PoGo.NecroBot.Logic.Model.Settings
{
    [JsonObject(MemberSerialization.OptOut)]
    public class CustomCatchConfig
    {
        [DefaultValue(true)]
        [JsonProperty("EnableHumanizedThrows", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool EnableHumanizedThrows = true;

        [DefaultValue(true)]
        [JsonProperty("EnableMissedThrows", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool EnableMissedThrows = true;

        [DefaultValue(25)]
        [Range(0,100)]
        [JsonProperty("ThrowMissPercentage", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public int ThrowMissPercentage = 25;

        [DefaultValue(40)]
        [Range(0, 100)]
        [JsonProperty("NiceThrowChance", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public int NiceThrowChance = 40;

        [DefaultValue(30)]
        [Range(0, 100)]
        [JsonProperty("GreatThrowChance", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public int GreatThrowChance = 30;

        [DefaultValue(10)]
        [Range(0, 100)]
        [JsonProperty("ExcellentThrowChance", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public int ExcellentThrowChance = 10;

        [DefaultValue(90)]
        [Range(0, 100)]
        [JsonProperty("CurveThrowChance", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public int CurveThrowChance = 90;

        [DefaultValue(90.00)]
        [Range(0, 100)]
        [JsonProperty("ForceGreatThrowOverIv", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public double ForceGreatThrowOverIv = 90.00;

        [DefaultValue(95.00)]
        [Range(0, 100)]
        [JsonProperty("ForceExcellentThrowOverIv", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public double ForceExcellentThrowOverIv = 95.00;

        [DefaultValue(1000)]
        [Range(0, 9999)]
        [JsonProperty("ForceGreatThrowOverCp", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public int ForceGreatThrowOverCp = 1000;

        [DefaultValue(1500)]
        [Range(0, 9999)]
        [JsonProperty("ForceExcellentThrowOverCp", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public int ForceExcellentThrowOverCp = 1500;
    }
}