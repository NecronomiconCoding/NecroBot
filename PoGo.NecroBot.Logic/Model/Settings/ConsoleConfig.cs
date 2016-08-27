using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace PoGo.NecroBot.Logic.Model.Settings
{
    [JsonObject(MemberSerialization.OptOut)]
    public class ConsoleConfig
    {
        [DefaultValue("en")]
        [RegularExpression(@"^[a-zA-Z]{2}(-[a-zA-Z]{2})*$")]
        [JsonProperty("TranslationLanguageCode", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string TranslationLanguageCode = "en";

        [DefaultValue(false)]
        [JsonProperty("StartupWelcomeDelay", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool StartupWelcomeDelay;

        [DefaultValue(10)]
        [Range(0, 999)]
        [JsonProperty("AmountOfPokemonToDisplayOnStart", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public int AmountOfPokemonToDisplayOnStart = 10;

        [DefaultValue(true)]
        [JsonProperty("DetailedCountsBeforeRecycling", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool DetailedCountsBeforeRecycling = true;
    }
}