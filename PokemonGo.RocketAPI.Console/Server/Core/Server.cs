using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PokemonGo.RocketAPI.Console.Server.Callbacks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using PokemonGo.RocketAPI.Console.Server.Models;

namespace PokemonGo.RocketAPI.Console.Server.Core
{
    public abstract class Server
    {
        #region Fields
        private TcpListener tcp;
        private int port;
        private Thread serverLoop;
        private bool end = false;
        private IServerCallBack callback;
        #endregion
        protected Server(int port)
        {
            this.port = port;
        }

        protected void setIServerCallBack(IServerCallBack callback)
        {
            this.callback = callback;
        }

        protected void startServer()
        {
            end = false;
            if (tcp == null) tcp = new TcpListener(IPAddress.Any, port);
            if (serverLoop == null)
                serverLoop = new Thread(new ThreadStart(() =>
                {
                    System.Console.WriteLine("Started");
                    tcp.Start();
                    while (true)
                    {
                        if (end) break;
                        if (tcp.Pending())
                            handleClient(tcp.AcceptTcpClient());
                    }
                }));
            if (!serverLoop.IsAlive)
            {
                serverLoop.Start();
            }

        }
        public void endServer()
        {
            end = true;
        }
        protected void handleClient(TcpClient tcpClient)
        {
            System.Console.WriteLine("Client Connected: " + tcpClient.Client.RemoteEndPoint);
            if(callback != null) callback.OnClientConnected(tcpClient);
            while (tcpClient.Connected)
            {

                if (end) break;
                if (tcpClient.GetStream().DataAvailable)
                    try
                    {
                        byte[] buffer = new byte[1024];
                        int k = tcpClient.Client.Receive(buffer);
                        char cc = ' ';
                        string test = null;
                        System.Console.WriteLine("Recieved...");
                        for (int i = 0; i < k; i++)
                        {
                            cc = Convert.ToChar(buffer[i]);
                            test += cc.ToString();
                        }
                        System.Console.WriteLine(test);

                    }
                    catch
                    {

                    }
              }
            if(callback != null) callback.OnClientDisconnected(tcpClient);
        }

        protected void sendData<T>(TcpClient client, ResponseModel<T> model)
        {
            if (client.Client.Connected)
            {
                client.Client.Send(ASCIIEncoding.ASCII.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(model, Formatting.None).ToString()));
            }
        }
    }
}
