using System;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;

using Newtonsoft.Json;
using POGOProtos.Enums;
using POGOProtos.Inventory.Item;
using PoGo.NecroBot.Logic.Filters;

namespace PoGo.NecroBot.Logic.Settings {
    public class ConfigurationSettings : IConfigurationSettings, INotifyPropertyChanged {
        #region "Default values"
        [JsonIgnore]
        private string _GpxFile = "GPXPath.GPX";
        [JsonIgnore]
        private string _TranslationLanguageCode = "en";
        [JsonIgnore]
        private bool _AutoUpdate = true;
        [JsonIgnore]
        private bool _EvolveAllPokemonAboveIv = false;
        [JsonIgnore]
        private bool _EvolveAllPokemonWithEnoughCandy = true;
        [JsonIgnore]
        private bool _DumpPokemonStats = false;
        [JsonIgnore]
        private bool _PrioritizeIvOverCp = true;
        [JsonIgnore]
        private bool _RenameAboveIv = true;
        [JsonIgnore]
        private bool _TransferDuplicatePokemon = true;
        [JsonIgnore]
        private bool _UseEggIncubators = true;
        [JsonIgnore]
        private bool _UseGpxPathing = false;
        [JsonIgnore]
        private bool _UseLuckyEggsWhileEvolving = false;
        [JsonIgnore]
        private bool _UsePokemonToNotCatchFilter = false;
        [JsonIgnore]
        private bool _StartupWelcomeDelay = true;
        [JsonIgnore]
        private int _WebSocketPort = 14251;
        [JsonIgnore]
        private int _DelayBetweenPokemonCatch = 2000;
        [JsonIgnore]
        private int _UseLuckyEggsMinPokemonAmount = 30;
        [JsonIgnore]
        private double _DefaultAltitude = 10;
        [JsonIgnore]
        private double _DefaultLatitude = 40.785091;
        [JsonIgnore]
        private double _DefaultLongitude = -73.968285;
        [JsonIgnore]
        private double _WalkingSpeedInKilometerPerHour = 15.0;
        [JsonIgnore]
        private float _EvolveAboveIvValue = 90;
        [JsonIgnore]
        private int _KeepMinCp = 1250;
        [JsonIgnore]
        private int _KeepMinDuplicatePokemon = 1;
        [JsonIgnore]
        private float _KeepMinIvPercentage = 95;
        [JsonIgnore]
        private bool _KeepPokemonsThatCanEvolve = false;
        [JsonIgnore]
        private int _MaxTravelDistanceInMeters = 1000;
        [JsonIgnore]
        private int _AmountOfPokemonToDisplayOnStart = 10;

        [JsonIgnore]
        private List<KeyValuePair<ItemId, int>> _ItemRecycleFilter = new List<KeyValuePair<ItemId, int>>
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

        [JsonIgnore]
        private List<PokemonId> _PokemonsNotToTransfer = new List<PokemonId>
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

        [JsonIgnore]
        private List<PokemonId> _PokemonsToEvolve = new List<PokemonId>
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

        [JsonIgnore]
        private List<PokemonId> _PokemonsToIgnore = new List<PokemonId>
        {
            PokemonId.Caterpie,
            PokemonId.Weedle,
            PokemonId.Pidgey,
            PokemonId.Rattata,
            PokemonId.Spearow,
            PokemonId.Zubat,
            PokemonId.Doduo
        };

        [JsonIgnore]
        private Dictionary<PokemonId, PokemonTransferFilter> _PokemonsTransferFilter = new Dictionary<PokemonId, PokemonTransferFilter>
        {
            {PokemonId.Pidgeotto, new PokemonTransferFilter(1500, 90, 1)},
            {PokemonId.Fearow, new PokemonTransferFilter(1500, 90, 2)},
            {PokemonId.Golbat, new PokemonTransferFilter(1500, 90, 2)},
            {PokemonId.Eevee, new PokemonTransferFilter(600, 90, 2)},
            {PokemonId.Mew, new PokemonTransferFilter(0, 0, 10)}
        };
        #endregion
        #region "Properties"
        public int AmountOfPokemonToDisplayOnStart
        {
            get { return _AmountOfPokemonToDisplayOnStart; }
            set
            {
                if (_AmountOfPokemonToDisplayOnStart != value) {
                    _AmountOfPokemonToDisplayOnStart = value;
                    NotifyPropertyDidChange("AmountOfPokemonToDisplayOnStart");
                }
            }
        }

        public bool AutoUpdate
        {
            get { return _AutoUpdate; }
            set
            {
                if (_AutoUpdate != value) {
                    _AutoUpdate = value;
                    NotifyPropertyDidChange("AutoUpdate");
                }
            }
        }

        public bool EvolveAllPokemonAboveIv
        {
            get { return _EvolveAllPokemonAboveIv; }
            set
            {
                if (_EvolveAllPokemonAboveIv != value) {
                    _EvolveAllPokemonAboveIv = value;
                    NotifyPropertyDidChange("EvolveAllPokemonAboveIv");
                }
            }
        }

        public bool EvolveAllPokemonWithEnoughCandy
        {
            get { return _EvolveAllPokemonWithEnoughCandy; }
            set
            {
                if (_EvolveAllPokemonWithEnoughCandy != value) {
                    _EvolveAllPokemonWithEnoughCandy = value;
                    NotifyPropertyDidChange("EvolveAllPokemonWithEnoughCandy");
                }
            }
        }

        public bool DumpPokemonStats
        {
            get { return _DumpPokemonStats; }
            set
            {
                if (_DumpPokemonStats != value) {
                    _DumpPokemonStats = value;
                    NotifyPropertyDidChange("DumpPokemonStats");
                }
            }
        }

        public bool PrioritizeIvOverCp
        {
            get { return _PrioritizeIvOverCp; }
            set
            {
                if (_PrioritizeIvOverCp != value) {
                    _PrioritizeIvOverCp = value;
                    NotifyPropertyDidChange("PrioritizeIvOverCp");
                }
            }
        }

        public bool RenameAboveIv
        {
            get { return _RenameAboveIv; }
            set
            {
                if (_RenameAboveIv != value) {
                    _RenameAboveIv = value;
                    NotifyPropertyDidChange("RenameAboveIv");
                }
            }
        }

        public bool TransferDuplicatePokemon
        {
            get { return _TransferDuplicatePokemon; }
            set
            {
                if (_TransferDuplicatePokemon != value) {
                    _TransferDuplicatePokemon = value;
                    NotifyPropertyDidChange("TransferDuplicatePokemon");
                }
            }
        }

        public bool UseEggIncubators
        {
            get { return _UseEggIncubators; }
            set
            {
                if (_UseEggIncubators != value) {
                    _UseEggIncubators = value;
                    NotifyPropertyDidChange("UseEggIncubators");
                }
            }
        }

        public bool UseGpxPathing
        {
            get { return _UseGpxPathing; }
            set
            {
                if (_UseGpxPathing != value) {
                    _UseGpxPathing = value;
                    NotifyPropertyDidChange("UseGpxPathing");
                }
            }
        }

        public bool UseLuckyEggsWhileEvolving
        {
            get { return _UseLuckyEggsWhileEvolving; }
            set
            {
                if (_UseLuckyEggsWhileEvolving != value) {
                    _UseLuckyEggsWhileEvolving = value;
                    NotifyPropertyDidChange("UseLuckyEggsWhileEvolving");
                }
            }
        }

        public bool UsePokemonToNotCatchFilter
        {
            get { return _UsePokemonToNotCatchFilter; }
            set
            {
                if (_UsePokemonToNotCatchFilter != value) {
                    _UsePokemonToNotCatchFilter = value;
                    NotifyPropertyDidChange("UsePokemonToNotCatchFilter");
                }
            }
        }

        public bool StartupWelcomeDelay
        {
            get { return _StartupWelcomeDelay; }
            set
            {
                if (_StartupWelcomeDelay != value) {
                    _StartupWelcomeDelay = value;
                    NotifyPropertyDidChange("StartupWelcomeDelay");
                }
            }
        }

        public double DefaultAltitude
        {
            get { return _DefaultAltitude; }
            set
            {
                if (_DefaultAltitude != value) {
                    _DefaultAltitude = value;
                    NotifyPropertyDidChange("DefaultAltitude");
                }
            }
        }

        public double DefaultLatitude
        {
            get { return _DefaultLatitude; }
            set
            {
                if (_DefaultLatitude != value) {
                    _DefaultLatitude = value;
                    NotifyPropertyDidChange("DefaultLatitude");
                }
            }
        }

        public double DefaultLongitude
        {
            get { return _DefaultLongitude; }
            set
            {
                if (_DefaultLongitude != value) {
                    _DefaultLongitude = value;
                    NotifyPropertyDidChange("DefaultLongitude");
                }
            }
        }

        public double WalkingSpeedInKilometerPerHour
        {
            get { return _WalkingSpeedInKilometerPerHour; }
            set
            {
                if (_WalkingSpeedInKilometerPerHour != value) {
                    _WalkingSpeedInKilometerPerHour = value;
                    NotifyPropertyDidChange("WalkingSpeedInKilometerPerHour");
                }
            }
        }

        public int DelayBetweenPokemonCatch
        {
            get { return _DelayBetweenPokemonCatch; }
            set
            {
                if (_DelayBetweenPokemonCatch != value) {
                    _DelayBetweenPokemonCatch = value;
                    NotifyPropertyDidChange("DelayBetweenPokemonCatch");
                }
            }
        }

        public int WebSocketPort
        {
            get { return _WebSocketPort; }
            set
            {
                if (_WebSocketPort != value) {
                    _WebSocketPort = value;
                    NotifyPropertyDidChange("WebSocketPort");
                }
            }
        }

        public int UseLuckyEggsMinPokemonAmount
        {
            get { return _UseLuckyEggsMinPokemonAmount; }
            set
            {
                if (_UseLuckyEggsMinPokemonAmount != value) {
                    _UseLuckyEggsMinPokemonAmount = value;
                    NotifyPropertyDidChange("UseLuckyEggsMinPokemonAmount");
                }
            }
        }

        public float EvolveAboveIvValue
        {
            get { return _EvolveAboveIvValue; }
            set
            {
                if (_EvolveAboveIvValue != value) {
                    _EvolveAboveIvValue = value;
                    NotifyPropertyDidChange("EvolveAboveIvValue");
                }
            }
        }

        public string GpxFile
        {
            get { return _GpxFile; }
            set
            {
                if (_GpxFile != value) {
                    _GpxFile = value;
                    NotifyPropertyDidChange("GpxFile");
                }
            }   
        }

        public string TranslationLanguageCode
        {
            get { return _TranslationLanguageCode; }
            set
            {
                if (_TranslationLanguageCode != value) {
                    _TranslationLanguageCode = value;
                    NotifyPropertyDidChange("TranslationLanguageCode");
                }
            }
        }

        public int KeepMinCp
        {
            get { return _KeepMinCp; }
            set
            {
                if (_KeepMinCp != value) {
                    _KeepMinCp = value;
                    NotifyPropertyDidChange("KeepMinCp");
                }
            }
        }

        public int KeepMinDuplicatePokemon
        {
            get { return _KeepMinDuplicatePokemon; }
            set
            {
                if (_KeepMinDuplicatePokemon != value) {
                    _KeepMinDuplicatePokemon = value;
                    NotifyPropertyDidChange("KeepMinDuplicatePokemon");
                }
            }
        }

        public float KeepMinIvPercentage
        {
            get { return _KeepMinIvPercentage; }
            set
            {
                if (_KeepMinIvPercentage != value) {
                    _KeepMinIvPercentage = value;
                    NotifyPropertyDidChange("KeepMinIvPercentage");
                }
            }
        }

        public bool KeepPokemonsThatCanEvolve
        {
            get { return _KeepPokemonsThatCanEvolve; }
            set
            {
                if (_KeepPokemonsThatCanEvolve != value) {
                    _KeepPokemonsThatCanEvolve = value;
                    NotifyPropertyDidChange("KeepPokemonsThatCanEvolve");
                }
            }
        }

        public int MaxTravelDistanceInMeters
        {
            get { return _MaxTravelDistanceInMeters; }
            set
            {
                if (_MaxTravelDistanceInMeters != value) {
                    _MaxTravelDistanceInMeters = value;
                    NotifyPropertyDidChange("MaxTravelDistanceInMeters");
                }
            }
        }

        public Dictionary<PokemonId, PokemonTransferFilter> PokemonsTransferFilter
        {
            get { return _PokemonsTransferFilter; }
            set
            {
                if (_PokemonsTransferFilter != value) {
                    _PokemonsTransferFilter = value;
                    NotifyPropertyDidChange("PokemonsTransferFilter");
                }
            }
        }

        public List<PokemonId> PokemonsNotToCatch
        {
            get { return _PokemonsToIgnore; }
            set
            {
                if (_PokemonsToIgnore != value) {
                    _PokemonsToIgnore = value;
                    NotifyPropertyDidChange("PokemonsToIgnore");
                }
            }
        }

        public List<PokemonId> PokemonsToEvolve
        {
            get { return _PokemonsToEvolve; }
            set
            {
                if (_PokemonsToEvolve != value) {
                    _PokemonsToEvolve = value;
                    NotifyPropertyDidChange("PokemonsToEvolve");
                }
            }
        }

        public List<PokemonId> PokemonsNotToTransfer
        {
            get { return _PokemonsNotToTransfer; }
            set
            {
                if (_PokemonsNotToTransfer != value) {
                    _PokemonsNotToTransfer = value;
                    NotifyPropertyDidChange("PokemonsNotToTransfer");
                }
            }
        }

        public List<KeyValuePair<ItemId, int>> ItemRecycleFilter
        {
            get { return _ItemRecycleFilter; }
            set
            {
                if (_ItemRecycleFilter != value) {
                    _ItemRecycleFilter = value;
                    NotifyPropertyDidChange("ItemRecycleFilter");
                }
            }
        }
        #endregion

        public ConfigurationSettings() {

        }

        #region "INotifyPropertyChanged Implementation"
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyDidChange(string property) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        #endregion
    }
}
