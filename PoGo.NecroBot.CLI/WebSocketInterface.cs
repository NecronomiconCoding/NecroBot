using Newtonsoft.Json;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.State;
using SuperSocket.SocketBase;
using SuperSocket.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoGo.NecroBot.CLI
{
    public class WebSocketInterface
    {
        private WebSocketServer _server;

        public WebSocketInterface(int port)
        {
            _server = new WebSocketServer();
            if(_server.Setup(port) == false)
            {
                Logic.Logging.Logger.Write($"Failed to start WebSocketServer on port : {port}", Logic.Logging.LogLevel.Error);
                return;
            }

            _server.NewMessageReceived += HandleMessage;
            _server.NewSessionConnected += HandleSession;

            _server.Start();
        }

        private void HandleMessage(WebSocketSession session, string message)
        {

        }

        private void HandleSession(WebSocketSession session)
        {
        }

        private void Broadcast(string message)
        {
            foreach(var session in _server.GetAllSessions())
            {
                try
                {
                    session.Send(message);
                }
                catch { }
            }
        }

        public void HandleEvent(ProfileEvent evt, Context ctx)
        {
        }

        public void HandleEvent(ErrorEvent evt, Context ctx)
        {
        }

        public void HandleEvent(NoticeEvent evt, Context ctx)
        {
        }

        public void HandleEvent(WarnEvent evt, Context ctx)
        {
        }

        public void HandleEvent(UseLuckyEggEvent evt, Context ctx)
        {
        }

        public void HandleEvent(PokemonEvolveEvent evt, Context ctx)
        {
        }

        public void HandleEvent(TransferPokemonEvent evt, Context ctx)
        {
        }

        public void HandleEvent(ItemRecycledEvent evt, Context ctx)
        {
        }

        public void HandleEvent(FortUsedEvent evt, Context ctx)
        {
        }

        public void HandleEvent(FortTargetEvent evt, Context ctx)
        {
        }

        public void HandleEvent(PokemonCaptureEvent evt, Context ctx)
        {
        }

        public void HandleEvent(NoPokeballEvent evt, Context ctx)
        {
        }

        public void HandleEvent(UseBerryEvent evt, Context ctx)
        {
        }

        public void Listen(IEvent evt, Context ctx)
        {
            dynamic eve = evt;

            var jsonSerializerSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            };

            Broadcast(JsonConvert.SerializeObject(eve, Formatting.None, jsonSerializerSettings));

            HandleEvent(eve, ctx);
        }
    }
}
