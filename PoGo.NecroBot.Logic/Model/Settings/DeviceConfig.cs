using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace PoGo.NecroBot.Logic.Model.Settings
{
    [JsonObject(Title = "Device Config", Description = "Set your device settings (set \"DevicePackageName\" to \"random\" for auto-generated device).", ItemRequired = Required.DisallowNull)]
    public class DeviceConfig
    {
        [DefaultValue("random")]
        [MinLength(0)]
        [MaxLength(32)]
        [RegularExpression(@"[a-zA-Z0-9_\-\.\s]")]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 1)]
        public string DevicePackageName = "random";

        [DefaultValue("8525f5d8201f78b5")]
        [MinLength(16)]
        [MaxLength(16)]
        [RegularExpression(@"^[0-9A-Fa-f]{16}$")]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 2)]
        public string DeviceId = "8525f5d8201f78b5";

        [DefaultValue("msm8996")]
        [MinLength(0)]
        [MaxLength(32)]
        [RegularExpression(@"[a-zA-Z0-9_\-\.\s]")]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 3)]
        public string AndroidBoardName = "msm8996";

        [DefaultValue("1.0.0.0000")]
        [MinLength(0)]
        [MaxLength(32)]
        [RegularExpression(@"[a-zA-Z0-9_\-\.\s]")]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 4)]
        public string AndroidBootloader = "1.0.0.0000";

        [DefaultValue("HTC")]
        [MinLength(0)]
        [MaxLength(32)]
        [RegularExpression(@"[a-zA-Z0-9_\-\.\s]")]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 5)]
        public string DeviceBrand = "HTC";

        [DefaultValue("HTC 10")]
        [MinLength(0)]
        [MaxLength(32)]
        [RegularExpression(@"[a-zA-Z0-9_\-\.\s]")]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 6)]
        public string DeviceModel = "HTC 10";

        [DefaultValue("pmewl_00531")]
        [MinLength(0)]
        [MaxLength(32)]
        [RegularExpression(@"[a-zA-Z0-9_\-\.\s]")]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 7)]
        public string DeviceModelIdentifier = "pmewl_00531";

        [DefaultValue("qcom")]
        [MinLength(0)]
        [MaxLength(32)]
        [RegularExpression(@"[a-zA-Z0-9_\-\.\s]")]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 8)]
        public string DeviceModelBoot = "qcom";

        [DefaultValue("HTC")]
        [MinLength(0)]
        [MaxLength(32)]
        [RegularExpression(@"[a-zA-Z0-9_\-\.\s]")]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 9)]
        public string HardwareManufacturer = "HTC";

        [DefaultValue("HTC 10")]
        [MinLength(0)]
        [MaxLength(32)]
        [RegularExpression(@"[a-zA-Z0-9_\-\.\s]")]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 10)]
        public string HardwareModel = "HTC 10";

        [DefaultValue("pmewl_00531")]
        [MinLength(0)]
        [MaxLength(32)]
        [RegularExpression(@"[a-zA-Z0-9_\-\.\s]")]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 11)]
        public string FirmwareBrand = "pmewl_00531";

        [DefaultValue("release-keys")]
        [MinLength(0)]
        [MaxLength(32)]
        [RegularExpression(@"[a-zA-Z0-9_\-\.\s]")]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 12)]
        public string FirmwareTags = "release-keys";

        [DefaultValue("user")]
        [MinLength(0)]
        [MaxLength(32)]
        [RegularExpression(@"[a-zA-Z0-9_\-\.\s]")]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 13)]
        public string FirmwareType = "user";

        [DefaultValue("htc/pmewl_00531/htc_pmewl:6.0.1/MMB29M/770927.1:user/release-keys")]
        [MinLength(0)]
        [MaxLength(128)]
        [RegularExpression(@"[[a-zA-Z0-9_\-\/\.\:]")]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 14)]
        public string FirmwareFingerprint = "htc/pmewl_00531/htc_pmewl:6.0.1/MMB29M/770927.1:user/release-keys";
    }
}