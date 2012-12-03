using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XiahBLL
{
    class DbNames
    {


        public const string USERLOGIN_STOREDPROC = "UserLogin";
        public const string USERLOGIN_USERNAME_PARAMETER = "@username";
        public const string USERLOGIN_PASSWORD_PARAMETER = "@password";
        public const string USERLOGIN_CONNECTIONTIME_PARAMETER = "@connectionTime";

        public const string LOGINVERIFICATION_STOREDPROC = "LoginVerification";
        public const string LOGINVERIFICATION_USERNAME_PARAMETER = "@username";
        public const string LOGINVERIFICATION_ACCOUNTID_PARAMETER = "@accountId";

        public const string EXITGAME_STOREDPROC = "ExitGame";
        public const string CONNECTVERIFICATION_STOREDPROC = "ConnectVerification";
        public const string CONNECTVERIFICATION_ACCOUNTID_PARAMETER = "@accountId";


        public const string GETALLCHARACTERSBYACCOUNTID_STOREDPROC = "GetAllCharactersByAccountId";
        public const string GETALLCHARACTERSBYACCOUNTID_ACCOUNTID_PARAMETER = "@accountId";


        public const string GETALLNPCITEMSBYBAGID_STOREDPROC = "GetItemsByNpcBagID";
        public const string GETALLNPCITEMSBYBAGID_BAGID_PARAMETER = "@bag";
        public const string GETALLNPCITEMSBYBAGID_NPCID_PARAMETER = "@npcId";

        public const string GETITEMBYREFERENCEID_STOREDPROC = "GetItemByItemReferenceID";
        public const string GETITEMBYREFERNECEID_REFID_PARAMETER = "@refID";

        public const string GETALLITEMSBYBAGID_STOREDPROC = "GetItemsByBagID";
        public const string GETALLITEMSBYBAGID_BAGID_PARAMETER = "@bag";
        public const string GETALLITEMSBYBAGID_CHARACTERID_PARAMETER = "@characterId";

        public const string INSERTITEM_STOREDPROC = "InsertItem";
        public const string INSERTITEM_REFERENCEID_PARAMETER = "@referenceId";
        public const string INSERTITEM_CHARACTERID_PARAMETER = "@characterId";
        public const string INSERTITEM_AMOUNT_PARAMETER = "@amount";
        public const string INSTERITEM_BAG_PARAMETER = "@bag";
        public const string INSERTITEM_SLOT_PARAMETER = "@slot";
        public const string INSERTITEM_ITEMID_PARAMETER = "@newItemId";

        public const string GETITEMBYITEMID_STOREDPROC = "GetItemByItemID";
        public const string GETITEMBYITEMID_ITEMID_PARAMETER = "@itemId";

        public const string GETCHARACTERBYCHARACTERID_STOREDPROC = "GetCharacterByCharacterId";
        public const string GETCHARACTERBYCHARACTERID_CHARACTERID_PARAMETER = "@characterId";

        public const string CREATECHARACTER_STOREDPROC = "CreateCharacter";
        public const string CREATECHARACTER_STOREDPROC_NAME_PARAMETER = "@name";
        public const string CREATECHARACTER_STOREDPROC_CLASS_PARAMETER = "@class";
        public const string CREATECHARACTER_STOREDPROC_ACCOUNTID_PARAMETER = "@accountId";
        public const string CREATECHARACTER_STOREDPROC_CHARACTERID_PARAMETER = "@characterId";

        public const string RETURNVALUE_PARAMETER = "@ReturnValue";

        public const string GETALLWORLDS_STOREDPROC = "GetAllWorlds";
        public const string GETALLCHANNELS_STOREDPROC = "GetAllChannels";

        public const string GETMAPBYID_STOREDPROC = "GetMapById";
        public const string GETMAPBYID_ID_PARAMETER = "@mapId";

        public const string GETPORTALSBYMAPID_STOREDPROC = "GetPortalsByMapId";
        public const string GETPORTALSBYMAPID_ID_PARAMETER = "@mapId";

        public const string GETNPCSBYMAPID_STOREDPROC = "GetNpcsByMapId";
        public const string GETNPCSBYMAPID_ID_PARAMETER = "@mapId";

        public const string GETALLMAPS_STOREDPROC = "GetAllMaps";

        public const string INSERTMAPITEM_STOREDPROC = "InsertMapItem";

        public const string INSERTMAPITEM_MAPID_PARAMETER = "@mapId";
        public const string INSERTMAPITEM_MAPX_PARAMETER = "@mapX";
        public const string INSERTMAPITEM_MAPY_PARAMETER = "@mapY";
        public const string INSERTMAPITEM_MAPZ_PARAMETER = "@mapZ";
        public const string INSERTMAPITEM_BTYPE_PARAMETER = "@bType";
        public const string INSERTMAPITEM_VISUALID_PARAMETER = "@visualId";
        public const string INSERTMAPITEM_REFERENCEID_PARAMETER = "@referenceId";
        public const string INSERTMAPITEM_ITEMID_PARAMETER = "@itemId";
        public const string INSERTMAPITEM_AMOUNT_PARAMETER = "@amount";
        public const string INSERTMAPITEM_MAPITEMID_OUTPUT = "@mapItemId";

        public const string DELETEMAPITEM_STOREDPROC = "DeleteMapItem";
        public const string DELETEMAPITEM_MAPITEMID_PARAMETER = "@MapItemId";

        public const string UPDATEITEM_STOREDPROC = "UpdateItemById";
        public const string UPDATEITEM_OWNERID_PARAMETER = "@OwnerID";
        public const string UPDATEITEM_COST_PARAMETER = "@Cost";
        public const string UPDATEITEM_REQCLASS_PARAMETER = "@ReqClass";
        public const string UPDATEITEM_AMOUNT_PARAMETER = "@Amount";
        public const string UPDATEITEM_REQLEVEL_PARAMETER = "@ReqLevel";
        public const string UPDATEITEM_REQDEX_PARAMETER = "@ReqDex";
        public const string UPDATEITEM_REQSTR_PARAMETER = "@ReqStr";
        public const string UPDATEITEM_REQSTA_PARAMETER = "@ReqSta";
        public const string UPDATEITEM_REQENE_PARAMETER = "@ReqEne";
        public const string UPDATEITEM_MAXIMBUETRIES_PARAMETER = "@MaxImbueTries";
        public const string UPDATEITEM_MAXDURA_PARAMETER = "@MaxDura";
        public const string UPDATEITEM_CURDURA_PARAMETER = "@CurDura";
        public const string UPDATEITEM_DAMAGE_PARAMETER = "@Damage";
        public const string UPDATEITEM_DEFENCE_PARAMETER = "@Defence";
        public const string UPDATEITEM_ATTACKRATING_PARAMETER = "@AttackRating";
        public const string UPDATEITEM_ATTACKSPEED_PARAMETER = "@AttackSpeed";
        public const string UPDATEITEM_ATTACKRANGE_PARAMETER = "@AttackRange";
        public const string UPDATEITEM_INCMAXLIFE_PARAMETER = "@IncMaxLife";
        public const string UPDATEITEM_INCMAXMANA_PARAMETER = "@IncMaxMana";
        public const string UPDATEITEM_LIFEREGEN_PARAMETER = "@LifeRegen";
        public const string UPDATEITEM_MANAREGEN_PARAMETER = "@ManaRegen";
        public const string UPDATEITEM_CRITICALHIT_PARAMETER = "@CriticalHit";
        public const string UPDATEITEM_PLUS_PARAMETER = "@Plus";
        public const string UPDATEITEM_SLVL_PARAMETER = "@Slvl";
        public const string UPDATEITEM_IMBUETRIES_PARAMETER = "@ImbueTries";
        public const string UPDATEITEM_DRAGONSUCCESSIMBUETRIES_PARAMETER = "@DragonSuccessImbueTries";
        public const string UPDATEITEM_DISCOUNTREPAIRFEE_PARAMETER = "@DiscountRepairFee";
        public const string UPDATEITEM_TOTALDRAGONIMBUETRIES_PARAMETER = "@TotalDragonImbueTries";
        public const string UPDATEITEM_DRAGONDMG_PARAMETER = "@DragonDmg";
        public const string UPDATEITEM_DRAGONDEF_PARAMETER = "@DragonDef";
        public const string UPDATEITEM_DRAGONAR_PARAMETER = "@DragonAR";
        public const string UPDATEITEM_DRAGONLIFE_PARAMETER = "@DragonLife";
        public const string UPDATEITEM_MAPPEDSTUFF_PARAMETER = "@MappedStuff";
        public const string UPDATEITEM_FORCENUMBER_PARAMETER = "@ForceNumber";
        public const string UPDATEITEM_REBIRTHHOLE_PARAMETER = "@RebirthHole";
        public const string UPDATEITEM_REBIRTHHOLEITEM_PARAMETER = "@RebirthHoleItem";
        public const string UPDATEITEM_REBIRTHHOLESTAT_PARAMETER = "@RebirthHoleStat";
        public const string UPDATEITEM_IMBUERATE_PARAMETER = "@ImbueRate";
        public const string UPDATEITEM_IMBUEINCREASE_PARAMETER = "@ImbueIncrease";
        public const string UPDATEITEM_POLISHSTONETRIES_PARAMETER = "@PolishStoneTries";
        public const string UPDATEITEM_VIGISTAT1_PARAMETER = "@VigiStat1";
        public const string UPDATEITEM_VIGISTAT2_PARAMETER = "@VigiStat2";
        public const string UPDATEITEM_VIGISTAT3_PARAMETER = "@VigiStat3";
        public const string UPDATEITEM_VIGISTAT4_PARAMETER = "@VigiStat4";
        public const string UPDATEITEM_VIGISTATADD1_PARAMETER = "@VigiStatAdd1";
        public const string UPDATEITEM_VIGISTATADD2_PARAMETER = "@VigiStatAdd2";
        public const string UPDATEITEM_VIGISTATADD3_PARAMETER = "@VigiStatAdd3";
        public const string UPDATEITEM_VIGISTATADD4_PARAMETER = "@VigiStatAdd4";
        public const string UPDATEITEM_BAG_PARAMETER = "@Bag";
        public const string UPDATEITEM_SLOT_PARAMETER = "@Slot";
        public const string UPDATEITEM_ITEMID_PARAMETER = "@ItemId";

        #region WorldInfo Table
        public const string WORLDINFO_ID = "ID";
        public const string WORLDINFO_NAME = "Name";
        public const string WORLDINFO_DESC = "Desc";
        public const string WORLDINFO_IP = "IP";
        public const string WORLDINFO_PORT = "Port";
        #endregion

        #region ChannelInfo Table
        public const string CHANNELINFO_ID = "ID";
        public const string CHANNELINFO_NAME = "Name";
        public const string CHANNELINFO_DESC = "Desc";
        public const string CHANNELINFO_IP = "IP";
        public const string CHANNELINFO_PORT = "Port";
        public const string CHANNELINFO_PK = "PK";
        #endregion

        #region Characters Table
        public const string CHARACTER_ID = "ID";
        public const string CHARACTER_ACCOUNTID = "AccountId";
        public const string CHARACTER_NAME = "Name";
        public const string CHARACTER_CLASS = "Class";
        public const string CHARACTER_LEVEL = "Level";
        public const string CHARACTER_MAPID = "MapId";
        public const string CHARACTER_CURRENTHP = "CurrentHp";
        public const string CHARACTER_CURRENTMANA = "CurrentMana";
        public const string CHARACTER_STRENGTH = "Strength";
        public const string CHARACTER_STAMINA = "Stamina";
        public const string CHARACTER_DEXTERITY = "Dexterity";
        public const string CHARACTER_ENERGY = "Energy";
        public const string CHARACTER_X = "X";
        public const string CHARACTER_Y = "Y";
        public const string CHARACTER_FAME = "Fame";
        public const string CHARACTER_MONEY = "Money";
        public const string CHARACTER_STATPOINT = "StatPoints";
        public const string CHARACTER_TRAININGPOINT = "TrainingPoints";
        public const string CHARACTER_FIVEELEMENTPOINT = "FiveElementPoints";
        public const string CHARACTER_REPULATIONPOINT = "RepulationPoints";
        public const string CHARACTER_CURRENTEXP = "CurrentExp";
        public const string CHARACTER_CURRENTFEEXP = "CurrentFEExp";
        public const string CHARACTER_REBIRTH = "Rebirths";
        #endregion

        #region Items Table
        public const string ITEM_ITEMID = "ItemID";
        public const string ITEM_OWNERID = "OwnerID";
        public const string ITEM_REFID = "ItemReferenceID";
        public const string ITEM_REFERENCEID = "ReferenceID";
        public const string ITEM_BTYPE = "bType";
        public const string ITEM_BKIND = "bKind";
        public const string ITEM_VISUALID = "VisualID";
        public const string ITEM_COST = "Cost";
        public const string ITEM_PRICE = "Price";
        public const string ITEM_CLASS = "ReqClass";
        public const string ITEM_AMOUNT = "Amount";
        public const string ITEM_LEVEL = "ReqLevel";
        public const string ITEM_DEX = "ReqDex";
        public const string ITEM_STR = "ReqStr";
        public const string ITEM_STA = "ReqSta";
        public const string ITEM_ENE = "ReqEne";
        public const string ITEM_MAXIMBUES = "MaxImbueTries";
        public const string ITEM_DURABILITY = "Durability";
        public const string ITEM_MAXDURA = "MaxDura";
        public const string ITEM_CURDURA = "CurDura";
        public const string ITEM_DAMAGE = "Damage";
        public const string ITEM_DEFENCE = "Defence";
        public const string ITEM_ATTACKRATING = "AttackRating";
        public const string ITEM_ATTACKSPEED = "AttackSpeed";
        public const string ITEM_ATTACKRANGE = "AttackRange";
        public const string ITEM_INCMAXLIFE = "IncMaxLife";
        public const string ITEM_INCMAXMANA = "IncMaxMana";
        public const string ITEM_LIFEREGEN = "LifeRegen";
        public const string ITEM_MANAREGEN = "ManaRegen";
        public const string ITEM_CRITICAL = "CriticalHit";
        public const string ITEM_PLUS = "Plus";
        public const string ITEM_SLVL = "Slvl";
        public const string ITEM_IMBUETRIES = "ImbueTries";
        public const string ITEM_DRAGONSUCCESSIMBUETRIES = "DragonSuccessImbueTries";
        public const string ITEM_DISCOUNTREPAIRFEE = "DiscountRepairFee";
        public const string ITEM_TOTALDRAGONIMBUES = "TotalDragonImbueTries";
        public const string ITEM_DRAGONDAMAGE = "DragonDmg";
        public const string ITEM_DRAGONDEFENCE = "DragonDef";
        public const string ITEM_DRAGONATTACKRATING = "DragonAR";
        public const string ITEM_DRAGONLIFE = "DragonLife";
        public const string ITEM_MAPPEDSTUFF = "MappedStuff";
        public const string ITEM_FORCENUMBER = "ForceNumber";
        public const string ITEM_REBIRTHHOLE = "RebirthHole";
        public const string ITEM_REBIRTHHOLEITEM = "RebirthHoleItem";
        public const string ITEM_REBIRTHHOLESTAT = "RebirthHoleStat";
        public const string ITEM_TOMAPID = "ToMapID";
        public const string ITEM_IMBUERATE = "ImbueRate";
        public const string ITEM_IMBUEINCREASE = "ImbueIncrease";
        public const string ITEM_BOOKSKILLID = "BookSkillID";
        public const string ITEM_BOOKSKILLLEVEL = "BookSkillLevel";
        public const string ITEM_BOOKSKILLDATA = "BookSkillData";
        public const string ITEM_MAXPOLISHTRIES = "MaxPolishStoneTries";
        public const string ITEM_POLISHTRIES = "PolishStoneTries";
        public const string ITEM_VIGISTAT1 = "VigiStat1";
        public const string ITEM_VIGISTAT2 = "VigiStat2";
        public const string ITEM_VIGISTAT3 = "VigiStat3";
        public const string ITEM_VIGISTAT4 = "VigiStat4";
        public const string ITEM_VIGISTATADD1 = "VigiStatAdd1";
        public const string ITEM_VIGISTATADD2 = "VigiStatAdd2";
        public const string ITEM_VIGISTATADD3 = "VigiStatAdd3";
        public const string ITEM_VIGISTATADD4 = "VigiStatAdd4";
        public const string ITEM_BAG = "Bag";
        public const string ITEM_SLOT = "Slot";
        public const string ITEM_SIZEX = "SizeX";
        public const string ITEM_SIZEY = "SizeY";

        #endregion

        #region Maps Table
        public const string MAP_ID = "ID";
        public const string MAP_NAME = "Name";
        public const string MAP_MULTIPLYVALUE = "MultiplyValue";
        public const string MAP_SPAWNX = "SpawnX";
        public const string MAP_SPAWNY = "SpawnY";
        #endregion

        #region Portals Table
        public const string PORTAL_MAPID = "MapID";
        public const string PORTAL_TOMAPID = "ToMapID";
        public const string PORTAL_WIDTH = "Width";
        public const string PORTAL_HEIGHT = "Height";
        public const string PORTAL_TOX = "ToX";
        public const string PORTAL_TOY = "ToY";
        public const string PORTAL_FROMX = "FromX";
        public const string PORTAL_FROMY = "FromY";
        #endregion

        #region Npcs Table
        public const string NPC_MAPID = "MapID";
        public const string NPC_NPCID = "NpcID";
        public const string NPC_NPCTYPE = "NpcType";
        public const string NPC_POSX = "PosX";
        public const string NPC_POSY = "PosY";
        public const string NPC_DIRECTION = "Direction";
        #endregion
    }
}
