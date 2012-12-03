using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XiahBLL
{
    class DbNames
    {
        public const string UPDATEGUILD = "UpdateGuild";
        public const string UPDATEGUILD_ID = "@guildID";
        public const string UPDATEGUILD_NOTICE = "@notice";
        public const string UPDATEGUILD_FAME = "@fame";
        public const string UPDATEGUILD_DONATIONS = "@donations";

        public const string UPDATEGUILDMEMBER = "UpdateGuildMember";
        public const string UPDATEGUILDMEMBER_CHARACTERID = "@characterId";
        public const string UPDATEGUILDMEMBER_TITLE = "@title";
        public const string UPDATEGUILDMEMBER_RANK = "@rank";

        public const string INSERTGUILDMEMBER = "InsertGuildMember";
        public const string INSERTGUILDMEMBER_GUILDID = "@guildId";
        public const string INSERTGUILDMEMBER_CHARACTERID = "@characterId";
        public const string INSERTGUILDMEMBER_TITLE = "@title";
        public const string INSERTGUILDMEMBER_RANK = "@rank";

        public const string GETGUILDBYCHARACTERID = "GetGuildIDByCharacterID";
        public const string GETGUILDBYCHARACTERID_CHARACTERID = "@characterId";

        public const string GETGUILDMEMBERSBYID = "GetGuildMembersByGuildID";
        public const string GETGUILDMEMBERSBYID_GUILDID = "@guildId";

        public const string CREATEGUILD = "InsertGuild";
        public const string CREATEGUILD_NAME = "@name";
        public const string CREATEGUILD_OWNERID = "@ownerId";
        public const string CREATEGUILD_GUILDID = "@guildId";

        public const string DELETEGUILD = "DeleteGuild";
        public const string DELETEGUILD_ID_PARAMETER = "@GuildID";

        public const string DELETEGUILDMEMBER = "DeleteGuildMember";
        public const string DELETEGUILDMEMBER_ID_PARAMETER = "@MemberID";

        public const string ISGM_STOREDPROC = "IsGM";
        public const string ISGM_ACCOUNTID_PARAMETER = "@accountId";

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

        public const string GETALLITEMSBYBAGID_STOREDPROC = "GetItemsByBagID";
        public const string GETALLITEMSBYBAGID_BAGID_PARAMETER = "@bag";
        public const string GETALLITEMSBYBAGID_CHARACTERID_PARAMETER = "@characterId";

        public const string MAKESHOP_STOREDPROC = "MakeShop";
        public const string MAKESHOP_CHARACTERID_PARAMETER = "@CharID";
        public const string MAKESHOP_SHOPID_PARAMETER = "@ShopID";

        public const string GETSHOPBYSHOPID_STOREDPROC = "GetShopByShopID";
        public const string GETSHOPBYSHOPID_SHOPID_PARAMETER = "@shopId";

        public const string GETSHOPITEMSBYOWNERID_STOREDPROC = "GetShopItemsByOwnerID";
        public const string GETSHOPITEMSBYOWNERID_OWNERID_PARAMETER = "@ownerId";

        public const string INSERTSHOPITEM_STOREDPROC = "InsertShopItem";
        public const string INSERTSHOPITEM_SHOPID_PARAMETER = "@shopId";
        public const string INSERTSHOPITEM_SLOT_PARAMETER = "@slot";
        public const string INSERTSHOPITEM_ITEMID_PARAMETER = "@itemid";
        public const string INSERTSHOPITEM_PRICE_PARAMETER = "@price";

        public const string REMOVESHOPITEMBYITEMID_STOREDPROC = "RemoveShopItemByID";
        public const string REMOVESHOPITEMBYITEMID_ITEMID_PARAMETER = "@itemId";

        public const string UPDATESHOPBYCHARACTERID_STOREDPROC = "UpdateShopByCharacterID";
        public const string UPDATESHOPBYCHARACTERID_CHARACTERID_PARAMETER = "@charId";
        public const string UPDATESHOPBYCHARACTERID_NAME_PARAMETER = "@name";
        public const string UPDATESHOPBYCHARACTERID_DESC_PARAMETER = "@desc";
        public const string UPDATESHOPBYCHARACTERID_TOTALMONEY_PARAMETER = "@totalmoney";

        public const string UPDATESHOPITEM_STOREDPROC = "UpdateShopItemByID";

        public const string GETIMBUEDROPITEM_STOREDPROC = "GetImbueItemDrops";

        public const string GETBOOKDROPITEM_STOREDPROC = "GetBookDropsByMonsterType";
        public const string GETBOOKDROPITEM_MONSTERTYPE = "@monsterType";

        public const string GETMONSTERDROPITEM_STOREDPROC = "GetMonsterDropItems";
        public const string GETMONSTERDROPITEM_MONSTERLEVEL_PARAMETER = "@level";

        public const string GETPILLDROPITEM_STOREDPROC = "GetRebirthPillDrops";
        public const string GETPILLDROPITEM_LEVEL_PARAMETER = "@level";

        public const string GETWAREHOUSUEITEMSBYACCOUNTID_STOREDPROC = "GetWarehouseItemsByAccountId";
        public const string GETWAREHOUSUEITEMSBYACCOUNTID_ACCOUNT_ID_PARAMETER = "@AccountId";

        public const string INSERTITEM_STOREDPROC = "InsertItem";
        public const string INSERTITEM_REFERENCEID_PARAMETER = "@referenceId";
        public const string INSERTITEM_CHARACTERID_PARAMETER = "@characterId";
        public const string INSERTITEM_AMOUNT_PARAMETER = "@amount";
        public const string INSTERITEM_BAG_PARAMETER = "@bag";
        public const string INSERTITEM_SLOT_PARAMETER = "@slot";
        public const string INSERTITEM_ITEMID_PARAMETER = "@newItemId";

        public const string GETITEMBYITEMID_STOREDPROC = "GetItemByItemID";
        public const string GETITEMBYITEMID_ITEMID_PARAMETER = "@itemId";

        public const string GETITEMBYREFID_STOREDPROC = "GetItemByReferenceID";
        public const string GETITEMBYREFID_REFID_PARAMETER = "@refId";

        public const string UPDATEQUICKSLOT_STOREDPROC = "UpdateQuickSlot";
        public const string UPDATEQUICKSLOT_CHARACTERID_PARAMETER = "@dwCharID";
        public const string UPDATEQUICKSLOT_SLOTNUMBER_PARAMETER = "@bSlotNum";
        public const string UPDATEQUICKSLOT_VALUE_PARAMETER = "@dwValue";
        public const string UPDATEQUICKSLOT_RESULT_PARAMETER = "@bResult";

        public const string ADDRELATION_STOREDPROC = "AddRelation";
        public const string ADDRELATION_CHARACTERID_PARAMETER = "@CharID";
        public const string ADDRELATION_TARGETID_PARAMETER = "@TargetID";
        public const string ADDRELATION_TYPE_PARAMETER = "@Type";
        public const string ADDRELATION_NAME_PARAMETER = "@Name";
        public const string ADDRELATION_RETURNVALUE_PARAMETER = "@ReturnValue";

        public const string GETFRIENDSBYCHARACTERID_STOREDPROC = "GetFriendsByCharacterID";
        public const string GETFRIENDSBYCHARACTERID_CHARACTERID_PARAMETER = "@CharID";

        public const string GETCHARACTERQUICKSLOT_STOREDPROC = "GetCharacterQuickSlot";
        public const string GETCHARACTERQUICKSLOT_CHARACTERID_PARAMETER = "@charId";

        public const string LEARNSKILL_STOREDPROC = "LearnSkill";
        public const string LEARNSKILL_CHARACTERID_PARAMETER = "@CharID";
        public const string LEARNSKILL_SKILLID_PARAMETER = "@SkillID";
        public const string LEARNSKILL_SKILLLEVEL_PARAMETER = "@SkillLevel";
        public const string LEARNSKILL_REMAINGTP_PARAMETER = "@RemainTp";

        public const string GETSKILLBYLEVELANDID_STOREDPROC = "GetSkillBySkillIDAndLevel";
        public const string GETSKILLBYLEVELANDID_SKILLID_PARAMETER = "@skillId";
        public const string GETSKILLBYLEVELANDID_SKILLLEVEL_PARAMETER = "@skillLevel";

        public const string GETSKILLBYCHARACTERID_STOREDPROC = "GetAllSkillsByCharacterID";
        public const string GETSKILLBYCHARACTERID_CHARACTERID_PARAMETER = "@characterId";

        public const string GETCHARACTERBYCHARACTERID_STOREDPROC = "GetCharacterByCharacterId";
        public const string GETCHARACTERBYCHARACTERID_CHARACTERID_PARAMETER = "@characterId";

        public const string GETALIVEPETBYCHARACTERID_STOREDPROC = "GetAlivePetByCharacterID";
        public const string GETALIVEPETBYCHARACTERID_CHARACTERID_PARAMETER = "@characterID";

        public const string GETPETBYID_STOREDPROC = "GetPetByID";
        public const string GETPETBYID_PETID_PARAMETER = "@petID";

        public const string GETLASTDIEDPETS_STOREDPROC = "GetLastDiedPetsByCharacterID";
        public const string GETLASTDIEDPETS_OWNERID_PARAMETER = "@characterID";

        public const string GETPETEVOLUTION_STOREDPROC = "GetPetEvolutionByType";
        public const string GETPETEVOLUTION_TYPE_PARAMETER = "@type";

        public const string PETEVOLUTION_LEVEL = "Level";
        public const string PETEVOLUTION_COUNT = "Evolution_Count";
        public const string PETEVOLUTION_TOTYPE = "Change_Type";

        public const string UPDATEPET_STOREDPROC = "UpdatePet";
        public const string UPDATEPET_PETID_PARAMETER = "@petid";
        public const string UPDATEPET_OWNERID_PARAMETER = "@ownerid";
        public const string UPDATEPET_MAPID_PARAMETER = "@mapid";
        public const string UPDATEPET_TYPE_PARAMETER = "@type";
        public const string UPDATEPET_LEVEL_PARAMETER = "@level";
        public const string UPDATEPET_NAME_PARAMETER = "@name";
        public const string UPDATEPET_CURHEALTH_PARAMETER = "@curhealth";
        public const string UPDATEPET_MAXHEALTH_PARAMATER = "@maxhealth";
        public const string UPDATEPET_DAMAGE_PARAMETER = "@damage";
        public const string UPDATEPET_DEFENSE_PARAMETER = "@defense";
        public const string UPDATEPET_ATTACKRATING_PARAMETER = "@attackrating";
        public const string UPDATEPET_CURRENTEXP_PARAMETER = "@currentexperience";
        public const string UPDATEPET_WILDNESS_PARAMATER = "@wildness";
        public const string UPDATEPET_EVOLUTION_PARAMETER = "@evolution";
        public const string UPDATEPET_ISLEGENDARY_PARAMETER = "@islegendary";
        public const string UPDATEPET_ISALIVE_PARAMETER = "@isalive";
        public const string UPDATEPET_ISSEALED_PARAMETER = "@issealed";
        public const string UPDATEPET_DEATHTIME_PARAMETER = "@deathtime";

        public const string UPDATECHARACTER_STOREDPROC = "UpdateCharacterById";
        public const string UPDATECHARACTER_ID_PARAMETER = "@CharID";
        public const string UPDATECHARACTER_LEVEL_PARAMETER = "@level";
        public const string UPDATECHARACTER_MAPID_PARAMETER = "@MapId";
        public const string UPDATECHARACTER_CURHP_PARAMETER = "@curHp";
        public const string UPDATECHARACTER_CURMANA_PARAMETER = "@curMana";
        public const string UPDATECHARACTER_STR_PARAMETER = "@str";
        public const string UPDATECHARACTER_STA_PARAMETER = "@sta";
        public const string UPDATECHARACTER_DEX_PARAMETER = "@dex";
        public const string UPDATECHARACTER_ENE_PARAMETER = "@ene";
        public const string UPDATECHARACTER_POSX_PARAMETER = "@posx";
        public const string UPDATECHARACTER_POSY_PARAMETER = "@posy";
        public const string UPDATECHARACTER_FAME_PARAMETER = "@fame";
        public const string UPDATECHARACTER_MONEY_PARAMETER = "@money";
        public const string UPDATECHARACTER_STATPOINTS_PARAMETER = "@statpoints";
        public const string UPDATECHARACTER_TRAININGPOINTS_PARAMETER = "@trainingpoints";
        public const string UPDATECHARACTER_CUREXPERIENCE_PARAMETER = "@currentexp";
        public const string UPDATECHARACTER_REBIRTH_PARAMETER = "@rebirth";
        public const string UPDATECHARACTER_GAINEDTP_PARAMETER = "@gainedtp";
        public const string UPDATECHARACTER_TPLEVEL_PARAMETER = "@tplevel";

        public const string GETTPEXPERIENCEBYTPLEVEL_STOREDPROC = "GetTpExpByTpLevel";
        public const string GETTPEXPERIENCEBYTPLEVEL_TPLEVEL_PARAMETER = "@tpLevel";
        public const string GETTPEXPERIENCE_EXPERIENCE = "TpExperience";

        public const string GETEXPERIENCETOLEVELBYLEVEL_STOREDPROC = "GetExpToLevelByLevel";
        public const string GETEXPERIENCETOLEVELBYLEVEL_LEVEL_PARAMETER = "@level";

        public const string GETNEGATIVEEXPBYLEVEL_STOREDPROC = "GetNegativeExpByLevel";
        public const string GETNEGATIVEEXPBYLEVEL_LEVEL_PARAMETER = "@level";
        public const string NEGATIVEEXP_RETURNVALUE = "Exp";

        public const string CREATECHARACTER_STOREDPROC = "CreateCharacter";
        public const string CREATECHARACTER_STOREDPROC_NAME_PARAMETER = "@name";
        public const string CREATECHARACTER_STOREDPROC_CLASS_PARAMETER = "@class";
        public const string CREATECHARACTER_STOREDPROC_ACCOUNTID_PARAMETER = "@accountId";
        public const string CREATECHARACTER_STOREDPROC_CHARACTERID_PARAMETER = "@characterId";

        public const string RETURNVALUE_PARAMETER = "@ReturnValue";

        public const string INSERTPET_STOREDPROC = "InsertPet";
        public const string INSERTPET_OWNERID_PARAMETER = "@ownerId";
        public const string INSERTPET_MAPID_PARAMETER = "@mapId";
        public const string INSERTPET_TYPE_PARAMETER = "@type";
        public const string INSERTPET_LEVEL_PARAMETER = "@level";
        public const string INSERTPET_CURHEALTH_PARAMETER = "@curHealth";
        public const string INSERTPET_MAXHEALTH_PARAMETER = "@maxHealth";
        public const string INSERTPET_DAMAGE_PARAMETER = "@damage";
        public const string INSERTPET_DEFENSE_PARAMETER = "@defense";
        public const string INSERTPET_ATTACKRATING_PARAMETER = "@attackrating";
        public const string INSERTPET_ISALIVE_PARAMETER = "@isalive";
        public const string INSERTPET_NAME_PARAMETER = "@name";
        public const string INSERTPET_PETID_OUTPUT = "@returnId";

        public const string GETALLWORLDS_STOREDPROC = "GetAllWorlds";
        public const string GETALLCHANNELS_STOREDPROC = "GetAllChannels";

        public const string GETMAPBYID_STOREDPROC = "GetMapById";
        public const string GETMAPBYID_ID_PARAMETER = "@mapId";

        public const string GETPORTALSBYMAPID_STOREDPROC = "GetPortalsByMapId";
        public const string GETPORTALSBYMAPID_ID_PARAMETER = "@mapId";

        public const string GETNPCSBYMAPID_STOREDPROC = "GetNpcsByMapId";
        public const string GETNPCSBYMAPID_ID_PARAMETER = "@mapId";

        public const string GETMONSTERSBYMAPID_STOREDPROC = "GetMonstersByMapID";
        public const string GETMONSTERSBYMAPID_ID_PARAMETER = "@mapId";

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
        public const string INSERTMAPITEM_DROPPEDBYPLAYERID = "@droppedByCharacterId";

        public const string DELETEMAPITEM_STOREDPROC = "DeleteMapItem";
        public const string DELETEMAPITEM_MAPITEMID_PARAMETER = "@MapItemId";
        public const string DELETEMAPITEM_ITEMID_PARAMETER = "@itemId";

        public const string DELETEITEM_STOREDPROC = "DeleteItem";
        public const string DELETEITEM_ITEMID_PARAMETER = "@ItemId";

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
        public const string UPDATEITEM_REBIRTHHOLESTAT_PARAMETER = "@RebirthHoleStat";
        public const string UPDATEITEM_IMBUERATE_PARAMETER = "@ImbueRate";
        public const string UPDATEITEM_IMBUEINCREASE_PARAMETER = "@ImbueIncrease";
        public const string UPDATEITEM_IMBUEDATA_PARAMETER = "@ImbueData";
        public const string UPDATEITEM_POLISHSTONETRIES_PARAMETER = "@PolishStoneTries";
        public const string UPDATEITEM_VIGISTAT1_PARAMETER = "@VigiStat1";
        public const string UPDATEITEM_VIGISTAT2_PARAMETER = "@VigiStat2";
        public const string UPDATEITEM_VIGISTAT3_PARAMETER = "@VigiStat3";
        public const string UPDATEITEM_VIGISTAT4_PARAMETER = "@VigiStat4";
        public const string UPDATEITEM_VIGISTATADD1_PARAMETER = "@VigiStatAdd1";
        public const string UPDATEITEM_VIGISTATADD2_PARAMETER = "@VigiStatAdd2";
        public const string UPDATEITEM_VIGISTATADD3_PARAMETER = "@VigiStatAdd3";
        public const string UPDATEITEM_VIGISTATADD4_PARAMETER = "@VigiStatAdd4";
        public const string UPDATEITEM_PETID_PARAMETER = "@petid";
        public const string UPDATEITEM_BAG_PARAMETER = "@Bag";
        public const string UPDATEITEM_SLOT_PARAMETER = "@Slot";
        public const string UPDATEITEM_ITEMID_PARAMETER = "@ItemId";

        public const string INSERTUSER_STOREDPROC = "InsertNewAccount";

        public const string INSERTUSER_ACCOUNT_PARAMETER = "@account";
        public const string INSERTUSER_PASSWORD_PARAMETER = "@password";

        public const string GETUSERIDBYUSERNAMEANDPASSWORD_STOREDPROC = "GetUserIdByUsernameAndPassword";

        public const string CHANGECHARACTERNICKNAME_STOREDPROC = "ChangeCharacterNickname";
        public const string CHANGECHARACTERNICKNAME_ACCOUNTID_PARAMETER = "@accountId";
        public const string CHANGECHARACTERNICKNAME_CHARACTERID_PARAMETER = "@characterId";
        public const string CHANGECHARACTERNICKNAME_NEWNICKNAME_PARAMETER = "@newNickname";

        public const string RESETCHARACTERSKILLS_STOREDPROC = "ResetSkills";
        public const string RESETCHARACTERSKILLS_ACCOUNTID_PARAMETER = "@accountId";
        public const string RESETCHARACTERSKILLS_CHARACTERID_PARAMETER = "@CharacterId";

        public const string DELETECHARACTER_STOREDPROC = "RemoveCharacter";
        public const string DELETECHARACTER_ACCOUNTID_PARAMETER = "@accountId";
        public const string DELETECHARACTER_CHARACTERID_PARAMETER = "@CharacterId";

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

        #region Shop Table
        public const string SHOP_SHOPID = "ShopID";
        public const string SHOP_OWNERID = "OwnerID";
        public const string SHOP_NAME = "Name";
        public const string SHOP_DESCRIPTION = "Description";
        public const string SHOP_MONEY = "Money";

        public const string SHOP_PRICE = "ShopPrice";
        public const string SHOP_SLOT = "ShopSlot";
        #endregion

        #region Friend Table
        public const string FRIEND_RELATIONTYPE = "RelationType";
        public const string FRIEND_TARGETID = "TargetID";
        public const string FRIEND_TARGETNAME = "TargetName";
        #endregion

        #region QuickSlot Table
        public const string QUICKSLOT_SLOT1 = "Slot1";
        public const string QUICKSLOT_SLOT2 = "Slot2";
        public const string QUICKSLOT_SLOT3 = "Slot3";
        public const string QUICKSLOT_SLOT4 = "Slot4";
        public const string QUICKSLOT_SLOT5 = "Slot5";
        public const string QUICKSLOT_SLOT6 = "Slot6";
        public const string QUICKSLOT_SLOT7 = "Slot7";
        public const string QUICKSLOT_SLOT8 = "Slot8";
        public const string QUICKSLOT_SLOT9 = "Slot9";
        public const string QUICKSLOT_SLOT10 = "Slot10";
        #endregion

        #region Skill Table
        public const string SKILL_CHARACTERID = "CharacterID";
        public const string SKILL_SKILLID = "SkillID";
        public const string SKILL_SKILLLEVEL = "SkillLevel";
        public const string SKILL_REQUIREDLEVEL = "RequiredLevel";
        public const string SKILL_REQUIREDSTR = "RequiredStrength";
        public const string SKILL_REQUIREDSTA = "RequiredStamina";
        public const string SKILL_REQUIREDDEX = "RequiredDexterity";
        public const string SKILL_REQUIREDENE = "RequiredEnergy";
        public const string SKILL_MANACOST = "ManaCost";
        public const string SKILL_PREDELAY = "PreDelay";
        public const string SKILL_POSTDELAY = "PostDelay";
        public const string SKILL_REQUIREDTRAINING = "RequiredTrainings";
        public const string SKILL_READONLYBOOK = "ReadOnlyBook";
        public const string SKILL_INCDAMAGE = "IncreaseDamage";
        public const string SKILL_INCDAMAGEPERC = "IncreaseDamagePerc";
        public const string SKILL_INCDEFENSE = "IncreaseDefense";
        public const string SKILL_INCDEFENSEPERC = "IncreaseDefensePerc";
        public const string SKILL_INCATTACKRATING = "IncreaseAttackRating";
        public const string SKILL_INCATTACKRATINGPERC = "IncreaseAttackRatingPerc";
        public const string SKILL_INCHPMAX = "IncreaseHpMax";
        public const string SKILL_INCHPCUR = "IncreaseHpCurrent";
        public const string SKILL_INCHPCURPERC = "IncreaseHpCurrentPerc";
        public const string SKILL_RECOVERHP = "RecoverHp";
        public const string SKILL_RECOVERHPPERC = "RecoverHpPerc";
        public const string SKILL_INCMANAMAX = "IncreaseManaMax";
        public const string SKILL_INCMANACUR = "IncreaseManaCurrent";
        public const string SKILL_INCMANACURPERC = "IncreaseManaCurrentPerc";
        public const string SKILL_RECOVERMANA = "RecoverMana";
        public const string SKILL_RECOVERMANAPERC = "RecoverManaPerc";
        public const string SKILL_INCCRITICAL = "IncreaseCritical";
        public const string SKILL_INCCRITICALPERC = "IncreaseCriticalPerc";
        public const string SKILL_INCPKPERC = "IncreasePkPerc";
        public const string SKILL_KEEPUPTIME = "KeepUpTime";
        public const string SKILL_DISTANCE = "Distance";
        public const string SKILL_SUCCESSCHANCE = "SuccessChance";
        public const string SKILL_NETC1 = "nEtc1";
        public const string SKILL_NETC2 = "nEtc2";
        public const string SKILL_NETC3 = "nEtc3";
        #endregion

        #region Level Table
        public const string LEVEL_EXPERIENCETOLEVEL = "Experience";
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
        public const string CHARACTER_GAINEDTRAININGS = "GainedTrainings";
        public const string CHARACTER_TPLEVEL = "TpLevel";
        public const string CHARACTER_FIVEELEMENTPOINT = "FiveElementPoints";
        public const string CHARACTER_REPULATIONPOINT = "RepulationPoints";
        public const string CHARACTER_CURRENTEXP = "CurrentExp";
        public const string CHARACTER_CURRENTFEEXP = "CurrentFEExp";
        public const string CHARACTER_REBIRTH = "Rebirths";
        #endregion

        #region Pet Table
        public const string PET_ID = "ID";
        public const string PET_OWNERID = "OwnerID";
        public const string PET_MAPID = "MapID";
        public const string PET_TYPE = "Type";
        public const string PET_BASETYPE = "BaseType";
        public const string PET_LEVEL = "Level";
        public const string PET_NAME = "Name";
        public const string PET_CURHEALTH = "CurHealth";
        public const string PET_MAXHEALTH = "MaxHealth";
        public const string PET_DAMAGE = "Damage";
        public const string PET_DEFENSE = "Defense";
        public const string PET_ATTACKRATING = "AttackRating";
        public const string PET_CURRENTEXPERIENCE = "CurrentExperience";
        public const string PET_WILDNESS = "Wildness";
        public const string PET_EVOLUTION = "Evolution";
        public const string PET_ISLEGENDARY = "IsLegendary";
        public const string PET_ISALIVE = "IsAlive";
        public const string PET_ISSEALED = "IsSealed";
        #endregion

        #region DropItems Table
        public const string DROPITEM_DURABILITY = "Durability";
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
        public const string ITEM_REBIRTHHOLESTAT = "RebirthHoleStat";
        public const string ITEM_TOMAPID = "ToMapID";
        public const string ITEM_IMBUERATE = "ImbueRate";
        public const string ITEM_IMBUEINCREASE = "ImbueIncrease";
        public const string ITEM_IMBUEDATA = "ImbueData";
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
        public const string ITEM_PETID = "PetID";
        public const string ITEM_DAMAGEABSORB = "DamageAbsorb";
        public const string ITEM_DEFENSEABSORB = "DefenseAbsorb";
        public const string ITEM_ATTACKRATINGABSORB = "AttackRatingAbsorb";
        public const string ITEM_LIFEABSORB = "LifeAbsorb";
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

        #region Monsters Table
        public const string MONSTER_COUNT = "Count";
        public const string MONSTER_TYPE = "Type";
        public const string MONSTER_LEVEL = "Level";
        public const string MONSTER_NAME = "Name";
        public const string MONSTER_HEALTH = "Health";
        public const string MONSTER_DAMAGE = "Damage";
        public const string MONSTER_DEFENSE = "Defense";
        public const string MONSTER_ATTACKRATING = "AttackRating";
        public const string MONSTER_WALKSPEED = "WalkSpeed";
        public const string MONSTER_SIGHTRANGE = "SightRange";
        public const string MONSTER_WANDERRANGE = "WanderRange";
        public const string MONSTER_ATTACKRANGE = "AttackRange";
        public const string MONSTER_EXPERIENCE = "Experience";
        public const string MONSTER_HEALPOINT = "HealPoint";
        public const string MONSTER_REGENERATION = "Regeneration";
        public const string MONSTER_ID = "ReferenceID";
        public const string MONSTER_MAPID = "MapID";
        public const string MONSTER_SPAWNX = "SpawnX";
        public const string MONSTER_SPAWNY = "SpawnY";
        public const string MONSTER_GROUPID = "GroupID";
        public const string MONSTER_GROUPORDER = "GroupOrder";
        public const string MONSTER_LEADERID = "LeaderID";
        public const string MONSTER_HEALTHINC = "HealthIncrease";
        public const string MONSTER_DAMAGEINC = "DamageIncrease";
        public const string MONSTER_DEFENSEINC = "DefenseIncrease";
        public const string MONSTER_ATTACKRATINGINC = "AttackRatingIncrease";
        public const string MONSTER_ATTACKRANGEINC = "AttackRangeIncrease";
        public const string MONSTER_EXPINC = "ExperienceIncrease";
        public const string MONSTER_ISTAMEABLE = "IsTameable";

        #endregion


        #region Guild Tabel

        public const string GUILD_ID = "GuildID";
        public const string GUILD_NAME = "Name";
        public const string GUILD_NOTICE = "Notice";
        public const string GUILD_FAME = "Fame";
        public const string GUILD_DONATIONS = "Donations";

        #endregion

        #region Guild Member Table

        public const string GUILDMEMBER_TITLE = "Title";
        public const string GUILDMEMBER_RANK = "Rank";

        #endregion
    }
}
