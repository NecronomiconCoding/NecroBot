using PoGo.NecroBot.CLI.WebSocketHandler.GetCommands.Tasks;
using PoGo.NecroBot.Logic.State;
using SuperSocket.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoGo.NecroBot.CLI.WebSocketHandler.GetCommands
{
    class GetItemsListHandler : IWebSocketRequestHandler
    {
        public string Command { get; private set; }

        public GetItemsListHandler()
        {
            Command = "GetItemsList";
        }

        public async Task Handle(ISession session, WebSocketSession webSocketSession, dynamic message)
        {
            await GetItemListTask.Execute(session, webSocketSession, (string)message.RequestID);
        }

    }
}
