using System.Threading.Tasks;
using PoGo.NecroBot.CLI.WebSocketHandler.GetCommands.Tasks;
using PoGo.NecroBot.Logic.State;
using SuperSocket.WebSocket;

namespace PoGo.NecroBot.CLI.WebSocketHandler.GetCommands
{
    class GetPokemonSnipeListHandler : IWebSocketRequestHandler
    {
        public string Command { get; private set; }

        public GetPokemonSnipeListHandler()
        {
            Command = "PokemonSnipeList";
        }

        public async Task Handle(ISession session, WebSocketSession webSocketSession, dynamic message)
        {
            await Logic.Tasks.HumanWalkSnipeTask.ExecuteFetchData(session);
            //await GetPokemonSnipeListTask.Execute(session, webSocketSession, (string)message.RequestID);
        }

    }
}
