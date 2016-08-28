using System.Threading.Tasks;
using PoGo.NecroBot.CLI.WebSocketHandler.GetCommands.Events;
using PoGo.NecroBot.Logic.State;
using SuperSocket.WebSocket;

namespace PoGo.NecroBot.CLI.WebSocketHandler.GetCommands.Tasks
{
    class GetPokemonSnipeListTask
    {
        public static async Task Execute(ISession session, WebSocketSession webSocketSession, string requestID)
        {
            var allItems = await Logic.Tasks.HumanWalkSnipeTask.GetCurrentQueueItems(session);

            webSocketSession.Send(EncodingHelper.Serialize(new SnipeListResponce(allItems, requestID)));
        }
    }
}
