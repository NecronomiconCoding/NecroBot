using System.Threading.Tasks;
using PoGo.NecroBot.Logic.State;
using SuperSocket.WebSocket;
using System.Threading;

namespace PoGo.NecroBot.CLI.WebSocketHandler.ActionCommands
{
    public class SetDestination : IWebSocketRequestHandler
    {
        public string Command { get; private set;}

        public SetDestination()
        {
            Command = "SetDestination";
        }

        public async Task Handle(ISession session, WebSocketSession webSocketSession, dynamic message)
        {
            Logic.Tasks.UseNearbyPokestopsTask.lat = (double)message.lat;
            Logic.Tasks.UseNearbyPokestopsTask.lng = (double)message.lng;
            Logic.Tasks.UseNearbyPokestopsTask.SetDestinationEnabled = true;
            //await Logic.Tasks.SetDestinationActionTask.Execute(lat, lng, session);
        }
    }
}
