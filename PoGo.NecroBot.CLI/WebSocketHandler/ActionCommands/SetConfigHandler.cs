using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using PoGo.NecroBot.Logic.State;
using SuperSocket.WebSocket;

namespace PoGo.NecroBot.CLI.WebSocketHandler.ActionCommands
{
    public class SetConfigHandler : IWebSocketRequestHandler
    {
        public string Command { get; private set;}

        public SetConfigHandler()
        {
            Command = "SetConfig";
        }

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
                if ((string) message.Tye == "Auth")
                {
                    var jObj =  session.GlobalSettings.Auth.JsonObject;
                    jObj[message.Id] = (JObject)message.Data;
                }
                else if ((string)message.Tye == "Config")
                {
                    var jObj = session.GlobalSettings.JsonObject;
                    jObj[message.Id] = (JObject)message.Data;
                }
                else if ((string)message.Tye == "Finalize")
                {
                    //session.GlobalSettings.Auth.JsonObject.ToString();
                }
                else if ((string)message.Tye == "Reload")
                {
                    session.GlobalSettings.Auth.Load(session.GlobalSettings.Auth.JsonObject);
                    session.GlobalSettings.Load(session.GlobalSettings.JsonObject);
                    session.LogicSettings.rese
                }
            }
            catch (Exception)
            {
                // ignored
            }


        }
    }
}
