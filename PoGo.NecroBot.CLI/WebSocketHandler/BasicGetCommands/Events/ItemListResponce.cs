using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoGo.NecroBot.CLI.WebSocketHandler.BasicGetCommands.Events
{
    class ItemListResponce : IWebSocketResponce
    {
        public ItemListResponce(dynamic data)
        {
            Command = "ItemListWeb";
            Data = data;
        }
        public string Command { get; private set; }
        public dynamic Data { get; private set; }
    }
}
