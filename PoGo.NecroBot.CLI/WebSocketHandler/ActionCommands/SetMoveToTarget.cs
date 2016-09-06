using System.Threading.Tasks;
using PoGo.NecroBot.Logic.State;
using SuperSocket.WebSocket;
using System.Threading;

namespace PoGo.NecroBot.CLI.WebSocketHandler.ActionCommands
{
    public class SetDestination : IWebSocketRequestHandler
    {
        public string Command { get; private set;}

        public SetDestination()
        {
            Command = "SetMoveToTarget";
        }

        public async Task Handle(ISession session, WebSocketSession webSocketSession, dynamic message)
        {
            Logic.Tasks.SetMoveToTargetTask.Execute((double)message.lat, (double)message.lng);
        }
    }
}
