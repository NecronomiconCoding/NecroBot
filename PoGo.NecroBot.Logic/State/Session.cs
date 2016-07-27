#region using directives

using PoGo.NecroBot.Logic.Common;
using PokemonGo.RocketAPI;
using POGOProtos.Networking.Responses;

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

        Translations Translations { get; }
    }


    public class Session : ISession
    {
        public Session(ISettings settings, ILogicSettings logicSettings)
        {
            Settings = settings;
            LogicSettings = logicSettings;
            Translations = Translations.Load(logicSettings.TranslationLanguageCode);
            Reset(settings, LogicSettings);
        }

        public ISettings Settings { get; }

        public Inventory Inventory { get; private set; }

        public Client Client { get; private set; }

        public GetPlayerResponse Profile { get; set; }
        public Navigation Navigation { get; private set; }

        public ILogicSettings LogicSettings { get; }

        public Translations Translations { get; private set; }

        public void Reset(ISettings settings, ILogicSettings logicSettings)
        {
            Client = new Client(Settings);
            Inventory = new Inventory(Client, logicSettings);
            Navigation = new Navigation(Client);
        }
    }
}