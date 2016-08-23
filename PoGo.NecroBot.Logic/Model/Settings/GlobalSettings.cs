using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PoGo.NecroBot.Logic.Common;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.State;
using PokemonGo.RocketAPI.Enums;
using POGOProtos.Enums;
using POGOProtos.Inventory.Item;

namespace PoGo.NecroBot.Logic.Model.Settings
{
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

        public ConsoleConfig ConsoleConfig = new ConsoleConfig();
        public UpdateConfig UpdateConfig = new UpdateConfig();
        public WebsocketsConfig WebsocketsConfig = new WebsocketsConfig();
        public LocationConfig LocationConfig = new LocationConfig();
        public TelegramConfig TelegramConfig = new TelegramConfig();
        public GpxConfig GPXConfig = new GpxConfig();
        public SnipeConfig SnipeConfig = new SnipeConfig();
        public PokeStopConfig PokeStopConfig = new PokeStopConfig();
        public PokemonConfig PokemonConfig = new PokemonConfig();
        public RecycleConfig RecycleConfig = new RecycleConfig();
        public CustomCatchConfig CustomCatchConfig = new CustomCatchConfig();
        public PlayerConfig PlayerConfig = new PlayerConfig();
        public SoftBanConfig SoftBanConfig = new SoftBanConfig();
        public GoogleWalkConfig GoogleWalkConfig = new GoogleWalkConfig();

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
            {PokemonId.Golduck, new TransferFilter(1800, 6, false, 95, "or", 1,new List<List<PokemonMove>>() { new List<PokemonMove>() { PokemonMove.WaterGunFast,PokemonMove.HydroPump }},null,"and")},
            {PokemonId.Aerodactyl, new TransferFilter(1250, 6, false, 80, "or", 1,new List<List<PokemonMove>>() { new List<PokemonMove>() { PokemonMove.BiteFast,PokemonMove.HyperBeam }},null,"and")},
            {PokemonId.Venusaur, new TransferFilter(1800, 6, false, 95, "or", 1,new List<List<PokemonMove>>() { new List<PokemonMove>() { PokemonMove.VineWhipFast,PokemonMove.SolarBeam }},null,"and")},
            {PokemonId.Farfetchd, new TransferFilter(1250, 6, false, 80, "or", 1)},
            {PokemonId.Krabby, new TransferFilter(1250, 6, false, 95, "or", 1)},
            {PokemonId.Kangaskhan, new TransferFilter(1500, 6, false, 60, "or", 1)},
            {PokemonId.Horsea, new TransferFilter(1250, 6, false, 95, "or", 1)},
            {PokemonId.Staryu, new TransferFilter(1250, 6, false, 95, "or", 1)},
            {PokemonId.MrMime, new TransferFilter(1250, 6, false, 40, "or", 1)},
            {PokemonId.Scyther, new TransferFilter(1800, 6, false, 80, "or", 1)},
            {PokemonId.Jynx, new TransferFilter(1250, 6, false, 95, "or", 1)},
            {PokemonId.Charizard, new TransferFilter(1250, 6, false, 80, "or", 1,new List<List<PokemonMove>>() { new List<PokemonMove>() { PokemonMove.WingAttackFast,PokemonMove.FireBlast }},null,"and")},
            {PokemonId.Electabuzz, new TransferFilter(1250, 6, false, 80, "or", 1,new List<List<PokemonMove>>() { new List<PokemonMove>() { PokemonMove.ThunderShockFast,PokemonMove.Thunder }},null,"and")},
            {PokemonId.Magmar, new TransferFilter(1500, 6, false, 80, "or", 1)},
            {PokemonId.Pinsir, new TransferFilter(1800, 6, false, 95, "or", 1,new List<List<PokemonMove>>() { new List<PokemonMove>() { PokemonMove.RockSmashFast,PokemonMove.XScissor }},null,"and")},
            {PokemonId.Tauros, new TransferFilter(1250, 6, false, 90, "or", 1)},
            {PokemonId.Magikarp, new TransferFilter(200, 6, false, 95, "or", 1)},
            {PokemonId.Exeggutor, new TransferFilter(1800, 6, false, 90, "or", 1,new List<List<PokemonMove>>() { new List<PokemonMove>() { PokemonMove.ZenHeadbuttFast,PokemonMove.SolarBeam }},null,"and")},
            {PokemonId.Gyarados, new TransferFilter(1250, 6, false, 90, "or", 1,new List<List<PokemonMove>>() { new List<PokemonMove>() { PokemonMove.DragonBreath,PokemonMove.HydroPump }},null,"and")},
            {PokemonId.Lapras, new TransferFilter(1800, 6, false, 80, "or", 1,new List<List<PokemonMove>>() { new List<PokemonMove>() { PokemonMove.FrostBreathFast,PokemonMove.Blizzard }},null,"and")},
            {PokemonId.Eevee, new TransferFilter(1250, 6, false, 95, "or", 1)},
            {PokemonId.Vaporeon, new TransferFilter(1500, 6, false, 90, "or", 1,new List<List<PokemonMove>>() { new List<PokemonMove>() { PokemonMove.WaterGun,PokemonMove.HydroPump }},null,"and")},
            {PokemonId.Jolteon, new TransferFilter(1500, 6, false, 90, "or", 1)},
            {PokemonId.Flareon, new TransferFilter(1500, 6, false, 90, "or", 1,new List<List<PokemonMove>>() { new List<PokemonMove>() { PokemonMove.Ember,PokemonMove.FireBlast }},null,"and")},
            {PokemonId.Porygon, new TransferFilter(1250, 6, false, 60, "or", 1)},
            {PokemonId.Arcanine, new TransferFilter(1800, 6, false, 80, "or", 1,new List<List<PokemonMove>>() { new List<PokemonMove>() { PokemonMove.FireFangFast,PokemonMove.FireBlast }},null,"and")},
            {PokemonId.Snorlax, new TransferFilter(2600, 6, false, 90, "or", 1,new List<List<PokemonMove>>() { new List<PokemonMove>() { PokemonMove.ZenHeadbuttFast,PokemonMove.HyperBeam }},null,"and")},
            {PokemonId.Dragonite, new TransferFilter(2600, 6, false, 90, "or", 1,new List<List<PokemonMove>>() { new List<PokemonMove>() { PokemonMove.DragonBreath,PokemonMove.DragonClaw }},null,"and")},
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
                //PokemonId.Farfetchd,
                PokemonId.Muk,
                PokemonId.Cloyster,
                PokemonId.Gengar,
                PokemonId.Exeggutor,
                PokemonId.Marowak,
                PokemonId.Hitmonchan,
                PokemonId.Lickitung,
                PokemonId.Rhydon,
                PokemonId.Chansey,
                //PokemonId.Kangaskhan,
                PokemonId.Starmie,
                //PokemonId.MrMime,
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

            if (!boolSkipSave || !settings.UpdateConfig.AutoUpdate)
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

            settings.ConsoleConfig.TranslationLanguageCode = strInput;
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
}