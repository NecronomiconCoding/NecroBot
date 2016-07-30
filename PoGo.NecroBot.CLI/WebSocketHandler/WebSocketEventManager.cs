﻿using SuperSocket.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Tasks;
using System.Reflection;
using PoGo.NecroBot.Logic.State;

namespace PoGo.NecroBot.CLI.WebSocketHandlers
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
            catch(Exception ex)
            {
                // todo
            } 
        }

        public void Handle(ISession session, WebSocketSession webSocketSession, dynamic message)
        {
            if (_registerdHandlers.ContainsKey((string)message.Command))
            {
                _registerdHandlers[(string)message.Command].Handle(session, webSocketSession, message);
            }
            else
            {



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
