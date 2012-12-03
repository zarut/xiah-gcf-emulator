using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using Entities;
using ServerEngine.PacketEngine;

namespace ServerEngine
{
    public class ClientConnectionEventArgs : EventArgs
    {
        private Client client;

        public ClientConnectionEventArgs(Client client)
        {
            this.client = client;
        }

        public Client Client
        {
            get { return client; }
        }
    }

    public class ClientExceptionEventArgs : EventArgs
    {
        Exception e;

        public ClientExceptionEventArgs(Exception e)
        {
            this.e = e;
        }

        public Exception ReferencedException
        {
            get { return e; }
        }
    }


    public class UnknownPacketEventArgs : EventArgs
    {
        Packet p;

        public UnknownPacketEventArgs(Packet unknown)
        {
            p = unknown;
        }

        public Packet UnknownPacket { get { return p; } }
    }

    public class ClientLoginEventArgs : EventArgs
    {
        public ClientLoginEventArgs(GameLogin gameLogin, Packet p)
        {
            this.gameLogin = gameLogin;
        }

        private GameLogin gameLogin;

        public GameLogin GameLoginDetails
        {
            get { return gameLogin; }
        }
    }

    public class ClientWorldSelectedEventArgs : EventArgs
    {

        public ClientWorldSelectedEventArgs(int worldID, Packet p)
        {
            this.worldID = worldID;
        }

        private int worldID;

        public int WorldID { get {return worldID;} }
    }

    public class ClientCharacterCreateInfoEventArgs : EventArgs
    {
        private Character createCharacter;

        public ClientCharacterCreateInfoEventArgs(Character createCharacterDetails, Packet p)
        {
            this.createCharacter = createCharacterDetails;
        }

        public Character CreateCharacterDetails { get { return createCharacter; } }
    }

    public class ClientChannelRequestEventArgs : EventArgs
    {
        private ChannelRequest channelRequest;

        public ClientChannelRequestEventArgs(ChannelRequest channelRequestDetails, Packet p)
        {
            this.channelRequest = channelRequestDetails;
        }

        public ChannelRequest ChannelRequestDetails { get { return channelRequest; } }
    }

    public class ClientUnitConnectionInfoEventArgs : EventArgs
    {
        private UnitLogin unitlogin;

        public ClientUnitConnectionInfoEventArgs(UnitLogin unitLoginDetails, Packet p)
        {
            this.unitlogin = unitLoginDetails;
        }

        public UnitLogin UnitLoginDetails { get { return unitlogin; } }
    }

    #region Channel Server

    public class ClientChannelLoginInfoEventArgs : EventArgs
    {
        private ChannelLogin channelLogin;

        public ClientChannelLoginInfoEventArgs(ChannelLogin ch)
        {
            this.channelLogin = ch;
        }

        public ChannelLogin ChannelLogin { get { return channelLogin; } }
    }

    public class ClientChannelChangeEventArgs : EventArgs
    {
        private ChannelChangeInfo channelInfo;

        public ClientChannelChangeEventArgs(ChannelChangeInfo cci)
        {
            this.channelInfo = cci;
        }

        public ChannelChangeInfo Info { get { return channelInfo; } }
    }

    public class ClientWalkToPortalEventArgs : EventArgs
    {
        private int mapID;

        public ClientWalkToPortalEventArgs(int map)
        {
            this.mapID = map;
        }

        public int MapID { get { return mapID; } }
    }

    public class ClientChannelChangeStuffEventArgs : EventArgs
    {
        public ClientChannelChangeStuffEventArgs()
        {
        }
    }

    public class ClientMapLoadEventArgs : EventArgs
    {
        private MapRequestInfo mr;

        public ClientMapLoadEventArgs(MapRequestInfo MR)
        {
            this.mr = MR;
        }

        public MapRequestInfo Map { get { return mr; } }
    }

    public class ClientSkillLoadEvenArgs : EventArgs
    {
        private int skill;

        public ClientSkillLoadEvenArgs(int Skill)
        {
            this.skill = Skill;
        }

        public int SkillList { get { return skill; } }
    }

    public class ClientInventoryLoadEvenArgs : EventArgs
    {
        private int inv;

        public ClientInventoryLoadEvenArgs(int inventory)
        {
            this.inv = inventory;
        }

        public int Inventory { get { return inv; } }
    }

    public class ClientStatLoadEventArgs : EventArgs
    {
        private StatRequestInfo sr;

        public ClientStatLoadEventArgs(StatRequestInfo Stats)
        {
            this.sr = Stats;
        }

        public StatRequestInfo Info { get { return sr; } }
    }

    public class ClientAddStatEventArgs : EventArgs
    {
        private AddStatInfo asi;

        public ClientAddStatEventArgs(AddStatInfo AddStat)
        {
            this.asi = AddStat;
        }

        public AddStatInfo Info { get { return asi; } }
    }

    public class ClientSpawnLoadEventArgs : EventArgs
    {
        private SpawnRequestInfo sr;

        public ClientSpawnLoadEventArgs(SpawnRequestInfo spawn)
        {
            this.sr = spawn;
        }

        public SpawnRequestInfo Info { get { return sr; } }
    }

    public class ClientSpawnOtherPlayerEventArgs : EventArgs
    {
        private SpawnRequestInfo sr;

        public ClientSpawnOtherPlayerEventArgs(SpawnRequestInfo spawn)
        {
            this.sr = spawn;
        }

        public SpawnRequestInfo Info { get { return sr; } }
    }

    public class ClientRequestStatsLoadInfo : EventArgs
    {
        private SpawnRequestInfo sr;

        public ClientRequestStatsLoadInfo(SpawnRequestInfo spawn)
        {
            this.sr = spawn;
        }

        public SpawnRequestInfo Info { get { return sr; } }
    }

    public class ClientSendPlayerIDEventArgs : EventArgs
    {
        private SpawnRequestInfo sr;

        public ClientSendPlayerIDEventArgs(SpawnRequestInfo spawn)
        {
            this.sr = spawn;
        }

        public SpawnRequestInfo Info { get { return sr; } }
    }

    public class ClientMovementInfoEventArgs : EventArgs
    {
        private MovementInfo mi;

        public ClientMovementInfoEventArgs(MovementInfo MI)
        {
            this.mi = MI;
        }

        public MovementInfo Info { get { return mi; } }
    }

    public class ClientChatMessageEventArgs : EventArgs
    {
        private ChatMessageInfo chat;

        public ClientChatMessageEventArgs(ChatMessageInfo message)
        {
            this.chat = message;
        }

        public ChatMessageInfo Info { get { return chat; } }
    }

    public class ClientCommandInfoEventArgs : EventArgs
    {
        private CommandInfo com;

        public ClientCommandInfoEventArgs(CommandInfo c)
        {
            this.com = c;
        }

        public CommandInfo Info { get { return com; } }
    }

    public class ClientMoveItemEventArgs : EventArgs
    {
        private MoveItemInfo moveinfo;

        public ClientMoveItemEventArgs(MoveItemInfo mii)
        {
            this.moveinfo = mii;
        }

        public MoveItemInfo Info { get { return moveinfo; } }
    }

    public class ClientDropItemEventArgs : EventArgs
    {
        private DropItemInfo dropinfo;

        public ClientDropItemEventArgs(DropItemInfo dii)
        {
            this.dropinfo = dii;
        }

        public DropItemInfo Info { get { return dropinfo; } }
    }

    public class ClientPickItemEventArgs : EventArgs
    {
        private PickItemInfo pickinfo;

        public ClientPickItemEventArgs(PickItemInfo p)
        {
            this.pickinfo = p;
        }

        public PickItemInfo Info { get { return pickinfo; } }
    }

    public class ClientImbueItemEventArgs : EventArgs
    {
        private ImbueItemInfo imbueinfo;

        public ClientImbueItemEventArgs(ImbueItemInfo i)
        {
            this.imbueinfo = i;
        }

        public ImbueItemInfo Info { get { return imbueinfo; } }
    }

    public class ClientAcceptImbueItemEventArgs : EventArgs
    {
        private AcceptImbueItem acceptinfo;

        public ClientAcceptImbueItemEventArgs(AcceptImbueItem i)
        {
            this.acceptinfo = i;
        }

        public AcceptImbueItem Info { get { return acceptinfo; } }
    }

    public class ClientNpcTradeEventArgs : EventArgs
    {
        private NpcTradeInfo tradeinfo;

        public ClientNpcTradeEventArgs(NpcTradeInfo n)
        {
            this.tradeinfo = n;
        }

        public NpcTradeInfo Info { get { return tradeinfo; } }
    }

    public class ClientBuyItemEventArgs : EventArgs
    {
        private BuyItemInfo buyinfo;

        public ClientBuyItemEventArgs(BuyItemInfo i)
        {
            buyinfo = i;
        }

        public BuyItemInfo Info { get { return buyinfo; } }
    }

    #endregion
}
