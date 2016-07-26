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
        public AuthSettings()
        {
            if (File.Exists(Directory.GetCurrentDirectory() + "\\config\\auth.json")) return;
            string type;
            do
            {
                Console.WriteLine("Please choose your AuthType");
                Console.WriteLine("(0) for Google Authentication");
                Console.WriteLine("(1) for Pokemon Trainer Club");
                type = Console.ReadLine();
            } while (type != "1" && type != "0");
            AuthType = type.Equals("0") ? AuthType.Google : AuthType.Ptc;
            if (AuthType == AuthType.Google)
            {
                Console.Clear();
                return;
            }               
            do
            {
                Console.WriteLine("Username:");
                PtcUsername = Console.ReadLine();
            } while (string.IsNullOrEmpty(PtcUsername));
            do
            {
                Console.WriteLine("Password:");
                PtcPassword = Console.ReadLine();
            } while (string.IsNullOrEmpty(PtcPassword));
            Console.Clear();
        }
        public AuthType AuthType;
        public string GoogleRefreshToken;
        public string PtcUsername;
        public string PtcPassword;
        

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
<<<<<<< HEAD
            GlobalSettings.AuthType = settingsToWrite.AuthType;
            GlobalSettings.DefaultAltitude = settingsToWrite.DefaultAltitude;
            GlobalSettings.DefaultLatitude = settingsToWrite.DefaultLatitude;
            GlobalSettings.DefaultLongitude = settingsToWrite.DefaultLongitude;
            GlobalSettings.DelayBetweenPokemonCatch = settingsToWrite.DelayBetweenPokemonCatch;
            GlobalSettings.EvolveAboveIvValue = settingsToWrite.EvolveAboveIvValue;
            GlobalSettings.EvolveAllPokemonAboveIv = settingsToWrite.EvolveAllPokemonAboveIv;
            GlobalSettings.EvolveAllPokemonWithEnoughCandy = settingsToWrite.EvolveAllPokemonWithEnoughCandy;
            GlobalSettings.GpxFile = settingsToWrite.GpxFile;
            GlobalSettings.KeepMinCp = settingsToWrite.KeepMinCp;
            GlobalSettings.KeepMinDuplicatePokemon = settingsToWrite.KeepMinDuplicatePokemon;
            GlobalSettings.KeepMinIvPercentage = settingsToWrite.KeepMinIvPercentage;
            GlobalSettings.KeepPokemonsThatCanEvolve =  settingsToWrite.KeepPokemonsThatCanEvolve;
            GlobalSettings.MaxTravelDistanceInMeters = settingsToWrite.MaxTravelDistanceInMeters;
            GlobalSettings.PrioritizeIvOverCp = settingsToWrite.PrioritizeIvOverCp;
            GlobalSettings.PtcPassword = settingsToWrite.PtcPassword;
            GlobalSettings.PtcUsername = settingsToWrite.PtcUsername;
            GlobalSettings.TransferDuplicatePokemon = settingsToWrite.TransferDuplicatePokemon;
            GlobalSettings.UseGpxPathing = settingsToWrite.UseGpxPathing;
            GlobalSettings.UseLuckyEggsWhileEvolving = settingsToWrite.UseLuckyEggsWhileEvolving;
            GlobalSettings.UsePokemonToNotCatchFilter = settingsToWrite.UsePokemonToNotCatchFilter;
            GlobalSettings.WalkingSpeedInKilometerPerHour = settingsToWrite.WalkingSpeedInKilometerPerHour;
            GlobalSettings.DumpPokemonStats = settingsToWrite.DumpPokemonStats;
=======
            var output = JsonConvert.SerializeObject(this, Formatting.Indented, new StringEnumConverter { CamelCaseText = true });

            string folder = Path.GetDirectoryName(path);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            File.WriteAllText(path, output);
>>>>>>> master
        }

        public void Save()
        {
<<<<<<< HEAD
            settingsToWrite.AuthType = GlobalSettings.AuthType;
            settingsToWrite.DefaultAltitude = GlobalSettings.DefaultAltitude;
            settingsToWrite.DefaultLatitude = GlobalSettings.DefaultLatitude;
            settingsToWrite.DefaultLongitude = GlobalSettings.DefaultLongitude;
            settingsToWrite.DelayBetweenPokemonCatch = GlobalSettings.DelayBetweenPokemonCatch;
            settingsToWrite.EvolveAboveIvValue = GlobalSettings.EvolveAboveIvValue;
            settingsToWrite.EvolveAllPokemonAboveIv = GlobalSettings.EvolveAllPokemonAboveIv;
            settingsToWrite.EvolveAllPokemonWithEnoughCandy = GlobalSettings.EvolveAllPokemonWithEnoughCandy;
            settingsToWrite.GpxFile = GlobalSettings.GpxFile;
            settingsToWrite.KeepMinCp = GlobalSettings.KeepMinCp;
            settingsToWrite.KeepMinDuplicatePokemon = GlobalSettings.KeepMinDuplicatePokemon;
            settingsToWrite.KeepMinIvPercentage = GlobalSettings.KeepMinIvPercentage;
            settingsToWrite.KeepPokemonsThatCanEvolve = GlobalSettings.KeepPokemonsThatCanEvolve;
            settingsToWrite.MaxTravelDistanceInMeters = GlobalSettings.MaxTravelDistanceInMeters;
            settingsToWrite.PrioritizeIvOverCp = GlobalSettings.PrioritizeIvOverCp;
            settingsToWrite.PtcPassword = GlobalSettings.PtcPassword;
            settingsToWrite.PtcUsername = GlobalSettings.PtcUsername;
            settingsToWrite.TransferDuplicatePokemon = GlobalSettings.TransferDuplicatePokemon;
            settingsToWrite.UseLuckyEggsWhileEvolving = GlobalSettings.UseLuckyEggsWhileEvolving;
            settingsToWrite.UsePokemonToNotCatchFilter = GlobalSettings.UsePokemonToNotCatchFilter;
            settingsToWrite.WalkingSpeedInKilometerPerHour = GlobalSettings.WalkingSpeedInKilometerPerHour;
            settingsToWrite.DumpPokemonStats = GlobalSettings.DumpPokemonStats;
        }
    }

    public static class GlobalSettings
    {
        public static AuthType AuthType = AuthType.Google;
        public static string PtcUsername = "username2";
        public static string PtcPassword = "pw";
        public static double DefaultLatitude = 52.379189;
        public static double DefaultLongitude = 4.899431;
        public static double DefaultAltitude = 10;
        public static float KeepMinIvPercentage = 85;
        public static int KeepMinCp = 1000;
        public static double WalkingSpeedInKilometerPerHour = 50;
        public static bool EvolveAllPokemonWithEnoughCandy = false;
        public static bool KeepPokemonsThatCanEvolve = false;
        public static bool TransferDuplicatePokemon = true;
        public static int DelayBetweenPokemonCatch = 5000;
        public static bool UsePokemonToNotCatchFilter = false;
        public static int KeepMinDuplicatePokemon = 1;
        public static bool PrioritizeIvOverCp = false;
        public static int MaxTravelDistanceInMeters = 1000;
        public static string GpxFile = "GPXFile.GPX";
        public static bool UseGpxPathing = false;
        public static bool UseLuckyEggsWhileEvolving = false;
        public static bool EvolveAllPokemonAboveIv = false;
        public static float EvolveAboveIvValue = 95;
        public static bool DumpPokemonStats = false;
    }

    public class GlobalSettingsStub
    {
        public AuthType AuthType;
        public string PtcUsername;
        public string PtcPassword;
        public double DefaultLatitude;
        public double DefaultLongitude;
        public double DefaultAltitude;
        public float KeepMinIvPercentage;
        public int KeepMinCp;
        public double WalkingSpeedInKilometerPerHour;
        public bool EvolveAllPokemonWithEnoughCandy;
        public bool KeepPokemonsThatCanEvolve;
        public bool TransferDuplicatePokemon;
        public int DelayBetweenPokemonCatch;
        public bool UsePokemonToNotCatchFilter;
        public int KeepMinDuplicatePokemon;
        public bool PrioritizeIvOverCp;
        public int MaxTravelDistanceInMeters;
        public string GpxFile;
        public bool UseGpxPathing;
        public bool UseLuckyEggsWhileEvolving;
        public bool EvolveAllPokemonAboveIv;
        public float EvolveAboveIvValue;
        public bool DumpPokemonStats;
    }
=======
            if(!string.IsNullOrEmpty(FilePath))
            {
                Save(FilePath);
            }
        }
    }

    public class GlobalSettings
    {
        public static GlobalSettings Default => new GlobalSettings();
        public static string ProfilePath;
        public static string ConfigPath;
>>>>>>> master

        public static GlobalSettings Load(string path)
        {
            ProfilePath = Directory.GetCurrentDirectory() + path;
            ConfigPath = ProfilePath + "\\config";

            var fullPath = ConfigPath + "\\config.json";

            GlobalSettings settings = null;
            if (File.Exists(fullPath))
            {
                //if the file exists, load the settings
                var input = File.ReadAllText(fullPath);

                JsonSerializerSettings jsonSettings = new JsonSerializerSettings();
                jsonSettings.Converters.Add(new StringEnumConverter { CamelCaseText = true });
                jsonSettings.ObjectCreationHandling = ObjectCreationHandling.Replace;
                jsonSettings.DefaultValueHandling = DefaultValueHandling.Populate;

                settings = JsonConvert.DeserializeObject<GlobalSettings>(input, jsonSettings);
            }
            else
            {
                settings = new GlobalSettings();
            }
            settings.Save(fullPath);
            settings.Auth.Load(ConfigPath + "\\auth.json");

            return settings;
        }

<<<<<<< HEAD
    public class LogicSettings : ILogicSettings
    {
        private ICollection<KeyValuePair<ItemId, int>> _itemRecycleFilter;
        private ICollection<PokemonId> _pokemonsNotToCatch;

        private ICollection<PokemonId> _pokemonsNotToTransfer;
        private ICollection<PokemonId> _pokemonsToEvolve;

        public float KeepMinIvPercentage => GlobalSettings.KeepMinIvPercentage;
        public int KeepMinCp => GlobalSettings.KeepMinCp;
        public double WalkingSpeedInKilometerPerHour => GlobalSettings.WalkingSpeedInKilometerPerHour;
        public bool EvolveAllPokemonWithEnoughCandy => GlobalSettings.EvolveAllPokemonWithEnoughCandy;
        public bool KeepPokemonsThatCanEvolve => GlobalSettings.KeepPokemonsThatCanEvolve;
        public bool TransferDuplicatePokemon => GlobalSettings.TransferDuplicatePokemon;
        public int DelayBetweenPokemonCatch => GlobalSettings.DelayBetweenPokemonCatch;
        public bool UsePokemonToNotCatchFilter => GlobalSettings.UsePokemonToNotCatchFilter;
        public int KeepMinDuplicatePokemon => GlobalSettings.KeepMinDuplicatePokemon;
        public bool PrioritizeIvOverCp => GlobalSettings.PrioritizeIvOverCp;
        public int MaxTravelDistanceInMeters => GlobalSettings.MaxTravelDistanceInMeters;
        public string GpxFile => GlobalSettings.GpxFile;
        public bool UseGpxPathing => GlobalSettings.UseGpxPathing;
        public bool UseLuckyEggsWhileEvolving => GlobalSettings.UseLuckyEggsWhileEvolving;
        public bool EvolveAllPokemonAboveIv => GlobalSettings.EvolveAllPokemonAboveIv;
        public float EvolveAboveIvValue => GlobalSettings.EvolveAboveIvValue;
        public bool DumpPokemonStats => GlobalSettings.DumpPokemonStats;

        public ICollection<KeyValuePair<ItemId, int>> ItemRecycleFilter
=======
        public void Save(string fullPath)
>>>>>>> master
        {
            var output = JsonConvert.SerializeObject(this, Formatting.Indented, new StringEnumConverter { CamelCaseText = true });

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
        public bool TransferDuplicatePokemon = true;
        public bool UseGpxPathing = false;
        public bool UseLuckyEggsWhileEvolving = false;
        public bool UsePokemonToNotCatchFilter = false;
        public double WalkingSpeedInKilometerPerHour = 50;
        public int AmountOfPokemonToDisplayOnStart = 10;
        public bool RenameAboveIv = false;
        public bool EnableWebSocket = false;
        public int WebSocketPort = 14561;
        
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
        public int AmountOfPokemonToDisplayOnStart => _settings.AmountOfPokemonToDisplayOnStart;
        public ICollection<KeyValuePair<ItemId, int>> ItemRecycleFilter => _settings.ItemRecycleFilter;
        public ICollection<PokemonId> PokemonsToEvolve => _settings.PokemonsToEvolve;
        public ICollection<PokemonId> PokemonsNotToTransfer => _settings.PokemonsNotToTransfer;
        public ICollection<PokemonId> PokemonsNotToCatch => _settings.PokemonsToIgnore;
    }
}
