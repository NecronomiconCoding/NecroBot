using System.Threading.Tasks;
using PoGo.NecroBot.Logic.State;
using SuperSocket.WebSocket;

namespace PoGo.NecroBot.CLI.WebSocketHandler.ActionCommands
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
            await Logic.Tasks.TransferPokemonTask.Execute(session, (ulong)message.PokemonId);
        }
    }
}
