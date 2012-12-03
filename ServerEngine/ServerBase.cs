using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using ServerEngine.PacketEngine;
using XiahBLL;

namespace ServerEngine
{
    public abstract class ServerBase : IDisposable
    {
        private int port;
        private IPAddress ipAddress;
        public event EventHandler<ClientConnectionEventArgs> ClientConnected;
        public event EventHandler ClientDisposed;
        public event EventHandler<ClientExceptionEventArgs> ClientExceptionOccoured;
        protected List<Client> clients = new List<Client>();
        private Socket listener;
        private Thread asyncListen;


        Random rand;

        public ServerBase()
            : this(IPAddress.Loopback.ToString(), 8600)
        {
        }

        public ServerBase(string ipAddress, int port)
        {
            this.ipAddress = IPAddress.Parse(ipAddress);
            this.port = port;
            rand = new Random();
        }

        public int Port
        {
            get { return port; }
        }

        public IPAddress IPAddress
        {
            get { return ipAddress; }
        }

        public void BeginListen()
        {
            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(ipAddress, port));
            listener.Listen(100);

            asyncListen = new Thread(AsyncListen);
            asyncListen.IsBackground = true;
            asyncListen.Start();
        }

        private void AsyncListen()
        {
            while (true)
            {
                Socket client = listener.Accept();
                byte key = (byte)rand.Next(1, 255);
                Client serverClient = new Client(client, key);
                clients.Add(serverClient);

                OnClientConnected(new ClientConnectionEventArgs(serverClient));
                serverClient.ClientExceptionOccoured += new EventHandler<ClientExceptionEventArgs>(serverClient_ClientExceptionOccoured);
                serverClient.ClientDisposed += new EventHandler(serverClient_ClientDisposed);

                serverClient.BeginReceive();
            }
        }

        protected virtual void serverClient_ClientDisposed(object sender, EventArgs e)
        {
            if (ClientDisposed != null)
                ClientDisposed(sender, e);
            Client c = (Client)sender;

            clients.Remove(c);
        }

        void serverClient_ClientExceptionOccoured(object sender, ClientExceptionEventArgs e)
        {
            if (ClientExceptionOccoured != null)
                ClientExceptionOccoured(sender, e);
        }

        protected virtual void OnClientConnected(ClientConnectionEventArgs e)
        {
            if (ClientConnected != null)
                ClientConnected(this, e);
        }

        protected virtual void SendToAll(byte[] p)
        {
            SendSpecific(clients.ToArray(), p);
        }

        protected virtual void SendSpecific(Client[] specificClients, byte[] p)
        {
            for (int i = 0; i < specificClients.Length; i++)
            {
                specificClients[i].Send(p);
            }
        }

        public void Dispose()
        {
            asyncListen.Abort();
            listener.Close();

            foreach (Client c in clients)
            {
                c.Dispose();
            }
        }
    }
}
