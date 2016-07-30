using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoGo.NecroBot.CLI.WebSocketHandlers
{
    interface IWebSocketResponce
    {
        string Command { get; }
        dynamic Data { get; }
    }
}
