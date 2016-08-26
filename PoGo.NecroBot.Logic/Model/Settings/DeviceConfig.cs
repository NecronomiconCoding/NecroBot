using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace PoGo.NecroBot.Logic.Model.Settings
{
    [JsonObject(MemberSerialization.OptOut)]
    public class DeviceConfig
    {
        [DefaultValue("random")]
        [RegularExpression(@"^[a-zA-Z0-9_\-\.\s]{1,32}$")]
        [JsonProperty("DevicePackageName", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string DevicePackageName = "random";

        [DefaultValue("8525f5d8201f78b5")]
        [RegularExpression(@"^[0-9A-Fa-f]{16}$")]
        [JsonProperty("DeviceId", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string DeviceId = "8525f5d8201f78b5";

        [DefaultValue("msm8996")]
        [RegularExpression(@"^[a-zA-Z0-9_\-\.\s]{1,32}$")]
        [JsonProperty("AndroidBoardName", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string AndroidBoardName = "msm8996";

        [DefaultValue("1.0.0.0000")]
        [RegularExpression(@"^[a-zA-Z0-9_\-\.\s]{1,32}$")]
        [JsonProperty("AndroidBootloader", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string AndroidBootloader = "1.0.0.0000";

        [DefaultValue("HTC")]
        [RegularExpression(@"^[a-zA-Z0-9_\-\.\s]{1,32}$")]
        [JsonProperty("DeviceBrand", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string DeviceBrand = "HTC";

        [DefaultValue("HTC 10")]
        [RegularExpression(@"^[a-zA-Z0-9_\-\.\s]{1,32}$")]
        [JsonProperty("DeviceModel", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string DeviceModel = "HTC 10";

        [DefaultValue("pmewl_00531")]
        [RegularExpression(@"^[a-zA-Z0-9_\-\.\s]{1,32}$")]
        [JsonProperty("DeviceModelIdentifier", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string DeviceModelIdentifier = "pmewl_00531";

        [DefaultValue("qcom")]
        [RegularExpression(@"^[a-zA-Z0-9_\-\.\s]{1,32}$")]
        [JsonProperty("DeviceModelBoot", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string DeviceModelBoot = "qcom";

        [DefaultValue("HTC")]
        [RegularExpression(@"^[a-zA-Z0-9_\-\.\s]{1,32}$")]
        [JsonProperty("HardwareManufacturer", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string HardwareManufacturer = "HTC";

        [DefaultValue("HTC 10")]
        [RegularExpression(@"^[a-zA-Z0-9_\-\.\s]{1,32}$")]
        [JsonProperty("HardwareModel", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string HardwareModel = "HTC 10";

        [DefaultValue("pmewl_00531")]
        [RegularExpression(@"^[a-zA-Z0-9_\-\.\s]{1,32}$")]
        [JsonProperty("FirmwareBrand", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string FirmwareBrand = "pmewl_00531";

        [DefaultValue("release-keys")]
        [RegularExpression(@"^[a-zA-Z0-9_\-\.\s]{1,32}$")]
        [JsonProperty("FirmwareTags", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string FirmwareTags = "release-keys";

        [DefaultValue("user")]
        [RegularExpression(@"^[a-zA-Z0-9_\-\.\s]{1,32}$")]
        [JsonProperty("FirmwareType", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string FirmwareType = "user";

        [DefaultValue("htc/pmewl_00531/htc_pmewl:6.0.1/MMB29M/770927.1:user/release-keys")]
        [RegularExpression(@"^[[a-zA-Z0-9_\-\/\.\:]{1,64}$")]
        [JsonProperty("FirmwareFingerprint", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string FirmwareFingerprint = "htc/pmewl_00531/htc_pmewl:6.0.1/MMB29M/770927.1:user/release-keys";
    }
}