#region using directives

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using PoGo.NecroBot.Logic.State;
using SuperSocket.WebSocket;

#endregion

namespace PoGo.NecroBot.CLI.WebSocketHandler.ActionCommands
{
    public class SetConfigHandler : IWebSocketRequestHandler
    {
        public SetConfigHandler()
        {
            Command = "SetConfig";
        }

        public string Command { get; }

        public async Task Handle(ISession session, WebSocketSession webSocketSession, dynamic message)
        {
            var profilePath = Path.Combine(Directory.GetCurrentDirectory(), "");
            var profileConfigPath = Path.Combine(profilePath, "config");
            var authFile = Path.Combine(profileConfigPath, "auth.json");
            var configFile = Path.Combine(profileConfigPath, "config.json");

            var jsonSerializeSettings = new JsonSerializerSettings
            {
                DefaultValueHandling = DefaultValueHandling.Include,
                Formatting = Formatting.Indented,
                Converters = new JsonConverter[] {new StringEnumConverter {CamelCaseText = true}}
            };

            try
            {
                var authJson = JsonConvert.SerializeObject((JObject) message.AuthJson, jsonSerializeSettings);
                if (!string.IsNullOrEmpty(authJson) && authJson != "null")
                    File.WriteAllText(authFile, authJson, Encoding.UTF8);
            }
            catch (Exception)
            {
                // ignored
            }

            try
            {
                var configJson = JsonConvert.SerializeObject((JObject) message.ConfigJson, jsonSerializeSettings);
                if (!string.IsNullOrEmpty(configJson) && configJson != "null")
                    File.WriteAllText(configFile, configJson, Encoding.UTF8);
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}