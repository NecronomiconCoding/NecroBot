using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace PoGo.NecroBot.Logic.Model.Settings
{
    [JsonObject(Description = "")]
    public class DeviceConfig
    {
        [DefaultValue("random")]
        [MinLength(0)]
        [MaxLength(32)]
        [RegularExpression(@"[a-zA-Z0-9_\-\.\s]")]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string DevicePackageName = "random";

        [DefaultValue("8525f5d8201f78b5")]
        [MinLength(16)]
        [MaxLength(16)]
        [RegularExpression(@"^[0-9A-Fa-f]{16}$")]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string DeviceId = "8525f5d8201f78b5";

        [DefaultValue("msm8996")]
        [MinLength(0)]
        [MaxLength(32)]
        [RegularExpression(@"[a-zA-Z0-9_\-\.\s]")]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string AndroidBoardName = "msm8996";

        [DefaultValue("1.0.0.0000")]
        [MinLength(0)]
        [MaxLength(32)]
        [RegularExpression(@"[a-zA-Z0-9_\-\.\s]")]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string AndroidBootloader = "1.0.0.0000";

        [DefaultValue("HTC")]
        [MinLength(0)]
        [MaxLength(32)]
        [RegularExpression(@"[a-zA-Z0-9_\-\.\s]")]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string DeviceBrand = "HTC";

        [DefaultValue("HTC 10")]
        [MinLength(0)]
        [MaxLength(32)]
        [RegularExpression(@"[a-zA-Z0-9_\-\.\s]")]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string DeviceModel = "HTC 10";

        [DefaultValue("pmewl_00531")]
        [MinLength(0)]
        [MaxLength(32)]
        [RegularExpression(@"[a-zA-Z0-9_\-\.\s]")]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string DeviceModelIdentifier = "pmewl_00531";

        [DefaultValue("qcom")]
        [MinLength(0)]
        [MaxLength(32)]
        [RegularExpression(@"[a-zA-Z0-9_\-\.\s]")]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string DeviceModelBoot = "qcom";

        [DefaultValue("HTC")]
        [MinLength(0)]
        [MaxLength(32)]
        [RegularExpression(@"[a-zA-Z0-9_\-\.\s]")]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string HardwareManufacturer = "HTC";

        [DefaultValue("HTC 10")]
        [MinLength(0)]
        [MaxLength(32)]
        [RegularExpression(@"[a-zA-Z0-9_\-\.\s]")]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string HardwareModel = "HTC 10";

        [DefaultValue("pmewl_00531")]
        [MinLength(0)]
        [MaxLength(32)]
        [RegularExpression(@"[a-zA-Z0-9_\-\.\s]")]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string FirmwareBrand = "pmewl_00531";

        [DefaultValue("release-keys")]
        [MinLength(0)]
        [MaxLength(32)]
        [RegularExpression(@"[a-zA-Z0-9_\-\.\s]")]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string FirmwareTags = "release-keys";

        [DefaultValue("user")]
        [MinLength(0)]
        [MaxLength(32)]
        [RegularExpression(@"[a-zA-Z0-9_\-\.\s]")]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string FirmwareType = "user";

        [DefaultValue("htc/pmewl_00531/htc_pmewl:6.0.1/MMB29M/770927.1:user/release-keys")]
        [MinLength(0)]
        [MaxLength(128)]
        [RegularExpression(@"[[a-zA-Z0-9_\-\/\.\:]")]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string FirmwareFingerprint = "htc/pmewl_00531/htc_pmewl:6.0.1/MMB29M/770927.1:user/release-keys";
    }
}