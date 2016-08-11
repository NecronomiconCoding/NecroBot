using PoGo.NecroBot.Logic.State;
using SuperSocket.WebSocket;
using System.Threading.Tasks;

namespace PoGo.NecroBot.CLI.WebSocketHandler.ActionCommands
{
    public class EvolvePokemonHandler : IWebSocketRequestHandler
    {
        public string Command { get; private set; }

        public EvolvePokemonHandler()
        {
            Command = "EvolvePokemon";
        }

        public async Task Handle(ISession session, WebSocketSession webSocketSession, dynamic message)
        {
            await Logic.Tasks.EvolveSpecificPokemonTask.Execute(session, (ulong)message.PokemonId);
        }
    }
}