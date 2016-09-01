#region using directives

using PoGo.NecroBot.Logic.Common;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.Interfaces.Configuration;
using PoGo.NecroBot.Logic.Model.Settings;
using PoGo.NecroBot.Logic.Service;
using PokemonGo.RocketAPI;
using POGOProtos.Networking.Responses;
using PoGo.NecroBot.Logic.Service.Elevation;

#endregion

namespace PoGo.NecroBot.Logic.State
{
    public interface ISession
    {
        ISettings Settings { get; set; }
        Inventory Inventory { get; }
        Client Client { get; }
        GetPlayerResponse Profile { get; set; }
        Navigation Navigation { get; }
        ILogicSettings LogicSettings { get; }
        ITranslation Translation { get; }
        IEventDispatcher EventDispatcher { get; }
        TelegramService Telegram { get; set; }
        SessionStats Stats { get; }
        ElevationService ElevationService { get; }
    }


    public class Session : ISession
    {
        public Session(ISettings settings, ILogicSettings logicSettings) : this(settings, logicSettings, Common.Translation.Load(logicSettings))
        {
        }

        public Session(ISettings settings, ILogicSettings logicSettings, ITranslation translation)
        {
            EventDispatcher = new EventDispatcher();
            LogicSettings = logicSettings;

            ElevationService = new ElevationService(this);

            // Update current altitude before assigning settings.
            settings.DefaultAltitude = ElevationService.GetElevation(settings.DefaultLatitude, settings.DefaultLongitude);
            
            Settings = settings;
            
            Translation = translation;
            Reset(settings, LogicSettings);
            Stats = new SessionStats();
        }

        public GlobalSettings GlobalSettings { get; set; }

        public ISettings Settings { get; set; }

        public Inventory Inventory { get; private set; }

        public Client Client { get; private set; }

        public GetPlayerResponse Profile { get; set; }
        public Navigation Navigation { get; private set; }

        public ILogicSettings LogicSettings { get; set; }

        public ITranslation Translation { get; }

        public IEventDispatcher EventDispatcher { get; }

        public TelegramService Telegram { get; set; }
        
        public SessionStats Stats { get; set; }

        public ElevationService ElevationService { get; }

        public void Reset(ISettings settings, ILogicSettings logicSettings)
        {
            ApiFailureStrategy _apiStrategy = new ApiFailureStrategy(this);
            Client = new Client(Settings, _apiStrategy);
            // ferox wants us to set this manually
            Inventory = new Inventory(Client, logicSettings);
            Navigation = new Navigation(Client, logicSettings);
        }
    }
}