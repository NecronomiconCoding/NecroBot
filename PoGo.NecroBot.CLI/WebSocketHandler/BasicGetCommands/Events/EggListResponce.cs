using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoGo.NecroBot.CLI.WebSocketHandler.BasicGetCommands.Events
{
    public class EggListResponce : IWebSocketResponce
    {
        public EggListResponce(dynamic data)
        {
            Command = "EggListWeb";
            Data = data;
        }
        public string Command { get; private set; }
        public dynamic Data { get; private set; }
    }
}
