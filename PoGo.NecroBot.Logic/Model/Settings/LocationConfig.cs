using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace PoGo.NecroBot.Logic.Model.Settings
{
    [JsonObject(Description = "")]
    public class LocationConfig
    {
        [DefaultValue(false)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool DisableHumanWalking;

        [DefaultValue(true)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool StartFromLastPosition = true;

        [DefaultValue(40.785092)]
        [Range(-90, 90)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public double DefaultLatitude = 40.785092;

        [DefaultValue(-73.968286)]
        [Range(-180, 180)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public double DefaultLongitude = -73.968286;

        [DefaultValue(4.16)]
        [Range(0, 999)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public double WalkingSpeedInKilometerPerHour = 4.16;

        [DefaultValue(true)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool UseWalkingSpeedVariant = true;

        [DefaultValue(1.2)]
        [Range(0, 999)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public double WalkingSpeedVariant = 1.2;

        [DefaultValue(false)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool ShowVariantWalking = false;

        [DefaultValue(true)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool RandomlyPauseAtStops = true;

        [DefaultValue(10)]
        [Range(0, 999)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public int MaxSpawnLocationOffset = 10;

        [DefaultValue(1000)]
        [Range(0, 9999)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public int MaxTravelDistanceInMeters = 1000;

        [JsonIgnore]
        public int ResumeTrack = 0;
        [JsonIgnore]
        public int ResumeTrackSeg = 0;
        [JsonIgnore]
        public int ResumeTrackPt = 0;
    }
}