using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using PokemonGo.RocketAPI;
using PokemonGo.RocketAPI.Enums;

namespace PoGo.NecroBot.UI.Config {
    class ClientSettings : ISettings {
        private GlobalSettings _settings;

        public ClientSettings(GlobalSettings settings) {
            _settings = settings;
        }

        public AuthType AuthType => _settings.Auth.AuthType;
        public string PtcUsername => _settings.Auth.PtcUsername;
        public string PtcPassword => _settings.Auth.PtcPassword;
        public double DefaultLatitude => _settings.DefaultLatitude;
        public double DefaultLongitude => _settings.DefaultLongitude;
        public double DefaultAltitude => _settings.DefaultAltitude;

        public string GoogleRefreshToken
        {
            get
            {
                return _settings.Auth.GoogleRefreshToken;
            }
            set
            {
                _settings.Auth.GoogleRefreshToken = value;
                _settings.Auth.Save();
            }
        }
    }
}
