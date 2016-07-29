#region using directives

using PoGo.NecroBot.Logic.Common;
using PokemonGo.RocketAPI;
using POGOProtos.Networking.Responses;
using PoGo.NecroBot.Logic.Event;
using System;
using PoGo.NecroBot.Logic.Service;

#endregion

namespace PoGo.NecroBot.Logic.State
{
    public interface ISession
    {
        ISettings Settings { get; }
        Inventory Inventory { get; }
        Client Client { get;  }
        GetPlayerResponse Profile { get; set; }
        Navigation Navigation { get;  }
        ILogicSettings LogicSettings { get; }
        ITranslation Translation { get; }
        IEventDispatcher EventDispatcher { get; }
        TelegramService Telegram { get; set;  }
    }


    public class Session : ISession
    {
        public Session(ISettings settings, ILogicSettings logicSettings)
        {
            Settings = settings;
            LogicSettings = logicSettings;
            EventDispatcher = new EventDispatcher();
            Translation = Common.Translation.Load(logicSettings);
            Reset(settings, LogicSettings);
        }

        public ISettings Settings { get; }

        public Inventory Inventory { get; private set; }

        public Client Client { get; private set; }

        public GetPlayerResponse Profile { get; set; }
        public Navigation Navigation { get; private set; }

        public ILogicSettings LogicSettings { get; }

        public ITranslation Translation { get; private set; }

        public IEventDispatcher EventDispatcher{ get; private set; }

        public TelegramService Telegram { get; set; }

        public void Reset(ISettings settings, ILogicSettings logicSettings)
        {
            Client = new Client(Settings);
            Client.AuthType = settings.AuthType; // ferox wants us to set this manually
            Inventory = new Inventory(Client, logicSettings);
            Navigation = new Navigation(Client);
        }
    }
}
