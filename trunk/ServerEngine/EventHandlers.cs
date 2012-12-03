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

    public class PlayerConnectionChangedEventArgs : EventArgs
    {
        Character ch;
        bool inGame;

        public PlayerConnectionChangedEventArgs(Character CH, bool INGAME)
        {
            ch = CH;
            inGame = INGAME;
        }

        public Character Character { get { return ch; } }
        public bool InGame { get { return inGame; } }
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

    public class LogPacketEventArgs : EventArgs
    {
        Packet p;

        public LogPacketEventArgs(Packet logpacket)
        {
            p = logpacket;
        }

        public Packet LogPacket { get { return p; } }
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

    public class ClientSendPetEventArgs : EventArgs
    {
        private PetRequestInfo sr;

        public ClientSendPetEventArgs(PetRequestInfo spawn)
        {
            this.sr = spawn;
        }

        public PetRequestInfo Info { get { return sr; } }
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

    public class ClientSellItemEventArgs : EventArgs
    {
        private SellItemInfo sellinfo;

        public ClientSellItemEventArgs(SellItemInfo i)
        {
            sellinfo = i;
        }

        public SellItemInfo Info { get { return sellinfo; } }
    }

    public class ClientUseItemEventArgs : EventArgs
    {
        private UseItemInfo useinfo;

        public ClientUseItemEventArgs(UseItemInfo i)
        {
            useinfo = i;
        }

        public UseItemInfo Info { get { return useinfo; } }
    }

    public class ClientUpdateQuickSlotEventArgs : EventArgs
    {
        private UpdateQuickSlotInfo quickInfo;

        public ClientUpdateQuickSlotEventArgs(UpdateQuickSlotInfo i)
        {
            quickInfo = i;
        }

        public UpdateQuickSlotInfo Info { get { return quickInfo; } }
    }

    public class ClientOpenWarehouseEventArgs : EventArgs
    {
        private OpenWarehouseInfo info;

        public ClientOpenWarehouseEventArgs(OpenWarehouseInfo i)
        {
            info = i;
        }

        public OpenWarehouseInfo Info { get { return info; } }
    }

    public class ClientAddItemToWarehouseEventargs : EventArgs
    {
        private AddItemToWarehouseInfo info;

        public ClientAddItemToWarehouseEventargs(AddItemToWarehouseInfo i)
        {
            info = i;
        }

        public AddItemToWarehouseInfo Info { get { return info; } }
    }

    public class ClientMoveWarehouseItemToBagEventArgs : EventArgs
    {
        private MoveWarehouseItemToBagInfo info;

        public ClientMoveWarehouseItemToBagEventArgs(MoveWarehouseItemToBagInfo i)
        {
            info = i;
        }

        public MoveWarehouseItemToBagInfo Info { get { return info; } }
    }

    public class ClientMoveWarehouseItemEventArgs : EventArgs
    {
        private MoveWarehouseItemInfo info;

        public ClientMoveWarehouseItemEventArgs(MoveWarehouseItemInfo i)
        {
            info = i;
        }

        public MoveWarehouseItemInfo Info { get { return info; } }
    }

    public class ClientAddItemToShopEventArgs : EventArgs
    {
        private AddItemToShopInfo info;

        public ClientAddItemToShopEventArgs(AddItemToShopInfo i)
        {
            info = i;
        }

        public AddItemToShopInfo Info { get { return info; } }
    }

    public class ClientMoveShopItemEventArgs : EventArgs
    {
        private MoveShopItemInfo info;

        public ClientMoveShopItemEventArgs(MoveShopItemInfo i)
        {
            info = i;
        }

        public MoveShopItemInfo Info { get { return info; } }
    }

    public class ClientChangeShopInfoEventArgs : EventArgs
    {
        private ChangeShopInfo info;

        public ClientChangeShopInfoEventArgs(ChangeShopInfo i)
        {
            info = i;
        }

        public ChangeShopInfo Info { get { return info; } }
    }

    public class ClientShopTakeMoneyInfoEventArgs : EventArgs
    {
        private ShopTakeMoneyInfo info;

        public ClientShopTakeMoneyInfoEventArgs(ShopTakeMoneyInfo i)
        {
            info = i;
        }

        public ShopTakeMoneyInfo Info {get {return info;}}
    }

    public class ClientMoveShopItemToBagEventArgs : EventArgs
    {
        private MoveShopItemToBagInfo info;

        public ClientMoveShopItemToBagEventArgs(MoveShopItemToBagInfo i)
        {
            info = i;
        }

        public MoveShopItemToBagInfo Info { get { return info; } }
    }

    public class ClientShopStartSellingEventArgs : EventArgs
    {
        private ShopStartSellingInfo info;

        public ClientShopStartSellingEventArgs(ShopStartSellingInfo i)
        {
            info = i;
        }

        public ShopStartSellingInfo Info { get { return info; } }
    }

    public class ClientOpenOtherPlayerShopEventArgs : EventArgs
    {
        private OpenOtherPlayerShopInfo info;

        public ClientOpenOtherPlayerShopEventArgs(OpenOtherPlayerShopInfo i)
        {
            info = i;
        }

        public OpenOtherPlayerShopInfo Info { get { return info; } }
    }

    public class ClientBuyOtherPlayerShopItemEventArgs : EventArgs
    {
        private BuyOtherPlayerShopItem info;

        public ClientBuyOtherPlayerShopItemEventArgs(BuyOtherPlayerShopItem i)
        {
            info = i;
        }

        public BuyOtherPlayerShopItem Info { get { return info; } }
    }

    public class ClientTradePlayerEventArgs : EventArgs
    {
        private TradePlayerInfo info;

        public ClientTradePlayerEventArgs(TradePlayerInfo i)
        {
            info = i;
        }

        public TradePlayerInfo Info { get { return info; } }
    }

    public class ClientTradePlayerCommandsEventArgs : EventArgs
    {
        private TradePlayerCommandsInfo info;

        public ClientTradePlayerCommandsEventArgs(TradePlayerCommandsInfo i)
        {
            info = i;
        }

        public TradePlayerCommandsInfo Info { get { return info; } }
    }

    public class ClientTradeAddMoneyEventArgs : EventArgs
    {
        private TradeAddMoneyInfo info;

        public ClientTradeAddMoneyEventArgs(TradeAddMoneyInfo i)
        {
            info = i;
        }

        public TradeAddMoneyInfo Info { get { return info; } }
    }

    public class ClientTradeAddItemEventArgs : EventArgs
    {
        private TradeAddItemInfo info;

        public ClientTradeAddItemEventArgs(TradeAddItemInfo i)
        {
            info = i;
        }

        public TradeAddItemInfo Info { get { return info; } }
    }

    public class ClientLearnSkillEventArgs : EventArgs
    {
        private LearnSkillInfo info;

        public ClientLearnSkillEventArgs(LearnSkillInfo i)
        {
            info = i;
        }

        public LearnSkillInfo Info { get { return info; } }
    }

    public class ClientAddFriendEventArgs : EventArgs
    {
        private FriendAddInfo info;

        public ClientAddFriendEventArgs(FriendAddInfo i)
        {
            info = i;
        }

        public FriendAddInfo Info { get { return info; } }
    }

    public class ClientMonsterSpawnRequestEventArgs : EventArgs
    {
        private MonsterSpawnRequestInfo info;

        public ClientMonsterSpawnRequestEventArgs(MonsterSpawnRequestInfo i)
        {
            info = i;
        }

        public MonsterSpawnRequestInfo Info { get { return info; } }
    }

    public class ClientRequestFriendListEventArgs : EventArgs
    {
    }

    public class ClientAttackEventArgs : EventArgs
    {
        private AttackInfo info;

        public ClientAttackEventArgs(AttackInfo i)
        {
            info = i;
        }

        public AttackInfo Info { get { return info; } }
    }

    public class ClientCastSkillEventArgs : EventArgs
    {
        private CastSkillInfo info;

        public ClientCastSkillEventArgs(CastSkillInfo i)
        {
            info = i;
        }

        public CastSkillInfo Info { get { return info; } }
    }

    public class ClientSelectSkillEventArgs : EventArgs
    {
        private SelectSkillInfo info;

        public ClientSelectSkillEventArgs(SelectSkillInfo i)
        {
            info = i;
        }

        public SelectSkillInfo Info { get { return info; } }
    }

    public class ClientUseTeleporterEventArgs : EventArgs
    {
        private UseTeleporterInfo info;

        public ClientUseTeleporterEventArgs(UseTeleporterInfo i)
        {
            info = i;
        }

        public UseTeleporterInfo Info { get { return info; } }
    }

    public class ClientCombineItemsEvenArgs : EventArgs
    {
        private CombineItemsInfo info;

        public ClientCombineItemsEvenArgs(CombineItemsInfo i)
        {
            info = i;
        }

        public CombineItemsInfo Info { get { return info; } }
    }

    public class ClientPartyRequestEventArgs : EventArgs
    {
        private PartyRequestInfo info;

        public ClientPartyRequestEventArgs(PartyRequestInfo i)
        {
            info = i;
        }

        public PartyRequestInfo Info { get { return info; } }
    }

    public class ClientAddToPartyRequestEventArgs : EventArgs
    {
        private AddToPartyRequestInfo info;

        public ClientAddToPartyRequestEventArgs(AddToPartyRequestInfo i)
        {
            info = i;
        }

        public AddToPartyRequestInfo Info { get { return info; } }
    }

    public class ClientLeavePartyEventArgs : EventArgs
    {
        private LeavePartyInfo info;

        public ClientLeavePartyEventArgs(LeavePartyInfo i)
        {
            info = i;
        }

        public LeavePartyInfo Info { get { return info; } }
    }

    public class ClientChangePartyLeaderEventArgs : EventArgs
    {
        private ChangePartyLeaderInfo info;

        public ClientChangePartyLeaderEventArgs(ChangePartyLeaderInfo i)
        {
            info = i;
        }

        public ChangePartyLeaderInfo Info { get { return info; } }
    }

    public class ClientPetMovementInfoEventArgs : EventArgs
    {
        private MovementInfo mi;

        public ClientPetMovementInfoEventArgs(MovementInfo MI)
        {
            this.mi = MI;
        }

        public MovementInfo Info { get { return mi; } }
    }

    public class ClientKillPetInfoEventArgs : EventArgs
    {
        private KillPet info;

        public ClientKillPetInfoEventArgs(KillPet i)
        {
            info = i;
        }

        public KillPet Info { get { return info; } }
    }

    public class ClientBaitPetInfoEventArgs : EventArgs
    {
        private BaitPetInfo info;

        public ClientBaitPetInfoEventArgs(BaitPetInfo i)
        {
            info = i;
        }

        public BaitPetInfo Info { get { return info; } }
    }

    public class ClientRequestPetStatsInfoEventArgs : EventArgs
    {
        private RequestPetStats info;

        public ClientRequestPetStatsInfoEventArgs(RequestPetStats i)
        {
            info = i;
        }

        public RequestPetStats Info { get { return info; } }
    }

    public class ClientRequestSpawnOtherPetInfoEventArgs : EventArgs
    {
        private RequestSpawnOtherPet info;

        public ClientRequestSpawnOtherPetInfoEventArgs(RequestSpawnOtherPet i)
        {
            info = i;
        }

        public RequestSpawnOtherPet Info { get { return info; } }
    }

    public class ClientResurrectPetInfoEventArgs : EventArgs
    {
        private ResurrectPetInfo info;

        public ClientResurrectPetInfoEventArgs(ResurrectPetInfo i)
        {
            info = i;
        }

        public ResurrectPetInfo Info { get { return info; } }
    }

    public class ClientRenamePetInfoEventArgs : EventArgs
    {
        private RenamePetInfo info;

        public ClientRenamePetInfoEventArgs(RenamePetInfo i)
        {
            info = i;
        }

        public RenamePetInfo Info { get { return info; } }
    }

    public class ClientSendPetToMeInfoEventArgs : EventArgs
    {
        private SendPetToMeInfo info;

        public ClientSendPetToMeInfoEventArgs(SendPetToMeInfo i)
        {
            info = i;
        }

        public SendPetToMeInfo Info { get { return info; } }
    }

    public class ClientTradePetInfoEventArgs : EventArgs
    {
        private TradePetInfo info;

        public ClientTradePetInfoEventArgs(TradePetInfo i)
        {
            info = i;
        }

        public TradePetInfo Info { get { return info; } }
    }

    public class ClientTradePetAmountInfoEventArgs : EventArgs
    {
        private TradePetAmountInfo info;

        public ClientTradePetAmountInfoEventArgs(TradePetAmountInfo i)
        {
            info = i;
        }

        public TradePetAmountInfo Info { get { return info; } }
    }

    public class ClientSealPetInfoEventArgs : EventArgs
    {
        private SealPetInfo info;

        public ClientSealPetInfoEventArgs(SealPetInfo i)
        {
            info = i;
        }

        public SealPetInfo Info { get { return info; } }
    }

    public class ClientUnSealPetInfoEventArgs : EventArgs
    {
        private UnSealPetInfo info;

        public ClientUnSealPetInfoEventArgs(UnSealPetInfo i)
        {
            info = i;
        }

        public UnSealPetInfo Info { get { return info; } }
    }

    public class ClientStackItemInfoEventArgs : EventArgs
    {
        private StackItemInfo info;

        public ClientStackItemInfoEventArgs(StackItemInfo i)
        {
            info = i;
        }

        public StackItemInfo Info { get { return info; } }
    }

    public class ClientRequestDeath : EventArgs
    {
        public ClientRequestDeath()
        {
        }
    }

    public class ClientCreateGuildInfoEventArgs : EventArgs
    {
        private CreateGuildInfo info;

        public ClientCreateGuildInfoEventArgs(CreateGuildInfo i)
        {
            info = i;
        }

        public CreateGuildInfo Info { get { return info; } }
    }

    public class ClientRequestGuildEventArgs : EventArgs
    {
        public ClientRequestGuildEventArgs()
        {
        }
    }

    public class ClientRequestGuildInfoEventArgs : EventArgs
    {
        private RequestGuildInfo info;

        public ClientRequestGuildInfoEventArgs(RequestGuildInfo i)
        {
            info = i;
        }

        public RequestGuildInfo Info { get { return info; } }
    }

    public class ClientRequestGuildMemberStatsEventArgs : EventArgs
    {
        private RequestGuildMemberStatsInfo info;

        public ClientRequestGuildMemberStatsEventArgs(RequestGuildMemberStatsInfo i)
        {
            info = i;
        }

        public RequestGuildMemberStatsInfo Info { get { return info; } }
    }

    public class ClientRequestGuildMemberRankChangeEventArgs : EventArgs
    {
        private RequestGuildMemberChangeRankInfo info;

        public ClientRequestGuildMemberRankChangeEventArgs(RequestGuildMemberChangeRankInfo i)
        {
            info = i;
        }

        public RequestGuildMemberChangeRankInfo Info { get { return info; } }
    }

    public class ClientRequestGuildMemberChangeTitleEventArgs : EventArgs
    {
        private RequestGuildMemberChangeTitleInfo info;

        public ClientRequestGuildMemberChangeTitleEventArgs(RequestGuildMemberChangeTitleInfo i)
        {
            info = i;
        }

        public RequestGuildMemberChangeTitleInfo Info { get { return info; } }
    }

    public class ClientRequestGuildChangeNoticeEventArgs : EventArgs
    {
        private RequestGuildChangeNoticeInfo info;

        public ClientRequestGuildChangeNoticeEventArgs(RequestGuildChangeNoticeInfo i)
        {
            info = i;
        }

        public RequestGuildChangeNoticeInfo Info { get { return info; } }
    }

    public class ClientRequestJoinGuildEventArgs : EventArgs
    {
        private RequestJoinGuildInfo info;

        public ClientRequestJoinGuildEventArgs(RequestJoinGuildInfo i)
        {
            info = i;
        }

        public RequestJoinGuildInfo Info { get { return info; } }
    }

    public class ClientRequestCloseGuildEventArgs : EventArgs
    {
        public ClientRequestCloseGuildEventArgs()
        {
        }
    }

    public class ClientRequestLeaveGuildEventArgs : EventArgs
    {
        private RequestLeaveGuildInfo info;

        public ClientRequestLeaveGuildEventArgs(RequestLeaveGuildInfo i)
        {
            info = i;
        }

        public RequestLeaveGuildInfo Info { get { return info; } }
    }

    public class ClientGuildChatEventArgs : EventArgs
    {
        private GuildChatInfo info;

        public ClientGuildChatEventArgs(GuildChatInfo i)
        {
            info = i;
        }

        public GuildChatInfo Info { get { return info; } }
    }

    public class ClientUseRebirthPillEventArgs : EventArgs
    {
        private UseRebirthPillInfo info;

        public ClientUseRebirthPillEventArgs(UseRebirthPillInfo i)
        {
            info = i;
        }

        public UseRebirthPillInfo Info { get { return info; } }
    }

    #endregion

#region Monster Engine

    public class MonsterMoveInfoEventArgs : EventArgs
    {
        private MonsterMoveInfo info;

        public MonsterMoveInfoEventArgs(MonsterMoveInfo i)
        {
            this.info = i;
        }

        public MonsterMoveInfo Info { get { return info; } }
    }

    public class MonsterAttackInfoEventArgs : EventArgs
    {
        private MonsterAttackInfo info;

        public MonsterAttackInfoEventArgs(MonsterAttackInfo i)
        {
            this.info = i;
        }

        public MonsterAttackInfo Info { get { return info; } }
    }
#endregion
}
