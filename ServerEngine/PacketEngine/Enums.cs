using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerEngine.PacketEngine
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

    public enum Bag : int
    {
        Equip = 0,
        Bag1 = 1,
        Bag2 = 2,
        Warehouse = 14, // not sure
    }

    public enum Slot : int
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

    public enum CharacterType : int
    {
        SwordMan = 1,
        Mage = 2,
        Warrior = 3,
        GhostFighter = 4,
    }
}