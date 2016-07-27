using System;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Windows;

using PoGo.NecroBot.UI.Config;

using PokemonGo.RocketAPI.Enums;

using POGOProtos.Data;
using POGOProtos.Enums;
using POGOProtos.Inventory.Item;
using POGOProtos.Networking.Responses;

using PoGo.NecroBot.Logic;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Utils;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.Logging;


namespace PoGo.NecroBot.UI.Models {
    public class StateModel : INotifyPropertyChanged {
        private Context _context;
        private GlobalSettings _settings;
        private StateMachine _stateMachine;
        private Statistics _statistics;
        private StatisticsAggregator _aggregator;

        protected GlobalSettings Settings { get { return _settings; } }
        protected Context Context { get { return _context; } }
        protected StateMachine StateMachine { get { return _stateMachine; } }
        protected Statistics Stats { get { return _statistics; } }
        protected StatisticsAggregator StatAggregator { get { return _aggregator; } }


        #region "Current Data References"
        private PlayerData _player;
        #endregion

        #region "Model Properties"
        private string _playerUsername;
        private TeamColor _playerTeam;
        private int _inventorySize;
        private int _storageSize;

        /// <summary>
        /// The player's Username in-game
        /// </summary>
        public string Username
        {
            get { return _playerUsername; }
            set
            {
                if (_playerUsername != value) {
                    _playerUsername = value;
                    NotifyPropertyChanged("Username");
                }
            }
        }

        /// <summary>
        /// The player's current Team
        /// </summary>
        public TeamColor Team
        {
            get { return _playerTeam; }
            set
            {
                if (_playerTeam != value) {
                    _playerTeam = value;
                    NotifyPropertyChanged("Team");
                }
            }
        }

        /// <summary>
        /// The player's current inventory size
        /// </summary>
        public int InventorySize
        {
            get { return _inventorySize; }
            set
            {
                if (_inventorySize != value) {
                    _inventorySize = value;
                    NotifyPropertyChanged("InventorySize");
                }
            }
        }

        /// <summary>
        /// The player's current Pokemon storage size
        /// </summary>
        public int StorageSize
        {
            get { return _storageSize; }
            set
            {
                if (_storageSize != value) {
                    _storageSize = value;
                    NotifyPropertyChanged("StorageSize");
                }
            }
        }
        #endregion

        #region "Configuration Properties"
        private static readonly string[] AUTH_SETTINGS_PROPERTIES = new string[] {
            "AuthType",
            "GoogleRefreshToken",
            "PtcUsername",
            "PtcPassword"
        };

        private static readonly string[] SETTINGS_PROPERTIES = new string[] {
            "DefaultAltitude",
            "DefaultLatitude",
            "DefaultLongitude",
            "DelayBetweenPokemonCatch",
            "EvolveAboveIvValue",
            "EvolveAllPokemonWithEnoughCandy",
            "GpxFile",
            "KeepMinCp",
            "KeepMinDuplicatePokemon",
            "KeepMinIvPercentage",
            "KeepPokemonsThatCanEvolve",
            "MaxTravelDistanceInMeters",
            "PrioritizeIvOverCp",
            "TransferDuplicatePokemon",
            "UseGpxPathing",
            "UseLuckyEggsWhileEvolving",
            "UsePokemonToNotCatchFilter",
            "WalkingSpeedInKilometerPerHour",
            "AmountOfPokemonToDisplayOnStart",
            "RenameAboveIv",
            "EnableWebSocket",
            "WebSocketPort"
        };

        private AuthType _authType = AuthType.Ptc;
        private string _googleRefreshToken;
        private string _ptcUsername;
        private string _ptcPassword;
        private bool _useGoogleAuth = false;

        private double _defaultAltitude;
        private double _defaultLatitude;
        private double _defaultLongitude;
        private int _delayBetweenPokemonCatch;
        private float _evolveAboveIvValue;
        private bool _evolveAllPokemonAboveIv;
        private bool _evolveAllPokemonWithEnoughCandy;
        private string _gpxFile;
        private int _keepMinCp;
        private int _keepMinDuplicatePokemon;
        private float _keepMinIvPercentage;
        private bool _keepPokemonsThatCanEvolve;
        private int _maxTravelDistanceInMeters;
        private bool _prioritizeIvOverCp;
        private bool _transferDuplicatePokemon;
        private bool _useGpxPathing = false;
        private bool _useLuckyEggsWhileEvolving;
        private bool _usePokemonToNotCatchFilter;
        private double _walkingSpeedInKilometerPerHour;
        private int _amountOfPokemonToDisplayOnStart;
        private bool _renameAboveIv;
        private bool _enableWebSocket;
        private int _webSocketPort;

        #region "Authentication Properties"
        public bool UseGoogleAuth
        {
            get { return _useGoogleAuth; }
            set
            {
                if (_useGoogleAuth != value) {
                    _useGoogleAuth = value;
                    if (_useGoogleAuth)
                        AuthType = AuthType.Google;

                    NotifyPropertyChanged("UseGoogleAuth");
                    NotifyPropertyChanged("UseTrainerClubAuth");
                    NotifyPropertyChanged("DisplayTrainerClub");
                }
            }
        }

        public Visibility DisplayTrainerClub
        {
            get { return (UseGoogleAuth) ? Visibility.Collapsed : Visibility.Visible; }
        }
        

        public bool UseTrainerClubAuth
        {
            get { return !UseGoogleAuth; }
        }

        public AuthType AuthType
        {
            get { return _authType; }
            set
            {
                if (!_authType.Equals(value)) {
                    _authType = value;
                    NotifyPropertyChanged("AuthType");
                }
            }
        }

        public string GoogleRefreshToken
        {
            get { return _googleRefreshToken; }
            set
            {
                if (_googleRefreshToken != value) {
                    _googleRefreshToken = value;
                    NotifyPropertyChanged("GoogleRefreshToken");
                }
            }
        }

        public string PtcUsername
        {
            get { return _ptcUsername; }
            set
            {
                if (_ptcUsername != value) {
                    _ptcUsername = value;
                    NotifyPropertyChanged("PtcUsername");
                }
            }
        }

        public string PtcPassword
        {
            get { return _ptcPassword; }
            set
            {
                if (_ptcPassword != value) {
                    _ptcPassword = value;
                    NotifyPropertyChanged("PtcPassword");
                }
            }
        }
        #endregion

        #region "GPS Properties"
        public double DefaultAltitude
        {
            get { return _defaultAltitude; }
            set
            {
                if (_defaultAltitude != value) {
                    _defaultAltitude = value;
                    NotifyPropertyChanged("DefaultAltitude");
                }
            }
        }

        public double DefaultLatitude
        {
            get { return _defaultLatitude; }
            set
            {
                if (_defaultLatitude != value) {
                    _defaultLatitude = value;
                    NotifyPropertyChanged("DefaultLatitude");
                }
            }
        }

        public double DefaultLongitude
        {
            get { return _defaultLongitude; }
            set
            {
                if (_defaultLongitude != value) {
                    _defaultLongitude = value;
                    NotifyPropertyChanged("DefaultLongitude");
                }
            }
        }

        public bool UseGpxPathing
        {
            get { return _useGpxPathing; }
            set
            {
                if (_useGpxPathing != value) {
                    _useGpxPathing = value;
                    NotifyPropertyChanged("UseGpxPathing");
                    NotifyPropertyChanged("GpxPathingReadonly");
                    NotifyPropertyChanged("DisplayGpxPathing");
                }
            }
        }

        public Visibility DisplayGpxPathing
        {
            get { return (UseGpxPathing) ? Visibility.Visible : Visibility.Collapsed; }
        }
        #endregion

        public int DelayBetweenPokemonCatch
        {
            get { return _delayBetweenPokemonCatch; }
            set
            {
                if (_delayBetweenPokemonCatch != value) {
                    _delayBetweenPokemonCatch = value;
                    NotifyPropertyChanged("DelayBetweenPokemonCatch");
                }
            }
        }

        

        public bool GpxPathingReadonly
        {
            get
            {
                return !_useGpxPathing;
            }
        }
        #endregion

        private void CopySettings() {
            AuthType = Settings.Auth.AuthType;
            GoogleRefreshToken = Settings.Auth.GoogleRefreshToken;
            PtcUsername = Settings.Auth.PtcUsername;
            PtcPassword = Settings.Auth.PtcPassword;

            UseGpxPathing = false;
        }

        private void PropertyDidChange(object sender, PropertyChangedEventArgs e) {
            if (AUTH_SETTINGS_PROPERTIES.Contains(e.PropertyName)) {
                var property = this.GetType().GetProperty(e.PropertyName);
                var value = property.GetValue(this);

                // Set the remote property value
                var remoteProperty = Settings.Auth.GetType().GetProperty(e.PropertyName);
                remoteProperty.SetValue(Settings.Auth, value);

                Settings.Auth.Save();
                return;
            }
        }

        public StateModel() {
            _settings = GlobalSettings.Load(string.Empty);
            
            // Copy settings so they're reflected in the UI
            CopySettings();

            // Assign and event handler for checking setting changes
            this.PropertyChanged += PropertyDidChange;


            //_stateMachine = new StateMachine();
            //_statistics = new Statistics();

            //_aggregator = new StatisticsAggregator(Stats);

            //StateMachine.EventListener += Listen;
            //StateMachine.EventListener += StatAggregator.Listen;

            //StateMachine.SetFailureState(new LoginState());

            //_context = new Context(new ClientSettings(Settings), new LogicSettings(Settings));
            //Context.Client.Login.GoogleDeviceCodeEvent += DidReceiveGoogleCode;

            //StateMachine.AsyncStart(new VersionCheckState(), Context);
            //StateMachine.
        }

        

        private void DidReceiveGoogleCode(string code, string uri) {
            Clipboard.SetText(code);
        }



        private void PlayerDidUpdate(PlayerData data) {
            Username = data.Username;
            Team = data.Team;
            InventorySize = data.MaxItemStorage;
            StorageSize = data.MaxPokemonStorage;
        }

        private void LuckyEggUsed(UseLuckyEggEvent data) {

        }

        #region "Event Handling"
        public void HandleEvent(ProfileEvent evt, Context ctx) {
            Logger.Write($"Playing as {evt.Profile.PlayerData.Username ?? ""}");
            
            PlayerDidUpdate(evt.Profile.PlayerData);
        }

        public void HandleEvent(ErrorEvent evt, Context ctx) {
            Logger.Write(evt.ToString(), LogLevel.Error);
        }

        public void HandleEvent(NoticeEvent evt, Context ctx) {
            Logger.Write(evt.ToString());
            
        }

        public void HandleEvent(WarnEvent evt, Context ctx) {
            Logger.Write(evt.ToString(), LogLevel.Warning);
        }

        public void HandleEvent(UseLuckyEggEvent evt, Context ctx) {
            Logger.Write($"Used Lucky Egg, remaining: {evt.Count}", LogLevel.Egg);
            LuckyEggUsed(evt);
        }

        public void HandleEvent(PokemonEvolveEvent evt, Context ctx) {
            Logger.Write(evt.Result == EvolvePokemonResponse.Types.Result.Success
                ? $"{evt.Id} successfully for {evt.Exp}xp"
                : $"Failed {evt.Id}. EvolvePokemonOutProto.Result was {evt.Result}, stopping evolving {evt.Id}",
                LogLevel.Evolve);
        }

        public void HandleEvent(TransferPokemonEvent evt, Context ctx) {
            Logger.Write(
                $"{evt.Id}\t- CP: {evt.Cp}  IV: {evt.Perfection.ToString("0.00")}%   [Best CP: {evt.BestCp}  IV: {evt.BestPerfection.ToString("0.00")}%] (Candies: {evt.FamilyCandies}) ",
                LogLevel.Transfer);
        }

        public void HandleEvent(ItemRecycledEvent evt, Context ctx) {
            Logger.Write($"{evt.Count}x {evt.Id}", LogLevel.Recycling);
        }

        public void HandleEvent(FortUsedEvent evt, Context ctx) {
            Logger.Write($"XP: {evt.Exp}, Gems: {evt.Gems}, Items: {evt.Items}", LogLevel.Pokestop);
        }

        public void HandleEvent(FortTargetEvent evt, Context ctx) {
            Logger.Write($"{evt.Name} in ({Math.Round(evt.Distance)}m)", LogLevel.Info, ConsoleColor.DarkRed);
        }

        public void HandleEvent(PokemonCaptureEvent evt, Context ctx) {
            Func<ItemId, string> returnRealBallName = a => {
                switch (a) {
                    case ItemId.ItemPokeBall:
                        return "Poke";
                    case ItemId.ItemGreatBall:
                        return "Great";
                    case ItemId.ItemUltraBall:
                        return "Ultra";
                    case ItemId.ItemMasterBall:
                        return "Master";
                    default:
                        return "Unknown";
                }
            };

            var catchType = evt.CatchType;

            var catchStatus = evt.Attempt > 1
                ? $"{evt.Status} Attempt #{evt.Attempt}"
                : $"{evt.Status}";

            var familyCandies = evt.FamilyCandies > 0
                ? $"Candies: {evt.FamilyCandies}"
                : "";

            Logger.Write(
                $"({catchStatus}) | ({catchType}) {evt.Id} Lvl: {evt.Level} CP: ({evt.Cp}/{evt.MaxCp}) IV: {evt.Perfection.ToString("0.00")}% | Chance: {evt.Probability}% | {Math.Round(evt.Distance)}m dist | with a {returnRealBallName(evt.Pokeball)}Ball ({evt.BallAmount} left). | {familyCandies}",
                LogLevel.Caught);
        }

        public void HandleEvent(NoPokeballEvent evt, Context ctx) {
            Logger.Write($"No Pokeballs - We missed a {evt.Id} with CP {evt.Cp}", LogLevel.Caught);
        }

        public void HandleEvent(UseBerryEvent evt, Context ctx) {
            Logger.Write($"Used, remaining: {evt.Count}", LogLevel.Berry);
        }

        public void HandleEvent(DisplayHighestsPokemonEvent evt, Context ctx) {
            Logger.Write($"====== DisplayHighests{evt.SortedBy} ======", LogLevel.Info, ConsoleColor.Yellow);
            foreach (var pokemon in evt.PokemonList)
                Logger.Write(
                    $"# CP {pokemon.Item1.Cp.ToString().PadLeft(4, ' ')}/{pokemon.Item2.ToString().PadLeft(4, ' ')} | ({pokemon.Item3.ToString("0.00")}% perfect)\t| Lvl {pokemon.Item4.ToString("00")}\t NAME: '{pokemon.Item1.PokemonId}'",
                    LogLevel.Info, ConsoleColor.Yellow);
        }

        public void Listen(IEvent evt, Context ctx) {
            dynamic eve = evt;

            HandleEvent(eve, ctx);
        }
        #endregion

        #region "INotifyPropertyChanged Interface"
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string property) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
        #endregion
    }
}
