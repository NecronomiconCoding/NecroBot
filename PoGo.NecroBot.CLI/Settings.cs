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
                string type;
                do
                {
                    Console.WriteLine($"Please choose your AuthType");
                    Console.WriteLine("(0) for Google Authentication");
                    Console.WriteLine("(1) for Pokemon Trainer Club");
                    type = Console.ReadLine();
                } while (type != "1" && type != "0");
                AuthType = type.Equals("0") ? AuthType.Google : AuthType.Ptc;
                if (AuthType == AuthType.Ptc)
                {
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
                }


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
            if (!string.IsNullOrEmpty(FilePath))
            {
                Save(FilePath);
            }
        }
    }

    public class GlobalSettings
    {
        public static GlobalSettings Default => new GlobalSettings();

        public static GlobalSettings Load(string path)
        {
            GlobalSettings settings;
            var profilePath = Path.Combine(Directory.GetCurrentDirectory(), path);
            var configPath = Path.Combine(profilePath, "config");
            var fullPath = Path.Combine(configPath, "config.json");

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

            if (settings.WebSocketPort == 0)
            {
                settings.WebSocketPort = 14251;
            }
            settings.ProfilePath = profilePath;
            settings.ConfigPath = configPath;
            settings.Save(fullPath);
            settings.Auth.Load(Path.Combine(configPath, "auth.json"));

            return settings;
        }

        public void Save(string fullPath)
        {
            var output = JsonConvert.SerializeObject(this, Formatting.Indented, new StringEnumConverter { CamelCaseText = true });

            string folder = Path.GetDirectoryName(fullPath);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            File.WriteAllText(fullPath, output);
        }

        public bool AutoUpdate = false;
        public string TranslationLanguageCode = "en";
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
        public float KeepMinIvPercentage = 95;
        public bool KeepPokemonsThatCanEvolve = false;
        public int MaxTravelDistanceInMeters = 1000;
        public bool PrioritizeIvOverCp = false;
        public bool TransferDuplicatePokemon = true;
        public bool UseEggIncubators = true;
        public bool UseGpxPathing = false;
        public bool UseLuckyEggsWhileEvolving = false;
        public int UseLuckyEggsMinPokemonAmount = 30;
        public bool UsePokemonToNotCatchFilter = false;
        public double WalkingSpeedInKilometerPerHour = 50;
        public int AmountOfPokemonToDisplayOnStart = 10;
        public bool RenameAboveIv = false;
        public int WebSocketPort = 14251;
        public string ProfilePath;
        public string ConfigPath;

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
                    PokemonId.Rattata,
                    PokemonId.Caterpie,
                    PokemonId.Weedle
                };

        public Dictionary<PokemonId, TransferFilter> PokemonsTransferFilter = new Dictionary<PokemonId, TransferFilter>
                {
                    { PokemonId.Pidgeotto, new TransferFilter(1500, 90, 1)},
                    { PokemonId.Fearow, new TransferFilter(1500, 90, 2)},
                    { PokemonId.Golbat, new TransferFilter(1500, 90, 2)},
                    { PokemonId.Eevee, new TransferFilter(600, 800, 2)},
                    { PokemonId.Mew, new TransferFilter(0, 0, 10)}
                };

        public int DelayHumanLikeWalkingCicleMin = 3000;
        public int DelayFarmState = 10000;
        public int DelayRetryLogin = 20000;
        public int DelayPositionCheckState = 3000;
        public int DelayCatchFarPokemons = 15000;
        public int DelayCatchClosePokemons = 500;
        public int DelayBetweenCatchAttempts = 2000;
        public int DelayAfterBerryIsUsed = 1500;
        public int DelayAfterPokemonIsEvolved = 3000;
        public int DelayAfterLuckyEggIsUsed = 2000;
        public int DelayAfterPokeStopIsFarmed = 1000;
        public int DelayAfterGoingOutOfRadius = 5000;
        public int DelayDisplayHighestPokemons = 500;
        public int DelayAfterItemIsRecycled = 500;
        public int DelayAfterIncubatorIsUsed = 500;
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
        public bool AutoUpdate => _settings.AutoUpdate;

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
        public string ProfilePath => _settings.ProfilePath;
        public string ConfigPath => _settings.ConfigPath;
        public bool AutoUpdate => _settings.AutoUpdate;
        public float KeepMinIvPercentage => _settings.KeepMinIvPercentage;
        public int KeepMinCp => _settings.KeepMinCp;
        public double WalkingSpeedInKilometerPerHour => _settings.WalkingSpeedInKilometerPerHour;
        public bool EvolveAllPokemonWithEnoughCandy => _settings.EvolveAllPokemonWithEnoughCandy;
        public bool KeepPokemonsThatCanEvolve => _settings.KeepPokemonsThatCanEvolve;
        public bool TransferDuplicatePokemon => _settings.TransferDuplicatePokemon;
        public bool UseEggIncubators => _settings.UseEggIncubators;
        public int DelayBetweenPokemonCatch => _settings.DelayBetweenPokemonCatch;
        public bool UsePokemonToNotCatchFilter => _settings.UsePokemonToNotCatchFilter;
        public int KeepMinDuplicatePokemon => _settings.KeepMinDuplicatePokemon;
        public bool PrioritizeIvOverCp => _settings.PrioritizeIvOverCp;
        public int MaxTravelDistanceInMeters => _settings.MaxTravelDistanceInMeters;
        public string GpxFile => _settings.GpxFile;
        public bool UseGpxPathing => _settings.UseGpxPathing;
        public bool UseLuckyEggsWhileEvolving => _settings.UseLuckyEggsWhileEvolving;
        public int UseLuckyEggsMinPokemonAmount => _settings.UseLuckyEggsMinPokemonAmount;
        public bool EvolveAllPokemonAboveIv => _settings.EvolveAllPokemonAboveIv;
        public float EvolveAboveIvValue => _settings.EvolveAboveIvValue;
        public bool RenameAboveIv => _settings.RenameAboveIv;
        public int AmountOfPokemonToDisplayOnStart => _settings.AmountOfPokemonToDisplayOnStart;
        public string TranslationLanguageCode => _settings.TranslationLanguageCode;
        public ICollection<KeyValuePair<ItemId, int>> ItemRecycleFilter => _settings.ItemRecycleFilter;
        public ICollection<PokemonId> PokemonsToEvolve => _settings.PokemonsToEvolve;
        public ICollection<PokemonId> PokemonsNotToTransfer => _settings.PokemonsNotToTransfer;
        public ICollection<PokemonId> PokemonsNotToCatch => _settings.PokemonsToIgnore;
        public Dictionary<PokemonId, TransferFilter> PokemonsTransferFilter => _settings.PokemonsTransferFilter;
        public int DelayHumanLikeWalkingCicleMin => _settings.DelayHumanLikeWalkingCicleMin;
        public int DelayFarmState => _settings.DelayFarmState;
        public int DelayRetryLogin => _settings.DelayRetryLogin;
        public int DelayPositionCheckState => _settings.DelayPositionCheckState;
        public int DelayCatchFarPokemons => _settings.DelayCatchFarPokemons;
        public int DelayCatchClosePokemons => _settings.DelayCatchClosePokemons;
        public int DelayBetweenCatchAttempts => _settings.DelayBetweenCatchAttempts;
        public int DelayAfterBerryIsUsed => _settings.DelayAfterBerryIsUsed;
        public int DelayAfterPokemonIsEvolved => _settings.DelayAfterPokemonIsEvolved;
        public int DelayAfterLuckyEggIsUsed => _settings.DelayAfterLuckyEggIsUsed;
        public int DelayAfterPokeStopIsFarmed => _settings.DelayAfterPokeStopIsFarmed;
        public int DelayAfterGoingOutOfRadius => _settings.DelayAfterGoingOutOfRadius;
        public int DelayDisplayHighestPokemons => _settings.DelayDisplayHighestPokemons;
        public int DelayAfterItemIsRecycled => _settings.DelayAfterItemIsRecycled;
        public int DelayAfterIncubatorIsUsed => _settings.DelayAfterIncubatorIsUsed;
    }
}
