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
            Command = "ForceUsePokestop";
        }

        public async Task Handle(ISession session, WebSocketSession webSocketSession, dynamic message)
        {
            Logic.Tasks.UseNearbyPokestopsTask.ForceUsePokestopLat = (double)message.lat;
            Logic.Tasks.UseNearbyPokestopsTask.ForceUsePokestopLng = (double)message.lng;
            Logic.Tasks.UseNearbyPokestopsTask.ForceUsePokestopEnabled = true;
            //await Logic.Tasks.SetDestinationActionTask.Execute(lat, lng, session);
        }
    }
}
