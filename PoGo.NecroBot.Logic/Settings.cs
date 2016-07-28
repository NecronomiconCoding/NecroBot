#region using directives

using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PoGo.NecroBot.Logic;
using PokemonGo.RocketAPI;
using PokemonGo.RocketAPI.Enums;
using POGOProtos.Enums;
using POGOProtos.Inventory.Item;

#endregion

namespace PoGo.NecroBot.CLI
{
    internal class AuthSettings
    {
        public AuthType AuthType;


        [JsonIgnore] private string _filePath;

        public string GoogleRefreshToken;
        public string PtcUsername;
        public string PtcPassword;
        public string GoogleUsername;
        public string GooglePassword;

        public void Load(string path)
        {
            _filePath = path;

            if (File.Exists(_filePath))
            {
                //if the file exists, load the settings
                var input = File.ReadAllText(_filePath);

                var settings = new JsonSerializerSettings();
                settings.Converters.Add(new StringEnumConverter {CamelCaseText = true});

                JsonConvert.PopulateObject(input, this, settings);
            }
            else
            {
                string type;
                do
                {
                    Console.WriteLine("Please choose your AuthType");
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


                Save(_filePath);
            }
        }

        public void Save(string path)
        {
            var output = JsonConvert.SerializeObject(this, Formatting.Indented,
                new StringEnumConverter {CamelCaseText = true});

            var folder = Path.GetDirectoryName(path);
            if (folder != null && !Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            File.WriteAllText(path, output);
        }

        public void Save()
        {
            if (!string.IsNullOrEmpty(_filePath))
            {
                Save(_filePath);
            }
        }
    }

    public class GlobalSettings
    {
        public int AmountOfPokemonToDisplayOnStart = 10;

        [JsonIgnore] internal AuthSettings Auth = new AuthSettings();

        public bool AutoUpdate = true;
        public string ConfigPath;
        public double DefaultAltitude = 10;
        public double DefaultLatitude = 40.785091;
        public double DefaultLongitude = -73.968285;
        public int DelayBetweenPokemonCatch = 2000;
        public float EvolveAboveIvValue = 90;
        public bool EvolveAllPokemonAboveIv = false;
        public bool EvolveAllPokemonWithEnoughCandy = true;
        public bool DumpPokemonStats = false;
        public string GpxFile = "GPXPath.GPX";

        public List<KeyValuePair<ItemId, int>> ItemRecycleFilter = new List<KeyValuePair<ItemId, int>>
        {
            new KeyValuePair<ItemId, int>(ItemId.ItemUnknown, 0),
            new KeyValuePair<ItemId, int>(ItemId.ItemPokeBall, 25),
            new KeyValuePair<ItemId, int>(ItemId.ItemGreatBall, 50),
            new KeyValuePair<ItemId, int>(ItemId.ItemUltraBall, 100),
            new KeyValuePair<ItemId, int>(ItemId.ItemMasterBall, 100),
            new KeyValuePair<ItemId, int>(ItemId.ItemPotion, 0),
            new KeyValuePair<ItemId, int>(ItemId.ItemSuperPotion, 10),
            new KeyValuePair<ItemId, int>(ItemId.ItemHyperPotion, 40),
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

        public int KeepMinCp = 1250;
        public int KeepMinDuplicatePokemon = 1;
        public float KeepMinIvPercentage = 95;
        public bool KeepPokemonsThatCanEvolve = false;
        public int MaxTravelDistanceInMeters = 1000;

        public List<PokemonId> PokemonsNotToTransfer = new List<PokemonId>
        {
            PokemonId.Venusaur,
            PokemonId.Charizard,
            PokemonId.Blastoise,
            PokemonId.Nidoqueen,
            PokemonId.Nidoking,
            PokemonId.Clefable,
            PokemonId.Vileplume,
            PokemonId.Arcanine,
            PokemonId.Poliwrath,
            PokemonId.Machamp,
            PokemonId.Victreebel,
            PokemonId.Golem,
            PokemonId.Slowbro,
            PokemonId.Farfetchd,
            PokemonId.Muk,
            PokemonId.Exeggutor,
            PokemonId.Lickitung,
            PokemonId.Chansey,
            PokemonId.Kangaskhan,
            PokemonId.MrMime,
            PokemonId.Gyarados,
            PokemonId.Lapras,
            PokemonId.Ditto,
            PokemonId.Vaporeon,
            PokemonId.Jolteon,
            PokemonId.Flareon,
            PokemonId.Porygon,
            PokemonId.Snorlax,
            PokemonId.Articuno,
            PokemonId.Zapdos,
            PokemonId.Moltres,
            PokemonId.Dragonite,
            PokemonId.Mewtwo,
            PokemonId.Mew
             //PokemonId.Golduck,
        };

        public List<PokemonId> PokemonsToEvolve = new List<PokemonId>
        {
            //12 candies
            PokemonId.Caterpie,
            PokemonId.Weedle,
            PokemonId.Pidgey,
            //25 candies
            PokemonId.Rattata,
            //PokemonId.NidoranFemale,
            //PokemonId.NidoranMale,
            //PokemonId.Oddish,
            //PokemonId.Poliwag,
            //PokemonId.Abra,
            //PokemonId.Machop,
            //PokemonId.Bellsprout,
            //PokemonId.Geodude,
            //PokemonId.Gastly,
            //PokemonId.Eevee,
            //PokemonId.Dratini
            //50 candies
            PokemonId.Spearow,
            PokemonId.Zubat,
            PokemonId.Doduo,
            PokemonId.Goldeen,
            PokemonId.Paras,
            PokemonId.Ekans,
            PokemonId.Staryu,
            PokemonId.Psyduck,
            PokemonId.Krabby,
            PokemonId.Venonat
        };

        public List<PokemonId> PokemonsToIgnore = new List<PokemonId>
        {
            PokemonId.Caterpie,
            PokemonId.Weedle,
            PokemonId.Pidgey,
            PokemonId.Rattata,
            PokemonId.Spearow,
            PokemonId.Zubat,
            PokemonId.Doduo
        };

        public Dictionary<PokemonId, TransferFilter> PokemonsTransferFilter = new Dictionary<PokemonId, TransferFilter>
        {
            {PokemonId.Pidgeotto, new TransferFilter(1500, 90, 1)},
            {PokemonId.Fearow, new TransferFilter(1500, 90, 2)},
            {PokemonId.Golbat, new TransferFilter(1500, 90, 2)},
            {PokemonId.Eevee, new TransferFilter(600, 90, 2)},
            {PokemonId.Mew, new TransferFilter(0, 0, 10)}
        };

        public bool PrioritizeIvOverCp = true;
        public string ProfilePath;
        public bool RenameAboveIv = true;
        public bool TransferDuplicatePokemon = true;
        public string TranslationLanguageCode = "en";
        public bool UseEggIncubators = true;
        public bool UseGpxPathing = false;
        public int UseLuckyEggsMinPokemonAmount = 30;
        public bool UseLuckyEggsWhileEvolving = false;
        public bool UsePokemonToNotCatchFilter = false;
        public double WalkingSpeedInKilometerPerHour = 15.0;
        public int WebSocketPort = 14251;
        public static GlobalSettings Default => new GlobalSettings();
        public bool StartupWelcomeDelay = true;

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

                var jsonSettings = new JsonSerializerSettings();
                jsonSettings.Converters.Add(new StringEnumConverter {CamelCaseText = true});
                jsonSettings.ObjectCreationHandling = ObjectCreationHandling.Replace;
                jsonSettings.DefaultValueHandling = DefaultValueHandling.Populate;

                settings = JsonConvert.DeserializeObject<GlobalSettings>(input, jsonSettings);
            }
            else
            {
                settings = new GlobalSettings();

                Func<string, double> setLatLong = (latLong) =>
                {
                    double retVal = 0;

                    Console.WriteLine("Choose your own {0}? (y/n)", latLong);
                    var setLat = Console.ReadLine();

                    if (setLat.ToUpper() == "Y")
                    {
                        var go = true;
                        do
                        {
                            Console.WriteLine(
                                "Enter the Default {0}:", latLong);

                            var defVal = Console.ReadLine();

                            double outVar;

                            if (double.TryParse(
                                defVal,
                                out outVar))
                            {
                                retVal = outVar;
                            }
                            else
                            {
                                Console.WriteLine("Invalid {0}, Try again? (y/n)", latLong);
                                var goAgain = Console.ReadLine();

                                if (goAgain.ToUpper() != "Y")
                                {
                                    go = false;
                                }
                            }
                        } while (!go);
                    }

                    return retVal;
                };

                var defLat = setLatLong("Latitude");

                if (defLat != 0)
                    settings.DefaultLatitude = defLat;

                var defLong = setLatLong("Longitude");

                if (defLong != 0)
                    settings.DefaultLongitude = defLong;
            }

            if (settings.WebSocketPort == 0)
            {
                settings.WebSocketPort = 14251;
            }
            settings.ProfilePath = profilePath;
            settings.ConfigPath = configPath;

            if (!File.Exists(fullPath))
            {
                settings.Save(fullPath);
                return null;
            }
            settings.Save(fullPath);

            settings.Auth.Load(Path.Combine(configPath, "auth.json"));

            return settings;
        }

        public void Save(string fullPath)
        {
            var output = JsonConvert.SerializeObject(this, Formatting.Indented,
                new StringEnumConverter {CamelCaseText = true});

            var folder = Path.GetDirectoryName(fullPath);
            if (folder != null && !Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            File.WriteAllText(fullPath, output);
        }
    }

    public class ClientSettings : ISettings
    {
        private readonly GlobalSettings _settings;

        public ClientSettings(GlobalSettings settings)
        {
            _settings = settings;
        }


        public string GoogleUsername => _settings.Auth.GoogleUsername;
        public string GooglePassword => _settings.Auth.GooglePassword;

        public string GoogleRefreshToken
        {
            get { return _settings.Auth.GoogleRefreshToken; }
            set
            {
                _settings.Auth.GoogleRefreshToken = value;
                _settings.Auth.Save();
            }
        }

        AuthType ISettings.AuthType
        {
            get
            {
                return _settings.Auth.AuthType;
            }

            set
            {
                _settings.Auth.AuthType = value;
            }
        }

        double ISettings.DefaultLatitude
        {
            get
            {
                return _settings.DefaultLatitude;
            }

            set
            {
                _settings.DefaultLatitude = value;
            }
        }

        double ISettings.DefaultLongitude
        {
            get
            {
                return _settings.DefaultLongitude;
            }

            set
            {
                _settings.DefaultLongitude = value;
            }
        }

        double ISettings.DefaultAltitude
        {
            get
            {
                return _settings.DefaultAltitude;
            }

            set
            {
                _settings.DefaultAltitude = value;
            }
        }

        string ISettings.PtcPassword
        {
            get
            {
                return _settings.Auth.PtcPassword;
            }

            set
            {
                _settings.Auth.PtcPassword = value;
            }
        }

        string ISettings.PtcUsername
        {
            get
            {
                return _settings.Auth.PtcUsername;
            }

            set
            {
                _settings.Auth.PtcUsername = value;
            }
        }

        string ISettings.GoogleUsername
        {
            get
            {
                return _settings.Auth.GoogleUsername;
            }

            set
            {
                _settings.Auth.GoogleUsername = value;
            }
        }
        string ISettings.GooglePassword
        {
            get
            {
                return _settings.Auth.GooglePassword;
            }

            set
            {
                _settings.Auth.GooglePassword = value;
            }
        }
    }

    public class LogicSettings : ILogicSettings
    {
        private readonly GlobalSettings _settings;

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
        public bool DumpPokemonStats => _settings.DumpPokemonStats;
        public string TranslationLanguageCode => _settings.TranslationLanguageCode;
        public ICollection<KeyValuePair<ItemId, int>> ItemRecycleFilter => _settings.ItemRecycleFilter;
        public ICollection<PokemonId> PokemonsToEvolve => _settings.PokemonsToEvolve;
        public ICollection<PokemonId> PokemonsNotToTransfer => _settings.PokemonsNotToTransfer;
        public ICollection<PokemonId> PokemonsNotToCatch => _settings.PokemonsToIgnore;
        public Dictionary<PokemonId, TransferFilter> PokemonsTransferFilter => _settings.PokemonsTransferFilter;
        public bool StartupWelcomeDelay => _settings.StartupWelcomeDelay;
    }
}
