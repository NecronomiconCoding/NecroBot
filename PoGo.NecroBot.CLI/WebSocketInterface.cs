#region using directives

using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PoGo.NecroBot.Logic;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Common;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.Settings;
using PoGo.NecroBot.Logic.Profiles;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperSocket.WebSocket;



#endregion

namespace PoGo.NecroBot.CLI
{
    public class WebSocketInterface
    {
        protected ITranslation I18n { get; set; }
        protected WebSocketSettings Settings { get; set; }
        protected SuperSocket.WebSocket.WebSocketServer Server { get; set; }
        protected Dictionary<Session, SocketMessage> LastPokestopCache { get; set; }
        protected Dictionary<Session, SocketMessage> LastProfileCache { get; set; }

        private Dictionary<string, SocketMessageType> EventTypeMap = new Dictionary<string, SocketMessageType> {
            {"PokeStepListEvent", SocketMessageType.ReceivedPokestopList },
            {"ProfileEvent", SocketMessageType.ReceivedPlayerData }
        };

        public WebSocketInterface() {
            LastPokestopCache = new Dictionary<Session, SocketMessage>();
            LastProfileCache = new Dictionary<Session, SocketMessage>();

            Settings = new WebSocketSettings();
            I18n = Translation.Load(Settings.Locale);
            Server = new SuperSocket.WebSocket.WebSocketServer();

            var complete = Server.Setup(new ServerConfig {
                Name = "NecroWebSocket",
                Ip = "Any",
                Port = Settings.WebSocketPort,
                Mode = SocketMode.Tcp,
                Security = "tls",
                Certificate = new CertificateConfig {
                    FilePath = @"cert.pfx",
                    Password = "necro"
                }
            });

            if (!complete) {
                Logger.Write(I18n.GetTranslation(TranslationString.WebSocketFailStart, Settings.WebSocketPort), LogLevel.Error);
                return;
            }

            Server.Start();
        }

        private void Broadcast(string msg) {
            foreach(var s in Server.GetAllSessions()) {
                try {
                    s.Send(msg);
                } catch { }
            }
        }

        private void HandleEvent(Session s, PokeStopListEvent e, SocketMessage msg) {
            LastPokestopCache[s] = msg;
        }

        private void HandleEvent(Session s, ProfileEvent e, SocketMessage msg) {
            LastProfileCache[s] = msg;
        }

        public void Listen(IEvent evt, Session session) {
            dynamic e = evt;
            var typeName = evt.GetType().Name;
            if (!EventTypeMap.ContainsKey(typeName))
                return;

            var msgType = EventTypeMap[typeName];
            var msg = new SocketMessage(msgType, session, e);
            try { HandleEvent(session, e, msg); } catch { }

            Broadcast(msg.Serialize());
        }

        #region "Message Handling"
        private void SessionDidConnect(WebSocketSession session) {
            foreach(var key in LastPokestopCache.Keys) {
                session.Send(LastPokestopCache[key].Serialize());
            }

            foreach(var key in LastProfileCache.Keys) {
                session.Send(LastProfileCache[key].Serialize());
            }
        }

        private void DidReceiveMessage(WebSocketSession session, string message) {
        }
        #endregion
    }

    public enum SocketMessageType {
        ReceivedPlayerData,
        ReceivedPokestopList
    }

    public class SocketMessage {
        public string SessionID { get; set; }
        public SocketMessageType Type { get; set; }
        public IEvent Data { get; set; }

        public SocketMessage() { }
        public SocketMessage(SocketMessageType type, Session session, IEvent data) {
            SessionID = session.BotProfile.Name;
            Type = type;
            Data = data;
        }

        public string Serialize() {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new StringEnumConverter { CamelCaseText = true });
            return JsonConvert.SerializeObject(this, Formatting.None, settings);
        }
    }
}