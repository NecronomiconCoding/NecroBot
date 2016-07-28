#region using directives

using PoGo.NecroBot.Logic.Common;
using PokemonGo.RocketAPI;
using POGOProtos.Networking.Responses;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.Profiles;
using PoGo.NecroBot.Logic.Settings;
using System;

#endregion

namespace PoGo.NecroBot.Logic.State
{
    public interface ISession
    {
        IProfile BotProfile { get; }
        Inventory Inventory { get; }
        Client Client { get;  }
        GetPlayerResponse Profile { get; set; }
        Navigation Navigation { get;  }
        ITranslation Translation { get; }
        IEventDispatcher EventDispatcher { get; }
        IConfigurationSettings LogicSettings { get; }
        ISettings Settings { get; }

        string ProfilePath { get; }
    }

    public class Session : ISession
    {
        public Session(IProfile profile) {
            BotProfile = profile;
            EventDispatcher = new EventDispatcher();
            Translation = Common.Translation.Load(BotProfile.Settings.Bot.TranslationLanguageCode);
            Reset(profile.Settings.Bot);
        }

        public IProfile BotProfile { get; private set; }
        public Inventory Inventory { get; private set; }
        public Client Client { get; private set; }
        public GetPlayerResponse Profile { get; set; }
        public Navigation Navigation { get; private set; }
        public ITranslation Translation { get; private set; }
        public IEventDispatcher EventDispatcher{ get; private set; }
        public ISettings Settings { get { return (ISettings)BotProfile; } }
        public IConfigurationSettings LogicSettings => BotProfile.Settings.Bot;
        public string ProfilePath => BotProfile.FilePath;

        public void Reset(IConfigurationSettings logicSettings)
        {
            Client = new Client((ISettings)BotProfile);
            Inventory = new Inventory(Client, logicSettings);
            Navigation = new Navigation(Client);
        }
    }
}