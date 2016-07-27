using PoGo.NecroBot.Logic.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.Localization;
using PokemonGo.RocketAPI;

namespace PoGo.NecroBot.CLI
{
    public class SimpleSession : ISession
    {
        public Client _client;
        public IEventDispatcher _dispatcher;
        public ILocalizer _localizer;

        public Client Client
        {
            get
            {
                return _client;
            }
        }

        public IEventDispatcher EventDispatcher
        {
            get
            {
                return _dispatcher;
            }
        }

        public ILocalizer Localizer
        {
            get
            {
                return _localizer;
            }
        }
    }
}
