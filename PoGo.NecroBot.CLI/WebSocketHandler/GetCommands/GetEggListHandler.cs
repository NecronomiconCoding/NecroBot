using System.Threading.Tasks;
using PoGo.NecroBot.CLI.WebSocketHandler.GetCommands.Tasks;
using PoGo.NecroBot.Logic.State;
using SuperSocket.WebSocket;

namespace PoGo.NecroBot.CLI.WebSocketHandler.GetCommands
{
    class GetEggListHandler : IWebSocketRequestHandler
    {
        public string Command { get; private set; }

        public GetEggListHandler()
        {
            Command = "GetEggList";
        }

        public async Task Handle(ISession session, WebSocketSession webSocketSession, dynamic message)
        {
            await GetEggListTask.Execute(session, webSocketSession, (string)message.RequestID);
        }
    }
}
