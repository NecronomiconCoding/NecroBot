#region using directives

using System.Threading.Tasks;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Tasks;
using SuperSocket.WebSocket;

#endregion

namespace PoGo.NecroBot.CLI.WebSocketHandler.ActionCommands
{
    public class HumanSnipePriorityPokemonHandler : IWebSocketRequestHandler
    {
        public HumanSnipePriorityPokemonHandler()
        {
            Command = "SnipePokemon";
        }

        public string Command { get; }

        public async Task Handle(ISession session, WebSocketSession webSocketSession, dynamic message)
        {
            await HumanWalkSnipeTask.TargetPokemonSnip(session, (string) message.Id);
            await Task.Delay(1000);
                // Logic.Tasks.RecycleItemsTask.DropItem(session, (ItemId)message.ItemId, (int)message.Count);
        }
    }
}