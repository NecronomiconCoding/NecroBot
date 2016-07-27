using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Event;
using PokemonGo.RocketAPI;

namespace PoGo.NecroBot.Logic.Player
{
    public class SimpleSession : ISession
    {
        private ISettings _settings;
        private ILogicSettings _loginSettings;
        private IEventHandler _eventHandler;
        private Client _client;

        public ISettings ApiSettings
        {
            get
            {
                return _settings;
            }
        }

        public Client Client
        {
            get
            {
                return _client;
            }
        }

        public IEventHandler EventHandler
        {
            get
            {
                return _eventHandler;
            }
        }

        public ILogicSettings LogicSettings
        {
            get
            {
                return _loginSettings;
            }
        }

        public SimpleSession(ISettings settings, ILogicSettings logicSettings, IEventHandler eventHandler)
        {
            _settings = settings;
            _loginSettings = logicSettings;
            _eventHandler = eventHandler;

            _client = new Client(_settings);
        }
    }
}
