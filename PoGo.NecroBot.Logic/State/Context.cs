#region using directives

using PoGo.NecroBot.Logic.Common;
using PokemonGo.RocketAPI;
using POGOProtos.Networking.Responses;

#endregion

namespace PoGo.NecroBot.Logic.State
{
    public class Context
    {
        public Context(ISettings settings, ILogicSettings logicSettings)
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

        public LogicClient LogicClient { get; private set; }

        public Translations Translations { get; private set; }

        public void Reset(ISettings settings, ILogicSettings logicSettings)
        {
            Client = new Client(Settings);
            LogicClient = new LogicClient(LogicSettings);
            Inventory = new Inventory(Client, LogicClient);
            Navigation = new Navigation(Client);
        }
    }
}