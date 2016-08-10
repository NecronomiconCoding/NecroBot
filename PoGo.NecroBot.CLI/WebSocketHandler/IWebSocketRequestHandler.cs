using PoGo.NecroBot.Logic.State;
using SuperSocket.WebSocket;
using System.Threading.Tasks;

namespace PoGo.NecroBot.CLI.WebSocketHandler
{
    internal interface IWebSocketRequestHandler
    {
        string Command { get; }

        Task Handle(ISession session, WebSocketSession webSocketSession, dynamic message);
    }
}