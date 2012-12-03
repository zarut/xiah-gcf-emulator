using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    public enum PacketIds
    {
        #region Send

        #region All Servers
        SendKey = 0,
        #endregion

        #region Login Server
        SendWorldList = 12548,
        SendWorldIP = 12616,
        SendLoginState = 12546,
        #endregion

        #region World Server

        SendUnitLoginState = 12552,
        SendCharacterList = 12554,
        SendCreateCharacter = 12556,
        SendChannelList = 12614, 
        SendChannelStatus = 12550,

        #endregion

        #region Channel Server

        SendRebirth = 13168,

        SendChannelChange = 12804,
        SendRemoveCharacter = 12810,

        SendMapId = 12802,
        SendPortal = 12808,

        SendMap = 12600,
        SendMapTime = 13313,

        SendSpawn = 12806,
        SendSpawnPlayer = 12560,
        SendSpawnPlayers = 12562,
        SendPlayerIdsSomething = 13618,

        SendNpcSpawn = 16434,
        SendNpcItemList = 16436,
        SendBuyItemState = 14385,

        SendMoneyLeft = 14867,
        SendSellState = 14425,
        SendItemsLeft = 13078,
        SendHealMana = 14861,

        SendStats = 14850,
        SendSpawnStats = 12564,

        SendMapSomething = 13624,
        SendMapSomething2 = 15907,

        SendTradePlayer = 14482,
        SendTradePlayerError = 14490,
        SendTradePlayerAccept = 14484,
        SendTradeAddMoney = 14506,
        SendTradeAddItem = 14486,
        SendTradeAcceptFinished = 14492,

        SendGuildCreate = 15130,
        SendGuildJoin = 15149,
        SendGuildChangeMemberRank = 15153,
        SendGuildChangeMemberTitle = 15165,
        SendGuildOwner = 15167,
        SendGuild = 15157,
        SendGuildMemberInfo = 15159,
        SendGuildNotice = 15161,
        SendGuildStats = 15163,
        SendGuildChangeNoticeError = 15198,
        SendGuildTown = 15961,
        SendJoinGuildAnswer = 15147,
        SendCloseGuild = 15132,
        SendLeaveGuild = 15151,

        SendSoftSkillList = 12572, // byte totalskills, int skillid, short skill level
        SendHardSkillList = 12573, // byte totalskills, int skillid, short skill level
        SendLearnSkill = 13604,
        SendPlayerActiveSkills = 13616,

        SendUpdateQuickSlot = 12594,
        SendQuickSlotList = 12590,

        SendQuestList = 17156,

        SendRemoveWarehouseItem = 14498,
        SendWarehouseError = 14500,
        SendWarehouseMoveItemError = 14504,
        SendWarehouseItemList = 14494,
        SendAddItemToWarehouse = 14496,

        SendPlayerShop = 17409,
        SendChangeShopInfo = 13840,
        SendChangeShopInfoError = 17411,
        SendAddItemToShopError = 17415,
        SendAddItemToShop = 17426,
        SendRemoveShopItem = 17427,
        SendMoveShopItemToBag = 17417,
        SendShopTimeLeft = 17428,
        SendShopStartSelling = 17419,
        SendOpenOtherPlayerShop = 17423,
        SendBuyOtherPlayerShopItemError = 17425,
        SendShopTaxAmount = 17421,

        SendItemList = 12568, 
        SendMoveItem = 13070,
        SendRemoveItem = 13064,
        SendItemOnGround = 13068,
        SendAddItem = 13066,
        SendPickUpText = 14911,
        SendRemoveMapItem = 13062,

        SendInsertItemToQuickSlot = 12594,
        SendSelectedSkill = 13592,

        SendStartMoving = 12812,
        SendKeepMoving = 12814,
        SendStopMoving = 12816,

        SendChatMessage = 14082,
        SendCommand = 13828,

        SendAcceptImbueItem = 13122,
        SendImbueMessage = 13124,
        SendImbuedItem = 13126,

        SendCombineMessage = 13152,

        SendAddFriend = 15169,
        SendFriendStatus = 15192,
        SendFriendList = 15190, // byte totalfriends, int friendid, string friendname
        SendFriendOnlinestatus = 15196,

        SendMonsterStartMoving = 16392,
        SendMonsterKeepMoving = 16394,
        SendMonsterStopMoving = 16396,

        SendSpawnMonster = 16428,
        SendMonsterDie = 16400,
        SendPetHealth = 16454,
        SendSummonPet = 16480,

        SendPetOnLogin = 14880,
        SendPetExperienceGained = 16452,
        SendPetEvolved = 16462,
        SendPetLeveled = 16450,

        SendUnSealPet = 16458,
        SendPetStats = 16446,
        SendPetOtherPlayerSpawn2 = 16440,
        SendPetOtherPlayerSpawn = 16444,
        SendPetOtherPlayerHasPet = 16386,
        SendRemovePet = 16390,
        SendSealPet = 16456,

        SendPetBaitTaken = 16414,
        SendPetTamed = 16402,
        SendLastDiedPets = 16481,
        SendResurrectPet = 14880,
        SendResurrectPet2 = 16483,
        SendPetRenamed = 16448,

        SendTradePetError = 16485,
        SendTradePetOffer = 16486,
        SendTradePetOfferError = 16488,
        SendTradedPet = 16483,

        SendExpEffectPacket = 13620,
        SendExperience = 14864,

        SendAttack1 = 13572,
        SendAttack2 = 13574,
        
        SendCast1 = 13588,
        SendCast2 = 13590,

        SendPlayerDie = 13570,
        SendPlayerDie1 = 13842,

        SendPartyAnswer = 14919,
        SendAddToPartyAnswer = 14923,
        SendPartyLeader = 14921,
        SendAddPartyMember = 14925,
        SendParty = 14935,
        SendLeaveParty = 14927,
        SendKillParty = 14929,
        SendChangePartyLeader = 14937,

        #endregion

        #endregion


        #region Recieve

        #region Login Server

        Recv_GameLogin = 12545, // this should have the login id like total login amount
        Recv_ConnectWorld = 12615,

        #endregion

        #region World Server

        RecvUnitLogin = 12551,
        RecvCreateCharacter = 12555,
        RecvChannelRequest = 12613,

        #endregion

        #region Channel Server

        RecvChannelChange = 12803,
        RecvChannelSomething = 12553,

        RecvChannelLogin = 12801,
        RecvLoadMap = 12599,
        RecvWalkToPortal = 12807,

        RecvStatRequest = 12563, // send spawn stats only 
        RecvSpawnRequest = 12805, // send, 02 34 01 00 00, spawn10, 32 35, 

        RecvSpawnPlayer = 12559, 
        RecvRequestStats = 12597,
        RecvAddStat = 13597,

        RecvPlayerPetReques = 14879,

        RecvTradePlayer = 14481,
        RecvTradePlayerCommands = 14489,
        RecvTradeAddMoney = 14505,
        RecvTradeAddItem = 14485,

        RecvLearnSkill = 13603,
        RecvRequestSkillList = 12569,
        RecvRequestFriendList = 15189,

        RecvCreateGuild = 15129,
        RecvRequestGuildList = 15156,
        RecvRequestGuildInformation = 15162,
        RecvRequestGuildMemberStats = 15158,
        RecvRequestChangeGuildMemberRank = 15152,
        RecvRequestChangeGuildMemberTitle = 15164,
        RecvRequestChangeGuildNotice = 15197,
        RecvRequestJoinGuild = 15146,
        RecvRequestCloseGuild = 15131,
        RecvRequestLeaveGuild = 15150,
        RecvGuildChat = 15160,

        RecvMoveWarehouseItem = 14503,
        RecvRequestOpenWarehouse = 14493,
        RecvAddItemToWarehouse = 14499,
        RecvMoveWarehouseItemToBag = 14501,

        RecvMoveShopItemToBag = 17416,
        RecvChangeShopInfo = 17410,
        RecvAddItemToShop = 17414,
        RecvMoveShopItem = 17412,
        RecvShopStartSelling = 17418,
        RecvOpenOtherPlayerShop = 17422,
        RecvBuyOtherPlayerShopItem = 17424,
        RecvShopTakeMoney = 17420,

        RecvRequestInventory = 12567,
        RecvMoveItem = 13069,
        RecvDropItem = 13059,
        RecvPickItem = 13057,
        RecvStackItem = 13073,
        RecvImbueItem = 13121,
        RecvAcceptImbueItem = 13123,

        RecvNpcTrade = 16435,
        RecvBuyItem = 14384,
        RecvSellItem = 14424,
        RecvUseItem = 13081,
        RecvInsertItemToQuickSlot = 12593,

        RecvUseRebirthPill = 13167,

        RecvPing = 12587,

        RecvQuickSlotRequest = 12589,
        RecvUpdateQuickSlot = 12593,
        RecvSelectSkill = 13591,

        RecvSomething2 = 15363,

        RecvStartMoving = 12811,
        RecvKeepMoving = 12813,
        RecvStopMoving = 12815,

        RecvPetStartMoving = 16391,
        RecvPetKeepMoving = 16393,
        RecvPetStopMoving = 16395,
        RecvKillPet = 16399,

        RecvBaitPet = 13083,
        RecvRequestPetStats = 16445,
        RecvRequestSpawnOtherPet = 16439,
        RecvResurrectPet = 16482,
        RecvRenamePet = 16447,
        RecvSendPetToMe = 16385,
        RecvTradePet = 16484,
        RecvTradePetAmount = 16487,
        RecvSealPet = 16455,
        RecvUnSealPet = 16457,
        RecvPetRequestItems = 16477,

        RecvUseTeleporter = 12826,
        RecvRequestDeath = 13841,

        RecvChatMessage = 14081,
        RecvCommand = 13827,

        RecvAddFriend = 15168,

        RecvMonsterSpawnRequest = 16427,

        RecvAttack1 = 13571,
        RecvAttack2 = 13573,

        RecvCast1 = 13587,
        RecvCast2 = 13589,

        RecvCombineItems = 13151,

        RecvPartyRequest = 14918,
        RecvAddToPartyRequest = 14922,
        RecvLeaveParty = 14926,
        RecvChangePartyLeader = 14936,

        RecvQuestListRequest = 17155,

        #endregion

        #endregion
    }

    public enum AttackType
    {
        Miss = 1,
        Hit = 2
    }

    public enum MonsterAIState
    {
        Wander,
        Chase,
        Attack
    }

    public enum MonsterMoveStatus
    {
        StartMoving,
        KeepMoving,
        StopMoving
    }

    public enum PartyType
    {
        Normal = 0,
        Relation = 1
    }

    public enum PartyError
    {
        None = 0,
        Accept = 1,
        LevelDifference = 4,
        NotPartyMaster = 5,
        CannotCreate = 6,
        MaxMembers = 7,
        AlreadyInParty = 8,
        Cancel = 9
    }

    public enum PetTradeAction
    {
        AmountWanted = 0,
        Ok = 1,
        Accept = 2,
        Cancel = 9
    }

    public enum PetTradeOfferError
    {
        Finished = 0,
        Cancel = 9
    }

    public enum PetTamedError
    {
        None = 0,
        FailedToTame = 4
    }

    public enum FriendAddAnswers : byte
    {
        CannotTeacherStudentDifClass = 2,
        Accepted = 10,
        Request = 11,
        Accept = 12,
        Cancel = 13
    }

    public enum FriendAddTypes : byte
    {
        Lover = 10,
        TeacherStudent = 20,
        Friend = 40
    }

    public enum GuildOrder : int
    {
        Guild_Master = 1,
        Second_Master = 2,
        Elder = 3,
        Manager = 4,
        Party_Master = 5,
        Member = 6
    }

    public enum GuildNoticeError : int
    {
        Success = 0,
        TooLowReputation = 1,
        OnlyGuildMasterCanUse = 2,
        TooManyLetters = 3
    }

    public enum GuildMemberError : byte
    {
        Success = 0,
        NoGuild = 1,
        NoRights = 2,
        NoMember = 3,
        ImproperWord = 4
    }

    public enum GuildJoinAnswer : byte
    {
        Join = 1,
        Accept = 2,
        Decline = 3,
        AlreadyInGuild = 4,
        NotInGuild = 5,
    }

    public enum QuestState : byte
    {
        Stopped = 1,
        Started = 2,
        Removed = 3,
        Completed = 4
    }

    public enum QuestObjectState : byte
    {
        Obtained = 1,
        NotObtained = 2,
        Hidden = 3
    }

    public enum QuestObjectType : byte
    {
        Hunt = 10,
        Tame = 12,
        Win_PVP = 15,
        Conversation = 16,
        Level_Up = 18,
        Obtain_Skill = 30,
        Join_Party = 31,
        Join_Guild = 32,
        Possesion = 70,
        Imbue = 73,
        Obtain_Money = 74,
        Repair_Item = 75,
        Wear = 76,
        Gamble = 78,
    }

    public enum QuestRewardType : byte
    {
        Fame = 42,
        Item = 70,
        PlusItem = 73,
        Money = 74
    }

    public enum TradePlayerCommands : byte
    {
        Accept = 0,
        Cancel = 9
    }

    public enum TradePlayerError : byte
    {
        None = 0,
        Accept = 1,
        Cancel = 9
    }

    public enum BuyOtherPlayerShopItemError : byte
    {
        Success = 0,
        CannotFindSeller = 1,
        ItemInfoHasBeenChanged = 2,
        NoSpaceToBuyItem = 4,
        NoMoney = 5,
        PriceChanged = 6
    }

    public enum AddItemToShopError : byte
    {
        Success = 0,
        ItemDoesNotExist = 2,
        CannotRegisterAnymoreBecauseYourStoreBagIsFull = 3,
        ItemCannotBeSold = 4,
    }

    public enum WarehouseError : byte
    {
        NotEnoughtMoney = 1,
        CannotInsert,
        YourStashIsFull,
        NotEnoughtSpace
    }

    public enum SkillLearnError : byte
    {
        NeedMoreDex = 1,
        NeedMoreStr,
        NeedMoreSta,
        NeedMoreEne,
        TooLowLevel,
        AlreadyTrained,
        NeedMartialArts,
        WrongClass,
        NeedMoreTps,
        NotEnoughtTps,
        YouCanOnlyHaveOneDragonAtk = 15,
        YouCanOnlyHaveOneRisingAtk,
        YouMustTrainFirstBookFirst,
    }

    public enum RemoveCharacterEffect : byte
    {
        None = 0,
        Bead = 1
    }

    public enum LoginState : int
    {
        Success = 0,
        Failure = 1,
        WrongPassword = 5,
        NotaGamePopAccount = 7,
        Blocked = 8,
        IncorrectInput = 11,
        IncorrectGameVersion = 12,
        OtherConnecting = 15,
        NotActived = 16,
    }

    public enum CharCreationState : int
    {
        Success = 0,
        NameInUse = 2,
        NameTooLong = 5,
        ImproperWord = 6,
        MaxAmountReached = 9,

    }

    public enum ImbueState : byte
    {
        Success = 0,
        ThisItemCannotBeImbued = 3
    }

    public enum BuyItemState : byte
    {
        Success = 0,
        NoItem = 1,
        NoMoney = 2,
        FailedToBuyItem = 3,
        BagFull = 6,
    }

    public enum ImbueError : byte
    {
        Success = 0,
        NotEnoughtMoney = 1,
        NoSuchItem = 2,
        YouDontHaveItemsToImbue = 3,
        FailedToRemake = 5,
        UnableToImbueAnymore = 6,
        ImbueImpossible = 7,
        ThisItemCannotBeExtracted = 11,
        YourBagIsFull = 12,
        ExtractedItemName = 13,
        FailedToExtract = 14,
        CantOpenForceByThisItem = 15,
        CantOpenForceAnymore = 16,
        NoForce = 17,
        NoEmpthyForce = 18,
        CantRemoveDueNoFiveElements = 19,
        FailedToOpenForce = 20,
        SucceedToRemakeUnusualItem = 21,
        AlreadyUnusualItem = 22,
        UnavailableItemToBeRemodeled = 23,
        FailedToRemakeUnusualItem = 24,
        ThisItemCannotBeImbuedd = 25,
        CanUseOnlyOneRefineStone = 26,
        ThereAreNotImperialSecretJewelAttributes = 27,
        ItIsAbleToBeRecovered = 30,
        ThisItemStaticsShouldNotHaveDecreased = 31,
        ThisItemAlreadyUsedSaltStone = 32,
        UseTheEffectsOfSaltStone = 33,
        CannotUseToOverPlus16Item = 34,
        IncorrectItemLevel = 35,
        CannotUsingDragonResources = 36,
        InconsistentWithDragnImbueSystem = 37,
        GainDragonEffectSuccessfully = 38,
        GainDragonEffectFailed = 39,
        AlreadyGainedAllOfDragonEffects = 40,
        CannotResetDragonEffects = 43,
        ResetDragonEffectsSuccessfully = 45,
        CannotUseOverPlus21Item = 47,
        Increased50TimesOfTotalImbueTries = 48,
        TotalImbueTimesLimitCanNotExceed251Times = 49,
        TooManyImbueResources = 50,
        ThisItemCanNotAddSpecialSkillAnymore = 52,
        FailedToAddSpecialSkills = 53,
        FourofItemsSpecialSkillAdded = 54,
        ThereIsNoItemsSpecialSkill = 55,
        ItemsAllSpecialSkillDisappered = 56,
        VigiItemsSpecialSkillIsNotEnought = 57,
        VigiItemsSpecialSkillDisappered = 58,
        RbHoleSuccess = 255,
    }

    public enum ImbueStat : byte
    {
        Damage = 0,
        Defense = 1,
        AttackRating = 2,
        CriticalHit = 3,
        MaxLife = 4,
        MaxMana = 5,
        LifeReg = 6,
        ManaReg = 7,
        RequiredLevel = 8,
        RequiredStrength = 9,
        RequiredDexterity = 10,
        RequiredStamina = 11,
        RequiredEnergy = 12,
        MaxDurability = 13,
        MovingSpeed = 14,
        DiscountRepairFee = 15,
        DefenseAndDamage = 20,
        None = 255
    }

    public enum bType : byte
    {
        Weapon = 1,
        Clothes = 2,
        Hat = 3,
        Shoes = 4,
        Cape = 5,

        Ring = 6,
        Necklace = 7,

        Mirror = 9,
        PetBoots = 11,
        PetHat = 12,
        PetTail = 14,
        PetSack = 15,
        PetItem = 16,
        PetArmor = 17,

        Pocket = 18,
        Materials = 19,
        ForceItems = 20,

        Book = 21,
        Bead = 22,
        Potion = 23,
        Jeon = 24,

        ImbueItem = 25,
        QuestItem = 26,
        LuckyTag = 27,

        Pill = 31,
        StoreTag = 32,
    }

    public enum SoftSkills : int
    {
        Attack_Focus = 1,
        Defense_Focus,
        Accuracy_Focus,
        Life_Increase,
        Mana_Increase,
        Fast_Healing,
        Fast_Mana_Refill,
        Sword_Mastery,
        Blade_Mastery,
        Fan_Mastery,
        Brush_Mastery,
        Claw_Mastery,
        Axe_Mastery,
        Talon_Mastery,
        Tonfa_Mastery
    }

    public enum HardSkills : int
    {
        Energy_Circulation = 30,
        Chi_Focus_Attack,
        Critical_Hit,
        Energy_Bomb,
        Wind_Walk,
        Sword_Missle,
        Rage,
        Reckless_Attack,
        Multi_Shot,
        Iron_Body,
        Sunder,
        Shadow_Strike,

        Chi_Circulation = 60,
        Charm_Monster,
        Attack_Song,
        Heal,
        Cloud_Walk,
        Power_Up_Pet,
        Mass_Heal,
        Death_Energy,
        Empathy,
        War_Cry,
        Life_Drain,
        Summon_Monster,

        Life_Circulation = 90,
        Multiple_Attack,
        Howl,
        Fist_Wind,
        Fast_Step,
        Reduce_Accuracy,
        Darkness,
        Crushing_Hands,
        Stun,
        Frenzy,
        Entangle,
        Reflection,

        Recuperation = 120,
        Poison_Hand,
        Poison_Sting,
        Poison_Shot,
        Fast_Movement,
        Chi_Poisoning,
        Life_Poisoning,
        Haze,
        Enrich,
        Poison_Shield,
        Dead_Man,
        Poison_Power_Up,
    }

    public enum AbsorbSkills : int
    {
        Ironbody_Focus = 191,
        Shadow_Focus,
        Warcry_Focus,
        Monster_Empathy_Focus,
        Entangle_Focus,
        Decrease_Accuracy_Focus,
        Poison_Power_Up_Focus,
        Life_Poisoning_Focus,
        Absorption_Focus1,
        Absorption_Focus2,
    }

    public enum RebirthSkills : int
    {
        Fire_Dragon = 150,
        Sea_Dragon, // 151
        Wood_Heaven, // 152
        Magic_Steel_Ghost, // 153
        Soil_Lighting_Hit, // 154
        Autumn_Wind, // 155
        Heaven_Earth, // 156 
        Butterfly_Hit, // 157
        Five_Elements_special_skill1, // 158
        Five_Elements_special_skill2, // 159
        Five_Elements_special_skill3, // 160
        Efficiency_Force, // 161 
        Weapon_Mastery, // 162 
        Clothes_Mastery, // 163
        Fire_Dragon_Attack, // 164
        Ice_Dragon_Attack, // 165
        Poison_Dragon_Attack, // 166
        Flash_Dragon_Attack, // 167
        Rising_Fire_Dragon, // 168
        Rising_Ice_Dragon, // 169
        Rising_Poison_Dragon, // 170
        Rising_Flash_Dragon, // 171
        Once_Attack, // 172
        Duplicate_Attack, // 173 
        Extend_Range, // 174
        Strengthen_Monster, // 175
        Focused_Force_Attack, // 176
        Absorb_Force, // 177
        Hiding, // 178
        Strong_Defense, // 179
    }

    public enum EntityType : byte
    {
        Player = 1,
        Npc,
        Monster,
        Pet
    }

    public enum bKindStones : byte
    {
        Black = 1,
        White = 2,
        Red = 3,
        Dragon = 9,
        MetallurgicalStone = 11,
        RbItem = 17,
        RepairJewel = 7,
    }

    public enum bKindPotions : byte
    {
        Normal = 0,
        Elixir = 1
    }

    public enum bKindBooks : byte
    {
        SoftBook = 0,
        HardBook = 1,
        FeBook = 2,
        FeSkillBook = 3,
        RebirdBook = 4,
        FourthBook = 5,
        FocusBook = 7
    }

    public enum bKindWeapons : byte
    {
        Sword = 0,
        Blade = 1,
        Fan = 2,
        Brush = 3,
        Axe = 4,
        Claw = 5,
        Talon = 6,
        Tonfa = 7,
        Hammer = 8
    }

    public enum bKindArmors : byte
    {
        SwordMan = 0,
        Mage = 1,
        Warrior = 2,
        GhostFighter = 3
    }
    public enum bKindBeads : byte
    {
        Normal = 0,
    }

    public enum bKindHats : byte
    {
        SwordMan = 0,
        Mage = 1,
        Warrior = 2,
        GhostFighter = 3
    }

    public enum bKindShoes : byte
    {
        Swordman = 0,
        Mage = 1,
        Warrior = 2,
        GhostFighter = 3
    }

    public enum bKindPetItems : byte
    {
        Taming = 0,
        Food = 1,
        Potion = 2,
        Resurect = 3,
        MorphingPill = 4,
        TransformingPill = 6
    }

    public enum bKindPills : byte
    {
        Rebirth = 2,
    }

    public enum Slot : byte
    {
        Weapon = 0,
        Hat = 1,
        Armor = 2,
        Shoes = 3,
        CrystalMirror = 4,
        Ring = 5,
        Necklace = 6,
        Cape = 7,
        Mirror = 8,
    }

    public enum Stat : int
    {
        Dexterity = 1,
        Strength = 2,
        Stamina = 3,
        Energy = 4,
    }

    public enum ChatType : byte
    {
        General = 0,
        Whisper = 1,
        Notice = 2,
        Party = 5,
        Guild = 6,
        Pipe = 10,
        Pipe2 = 11,
        Pipe3 = 12,
        Congurations = 14
    }

    public enum CharacterType : int
    {
        SwordMan = 1,
        Mage = 2,
        Warrior = 3,
        GhostFighter = 4,
    }

    public enum MapName : int
    {
        Bizzare_Rock = 1,
        Volcano, // 2
        Glacier, // 3
        Desert, // 4
        Swamp, // 5
        Prairie, // 6
        Alpine, // 7
        IsNotWorked = 9,
        Guild_Area, // 10
        Poison_Magic_Cave, // 11
        Blood_Devil_Castle, // 12
        The_Bright_War_Area, // 13
        The_Emperor_War_Area, // 14
        Accursed_Ruins, // 15
        Fiend_Basin // 16
    }

    public enum RebirthHoleItems : short
    {
        Damage = 100,
        Defense = 200,
        AttackRating = 300,
        MaxLife = 400,
        MaxMana = 500,
        LifeReg = 600,
        ManaReg = 700,
        Critical = 800,
        Durability = 900,
        LevelReq = 1000,
        BlueGhost = 1100,
        RedGhost = 1101,
        WhiteGhost = 1102,
        BlackGhost = 1103,
        BlackImbueTier1 = 1200,
        BlackImbueTier2 = 1201,
        BlackImbueTier3 = 1202,
        DragonImbueTier1 = 1300,
        DragonImbueTier2 = 1301,
        DragonImbueTier3 = 1302,
    }

    public enum RebirthError : byte
    {
        Success = 0,
        LowLevel = 1,
        LowRebirth = 2,
    }

    public enum ImperialSet
    {
        None,
        IncreaseAttack, //  Mystic Apricot Pendant - Sakura Pendant - Bright moon Lu - Tomb Grass Lu
        IncreaseDefence, // Deep Aroma Pendant - White Lotus Pendant - Fallen Snow Lu - Missing Crow Lu
        IncreaseAccuracy, //  Sakura Pendent - Bright Moon Lu - Tomb Grass Lu - Golden Silkworm Thread
        IncreaseLife, // White Lotus Pendent - Fallen Snow Lu - Missing Crow Lu - Silver Silkworm Thread
        IncreaseWeaponDamage, // Bright Moon Lu - Tomb Grass Lu - Golden Silkworm Thread - Blue Ghost Card
        IncreaseClothesDefence, // Fallen Snow Lu - Missing Crow Lu - Silver Silkworm Thread - Red Ghost Card
        IncreaseHatAccuracy, // Mystic Apricot Pendent - Tomb Grass Lu - Golden Silkworm Thread - Blue Ghost Card
        IncreaseFEBonus, // Deep Aroma Pendent - White Lotus Pendant - Silver Silkworm Thread - Red Ghost Card
        IncreaseSealBonus, // Mystic Apricot Pendent - Sakura Pendent - Bright Moon Lu - Blue Ghost Card.
        IncreasePetExperience, // Deep Aroma Pendent - White Lotus Pendent - Fallen Snow Lu - Red Ghost Card.
        IncreaseDmgDefAr, // Blue Ghost Card - Red Ghost Card - White Ghost Card - Black Ghost Card.
        IncreaseExperience1, // Deep Aroma Pendent - Mission Crow Lu - Silver Silkworm Thread - Read Ghost Card 
        IncreaseExperience2, // Golden Silkworm Thread - Blue Ghost Card - White Ghost Card - White Ghost Card
        IncreaseExperience3, // White Ghost Card - White Ghost Card - Black Ghost Card - Black Ghost Card
        IncreaseFExperience1, // Mystic Apricot Pendent - Sakura Pendent - Golden Silkworm Thread - Blue Ghost Card.
        IncreaseFExperience2, // Golden Silkworm Thread - Red Ghost Card - Black Ghost Card - Black Ghost Card.
        IncreasePvpDamage, // Blue Ghost Card - White Ghost Card - White Ghost Card - Black Ghost Card
        DecreasePvpDamage, // Red Ghost Card - White Ghost Card - Black Ghost Card - Black Ghost Card
    }
}