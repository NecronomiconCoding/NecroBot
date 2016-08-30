using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoGo.NecroBot.CLI.WebSocketHandler.GetCommands.Events;
using PoGo.NecroBot.CLI.WebSocketHandler.GetCommands.Helpers;
using PoGo.NecroBot.Logic.Model.Settings;
using PoGo.NecroBot.Logic.State;
using POGOProtos.Inventory.Item;
using SuperSocket.WebSocket;

namespace PoGo.NecroBot.CLI.WebSocketHandler.GetCommands.Tasks
{
    class GetConfigTask
    {

        public static async Task Execute(ISession session, WebSocketSession webSocketSession, string requestID)
        {
            var profilePath = Path.Combine(Directory.GetCurrentDirectory(), "");
            var profileConfigPath = Path.Combine(profilePath, "config");
            var authFile = Path.Combine(profileConfigPath, "auth.json");
            var authSchemaFile = Path.Combine(profileConfigPath, "auth.schema.json");
            var configFile = Path.Combine(profileConfigPath, "config.json");
            var configSchemaFile = Path.Combine(profileConfigPath, "config.schema.json");

            var authJson = File.ReadAllText(authFile, Encoding.UTF8);
            var authSchemaJson = File.ReadAllText(authSchemaFile, Encoding.UTF8);
            var configJson = File.ReadAllText(configFile, Encoding.UTF8);
            var configSchemaJson = File.ReadAllText(configSchemaFile, Encoding.UTF8);

            var list = new ConfigWeb
            {
                AuthJson = authJson,
                AuthSchemaJson = authSchemaJson,
                ConfigJson = configJson,
                ConfigSchemaJson = configSchemaJson
            };

            webSocketSession.Send(EncodingHelper.Serialize(new ConfigResponce(list, requestID)));
        }
    }
}
