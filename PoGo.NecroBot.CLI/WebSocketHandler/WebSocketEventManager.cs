using SuperSocket.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Tasks;
using System.Reflection;
using PoGo.NecroBot.Logic.State;

namespace PoGo.NecroBot.CLI.WebSocketHandler
{
    class WebSocketEventManager
    {
        Dictionary<string, IWebSocketRequestHandler> _registerdHandlers = new Dictionary<string, IWebSocketRequestHandler>();
        
        public void RegisterHandler(string actionName, IWebSocketRequestHandler action)
        { 
            try
            {
                _registerdHandlers.Add(actionName,action);
            }
            catch
            {
                // ignore
            } 
        }

        public async Task Handle(ISession session, WebSocketSession webSocketSession, dynamic message)
        {
            if (_registerdHandlers.ContainsKey((string)message.Command))
            {
                await _registerdHandlers[(string)message.Command].Handle(session, webSocketSession, message);
            }
            else
            {
                // Unknown command.
            }

            
        }

        // Registers all IWebSocketRequestHandler's automatically. 

        public static WebSocketEventManager CreateInstance()
        {
            var manager = new WebSocketEventManager();

            var type = typeof(IWebSocketRequestHandler);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && p.IsClass);

            foreach(var plugin in types)
            {
                IWebSocketRequestHandler instance = (IWebSocketRequestHandler)Activator.CreateInstance(plugin);
                manager.RegisterHandler(instance.Command, instance);
            }

            return manager;
        }
    }
}
