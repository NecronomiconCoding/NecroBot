using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.WebSocket;
using PoGo.NecroBot.CLI.WebSocketHandler.BasicGetCommands.Tasks;
using PoGo.NecroBot.Logic.State;

namespace PoGo.NecroBot.CLI.WebSocketHandler.BasicGetCommands
{
    public class TransferPokemonHandler : IWebSocketRequestHandler
    {
        public string Command { get; private set;}

        public TransferPokemonHandler()
        {
            Command = "TransferPokemon";
        }

        public async Task Handle(ISession session, WebSocketSession webSocketSession, dynamic message)
        {
            await TransferPokemonTask.Execute(session, webSocketSession, (ulong)message.PokemonId, (string)message.RequestID);
        }
    }
}
