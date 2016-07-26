using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGo.RocketAPI.Console.Server.Models.Common
{
    public class Device : DeviceModel
    {
        public static Device Factory(TcpClient client)
        {
            Device d = new Device();
            d.client = client;
            return d;
        }
        public TcpClient client;
    }
}
