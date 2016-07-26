using Newtonsoft.Json;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.State;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
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
            bool setupComplete = _server.Setup(new ServerConfig
            {
                Name = "NecroWebSocket",
                Ip = "Any",
                Port = port,
                Mode = SocketMode.Tcp,
                Security = "tls",
                Certificate = new CertificateConfig
                {
                    FilePath = @"cert.pfx",
                    Password = "necro"
                }
            });

            if(setupComplete == false)
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

        public void Listen(IEvent evt, Context ctx)
        {
            dynamic eve = evt;

            var jsonSerializerSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            };

            Broadcast(JsonConvert.SerializeObject(eve, Formatting.None, jsonSerializerSettings));
        }
    }
}
