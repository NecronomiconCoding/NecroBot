using PokemonGo.RocketAPI.GeneratedCode;
using PokemonGo.RocketAPI.Logic.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGo.RocketAPI.Logic.State
{
    public class Context
    {
        private Client _client;
        private readonly ISettings _clientSettings;
        private Inventory _inventory;
        private Navigation _navigation;
        private GetPlayerResponse _playerProfile;
        private readonly Statistics _stats;

        public Context(ISettings settings)
        {
            _clientSettings = settings;
            _stats = new Statistics();

            Reset(settings);
        }

        public void Reset(ISettings settings)
        {
            _client = new Client(_clientSettings);
            _inventory = new Inventory(_client);
            _navigation = new Navigation(_client);
        }
    }
}
