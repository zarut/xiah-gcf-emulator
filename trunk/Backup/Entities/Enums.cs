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


        SendStats = 14850,
        SendSpawnStats = 12564,

        SendMapSomething = 13624,
        SendMapSomething2 = 15907,

        SendGuildList = 15157, 
        SendFriendList = 15190, // byte totalfriends, int friendid, string friendname

        SendSoftSkillList = 12572, // byte totalskills, int skillid, short skill level
        SendHardSkillList = 12573, // byte totalskills, int skillid, short skill level

        SendItemList = 12568, 
        SendMoveItem = 13070,
        SendRemoveItem = 13064,
        SendItemOnGround = 13068,
        SendAddItem = 13066,
        SendPickUpText = 14911,
        SendRemoveMapItem = 13062,

        SendStartMoving = 12812,
        SendKeepMoving = 12814,
        SendStopMoving = 12816,

        SendChatMessage = 14082,
        SendCommand = 13828,

        SendAcceptImbueItem = 13122,
        SendImbueMessage = 13124,
        SendImbuedItem = 13126,

        SendAcceptFriend = 15169,
        SendFriendStatus = 15192,

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

        RecvPlayerID = 14879,

        RecvRequestSkillList = 12569,
        RecvRequestFriendList = 15189,
        RecvRequestGuildList = 15156,

        RecvRequestInventory = 12567,
        RecvMoveItem = 13069,
        RecvDropItem = 13059,
        RecvPickItem = 13057,
        RecvImbueItem = 13121,
        RecvAcceptImbueItem = 13123,

        RecvNpcTrade = 16435,
        RecvBuyItem = 14384,

        RecvPing = 12587,

        RecvSomething = 12589,
        RecvSomething2 = 15363,

        RecvStartMoving = 12811,
        RecvKeepMoving = 12813,
        RecvStopMoving = 12815,

        RecvChatMessage = 14081,
        RecvCommand = 13827,

        RecvAddFriend = 15168,


        #endregion

        #endregion
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
        NoMoney = 2
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
        VigiItemsSpecialSkillDisappered = 58
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
        PetPotion = 16,
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
    }

    public enum bKindStones : byte
    {
        Black = 1,
        White = 2,
        Red = 3,
        Dragon = 9,
        MetallurgicalStone = 11,
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
        HardBook = 1
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
}