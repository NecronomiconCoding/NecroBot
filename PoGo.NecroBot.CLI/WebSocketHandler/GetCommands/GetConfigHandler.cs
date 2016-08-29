using System.Threading.Tasks;
using PoGo.NecroBot.CLI.WebSocketHandler.GetCommands.Tasks;
using PoGo.NecroBot.Logic.State;
using SuperSocket.WebSocket;

namespace PoGo.NecroBot.CLI.WebSocketHandler.GetCommands
{
    class GetConfigHandler : IWebSocketRequestHandler
    {
        public string Command { get; private set; }

        public GetConfigHandler()
        {
            Command = "GetConfig";
        }

        public async Task Handle(ISession session, WebSocketSession webSocketSession, dynamic message)
        {
            await GetConfigTask.Execute(session, webSocketSession, (string)message.RequestID);
        }
    }
}
