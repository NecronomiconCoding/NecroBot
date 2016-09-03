#region using directives

using System.Threading.Tasks;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Tasks;
using SuperSocket.WebSocket;

#endregion

namespace PoGo.NecroBot.CLI.WebSocketHandler.ActionCommands
{
    public class EvolvePokemonHandler : IWebSocketRequestHandler
    {
        public EvolvePokemonHandler()
        {
            Command = "EvolvePokemon";
        }

        public string Command { get; }

        public async Task Handle(ISession session, WebSocketSession webSocketSession, dynamic message)
        {
            await EvolveSpecificPokemonTask.Execute(session, (ulong) message.PokemonId);
        }
    }
}