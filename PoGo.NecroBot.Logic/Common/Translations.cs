using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PoGo.NecroBot.Logic.Common
{
    public class Translations
    {
        public static Translations Default => new Translations();
        public static string ProfilePath;
        public static string ConfigPath;

        public static Translations Load(string translationsLanguageCode)
        {
            ProfilePath = Directory.GetCurrentDirectory();
            ConfigPath = Path.Combine(ProfilePath, "config", "translations");

            var fullPath = Path.Combine(ConfigPath, "translation." + translationsLanguageCode + ".json");

            Translations translations;
            if (File.Exists(fullPath))
            {
                var input = File.ReadAllText(fullPath);

                JsonSerializerSettings jsonSettings = new JsonSerializerSettings();
                jsonSettings.Converters.Add(new StringEnumConverter { CamelCaseText = true });
                jsonSettings.ObjectCreationHandling = ObjectCreationHandling.Replace;
                jsonSettings.DefaultValueHandling = DefaultValueHandling.Populate;

                translations = JsonConvert.DeserializeObject<Translations>(input, jsonSettings);
                translations.Save(fullPath);
            }
            else
            {
                translations = new Translations();
                translations.Save(Path.Combine(ConfigPath, "translation.en.json"));
            }
            return translations;
        }

        public void Save(string fullPath)
        {
            var output = JsonConvert.SerializeObject(this, Formatting.Indented, new StringEnumConverter { CamelCaseText = true });

            var folder = Path.GetDirectoryName(fullPath);
            if (folder != null && !Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            File.WriteAllText(fullPath, output);
        }

        public string GetTranslation(TranslationString translationString, params object[] data)
        {
            string translation = TranslationStrings.FirstOrDefault(t => t.Key.Equals(translationString)).Value;
            if (translation != default(string))
                return string.Format(translation, data);
            return $"Translation for {translationString} is missing";
        }

        public string GetTranslation(TranslationString translationString)
        {
            string translation = TranslationStrings.FirstOrDefault(t => t.Key.Equals(translationString)).Value;
            if (translation != default(string))
                return translation;
            return $"Translation for {translationString} is missing";
        }

        //Default Translations (ENGLISH)
        public List<KeyValuePair<TranslationString, string>> TranslationStrings = new List<KeyValuePair<TranslationString, string>>
        {
            new KeyValuePair<TranslationString, string>(TranslationString.Pokeball, "PokeBall"),
            new KeyValuePair<TranslationString, string>(TranslationString.GreatPokeball, "GreatBall"),
            new KeyValuePair<TranslationString, string>(TranslationString.UltraPokeball, "UltraBall"),
            new KeyValuePair<TranslationString, string>(TranslationString.MasterPokeball, "MasterBall"),
            new KeyValuePair<TranslationString, string>(TranslationString.WrongAuthType, "Unknown AuthType in config.json"),
            new KeyValuePair<TranslationString, string>(TranslationString.FarmPokestopsOutsideRadius, "You're outside of your defined radius! Walking to start ({0}m away) in 5 seconds. Is your Coords.ini file correct?"),
            new KeyValuePair<TranslationString, string>(TranslationString.FarmPokestopsNoUsableFound, "No usable PokeStops found in your area. Is your maximum distance too small?"),
            new KeyValuePair<TranslationString, string>(TranslationString.EventFortUsed, "XP: {0}, Gems: {1}, Items: {2}"),
            new KeyValuePair<TranslationString, string>(TranslationString.EventFortTargeted, "{0} in ({1}m)"),
            new KeyValuePair<TranslationString, string>(TranslationString.EventProfileLogin, "Playing as {0}"),
            new KeyValuePair<TranslationString, string>(TranslationString.EventUsedLuckyEgg, "Used Lucky Egg, remaining: {0}"),
            new KeyValuePair<TranslationString, string>(TranslationString.EventPokemonEvolvedSuccess, "{0} successfully for {1}xp"),
            new KeyValuePair<TranslationString, string>(TranslationString.EventPokemonEvolvedFailed, "Failed {0}. Result was {1}, stopping evolving {2}"),
            new KeyValuePair<TranslationString, string>(TranslationString.EventPokemonTransferred, "{0}\t- CP: {1}  IV: {2}%   [Best CP: {3}  IV: {4}%] (Candies: {5})"),
            new KeyValuePair<TranslationString, string>(TranslationString.EventItemRecycled, "{0}x {1}"),
            new KeyValuePair<TranslationString, string>(TranslationString.EventPokemonCapture, "({0}) | ({1}) {2} Lvl: {3} CP: ({4}/{5}) IV: {6}% | Chance: {7}% | {8}m dist | with a {9} ({10} left). | {11}"),
            new KeyValuePair<TranslationString, string>(TranslationString.EventNoPokeballs, "No Pokeballs - We missed a {0} with CP {1}"),
            new KeyValuePair<TranslationString, string>(TranslationString.CatchStatusAttempt, "{0} Attempt #{1}"),
            new KeyValuePair<TranslationString, string>(TranslationString.CatchStatus, "{0}"),
            new KeyValuePair<TranslationString, string>(TranslationString.Candies, "Candies: {0}")
        };
    }

    public enum TranslationString
    {
        Pokeball,
        GreatPokeball,
        UltraPokeball,
        MasterPokeball,
        LogLevelDebug,
        LogLevelPokestop,
        WrongAuthType,
        FarmPokestopsOutsideRadius,
        FarmPokestopsNoUsableFound,
        EventFortUsed,
        EventFortTargeted,
        EventProfileLogin,
        EventUsedLuckyEgg,
        EventPokemonEvolvedSuccess,
        EventPokemonEvolvedFailed,
        EventPokemonTransferred,
        EventItemRecycled,
        EventPokemonCapture,
        EventNoPokeballs,
        CatchStatusAttempt,
        CatchStatus,
        Candies,
    }
}
