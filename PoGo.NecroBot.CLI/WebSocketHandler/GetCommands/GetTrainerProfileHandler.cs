using System.Threading.Tasks;
using PoGo.NecroBot.CLI.WebSocketHandler.GetCommands.Tasks;
using PoGo.NecroBot.Logic.State;
using SuperSocket.WebSocket;
using System.Threading.Tasks;

namespace PoGo.NecroBot.CLI.WebSocketHandler.GetCommands
{
    internal class GetTrainerProfileHandler : IWebSocketRequestHandler
    {
        public string Command { get; private set; }

        public GetTrainerProfileHandler()
        {
            Command = "GetTrainerProfile";
        }

        public async Task Handle(ISession session, WebSocketSession webSocketSession, dynamic message)
        {
            await GetTrainerProfileTask.Execute(session, webSocketSession, (string)message.RequestID);
        }
    }
}