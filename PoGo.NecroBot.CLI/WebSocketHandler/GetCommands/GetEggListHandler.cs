#region using directives

using System.Threading.Tasks;
using PoGo.NecroBot.CLI.WebSocketHandler.GetCommands.Tasks;
using PoGo.NecroBot.Logic.State;
using SuperSocket.WebSocket;

#endregion

namespace PoGo.NecroBot.CLI.WebSocketHandler.GetCommands
{
    internal class GetEggListHandler : IWebSocketRequestHandler
    {
        public GetEggListHandler()
        {
            Command = "GetEggList";
        }

        public string Command { get; }

        public async Task Handle(ISession session, WebSocketSession webSocketSession, dynamic message)
        {
            await GetEggListTask.Execute(session, webSocketSession, (string) message.RequestID);
        }
    }
}