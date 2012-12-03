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

        public event EventHandler<ClientOpenWarehouseEventArgs> ClientOpenWarehouseInfo;
        public event EventHandler<ClientMoveWarehouseItemToBagEventArgs> ClientMoveWarehouseItemToBagInfo;
        public event EventHandler<ClientAddItemToWarehouseEventargs> ClientAddItemToWarehouseInfo;
        public event EventHandler<ClientMoveWarehouseItemEventArgs> ClientMoveWarehouseItemInfo;

        public event EventHandler<ClientInventoryLoadEvenArgs> ClientInventoryLoadInfo;
        public event EventHandler<ClientMoveItemEventArgs> ClientMoveItemInfo;
        public event EventHandler<ClientDropItemEventArgs> ClientDropItemInfo;
        public event EventHandler<ClientPickItemEventArgs> ClientPickItemInfo;
        public event EventHandler<ClientImbueItemEventArgs> ClientImbueItemInfo;
        public event EventHandler<ClientAcceptImbueItemEventArgs> ClientAcceptImbueItemInfo;
        public event EventHandler<ClientStackItemInfoEventArgs> ClientStackItemInfo;

        public event EventHandler<ClientNpcTradeEventArgs> ClientNpcTradeInfo;
        public event EventHandler<ClientBuyItemEventArgs> ClientBuyItemInfo;
        public event EventHandler<ClientSellItemEventArgs> ClientSellItemInfo;
        public event EventHandler<ClientUseItemEventArgs> ClientUseItemInfo;

        public event EventHandler<ClientAddItemToShopEventArgs> ClientAddItemToShopInfo;
        public event EventHandler<ClientMoveShopItemEventArgs> ClientMoveShopItemInfo;
        public event EventHandler<ClientChangeShopInfoEventArgs> ClientChangeShopInfo;
        public event EventHandler<ClientMoveShopItemToBagEventArgs> ClientMoveShopItemToBagInfo;
        public event EventHandler<ClientShopStartSellingEventArgs> ClientShopStartSellingInfo;
        public event EventHandler<ClientOpenOtherPlayerShopEventArgs> ClientOpenOtherPlayerShopInfo;
        public event EventHandler<ClientBuyOtherPlayerShopItemEventArgs> ClientBuyOtherPlayerItemInfo;
        public event EventHandler<ClientShopTakeMoneyInfoEventArgs> ClientShopTakeMoneyInfo;

        public event EventHandler<ClientTradePlayerEventArgs> ClientTradePlayerInfo;
        public event EventHandler<ClientTradePlayerCommandsEventArgs> ClientTradePlayerCommandsInfo;
        public event EventHandler<ClientTradeAddMoneyEventArgs> ClientTradeAddMoneyInfo;
        public event EventHandler<ClientTradeAddItemEventArgs> ClientTradeAddItemInfo;

        public event EventHandler<ClientUpdateQuickSlotEventArgs> ClientUpdateQuickSlotInfo;
        public event EventHandler<ClientSelectSkillEventArgs> ClientSelectSkillInfo;

        public event EventHandler<ClientLearnSkillEventArgs> ClientLearnSkillInfo;

        public event EventHandler<ClientStatLoadEventArgs> ClientSpawnStatLoadInfo;
        public event EventHandler<ClientRequestStatsLoadInfo> ClientRequestStatsLoadInfo;
        public event EventHandler<ClientAddStatEventArgs> ClientAddStatInfo;

        public event EventHandler<ClientSpawnLoadEventArgs> ClientSpawnLoadInfo;
        public event EventHandler<ClientSendPetEventArgs> ClientSendPetInfo;
        public event EventHandler<ClientSpawnOtherPlayerEventArgs> ClientSpawnOtherPlayerInfo;

        public event EventHandler<ClientMovementInfoEventArgs> ClientMovementInfo;

        public event EventHandler<ClientAddFriendEventArgs> ClientAddFriendInfo;
        public event EventHandler<ClientRequestFriendListEventArgs> ClientRequestFriendListInfo;

        public event EventHandler<ClientChatMessageEventArgs> ClientChatMessageInfo;
        public event EventHandler<ClientCommandInfoEventArgs> ClientCommandInfo;

        public event EventHandler<ClientMonsterSpawnRequestEventArgs> ClientMonsterSpawnRequestInfo;

        public event EventHandler<ClientAttackEventArgs> ClientAttackInfo;
        public event EventHandler<ClientCastSkillEventArgs> ClientCastSkillInfo;

        public event EventHandler<ClientUseTeleporterEventArgs> ClientUseTeleporterInfo;

        public event EventHandler<ClientCombineItemsEvenArgs> ClientCombineItemsInfo;

        public event EventHandler<ClientPartyRequestEventArgs> ClientPartyRequestInfo;
        public event EventHandler<ClientAddToPartyRequestEventArgs> ClientAddToPartyRequestInfo;
        public event EventHandler<ClientLeavePartyEventArgs> ClientLeavePartyInfo;
        public event EventHandler<ClientChangePartyLeaderEventArgs> ClientChangePartyLeaderInfo;

        public event EventHandler<ClientPetMovementInfoEventArgs> ClientPetMovementInfo;
        public event EventHandler<ClientKillPetInfoEventArgs> ClientKillPetInfo;

        public event EventHandler<ClientCreateGuildInfoEventArgs> ClientCreateGuildInfo;
        public event EventHandler<ClientRequestGuildEventArgs> ClientRequestGuildInfo;
        public event EventHandler<ClientRequestGuildInfoEventArgs> ClientRequestGuildInformation;
        public event EventHandler<ClientRequestGuildMemberStatsEventArgs> ClientRequestGuildMemberStatsInfo;
        public event EventHandler<ClientRequestGuildMemberRankChangeEventArgs> ClientRequestGuildMemberChangeRankInfo;
        public event EventHandler<ClientRequestGuildMemberChangeTitleEventArgs> ClientRequestGuildMemberChangeTitleInfo;
        public event EventHandler<ClientRequestGuildChangeNoticeEventArgs> ClientRequestGuildChangeNoticeInfo;
        public event EventHandler<ClientRequestJoinGuildEventArgs> ClientRequestJoinGuildInfo;
        public event EventHandler<ClientRequestCloseGuildEventArgs> ClientRequestCloseGuildInfo;
        public event EventHandler<ClientRequestLeaveGuildEventArgs> ClientRequestLeaveGuildInfo;
        public event EventHandler<ClientGuildChatEventArgs> ClientGuildChatInfo;

        public event EventHandler<ClientBaitPetInfoEventArgs> ClientBaitPetInfo;
        public event EventHandler<ClientRequestPetStatsInfoEventArgs> ClientRequestPetStatsInfo;
        public event EventHandler<ClientRequestSpawnOtherPetInfoEventArgs> ClientRequestSpawnOtherPetInfo;
        public event EventHandler<ClientResurrectPetInfoEventArgs> ClientResurrectPetInfo;
        public event EventHandler<ClientRenamePetInfoEventArgs> ClientRenamePetInfo;
        public event EventHandler<ClientSendPetToMeInfoEventArgs> ClientSendPetToMeInfo;
        public event EventHandler<ClientTradePetInfoEventArgs> ClientTradePetInfo;
        public event EventHandler<ClientTradePetAmountInfoEventArgs> ClientTradePetAmountInfo;
        public event EventHandler<ClientSealPetInfoEventArgs> ClientSealPetInfo;
        public event EventHandler<ClientUnSealPetInfoEventArgs> ClientUnSealPetInfo;
        public event EventHandler<ClientRequestDeath> ClientRequestDeath;

        public event EventHandler<ClientUseRebirthPillEventArgs> ClientUseRebirthPillInfo;

        #endregion

        public event EventHandler<ClientExceptionEventArgs> ClientExceptionOccoured;
        public event EventHandler<UnknownPacketEventArgs> ClientUnknownPacket;
        public event EventHandler<LogPacketEventArgs> ClientLogPacket;

        public event EventHandler ClientDisposed;

        public void LogPacket(Packet p)
        {
            if (ClientLogPacket != null)
                ClientLogPacket(this, new LogPacketEventArgs(p));
        }

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

                    case PacketIds.RecvRequestOpenWarehouse:
                        HandleRecvOpenWarehouse(Parsed);
                        break;

                    case PacketIds.RecvMoveWarehouseItemToBag:
                        HandleRecvMoveWarehouseItemToBag(Parsed);
                        break;

                    case PacketIds.RecvAddItemToWarehouse:
                        HandleRecvAddItemToWarehouse(Parsed);
                        break;

                    case PacketIds.RecvMoveWarehouseItem:
                        HandleRecvMoveWarehouseItem(Parsed);
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

                    case PacketIds.RecvStackItem:
                        HandleRecvStackItem(Parsed);
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

                    case PacketIds.RecvSellItem:
                        HandleRecvSellItem(Parsed);
                        break;

                    case PacketIds.RecvUseItem:
                        HandleRecvUseItem(Parsed);
                        break;

                    case PacketIds.RecvUseRebirthPill:
                        HandleRecvUseRebirthPill(Parsed);
                        break;

                    case PacketIds.RecvTradePlayer:
                        HandleRecvTradePlayer(Parsed);
                        break;

                    case PacketIds.RecvTradePlayerCommands:
                        HandleRecvTradePlayerCommands(Parsed);
                        break;

                    case PacketIds.RecvTradeAddMoney:
                        HandleRecvTradeAddMoney(Parsed);
                        break;

                    case PacketIds.RecvTradeAddItem:
                        HandleRecvTradeAddItem(Parsed);
                        break;

                    case PacketIds.RecvBuyOtherPlayerShopItem:
                        HandleRecvBuyOtherPlayerShopItem(Parsed);
                        break;

                    case PacketIds.RecvShopTakeMoney:
                        HandleRecvShopTakeMoney(Parsed);
                        break;

                    case PacketIds.RecvOpenOtherPlayerShop:
                        HandleRecvOpenOtherPlayerShop(Parsed);
                        break;

                    case PacketIds.RecvShopStartSelling:
                        HandleRecvShopStartSelling(Parsed);
                        break;

                    case PacketIds.RecvAddItemToShop:
                        HandleRecvAddItemToShop(Parsed);
                        break;

                    case PacketIds.RecvMoveShopItem:
                        HandleRecvMoveShopItem(Parsed);
                        break;

                    case PacketIds.RecvMoveShopItemToBag:
                        HandleRecvMoveShopItemToBag(Parsed);
                        break;

                    case PacketIds.RecvChangeShopInfo:
                        HandleRecvChangeShopInfo(Parsed);
                        break;

                    case PacketIds.RecvLearnSkill:
                        HandleRecvLearnSkill(Parsed);
                        break;

                    case PacketIds.RecvQuickSlotRequest:
                        Send(PacketManager.SendQuickSlots(this.MyCharacter));
                        break;

                    case PacketIds.RecvUpdateQuickSlot:
                        HandleRecvUpdateQuickSlot(Parsed);
                        break;

                    case PacketIds.RecvSelectSkill:
                        HandleRecvSelectSkill(Parsed);
                        break;

                    case PacketIds.RecvRequestGuildList:
                        HandleRecvRequestGuild(Parsed);
                        break;

                    case PacketIds.RecvCreateGuild:
                        HandleRecvCreateGuild(Parsed);
                        break;

                    case PacketIds.RecvRequestGuildInformation:
                        HandleRecvRequestGuildInfo(Parsed);
                        break;

                    case PacketIds.RecvRequestGuildMemberStats:
                        HandleRecvRequestGuildMemberStats(Parsed);
                        break;

                    case PacketIds.RecvRequestChangeGuildMemberRank:
                        HandleRecvRequestChangeGuildMemberRank(Parsed);
                        break;

                    case PacketIds.RecvRequestChangeGuildMemberTitle:
                        HandleRecvRequestChangeGuildMemberTitle(Parsed);
                        break;

                    case PacketIds.RecvRequestChangeGuildNotice:
                        HandleRecvRequestChangeGuildNotice(Parsed);
                        break;

                    case PacketIds.RecvRequestJoinGuild:
                        HandleRecvRequestJoinGuild(Parsed);
                        break;

                    case PacketIds.RecvRequestCloseGuild:
                        HandleRecvRequestCloseGuild(Parsed);
                        break;

                    case PacketIds.RecvRequestLeaveGuild:
                        HandleRecvRequestLeaveGuild(Parsed);
                        break;

                    case PacketIds.RecvGuildChat:
                        HandleRecvGuildChat(Parsed);
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

                    case PacketIds.RecvPlayerPetReques:
                        HandleRecvSendPlayerPetRequest(Parsed);
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

                    case PacketIds.RecvRequestFriendList:
                        HandleRecvRequestFriendList(Parsed);
                        break;

                    case PacketIds.RecvAddFriend:
                        HandleRecvAddFriend(Parsed);
                        break;

                    case PacketIds.RecvChatMessage:
                        HandleRecvChatMessage(Parsed);
                        break;

                    case PacketIds.RecvCommand:
                        HandleRecvCommand(Parsed);
                        break;

                    case PacketIds.RecvMonsterSpawnRequest:
                        HandleRecvMonsterSpawn(Parsed);
                        break;

                    case PacketIds.RecvUseTeleporter:
                        HandleRecvUseTeleporter(Parsed);
                        break;

                    case PacketIds.RecvAttack1:
                        HandleRecvAttack1(Parsed);
                        break;

                    case PacketIds.RecvAttack2:
                        HandleRecvAttack2(Parsed);
                        break;

                    case PacketIds.RecvCast1:
                        HandleRecvCast1(Parsed);
                        break;

                    case PacketIds.RecvCast2:
                        HandleRecvCast2(Parsed);
                        break;

                    case PacketIds.RecvCombineItems:
                        HandleRecvCombineItems(Parsed);
                        break;

                    case PacketIds.RecvPartyRequest:
                        HandleRecvPartyRequest(Parsed);
                        break;

                    case PacketIds.RecvLeaveParty:
                        HandleRecvLeaveParty(Parsed);
                        break;

                    case PacketIds.RecvChangePartyLeader:
                        HandleRecvChangePartyLeader(Parsed);
                        break;

                    case PacketIds.RecvAddToPartyRequest:
                        HandleRecvAddToParty(Parsed);
                        break;

                    case PacketIds.RecvPetStartMoving:
                        HandleRecvPetStartMoving(Parsed);
                        break;

                    case PacketIds.RecvPetKeepMoving:
                        HandleRecvPetKeepMoving(Parsed);
                        break;

                    case PacketIds.RecvPetStopMoving:
                        HandleRecvPetStopMoving(Parsed);
                        break;

                    case PacketIds.RecvKillPet:
                        HandleRecvKillPet(Parsed);
                        break;

                    case PacketIds.RecvBaitPet:
                        HandleRecvBaitPet(Parsed);
                        break;

                    case PacketIds.RecvRequestPetStats:
                        HandleRecvRequestPetStats(Parsed);
                        break;

                    case PacketIds.RecvRequestSpawnOtherPet:
                        HandleRecvSpawnOtherPet(Parsed);
                        break;

                    case PacketIds.RecvResurrectPet:
                        HandleRecvResurrectPet(Parsed);
                        break;

                    case PacketIds.RecvRenamePet:
                        HandleRecvRenamePet(Parsed);
                        break;

                    case PacketIds.RecvSendPetToMe:
                        HandleRecvSendPetToMe(Parsed);
                        break;

                    case PacketIds.RecvTradePet:
                        HandleRecvTradePet(Parsed);
                        break;

                    case PacketIds.RecvTradePetAmount:
                        HandleRecvTradePetAmount(Parsed);
                        break;

                    case PacketIds.RecvSealPet:
                        HandleRecvSealPet(Parsed);
                        break;

                    case PacketIds.RecvUnSealPet:
                        HandleRecvUnSealPet(Parsed);
                        break;

                    case PacketIds.RecvQuestListRequest:
                         List<QuestObject> objects = new List<QuestObject>();
                        objects.Add(new QuestObject(QuestObjectType.Hunt, 0, 5, 0));
                        objects.Add(new QuestObject(QuestObjectType.Hunt, 1, 5, 0));
                        objects.Add(new QuestObject(QuestObjectType.Hunt, 2, 5, 0));
                        objects.Add(new QuestObject(QuestObjectType.Join_Party, 0, 1, 1));
                        objects.Add(new QuestObject(QuestObjectType.Level_Up, 0, 5, 1));
                        objects.Add(new QuestObject(QuestObjectType.Obtain_Money, 0, 1000, 2));

                        List<QuestReward> rewards = new List<QuestReward>();
                        rewards.Add(new QuestReward(0, 23273, 10, QuestRewardType.PlusItem));
                        rewards.Add(new QuestReward(1, 20041, 10, QuestRewardType.Item));
                        rewards.Add(new QuestReward(2, 0, 1000, QuestRewardType.Money));
                        rewards.Add(new QuestReward(2, 0, 1, QuestRewardType.Fame));

                        Quest quest = new Quest
                        {
                            Name = "Fate and Meeting!",
                            ID = 1009,
                            Objectives = objects,
                            Rewards = rewards,
                            CurrentObjective = 2,
                            State = QuestState.Started
                        };

                        Send(PacketManager.SendQuestList(quest));
                        break;

                    case PacketIds.RecvPetRequestItems:
                        // this should send the equips and items in bag but without having anythign on it wont send anything
                        break;


                    case PacketIds.RecvRequestDeath:
                        HandleRecvRequestDeath(Parsed);
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

        private void HandleRecvOpenWarehouse(Packet parsed)
        {
            OpenWarehouseInfo i = PacketManager.RecvOpenWarehouse(parsed);
            if (ClientOpenWarehouseInfo != null)
                ClientOpenWarehouseInfo(this, new ClientOpenWarehouseEventArgs(i));
        }

        private void HandleRecvMoveWarehouseItemToBag(Packet parsed)
        {
            MoveWarehouseItemToBagInfo i = PacketManager.RecvMoveWarehouseItemToBag(parsed);
            if (ClientMoveWarehouseItemToBagInfo != null)
                ClientMoveWarehouseItemToBagInfo(this, new ClientMoveWarehouseItemToBagEventArgs(i));
        }

        private void HandleRecvAddItemToWarehouse(Packet parsed)
        {
            AddItemToWarehouseInfo i = PacketManager.RecvAddItemToWarehouse(parsed);
            if (ClientAddItemToWarehouseInfo != null)
                ClientAddItemToWarehouseInfo(this, new ClientAddItemToWarehouseEventargs(i));
        }

        private void HandleRecvMoveWarehouseItem(Packet parsed)
        {
            MoveWarehouseItemInfo i = PacketManager.RecvMoveWarehouseItem(parsed);
            if (ClientMoveWarehouseItemInfo != null)
                ClientMoveWarehouseItemInfo(this, new ClientMoveWarehouseItemEventArgs(i));
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

        private void HandleRecvStackItem(Packet parsed)
        {
            StackItemInfo i = PacketManager.RecvStackItem(parsed);
            if (ClientStackItemInfo != null)
                ClientStackItemInfo(this, new ClientStackItemInfoEventArgs(i));
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

        private void HandleRecvSellItem(Packet parsed)
        {
            SellItemInfo i = PacketManager.RecvSellItem(parsed);
            if (ClientSellItemInfo != null)
                ClientSellItemInfo(this, new ClientSellItemEventArgs(i));
        }

        private void HandleRecvUseItem(Packet parsed)
        {
            UseItemInfo i = PacketManager.RecvUseItem(parsed);
            if (ClientUseItemInfo != null)
                ClientUseItemInfo(this, new ClientUseItemEventArgs(i));
        }

        private void HandleRecvUseRebirthPill(Packet parsed)
        {
            UseRebirthPillInfo i = PacketManager.RecvUseRebirthPill(parsed);
            if (ClientUseRebirthPillInfo != null)
                ClientUseRebirthPillInfo(this, new ClientUseRebirthPillEventArgs(i));
        }

        private void HandleRecvTradePlayer(Packet parsed)
        {
            TradePlayerInfo i = PacketManager.RecvTradePlayer(parsed);
            if (ClientTradePlayerInfo != null)
                ClientTradePlayerInfo(this, new ClientTradePlayerEventArgs(i));
        }

        private void HandleRecvTradePlayerCommands(Packet parsed)
        {
            TradePlayerCommandsInfo i = PacketManager.RecvTradePlayerCommands(parsed);
            if (ClientTradePlayerCommandsInfo != null)
                ClientTradePlayerCommandsInfo(this, new ClientTradePlayerCommandsEventArgs(i));
        }

        private void HandleRecvTradeAddItem(Packet parsed)
        {
            TradeAddItemInfo i = PacketManager.RecvTradeAdditem(parsed);
            if (ClientTradeAddItemInfo != null)
                ClientTradeAddItemInfo(this, new ClientTradeAddItemEventArgs(i));
        }

        private void HandleRecvTradeAddMoney(Packet parsed)
        {
            TradeAddMoneyInfo i = PacketManager.RecvTradeAddMoney(parsed);
            if (ClientTradeAddMoneyInfo != null)
                ClientTradeAddMoneyInfo(this, new ClientTradeAddMoneyEventArgs(i));
        }

        private void HandleRecvBuyOtherPlayerShopItem(Packet parsed)
        {
            BuyOtherPlayerShopItem i = PacketManager.RecvBuyOtherPlayerShopItem(parsed);
            if (ClientBuyOtherPlayerItemInfo != null)
                ClientBuyOtherPlayerItemInfo(this, new ClientBuyOtherPlayerShopItemEventArgs(i));
        }

        private void HandleRecvShopTakeMoney(Packet parsed)
        {
            ShopTakeMoneyInfo i = PacketManager.RecvShopTakeMoney(parsed);
            if (ClientShopTakeMoneyInfo != null)
                ClientShopTakeMoneyInfo(this, new ClientShopTakeMoneyInfoEventArgs(i));
        }

        private void HandleRecvOpenOtherPlayerShop(Packet parsed)
        {
            OpenOtherPlayerShopInfo i = PacketManager.RecvOpenOtherPlayerShop(parsed);
            if (ClientOpenOtherPlayerShopInfo != null)
                ClientOpenOtherPlayerShopInfo(this, new ClientOpenOtherPlayerShopEventArgs(i));
        }

        private void HandleRecvLearnSkill(Packet parsed)
        {
            LearnSkillInfo i = PacketManager.RecvLearnSkill(parsed);
            if (ClientLearnSkillInfo != null)
                ClientLearnSkillInfo(this, new ClientLearnSkillEventArgs(i));
        }

        private void HandleRecvChangeShopInfo(Packet parsed)
        {
            ChangeShopInfo i = PacketManager.RecvChangeShopInfo(parsed);
            if (ClientChangeShopInfo != null)
                ClientChangeShopInfo(this, new ClientChangeShopInfoEventArgs(i));
        }

        private void HandleRecvAddItemToShop(Packet parsed)
        {
            AddItemToShopInfo i = PacketManager.RecvAddItemToShop(parsed);
            if (ClientAddItemToShopInfo != null)
                ClientAddItemToShopInfo(this, new ClientAddItemToShopEventArgs(i));
        }

        private void HandleRecvMoveShopItem(Packet parsed)
        {
            MoveShopItemInfo i = PacketManager.RecvMoveShopItem(parsed);
            if (ClientMoveShopItemInfo != null)
                ClientMoveShopItemInfo(this, new ClientMoveShopItemEventArgs(i));
        }

        private void HandleRecvMoveShopItemToBag(Packet parsed)
        {
            MoveShopItemToBagInfo i = PacketManager.RecvMoveShopItemToBag(parsed);
            if (ClientMoveShopItemToBagInfo != null)
                ClientMoveShopItemToBagInfo(this, new ClientMoveShopItemToBagEventArgs(i));
        }

        private void HandleRecvShopStartSelling(Packet parsed)
        {
            ShopStartSellingInfo i = PacketManager.RecvShopStartSelling(parsed);
            if (ClientShopStartSellingInfo != null)
                ClientShopStartSellingInfo(this, new ClientShopStartSellingEventArgs(i));
        }

        private void HandleRecvUpdateQuickSlot(Packet parsed)
        {
            UpdateQuickSlotInfo i = PacketManager.RecvUpdateQuickSlot(parsed);
            if (ClientUpdateQuickSlotInfo != null)
                ClientUpdateQuickSlotInfo(this, new ClientUpdateQuickSlotEventArgs(i));
        }

        private void HandleRecvSelectSkill(Packet parsed)
        {
            SelectSkillInfo i = PacketManager.RecvSelectSkill(parsed);
            if (ClientSelectSkillInfo != null)
                ClientSelectSkillInfo(this, new ClientSelectSkillEventArgs(i));
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


        private void HandleRecvSendPlayerPetRequest(Packet parsed)
        {
            PetRequestInfo sr = PacketManager.RecvPlayerPetRequest(parsed);
            if (ClientSendPetInfo != null)
                ClientSendPetInfo(this, new ClientSendPetEventArgs(sr));
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

        private void HandleRecvRequestFriendList(Packet parsed)
        {
            if (ClientRequestFriendListInfo != null)
                ClientRequestFriendListInfo(this, new ClientRequestFriendListEventArgs());
        }

        private void HandleRecvAddFriend(Packet parsed)
        {
            FriendAddInfo i = PacketManager.RecvFriendAdd(parsed);
            if (ClientAddFriendInfo != null)
                ClientAddFriendInfo(this, new ClientAddFriendEventArgs(i));
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

        private void HandleRecvMonsterSpawn(Packet parsed)
        {
            MonsterSpawnRequestInfo i = PacketManager.RecvMonsterSpawnRequest(parsed);
            if (ClientMonsterSpawnRequestInfo != null)
                ClientMonsterSpawnRequestInfo(this, new ClientMonsterSpawnRequestEventArgs(i));
        }

        private void HandleRecvUseTeleporter(Packet parsed)
        {
            UseTeleporterInfo i = PacketManager.RecvUseTeleporter(parsed);
            if (ClientUseTeleporterInfo != null)
                ClientUseTeleporterInfo(this, new ClientUseTeleporterEventArgs(i));
        }

        private void HandleRecvAttack1(Packet parsed)
        {
            AttackInfo i = PacketManager.RecvAttack1(parsed);
            if (ClientAttackInfo != null)
                ClientAttackInfo(this, new ClientAttackEventArgs(i));
        }

        private void HandleRecvAttack2(Packet parsed)
        {
            AttackInfo i = PacketManager.RecvAttack2(parsed);
            if (ClientAttackInfo != null)
                ClientAttackInfo(this, new ClientAttackEventArgs(i));
        }

        private void HandleRecvCast1(Packet parsed)
        {
            CastSkillInfo i = PacketManager.RecvCastSkill1(parsed);
            if (ClientCastSkillInfo != null)
                ClientCastSkillInfo(this, new ClientCastSkillEventArgs(i));
        }

        private void HandleRecvCast2(Packet parsed)
        {
            CastSkillInfo i = PacketManager.RecvCastSkill2(parsed);
            if (ClientCastSkillInfo != null)
                ClientCastSkillInfo(this, new ClientCastSkillEventArgs(i));
        }

        private void HandleRecvCombineItems(Packet parsed)
        {
            CombineItemsInfo i = PacketManager.RecvCombineItems(parsed);
            if (ClientCombineItemsInfo != null)
                ClientCombineItemsInfo(this, new ClientCombineItemsEvenArgs(i));
        }

        private void HandleRecvPartyRequest(Packet parsed)
        {
            PartyRequestInfo i = PacketManager.RecvPartyRequest(parsed);
            if (ClientPartyRequestInfo != null)
                ClientPartyRequestInfo(this, new ClientPartyRequestEventArgs(i));
        }

        private void HandleRecvAddToParty(Packet parsed)
        {
            AddToPartyRequestInfo i = PacketManager.RecvAddToPartyRequest(parsed);
            if (ClientAddToPartyRequestInfo != null)
                ClientAddToPartyRequestInfo(this, new ClientAddToPartyRequestEventArgs(i));
        }

        private void HandleRecvLeaveParty(Packet parsed)
        {
            LeavePartyInfo i = PacketManager.RecvLeaveParty(parsed);
            if (ClientLeavePartyInfo != null)
                ClientLeavePartyInfo(this, new ClientLeavePartyEventArgs(i));
        }

        private void HandleRecvChangePartyLeader(Packet parsed)
        {
            ChangePartyLeaderInfo i = PacketManager.RecvChangePartyLeader(parsed);
            if (ClientChangePartyLeaderInfo != null)
                ClientChangePartyLeaderInfo(this, new ClientChangePartyLeaderEventArgs(i));
        }

        private void HandleRecvPetStartMoving(Packet parsed)
        {
            MovementInfo i = PacketManager.RecvPetStartMoving(parsed);
            if (ClientPetMovementInfo != null)
                ClientPetMovementInfo(this, new ClientPetMovementInfoEventArgs(i));
        }

        private void HandleRecvPetKeepMoving(Packet parsed)
        {
            MovementInfo i = PacketManager.RecvPetKeepMoving(parsed);
            if (ClientPetMovementInfo != null)
                ClientPetMovementInfo(this, new ClientPetMovementInfoEventArgs(i));
        }

        private void HandleRecvPetStopMoving(Packet parsed)
        {
            MovementInfo i = PacketManager.RecvPetStopMoving(parsed);
            if (ClientPetMovementInfo != null)
                ClientPetMovementInfo(this, new ClientPetMovementInfoEventArgs(i));
        }

        private void HandleRecvKillPet(Packet parsed)
        {
            KillPet i = PacketManager.RecvKillPet(parsed);
            if (ClientKillPetInfo != null)
                ClientKillPetInfo(this, new ClientKillPetInfoEventArgs(i));
        }

        private void HandleRecvBaitPet(Packet parsed)
        {
            BaitPetInfo i = PacketManager.RecvBaitPet(parsed);
            if (ClientBaitPetInfo != null)
                ClientBaitPetInfo(this, new ClientBaitPetInfoEventArgs(i));
        }

        private void HandleRecvRequestPetStats(Packet parsed)
        {
            RequestPetStats i = PacketManager.RecvPetStatRequest(parsed);

            if (ClientRequestPetStatsInfo != null)
                ClientRequestPetStatsInfo(this, new ClientRequestPetStatsInfoEventArgs(i));
        }

        private void HandleRecvSpawnOtherPet(Packet parsed)
        {
            RequestSpawnOtherPet i = PacketManager.RecvSpawnOtherPet(parsed);
            if (ClientRequestSpawnOtherPetInfo != null)
                ClientRequestSpawnOtherPetInfo(this, new ClientRequestSpawnOtherPetInfoEventArgs(i));
        }

        private void HandleRecvResurrectPet(Packet parsed)
        {
            ResurrectPetInfo i = PacketManager.RecvResurrectPet(parsed);
            if (ClientResurrectPetInfo != null)
                ClientResurrectPetInfo(this, new ClientResurrectPetInfoEventArgs(i));
        }

        private void HandleRecvRenamePet(Packet parsed)
        {
            RenamePetInfo i = PacketManager.RecvRenamePet(parsed);
            if (ClientRenamePetInfo != null)
                ClientRenamePetInfo(this, new ClientRenamePetInfoEventArgs(i));
        }

        private void HandleRecvSendPetToMe(Packet parsed)
        {
            SendPetToMeInfo i = PacketManager.RecvSendPetToMe(parsed);
            if (ClientSendPetToMeInfo != null)
                ClientSendPetToMeInfo(this, new ClientSendPetToMeInfoEventArgs(i));
        }

        private void HandleRecvTradePet(Packet parsed)
        {
            TradePetInfo i = PacketManager.RecvTradePet(parsed);
            if (ClientTradePetInfo != null)
                ClientTradePetInfo(this, new ClientTradePetInfoEventArgs(i));
        }

        private void HandleRecvTradePetAmount(Packet parsed)
        {
            TradePetAmountInfo i = PacketManager.RecvTradePetAmount(parsed);
            if (ClientTradePetAmountInfo != null)
                ClientTradePetAmountInfo(this, new ClientTradePetAmountInfoEventArgs(i));
        }

        private void HandleRecvSealPet(Packet parsed)
        {
            SealPetInfo i = PacketManager.RecvSealPet(parsed);
            if (ClientSealPetInfo != null)
                ClientSealPetInfo(this, new ClientSealPetInfoEventArgs(i));
        }

        private void HandleRecvUnSealPet(Packet parsed)
        {
            UnSealPetInfo i = PacketManager.RecvUnSealPet(parsed);
            if (ClientUnSealPetInfo != null)
                ClientUnSealPetInfo(this, new ClientUnSealPetInfoEventArgs(i));
        }

        private void HandleRecvRequestDeath(Packet parsed)
        {
            if (ClientRequestDeath != null)
                ClientRequestDeath(this, null);

        }

        private void HandleRecvCreateGuild(Packet parsed)
        {
            CreateGuildInfo i = PacketManager.RecvCreateGuild(parsed);
            if (ClientCreateGuildInfo != null)
                ClientCreateGuildInfo(this, new ClientCreateGuildInfoEventArgs(i));
        }

        private void HandleRecvRequestGuild(Packet parsed)
        {
            if (ClientRequestGuildInfo != null)
                ClientRequestGuildInfo(this, new ClientRequestGuildEventArgs());
        }

        private void HandleRecvRequestGuildInfo(Packet parsed)
        {
            RequestGuildInfo i = PacketManager.RecvRequestGuildInfo(parsed);
            if (ClientRequestGuildInformation != null)
                ClientRequestGuildInformation(this, new ClientRequestGuildInfoEventArgs(i));
        }

        private void HandleRecvRequestGuildMemberStats(Packet parsed)
        {
            RequestGuildMemberStatsInfo i = PacketManager.RecvRequestGuildMemberStats(parsed);
            if (ClientRequestGuildMemberStatsInfo != null)
                ClientRequestGuildMemberStatsInfo(this, new ClientRequestGuildMemberStatsEventArgs(i));
        }

        private void HandleRecvRequestChangeGuildMemberRank(Packet parsed)
        {
            RequestGuildMemberChangeRankInfo i = PacketManager.RecvRequestChangeGuildMemberRank(parsed);
            if (ClientRequestGuildMemberChangeRankInfo != null)
                ClientRequestGuildMemberChangeRankInfo(this, new ClientRequestGuildMemberRankChangeEventArgs(i));
        }

        private void HandleRecvRequestChangeGuildMemberTitle(Packet parsed)
        {
            RequestGuildMemberChangeTitleInfo i = PacketManager.RecvRequestChangeGuildMemberTitle(parsed);
            if (ClientRequestGuildMemberChangeTitleInfo != null)
                ClientRequestGuildMemberChangeTitleInfo(this, new ClientRequestGuildMemberChangeTitleEventArgs(i));
        }

        private void HandleRecvRequestChangeGuildNotice(Packet parsed)
        {
            RequestGuildChangeNoticeInfo i = PacketManager.RecvRequestChangeGuildNotice(parsed);
            if (ClientRequestGuildChangeNoticeInfo != null)
                ClientRequestGuildChangeNoticeInfo(this, new ClientRequestGuildChangeNoticeEventArgs(i));
        }

        private void HandleRecvRequestJoinGuild(Packet parsed)
        {
            RequestJoinGuildInfo i = PacketManager.RecvRequestJoinGuild(parsed);
            if (ClientRequestJoinGuildInfo != null)
                ClientRequestJoinGuildInfo(this, new ClientRequestJoinGuildEventArgs(i));
        }

        private void HandleRecvRequestCloseGuild(Packet parsed)
        {
            if (ClientRequestCloseGuildInfo != null)
                ClientRequestCloseGuildInfo(this, null);
        }

        private void HandleRecvRequestLeaveGuild(Packet parsed)
        {
            RequestLeaveGuildInfo i = PacketManager.RecvRequestLeaveGuild(parsed);
            if (ClientRequestLeaveGuildInfo != null)
                ClientRequestLeaveGuildInfo(this, new ClientRequestLeaveGuildEventArgs(i));
        }

        private void HandleRecvGuildChat(Packet parsed)
        {
            GuildChatInfo i = PacketManager.RecvGuildChat(parsed);
            if (ClientGuildChatInfo != null)
                ClientGuildChatInfo(this, new ClientGuildChatEventArgs(i));
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
            if (client.Connected)
            {
                Packet packet = Packet.Encrypt(new Packet(p), key);
                client.Send(packet.GetBuffer());
            }
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
            if (ClientDisposed != null)
                ClientDisposed(this, new EventArgs());
            client.Close();
        }
    }
}
