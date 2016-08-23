using System.Threading.Tasks;
using PoGo.NecroBot.Logic.State;
using SuperSocket.WebSocket;
using POGOProtos.Inventory.Item;

namespace PoGo.NecroBot.CLI.WebSocketHandler.ActionCommands
{
    public class DropItemHandler : IWebSocketRequestHandler
    {
        public string Command { get; private set;}

        public DropItemHandler()
        {
            Command = "DropItem";
        }

        public async Task Handle(ISession session, WebSocketSession webSocketSession, dynamic message)
        {
            await Logic.Tasks.RecycleItemsTask.DropItem(session, (ItemId)message.ItemId, (int)message.Count);
        }
    }
}
