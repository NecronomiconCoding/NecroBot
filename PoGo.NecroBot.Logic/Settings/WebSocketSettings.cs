using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace PoGo.NecroBot.Logic.Settings {
    public class WebSocketSettings : SettingsFile {
        [JsonIgnore]
        private int _webSocketPort = 14251;
        [JsonIgnore]
        private string _locale = "en";

        public int WebSocketPort
        {
            get { return _webSocketPort; }
            set { _webSocketPort = value; }
        }

        public string Locale
        {
            get { return _locale; }
            set { _locale = value; }
        }

        public WebSocketSettings() {
            FilePath = Path.Combine(Directory.GetCurrentDirectory(), "Config", "websockets.json");

            LoadOrInitializeFileAsync();
        }
    }
}
