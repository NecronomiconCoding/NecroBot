using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGo.RocketAPI.Console.Server.Callbacks
{
    public interface IServerCallBack
    {
        void OnClientConnected(TcpClient client);
        void OnClientDisconnected(TcpClient client);

        void OnClientMessageRecieved(TcpClient client, string message);
        void OnClientMessageRecieved(TcpClient client, JObject json);

    }
}
