#region using directives

using System.IO;
using System.Text;
using System.Threading.Tasks;
using PoGo.NecroBot.CLI.WebSocketHandler.GetCommands.Events;
using PoGo.NecroBot.CLI.WebSocketHandler.GetCommands.Helpers;
using PoGo.NecroBot.Logic.State;
using SuperSocket.WebSocket;

#endregion

namespace PoGo.NecroBot.CLI.WebSocketHandler.GetCommands.Tasks
{
    internal class GetConfigTask
    {
        public static async Task Execute(ISession session, WebSocketSession webSocketSession, string requestID)
        {
            var profilePath = Path.Combine(Directory.GetCurrentDirectory(), "");
            var profileConfigPath = Path.Combine(profilePath, "config");

            var authFile = Path.Combine(profileConfigPath, "auth.json");
            var authSchemaFile = Path.Combine(profileConfigPath, "auth.schema.json");
            var authJson = File.ReadAllText(authFile, Encoding.UTF8);
            var authSchemaJson = File.ReadAllText(authSchemaFile, Encoding.UTF8);

            var configFile = Path.Combine(profileConfigPath, "config.json");
            var configSchemaFile = Path.Combine(profileConfigPath, "config.schema.json");
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