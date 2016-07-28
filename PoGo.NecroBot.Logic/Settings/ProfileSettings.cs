using System;
using System.IO;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using PokemonGo.RocketAPI.Enums;

namespace PoGo.NecroBot.Logic.Settings {
    public class ProfileSettings : SettingsFile, IProfileSettings {
        [JsonIgnore]
        private IAuthenticationSettings _account;
        [JsonIgnore]
        private IConfigurationSettings _bot;

        public IAuthenticationSettings Account { get { return _account; } }
        public IConfigurationSettings Bot { get { return _bot; } }


        public ProfileSettings(string path) {
            FilePath = path;

            _account = new AuthenticationSettings();
            _bot = new ConfigurationSettings();

            LoadOrInitializeFileAsync();
        }

        protected override void LoadOrInitializeFile() {
            base.LoadOrInitializeFile();

            if (Account != null)
                ((AuthenticationSettings)Account).PropertyChanged += SettingDidChange;

            if (Bot != null)
                ((ConfigurationSettings)Bot).PropertyChanged += SettingDidChange;
        }

        private void SettingDidChange(object sender, PropertyChangedEventArgs e) {
            SaveFileAsync();   
        }

        
    }
}
