using System.IO;
using System.ComponentModel;
using Newtonsoft.Json;
using PokemonGo.RocketAPI.Enums;


namespace PoGo.NecroBot.Logic.Settings {
    public class AuthenticationSettings : IAuthenticationSettings, INotifyPropertyChanged {
        #region "Authentication Settings"
        private AuthType _authenticationType;
        private string _googleRefreshToken;
        private string _ptcUsername;
        private string _ptcPassword;

        public AuthType AuthenticationType
        {
            get { return _authenticationType; }
            set
            {
                if (_authenticationType != value) {
                    _authenticationType = value;
                    NotifyPropertyDidChange("AuthenticationType");
                }
            }
        }

        public string GoogleRefreshToken
        {
            get { return _googleRefreshToken; }
            set
            {
                if (_googleRefreshToken != value) {
                    _googleRefreshToken = value;
                    NotifyPropertyDidChange("GoogleRefreshToken");
                }
            }
        }

        public string PtcUsername
        {
            get { return _ptcUsername; }
            set
            {
                if (_ptcUsername != value) {
                    _ptcUsername = value;
                    NotifyPropertyDidChange("PtcUsername");
                }
            }
        }

        public string PtcPassword
        {
            get { return _ptcPassword; }
            set
            {
                if (_ptcPassword != value) {
                    _ptcPassword = value;
                    NotifyPropertyDidChange("PtcPassword");
                }
            }
        }
        #endregion

        [JsonIgnore]
        public bool IsGoogleLoginRequired
        {
            get
            {
                return (AuthenticationType == AuthType.Google && string.IsNullOrEmpty(GoogleRefreshToken));
            }
        }

        #region "INotifyPropertyChanged Implementation"
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyDidChange(string property) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        #endregion
    }
}
