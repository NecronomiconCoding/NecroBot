using System.Threading.Tasks;
using PoGo.NecroBot.Logic.State;
using SuperSocket.WebSocket;

namespace PoGo.NecroBot.CLI.WebSocketHandler.ActionCommands
{
    public class FavoritePokemonHandler : IWebSocketRequestHandler
    {
        public string Command { get; private set;}

        public FavoritePokemonHandler()
        {
            Command = "FavoritePokemon";
        }

        public async Task Handle(ISession session, WebSocketSession webSocketSession, dynamic message)
        {
            await Logic.Tasks.FavoritePokemonTask.Execute(session, (ulong)message.PokemonId, (bool)message.Favorite);
        }
    }
}
