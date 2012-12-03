using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XiahBLL;
using Entities;
using ServerEngine.PacketEngine;
using System.Configuration;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ServerEngine
{
    public class ChannelServer : ServerBase
    {
        private GameEngine gameEngine;
        private MapEngine mapEngine;
        private AccountManager accountManager;
        private CharacterManager characterManager;
        private ItemDataManager itemDataManager;

        #region Base Events
        public event EventHandler<ClientUnitConnectionInfoEventArgs> ClientUnitConnection;
        public event EventHandler<UnknownPacketEventArgs> ClientUnknownPacket;
        #endregion

        public ChannelServer(string ipAddress, int port)
            : base(ipAddress, port)
        {
            gameEngine = new GameEngine();
            mapEngine = new MapEngine(ConfigurationManager.ConnectionStrings["XiahDb"].ConnectionString, ConfigurationManager.ConnectionStrings["XiahDb"].ProviderName);
            accountManager = new AccountManager(ConfigurationManager.ConnectionStrings["XiahDb"].ConnectionString, ConfigurationManager.ConnectionStrings["XiahDb"].ProviderName);
            characterManager = new CharacterManager(ConfigurationManager.ConnectionStrings["XiahDb"].ConnectionString, ConfigurationManager.ConnectionStrings["XiahDb"].ProviderName);
            itemDataManager = new ItemDataManager(ConfigurationManager.ConnectionStrings["XiahDb"].ConnectionString, ConfigurationManager.ConnectionStrings["XiahDb"].ProviderName);
        }

        #region Base Stuff

        protected override void OnClientConnected(ClientConnectionEventArgs e)
        {
            base.OnClientConnected(e);

            Client c = e.Client;
            c.ClientUnknownPacket += new EventHandler<UnknownPacketEventArgs>(c_ClientUnknownPacket);
            c.ClientUnitConnectionInfo += new EventHandler<ClientUnitConnectionInfoEventArgs>(c_ClientUnitConnectionInfo);

            c.ClientChannelLoginInfo += new EventHandler<ClientChannelLoginInfoEventArgs>(c_ClientChannelLoginInfo);
            c.ClientChannelChangeInfo += new EventHandler<ClientChannelChangeEventArgs>(c_ClientChannelChangeInfo);
            c.ClientChannelStuffInfo += new EventHandler<ClientChannelChangeStuffEventArgs>(c_ClientChannelStuffInfo);

            c.ClientWalkToPortalInfo += new EventHandler<ClientWalkToPortalEventArgs>(c_ClientWalkToPortalInfo);
            c.ClientMapLoadInfo += new EventHandler<ClientMapLoadEventArgs>(c_ClientMapLoadInfo);
            c.ClientSkillLoadInfo += new EventHandler<ClientSkillLoadEvenArgs>(c_ClientSkillLoadInfo);

            c.ClientInventoryLoadInfo += new EventHandler<ClientInventoryLoadEvenArgs>(c_ClientInventoryLoadInfo);
            c.ClientMoveItemInfo += new EventHandler<ClientMoveItemEventArgs>(c_ClientMoveItemInfo);
            c.ClientDropItemInfo += new EventHandler<ClientDropItemEventArgs>(c_ClientDropItemInfo);
            c.ClientPickItemInfo += new EventHandler<ClientPickItemEventArgs>(c_ClientPickItemInfo);
            c.ClientImbueItemInfo += new EventHandler<ClientImbueItemEventArgs>(c_ClientImbueItemInfo);
            c.ClientAcceptImbueItemInfo += new EventHandler<ClientAcceptImbueItemEventArgs>(c_ClientAcceptImbueItemInfo);

            c.ClientNpcTradeInfo += new EventHandler<ClientNpcTradeEventArgs>(c_ClientNpcTradeInfo);
            c.ClientBuyItemInfo += new EventHandler<ClientBuyItemEventArgs>(c_ClientBuyItemInfo);

            c.ClientSpawnStatLoadInfo += new EventHandler<ClientStatLoadEventArgs>(c_ClientSpawnStatLoadInfo);
            c.ClientSpawnLoadInfo += new EventHandler<ClientSpawnLoadEventArgs>(c_ClientSpawnLoadInfo);
            c.ClientRequestStatsLoadInfo += new EventHandler<ClientRequestStatsLoadInfo>(c_ClientRequestStatsLoadInfo);
            c.ClientAddStatInfo += new EventHandler<ClientAddStatEventArgs>(c_ClientAddStatInfo);

            c.ClientSendPlayerIDInfo += new EventHandler<ClientSendPlayerIDEventArgs>(c_ClientSendPlayerIDInfo);
            c.ClientSpawnOtherPlayerInfo += new EventHandler<ClientSpawnOtherPlayerEventArgs>(c_ClientSpawnOtherPlayerInfo);

            c.ClientMovementInfo += new EventHandler<ClientMovementInfoEventArgs>(c_ClientMovementInfo);

            c.ClientChatMessageInfo += new EventHandler<ClientChatMessageEventArgs>(c_ClientChatMessageInfo);
            c.ClientCommandInfo += new EventHandler<ClientCommandInfoEventArgs>(c_ClientCommandInfo);
        }

        private Client[] GetNearByClients(Client c)
        {
            var nearbyClients = clients.Where(x => (mapEngine.GetDistance(x.MyCharacter.X, x.MyCharacter.Y, c.MyCharacter.X, c.MyCharacter.Y) < 150));

            return nearbyClients.ToArray();
        }

        private Client GetClientByName(string name)
        {
            foreach (Client c in clients)
            {
                if (c.MyCharacter.Name == name)
                    return c;
            }
            return null;
        }

        private Client GetClientByCharacterID(int characterId)
        {
            foreach (Client c in clients)
            {
                if (c.MyCharacter.CharacterId == characterId)
                    return c;
            }
            return null;
        }

        private Client[] GetNearByClients(Client c, bool GetMe)
        {
            var nearbyClients = clients.Where(x => (mapEngine.GetDistance(x.MyCharacter.X, x.MyCharacter.Y, c.MyCharacter.X, c.MyCharacter.Y) < 150) && (x.MyCharacter.CharacterId != c.MyCharacter.CharacterId));

            return nearbyClients.ToArray();
        }

        private Portal GetNearbyPortal(Client c)
        {
            var portal = c.Map.Portals.Where(x => mapEngine.GetDistance(c.MyCharacter.X, c.MyCharacter.Y, x.FromX, x.FromY) <= x.Width).First();
            return portal;
        }

        void c_ClientUnknownPacket(object sender, UnknownPacketEventArgs e)
        {
            if (ClientUnknownPacket != null)
                ClientUnknownPacket(sender, e);
        }

        #endregion

        #region Packet Stuff

        void c_ClientUnitConnectionInfo(object sender, ClientUnitConnectionInfoEventArgs e)
        {
            if (ClientUnitConnection != null)
                ClientUnitConnection(sender, e);

            Client c = (Client)sender;
            UnitLogin u = e.UnitLoginDetails;

            int accountId = u.AccountID;

            if (accountManager.ConnectVerification(accountId))
            {
                byte[] bufferUniLoginState = PacketManager.SendUnitLoginState(LoginState.Success, u.CharacterID);
                c.Send(bufferUniLoginState);
            }
            else
            {
                Client[] otherPlayer = clients.Where(x => (x.AccountID == u.AccountID) && (x != c)).ToArray();
                for (int i = 0; i < otherPlayer.Length; i++)
                {
                    otherPlayer[i].Send(PacketManager.SendUnitLoginState(LoginState.OtherConnecting, 0));
                    otherPlayer[i].Dispose();
                }

                byte[] bufferUniLoginState = PacketManager.SendUnitLoginState(LoginState.Success, u.CharacterID);
                c.Send(bufferUniLoginState);
            }
        }

        void c_ClientChannelLoginInfo(object sender, ClientChannelLoginInfoEventArgs e)
        {
            Client c = (Client)sender;
            ChannelLogin ch = e.ChannelLogin;

            Character cha = characterManager.GetCharacterByCharacterId(e.ChannelLogin.CharacterID);
            c.MyCharacter = cha;

            byte[] bufferSendMapID = PacketManager.SendMapID(cha.MapId);
            c.Send(bufferSendMapID);

            byte[] bufferSendMapTime = PacketManager.SendMapTime(7, 5, 25, 0);
            c.Send(bufferSendMapTime);

            byte[] bufferSendMapLoad2 = PacketManager.SendMapLoad2();
            c.Send(bufferSendMapLoad2);

            byte[] bufferSendMapLoad3 = PacketManager.SendMapLoad3();
            c.Send(bufferSendMapLoad3);
        }

        void c_ClientChannelChangeInfo(object sender, ClientChannelChangeEventArgs e)
        {
            Client c = (Client)sender;
            ChannelChangeInfo cci = e.Info;

            Client[] nearbyClients = GetNearByClients(c, false);

            switch (cci.Something2)
            {
                case 1:
                    byte[] bufferSendChannelChange = PacketManager.SendChannelChange(cci.Something, cci.Something2);
                    c.Send(bufferSendChannelChange);

                    byte[] bufferRemoveCharacter = PacketManager.SendRemoveCharacter(c.MyCharacter);
                    SendSpecific(nearbyClients, bufferRemoveCharacter);
                    break;

                case 0:
                    accountManager.ExitGame(c.AccountID);

                    bufferRemoveCharacter = PacketManager.SendRemoveCharacter(c.MyCharacter);
                    SendSpecific(nearbyClients, bufferRemoveCharacter);
                    break;
            }
        }

        void c_ClientChannelStuffInfo(object sender, ClientChannelChangeStuffEventArgs e)
        {
            Client c = (Client)sender;

            Client[] nearbyClients = GetNearByClients(c, false);
            Character[] charlist = characterManager.GetAllCharactersByAccountId(c.AccountID);

            byte[] bufferSendChannelChange2 = PacketManager.SendChannelChange(2, 0);
            c.Send(bufferSendChannelChange2);

            byte[] bufferSendCharList = PacketManager.SendCharacterList(charlist);
            c.Send(bufferSendCharList);

            byte[] bufferRemoveCharacter = PacketManager.SendRemoveCharacter(c.MyCharacter);
            SendSpecific(nearbyClients, bufferRemoveCharacter);
        }

        void c_ClientMapLoadInfo(object sender, ClientMapLoadEventArgs e)
        {
            Client c = (Client)sender;
            Map curMap = mapEngine.GetMapById(e.Map.MapID);
            c.Map = curMap;

            //c.MyCharacter.X = (short)curMap.SpawnX;
            //c.MyCharacter.Y = (short)curMap.SpawnY;

            byte[] bufferSendMap = PacketManager.SendMap(curMap);
            c.Send(bufferSendMap);

            byte[] bufferSendMap2 = PacketManager.SendMap2();
            c.Send(bufferSendMap2);
        }

        void c_ClientSkillLoadInfo(object sender, ClientSkillLoadEvenArgs e)
        {
            Client c = (Client)sender;
            int skill = e.SkillList;
            switch (skill)
            {
                case 0:
                    byte[] bufferSoftSkillList = PacketManager.SendShoftSkillList();
                    c.Send(bufferSoftSkillList);
                    break;

                case 1:
                    byte[] bufferHardSkillList = PacketManager.SendHardSkillList();
                    c.Send(bufferHardSkillList);
                    break;
            }
        }

        void c_ClientInventoryLoadInfo(object sender, ClientInventoryLoadEvenArgs e)
        {
            Client c = (Client)sender;
            int inv = e.Inventory;


            switch (inv)
            {
                case 0:

                    byte[] bufferSendEquips = PacketManager.SendBagItems(c.MyCharacter.GetAllEquips().Cast<BaseItem>().ToList(), inv);
                    c.Send(bufferSendEquips);
                    break;

                case 1:
                    //byte[] bufferSendBag1 = PacketManager.SendTempBag1();
                    //c.Send(bufferSendBag1);
                    var bagOne = itemDataManager.GetAllItemsInBag(1, c.MyCharacter.CharacterId);
                    c.MyCharacter.Bags.Add(new Bag(bagOne));
                    if (c.MyCharacter.Bags[0].ItemCount > 0)
                    {
                        byte[] bufferSendBag1 = PacketManager.SendBagItems(c.MyCharacter.Bags[0].Items.Cast<BaseItem>().ToList(), inv);
                        c.Send(bufferSendBag1);
                    }
                    break;

                case 2:
                    var bagTwo = itemDataManager.GetAllItemsInBag(2, c.MyCharacter.CharacterId);
                    c.MyCharacter.Bags.Add(new Bag(bagTwo));
                    if (c.MyCharacter.Bags[1].ItemCount > 0)
                    {
                        byte[] bufferSendBag2 = PacketManager.SendBagItems(c.MyCharacter.Bags[1].Items.Cast<BaseItem>().ToList(), inv);
                        c.Send(bufferSendBag2);
                    }
                    break;

                case 3:
                    break;
            }
        }

        void c_ClientMoveItemInfo(object sender, ClientMoveItemEventArgs e)
        {
            Client c = (Client)sender;
            MoveItemInfo m = e.Info;
            Client[] nearbyClients = GetNearByClients(c);
            BaseItem item = null;

            if (m.FromBag > 0)
            {
                item = c.MyCharacter.Bags[m.FromBag - 1].Items.First(x => x.ItemID == m.ItemID);
            }
            else
            {
                switch (m.FromSlot)
                {
                    case (byte)Slot.Weapon: // Hand
                        item = c.MyCharacter.Hand;
                        break;

                    case (byte)Slot.Hat: // Hat
                        item = c.MyCharacter.Head;
                        break;

                    case (byte)Slot.Armor: // Armor
                        item = c.MyCharacter.Body;
                        break;

                    case (byte)Slot.Shoes: // Shoes
                        item = c.MyCharacter.Feet;
                        break;

                    case (byte)Slot.CrystalMirror: // crystalmirror
                        item = c.MyCharacter.Charm;
                        break;

                    case (byte)Slot.Ring:
                        item = c.MyCharacter.Ring;
                        break;

                    case (byte)Slot.Necklace:
                        item = c.MyCharacter.Necklace;
                        break;

                    case (byte)Slot.Cape:
                        item = c.MyCharacter.Cape;
                        break;

                    case (byte)Slot.Mirror:
                        item = c.MyCharacter.Mirror;
                        break;
                    default:
                        break;
                }
            }

            byte error = 0;
            // ADD MOVE ITEM FUNC TO THAT BAG CLASS BLABLA

            

            if (item.Slot != 255)
            {
                itemDataManager.UpdateItem(item);
                byte[] SendMoveItem = PacketManager.SendMoveItem(m, error);
                c.Send(SendMoveItem);
            }
            else
            {
                byte[] SendMoveItem = PacketManager.SendMoveItem(m, error);
                c.Send(SendMoveItem);
            }
        }

        //private static bool HandleItemMove(Client c, MoveItemInfo m, BaseItem item)
        //{

        //    if (c.MyCharacter.Bags[m.ToBag - 1].MoveItem(item, m.ToBag, m.ToSlot, out BaseItem))
        //    {
        //        item.Slot = (byte)(m.ToSlot);
        //        item.Bag = (byte)(m.ToBag);
        //        if (m.FromBag != m.ToBag && m.FromBag > 0)
        //        {
        //            c.MyCharacter.Bags[m.FromBag - 1].RemoveItem(item);
        //        }


        //        return true;
        //    }

        //    return false;
        //}


        void c_ClientDropItemInfo(object sender, ClientDropItemEventArgs e)
        {
            Client c = (Client)sender;
            DropItemInfo d = e.Info;

            //Equipment item = channelManager.GetItemByItemID(d.ItemID);

            BaseItem item = c.MyCharacter.Bags[d.FromBag - 1].Items.Where(x => x.ItemID == d.ItemID).First();

            MapItem m = mapEngine.PlayerDropItem(item, d.ToX, d.ToY, d.ToZ, c.Map.MapID);

            c.MyCharacter.Bags[d.FromBag - 1].RemoveItem(item);


            Client[] nearbyClients = GetNearByClients(c);

            byte[] SendRemoveItem = PacketManager.SendRemoveItem(d.FromBag, d.FromSlot);
            c.Send(SendRemoveItem);

            byte[] SendItemOnGround = PacketManager.SendItemOnGround(m);
            SendSpecific(nearbyClients, SendItemOnGround);
        }

        void c_ClientPickItemInfo(object sender, ClientPickItemEventArgs e)
        {
            Client c = (Client)sender;
            PickItemInfo p = e.Info;

            //MapItem m = mapEngine.GetMapItemById(p.MapItemID);
            // NEED BETTER LOAD FOR LIKE ALL ITEM DATA, NOT JUST MAPITEM STUFF KPLXTHX

            //Equipment item = (Equipment)channelManager.GetItemByItemID(p.ItemID);
            BaseItem item = null;
            Client[] nearbyClients = GetNearByClients(c);

            try
            {
                item = mapEngine.PickupItem(p.MapItemID, c.MyCharacter);
            }
            catch (BagIsFullException)
            {
                byte[] bufferBagIsFull = PacketManager.SendBagIsFull();
                c.Send(bufferBagIsFull);
                //send bag is full
            }

            //.MyCharacter.Bags[0].AddItem(item);
            if (item == null)
            {
                //send item not exist
                return;
            }

            byte[] SendPickUp = PacketManager.SendAddItem(item);
            c.Send(SendPickUp);

            byte[] SendPickUpText = PacketManager.SendPickUpText(item.ReferenceID, item.Amount);
            c.Send(SendPickUpText);

            byte[] SendRemoveMapItem = PacketManager.SendRemoveMapItem(p.MapItemID);
            SendSpecific(nearbyClients, SendRemoveMapItem);
        }


        void c_ClientImbueItemInfo(object sender, ClientImbueItemEventArgs e)
        {
            Client c = (Client)sender;
            ImbueItemInfo i = e.Info;
            BaseItem item = null;
            try
            {
                item = c.MyCharacter.Bags[i.Bag - 1].Items.Single(x => (x.ItemID == i.ItemID && x.Slot == i.Slot) && (x.bType <= 7));

                byte[] SendAcceptImbueItem = PacketManager.SendAcceptImbueItem(ImbueState.Success);
                if (item != null)
                    c.Send(SendAcceptImbueItem);
            }
            catch
            {
                byte[] SendAcceptImbueItem = PacketManager.SendAcceptImbueItem(ImbueState.ThisItemCannotBeImbued);
                c.Send(SendAcceptImbueItem);
            }
        }


        void c_ClientAcceptImbueItemInfo(object sender, ClientAcceptImbueItemEventArgs e)
        {
            Client c = (Client)sender;
            AcceptImbueItem i = e.Info;
            Equipment Item = null;
            BaseItem item = null;
            BaseItem imbueItem1 = null;
            ImbueError error;
            ImbueStat stat = ImbueStat.CriticalHit;

            try
            {
                item = c.MyCharacter.Bags[i.ToImbueItemBag - 1].Items.Single(x => (x.ItemID == i.ToImbueItemID && x.Slot == i.ToImbueItemSlot));
                if(item is Equipment)
                   Item = item as Equipment;
                
                imbueItem1 = c.MyCharacter.Bags[i.ImbueItem1Bag - 1].Items.Single(x => (x.ItemID == i.ImbueItem1ID));
              
                double SuccessChance = 33.3f; // flat 1/3 chance as there chance to success, fail, and drop so :/


                if (Item.Plus == 0 && imbueItem1 is Black)
                    SuccessChance = 100;

                if (imbueItem1 is ImbueItem)
                {
                    ImbueItem imbueitem = imbueItem1 as ImbueItem;
                    SuccessChance += (double)imbueitem.ImbueChance;
                }

                // ADD UBER LEET REMOVE FROM BAG & SAVE DATABASE YOU LOST THE ITEM PLXO

                int value = 0;

                if (itemDataManager.PercentSuccess(SuccessChance))
                {
                    

                    value = 1; // just - value to decrease xd
                    Item.Damage += (short)value;
                    Item.Plus++;
                    error = ImbueError.Success;
                    stat = ImbueStat.Damage;

                }
                else
                {
                    if (Item.Plus >= 4 && itemDataManager.PercentSuccess(30))
                    {
                        // I DROP YO STATS 
                    }
                    error = ImbueError.FailedToRemake;
                    stat = ImbueStat.None;
                    value = 0;
                }

                c.Send(PacketManager.SendImbueMessage(error, stat, value, Item));

                c.Send(PacketManager.SendRemoveItem(i.ImbueItem1Bag, i.ImbueItem1Slot));

                Item.ImbueTries++;
                c.Send(PacketManager.SendImbuedItem((BaseItem)Item));
                itemDataManager.UpdateItem((BaseItem)Item);
            }
            catch
            {
            }
        }


        void c_ClientNpcTradeInfo(object sender, ClientNpcTradeEventArgs e)
        {
            Client c = (Client)sender;
            NpcTradeInfo n = e.Info;
            Npc npc = mapEngine.GetNpcByID(n.NpcID, n.MapID);

            byte[] SendNpcItemList = PacketManager.SendNpcItemList(npc, n.Bag);
            c.Send(SendNpcItemList);
        }

        /// <summary>
        /// SELL ITEM PRICE FOR PLAIN ITEMS IS ITEM.PRICE / 2.5
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void c_ClientBuyItemInfo(object sender, ClientBuyItemEventArgs e)
        {
            Client c = (Client)sender;
            BuyItemInfo i = e.Info;
            BaseItem buyitem = null;
            BaseItem npcitem = null;
            Npc npc = null;

            try
            {
                npc = mapEngine.GetNpcByID(i.NpcID, c.MyCharacter.MapId);

                npcitem = npc.Bags[i.Bag].Items.Single(x => (x.ReferenceID == i.ReferenceID));

                if (c.MyCharacter.Money >= npcitem.Price)
                {
                    c.MyCharacter.Money -= npcitem.Price;
                    c.Send(PacketManager.SendMoneyLeft(c.MyCharacter.Money));
                    c.Send(PacketManager.SendBuyItemState(BuyItemState.Success));

                    BagSlot bagSlot = new BagSlot();

                    if (!c.MyCharacter.FindFreeSlotInBags(npcitem, bagSlot))
                    {
                        //Throw exception here
                    }

                    MemoryStream stream = new MemoryStream();

                    BinaryFormatter formatter = new BinaryFormatter();

                    formatter.Serialize(stream, npcitem);

                    stream.Position = 0;

                    buyitem = (BaseItem)formatter.Deserialize(stream);

                    stream.Close();

                    buyitem.Bag = bagSlot.Bag;
                    buyitem.Slot = bagSlot.Slot;
                    buyitem.OwnerID = c.MyCharacter.CharacterId;
                    buyitem.Amount = i.Amount;

                    buyitem.ItemID = itemDataManager.InsertItem(buyitem);

                    c.MyCharacter.Bags[buyitem.Bag - 1].AddItem(buyitem);

                    c.Send(PacketManager.SendAddItem(buyitem));
                }
                else
                {
                    c.Send(PacketManager.SendBuyItemState(BuyItemState.NoMoney));
                }

            }
            catch
            {
            }
        }


        void c_ClientSpawnStatLoadInfo(object sender, ClientStatLoadEventArgs e)
        {
            Client c = (Client)sender;
            StatRequestInfo sr = e.Info;

            //Character ch = channelManager.GetCharacterByCharacterId(sr.CharacterID);
            byte[] bufferSendStats = PacketManager.SendSpawnStats(c.MyCharacter);
            c.Send(bufferSendStats);
        }

        void c_ClientRequestStatsLoadInfo(object sender, ClientRequestStatsLoadInfo e)
        {
            Client c = (Client)sender;
            SpawnRequestInfo sr = e.Info;

            Character ch = characterManager.GetCharacterByCharacterId(sr.CharacterID);

            byte[] bufferSendStats = PacketManager.SendStats(c.MyCharacter);
            c.Send(bufferSendStats);

            byte[] bufferSendOtherPlayer2 = PacketManager.SendSpawnPlayerSomething2();
            c.Send(bufferSendOtherPlayer2);
        }

        void c_ClientAddStatInfo(object sender, ClientAddStatEventArgs e)
        {
            Client c = (Client)sender;
            AddStatInfo statinfo = e.Info;

            if (c.MyCharacter.StatPoint >= statinfo.Amount)
            {
                int pointsleft = -1;
                int statafter = -1;

                switch (statinfo.Stat)
                {
                    case 1: // dex
                        c.MyCharacter.StatPoint -= 1;
                        c.MyCharacter.Dexterity += 1;
                        statafter = c.MyCharacter.Dexterity;
                        break;

                    case 2: // str
                        c.MyCharacter.StatPoint -= 1;
                        c.MyCharacter.Strength += 1;
                        statafter = c.MyCharacter.Strength;
                        break;

                    case 3: // sta

                        c.MyCharacter.StatPoint -= 1;
                        c.MyCharacter.Stamina += 1;
                        statafter = c.MyCharacter.Stamina;
                        break;

                    case 4: // energy
                        c.MyCharacter.StatPoint -= 1;
                        c.MyCharacter.Energy += 1;
                        statafter = c.MyCharacter.Energy;
                        break;
                }
                pointsleft = c.MyCharacter.StatPoint;

                byte[] bufferSendAddStat = PacketManager.SendAddStat(0, statinfo, pointsleft, statafter);
                c.Send(bufferSendAddStat);
            }
            else
            {
                byte[] bufferSendStatError = PacketManager.SendAddStat(3, statinfo, 0, 0);
                c.Send(bufferSendStatError);
            }

            byte[] bufferSendStats = PacketManager.SendStats(c.MyCharacter);
            c.Send(bufferSendStats);
        }

        void c_ClientSpawnLoadInfo(object sender, ClientSpawnLoadEventArgs e)
        {
            Client c = (Client)sender;
            SpawnRequestInfo sr = e.Info;

            Client[] nearbyClients = GetNearByClients(c);
            MapItem[] nearbyMapItems = mapEngine.GetNearbyMapItems(c.MyCharacter.X, c.MyCharacter.Y);

            byte[] bufferSendShowMap = PacketManager.SendShowMap();
            c.Send(bufferSendShowMap);

            byte[] bufferSendSpawn10 = PacketManager.SendSpawn10(c.MyCharacter);
            // c.Send(bufferSendSpawn10);
            SendSpecific(nearbyClients, bufferSendSpawn10);

            for (int i = 0; i < nearbyClients.Length; i++)
            {
                if (nearbyClients[i].MyCharacter.CharacterId != c.MyCharacter.CharacterId)
                {
                    byte[] bufferSendMeToThem = PacketManager.SendSpawnPlayers(c.MyCharacter);
                    nearbyClients[i].Send(bufferSendMeToThem);

                    byte[] bufferSendPlayers = PacketManager.SendSpawnPlayers(nearbyClients[i].MyCharacter);
                    c.Send(bufferSendPlayers);
                }
            }

            for (int i = 0; i < nearbyMapItems.Length; i++)
            {
                byte[] bufferSendMapItems = PacketManager.SendItemOnGround(nearbyMapItems[i]);
                c.Send(bufferSendMapItems);
            }

            // this should send everything in range, players,monsters etc
            byte[] bufferSendSpawn12 = PacketManager.SendSpawn12(nearbyClients);
            c.Send(bufferSendSpawn12);

            Npc[] nearbyNpcs = mapEngine.GetNearbyNpcs(c.MyCharacter);

            byte[] bufferSendNpcList = PacketManager.SendSpawnNpc(nearbyNpcs.Cast<Npc>().ToList(), c.Map.MapID);
            c.Send(bufferSendNpcList);
        }

        void c_ClientSendPlayerIDInfo(object sender, ClientSendPlayerIDEventArgs e)
        {
            Client c = (Client)sender;
            SpawnRequestInfo sr = e.Info;

            byte[] bufferSendPlayerID = PacketManager.SendPlayerID(sr.CharacterID);
            c.Send(bufferSendPlayerID);
        }

        void c_ClientSpawnOtherPlayerInfo(object sender, ClientSpawnOtherPlayerEventArgs e)
        {
            Client c = (Client)sender;
            SpawnRequestInfo sr = e.Info;

            Character ch = characterManager.GetCharacterByCharacterId(sr.CharacterID);
            Client OtherClient = GetClientByCharacterID(ch.CharacterId);

            byte[] bufferSendOtherPlayers = PacketManager.SendSpawnPlayers(ch);
            c.Send(bufferSendOtherPlayers);

            byte[] bufferSendOtherPlayers2 = PacketManager.SendSpawnPlayers(c.MyCharacter);
            OtherClient.Send(bufferSendOtherPlayers2);
        }


        void c_ClientMovementInfo(object sender, ClientMovementInfoEventArgs e)
        {
            Client c = (Client)sender;
            MovementInfo mi = e.Info;

            Client[] nearbyClients = GetNearByClients(c);

            c.MyCharacter.X = mi.FromX;
            c.MyCharacter.Y = mi.FromY;

            Npc[] nearbyNpcs = mapEngine.GetNearbyNpcs(c.MyCharacter);

            byte[] bufferSendNpcList = PacketManager.SendSpawnNpc(nearbyNpcs.Cast<Npc>().ToList(), c.Map.MapID);
            c.Send(bufferSendNpcList);

            switch (mi.PacketID)
            {
                case 1:
                    byte[] SendStartMoving = PacketManager.SendStartMoving(mi, c.MyCharacter.MovingSpeed);
                    SendSpecific(nearbyClients, SendStartMoving);
                    break;

                case 2:
                    byte[] SendKeepMoving = PacketManager.SendKeepMoving(mi, c.MyCharacter.MovingSpeed);
                    c.Send(SendKeepMoving);
                    break;

                case 3:
                    byte[] SendStopMoving = PacketManager.SendStopMoving(mi);
                    SendSpecific(nearbyClients, SendStopMoving);
                    break;
            }
        }

        void c_ClientWalkToPortalInfo(object sender, ClientWalkToPortalEventArgs e)
        {
            Client c = (Client)sender;
            int MapID = e.MapID;

            Portal p = GetNearbyPortal(c);
            Client[] nearbyClients = GetNearByClients(c);

            if (p != null)
            {
                c.MyCharacter.X = p.ToX;
                c.MyCharacter.Y = p.ToY;
                c.MyCharacter.MapId = p.ToMapID;

                byte[] bufferRemoveMe = PacketManager.SendRemoveCharacter(c.MyCharacter);
                SendSpecific(nearbyClients, bufferRemoveMe);

                byte[] bufferSendPortal = PacketManager.SendPortal(p);
                c.Send(bufferSendPortal);
            }
            //c.Send(PacketManager.SendPortal());
            // need portal codes here
        }


        void c_ClientChatMessageInfo(object sender, ClientChatMessageEventArgs e)
        {
            Client c = (Client)sender;
            ChatMessageInfo cmi = e.Info;

            Client[] nearbyClients = GetNearByClients(c);
            Client target = null;

            if (cmi.Type == ChatType.Whisper)
            {
                if (cmi.TargetName != "")
                    target = GetClientByName(cmi.TargetName);
                else
                    target = GetClientByCharacterID(cmi.TargetID);

                if (target != null)
                {
                    cmi.TargetID = target.MyCharacter.CharacterId;
                    byte[] bufferSendMessage = PacketManager.SendChatMessage(c.MyCharacter, cmi);
                    target.Send(bufferSendMessage);
                }
                else
                {
                    cmi.TargetID = 0;
                }

                byte[] bufferSendMessage2 = PacketManager.SendChatMessage(c.MyCharacter, cmi);
                c.Send(bufferSendMessage2);
            }
            else
            {
                if (cmi.Message.StartsWith("!"))
                {
                    cmi.Type = ChatType.Notice;
                    cmi.Message = cmi.Message.Substring(1);
                }
                byte[] bufferSendMessage = PacketManager.SendChatMessage(c.MyCharacter, cmi);
                SendSpecific(nearbyClients, bufferSendMessage);
            }
        }


        void c_ClientCommandInfo(object sender, ClientCommandInfoEventArgs e)
        {
            Client c = (Client)sender;
            CommandInfo ci = e.Info;
            Client[] nearbyClients = GetNearByClients(c);

            byte[] bufferSendCommand = PacketManager.SendCommand(c.MyCharacter, ci);
            SendSpecific(nearbyClients, bufferSendCommand);
        }

        #endregion
    }
}
