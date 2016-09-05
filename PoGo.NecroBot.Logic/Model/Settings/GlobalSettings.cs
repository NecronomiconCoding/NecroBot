using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using PoGo.NecroBot.Logic.Common;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.State;
using PokemonGo.RocketAPI.Enums;
using POGOProtos.Enums;

namespace PoGo.NecroBot.Logic.Model.Settings
{
    [JsonObject(Title = " Global Settings", Description = "Set your global settings.", ItemRequired = Required.DisallowNull)]
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

        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ConsoleConfig ConsoleConfig = new ConsoleConfig();

        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public UpdateConfig UpdateConfig = new UpdateConfig();

        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public WebsocketsConfig WebsocketsConfig = new WebsocketsConfig();

        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public LocationConfig LocationConfig = new LocationConfig();

        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public TelegramConfig TelegramConfig = new TelegramConfig();

        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public GpxConfig GPXConfig = new GpxConfig();

        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public SnipeConfig SnipeConfig = new SnipeConfig();

        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public HumanWalkSnipeConfig HumanWalkSnipeConfig = new HumanWalkSnipeConfig();

        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public PokeStopConfig PokeStopConfig = new PokeStopConfig();

        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public PokemonConfig PokemonConfig = new PokemonConfig();

        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ItemRecycleConfig RecycleConfig = new ItemRecycleConfig();

        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public CustomCatchConfig CustomCatchConfig = new CustomCatchConfig();

        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public PlayerConfig PlayerConfig = new PlayerConfig();

        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public SoftBanConfig SoftBanConfig = new SoftBanConfig();

        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public GoogleWalkConfig GoogleWalkConfig = new GoogleWalkConfig();

        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public YoursWalkConfig YoursWalkConfig = new YoursWalkConfig();

        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<ItemRecycleFilter> ItemRecycleFilter = Settings.ItemRecycleFilter.ItemRecycleFilterDefault();

        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<PokemonId> PokemonsNotToTransfer = TransferConfig.PokemonsNotToTransferDefault();

        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<PokemonId> PokemonsToEvolve = EvolveConfig.PokemonsToEvolveDefault();

        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<PokemonId> PokemonsToLevelUp = LevelUpConfig.PokemonsToLevelUpDefault();

        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<PokemonId> PokemonsToIgnore = CatchConfig.PokemonsToIgnoreDefault();

        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Dictionary<PokemonId, TransferFilter> PokemonsTransferFilter = TransferFilter.TransferFilterDefault();

        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public SnipeSettings PokemonToSnipe = SnipeSettings.Default();

        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<PokemonId> PokemonToUseMasterball = CatchConfig.PokemonsToUseMasterballDefault();

        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Dictionary<PokemonId, HumanWalkSnipeFilter> HumanWalkSnipeFilters = HumanWalkSnipeFilter.Default();

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

        public Dictionary<PokemonId, UpgradeFilter> PokemonUpgradeFilters = UpgradeFilter.Default();

        private static JSchema _schema;
        private static JSchema JsonSchema
        {
            get
            {
                if (_schema != null)
                    return _schema;
                // JSON Schemas from .NET types
                var generator = new JSchemaGenerator
                {
                    // change contract resolver so property names are camel case
                    //ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    // types with no defined ID have their type name as the ID
                    SchemaIdGenerationHandling = SchemaIdGenerationHandling.TypeName,
                    // use the default order of properties.
                    SchemaPropertyOrderHandling = SchemaPropertyOrderHandling.Default,
                    // referenced schemas are inline.
                    SchemaLocationHandling = SchemaLocationHandling.Inline,
                    // no schemas can be referenced.    
                    SchemaReferenceHandling = SchemaReferenceHandling.None
                };
                // change Zone enum to generate a string property
                var strEnumGen = new StringEnumGenerationProvider { CamelCaseText = true };
                generator.GenerationProviders.Add(strEnumGen);
                // generate json schema 
                var type = typeof(GlobalSettings);
                var schema = generator.Generate(type);
                schema.Title = type.Name;
                //
                _schema = schema;
                return _schema;
            }
        }

        //private JObject _jsonObject;
        //public JObject JsonObject
        //{
        //    get
        //    {
        //        if (_jsonObject == null)
        //            _jsonObject = JObject.FromObject(this);

        //        return _jsonObject;
        //    }
        //    set
        //    {
        //        _jsonObject = value;
        //    }
        //}


        //public void Load(JObject jsonObj)
        //{
        //    try
        //    {
        //        var input = jsonObj.ToString(Formatting.None, new StringEnumConverter { CamelCaseText = true });
        //        var settings = new JsonSerializerSettings();
        //        settings.Converters.Add(new StringEnumConverter { CamelCaseText = true });
        //        JsonConvert.PopulateObject(input, this, settings);
        //        var configFile = Path.Combine(ProfileConfigPath, "config.json");
        //        this.Save(configFile);

        //    }
        //    catch (JsonReaderException exception)
        //    {
        //            Logger.Write("JSON Exception: " + exception.Message, LogLevel.Error);
        //    }
        //}

        public static GlobalSettings Load(string path, bool boolSkipSave = false, bool validate = true)
        {
            GlobalSettings settings;
            var profilePath = Path.Combine(Directory.GetCurrentDirectory(), path);
            var profileConfigPath = Path.Combine(profilePath, "config");
            var configFile = Path.Combine(profileConfigPath, "config.json");
            var shouldExit = false;

            if (File.Exists(configFile))
            {
                try
                {
                    //if the file exists, load the settings
                    string input;
                    int count = 0;
                    while (true)
                    {
                        try
                        {
                            input = File.ReadAllText(configFile, Encoding.UTF8);
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
                    }

                    var jsonSettings = new JsonSerializerSettings();
                    jsonSettings.Converters.Add(new StringEnumConverter { CamelCaseText = true });
                    jsonSettings.ObjectCreationHandling = ObjectCreationHandling.Replace;
                    jsonSettings.DefaultValueHandling = DefaultValueHandling.Populate;

                    try
                    {
                        // validate Json using JsonSchema
                        if (validate)
                        {
                            Logger.Write("Validating config.json...");
                            var jsonObj = JObject.Parse(input);
                            IList<ValidationError> errors;
                            var valid = jsonObj.IsValid(JsonSchema, out errors);
                            if (!valid)
                            {
                                foreach (var error in errors)
                                {
                                    Logger.Write(
                                        "config.json [Line: " + error.LineNumber + ", Position: " + error.LinePosition + "]: " +
                                        error.Path + " " +
                                        error.Message, LogLevel.Error);
                                }
                                Logger.Write("Fix config.json and restart NecroBot or press a key to ignore and continue...",
                                    LogLevel.Warning);
                                Console.ReadKey();
                            }
                        }

                        settings = JsonConvert.DeserializeObject<GlobalSettings>(input, jsonSettings);
                    }
                    catch (JsonSerializationException exception)
                    {
                        Logger.Write("JSON Exception: " + exception.Message, LogLevel.Error);
                        return null;
                    }
                    catch (JsonReaderException exception)
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

            if (!boolSkipSave || !settings.UpdateConfig.AutoUpdate)
            {
                settings.Save(configFile);
                settings.Auth.Load(Path.Combine(profileConfigPath, "auth.json"), boolSkipSave, validate);
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
            SetupWalkingConfig(newSession.Translation, settings);
            SetupTelegramConfig(newSession.Translation, settings);
            SetupProxyConfig(newSession.Translation, settings);
            SetupAutoCompleteTutConfig(newSession.Translation, settings);
            SetupWebSocketConfig(newSession.Translation, settings);
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

            settings.ConsoleConfig.TranslationLanguageCode = strInput;
            session = new Session(new ClientSettings(settings), new LogicSettings(settings));
            translator = session.Translation;
            Logger.Write(translator.GetTranslation(TranslationString.FirstStartLanguageConfirm, strInput));

            return session;
        }

        private static void SetupProxyConfig(ITranslation translator, GlobalSettings settings)
        {
            Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupProxyPrompt, "Y", "N"), LogLevel.None);
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
                        return;
                    default:
                        Logger.Write(translator.GetTranslation(TranslationString.PromptError, "y", "n"), LogLevel.Error);
                        continue;
                }
            }

            settings.Auth.ProxyConfig.UseProxy = true;

            Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupProxyHostPrompt));
            strInput = Console.ReadLine();
            settings.Auth.ProxyConfig.UseProxyHost = strInput;
            Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupProxyHostConfirm, strInput));

            Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupProxyPortPrompt));
            strInput = Console.ReadLine();
            settings.Auth.ProxyConfig.UseProxyPort = strInput;
            Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupProxyPortConfirm, strInput));

            Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupProxyAuthPrompt, "Y", "N"), LogLevel.None);

            boolBreak = false;
            while (!boolBreak)
            {
                strInput = Console.ReadLine().ToLower();

                switch (strInput)
                {
                    case "y":
                        boolBreak = true;
                        break;
                    case "n":
                        return;
                    default:
                        Logger.Write(translator.GetTranslation(TranslationString.PromptError, "y", "n"), LogLevel.Error);
                        continue;
                }
            }

            settings.Auth.ProxyConfig.UseProxyAuthentication = true;

            Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupProxyUsernamePrompt));
            strInput = Console.ReadLine();
            settings.Auth.ProxyConfig.UseProxyUsername = strInput;
            Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupProxyUsernameConfirm, strInput));
            Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupProxyPasswordPrompt));
            strInput = Console.ReadLine();
            settings.Auth.ProxyConfig.UseProxyPassword = strInput;
            Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupProxyPasswordConfirm, strInput));
        }

        private static void SetupWalkingConfig(ITranslation translator, GlobalSettings settings)
        {
            Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupWalkingSpeedPrompt, "Y", "N"), LogLevel.None);
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
                        return;
                    default:
                        Logger.Write(translator.GetTranslation(TranslationString.PromptError, "y", "n"), LogLevel.Error);
                        continue;
                }
            }

            Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupWalkingSpeedKmHPrompt));
            strInput = Console.ReadLine();
            settings.LocationConfig.WalkingSpeedInKilometerPerHour = Double.Parse(strInput);
            Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupWalkingSpeedKmHConfirm, strInput));
            Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupUseWalkingSpeedVariantPrompt, "Y", "N"), LogLevel.None);

            boolBreak = false;
            while (!boolBreak)
            {
                strInput = Console.ReadLine().ToLower();

                switch (strInput)
                {
                    case "y":
                        boolBreak = true;
                        settings.LocationConfig.UseWalkingSpeedVariant = true;
                        break;
                    case "n":
                        settings.LocationConfig.UseWalkingSpeedVariant = false;
                        return;
                    default:
                        Logger.Write(translator.GetTranslation(TranslationString.PromptError, "y", "n"), LogLevel.Error);
                        continue;
                }
            }

            Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupWalkingSpeedVariantPrompt));
            strInput = Console.ReadLine();
            settings.LocationConfig.WalkingSpeedVariant = Double.Parse(strInput);
            Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupWalkingSpeedVariantConfirm, strInput));

        }

        private static void SetupAutoCompleteTutConfig(ITranslation translator, GlobalSettings settings)
        {
            Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupAutoCompleteTutPrompt, "Y", "N"), LogLevel.None);
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
                        return;
                    default:
                        Logger.Write(translator.GetTranslation(TranslationString.PromptError, "y", "n"), LogLevel.Error);
                        continue;
                }
            }

            settings.PlayerConfig.AutoCompleteTutorial = true;

            Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupAutoCompleteTutNicknamePrompt));
            strInput = Console.ReadLine();
            settings.PlayerConfig.DesiredNickname = strInput;
            Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupAutoCompleteTutNicknameConfirm, strInput));

            Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupAutoCompleteTutGenderPrompt));
            strInput = Console.ReadLine();
            settings.PlayerConfig.DesiredGender = strInput;
            Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupAutoCompleteTutGenderConfirm, strInput));

            Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupAutoCompleteTutStarterPrompt));
            strInput = Console.ReadLine();
            settings.PlayerConfig.DesiredStarter = strInput;
            Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupAutoCompleteTutStarterConfirm, strInput));
        }

        private static void SetupWebSocketConfig(ITranslation translator, GlobalSettings settings)
        {
            Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupWebSocketPrompt, "Y", "N"), LogLevel.None);
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
                        return;
                    default:
                        Logger.Write(translator.GetTranslation(TranslationString.PromptError, "y", "n"), LogLevel.Error);
                        continue;
                }
            }

            settings.WebsocketsConfig.UseWebsocket = true;

            Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupWebSocketPortPrompt));
            strInput = Console.ReadLine();
            settings.WebsocketsConfig.WebSocketPort = int.Parse(strInput);
            Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupWebSocketPortConfirm, strInput));
        }

        private static void SetupTelegramConfig(ITranslation translator, GlobalSettings settings)
        {
            Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupTelegramPrompt, "Y", "N"), LogLevel.None);
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
                        return;
                    default:
                        Logger.Write(translator.GetTranslation(TranslationString.PromptError, "y", "n"), LogLevel.Error);
                        continue;
                }
            }

            settings.TelegramConfig.UseTelegramAPI = true;

            Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupTelegramCodePrompt));
            strInput = Console.ReadLine();
            settings.TelegramConfig.TelegramAPIKey = strInput;
            Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupTelegramCodeConfirm, strInput));

            Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupTelegramPasswordPrompt));
            strInput = Console.ReadLine();
            settings.TelegramConfig.TelegramPassword = strInput;
            Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupTelegramPasswordConfirm, strInput));
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
                    string[] strSplit = strInput.Split(',');

                    if (strSplit.Length > 1)
                    {
                        double dblLat = double.Parse(strSplit[0].Trim(' '));
                        double dblLong = double.Parse(strSplit[1].Trim(' '));

                        settings.LocationConfig.DefaultLatitude = dblLat;
                        settings.LocationConfig.DefaultLongitude = dblLong;

                        Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupDefaultLatLongConfirm, $"{dblLat}, {dblLong}"));
                    }
                    else
                    {
                        Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupDefaultLocationError, $"{settings.LocationConfig.DefaultLatitude}, {settings.LocationConfig.DefaultLongitude}", LogLevel.Error));
                        continue;
                    }

                    break;
                }
                catch (FormatException)
                {
                    Logger.Write(translator.GetTranslation(TranslationString.FirstStartSetupDefaultLocationError, $"{settings.LocationConfig.DefaultLatitude}, {settings.LocationConfig.DefaultLongitude}", LogLevel.Error));
                }
            }
        }

        private static void SaveFiles(GlobalSettings settings, String configFile)
        {
            settings.Save(configFile);
            settings.Auth.Load(Path.Combine(settings.ProfileConfigPath, "auth.json"));
        }

        public void Save(string fullPath, bool validate = false)
        {
            var output = JsonConvert.SerializeObject(this, Formatting.Indented,
                new StringEnumConverter { CamelCaseText = true });

            var folder = Path.GetDirectoryName(fullPath);
            if (folder != null && !Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            File.WriteAllText(fullPath, output, Encoding.UTF8);

            //JsonSchema
            File.WriteAllText(fullPath.Replace(".json", ".schema.json"), JsonSchema.ToString(), Encoding.UTF8);

            if (!validate) return;

            // validate Json using JsonSchema
            Logger.Write("Validating config.json...");
            var jsonObj = JObject.Parse(output);
            IList<ValidationError> errors;
            var valid = jsonObj.IsValid(JsonSchema, out errors);
            if (valid) return;
            foreach (var error in errors)
            {
                Logger.Write(
                    "config.json [Line: " + error.LineNumber + ", Position: " + error.LinePosition + "]: " + error.Path + " " +
                    error.Message, LogLevel.Error);
                //"Default value is '" + error.Schema.Default + "'"
            }
            Logger.Write("Fix config.json and restart NecroBot or press a key to ignore and continue...", LogLevel.Warning);
            Console.ReadKey();
        }
    }
}
