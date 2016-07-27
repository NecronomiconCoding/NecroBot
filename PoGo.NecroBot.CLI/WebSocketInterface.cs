#region using directives

using Newtonsoft.Json;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.State;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperSocket.WebSocket;

#endregion

namespace PoGo.NecroBot.CLI
{
    public class WebSocketInterface
    {
        private PokeStopListEvent _lastPokeStopList;
        private ProfileEvent _lastProfile;
        private readonly WebSocketServer _server;

        public WebSocketInterface(int port)
        {
            _server = new WebSocketServer();
            var setupComplete = _server.Setup(new ServerConfig
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

            if (setupComplete == false)
            {
                Logger.Write($"Failed to start WebSocketServer on port : {port}", LogLevel.Error);
                return;
            }

            _server.NewMessageReceived += HandleMessage;
            _server.NewSessionConnected += HandleSession;

            _server.Start();
        }

        private void Broadcast(string message)
        {
            foreach (var session in _server.GetAllSessions())
            {
                try
                {
                    session.Send(message);
                }
                catch
                {
                    // ignored
                }
            }
        }

        private void HandleEvent(PokeStopListEvent evt)
        {
            _lastPokeStopList = evt;
        }

        private void HandleEvent(ProfileEvent evt)
        {
            _lastProfile = evt;
        }

        private void HandleMessage(WebSocketSession session, string message)
        {
        }

        private void HandleSession(WebSocketSession session)
        {
            if (_lastProfile != null)
                session.Send(Serialize(_lastProfile));

            if (_lastPokeStopList != null)
                session.Send(Serialize(_lastPokeStopList));
        }

        public void Listen(IEvent evt, Context ctx)
        {
            dynamic eve = evt;

            try
            {
                HandleEvent(eve);
            }
            catch
            {
                // ignored
            }

            Broadcast(Serialize(eve));
        }

        private string Serialize(dynamic evt)
        {
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };

            return JsonConvert.SerializeObject(evt, Formatting.None, jsonSerializerSettings);
        }
    }
}