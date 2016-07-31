using PoGo.NecroBot.Logic.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoGo.NecroBot.CLI.WebSocketHandler.BasicGetCommands.Events
{
    public class PokemonListResponce : IWebSocketResponce, IEvent
    {
        public PokemonListResponce(dynamic data)
        {
            Command = "PokemonListWeb";
            Data = data;
        }
        public string Command { get; private set; }
        public dynamic Data { get; private set; }
    }
}
