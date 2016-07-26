#region using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using PoGo.NecroBot.Logic;
using PoGo.NecroBot.Logic.Logging;
using PokemonGo.RocketAPI;
using PokemonGo.RocketAPI.Enums;
using POGOProtos.Enums;
using POGOProtos.Inventory.Item;
using Newtonsoft.Json.Converters;

#endregion

namespace PoGo.NecroBot.CLI
{
    internal class AuthSettings
    {
        public AuthType AuthType = AuthType.Google;
        public string GoogleRefreshToken = "";
        public string PtcUsername = "username2";
        public string PtcPassword = "pw";
        

        [JsonIgnore]
        private string FilePath;

        public void Load(string path)
        {
            FilePath = path;

            if (File.Exists(FilePath))
            {
                //if the file exists, load the settings
                var input = File.ReadAllText(FilePath);

                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.Converters.Add(new StringEnumConverter { CamelCaseText = true });

                JsonConvert.PopulateObject(input, this, settings);
            }
            else
            {
                Save(FilePath);
            }
        }

        public void Save(string path)
        {
            var output = JsonConvert.SerializeObject(this, Formatting.Indented, new StringEnumConverter { CamelCaseText = true });

            string folder = Path.GetDirectoryName(path);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            File.WriteAllText(path, output);
        }

        public void Save()
        {
            if(!string.IsNullOrEmpty(FilePath))
            {
                Save(FilePath);
            }
        }
    }

    public class GlobalSettings
    {
        public static GlobalSettings Default => new GlobalSettings();

        private static string GetAuthPath(string path)
        {
            var fullPath = Directory.GetCurrentDirectory() + path;
            string folder = Path.GetDirectoryName(fullPath);
            folder += "\\auth.json";

            return folder;
        }

        public static GlobalSettings Load(string path)
        {
            var fullPath = Directory.GetCurrentDirectory() + path;

            GlobalSettings settings = null;
            if (File.Exists(fullPath))
            {
                //if the file exists, load the settings
                var input = File.ReadAllText(fullPath);

                JsonSerializerSettings jsonSettings = new JsonSerializerSettings();
                jsonSettings.Converters.Add(new StringEnumConverter { CamelCaseText = true });
                jsonSettings.ObjectCreationHandling = ObjectCreationHandling.Replace;

                settings = JsonConvert.DeserializeObject<GlobalSettings>(input, jsonSettings);
            }
            else
            {
                settings = new GlobalSettings();
                settings.Save(path);
            }

            settings.Auth.Load(GetAuthPath(path));

            return settings;
        }

        public void Save(string path)
        {
            var output = JsonConvert.SerializeObject(this, Formatting.Indented, new StringEnumConverter { CamelCaseText = true });

            var fullPath = Directory.GetCurrentDirectory() + path;
            string folder = Path.GetDirectoryName(fullPath);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            File.WriteAllText(fullPath, output);
        }

        public double DefaultAltitude = 10;
        public double DefaultLatitude = 52.379189;
        public double DefaultLongitude = 4.899431;
        public int DelayBetweenPokemonCatch = 2000;
        public float EvolveAboveIvValue = 95;
        public bool EvolveAllPokemonAboveIv = false;
        public bool EvolveAllPokemonWithEnoughCandy = false;
        public string GpxFile = "GPXPath.GPX";
        public int KeepMinCp = 1000;
        public int KeepMinDuplicatePokemon = 1;
        public float KeepMinIvPercentage = 85;
        public bool KeepPokemonsThatCanEvolve = true;
        public int MaxTravelDistanceInMeters = 1000;
        public bool PrioritizeIvOverCp = true;
        public bool DontCatchDuplicatedPokemon = false;
        public bool TransferDuplicatePokemon = true;
        public bool UseGpxPathing = false;
        public bool UseLuckyEggsWhileEvolving = false;
        public bool UsePokemonToNotCatchFilter = false;
        public double WalkingSpeedInKilometerPerHour = 50;
        public bool RenameAboveIv = false;
        
        [JsonIgnore]
        internal AuthSettings Auth = new AuthSettings();

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

    public class ClientSettings : ISettings
    {
        private GlobalSettings _settings;

        public ClientSettings(GlobalSettings settings)
        {
            _settings = settings;
        }

        public AuthType AuthType => _settings.Auth.AuthType;
        public string PtcUsername => _settings.Auth.PtcUsername;
        public string PtcPassword => _settings.Auth.PtcPassword;
        public double DefaultLatitude => _settings.DefaultLatitude;
        public double DefaultLongitude => _settings.DefaultLongitude;
        public double DefaultAltitude => _settings.DefaultAltitude;

        public string GoogleRefreshToken
        {
            get
            {
                return _settings.Auth.GoogleRefreshToken;
            }
            set
            {
                _settings.Auth.GoogleRefreshToken = value;
                _settings.Auth.Save();
            }
        }
    }

    public class LogicSettings : ILogicSettings
    {
        private GlobalSettings _settings;

        public LogicSettings(GlobalSettings settings)
        {
            _settings = settings;
        }

        public float KeepMinIvPercentage => _settings.KeepMinIvPercentage;
        public int KeepMinCp => _settings.KeepMinCp;
        public double WalkingSpeedInKilometerPerHour => _settings.WalkingSpeedInKilometerPerHour;
        public bool EvolveAllPokemonWithEnoughCandy => _settings.EvolveAllPokemonWithEnoughCandy;
        public bool KeepPokemonsThatCanEvolve => _settings.KeepPokemonsThatCanEvolve;
        public bool DontCatchDuplicatedPokemon => _settings.DontCatchDuplicatedPokemon;
        public bool TransferDuplicatePokemon => _settings.TransferDuplicatePokemon;
        public int DelayBetweenPokemonCatch => _settings.DelayBetweenPokemonCatch;
        public bool UsePokemonToNotCatchFilter => _settings.UsePokemonToNotCatchFilter;
        public int KeepMinDuplicatePokemon => _settings.KeepMinDuplicatePokemon;
        public bool PrioritizeIvOverCp => _settings.PrioritizeIvOverCp;
        public int MaxTravelDistanceInMeters => _settings.MaxTravelDistanceInMeters;
        public string GpxFile => _settings.GpxFile;
        public bool UseGpxPathing => _settings.UseGpxPathing;
        public bool UseLuckyEggsWhileEvolving => _settings.UseLuckyEggsWhileEvolving;
        public bool EvolveAllPokemonAboveIv => _settings.EvolveAllPokemonAboveIv;
        public float EvolveAboveIvValue => _settings.EvolveAboveIvValue;
        public bool RenameAboveIv => _settings.RenameAboveIv;
        public ICollection<KeyValuePair<ItemId, int>> ItemRecycleFilter => _settings.ItemRecycleFilter;
        public ICollection<PokemonId> PokemonsToEvolve => _settings.PokemonsToEvolve;
        public ICollection<PokemonId> PokemonsNotToTransfer => _settings.PokemonsNotToTransfer;
        public ICollection<PokemonId> PokemonsNotToCatch => _settings.PokemonsToIgnore;
    }
}