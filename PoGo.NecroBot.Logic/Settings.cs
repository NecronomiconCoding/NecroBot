
#region using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PoGo.NecroBot.Logic.Common;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Utils;
using PokemonGo.RocketAPI;
using PokemonGo.RocketAPI.Enums;
using POGOProtos.Enums;
using POGOProtos.Inventory.Item;

#endregion

namespace PoGo.NecroBot.Logic
{
    public class AuthConfig
    {
        public AuthType AuthType;
        public string GoogleUsername;
        public string GooglePassword;
        public string PtcUsername;
        public string PtcPassword;
    }

    public class ProxyConfig
    {
        public bool UseProxy;
        public string UseProxyHost;
        public string UseProxyPort;
        public bool UseProxyAuthentication;
        public string UseProxyUsername;
        public string UseProxyPassword;
    }

    public class DeviceConfig
    {
        public string DevicePackageName = "random";
        public string DeviceId = "8525f5d8201f78b5";
        public string AndroidBoardName = "msm8996";
        public string AndroidBootloader = "1.0.0.0000";
        public string DeviceBrand = "HTC";
        public string DeviceModel = "HTC 10";
        public string DeviceModelIdentifier = "pmewl_00531";
        public string DeviceModelBoot = "qcom";
        public string HardwareManufacturer = "HTC";
        public string HardwareModel = "HTC 10";
        public string FirmwareBrand = "pmewl_00531";
        public string FirmwareTags = "release-keys";
        public string FirmwareType = "user";
        public string FirmwareFingerprint = "htc/pmewl_00531/htc_pmewl:6.0.1/MMB29M/770927.1:user/release-keys";
    }

    public class AuthSettings
    {
        [JsonIgnore]
        private string _filePath;

        public AuthConfig AuthConfig = new AuthConfig();
        public ProxyConfig ProxyConfig = new ProxyConfig();
        public DeviceConfig DeviceConfig = new DeviceConfig();

        public AuthSettings()
        {
            InitializePropertyDefaultValues(this);
        }

        public void InitializePropertyDefaultValues(object obj)
        {
            FieldInfo[] fields = obj.GetType().GetFields();

            foreach (FieldInfo field in fields)
            {
                var d = field.GetCustomAttribute<DefaultValueAttribute>();

                if (d != null)
                    field.SetValue(obj, d.Value);
            }
        }

        public void Load(string path)
        {
            try
            {
                _filePath = path;

                if (File.Exists(_filePath))
                {
                    // if the file exists, load the settings
                    var input = File.ReadAllText(_filePath);

                    var settings = new JsonSerializerSettings();
                    settings.Converters.Add(new StringEnumConverter { CamelCaseText = true });
                    JsonConvert.PopulateObject(input, this, settings);
                }
                // Do some post-load logic to determine what device info to be using - if 'custom' is set we just take what's in the file without question
                if (!this.DeviceConfig.DevicePackageName.Equals("random", StringComparison.InvariantCultureIgnoreCase) && !this.DeviceConfig.DevicePackageName.Equals("custom", StringComparison.InvariantCultureIgnoreCase))
                {
                    // User requested a specific device package, check to see if it exists and if so, set it up - otherwise fall-back to random package
                    string keepDevId = this.DeviceConfig.DeviceId;
                    SetDevInfoByKey(this.DeviceConfig.DevicePackageName);
                    this.DeviceConfig.DeviceId = keepDevId;
                }
                if (this.DeviceConfig.DevicePackageName.Equals("random", StringComparison.InvariantCultureIgnoreCase))
                {
                    // Random is set, so pick a random device package and set it up - it will get saved to disk below and re-used in subsequent sessions
                    Random rnd = new Random();
                    int rndIdx = rnd.Next(0, DeviceInfoHelper.DeviceInfoSets.Keys.Count - 1);
                    this.DeviceConfig.DevicePackageName = DeviceInfoHelper.DeviceInfoSets.Keys.ToArray()[rndIdx];
                    SetDevInfoByKey(this.DeviceConfig.DevicePackageName);
                }
                if (string.IsNullOrEmpty(this.DeviceConfig.DeviceId) || this.DeviceConfig.DeviceId == "8525f5d8201f78b5")
                    this.DeviceConfig.DeviceId = this.RandomString(16, "0123456789abcdef"); // changed to random hex as full alphabet letters could have been flagged

                // Jurann: Note that some device IDs I saw when adding devices had smaller numbers, only 12 or 14 chars instead of 16 - probably not important but noted here anyway

                Save(_filePath);
            }
            catch (JsonReaderException exception)
            {
                if (exception.Message.Contains("Unexpected character") && exception.Message.Contains("PtcUsername"))
                    Logger.Write("JSON Exception: You need to properly configure your PtcUsername using quotations.",
                        LogLevel.Error);
                else if (exception.Message.Contains("Unexpected character") && exception.Message.Contains("PtcPassword"))
                    Logger.Write(
                        "JSON Exception: You need to properly configure your PtcPassword using quotations.",
                        LogLevel.Error);
                else if (exception.Message.Contains("Unexpected character") &&
                         exception.Message.Contains("GoogleUsername"))
                    Logger.Write(
                        "JSON Exception: You need to properly configure your GoogleUsername using quotations.",
                        LogLevel.Error);
                else if (exception.Message.Contains("Unexpected character") &&
                         exception.Message.Contains("GooglePassword"))
                    Logger.Write(
                        "JSON Exception: You need to properly configure your GooglePassword using quotations.",
                        LogLevel.Error);
                else
                    Logger.Write("JSON Exception: " + exception.Message, LogLevel.Error);
            }
        }

        public void Save(string fullPath)
        {
            var jsonSerializeSettings = new JsonSerializerSettings
            {
                DefaultValueHandling = DefaultValueHandling.Include,
                Formatting = Formatting.Indented,
                Converters = new JsonConverter[] { new StringEnumConverter { CamelCaseText = true } }
            };

            var output = JsonConvert.SerializeObject(this, jsonSerializeSettings);

            var folder = Path.GetDirectoryName(fullPath);
            if (folder != null && !Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            File.WriteAllText(fullPath, output);
        }

        public void Save()
        {
            if (!string.IsNullOrEmpty(_filePath))
            {
                Save(_filePath);
            }
        }

        public void checkProxy(ITranslation translator)
        {
            using (var tempWebClient = new NecroWebClient())
            {
                string unproxiedIP = WebClientExtensions.DownloadString(tempWebClient, new Uri("https://api.ipify.org/?format=text"));
                if (ProxyConfig.UseProxy)
                {
                    tempWebClient.Proxy = this.InitProxy();
                    string proxiedIPres = WebClientExtensions.DownloadString(tempWebClient, new Uri("https://api.ipify.org/?format=text"));
                    string proxiedIP = proxiedIPres == null?"INVALID PROXY": proxiedIPres;
                    Logger.Write(translator.GetTranslation(TranslationString.Proxied, unproxiedIP, proxiedIP), LogLevel.Info, (unproxiedIP == proxiedIP) ? ConsoleColor.Red : ConsoleColor.Green);

                    if (unproxiedIP == proxiedIP || proxiedIPres == null)
                    {
                        Logger.Write(translator.GetTranslation(TranslationString.FixProxySettings), LogLevel.Info, ConsoleColor.Red);
                        Console.ReadKey();
                        Environment.Exit(0);
                    }
                }
                else
                {
                    Logger.Write(translator.GetTranslation(TranslationString.Unproxied, unproxiedIP), LogLevel.Info, ConsoleColor.Red);
                }
            }
        }

        private string RandomString(int length, string alphabet = "abcdefghijklmnopqrstuvwxyz0123456789")
        {
            var outOfRange = Byte.MaxValue + 1 - (Byte.MaxValue + 1) % alphabet.Length;

            return string.Concat(
                Enumerable
                    .Repeat(0, Int32.MaxValue)
                    .Select(e => this.RandomByte())
                    .Where(randomByte => randomByte < outOfRange)
                    .Take(length)
                    .Select(randomByte => alphabet[randomByte % alphabet.Length])
            );
        }

        private byte RandomByte()
        {
            using (var randomizationProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[1];
                randomizationProvider.GetBytes(randomBytes);
                return randomBytes.Single();
            }
        }

        private void SetDevInfoByKey(string devKey)
        {
            if (DeviceInfoHelper.DeviceInfoSets.ContainsKey(this.DeviceConfig.DevicePackageName))
            {
                this.DeviceConfig.AndroidBoardName = DeviceInfoHelper.DeviceInfoSets[this.DeviceConfig.DevicePackageName]["AndroidBoardName"];
                this.DeviceConfig.AndroidBootloader = DeviceInfoHelper.DeviceInfoSets[this.DeviceConfig.DevicePackageName]["AndroidBootloader"];
                this.DeviceConfig.DeviceBrand = DeviceInfoHelper.DeviceInfoSets[this.DeviceConfig.DevicePackageName]["DeviceBrand"];
                this.DeviceConfig.DeviceId = DeviceInfoHelper.DeviceInfoSets[this.DeviceConfig.DevicePackageName]["DeviceId"];
                this.DeviceConfig.DeviceModel = DeviceInfoHelper.DeviceInfoSets[this.DeviceConfig.DevicePackageName]["DeviceModel"];
                this.DeviceConfig.DeviceModelBoot = DeviceInfoHelper.DeviceInfoSets[this.DeviceConfig.DevicePackageName]["DeviceModelBoot"];
                this.DeviceConfig.DeviceModelIdentifier = DeviceInfoHelper.DeviceInfoSets[this.DeviceConfig.DevicePackageName]["DeviceModelIdentifier"];
                this.DeviceConfig.FirmwareBrand = DeviceInfoHelper.DeviceInfoSets[this.DeviceConfig.DevicePackageName]["FirmwareBrand"];
                this.DeviceConfig.FirmwareFingerprint = DeviceInfoHelper.DeviceInfoSets[this.DeviceConfig.DevicePackageName]["FirmwareFingerprint"];
                this.DeviceConfig.FirmwareTags = DeviceInfoHelper.DeviceInfoSets[this.DeviceConfig.DevicePackageName]["FirmwareTags"];
                this.DeviceConfig.FirmwareType = DeviceInfoHelper.DeviceInfoSets[this.DeviceConfig.DevicePackageName]["FirmwareType"];
                this.DeviceConfig.HardwareManufacturer = DeviceInfoHelper.DeviceInfoSets[this.DeviceConfig.DevicePackageName]["HardwareManufacturer"];
                this.DeviceConfig.HardwareModel = DeviceInfoHelper.DeviceInfoSets[this.DeviceConfig.DevicePackageName]["HardwareModel"];
            }
            else
            {
                throw new ArgumentException("Invalid device info package! Check your auth.config file and make sure a valid DevicePackageName is set. For simple use set it to 'random'. If you have a custom device, then set it to 'custom'.");
            }
        }

        private WebProxy InitProxy()
        {
            if (!ProxyConfig.UseProxy) return null;

            WebProxy prox = new WebProxy(new System.Uri($"http://{ProxyConfig.UseProxyHost}:{ProxyConfig.UseProxyPort}"), false, null);

            if (ProxyConfig.UseProxyAuthentication)
                prox.Credentials = new NetworkCredential(ProxyConfig.UseProxyUsername, ProxyConfig.UseProxyPassword);

            return prox;
        }
    }
    
    public class ConsoleConfig
    {
        public string TranslationLanguageCode = "en";
        public bool StartupWelcomeDelay;
        public int AmountOfPokemonToDisplayOnStart = 10;
        public bool DetailedCountsBeforeRecycling = true;
    }

    public class UpdateConfig
    {
        public bool CheckForUpdates = true;
        public bool AutoUpdate = true;
        public bool TransferConfigAndAuthOnUpdate = true;
    }

    public class WebsocketsConfig
    {
        public bool UseWebsocket;
        public int WebSocketPort = 14251;
    }

    public class LocationConfig
    {
        public bool DisableHumanWalking;
        public double DefaultLatitude = 40.785092;
        public double DefaultLongitude = -73.968286;
        public double WalkingSpeedInKilometerPerHour = 4.16;
        public bool UseWalkingSpeedVariant = true;
        public double WalkingSpeedVariant = 1.2;
        public bool ShowVariantWalking = false;
        public bool RandomlyPauseAtStops = true;
        public int MaxSpawnLocationOffset = 10;
        public int MaxTravelDistanceInMeters = 1000;
    }

    public class TelegramConfig
    {
        public bool UseTelegramAPI;
        public string TelegramAPIKey;
        public string TelegramPassword = "12345";
    }

    public class GPXConfig
    {
        public bool UseGpxPathing;
        public string GpxFile = "GPXPath.GPX";
    }

    public class SnipeConfig
    {
        public bool UseSnipeLocationServer;
        public string SnipeLocationServer = "localhost";
        public int SnipeLocationServerPort = 16969;
        public bool GetSniperInfoFromPokezz;
        public bool GetOnlyVerifiedSniperInfoFromPokezz = true;
        public bool GetSniperInfoFromPokeSnipers = true;
        public bool GetSniperInfoFromPokeWatchers = true;
        public bool GetSniperInfoFromSkiplagged = true;
        public int MinPokeballsToSnipe = 20;
        public int MinPokeballsWhileSnipe = 0;
        public int MinDelayBetweenSnipes = 60000;
        public double SnipingScanOffset = 0.005;
        public bool SnipeAtPokestops;
        public bool SnipeIgnoreUnknownIv;
        public bool UseTransferIvForSnipe;
        public bool SnipePokemonNotInPokedex;
    }

    public class PokemonConfig
    {
        /*Catch*/
        public bool CatchPokemon = true;
        public int DelayBetweenPokemonCatch = 2000;
        /*Incense*/
        public bool UseIncenseConstantly;
        /*Egg*/
        public bool UseEggIncubators = true;
        public int UseEggIncubatorMinKm = 2;
        public bool UseLuckyEggConstantly;
        public int UseLuckyEggsMinPokemonAmount = 30;
        public bool UseLuckyEggsWhileEvolving;
        /*Berries*/
        public int UseBerriesMinCp = 1000;
        public float UseBerriesMinIv = 90;
        public double UseBerriesBelowCatchProbability = 0.20;
        public string UseBerriesOperator = "or";
        public int MaxBerriesToUsePerPokemon = 3;
        /*Transfer*/
        public bool TransferWeakPokemon;
        public bool TransferDuplicatePokemon = true;
        public bool TransferDuplicatePokemonOnCapture = true;
        /*Rename*/
        public bool RenamePokemon;
        public bool RenameOnlyAboveIv = true;
        public string RenameTemplate = "{1}_{0}";
        /*Favorite*/
        public float FavoriteMinIvPercentage = 95;
        public bool AutoFavoritePokemon;
        /*PokeBalls*/
        public int MaxPokeballsPerPokemon = 6;
        public int UseGreatBallAboveCp = 1000;
        public int UseUltraBallAboveCp = 1250;
        public int UseMasterBallAboveCp = 1500;
        public double UseGreatBallAboveIv = 85.0;
        public double UseUltraBallAboveIv = 95.0;
        public double UseGreatBallBelowCatchProbability = 0.2;
        public double UseUltraBallBelowCatchProbability = 0.1;
        public double UseMasterBallBelowCatchProbability = 0.05;
        /*PoweUp*/
        public bool AutomaticallyLevelUpPokemon;
        public bool OnlyUpgradeFavorites = true;
        public bool UseLevelUpList = true;
        public int AmountOfTimesToUpgradeLoop = 5;
        public int GetMinStarDustForLevelUp = 5000;
        public string LevelUpByCPorIv = "iv";
        public float UpgradePokemonCpMinimum = 1000;
        public float UpgradePokemonIvMinimum = 95;
        public string UpgradePokemonMinimumStatsOperator = "and";
        /*Evolve*/
        public float EvolveAboveIvValue = 95;
        public bool EvolveAllPokemonAboveIv;
        public bool EvolveAllPokemonWithEnoughCandy = true;
        public double EvolveKeptPokemonsAtStorageUsagePercentage = 90.0;
        /*Keep*/
        public bool KeepPokemonsThatCanEvolve;
        public int KeepMinCp = 1250;
        public float KeepMinIvPercentage = 90;
        public int KeepMinLvl = 6;
        public string KeepMinOperator = "or";
        public bool UseKeepMinLvl;
        public bool PrioritizeIvOverCp;
        public int KeepMinDuplicatePokemon = 1;
        /*NotCatch*/
        public bool UsePokemonToNotCatchFilter;
        public bool UsePokemonSniperFilterOnly;
        /*Dump Stats*/
        public bool DumpPokemonStats;
    }

    public class RecycleConfig
    {
        public bool VerboseRecycling = true;
        public double RecycleInventoryAtUsagePercentage = 90.0;
        public bool RandomizeRecycle;
        public int RandomRecycleValue = 5;
        public bool DelayBetweenRecycleActions;
        /*Amounts*/
        public int TotalAmountOfPokeballsToKeep = 120;
        public int TotalAmountOfPotionsToKeep = 80;
        public int TotalAmountOfRevivesToKeep = 60;
        public int TotalAmountOfBerriesToKeep = 50;
    }

    public class CustomCatchConfig
    {
        public bool EnableHumanizedThrows = true;
        public bool EnableMissedThrows;
        public int ThrowMissPercentage = 25;
        public int NiceThrowChance = 40;
        public int GreatThrowChance = 30;
        public int ExcellentThrowChance = 10;
        public int CurveThrowChance = 90;
        public double ForceGreatThrowOverIv = 90.00;
        public double ForceExcellentThrowOverIv = 95.00;
        public int ForceGreatThrowOverCp = 1000;
        public int ForceExcellentThrowOverCp = 1500;
    }

    public class PlayerConfig
    {
        public int DelayBetweenPlayerActions = 5000;
    }

    public class SoftBanConfig
    {
        public bool FastSoftBanBypass;
        public bool UseKillSwitchCatch = true;
        public int CatchErrorPerHours = 40;
        public int CatchEscapePerHours = 40;
        public int CatchFleePerHours = 40;
        public int CatchMissedPerHours = 40;
        public int CatchSuccessPerHours = 40;
        public bool UseKillSwitchPokestops = true;
        public int AmountPokestops = 80;
    }

    public class GlobalSettings
    {
        [JsonIgnore]
        public AuthSettings Auth = new AuthSettings();
        [JsonIgnore]
        public string GeneralConfigPath;
        [JsonIgnore]
        public string ProfileConfigPath;
        [JsonIgnore]
        public string ProfilePath;
        
        public ConsoleConfig ConsoleSettings = new ConsoleConfig();
        public UpdateConfig UpdateSettings = new UpdateConfig();
        public WebsocketsConfig WebsocketsSettings = new WebsocketsConfig();
        public LocationConfig LocationSettings = new LocationConfig();
        public TelegramConfig TelegramSettings = new TelegramConfig();
        public GPXConfig GPXSettings = new GPXConfig();
        public SnipeConfig SnipingSettings = new SnipeConfig();
        public PokemonConfig PokemonSettings = new PokemonConfig();
        public RecycleConfig RecycleSettings = new RecycleConfig();
        public CustomCatchConfig CustomCatchSettings = new CustomCatchConfig();
        public PlayerConfig PlayerSettings = new PlayerConfig();
        public SoftBanConfig SoftBanSettings = new SoftBanConfig();

        public List<KeyValuePair<ItemId, int>> ItemRecycleFilter = new List<KeyValuePair<ItemId, int>>
        {
            new KeyValuePair<ItemId, int>(ItemId.ItemUnknown, 0),
            new KeyValuePair<ItemId, int>(ItemId.ItemLuckyEgg, 200),
            new KeyValuePair<ItemId, int>(ItemId.ItemIncenseOrdinary, 100),
            new KeyValuePair<ItemId, int>(ItemId.ItemIncenseSpicy, 100),
            new KeyValuePair<ItemId, int>(ItemId.ItemIncenseCool, 100),
            new KeyValuePair<ItemId, int>(ItemId.ItemIncenseFloral, 100),
            new KeyValuePair<ItemId, int>(ItemId.ItemTroyDisk, 100),
            new KeyValuePair<ItemId, int>(ItemId.ItemXAttack, 100),
            new KeyValuePair<ItemId, int>(ItemId.ItemXDefense, 100),
            new KeyValuePair<ItemId, int>(ItemId.ItemXMiracle, 100),
            new KeyValuePair<ItemId, int>(ItemId.ItemSpecialCamera, 100),
            new KeyValuePair<ItemId, int>(ItemId.ItemIncubatorBasicUnlimited, 100),
            new KeyValuePair<ItemId, int>(ItemId.ItemIncubatorBasic, 100),
            new KeyValuePair<ItemId, int>(ItemId.ItemPokemonStorageUpgrade, 100),
            new KeyValuePair<ItemId, int>(ItemId.ItemItemStorageUpgrade, 100)
        };

        
        public List<PokemonId> PokemonsNotToTransfer = new List<PokemonId>
        {
            //criteria: from SS Tier to A Tier + Regional Exclusive
            PokemonId.Venusaur,
            PokemonId.Charizard,
            PokemonId.Blastoise,
            //PokemonId.Nidoqueen,
            //PokemonId.Nidoking,
            PokemonId.Clefable,
            //PokemonId.Vileplume,
            //PokemonId.Golduck,
            //PokemonId.Arcanine,
            //PokemonId.Poliwrath,
            //PokemonId.Machamp,
            //PokemonId.Victreebel,
            //PokemonId.Golem,
            //PokemonId.Slowbro,
            //PokemonId.Farfetchd,
            PokemonId.Muk,
            //PokemonId.Exeggutor,
            //PokemonId.Lickitung,
            PokemonId.Chansey,
            //PokemonId.Kangaskhan,
            //PokemonId.MrMime,
            //PokemonId.Tauros,
            PokemonId.Gyarados,
            //PokemonId.Lapras,
            PokemonId.Ditto,
            //PokemonId.Vaporeon,
            //PokemonId.Jolteon,
            //PokemonId.Flareon,
            //PokemonId.Porygon,
            PokemonId.Snorlax,
            PokemonId.Articuno,
            PokemonId.Zapdos,
            PokemonId.Moltres,
            PokemonId.Dragonite,
            PokemonId.Mewtwo,
            PokemonId.Mew
        };

        public List<PokemonId> PokemonsToEvolve = new List<PokemonId>
        {
            /*NOTE: keep all the end-of-line commas exept for the last one or an exception will be thrown!
            criteria: 12 candies*/
            PokemonId.Caterpie,
            PokemonId.Weedle,
            PokemonId.Pidgey,
            /*criteria: 25 candies*/
            //PokemonId.Bulbasaur,
            //PokemonId.Charmander,
            //PokemonId.Squirtle,
            PokemonId.Rattata
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
            //PokemonId.Dratini,
            /*criteria: 50 candies commons*/
            //PokemonId.Spearow,
            //PokemonId.Ekans,
            //PokemonId.Zubat,
            //PokemonId.Paras,
            //PokemonId.Venonat,
            //PokemonId.Psyduck,
            //PokemonId.Slowpoke,
            //PokemonId.Doduo,
            //PokemonId.Drowzee,
            //PokemonId.Krabby,
            //PokemonId.Horsea,
            //PokemonId.Goldeen,
            //PokemonId.Staryu
        };
        public List<PokemonId> PokemonsToLevelUp = new List<PokemonId>
        {
            //criteria: from SS Tier to A Tier + Regional Exclusive
            PokemonId.Venusaur,
            PokemonId.Charizard,
            PokemonId.Blastoise,
            //PokemonId.Nidoqueen,
            //PokemonId.Nidoking,
            PokemonId.Clefable,
            //PokemonId.Vileplume,
            //PokemonId.Golduck,
            //PokemonId.Arcanine,
            //PokemonId.Poliwrath,
            //PokemonId.Machamp,
            //PokemonId.Victreebel,
            //PokemonId.Golem,
            //PokemonId.Slowbro,
            //PokemonId.Farfetchd,
            PokemonId.Muk,
            //PokemonId.Exeggutor,
            //PokemonId.Lickitung,
            PokemonId.Chansey,
            //PokemonId.Kangaskhan,
            //PokemonId.MrMime,
            //PokemonId.Tauros,
            PokemonId.Gyarados,
            //PokemonId.Lapras,
            PokemonId.Ditto,
            //PokemonId.Vaporeon,
            //PokemonId.Jolteon,
            //PokemonId.Flareon,
            //PokemonId.Porygon,
            PokemonId.Snorlax,
            PokemonId.Articuno,
            PokemonId.Zapdos,
            PokemonId.Moltres,
            PokemonId.Dragonite,
            PokemonId.Mewtwo,
            PokemonId.Mew
        };
        public List<PokemonId> PokemonsToIgnore = new List<PokemonId>
        {
            //criteria: most common
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
            //criteria: based on NY Central Park and Tokyo variety + sniping optimization
            {PokemonId.Golduck, new TransferFilter(1800, 6, false, 95, "or", 1)},
            {PokemonId.Farfetchd, new TransferFilter(1250, 6, false, 80, "or", 1)},
            {PokemonId.Krabby, new TransferFilter(1250, 6, false, 95, "or", 1)},
            {PokemonId.Kangaskhan, new TransferFilter(1500, 6, false, 60, "or", 1)},
            {PokemonId.Horsea, new TransferFilter(1250, 6, false, 95, "or", 1)},
            {PokemonId.Staryu, new TransferFilter(1250, 6, false, 95, "or", 1)},
            {PokemonId.MrMime, new TransferFilter(1250, 6, false, 40, "or", 1)},
            {PokemonId.Scyther, new TransferFilter(1800, 6, false, 80, "or", 1)},
            {PokemonId.Jynx, new TransferFilter(1250, 6, false, 95, "or", 1)},
            {PokemonId.Electabuzz, new TransferFilter(1250, 6, false, 80, "or", 1)},
            {PokemonId.Magmar, new TransferFilter(1500, 6, false, 80, "or", 1)},
            {PokemonId.Pinsir, new TransferFilter(1800, 6, false, 95, "or", 1)},
            {PokemonId.Tauros, new TransferFilter(1250, 6, false, 90, "or", 1)},
            {PokemonId.Magikarp, new TransferFilter(200, 6, false, 95, "or", 1)},
            {PokemonId.Gyarados, new TransferFilter(1250, 6, false, 90, "or", 1)},
            {PokemonId.Lapras, new TransferFilter(1800, 6, false, 80, "or", 1)},
            {PokemonId.Eevee, new TransferFilter(1250, 6, false, 95, "or", 1)},
            {PokemonId.Vaporeon, new TransferFilter(1500, 6, false, 90, "or", 1)},
            {PokemonId.Jolteon, new TransferFilter(1500, 6, false, 90, "or", 1)},
            {PokemonId.Flareon, new TransferFilter(1500, 6, false, 90, "or", 1)},
            {PokemonId.Porygon, new TransferFilter(1250, 6, false, 60, "or", 1)},
            {PokemonId.Snorlax, new TransferFilter(2600, 6, false, 90, "or", 1)},
            {PokemonId.Dragonite, new TransferFilter(2600, 6, false, 90, "or", 1)}
        };

        public SnipeSettings PokemonToSnipe = new SnipeSettings
        {
            Locations = new List<Location>
            {
                new Location(38.55680748646112, -121.2383794784546), //Dratini Spot
                new Location(-33.85901900, 151.21309800), //Magikarp Spot
                new Location(47.5014969, -122.0959568), //Eevee Spot
                new Location(51.5025343, -0.2055027) //Charmender Spot
            },
            Pokemon = new List<PokemonId>
            {
                PokemonId.Venusaur,
                PokemonId.Charizard,
                PokemonId.Blastoise,
                PokemonId.Beedrill,
                PokemonId.Raichu,
                PokemonId.Sandslash,
                PokemonId.Nidoking,
                PokemonId.Nidoqueen,
                PokemonId.Clefable,
                PokemonId.Ninetales,
                PokemonId.Golbat,
                PokemonId.Vileplume,
                PokemonId.Golduck,
                PokemonId.Primeape,
                PokemonId.Arcanine,
                PokemonId.Poliwrath,
                PokemonId.Alakazam,
                PokemonId.Machamp,
                PokemonId.Golem,
                PokemonId.Rapidash,
                PokemonId.Slowbro,
                PokemonId.Farfetchd,
                PokemonId.Muk,
                PokemonId.Cloyster,
                PokemonId.Gengar,
                PokemonId.Exeggutor,
                PokemonId.Marowak,
                PokemonId.Hitmonchan,
                PokemonId.Lickitung,
                PokemonId.Rhydon,
                PokemonId.Chansey,
                PokemonId.Kangaskhan,
                PokemonId.Starmie,
                PokemonId.MrMime,
                PokemonId.Scyther,
                PokemonId.Magmar,
                PokemonId.Electabuzz,
                PokemonId.Jynx,
                PokemonId.Gyarados,
                PokemonId.Lapras,
                PokemonId.Ditto,
                PokemonId.Vaporeon,
                PokemonId.Jolteon,
                PokemonId.Flareon,
                PokemonId.Porygon,
                PokemonId.Kabutops,
                PokemonId.Aerodactyl,
                PokemonId.Snorlax,
                PokemonId.Articuno,
                PokemonId.Zapdos,
                PokemonId.Moltres,
                PokemonId.Dragonite,
                PokemonId.Mewtwo,
                PokemonId.Mew
            }
        };

        public List<PokemonId> PokemonToUseMasterball = new List<PokemonId>
        {
            PokemonId.Articuno,
            PokemonId.Zapdos,
            PokemonId.Moltres,
            PokemonId.Mew,
            PokemonId.Mewtwo
        };

        public GlobalSettings()
        {
            InitializePropertyDefaultValues(this);
        }

        public void InitializePropertyDefaultValues(object obj)
        {
            FieldInfo[] fields = obj.GetType().GetFields();

            foreach (FieldInfo field in fields)
            {
                var d = field.GetCustomAttribute<DefaultValueAttribute>();

                if (d != null)
                    field.SetValue(obj, d.Value);
            }
        }

        public static GlobalSettings Default => new GlobalSettings();

        public static GlobalSettings Load(string path, bool boolSkipSave = false)
        {
            GlobalSettings settings = null;
            var profilePath = Path.Combine(Directory.GetCurrentDirectory(), path);
            var profileConfigPath = Path.Combine(profilePath, "config");
            var configFile = Path.Combine(profileConfigPath, "config.json");
            var shouldExit = false;

            if (File.Exists(configFile))
            {
                try
                {
                    //if the file exists, load the settings
                    string input = "";
                    int count = 0;
                    while (true)
                    {
                        try
                        {
                            input = File.ReadAllText(configFile);
                            if (!input.Contains("DeprecatedMoves"))
                                input = input.Replace("\"Moves\"", $"\"DeprecatedMoves\"");

                            break;
                        }
                        catch (Exception exception)
                        {
                            if (count > 10)
                            {
                                //sometimes we have to wait close to config.json for access
                                Logger.Write("configFile: " + exception.Message, LogLevel.Error);
                            }
                            count++;
                            Thread.Sleep(1000);
                        }
                    };

                    var jsonSettings = new JsonSerializerSettings();
                    jsonSettings.Converters.Add(new StringEnumConverter { CamelCaseText = true });
                    jsonSettings.ObjectCreationHandling = ObjectCreationHandling.Replace;
                    jsonSettings.DefaultValueHandling = DefaultValueHandling.Populate;

                    try
                    {
                        settings = JsonConvert.DeserializeObject<GlobalSettings>(input, jsonSettings);
                    }
                    catch (Newtonsoft.Json.JsonSerializationException exception)
                    {
                        Logger.Write("JSON Exception: " + exception.Message, LogLevel.Error);
                        return null;
                    }

                    //This makes sure that existing config files dont get null values which lead to an exception
                    foreach (var filter in settings.PokemonsTransferFilter.Where(x => x.Value.KeepMinOperator == null))
                    {
                        filter.Value.KeepMinOperator = "or";
                    }
                    foreach (var filter in settings.PokemonsTransferFilter.Where(x => x.Value.Moves == null))
                    {
                        filter.Value.Moves = (filter.Value.DeprecatedMoves != null)
                                                ? new List<List<PokemonMove>> { filter.Value.DeprecatedMoves }
                                                : filter.Value.Moves ?? new List<List<PokemonMove>>();
                    }
                    foreach (var filter in settings.PokemonsTransferFilter.Where(x => x.Value.MovesOperator == null))
                    {
                        filter.Value.MovesOperator = "or";
                    }
                }
                catch (JsonReaderException exception)
                {
                    Logger.Write("JSON Exception: " + exception.Message, LogLevel.Error);
                    return null;
                }
            }
            else
            {
                settings = new GlobalSettings();
                shouldExit = true;
            }
            
            settings.ProfilePath = profilePath;
            settings.ProfileConfigPath = profileConfigPath;
            settings.GeneralConfigPath = Path.Combine(Directory.GetCurrentDirectory(), "config");

            if ( !boolSkipSave || !settings.UpdateSettings.AutoUpdate )
            {
                settings.Save(configFile);
                settings.Auth.Load(Path.Combine(profileConfigPath, "auth.json"));
            }
            
            return shouldExit ? null : settings;
        }

        public void checkProxy(ITranslation translator)
        {
            Auth.checkProxy(translator);
        }

        public static bool PromptForSetup(ITranslation translator)
        {
            Logger.Write(translator.GetTranslation(TranslationString.FirstStartPrompt, "Y", "N"), LogLevel.Warning);

            while (true)
            {
                string strInput = Console.ReadLine().ToLower();

                switch (strInput)
                {
                    case "y":
                        return true;
                    case "n":
                        Logger.Write(translator.GetTranslation(TranslationString.FirstStartAutoGenSettings));
                        return false;
                    default:
                        Logger.Write(translator.GetTranslation(TranslationString.PromptError, "Y", "N"), LogLevel.Error);
                        continue;
                }
            }
        }

        public static Session SetupSettings(Session session, GlobalSettings settings, String configPath)
        {
            Session newSession = SetupTranslationCode(session, session.Translation, settings);

            SetupAccountType(newSession.Translation, settings);
            SetupUserAccount(newSession.Translation, settings);
            SetupConfig(newSession.Translation, settings);
            SaveFiles(settings, configPath);

            Logger.Write(session.Translation.GetTranslation(TranslationString.FirstStartSetupCompleted), LogLevel.None);

            return newSession;
        }

        private static Session SetupTranslationCode(Session session, ITranslation translator, GlobalSettings settings)
        {
            Logger.Write(translator.GetTranslation(TranslationString.FirstStartLanguagePrompt, "Y", "N"), LogLevel.None);
            string strInput;

            bool boolBreak = false;
            while (!boolBreak)
            {
                strInput = Console.ReadLine().ToLower();

                switch (strInput)
                {
                    case "y":
                        boolBreak = true;
                        break;
                    case "n":
                        return session;
                    default:
                        Logger.Write(translator.GetTranslation(TranslationString.PromptError, "y", "n"), LogLevel.Error);
                        continue;
                }
            }

            Logger.Write(translator.GetTranslation(TranslationString.FirstStartLanguageCodePrompt));
            strInput = Console.ReadLine();

            settings.ConsoleSettings.TranslationLanguageCode = strInput;
            session = new Session(new ClientSettings(settings), new LogicSettings(settings));
            translator = session.Translation;
            Logger.Write(translator.GetTranslation(TranslationString.FirstStartLanguageConfirm, strInput));

            return session;
        }


        private static void SetupAccountType(ITranslation translator, GlobalSettings settings)
        {
            string strInput;
            Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupAccount), LogLevel.None);
            Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupTypePrompt, "google", "ptc"));

            while (true)
            {
                strInput = Console.ReadLine().ToLower();

                switch (strInput)
                {
                    case "google":
                        settings.Auth.AuthConfig.AuthType = AuthType.Google;
                        Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupTypeConfirm, "GOOGLE"));
                        return;
                    case "ptc":
                        settings.Auth.AuthConfig.AuthType = AuthType.Ptc;
                        Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupTypeConfirm, "PTC"));
                        return;
                    default:
                        Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupTypePromptError, "google", "ptc"), LogLevel.Error);
                        break;
                }
            }
        }

        private static void SetupUserAccount(ITranslation translator, GlobalSettings settings)
        {
            Console.WriteLine("");
            Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupUsernamePrompt), LogLevel.None);
            string strInput = Console.ReadLine();

            if (settings.Auth.AuthConfig.AuthType == AuthType.Google)
                settings.Auth.AuthConfig.GoogleUsername = strInput;
            else
                settings.Auth.AuthConfig.PtcUsername = strInput;
            Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupUsernameConfirm, strInput));

            Console.WriteLine("");
            Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupPasswordPrompt), LogLevel.None);
            strInput = Console.ReadLine();

            if (settings.Auth.AuthConfig.AuthType == AuthType.Google)
                settings.Auth.AuthConfig.GooglePassword = strInput;
            else
                settings.Auth.AuthConfig.PtcPassword = strInput;
            Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupPasswordConfirm, strInput));

            Logger.Write(translator.GetTranslation(TranslationString.FirstStartAccountCompleted), LogLevel.None);
        }

        private static void SetupConfig(ITranslation translator, GlobalSettings settings)
        {
            Logger.Write(translator.GetTranslation(TranslationString.FirstStartDefaultLocationPrompt, "Y", "N"), LogLevel.None);

            bool boolBreak = false;
            while (!boolBreak)
            {
                string strInput = Console.ReadLine().ToLower();

                switch (strInput)
                {
                    case "y":
                        boolBreak = true;
                        break;
                    case "n":
                        Logger.Write(translator.GetTranslation(TranslationString.FirstStartDefaultLocationSet));
                        return;
                    default:
                        // PROMPT ERROR \\
                        Logger.Write(translator.GetTranslation(TranslationString.PromptError, "y", "n"), LogLevel.Error);
                        continue;
                }
            }

            Logger.Write(translator.GetTranslation(TranslationString.FirstStartDefaultLocation), LogLevel.None);
            Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupDefaultLatLongPrompt));
            while (true)
            {
                try
                {
                    string strInput = Console.ReadLine();
                    string[] strSplit = strInput.Split( ',' );

                    if( strSplit.Length > 1 )
                    {
                        double dblLat = double.Parse( strSplit[ 0 ].Trim( ' ' ) );
                        double dblLong = double.Parse( strSplit[ 1 ].Trim( ' ' ) );

                        settings.LocationSettings.DefaultLatitude = dblLat;
                        settings.LocationSettings.DefaultLongitude = dblLong;

                        Logger.Write( translator.GetTranslation( TranslationString.FirstStartSetupDefaultLatLongConfirm, $"{dblLat}, {dblLong}" ) );
                    }
                    else
                    {
                        Logger.Write( translator.GetTranslation( TranslationString.FirstStartSetupDefaultLocationError, $"{settings.LocationSettings.DefaultLatitude}, {settings.LocationSettings.DefaultLongitude}", LogLevel.Error ) );
                        continue;
                    }
                    
                    break;
                }
                catch (FormatException)
                {
                    Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupDefaultLocationError, $"{settings.LocationSettings.DefaultLatitude}, {settings.LocationSettings.DefaultLongitude}", LogLevel.Error));
                    continue;
                }
            }
        }

        private static void SaveFiles(GlobalSettings settings, String configFile)
        {
            settings.Save(configFile);
            settings.Auth.Load(Path.Combine(settings.ProfileConfigPath, "auth.json"));
        }

        public void Save(string fullPath)
        {
            var output = JsonConvert.SerializeObject(this, Formatting.Indented,
                new StringEnumConverter { CamelCaseText = true });

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
        // Never spawn at the same position.
        private readonly Random _rand = new Random();
        private readonly GlobalSettings _settings;

        public ClientSettings(GlobalSettings settings)
        {
            _settings = settings;
        }


        public string GoogleUsername => _settings.Auth.AuthConfig.GoogleUsername;
        public string GooglePassword => _settings.Auth.AuthConfig.GooglePassword;

        #region Auth Config Values

        public bool UseProxy
        {
            get { return _settings.Auth.ProxyConfig.UseProxy; }
            set { _settings.Auth.ProxyConfig.UseProxy = value; }
        }

        public string UseProxyHost
        {
            get { return _settings.Auth.ProxyConfig.UseProxyHost; }
            set { _settings.Auth.ProxyConfig.UseProxyHost = value; }
        }

        public string UseProxyPort
        {
            get { return _settings.Auth.ProxyConfig.UseProxyPort; }
            set { _settings.Auth.ProxyConfig.UseProxyPort = value; }
        }

        public bool UseProxyAuthentication
        {
            get { return _settings.Auth.ProxyConfig.UseProxyAuthentication; }
            set { _settings.Auth.ProxyConfig.UseProxyAuthentication = value; }
        }

        public string UseProxyUsername
        {
            get { return _settings.Auth.ProxyConfig.UseProxyUsername; }
            set { _settings.Auth.ProxyConfig.UseProxyUsername = value; }
        }

        public string UseProxyPassword
        {
            get { return _settings.Auth.ProxyConfig.UseProxyPassword; }
            set { _settings.Auth.ProxyConfig.UseProxyPassword = value; }
        }

        public string GoogleRefreshToken
        {
            get { return null; }
            set { GoogleRefreshToken = null; }
        }
        AuthType ISettings.AuthType
        {
            get { return _settings.Auth.AuthConfig.AuthType; }

            set { _settings.Auth.AuthConfig.AuthType = value; }
        }

        string ISettings.GoogleUsername
        {
            get { return _settings.Auth.AuthConfig.GoogleUsername; }

            set { _settings.Auth.AuthConfig.GoogleUsername = value; }
        }

        string ISettings.GooglePassword
        {
            get { return _settings.Auth.AuthConfig.GooglePassword; }

            set { _settings.Auth.AuthConfig.GooglePassword = value; }
        }

        string ISettings.PtcUsername
        {
            get { return _settings.Auth.AuthConfig.PtcUsername; }

            set { _settings.Auth.AuthConfig.PtcUsername = value; }
        }

        string ISettings.PtcPassword
        {
            get { return _settings.Auth.AuthConfig.PtcPassword; }

            set { _settings.Auth.AuthConfig.PtcPassword = value; }
        }

        #endregion Auth Config Values

        #region Device Config Values

        string DevicePackageName
        {
            get { return _settings.Auth.DeviceConfig.DevicePackageName; }
            set { _settings.Auth.DeviceConfig.DevicePackageName = value; }
        }
        string ISettings.DeviceId
        {
            get { return _settings.Auth.DeviceConfig.DeviceId; }
            set { _settings.Auth.DeviceConfig.DeviceId = value; }
        }
        string ISettings.AndroidBoardName
        {
            get { return _settings.Auth.DeviceConfig.AndroidBoardName; }
            set { _settings.Auth.DeviceConfig.AndroidBoardName = value; }
        }
        string ISettings.AndroidBootloader
        {
            get { return _settings.Auth.DeviceConfig.AndroidBootloader; }
            set { _settings.Auth.DeviceConfig.AndroidBootloader = value; }
        }
        string ISettings.DeviceBrand
        {
            get { return _settings.Auth.DeviceConfig.DeviceBrand; }
            set { _settings.Auth.DeviceConfig.DeviceBrand = value; }
        }
        string ISettings.DeviceModel
        {
            get { return _settings.Auth.DeviceConfig.DeviceModel; }
            set { _settings.Auth.DeviceConfig.DeviceModel = value; }
        }
        string ISettings.DeviceModelIdentifier
        {
            get { return _settings.Auth.DeviceConfig.DeviceModelIdentifier; }
            set { _settings.Auth.DeviceConfig.DeviceModelIdentifier = value; }
        }
        string ISettings.DeviceModelBoot
        {
            get { return _settings.Auth.DeviceConfig.DeviceModelBoot; }
            set { _settings.Auth.DeviceConfig.DeviceModelBoot = value; }
        }
        string ISettings.HardwareManufacturer
        {
            get { return _settings.Auth.DeviceConfig.HardwareManufacturer; }
            set { _settings.Auth.DeviceConfig.HardwareManufacturer = value; }
        }
        string ISettings.HardwareModel
        {
            get { return _settings.Auth.DeviceConfig.HardwareModel; }
            set { _settings.Auth.DeviceConfig.HardwareModel = value; }
        }
        string ISettings.FirmwareBrand
        {
            get { return _settings.Auth.DeviceConfig.FirmwareBrand; }
            set { _settings.Auth.DeviceConfig.FirmwareBrand = value; }
        }
        string ISettings.FirmwareTags
        {
            get { return _settings.Auth.DeviceConfig.FirmwareTags; }
            set { _settings.Auth.DeviceConfig.FirmwareTags = value; }
        }
        string ISettings.FirmwareType
        {
            get { return _settings.Auth.DeviceConfig.FirmwareType; }
            set { _settings.Auth.DeviceConfig.FirmwareType = value; }
        }
        string ISettings.FirmwareFingerprint
        {
            get { return _settings.Auth.DeviceConfig.FirmwareFingerprint; }
            set { _settings.Auth.DeviceConfig.FirmwareFingerprint = value; }
        }

        #endregion Device Config Values

        double ISettings.DefaultLatitude
        {
            get
            {
                return _settings.LocationSettings.DefaultLatitude + _rand.NextDouble() * ((double)_settings.LocationSettings.MaxSpawnLocationOffset / 111111);
            }

            set { _settings.LocationSettings.DefaultLatitude = value; }
        }

        double ISettings.DefaultLongitude
        {
            get
            {
                return _settings.LocationSettings.DefaultLongitude +
                       _rand.NextDouble() *
                       ((double)_settings.LocationSettings.MaxSpawnLocationOffset / 111111 / Math.Cos(_settings.LocationSettings.DefaultLatitude));
            }

            set { _settings.LocationSettings.DefaultLongitude = value; }
        }

        double ISettings.DefaultAltitude
        {
            get
            {
                return
                    LocationUtils.getElevation(_settings.LocationSettings.DefaultLatitude, _settings.LocationSettings.DefaultLongitude) +
                    _rand.NextDouble() *
                    ((double)5 / Math.Cos(LocationUtils.getElevation(_settings.LocationSettings.DefaultLatitude, _settings.LocationSettings.DefaultLongitude)));
            }


            set { }
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
        public string ProfileConfigPath => _settings.ProfileConfigPath;
        public string GeneralConfigPath => _settings.GeneralConfigPath;
        public bool CheckForUpdates => _settings.UpdateSettings.CheckForUpdates;
        public bool AutoUpdate => _settings.UpdateSettings.AutoUpdate;
        public bool TransferConfigAndAuthOnUpdate => _settings.UpdateSettings.TransferConfigAndAuthOnUpdate;
        public bool UseWebsocket => _settings.WebsocketsSettings.UseWebsocket;
        public bool CatchPokemon => _settings.PokemonSettings.CatchPokemon;
        public bool TransferWeakPokemon => _settings.PokemonSettings.TransferWeakPokemon;
        public bool DisableHumanWalking => _settings.LocationSettings.DisableHumanWalking;
        public int MaxBerriesToUsePerPokemon => _settings.PokemonSettings.MaxBerriesToUsePerPokemon;
        public float KeepMinIvPercentage => _settings.PokemonSettings.KeepMinIvPercentage;
        public string KeepMinOperator => _settings.PokemonSettings.KeepMinOperator;
        public int KeepMinCp => _settings.PokemonSettings.KeepMinCp;
        public int KeepMinLvl => _settings.PokemonSettings.KeepMinLvl;
        public bool UseKeepMinLvl => _settings.PokemonSettings.UseKeepMinLvl;
        public bool AutomaticallyLevelUpPokemon => _settings.PokemonSettings.AutomaticallyLevelUpPokemon;
        public bool OnlyUpgradeFavorites => _settings.PokemonSettings.OnlyUpgradeFavorites;
        public bool UseLevelUpList => _settings.PokemonSettings.UseLevelUpList;
        public int AmountOfTimesToUpgradeLoop => _settings.PokemonSettings.AmountOfTimesToUpgradeLoop;
        public string LevelUpByCPorIv => _settings.PokemonSettings.LevelUpByCPorIv;
        public int GetMinStarDustForLevelUp => _settings.PokemonSettings.GetMinStarDustForLevelUp;
        public bool UseLuckyEggConstantly => _settings.PokemonSettings.UseLuckyEggConstantly;
        public bool UseIncenseConstantly => _settings.PokemonSettings.UseIncenseConstantly;
        public int UseBerriesMinCp => _settings.PokemonSettings.UseBerriesMinCp;
        public float UseBerriesMinIv => _settings.PokemonSettings.UseBerriesMinIv;
        public double UseBerriesBelowCatchProbability => _settings.PokemonSettings.UseBerriesBelowCatchProbability;
        public string UseBerriesOperator => _settings.PokemonSettings.UseBerriesOperator;
        public float UpgradePokemonIvMinimum => _settings.PokemonSettings.UpgradePokemonIvMinimum;
        public float UpgradePokemonCpMinimum => _settings.PokemonSettings.UpgradePokemonCpMinimum;
        public string UpgradePokemonMinimumStatsOperator => _settings.PokemonSettings.UpgradePokemonMinimumStatsOperator;
        public double WalkingSpeedInKilometerPerHour => _settings.LocationSettings.WalkingSpeedInKilometerPerHour;
        public bool UseWalkingSpeedVariant => _settings.LocationSettings.UseWalkingSpeedVariant;
        public double WalkingSpeedVariant => _settings.LocationSettings.WalkingSpeedVariant;
        public bool ShowVariantWalking => _settings.LocationSettings.ShowVariantWalking;
        public bool FastSoftBanBypass => _settings.SoftBanSettings.FastSoftBanBypass;
        public bool EvolveAllPokemonWithEnoughCandy => _settings.PokemonSettings.EvolveAllPokemonWithEnoughCandy;
        public bool KeepPokemonsThatCanEvolve => _settings.PokemonSettings.KeepPokemonsThatCanEvolve;
        public bool TransferDuplicatePokemon => _settings.PokemonSettings.TransferDuplicatePokemon;
        public bool TransferDuplicatePokemonOnCapture => _settings.PokemonSettings.TransferDuplicatePokemonOnCapture;
        public bool UseEggIncubators => _settings.PokemonSettings.UseEggIncubators;
        public int UseEggIncubatorMinKm => _settings.PokemonSettings.UseEggIncubatorMinKm;
        public int UseGreatBallAboveCp => _settings.PokemonSettings.UseGreatBallAboveCp;
        public int UseUltraBallAboveCp => _settings.PokemonSettings.UseUltraBallAboveCp;
        public int UseMasterBallAboveCp => _settings.PokemonSettings.UseMasterBallAboveCp;
        public double UseGreatBallAboveIv => _settings.PokemonSettings.UseGreatBallAboveIv;
        public double UseUltraBallAboveIv => _settings.PokemonSettings.UseUltraBallAboveIv;
        public double UseMasterBallBelowCatchProbability => _settings.PokemonSettings.UseMasterBallBelowCatchProbability;
        public double UseUltraBallBelowCatchProbability => _settings.PokemonSettings.UseUltraBallBelowCatchProbability;
        public double UseGreatBallBelowCatchProbability => _settings.PokemonSettings.UseGreatBallBelowCatchProbability;
        public bool EnableHumanizedThrows => _settings.CustomCatchSettings.EnableHumanizedThrows;
        public bool EnableMissedThrows => _settings.CustomCatchSettings.EnableMissedThrows;
        public int ThrowMissPercentage => _settings.CustomCatchSettings.ThrowMissPercentage;
        public int NiceThrowChance => _settings.CustomCatchSettings.NiceThrowChance;
        public int GreatThrowChance => _settings.CustomCatchSettings.GreatThrowChance;
        public int ExcellentThrowChance => _settings.CustomCatchSettings.ExcellentThrowChance;
        public int CurveThrowChance => _settings.CustomCatchSettings.CurveThrowChance;
        public double ForceGreatThrowOverIv => _settings.CustomCatchSettings.ForceGreatThrowOverIv;
        public double ForceExcellentThrowOverIv => _settings.CustomCatchSettings.ForceExcellentThrowOverIv;
        public int ForceGreatThrowOverCp => _settings.CustomCatchSettings.ForceGreatThrowOverCp;
        public int ForceExcellentThrowOverCp => _settings.CustomCatchSettings.ForceExcellentThrowOverCp;
        public int DelayBetweenPokemonCatch => _settings.PokemonSettings.DelayBetweenPokemonCatch;
        public int DelayBetweenPlayerActions => _settings.PlayerSettings.DelayBetweenPlayerActions;
        public bool UsePokemonToNotCatchFilter => _settings.PokemonSettings.UsePokemonToNotCatchFilter;
        public bool UsePokemonSniperFilterOnly => _settings.PokemonSettings.UsePokemonSniperFilterOnly;
        public int KeepMinDuplicatePokemon => _settings.PokemonSettings.KeepMinDuplicatePokemon;
        public bool PrioritizeIvOverCp => _settings.PokemonSettings.PrioritizeIvOverCp;
        public int MaxTravelDistanceInMeters => _settings.LocationSettings.MaxTravelDistanceInMeters;
        public string GpxFile => _settings.GPXSettings.GpxFile;
        public bool UseGpxPathing => _settings.GPXSettings.UseGpxPathing;
        public bool UseLuckyEggsWhileEvolving => _settings.PokemonSettings.UseLuckyEggsWhileEvolving;
        public int UseLuckyEggsMinPokemonAmount => _settings.PokemonSettings.UseLuckyEggsMinPokemonAmount;
        public bool EvolveAllPokemonAboveIv => _settings.PokemonSettings.EvolveAllPokemonAboveIv;
        public float EvolveAboveIvValue => _settings.PokemonSettings.EvolveAboveIvValue;
        public bool RenamePokemon => _settings.PokemonSettings.RenamePokemon;
        public bool RenameOnlyAboveIv => _settings.PokemonSettings.RenameOnlyAboveIv;
        public float FavoriteMinIvPercentage => _settings.PokemonSettings.FavoriteMinIvPercentage;
        public bool AutoFavoritePokemon => _settings.PokemonSettings.AutoFavoritePokemon;
        public string RenameTemplate => _settings.PokemonSettings.RenameTemplate;
        public int AmountOfPokemonToDisplayOnStart => _settings.ConsoleSettings.AmountOfPokemonToDisplayOnStart;
        public bool DumpPokemonStats => _settings.PokemonSettings.DumpPokemonStats;
        public string TranslationLanguageCode => _settings.ConsoleSettings.TranslationLanguageCode;
        public bool DetailedCountsBeforeRecycling => _settings.ConsoleSettings.DetailedCountsBeforeRecycling;
        public bool VerboseRecycling => _settings.RecycleSettings.VerboseRecycling;
        public double RecycleInventoryAtUsagePercentage => _settings.RecycleSettings.RecycleInventoryAtUsagePercentage;
        public double EvolveKeptPokemonsAtStorageUsagePercentage => _settings.PokemonSettings.EvolveKeptPokemonsAtStorageUsagePercentage;
        public bool UseKillSwitchCatch => _settings.SoftBanSettings.UseKillSwitchCatch;
        public int CatchErrorPerHours => _settings.SoftBanSettings.CatchErrorPerHours;
        public int CatchEscapePerHours => _settings.SoftBanSettings.CatchEscapePerHours;
        public int CatchFleePerHours => _settings.SoftBanSettings.CatchFleePerHours;
        public int CatchMissedPerHours => _settings.SoftBanSettings.CatchMissedPerHours;
        public int CatchSuccessPerHours => _settings.SoftBanSettings.CatchSuccessPerHours;
        public bool UseKillSwitchPokestops => _settings.SoftBanSettings.UseKillSwitchPokestops;
        public int AmountPokestops => _settings.SoftBanSettings.AmountPokestops;
        public ICollection<KeyValuePair<ItemId, int>> ItemRecycleFilter => _settings.ItemRecycleFilter;
        public ICollection<PokemonId> PokemonsToEvolve => _settings.PokemonsToEvolve;
        public ICollection<PokemonId> PokemonsToLevelUp => _settings.PokemonsToLevelUp;
        public ICollection<PokemonId> PokemonsNotToTransfer => _settings.PokemonsNotToTransfer;
        public ICollection<PokemonId> PokemonsNotToCatch => _settings.PokemonsToIgnore;

        public ICollection<PokemonId> PokemonToUseMasterball => _settings.PokemonToUseMasterball;
        public Dictionary<PokemonId, TransferFilter> PokemonsTransferFilter => _settings.PokemonsTransferFilter;
        public bool StartupWelcomeDelay => _settings.ConsoleSettings.StartupWelcomeDelay;
        public bool SnipeAtPokestops => _settings.SnipingSettings.SnipeAtPokestops;

        public bool UseTelegramAPI => _settings.TelegramSettings.UseTelegramAPI;
        public string TelegramAPIKey => _settings.TelegramSettings.TelegramAPIKey;
        public string TelegramPassword => _settings.TelegramSettings.TelegramPassword;
        public int MinPokeballsToSnipe => _settings.SnipingSettings.MinPokeballsToSnipe;
        public int MinPokeballsWhileSnipe => _settings.SnipingSettings.MinPokeballsWhileSnipe;
        public int MaxPokeballsPerPokemon => _settings.PokemonSettings.MaxPokeballsPerPokemon;
        public bool RandomlyPauseAtStops => _settings.LocationSettings.RandomlyPauseAtStops;
        public SnipeSettings PokemonToSnipe => _settings.PokemonToSnipe;
        public string SnipeLocationServer => _settings.SnipingSettings.SnipeLocationServer;
        public int SnipeLocationServerPort => _settings.SnipingSettings.SnipeLocationServerPort;
        public bool GetSniperInfoFromPokezz => _settings.SnipingSettings.GetSniperInfoFromPokezz;
        public bool GetOnlyVerifiedSniperInfoFromPokezz => _settings.SnipingSettings.GetOnlyVerifiedSniperInfoFromPokezz;
        public bool GetSniperInfoFromPokeSnipers => _settings.SnipingSettings.GetSniperInfoFromPokeSnipers;
        public bool GetSniperInfoFromPokeWatchers => _settings.SnipingSettings.GetSniperInfoFromPokeWatchers;
        public bool GetSniperInfoFromSkiplagged => _settings.SnipingSettings.GetSniperInfoFromSkiplagged;
        public bool UseSnipeLocationServer => _settings.SnipingSettings.UseSnipeLocationServer;
        public bool UseTransferIvForSnipe => _settings.SnipingSettings.UseTransferIvForSnipe;
        public bool SnipeIgnoreUnknownIv => _settings.SnipingSettings.SnipeIgnoreUnknownIv;
        public int MinDelayBetweenSnipes => _settings.SnipingSettings.MinDelayBetweenSnipes;
        public double SnipingScanOffset => _settings.SnipingSettings.SnipingScanOffset;
        public bool SnipePokemonNotInPokedex => _settings.SnipingSettings.SnipePokemonNotInPokedex;
        public bool RandomizeRecycle => _settings.RecycleSettings.RandomizeRecycle;
        public int RandomRecycleValue => _settings.RecycleSettings.RandomRecycleValue;
        public bool DelayBetweenRecycleActions => _settings.RecycleSettings.DelayBetweenRecycleActions;
        public int TotalAmountOfPokeballsToKeep => _settings.RecycleSettings.TotalAmountOfPokeballsToKeep;
        public int TotalAmountOfPotionsToKeep => _settings.RecycleSettings.TotalAmountOfPotionsToKeep;
        public int TotalAmountOfRevivesToKeep => _settings.RecycleSettings.TotalAmountOfRevivesToKeep;
        public int TotalAmountOfBerriesToKeep => _settings.RecycleSettings.TotalAmountOfBerriesToKeep;
    }
}
