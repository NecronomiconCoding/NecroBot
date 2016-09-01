using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace PoGo.NecroBot.Logic.Model.Settings
{
    [JsonObject(Title = "Console Config", Description = "Set your console settings.", ItemRequired = Required.DisallowNull)]
    public class ConsoleConfig
    {
        [DefaultValue("en")]
        [RegularExpression(@"^[a-zA-Z]{2}(-[a-zA-Z]{2})*$")]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 1)]
        public string TranslationLanguageCode = "en";

        [DefaultValue(false)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 2)]
        public bool StartupWelcomeDelay;

        [DefaultValue(10)]
        [Range(0, 250)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 3)]
        public int AmountOfPokemonToDisplayOnStart = 10;

        [DefaultValue(true)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 4)]
        public bool DetailedCountsBeforeRecycling = true;
    }
}