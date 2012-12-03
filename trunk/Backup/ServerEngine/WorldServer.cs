using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XiahBLL;
using Entities;
using ServerEngine.PacketEngine;
using System.Configuration;

namespace ServerEngine
{
    public class WorldServer : ServerBase
    {
        private ChannelInfo[] channelInfos;
        private CharacterManager characterManager;
        private ChannelManager channelManager;
        public event EventHandler<ClientUnitConnectionInfoEventArgs> ClientUnitConnection;
        public event EventHandler<ClientCharacterCreateInfoEventArgs> ClientCreateCharacterInfo;
        public event EventHandler<ClientChannelRequestEventArgs> ClientChannelRequestInfo;

        public WorldServer(string ipAddress, int port)
            : base(ipAddress, port)
        {
            characterManager = new CharacterManager(ConfigurationManager.ConnectionStrings["XiahDb"].ConnectionString, ConfigurationManager.ConnectionStrings["XiahDb"].ProviderName);
            channelManager = new ChannelManager(ConfigurationManager.ConnectionStrings["XiahDb"].ConnectionString, ConfigurationManager.ConnectionStrings["XiahDb"].ProviderName);
            InitiateChannels();
        }

        private void InitiateChannels()
        {
            channelInfos = channelManager.GetAllChannels();
        }

        protected override void OnClientConnected(ClientConnectionEventArgs e)
        {
            base.OnClientConnected(e);

            Client c = e.Client;

            c.ClientUnitConnectionInfo += new EventHandler<ClientUnitConnectionInfoEventArgs>(c_ClientUnitConnectionInfo);
            c.ClientCreateCharacterInfo += new EventHandler<ClientCharacterCreateInfoEventArgs>(c_ClientCreateCharacterInfo);
            c.ClientChannelRequestInfo += new EventHandler<ClientChannelRequestEventArgs>(c_ClientChannelRequestInfo);
        }

        void c_ClientChannelRequestInfo(object sender, ClientChannelRequestEventArgs e)
        {
            if (ClientChannelRequestInfo != null)
                ClientChannelRequestInfo(sender, e);

            Client c = (Client)sender;
            ChannelRequest cr = e.ChannelRequestDetails;

            foreach (ChannelInfo ci in channelInfos)
            {
                byte[] bufferChannelList = PacketManager.SendChannel(ci, channelInfos.Length);
                c.Send(bufferChannelList);
            }
            

            byte[] bufferChannelStatus = PacketManager.SendChannelStatus();
            c.Send(bufferChannelStatus);
        }

        void c_ClientCreateCharacterInfo(object sender, ClientCharacterCreateInfoEventArgs e)
        {
            if (ClientCreateCharacterInfo != null)
                ClientCreateCharacterInfo(sender, e);

            Client c = (Client)sender;
            Character cc = e.CreateCharacterDetails;

            int characterID = -1;
            if (characterManager.CreateCharacter(cc.Name, cc.Class, c.AccountID, ref characterID))
            {
                // needs checks for name if its in use, if success return with playerid and sends it with this packet
                byte[] bufferCreateCharacter = PacketManager.SendCreateCharacter(CharCreationState.Success, characterID);
                c.Send(bufferCreateCharacter);

                // if it success creating character, needs to load new list and send it to client
                Character[] charList = characterManager.GetAllCharactersByAccountId(c.AccountID);

                byte[] bufferCharacterList = PacketManager.SendCharacterList(charList);
                c.Send(bufferCharacterList);
            }
            else
            {
                byte[] bufferCreateCharacter = PacketManager.SendCreateCharacter(CharCreationState.NameInUse, characterID);
                c.Send(bufferCreateCharacter);
            }
        }

        void c_ClientUnitConnectionInfo(object sender, ClientUnitConnectionInfoEventArgs e)
        {
            if (ClientUnitConnection != null)
                ClientUnitConnection(sender, e);

            Client c = (Client)sender;
            UnitLogin u = e.UnitLoginDetails;

            int accountId = u.AccountID;

            byte[] bufferUniLoginState = PacketManager.SendUnitLoginState(LoginState.Success, u.CharacterID);
            c.Send(bufferUniLoginState);

            Character[] charList = characterManager.GetAllCharactersByAccountId(accountId);

            byte[] bufferCharacterList = PacketManager.SendCharacterList(charList);
            c.Send(bufferCharacterList);
        }
    }
}
