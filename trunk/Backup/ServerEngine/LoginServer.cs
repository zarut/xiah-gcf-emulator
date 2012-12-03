using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XiahBLL;
using System.Threading;
using Entities;
using ServerEngine.PacketEngine;
using System.Configuration;

namespace ServerEngine
{
    public class LoginServer : ServerBase
    {
        private WorldInfo[] worldInfos;
        private WorldManager worldManager;
        private AccountManager accountManager;

        public event EventHandler<ClientLoginEventArgs> ClientLogin;
        public event EventHandler<ClientWorldSelectedEventArgs> ClientWorldSelected;
        

        public LoginServer(string ipAddress, int port)
            : base(ipAddress, port)
        {
            worldManager = new WorldManager(ConfigurationManager.ConnectionStrings["XiahDb"].ConnectionString, ConfigurationManager.ConnectionStrings["XiahDb"].ProviderName);
            accountManager = new AccountManager(ConfigurationManager.ConnectionStrings["XiahDb"].ConnectionString, ConfigurationManager.ConnectionStrings["XiahDb"].ProviderName);
            InitiateWorlds();
        }

        private void InitiateWorlds()
        {
            worldInfos = worldManager.GetAllWorlds();
        }

        public WorldInfo[] Worlds
        {
            get { return worldInfos; }
        }

        protected override void OnClientConnected(ClientConnectionEventArgs e)
        {
            base.OnClientConnected(e);
            Client c = e.Client;

            c.ClientLogin += new EventHandler<ClientLoginEventArgs>(c_ClientLogin);
            c.ClientWorldSelected += new EventHandler<ClientWorldSelectedEventArgs>(c_ClientWorldSelected);
        }
  
        void c_ClientLogin(object sender, ClientLoginEventArgs e)
        {
            if (ClientLogin != null)
                ClientLogin(sender, e);

            Client c = (Client)sender;
            GameLogin g = e.GameLoginDetails;

            int accountId = -1;

            if (accountManager.LoginVerification(g.AccountName, ref accountId))
            {
                byte[] bufferLoginState = PacketManager.SendLoginState(LoginState.Success, accountId);
                c.Send(bufferLoginState);

                byte[] bufferWorldList = PacketManager.SendWorldList(worldInfos);
                c.Send(bufferWorldList);
            }
            else
            {
                byte[] bufferLoginState = PacketManager.SendLoginState(LoginState.Failure, 0);
                c.Send(bufferLoginState);
            }
            //do c.Send() with w/e you need here
        }


        void c_ClientWorldSelected(object sender, ClientWorldSelectedEventArgs e)
        {
            if (ClientWorldSelected != null)
                ClientWorldSelected(sender, e);

            Client c = (Client)sender;
            int world = e.WorldID;

            var worldInfo = worldInfos.Where(x => x.WorldId == world).First();

            byte[] bufferWorldSelected = PacketManager.SendWorldIP(worldInfo.IPAddress, worldInfo.Port);
            c.Send(bufferWorldSelected);
        }
    }
}
