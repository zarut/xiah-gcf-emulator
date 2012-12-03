using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using ServerEngine.PacketEngine;
using System.Net;
using Entities;

namespace ServerEngine
{
    public class Client : IDisposable
    {
        private Socket client;
        private byte key;
        private int accountId;
        private Character currentChar;
        private Map currentMap;
        const int packetSize = 65000;
        public event EventHandler<ClientLoginEventArgs> ClientLogin;
        public event EventHandler<ClientWorldSelectedEventArgs> ClientWorldSelected;
        public event EventHandler<ClientUnitConnectionInfoEventArgs> ClientUnitConnectionInfo;
        public event EventHandler<ClientCharacterCreateInfoEventArgs> ClientCreateCharacterInfo;
        public event EventHandler<ClientChannelRequestEventArgs> ClientChannelRequestInfo;

        #region Channel Server Events
        public event EventHandler<ClientChannelLoginInfoEventArgs> ClientChannelLoginInfo;
        public event EventHandler<ClientChannelChangeEventArgs> ClientChannelChangeInfo;
        public event EventHandler<ClientChannelChangeStuffEventArgs> ClientChannelStuffInfo;

        public event EventHandler<ClientWalkToPortalEventArgs> ClientWalkToPortalInfo;
        public event EventHandler<ClientMapLoadEventArgs> ClientMapLoadInfo;
        public event EventHandler<ClientSkillLoadEvenArgs> ClientSkillLoadInfo;

        public event EventHandler<ClientInventoryLoadEvenArgs> ClientInventoryLoadInfo;
        public event EventHandler<ClientMoveItemEventArgs> ClientMoveItemInfo;
        public event EventHandler<ClientDropItemEventArgs> ClientDropItemInfo;
        public event EventHandler<ClientPickItemEventArgs> ClientPickItemInfo;
        public event EventHandler<ClientImbueItemEventArgs> ClientImbueItemInfo;
        public event EventHandler<ClientAcceptImbueItemEventArgs> ClientAcceptImbueItemInfo;

        public event EventHandler<ClientNpcTradeEventArgs> ClientNpcTradeInfo;
        public event EventHandler<ClientBuyItemEventArgs> ClientBuyItemInfo;

        public event EventHandler<ClientStatLoadEventArgs> ClientSpawnStatLoadInfo;
        public event EventHandler<ClientRequestStatsLoadInfo> ClientRequestStatsLoadInfo;
        public event EventHandler<ClientAddStatEventArgs> ClientAddStatInfo;

        public event EventHandler<ClientSpawnLoadEventArgs> ClientSpawnLoadInfo;
        public event EventHandler<ClientSendPlayerIDEventArgs> ClientSendPlayerIDInfo;
        public event EventHandler<ClientSpawnOtherPlayerEventArgs> ClientSpawnOtherPlayerInfo;

        public event EventHandler<ClientMovementInfoEventArgs> ClientMovementInfo;

        public event EventHandler<ClientChatMessageEventArgs> ClientChatMessageInfo;
        public event EventHandler<ClientCommandInfoEventArgs> ClientCommandInfo;

        #endregion

        public event EventHandler<ClientExceptionEventArgs> ClientExceptionOccoured;
        public event EventHandler<UnknownPacketEventArgs> ClientUnknownPacket;

        public event EventHandler ClientDisposed;

        public Client(Socket client, byte key)
        {
            this.client = client;
            this.key = key;
            SendKey();
        }

        public Character MyCharacter
        {
            get { return currentChar; }
            set { currentChar = value; }
        }

        public Map Map
        {
            get { return currentMap; }
            set { currentMap = value; }
        }

        public byte Key
        {
            get { return key; }
        }

        public int AccountID
        {
            get { return accountId; }
            set { accountId = value; }
        }

        public string IPAddress
        {
            get { return ((IPEndPoint)client.RemoteEndPoint).Address.ToString(); }
        }

        public void BeginReceive()
        {
            byte[] buffer = new byte[packetSize];
            client.BeginReceive(buffer, 0, packetSize, 0, RecieveCallBack, buffer);
        }

        private void RecieveCallBack(IAsyncResult result)
        {
            try
            {
                byte[] buffer = result.AsyncState as byte[];
                int bytesRecieved = client.EndReceive(result);
                Packet p = new Packet(buffer);
                p.Length = bytesRecieved;// assign packet length to length of bytes received, so it wont say its 65k
                if (bytesRecieved > 0)
                {
                    HandlePacket(p);
                    BeginReceive();
                }
                else
                {
                    this.Dispose();
                    // disconnected...
                }
            }
            catch (Exception e)
            {
                if(ClientExceptionOccoured!=null)
                        ClientExceptionOccoured(this, new ClientExceptionEventArgs(e));
                this.Dispose();
            }
        }

        private void HandlePacket(Packet p)
        {
            int curByte = 0;
            int maxByte = p.Length;

            while (curByte < maxByte)
            {
                int parsedLength = p.ReadShort((curByte + 2)) + 4;
                byte[] parsed = new byte[parsedLength];

                // read bytes from buffer to parsed packets.
                for (int i = 0; i < parsed.Length; i++)
                {
                    parsed[i] = p.GetBuffer()[i + curByte];
                }

                // decrypt those bytes as a new real deal parsed packet
                Packet Parsed = Packet.Decrypt(new Packet(parsed), key);
                curByte += parsedLength;

                PacketIds PI = (PacketIds)Parsed.ReadShort();
                switch (PI)
                {
                    #region Login Recvs
                    case PacketIds.Recv_GameLogin:
                        HandleRecvGameLogin(Parsed);
                        break;

                    case PacketIds.Recv_ConnectWorld:
                        HandleRecvConnectWorld(Parsed);
                        break;

                    #endregion

                    #region World Recvs

                    case PacketIds.RecvUnitLogin:
                        HandleRecvUnitLogin(Parsed);
                        break;

                    case PacketIds.RecvCreateCharacter:
                        HandleRecvCreateCharacter(Parsed);
                        break;

                    case PacketIds.RecvChannelRequest:
                        HandleRecvChannelRequest(Parsed);
                        break;

                    #endregion

                    #region Channel Recvs
                    case PacketIds.RecvPing:
                        // needs check if too fast or so :I
                        break;

                    case PacketIds.RecvChannelLogin:
                        HandleRecvChannelLogin(Parsed);
                        break;

                    case PacketIds.RecvChannelChange:
                        HandleRecvChannelChange(Parsed);
                        break;

                    case PacketIds.RecvChannelSomething:
                        HandleRecvChannelStuff(Parsed);
                        break;

                    case PacketIds.RecvLoadMap:
                        HandleRecvMapRequest(Parsed);
                        break;

                    case PacketIds.RecvRequestSkillList:
                        HandleRecvSkillRequest(Parsed);
                        break;

                    case PacketIds.RecvRequestInventory:
                        HandleRecvInventoryRequest(Parsed);
                        break;

                    case PacketIds.RecvMoveItem:
                        HandleRecvMoveItem(Parsed);
                        break;
                        
                    case PacketIds.RecvDropItem:
                        HandleRecvDropItem(Parsed);
                        break;

                    case PacketIds.RecvPickItem:
                        HandleRecvPickItem(Parsed);
                        break;

                    case PacketIds.RecvImbueItem:
                        HandleRecvImbueItem(Parsed);
                        break;

                    case PacketIds.RecvAcceptImbueItem:
                        HandleRecvAcceptImbueItem(Parsed);
                        break;

                    case PacketIds.RecvNpcTrade:
                        HandleRecvNpcTrade(Parsed);
                        break;

                    case PacketIds.RecvBuyItem:
                        HandleRecvBuyItem(Parsed);
                        break;

                    case PacketIds.RecvSomething:
                        Send(PacketManager.SendSomething());
                        break;

                    case PacketIds.RecvRequestFriendList:
                        Send(PacketManager.SendFriendList());
                        break;

                    case PacketIds.RecvRequestGuildList:
                        // AD GUILD LIST SEND
                        break;

                    case PacketIds.RecvSomething2:
                        Send(PacketManager.SendSomething2());
                        break;

                    case PacketIds.RecvStatRequest:
                        HandleRecvStatRequest(Parsed);
                        break;

                    case PacketIds.RecvSpawnRequest:
                        HandleRecvSpawnRequest(Parsed);
                        break;

                    case PacketIds.RecvSpawnPlayer: // request for other player to spawn
                        HandleRecvSpawnOtherPlayerRequest(Parsed);
                        break;

                    case PacketIds.RecvRequestStats:
                        HandleRecvRequestStats(Parsed);
                        break;

                    case PacketIds.RecvAddStat:
                        HandleRecvAddStat(Parsed);
                        break;

                    case PacketIds.RecvPlayerID:
                        HandleRecvSendPlayerIDRequest(Parsed);
                        break;

                    case PacketIds.RecvStartMoving:
                        HandleRecvStartMoving(Parsed);
                        break;

                    case PacketIds.RecvKeepMoving:
                        HandleRecvKeepMoving(Parsed);
                        break;

                    case PacketIds.RecvStopMoving:
                        HandleRecvStopMoving(Parsed);
                        break;

                    case PacketIds.RecvWalkToPortal:
                        HandleRecvWalkToPortal(Parsed);
                        break;

                    case PacketIds.RecvChatMessage:
                        HandleRecvChatMessage(Parsed);
                        break;

                    case PacketIds.RecvCommand:
                        HandleRecvCommand(Parsed);
                        break;

                    #endregion

                    default:
                        HandleRecvUnknownPacket(Parsed);
                        break; // needs logging system like "packet id received: " then whole buffer like in old one
                }
            }
        }

        #region Channel Handles

        private void HandleRecvChannelLogin(Packet parsed)
        {
            ChannelLogin ch = PacketManager.RecvChannelLogin(parsed);
            if (ClientChannelLoginInfo != null)
                ClientChannelLoginInfo(this, new ClientChannelLoginInfoEventArgs(ch));
        }

        private void HandleRecvChannelChange(Packet parsed)
        {
            ChannelChangeInfo cci = PacketManager.RecvChannelChange(parsed);
            if (ClientChannelChangeInfo != null)
                ClientChannelChangeInfo(this, new ClientChannelChangeEventArgs(cci));
        }

        private void HandleRecvChannelStuff(Packet parsed)
        {
            if (ClientChannelStuffInfo != null)
                ClientChannelStuffInfo(this, new ClientChannelChangeStuffEventArgs());
        }

        private void HandleRecvMapRequest(Packet parsed)
        {
            MapRequestInfo mr = PacketManager.RecvMapRequest(parsed);
            if (ClientMapLoadInfo != null)
                ClientMapLoadInfo(this, new ClientMapLoadEventArgs(mr));
        }

        private void HandleRecvSkillRequest(Packet parsed)
        {
            int skill = PacketManager.RecvSkillListRequest(parsed);
            if (ClientSkillLoadInfo != null)
                ClientSkillLoadInfo(this, new ClientSkillLoadEvenArgs(skill));
        }

        private void HandleRecvInventoryRequest(Packet parsed)
        {
            int inv = PacketManager.RecvInventoryRequest(parsed);
            if (ClientInventoryLoadInfo != null)
                ClientInventoryLoadInfo(this, new ClientInventoryLoadEvenArgs(inv));
        }

        private void HandleRecvMoveItem(Packet parsed)
        {
            MoveItemInfo mii = PacketManager.RecvMoveItem(parsed);
            if (ClientMoveItemInfo != null)
                ClientMoveItemInfo(this, new ClientMoveItemEventArgs(mii));
        }

        private void HandleRecvDropItem(Packet parsed)
        {
            DropItemInfo d = PacketManager.RecvDropItem(parsed);
            if (ClientDropItemInfo != null)
                ClientDropItemInfo(this, new ClientDropItemEventArgs(d));
        }

        private void HandleRecvPickItem(Packet parsed)
        {
            PickItemInfo p = PacketManager.RecvPickItem(parsed);
            if (ClientPickItemInfo != null)
                ClientPickItemInfo(this, new ClientPickItemEventArgs(p));
        }

        private void HandleRecvImbueItem(Packet parsed)
        {
            ImbueItemInfo p = PacketManager.RecvImbueItem(parsed);
            if (ClientImbueItemInfo != null)
                ClientImbueItemInfo(this, new ClientImbueItemEventArgs(p));
        }

        private void HandleRecvAcceptImbueItem(Packet parsed)
        {
            AcceptImbueItem p = PacketManager.RecvAcceptImbueItem(parsed);
            if (ClientAcceptImbueItemInfo != null)
                ClientAcceptImbueItemInfo(this, new ClientAcceptImbueItemEventArgs(p));
        }

        private void HandleRecvNpcTrade(Packet parsed)
        {
            NpcTradeInfo n = PacketManager.RecvNpcTrade(parsed);
            if (ClientNpcTradeInfo != null)
                ClientNpcTradeInfo(this, new ClientNpcTradeEventArgs(n));
        }

        private void HandleRecvBuyItem(Packet parsed)
        {
            BuyItemInfo i = PacketManager.RecvBuyItem(parsed);
            if (ClientBuyItemInfo != null)
                ClientBuyItemInfo(this, new ClientBuyItemEventArgs(i));
        }

        private void HandleRecvStatRequest(Packet parsed)
        {
            StatRequestInfo sr = PacketManager.RecvStatRequest(parsed);
            if (ClientSpawnStatLoadInfo != null)
                ClientSpawnStatLoadInfo(this, new ClientStatLoadEventArgs(sr));
        }

        private void HandleRecvSpawnRequest(Packet parsed)
        {
            SpawnRequestInfo sr = PacketManager.RecvSpawnRequest(parsed);
            if (ClientSpawnLoadInfo != null)
                ClientSpawnLoadInfo(this, new ClientSpawnLoadEventArgs(sr));
        }

        private void HandleRecvSpawnOtherPlayerRequest(Packet parsed)
        {
            SpawnRequestInfo sr = PacketManager.RecvSpawnPlayer(parsed);
            if (ClientSpawnOtherPlayerInfo != null)
                ClientSpawnOtherPlayerInfo(this, new ClientSpawnOtherPlayerEventArgs(sr));
        }

        private void HandleRecvRequestStats(Packet parsed)
        {
            SpawnRequestInfo sr = PacketManager.RecvSpawnOtherPlayerRequest(parsed);
            if (ClientRequestStatsLoadInfo != null)
                ClientRequestStatsLoadInfo(this, new ClientRequestStatsLoadInfo(sr));
        }

        private void HandleRecvAddStat(Packet parsed)
        {
            AddStatInfo addstat = PacketManager.RecvAddStatRequest(parsed);
            if (ClientAddStatInfo != null)
                ClientAddStatInfo(this, new ClientAddStatEventArgs(addstat));
        }


        private void HandleRecvSendPlayerIDRequest(Packet parsed)
        {
            SpawnRequestInfo sr = PacketManager.RecvPlayerIDRequest(parsed);
            if (ClientSendPlayerIDInfo != null)
                ClientSendPlayerIDInfo(this, new ClientSendPlayerIDEventArgs(sr));
        }

        private void HandleRecvStartMoving(Packet parsed)
        {
            MovementInfo mi = PacketManager.RecvStartMoving(parsed);
            if (ClientMovementInfo != null)
                ClientMovementInfo(this, new ClientMovementInfoEventArgs(mi));
        }

        private void HandleRecvKeepMoving(Packet parsed)
        {
            MovementInfo mi = PacketManager.RecvKeepMoving(parsed);
            if (ClientMovementInfo != null)
                ClientMovementInfo(this, new ClientMovementInfoEventArgs(mi));
        }

        private void HandleRecvStopMoving(Packet parsed)
        {
            MovementInfo mi = PacketManager.RecvStopMoving(parsed);
            if (ClientMovementInfo != null)
                ClientMovementInfo(this, new ClientMovementInfoEventArgs(mi));
        }

        private void HandleRecvWalkToPortal(Packet parsed)
        {
            int Map = PacketManager.RecvWalkToPortal(parsed);
            if (ClientWalkToPortalInfo != null)
                ClientWalkToPortalInfo(this, new ClientWalkToPortalEventArgs(Map));
        }

        private void HandleRecvChatMessage(Packet parsed)
        {
            ChatMessageInfo cmi = PacketManager.RecvChatMessage(parsed);
            if (ClientChatMessageInfo != null)
                ClientChatMessageInfo(this, new ClientChatMessageEventArgs(cmi));
        }

        private void HandleRecvCommand(Packet parsed)
        {
            CommandInfo c = PacketManager.RecvCommand(parsed);
            if (ClientCommandInfo != null)
                ClientCommandInfo(this, new ClientCommandInfoEventArgs(c));
        }

        #endregion


        private void HandleRecvUnknownPacket(Packet parsed)
        {
            if(ClientUnknownPacket != null)
                ClientUnknownPacket(this, new UnknownPacketEventArgs(parsed));
        }

        private void HandleRecvGameLogin(Packet parsed)
        {
            GameLogin g = PacketManager.RecvGameLogin(parsed);
            if (ClientLogin != null)
                ClientLogin(this, new ClientLoginEventArgs(g, parsed));
        }

        private void HandleRecvConnectWorld(Packet parsed)
        {
            int a = PacketManager.RecvConnectWorld(parsed);
            if (ClientWorldSelected != null)
                ClientWorldSelected(this, new ClientWorldSelectedEventArgs(a, parsed));
        }

        private void HandleRecvUnitLogin(Packet parsed)
        {
            UnitLogin u = PacketManager.RecvUnitLogin(parsed);
            this.accountId = u.AccountID;
            if (ClientUnitConnectionInfo != null)
                ClientUnitConnectionInfo(this, new ClientUnitConnectionInfoEventArgs(u, parsed));
        }

        private void HandleRecvCreateCharacter(Packet parsed)
        {
            Character c = PacketManager.RecvCreateCharacter(parsed);
            if (ClientCreateCharacterInfo != null)
                ClientCreateCharacterInfo(this, new ClientCharacterCreateInfoEventArgs(c, parsed));
        }

        private void HandleRecvChannelRequest(Packet parsed)
        {
            ChannelRequest c = PacketManager.RecvChannelRequest(parsed);
            if (ClientChannelRequestInfo != null)
                ClientChannelRequestInfo(this, new ClientChannelRequestEventArgs(c, parsed));
        }

        public void Send(byte[] p)
        {
            Packet packet = Packet.Encrypt(new Packet(p), key);
            client.Send(packet.GetBuffer());
        }

        public void SendRaw(byte[] p)
        {
            client.Send(p);
        }

        private void SendKey()
        {
            client.Send(PacketManager.SendKey(this.key));
        }

        public void Dispose()
        {
            client.Close();
            if (ClientDisposed != null)
                ClientDisposed(this, new EventArgs());
        }
    }
}
