#region using directives

using System.Threading.Tasks;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Tasks;
using POGOProtos.Inventory.Item;
using SuperSocket.WebSocket;

#endregion

namespace PoGo.NecroBot.CLI.WebSocketHandler.ActionCommands
{
    public class DropItemHandler : IWebSocketRequestHandler
    {
        public DropItemHandler()
        {
            Command = "DropItem";
        }

        public string Command { get; }

        public async Task Handle(ISession session, WebSocketSession webSocketSession, dynamic message)
        {
            await RecycleItemsTask.DropItem(session, (ItemId) message.ItemId, (int) message.Count);
        }
    }
}