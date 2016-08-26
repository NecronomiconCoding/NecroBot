using System.Threading.Tasks;
using PoGo.NecroBot.CLI.WebSocketHandler.GetCommands.Tasks;
using PoGo.NecroBot.Logic.State;
using SuperSocket.WebSocket;

namespace PoGo.NecroBot.CLI.WebSocketHandler.GetCommands
{
    class GetSnipeQueueHandler : IWebSocketRequestHandler
    {
        public string Command { get; private set; }

        public GetSnipeQueueHandler()
        {
            Command = "GetSnipeQueue";
        }

        public async Task Handle(ISession session, WebSocketSession webSocketSession, dynamic message)
        {
            await GetSnipeQueueTask.Execute(session, webSocketSession, (string)message.RequestID);
        }

    }
}
