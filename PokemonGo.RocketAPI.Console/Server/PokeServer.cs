using PokemonGo.RocketAPI.Console.Server.Callbacks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using Newtonsoft.Json.Linq;
using PokemonGo.RocketAPI.Console.Server.Models.Common;
using PokemonGo.RocketAPI.Console.Server.Models.Constants;
using PokemonGo.RocketAPI.Console.Server.Models;
using PokemonGo.RocketAPI.GeneratedCode;

namespace PokemonGo.RocketAPI.Console.Server
{
    public class PokeServer : Core.Server, IServerCallBack
    {
        private List<Device> devices = new List<Device>();
        private List<IServerCallBack> callbacks = new List<IServerCallBack>();
        public PokeServer(int port = 4711) : base(port)
        {
            base.setIServerCallBack(this);
            base.startServer();
        }

        public void addCallBack(IServerCallBack callback)
        {
            callbacks.Add(callback);
        }

        #region Public Accessible Methods
        public List<Device> getDevices()
        {
            return devices;
        }

        public Device getDeviceAtIndex(int index)
        {
            return devices[index];
        }

        #endregion

        #region Listeners

        /// <summary>
        /// When the client is connected we will want to add it to the list.
        /// We will add the identity of the device aswell
        /// </summary>
        /// <param name="client"></param>
        void IServerCallBack.OnClientConnected(TcpClient client)
        {
            this.devices.Add(Device.Factory(client));
            foreach(IServerCallBack cb in callbacks){
                cb.OnClientConnected(client);
            }
        }

        /// <summary>
        /// When the client is disconnected we will want to remove it from the list.
        /// </summary>
        /// <param name="client"></param>
        void IServerCallBack.OnClientDisconnected(TcpClient client)
        {
            this.devices.RemoveAll(p => p.client.Client == client.Client);
            foreach (IServerCallBack cb in callbacks)
            {
                cb.OnClientDisconnected(client);
            }
        }

        /// <summary>
        /// When the message is recieved here, we will check the status of that model to signal what to do.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="json"></param>
        void IServerCallBack.OnClientMessageRecieved(TcpClient client, JObject json)
        {
            switch (Convert.ToInt32(json["status"]))
            {
                case Constants.IDENTITY:
                    System.Console.WriteLine(json["message"]);
                    break;
            }

            foreach (IServerCallBack cb in callbacks)
            {
                cb.OnClientMessageRecieved(client,json);
            }
        }

        /// <summary>
        /// If a message was recieved and it is not in a json format, the content will appear in this function
        /// </summary>
        /// <param name="client"></param>
        /// <param name="message"></param>
        void IServerCallBack.OnClientMessageRecieved(TcpClient client, string message)
        {
            foreach (IServerCallBack cb in callbacks)
            {
                cb.OnClientMessageRecieved(client, message);
            }
        }

        public void sendData<T>(ResponseModel<T> model)
        {
            devices.ForEach(a =>
            {
                if (a.client.Connected)
                {
                    base.sendData<T>(a.client, model);
                }
            });
        }

        #endregion
    }
}
