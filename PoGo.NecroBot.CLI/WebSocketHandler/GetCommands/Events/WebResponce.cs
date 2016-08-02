using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoGo.NecroBot.CLI.WebSocketHandler.GetCommands.Events
{
    public class WebResponce : IWebSocketResponce
    {
        public string RequestID { get; set; }
        public string Command { get; set; }
        public dynamic Data { get; set; }
        
    }
}
