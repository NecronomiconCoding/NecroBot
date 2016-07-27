using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using PoGo.NecroBot.Logic;
using PokemonGo.RocketAPI;
using PokemonGo.RocketAPI.Enums;
using POGOProtos.Enums;
using POGOProtos.Inventory.Item;


namespace PoGo.NecroBot.UI.Config {
    public class GlobalSettings {
        #region "Static Class Members"
        public static GlobalSettings Default => new GlobalSettings();
        public static string ProfilePath;
        public static string ConfigPath;
        public static string ConfigFilePath;

        public static GlobalSettings Load(string path) {
            string _documentsPath = Directory.GetCurrentDirectory();
            string _relativePath = @"config";
            ConfigPath = Path.Combine(_documentsPath, _relativePath);
            ConfigFilePath = Path.Combine(ConfigPath, "config.json");

            GlobalSettings _settings = null;
            if (File.Exists(ConfigFilePath)) {
                string _input = File.ReadAllText(ConfigFilePath);

                JsonSerializerSettings _jsonSettings = new JsonSerializerSettings();
                _jsonSettings.Converters.Add(new StringEnumConverter { CamelCaseText = true });
                _jsonSettings.ObjectCreationHandling = ObjectCreationHandling.Replace;
                _jsonSettings.DefaultValueHandling = DefaultValueHandling.Populate;

                _settings = JsonConvert.DeserializeObject<GlobalSettings>(_input, _jsonSettings);
            } else {
                _settings = new GlobalSettings();
            }


            _settings.Save();
            _settings.Auth.Load();

            return _settings;
        }
        #endregion

        [JsonIgnore]
        internal AuthSettings Auth = new AuthSettings();

        public double   DefaultAltitude = 10;
        public double   DefaultLatitude = 52.379189;
        public double   DefaultLongitude = 4.899431;
        public int      DelayBetweenPokemonCatch = 2000;
        public float    EvolveAboveIvValue = 95;
        public bool     EvolveAllPokemonAboveIv = false;
        public bool     EvolveAllPokemonWithEnoughCandy = false;
        public string   GpxFile = "GPXPath.GPX";
        public int      KeepMinCp = 1000;
        public int      KeepMinDuplicatePokemon = 1;
        public float    KeepMinIvPercentage = 85;
        public bool     KeepPokemonsThatCanEvolve = true;
        public int      MaxTravelDistanceInMeters = 1000;
        public bool     PrioritizeIvOverCp = true;
        public bool     TransferDuplicatePokemon = true;
        public bool     UseGpxPathing = false;
        public bool     UseLuckyEggsWhileEvolving = false;
        public bool     UsePokemonToNotCatchFilter = false;
        public double   WalkingSpeedInKilometerPerHour = 50;
        public int      AmountOfPokemonToDisplayOnStart = 10;
        public bool     RenameAboveIv = false;
        public bool     EnableWebSocket = false;
        public int      WebSocketPort = 14561;

        public void Save() {
            string _output = JsonConvert.SerializeObject(this, Formatting.Indented, new StringEnumConverter { CamelCaseText = true });
            string _folder = Path.GetDirectoryName(ConfigFilePath);

            if (!Directory.Exists(_folder)) {
                Directory.CreateDirectory(_folder);
            }

            File.WriteAllText(ConfigFilePath, _output);
        }

        public List<KeyValuePair<ItemId, int>> ItemRecycleFilter = new List<KeyValuePair<ItemId, int>>
                {
                    new KeyValuePair<ItemId, int>(ItemId.ItemUnknown, 0),
                    new KeyValuePair<ItemId, int>(ItemId.ItemPokeBall, 25),
                    new KeyValuePair<ItemId, int>(ItemId.ItemGreatBall, 50),
                    new KeyValuePair<ItemId, int>(ItemId.ItemUltraBall, 75),
                    new KeyValuePair<ItemId, int>(ItemId.ItemMasterBall, 100),
                    new KeyValuePair<ItemId, int>(ItemId.ItemPotion, 0),
                    new KeyValuePair<ItemId, int>(ItemId.ItemSuperPotion, 25),
                    new KeyValuePair<ItemId, int>(ItemId.ItemHyperPotion, 50),
                    new KeyValuePair<ItemId, int>(ItemId.ItemMaxPotion, 75),
                    new KeyValuePair<ItemId, int>(ItemId.ItemRevive, 25),
                    new KeyValuePair<ItemId, int>(ItemId.ItemMaxRevive, 50),
                    new KeyValuePair<ItemId, int>(ItemId.ItemLuckyEgg, 200),
                    new KeyValuePair<ItemId, int>(ItemId.ItemIncenseOrdinary, 100),
                    new KeyValuePair<ItemId, int>(ItemId.ItemIncenseSpicy, 100),
                    new KeyValuePair<ItemId, int>(ItemId.ItemIncenseCool, 100),
                    new KeyValuePair<ItemId, int>(ItemId.ItemIncenseFloral, 100),
                    new KeyValuePair<ItemId, int>(ItemId.ItemTroyDisk, 100),
                    new KeyValuePair<ItemId, int>(ItemId.ItemXAttack, 100),
                    new KeyValuePair<ItemId, int>(ItemId.ItemXDefense, 100),
                    new KeyValuePair<ItemId, int>(ItemId.ItemXMiracle, 100),
                    new KeyValuePair<ItemId, int>(ItemId.ItemRazzBerry, 50),
                    new KeyValuePair<ItemId, int>(ItemId.ItemBlukBerry, 10),
                    new KeyValuePair<ItemId, int>(ItemId.ItemNanabBerry, 10),
                    new KeyValuePair<ItemId, int>(ItemId.ItemWeparBerry, 30),
                    new KeyValuePair<ItemId, int>(ItemId.ItemPinapBerry, 30),
                    new KeyValuePair<ItemId, int>(ItemId.ItemSpecialCamera, 100),
                    new KeyValuePair<ItemId, int>(ItemId.ItemIncubatorBasicUnlimited, 100),
                    new KeyValuePair<ItemId, int>(ItemId.ItemIncubatorBasic, 100),
                    new KeyValuePair<ItemId, int>(ItemId.ItemPokemonStorageUpgrade, 100),
                    new KeyValuePair<ItemId, int>(ItemId.ItemItemStorageUpgrade, 100)
                };

        public List<PokemonId> PokemonsToIgnore = new List<PokemonId>
                {
                    PokemonId.Zubat,
                    PokemonId.Pidgey,
                    PokemonId.Rattata
                };

        public List<PokemonId> PokemonsNotToTransfer = new List<PokemonId>
                {
                    PokemonId.Dragonite,
                    PokemonId.Charizard,
                    PokemonId.Zapdos,
                    PokemonId.Snorlax,
                    PokemonId.Alakazam,
                    PokemonId.Mew,
                    PokemonId.Mewtwo
                };

        public List<PokemonId> PokemonsToEvolve = new List<PokemonId>
                {
                    PokemonId.Zubat,
                    PokemonId.Pidgey,
                    PokemonId.Rattata
                };
    }
}
