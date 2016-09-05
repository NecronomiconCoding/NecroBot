#region using directives

using System.Threading.Tasks;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Tasks;
using SuperSocket.WebSocket;

#endregion

namespace PoGo.NecroBot.CLI.WebSocketHandler.ActionCommands
{
    public class FavoritePokemonHandler : IWebSocketRequestHandler
    {
        public FavoritePokemonHandler()
        {
            Command = "FavoritePokemon";
        }

        public string Command { get; }

        public async Task Handle(ISession session, WebSocketSession webSocketSession, dynamic message)
        {
            await FavoritePokemonTask.Execute(session, (ulong) message.PokemonId, (bool) message.Favorite);
        }
    }
}