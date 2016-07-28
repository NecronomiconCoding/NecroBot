using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Settings;

using PokemonGo.RocketAPI;
using PokemonGo.RocketAPI.Enums;

namespace PoGo.NecroBot.Logic.Profiles {
    public class Profile : IProfile, ISettings {
        private IProfileSettings _settings;

        public string Name { get; set; }
        
        public string FilePath
        {
            get
            {
                return Path.Combine(Profile.BasePath, Name);
            }
        }
        public IProfileSettings Settings { get { return _settings; } }


        public Profile(IProfileSettings settings) {
            _settings = settings;
        }

        #region "ISettings Implementation"
        public AuthType AuthType
        {
            get { return Settings.Account.AuthenticationType; }
            set { Settings.Account.AuthenticationType = value; }
        }
        public double DefaultLatitude
        {
            get { return Settings.Bot.DefaultLatitude; }
            set { Settings.Bot.DefaultLatitude = value; }
        }
        public double DefaultLongitude
        {
            get { return Settings.Bot.DefaultLongitude; }
            set { Settings.Bot.DefaultLongitude = value; }
        }
        public double DefaultAltitude
        {
            get { return Settings.Bot.DefaultAltitude; }
            set { Settings.Bot.DefaultAltitude = value; }
        }
        public string GoogleRefreshToken
        {
            get { return Settings.Account.GoogleRefreshToken; }
            set { Settings.Account.GoogleRefreshToken = value; }
        }
        public string PtcUsername
        {
            get { return Settings.Account.PtcUsername; }
            set { Settings.Account.PtcUsername = value; }
        }
        public string PtcPassword
        {
            get { return Settings.Account.PtcPassword; }
            set { Settings.Account.PtcPassword = value; }
        }
        public string GoogleUsername
        {
            get { return Settings.Account.GoogleUsername; }
            set { Settings.Account.GoogleUsername = value; }
        }

        public string GooglePassword
        {
            get { return Settings.Account.GooglePassword; }
            set { Settings.Account.GooglePassword = value; }
        }
        #endregion

        public static string BasePath
        {
            get
            {
                return Path.Combine(Directory.GetCurrentDirectory(), "Config", "Profiles");
            }
        }
        public static IEnumerable<IProfile> LoadAll() {
            List<IProfile> profiles = new List<IProfile>();

            if (!Directory.Exists(BasePath))
                Directory.CreateDirectory(BasePath);

            foreach(var file in Directory.GetFiles(BasePath)) {
                string profileName = Path.GetFileNameWithoutExtension(file);
                var profile = Load(profileName);
                if (profile != null)
                    profiles.Add(profile);
            }

            return profiles;
        }

        public static IProfile Load(string name) {
            string profilePath = Path.Combine(BasePath, string.Format("{0}.json", name));
            IProfileSettings settings = new ProfileSettings(profilePath);
            return new Profile(settings) { Name = name };
        }

        public static bool Exists(string name) {
            string profilePath = Path.Combine(BasePath, string.Format("{0}.json"));
            return File.Exists(profilePath);
        }
    }
}
