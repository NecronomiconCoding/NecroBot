#region using directives

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PoGo.NecroBot.CLI.WebSocketHandler;
using PoGo.NecroBot.Logic.Common;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Tasks;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperSocket.WebSocket;

#endregion

namespace PoGo.NecroBot.CLI
{
    public class WebSocketInterface
    {
        private readonly WebSocketServer _server;
        private readonly Session _session;
        private readonly WebSocketEventManager _websocketHandler;
        private PokeStopListEvent _lastPokeStopList;
        private ProfileEvent _lastProfile;

        public WebSocketInterface(int port, Session session)
        {
            _session = session;
            var translations = session.Translation;
            _server = new WebSocketServer();
            _websocketHandler = WebSocketEventManager.CreateInstance();
            var config = new ServerConfig
            {
                Name = "NecroWebSocket",
                Mode = SocketMode.Tcp,
                Certificate = new CertificateConfig
                {
                    FilePath = @"cert.pfx",
                    Password = "necro"
                },
                Listeners = new List<ListenerConfig>
                {
                    new ListenerConfig
                    {
                        Ip = "Any",
                        Port = port,
                        Security = "tls"
                    },
                    new ListenerConfig
                    {
                        Ip = "Any",
                        Port = port + 1,
                        Security = "none"
                    }
                }
            };

            var setupComplete = _server.Setup(config);

            if (setupComplete == false)
            {
                Logger.Write(translations.GetTranslation(TranslationString.WebSocketFailStart, port), LogLevel.Error);
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

        private async void HandleMessage(WebSocketSession session, string message)
        {
            switch (message)
            {
                case "PokemonList":
                    await PokemonListTask.Execute(_session);
                    break;
                case "EggsList":
                    await EggsListTask.Execute(_session);
                    break;
                case "InventoryList":
                    await InventoryListTask.Execute(_session);
                    break;
                case "PokemonSnipeList":
                    await HumanWalkSnipeTask.ExecuteFetchData(_session);
                    break;
            }

            // Setup to only send data back to the session that requested it. 
            try
            {
                dynamic decodedMessage = JObject.Parse(message);
                var handle = _websocketHandler?.Handle(_session, session, decodedMessage);
                if (handle != null)
                    await handle;
            }
            catch
            {
                // ignored
            }
        }

        private void HandleSession(WebSocketSession session)
        {
            if (_lastProfile != null)
                session.Send(Serialize(_lastProfile));

            if (_lastPokeStopList != null)
                session.Send(Serialize(_lastPokeStopList));

            try
            {
                session.Send(Serialize(new UpdatePositionEvent
                {
                    Latitude = _session.Client.CurrentLatitude,
                    Longitude = _session.Client.CurrentLongitude
                }));
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public void Listen(IEvent evt, Session session)
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

        private static string Serialize(dynamic evt)
        {
            var jsonSerializerSettings = new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.All};

            // Add custom seriaizer to convert uong to string (ulong shoud not appear to json according to json specs)
            jsonSerializerSettings.Converters.Add(new IdToStringConverter());

            return JsonConvert.SerializeObject(evt, Formatting.None, jsonSerializerSettings);
        }
    }

    public class IdToStringConverter : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var jt = JToken.ReadFrom(reader);
            return jt.Value<long>();
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(long) == objectType || typeof(ulong) == objectType;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value.ToString());
        }
    }
}