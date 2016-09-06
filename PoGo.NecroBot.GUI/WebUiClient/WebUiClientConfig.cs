#region using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

#endregion

namespace PoGo.NecroBot.GUI.WebUiClient
{
    [JsonObject(Title = "PoGo.NecroBot.GUI Config", Description = "", ItemRequired = Required.DisallowNull)]
    public class WebUiClientConfig
    {
        [JsonIgnore]
        private string _filePath;

        [DefaultValue(false)]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 1)]
        public bool AutoUpdateWebUiClient;

        [DefaultValue("PokeEase")]
        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Populate, Order = 2)]
        public string CurrentWebUiClient = "PokeEase";

        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Ignore, Order = 3)]
        public Dictionary<string, WebUiClient> WebUiClients = new Dictionary<string, WebUiClient>
        {
            {"PokeEase", new WebUiClient("GediminasMasaitis", "PokeEase", "src", "PokeEase.html")},
            {"PokeEase Fork By jjskuld", new WebUiClient("jjskuld", "PokeEase", "src", "index.html")},
            {"NecrobotVisualizer", new WebUiClient("nicoschmitt", "necrobotvisualizer", "app", "index.html")},
            {"NecrobotJavanHawk", new WebUiClient("AndikaTanpaH", "NecrobotJavanHawk", "app", "index.html")}
        };

        public WebUiClientConfig()
        {
            InitializePropertyDefaultValues(this);
        }

        public void InitializePropertyDefaultValues(object obj)
        {
            var fields = obj.GetType().GetFields();

            foreach (var field in fields)
            {
                var d = field.GetCustomAttribute<DefaultValueAttribute>();

                if (d != null)
                    field.SetValue(obj, d.Value);
            }
        }

        public void Load(string path, bool boolSkipSave = false)
        {
            try
            {
                _filePath = path;
                if (File.Exists(_filePath))
                {
                    // if the file exists, load the settings
                    var input = File.ReadAllText(_filePath, Encoding.UTF8);
                    var settings = new JsonSerializerSettings();
                    settings.Converters.Add(new StringEnumConverter {CamelCaseText = true});
                    JsonConvert.PopulateObject(input, this, settings);
                }
                if (!boolSkipSave)
                    Save();
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public void Save()
        {
            if (string.IsNullOrEmpty(_filePath)) return;
            var jsonSerializeSettings = new JsonSerializerSettings
            {
                DefaultValueHandling = DefaultValueHandling.Include,
                Formatting = Formatting.Indented,
                Converters = new JsonConverter[] {new StringEnumConverter {CamelCaseText = true}}
            };
            var output = JsonConvert.SerializeObject(this, jsonSerializeSettings);

            var folder = Path.GetDirectoryName(_filePath);
            if (folder != null && !Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            File.WriteAllText(_filePath, output, Encoding.UTF8);
        }
    }
}