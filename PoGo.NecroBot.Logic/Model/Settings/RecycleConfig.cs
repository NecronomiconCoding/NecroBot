using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace PoGo.NecroBot.Logic.Model.Settings
{
    [JsonObject(Description = "")]
    public class RecycleConfig
    {
        [DefaultValue(true)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool VerboseRecycling = true;

        [DefaultValue(90)]
        [Range(0, 100)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public double RecycleInventoryAtUsagePercentage = 90.0;

        [DefaultValue(false)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool RandomizeRecycle;

        [DefaultValue(5)]
        [Range(0, 100)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public int RandomRecycleValue = 5;

        [DefaultValue(false)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool DelayBetweenRecycleActions;

        /*Amounts*/
        [DefaultValue(120)]
        [Range(0, 999)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public int TotalAmountOfPokeballsToKeep = 120;

        [DefaultValue(80)]
        [Range(0, 999)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public int TotalAmountOfPotionsToKeep = 80;

        [DefaultValue(60)]
        [Range(0, 999)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public int TotalAmountOfRevivesToKeep = 60;

        [DefaultValue(50)]
        [Range(0, 999)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate)]
        public int TotalAmountOfBerriesToKeep = 50;
    }
}