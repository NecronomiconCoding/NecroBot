using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using PokemonGo.RocketAPI.Enums;


namespace PoGo.NecroBot.UI.Config {
    internal class AuthSettings {
        [JsonIgnore]
        private string FilePath;

        [JsonIgnore]
        public bool IsConfigured { get; set; }
                
        public AuthType AuthType            { get; set; }
        public string   GoogleRefreshToken  { get; set; }
        public string   PtcUsername         { get; set; }
        public string   PtcPassword         { get; set; }

        public AuthSettings() {
            IsConfigured = false;
            string _documentsPath = Directory.GetCurrentDirectory();
            string _relativePath = @"config";
            FilePath = Path.Combine(_documentsPath, _relativePath, "auth.json");
            Load();

            if (!AuthType.Equals(null) && (!string.IsNullOrEmpty(GoogleRefreshToken) || (!string.IsNullOrEmpty(PtcUsername) && !string.IsNullOrEmpty(PtcPassword)))) {
                IsConfigured = true;
            }
        }

        public void Load() {
            if (File.Exists(FilePath)) {
                string _input = File.ReadAllText(FilePath);

                JsonSerializerSettings _settings = new JsonSerializerSettings();
                _settings.Converters.Add(new StringEnumConverter { CamelCaseText = true });

                JsonConvert.PopulateObject(_input, this, _settings);
            } else {
                Save();
            }
        }

        public void Save() {
            string _output = JsonConvert.SerializeObject(this, Formatting.Indented, new StringEnumConverter { CamelCaseText = true });
            string _folder = Path.GetDirectoryName(FilePath);
            if (!Directory.Exists(_folder))
                Directory.CreateDirectory(_folder);

            File.WriteAllText(FilePath, _output);
        }
    }
}
