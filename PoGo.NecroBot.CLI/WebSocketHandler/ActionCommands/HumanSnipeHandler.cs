using System.Threading.Tasks;
using PoGo.NecroBot.Logic.State;
using SuperSocket.WebSocket;
using POGOProtos.Inventory.Item;

namespace PoGo.NecroBot.CLI.WebSocketHandler.ActionCommands
{
    public class HumanSnipeHandler : IWebSocketRequestHandler
    {
        public string Command { get; private set;}

        public HumanSnipeHandler()
        {
            Command = "SnipePokemon";
        }

        public async Task Handle(ISession session, WebSocketSession webSocketSession, dynamic message)
        {
            await Logic.Tasks.HumanWalkSnipeTask.TargetPokemonSnip(session, (string)message.Id);
            await Task.Delay(1000);// Logic.Tasks.RecycleItemsTask.DropItem(session, (ItemId)message.ItemId, (int)message.Count);
        }
    }
}
