using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace PoGo.NecroBot.Logic.Model.Settings
{

    enum YoursWalkTravelModes
    {
        motorcar,
        hgv,
        goods,
        psv,
        bicycle,
        cycleroute,
        foot,
        moped,
        mofa
    }

    [JsonObject(Description = "")]
    public class YoursWalkConfig
    {
        [DefaultValue(false)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool UseYoursWalk = false;


        [DefaultValue("bicycle")]
        [EnumDataType(typeof(YoursWalkTravelModes))]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string YoursWalkHeuristic = "bicycle";
    }
}