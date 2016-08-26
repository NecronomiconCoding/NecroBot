using System.ComponentModel;
using Newtonsoft.Json;

namespace PoGo.NecroBot.Logic.Model.Settings
{
    [JsonObject(MemberSerialization.OptOut)]
    public class DeviceConfig
    {
        [DefaultValue("random")]
        [JsonProperty("DevicePackageName", Required = Required.AllowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string DevicePackageName = "random";

        [DefaultValue("8525f5d8201f78b5")]
        [JsonProperty("DeviceId", Required = Required.AllowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string DeviceId = "8525f5d8201f78b5";

        [DefaultValue("msm8996")]
        [JsonProperty("AndroidBoardName", Required = Required.AllowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string AndroidBoardName = "msm8996";

        [DefaultValue("1.0.0.0000")]
        [JsonProperty("AndroidBootloader", Required = Required.AllowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string AndroidBootloader = "1.0.0.0000";

        [DefaultValue("HTC")]
        [JsonProperty("DeviceBrand", Required = Required.AllowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string DeviceBrand = "HTC";

        [DefaultValue("HTC 10")]
        [JsonProperty("DeviceModel", Required = Required.AllowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string DeviceModel = "HTC 10";

        [DefaultValue("pmewl_00531")]
        [JsonProperty("DeviceModelIdentifier", Required = Required.AllowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string DeviceModelIdentifier = "pmewl_00531";

        [DefaultValue("qcom")]
        [JsonProperty("DeviceModelBoot", Required = Required.AllowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string DeviceModelBoot = "qcom";

        [DefaultValue("HTC")]
        [JsonProperty("HardwareManufacturer", Required = Required.AllowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string HardwareManufacturer = "HTC";

        [DefaultValue("HTC 10")]
        [JsonProperty("HardwareModel", Required = Required.AllowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string HardwareModel = "HTC 10";

        [DefaultValue("pmewl_00531")]
        [JsonProperty("FirmwareBrand", Required = Required.AllowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string FirmwareBrand = "pmewl_00531";

        [DefaultValue("release-keys")]
        [JsonProperty("FirmwareTags", Required = Required.AllowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string FirmwareTags = "release-keys";

        [DefaultValue("user")]
        [JsonProperty("FirmwareType", Required = Required.AllowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string FirmwareType = "user";

        [DefaultValue("htc/pmewl_00531/htc_pmewl:6.0.1/MMB29M/770927.1:user/release-keys")]
        [JsonProperty("FirmwareFingerprint", Required = Required.AllowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string FirmwareFingerprint = "htc/pmewl_00531/htc_pmewl:6.0.1/MMB29M/770927.1:user/release-keys";
    }
}