using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using System.Data.Common;
using System.Data;
using System.Configuration;

namespace XiahBLL
{
    public class CharacterManager : ManagerBase
    {
        ItemDataManager itemDataManager;
        MapManager mapManager;
        PetManager petManager;
        GuildManager guildManager;

        public CharacterManager(string conString, string providerName)
            : base(conString, providerName)
        {
            itemDataManager = new ItemDataManager(conString, providerName);
            mapManager = new MapManager(conString, providerName);
            petManager = new PetManager(conString, providerName);
            guildManager = new GuildManager(conString, providerName);
        }

        public CharacterManager()
            : base(ConfigurationManager.ConnectionStrings["XiahDb"].ConnectionString,
                ConfigurationManager.ConnectionStrings["XiahDb"].ProviderName)
        {
            itemDataManager = new ItemDataManager(ConfigurationManager.ConnectionStrings["XiahDb"].ConnectionString,
                ConfigurationManager.ConnectionStrings["XiahDb"].ProviderName);
            mapManager = new MapManager(ConfigurationManager.ConnectionStrings["XiahDb"].ConnectionString,
                ConfigurationManager.ConnectionStrings["XiahDb"].ProviderName);
            petManager = new PetManager(ConfigurationManager.ConnectionStrings["XiahDb"].ConnectionString,
                ConfigurationManager.ConnectionStrings["XiahDb"].ProviderName);
            guildManager = new GuildManager(ConfigurationManager.ConnectionStrings["XiahDb"].ConnectionString,
              ConfigurationManager.ConnectionStrings["XiahDb"].ProviderName);
        }

        public bool CreateCharacter(string name, byte Class, int accountId, ref int characterId)
        {
            bool createSuccess = false;

            DbParameter nameParamater = _db.CreateParameter(DbNames.CREATECHARACTER_STOREDPROC_NAME_PARAMETER, name);
            DbParameter classParameter = _db.CreateParameter(DbNames.CREATECHARACTER_STOREDPROC_CLASS_PARAMETER, Class);
            DbParameter accountIdParameter = _db.CreateParameter(DbNames.CREATECHARACTER_STOREDPROC_ACCOUNTID_PARAMETER, accountId);
            DbParameter characterIdParameter = _db.CreateParameter(DbNames.CREATECHARACTER_STOREDPROC_CHARACTERID_PARAMETER, DbType.Int32);
            DbParameter returnValueParameter = _db.CreateParameter(DbNames.RETURNVALUE_PARAMETER, DbType.Int32);

            returnValueParameter.DbType = System.Data.DbType.Int32;
            returnValueParameter.Direction = System.Data.ParameterDirection.ReturnValue;

            nameParamater.DbType = System.Data.DbType.String;
            nameParamater.Size = 20;

            classParameter.DbType = DbType.Byte;
            classParameter.Size = 1;

            accountIdParameter.DbType = DbType.Int32;

            characterIdParameter.Direction = System.Data.ParameterDirection.Output;

            _db.Open();

            _db.ExecuteNonQuery(DbNames.CREATECHARACTER_STOREDPROC,
                System.Data.CommandType.StoredProcedure,
                returnValueParameter,
                nameParamater,
                classParameter,
                accountIdParameter,
                characterIdParameter);

            _db.Close();

            int returnValue = (int)returnValueParameter.Value;

            createSuccess = (returnValue == 1);

            if (createSuccess)
                characterId = (int)characterIdParameter.Value;

            return createSuccess;
        }

        public int MakeShop(int characterId)
        {
            int ShopID = -1;
            DbParameter characterIdParameter = _db.CreateParameter(DbNames.MAKESHOP_CHARACTERID_PARAMETER, characterId);
            DbParameter shopIdParameter = _db.CreateParameter(DbNames.MAKESHOP_SHOPID_PARAMETER, DbType.Int32);

            shopIdParameter.Direction = System.Data.ParameterDirection.Output;

            _db.Open();

            _db.ExecuteNonQuery(DbNames.MAKESHOP_STOREDPROC,
                System.Data.CommandType.StoredProcedure,
                characterIdParameter,
                shopIdParameter);

            _db.Close();

            if ((int)shopIdParameter.Value > 0)
                ShopID = (int)shopIdParameter.Value;

            return ShopID;
        }

        public Shop GetShopByID(int shopid)
        {
            DbParameter shopIdParameter = _db.CreateParameter(DbNames.GETSHOPBYSHOPID_SHOPID_PARAMETER, shopid);

            _db.Open();

            DbDataReader reader = _db.ExcecuteReader(DbNames.GETSHOPBYSHOPID_STOREDPROC, CommandType.StoredProcedure, shopIdParameter);

            int ordinalShop_shopid = reader.GetOrdinal(DbNames.SHOP_SHOPID);
            int ordinalShop_ownerid = reader.GetOrdinal(DbNames.SHOP_OWNERID);
            int ordinalShop_name = reader.GetOrdinal(DbNames.SHOP_NAME);
            int ordinalShop_description = reader.GetOrdinal(DbNames.SHOP_DESCRIPTION);
            int ordinalShop_money = reader.GetOrdinal(DbNames.SHOP_MONEY);

            Shop newShop = null;
            int shopId = -1;
            int ownerId = -1;
            int totalMoney = -1;
            string name = null;
            string desc = null;

            while (reader.Read())
            {
                shopId = reader.GetInt32(ordinalShop_shopid);
                ownerId = reader.GetInt32(ordinalShop_ownerid);
                totalMoney = reader.GetInt32(ordinalShop_money);
                name = reader.GetString(ordinalShop_name);
                desc = reader.GetString(ordinalShop_description);
            }

            reader.Close();
            _db.Close();

            // add new list creating thing to load shop items ^_ ^
            List<BaseItem> items = itemDataManager.GetShopItemsByOwnerID(ownerId);

            newShop = new Shop(name, desc, shopid, totalMoney, ownerId, items);

            return newShop;
        }

        public List<Friend> GetFriendsByCharacterId(int characterId)
        {
            DbParameter characterIdParameter = _db.CreateParameter(DbNames.GETFRIENDSBYCHARACTERID_CHARACTERID_PARAMETER, characterId);
            characterIdParameter.DbType = DbType.Int32;

            _db.Open();

            DbDataReader reader = _db.ExcecuteReader(DbNames.GETFRIENDSBYCHARACTERID_STOREDPROC, CommandType.StoredProcedure, characterIdParameter);

            int ordinalFriend_relationtype = reader.GetOrdinal(DbNames.FRIEND_RELATIONTYPE);
            int ordinalFriend_targetid = reader.GetOrdinal(DbNames.FRIEND_TARGETID);
            int ordinalFriend_targetname = reader.GetOrdinal(DbNames.FRIEND_TARGETNAME);

            List<Friend> friends = new List<Friend>();

            while (reader.Read())
            {
                Friend friend = new Friend
                {
                    RelationType = (FriendAddTypes)reader.GetByte(ordinalFriend_relationtype),
                    FriendID = reader.GetInt32(ordinalFriend_targetid),
                    FriendName = reader.GetString(ordinalFriend_targetname),
                    FriendOnline = false // check later somehow with clients in channel server :/
                };

                friends.Add(friend);
            }

            reader.Close();
            _db.Close();

            return friends;
        }

        public bool AddRelation(int CharacterId, FriendAddTypes type, int TargetID, string TargetName)
        {
            bool addSuccess = false;

            DbParameter characterIdParameter = _db.CreateParameter(DbNames.ADDRELATION_CHARACTERID_PARAMETER, CharacterId);
            characterIdParameter.DbType = DbType.Int32;

            DbParameter typeParameter = _db.CreateParameter(DbNames.ADDRELATION_TYPE_PARAMETER, type);
            typeParameter.DbType = DbType.Byte;

            DbParameter targetIdParameter = _db.CreateParameter(DbNames.ADDRELATION_TARGETID_PARAMETER, TargetID);
            targetIdParameter.DbType = DbType.Int32;

            DbParameter targetNameParameter = _db.CreateParameter(DbNames.ADDRELATION_NAME_PARAMETER, TargetName);
            targetIdParameter.DbType = DbType.String;

            DbParameter returnValueParameter = _db.CreateParameter(DbNames.ADDRELATION_RETURNVALUE_PARAMETER, DbType.Int32);
            returnValueParameter.Direction = ParameterDirection.Output;

            _db.Open();

            _db.ExecuteNonQuery(DbNames.ADDRELATION_STOREDPROC,
                System.Data.CommandType.StoredProcedure,
                characterIdParameter,
                typeParameter,
                targetIdParameter,
                targetNameParameter,
                returnValueParameter);

            _db.Close();

            int returnValue = (int)returnValueParameter.Value;

            addSuccess = (returnValue == 0);

            return addSuccess;
        }

        public bool UpdateQuickSlot(int CharacterId, short SlotNumber, int Value)
        {
            bool updateSuccess = false;

            DbParameter characterIdParameter = _db.CreateParameter(DbNames.UPDATEQUICKSLOT_CHARACTERID_PARAMETER, CharacterId);
            characterIdParameter.DbType = DbType.Int32;

            DbParameter slotNumberParameter = _db.CreateParameter(DbNames.UPDATEQUICKSLOT_SLOTNUMBER_PARAMETER, SlotNumber);
            slotNumberParameter.DbType = DbType.Int16;

            DbParameter valueParameter = _db.CreateParameter(DbNames.UPDATEQUICKSLOT_VALUE_PARAMETER, Value);
            valueParameter.DbType = DbType.Int32;

            DbParameter resultParameter = _db.CreateParameter(DbNames.UPDATEQUICKSLOT_RESULT_PARAMETER, DbType.Int32);

            resultParameter.DbType = System.Data.DbType.Int32;
            resultParameter.Direction = System.Data.ParameterDirection.Output;

            _db.Open();

            _db.ExecuteNonQuery(DbNames.UPDATEQUICKSLOT_STOREDPROC,
                System.Data.CommandType.StoredProcedure,
                resultParameter,
                characterIdParameter,
                slotNumberParameter,
                valueParameter);

            _db.Close();

            int returnValue = (int)resultParameter.Value;

            updateSuccess = (returnValue == 0);

            return updateSuccess;
        }

        public QuickSlot[] GetQuickSlotByCharacterId(int characterId)
        {
            DbParameter characterIdParameter = _db.CreateParameter(DbNames.GETCHARACTERQUICKSLOT_CHARACTERID_PARAMETER, characterId);
            characterIdParameter.DbType = DbType.Int32;

            _db.Open();

            DbDataReader reader = _db.ExcecuteReader(DbNames.GETCHARACTERQUICKSLOT_STOREDPROC, CommandType.StoredProcedure, characterIdParameter);

            int ordinalQuickslot_slot1 = reader.GetOrdinal(DbNames.QUICKSLOT_SLOT1);
            int ordinalQuickslot_slot2 = reader.GetOrdinal(DbNames.QUICKSLOT_SLOT2);
            int ordinalQuickslot_slot3 = reader.GetOrdinal(DbNames.QUICKSLOT_SLOT3);
            int ordinalQuickslot_slot4 = reader.GetOrdinal(DbNames.QUICKSLOT_SLOT4);
            int ordinalQuickslot_slot5 = reader.GetOrdinal(DbNames.QUICKSLOT_SLOT5);
            int ordinalQuickslot_slot6 = reader.GetOrdinal(DbNames.QUICKSLOT_SLOT6);
            int ordinalQuickslot_slot7 = reader.GetOrdinal(DbNames.QUICKSLOT_SLOT7);
            int ordinalQuickslot_slot8 = reader.GetOrdinal(DbNames.QUICKSLOT_SLOT8);
            int ordinalQuickslot_slot9 = reader.GetOrdinal(DbNames.QUICKSLOT_SLOT9);
            int ordinalQuickslot_slot10 = reader.GetOrdinal(DbNames.QUICKSLOT_SLOT10);

            QuickSlot[] slots = new QuickSlot[10];

            while (reader.Read())
            {
                slots[0] = new QuickSlot(1, reader.GetInt32(ordinalQuickslot_slot1));
                slots[1] = new QuickSlot(2, reader.GetInt32(ordinalQuickslot_slot2));
                slots[2] = new QuickSlot(3, reader.GetInt32(ordinalQuickslot_slot3));
                slots[3] = new QuickSlot(4, reader.GetInt32(ordinalQuickslot_slot4));
                slots[4] = new QuickSlot(5, reader.GetInt32(ordinalQuickslot_slot5));
                slots[5] = new QuickSlot(6, reader.GetInt32(ordinalQuickslot_slot6));
                slots[6] = new QuickSlot(7, reader.GetInt32(ordinalQuickslot_slot7));
                slots[7] = new QuickSlot(8, reader.GetInt32(ordinalQuickslot_slot8));
                slots[8] = new QuickSlot(9, reader.GetInt32(ordinalQuickslot_slot9));
                slots[9] = new QuickSlot(10, reader.GetInt32(ordinalQuickslot_slot10));
            }

            reader.Close();
            _db.Close();

            return slots;
        }

        public List<BaseSkill> GetAllSkillsByCharacterID(int characterId)
        {
            List<BaseSkill> returnskills = new List<BaseSkill>();
            List<BaseSkill> skills = new List<BaseSkill>();
            DbParameter characterIdParameter = _db.CreateParameter(DbNames.GETSKILLBYCHARACTERID_CHARACTERID_PARAMETER, characterId);
            characterIdParameter.DbType = DbType.Int32;

            _db.Open();

            DbDataReader reader = _db.ExcecuteReader(DbNames.GETSKILLBYCHARACTERID_STOREDPROC, CommandType.StoredProcedure, characterIdParameter);

            int ordinalSkill_skillid = reader.GetOrdinal(DbNames.SKILL_SKILLID);
            int ordinalSkill_skilllevel = reader.GetOrdinal(DbNames.SKILL_SKILLLEVEL);

            while (reader.Read())
            {
                BaseSkill skill = new BaseSkill
                {
                    SkillID = reader.GetInt16(ordinalSkill_skillid),
                    SkillLevel = reader.GetByte(ordinalSkill_skilllevel)
                };

                skills.Add(skill);
            }

            reader.Close();
            _db.Close();

            foreach (BaseSkill s in skills)
            {
                returnskills.Add(GetSkillByLevelAndID(s.SkillID, s.SkillLevel));
            }

            return returnskills;
        }

        public void LearnSkill(int CharacterId, int SkillId, byte SkillLevel, ref short RemainingTp)
        {

            DbParameter characterIdParameter = _db.CreateParameter(DbNames.LEARNSKILL_CHARACTERID_PARAMETER, CharacterId);
            characterIdParameter.DbType = DbType.Int32;

            DbParameter skillIdParameter = _db.CreateParameter(DbNames.LEARNSKILL_SKILLID_PARAMETER, SkillId);
            skillIdParameter.DbType = DbType.Int32;

            DbParameter skillLevelParameter = _db.CreateParameter(DbNames.LEARNSKILL_SKILLLEVEL_PARAMETER, SkillLevel);
            skillLevelParameter.DbType = DbType.Byte;

            DbParameter remaingTpParamtere = _db.CreateParameter(DbNames.LEARNSKILL_REMAINGTP_PARAMETER, DbType.Int16);
            remaingTpParamtere.DbType = DbType.Int16;
            remaingTpParamtere.Direction = ParameterDirection.Output;

            _db.Open();

            _db.ExecuteNonQuery(DbNames.LEARNSKILL_STOREDPROC,
                System.Data.CommandType.StoredProcedure,
                characterIdParameter,
                skillIdParameter,
                skillLevelParameter,
                remaingTpParamtere);

            _db.Close();

            RemainingTp = (short)remaingTpParamtere.Value;
        }

        public BaseSkill GetSkillByLevelAndID(int skillid, int skilllevel)
        {
            DbParameter skillIdParameter = _db.CreateParameter(DbNames.GETSKILLBYLEVELANDID_SKILLID_PARAMETER, skillid);
            skillIdParameter.DbType = DbType.Int32;

            DbParameter skillLevelParameter = _db.CreateParameter(DbNames.GETSKILLBYLEVELANDID_SKILLLEVEL_PARAMETER, skilllevel);
            skillLevelParameter.DbType = DbType.Int32;

            _db.Open();

            DbDataReader reader = _db.ExcecuteReader(DbNames.GETSKILLBYLEVELANDID_STOREDPROC, CommandType.StoredProcedure, skillLevelParameter, skillIdParameter);

            int ordinalSkill_skillid = reader.GetOrdinal(DbNames.SKILL_SKILLID);
            int ordinalSkill_skilllevel = reader.GetOrdinal(DbNames.SKILL_SKILLLEVEL);
            int ordinalSkill_requiredlevel = reader.GetOrdinal(DbNames.SKILL_REQUIREDLEVEL);
            int ordinalSkill_requiredstr = reader.GetOrdinal(DbNames.SKILL_REQUIREDSTR);
            int ordinalSkill_requiredsta = reader.GetOrdinal(DbNames.SKILL_REQUIREDSTA);
            int ordinalSkill_requireddex = reader.GetOrdinal(DbNames.SKILL_REQUIREDDEX);
            int ordinalSkill_requiredene = reader.GetOrdinal(DbNames.SKILL_REQUIREDENE);
            int ordinalSkill_manacost = reader.GetOrdinal(DbNames.SKILL_MANACOST);
            int ordinalSkill_predelay = reader.GetOrdinal(DbNames.SKILL_PREDELAY);
            int ordinalSkill_postdelay = reader.GetOrdinal(DbNames.SKILL_POSTDELAY);
            int ordinalSkill_requiredtraining = reader.GetOrdinal(DbNames.SKILL_REQUIREDTRAINING);
            int ordinalSkill_readonlybook = reader.GetOrdinal(DbNames.SKILL_READONLYBOOK);
            int ordinalSkill_incdamage = reader.GetOrdinal(DbNames.SKILL_INCDAMAGE);
            int ordinalSkill_incdamageperc = reader.GetOrdinal(DbNames.SKILL_INCDAMAGEPERC);
            int ordinalSkill_incdefense = reader.GetOrdinal(DbNames.SKILL_INCDEFENSE);
            int ordinalSkill_incdefenseperc = reader.GetOrdinal(DbNames.SKILL_INCDEFENSEPERC);
            int ordinalSkill_incattackrating = reader.GetOrdinal(DbNames.SKILL_INCATTACKRATING);
            int ordinalSkill_incattackratingperc = reader.GetOrdinal(DbNames.SKILL_INCATTACKRATINGPERC);
            int ordinalSkill_inchpmax = reader.GetOrdinal(DbNames.SKILL_INCHPMAX);
            int ordinalSkill_inchpcur = reader.GetOrdinal(DbNames.SKILL_INCHPCUR);
            int ordinalSkill_inchpcurperc = reader.GetOrdinal(DbNames.SKILL_INCHPCURPERC);
            int ordinalSkill_recoverhp = reader.GetOrdinal(DbNames.SKILL_RECOVERHP);
            int ordinalSkill_recoverhpperc = reader.GetOrdinal(DbNames.SKILL_RECOVERHPPERC);
            int ordinalSkill_incmanamax = reader.GetOrdinal(DbNames.SKILL_INCMANAMAX);
            int ordinalSkill_incmanacur = reader.GetOrdinal(DbNames.SKILL_INCMANACUR);
            int ordinalSkill_incmanacurperc = reader.GetOrdinal(DbNames.SKILL_INCMANACURPERC);
            int ordinalSkill_recovermana = reader.GetOrdinal(DbNames.SKILL_RECOVERMANA);
            int ordinalSkill_recovermanaperc = reader.GetOrdinal(DbNames.SKILL_RECOVERMANAPERC);
            int ordinalSkill_inccritical = reader.GetOrdinal(DbNames.SKILL_INCCRITICAL);
            int ordinalSkill_inccriticalperc = reader.GetOrdinal(DbNames.SKILL_INCCRITICALPERC);
            int ordinalSkill_incpkperc = reader.GetOrdinal(DbNames.SKILL_INCPKPERC);
            int ordinalSkill_keepuptime = reader.GetOrdinal(DbNames.SKILL_KEEPUPTIME);
            int ordinalSkill_distance = reader.GetOrdinal(DbNames.SKILL_DISTANCE);
            int ordinalSkill_successchance = reader.GetOrdinal(DbNames.SKILL_SUCCESSCHANCE);
            int ordinalSkill_netc1 = reader.GetOrdinal(DbNames.SKILL_NETC1);
            int ordinalSkill_netc2 = reader.GetOrdinal(DbNames.SKILL_NETC2);
            int ordinalSkill_netc3 = reader.GetOrdinal(DbNames.SKILL_NETC3);

            BaseSkill skill = null;

            while (reader.Read())
            {
                short id = reader.GetInt16(ordinalSkill_skillid);

                if (id <= 15)
                    skill = new SoftSkill();
                else
                    skill = new HardSkill();

                skill.SkillID = reader.GetInt16(ordinalSkill_skillid);
                skill.SkillLevel = reader.GetByte(ordinalSkill_skilllevel);
                skill.RequiredLevel = reader.GetByte(ordinalSkill_requiredlevel);
                skill.RequiredStrength = reader.GetInt16(ordinalSkill_requiredstr);
                skill.RequiredStamina = reader.GetInt16(ordinalSkill_requiredsta);
                skill.RequiredDexterity = reader.GetInt16(ordinalSkill_requireddex);
                skill.RequiredEnergy = reader.GetInt16(ordinalSkill_requiredene);
                skill.ManaCost = reader.GetInt16(ordinalSkill_manacost);
                skill.PreDelay = reader.GetInt16(ordinalSkill_predelay);
                skill.PostDelay = reader.GetInt16(ordinalSkill_postdelay);
                skill.RequiredTraining = reader.GetInt16(ordinalSkill_requiredtraining);
                skill.ReadOnlyBook = reader.GetByte(ordinalSkill_readonlybook);
                skill.IncDamage = reader.GetInt16(ordinalSkill_incdamage);
                skill.IncDamagePerc = reader.GetInt16(ordinalSkill_incdamageperc);
                skill.IncDefense = reader.GetInt16(ordinalSkill_incdefense);
                skill.IncDefensePerc = reader.GetInt16(ordinalSkill_incdefenseperc);
                skill.IncAttackRating = reader.GetInt16(ordinalSkill_incattackrating);
                skill.IncAttackRatingPerc = reader.GetInt16(ordinalSkill_incattackratingperc);
                skill.IncHpMax = reader.GetInt16(ordinalSkill_inchpmax);
                skill.IncHpCur = reader.GetInt16(ordinalSkill_inchpcur);
                skill.IncHpCurPerc = reader.GetInt16(ordinalSkill_inchpcurperc);
                skill.RecoverHp = reader.GetInt16(ordinalSkill_recoverhp);
                skill.RecoverHpPerc = reader.GetInt16(ordinalSkill_recoverhpperc);
                skill.IncManaMax = reader.GetInt16(ordinalSkill_incmanamax);
                skill.IncManaCur = reader.GetInt16(ordinalSkill_incmanacur);
                skill.IncManaCurPerc = reader.GetInt16(ordinalSkill_incmanacurperc);
                skill.RecoverMana = reader.GetInt16(ordinalSkill_recovermana);
                skill.RecoverManaPerc = reader.GetInt16(ordinalSkill_recovermanaperc);
                skill.IncCritical = reader.GetInt16(ordinalSkill_inccritical);
                skill.IncCriticalPerc = reader.GetInt16(ordinalSkill_inccriticalperc);
                skill.IncPKPerc = reader.GetInt16(ordinalSkill_incpkperc);
                skill.KeepUpTime = reader.GetInt32(ordinalSkill_keepuptime);
                skill.Distance = reader.GetInt16(ordinalSkill_distance);
                skill.SuccessChance = reader.GetInt16(ordinalSkill_successchance);
                skill.nEtc1 = reader.GetInt32(ordinalSkill_netc1);
                skill.nEtc2 = reader.GetInt32(ordinalSkill_netc2);
                skill.nEtc3 = reader.GetInt32(ordinalSkill_netc3);
            }

            reader.Close();
            _db.Close();

            return skill;
        }

        public long GetNegativeExpByLevel(int level)
        {
            DbParameter levelParameter = _db.CreateParameter(DbNames.GETNEGATIVEEXPBYLEVEL_LEVEL_PARAMETER, level);
            levelParameter.DbType = DbType.Int32;

            _db.Open();
            DbDataReader reader = _db.ExcecuteReader(DbNames.GETNEGATIVEEXPBYLEVEL_STOREDPROC, CommandType.StoredProcedure, levelParameter);
            int ordinalLevel_experiencetolevel = reader.GetOrdinal(DbNames.NEGATIVEEXP_RETURNVALUE);

            long experience = 0;

            while (reader.Read())
            {
                experience = reader.GetInt64(ordinalLevel_experiencetolevel);
            }

            reader.Close();
            _db.Close();

            return experience;
        }

        public long GetExperienceToLevelByLevel(int level)
        {
            DbParameter levelParameter = _db.CreateParameter(DbNames.GETEXPERIENCETOLEVELBYLEVEL_LEVEL_PARAMETER, level);
            levelParameter.DbType = DbType.Int32;

            _db.Open();
            DbDataReader reader = _db.ExcecuteReader(DbNames.GETEXPERIENCETOLEVELBYLEVEL_STOREDPROC, CommandType.StoredProcedure, levelParameter);
            int ordinalLevel_experiencetolevel = reader.GetOrdinal(DbNames.LEVEL_EXPERIENCETOLEVEL);

            long experience = -1;

            while (reader.Read())
            {
                experience = reader.GetInt64(ordinalLevel_experiencetolevel);
            }

            reader.Close();
            _db.Close();

            return experience;
        }

        public long GetExperienceToTpByTpLevel(int tpLevel)
        {
            DbParameter levelParameter = _db.CreateParameter(DbNames.GETTPEXPERIENCEBYTPLEVEL_TPLEVEL_PARAMETER, tpLevel);
            levelParameter.DbType = DbType.Int32;

            _db.Open();
            DbDataReader reader = _db.ExcecuteReader(DbNames.GETTPEXPERIENCEBYTPLEVEL_STOREDPROC, CommandType.StoredProcedure, levelParameter);
            int ordinalLevel_experiencetolevel = reader.GetOrdinal(DbNames.GETTPEXPERIENCE_EXPERIENCE);

            long experience = -1;

            while (reader.Read())
            {
                experience = reader.GetInt64(ordinalLevel_experiencetolevel);
            }

            reader.Close();
            _db.Close();

            return experience;
        }

        public void UpdateCharacter(Character ch)
        {
            DbParameter idParameter = _db.CreateParameter(DbNames.UPDATECHARACTER_ID_PARAMETER, ch.CharacterId);
            idParameter.DbType = DbType.Int32;

            DbParameter levelParameter = _db.CreateParameter(DbNames.UPDATECHARACTER_LEVEL_PARAMETER, ch.Level);
            levelParameter.DbType = DbType.Int16;

            DbParameter mapIdParameter = _db.CreateParameter(DbNames.UPDATECHARACTER_MAPID_PARAMETER, ch.MapId);
            mapIdParameter.DbType = DbType.Int32;

            DbParameter curHpParameter = _db.CreateParameter(DbNames.UPDATECHARACTER_CURHP_PARAMETER, ch.CurrentHp);
            curHpParameter.DbType = DbType.Int32;

            DbParameter curManaParameter = _db.CreateParameter(DbNames.UPDATECHARACTER_CURMANA_PARAMETER, ch.CurrentMana);
            curManaParameter.DbType = DbType.Int32;

            DbParameter strParameter = _db.CreateParameter(DbNames.UPDATECHARACTER_STR_PARAMETER, ch.Strength);
            strParameter.DbType = DbType.Int16;

            DbParameter staParameter = _db.CreateParameter(DbNames.UPDATECHARACTER_STA_PARAMETER, ch.Stamina);
            staParameter.DbType = DbType.Int16;

            DbParameter dexParameter = _db.CreateParameter(DbNames.UPDATECHARACTER_DEX_PARAMETER, ch.Dexterity);
            dexParameter.DbType = DbType.Int16;

            DbParameter eneParameter = _db.CreateParameter(DbNames.UPDATECHARACTER_ENE_PARAMETER, ch.Energy);
            eneParameter.DbType = DbType.Int16;

            DbParameter posXParameter = _db.CreateParameter(DbNames.UPDATECHARACTER_POSX_PARAMETER, (short)ch.Map.SpawnX);
            posXParameter.DbType = DbType.Int16;

            DbParameter posYParameter = _db.CreateParameter(DbNames.UPDATECHARACTER_POSY_PARAMETER, (short)ch.Map.SpawnY);
            posYParameter.DbType = DbType.Int16;

            DbParameter fameParameter = _db.CreateParameter(DbNames.UPDATECHARACTER_FAME_PARAMETER, ch.Fame);
            fameParameter.DbType = DbType.Int32;

            DbParameter moneyParameter = _db.CreateParameter(DbNames.UPDATECHARACTER_MONEY_PARAMETER, ch.Money);
            moneyParameter.DbType = DbType.Int32;

            DbParameter statPointParameter = _db.CreateParameter(DbNames.UPDATECHARACTER_STATPOINTS_PARAMETER, ch.StatPoint);
            statPointParameter.DbType = DbType.Int16;

            DbParameter trainingPointParameter = _db.CreateParameter(DbNames.UPDATECHARACTER_TRAININGPOINTS_PARAMETER, ch.TrainingPoint);
            trainingPointParameter.DbType = DbType.Int16;

            DbParameter currentExpParameter = _db.CreateParameter(DbNames.UPDATECHARACTER_CUREXPERIENCE_PARAMETER, ch.CurrentExp);
            currentExpParameter.DbType = DbType.Int64;

            DbParameter rebirthParameter = _db.CreateParameter(DbNames.UPDATECHARACTER_REBIRTH_PARAMETER, ch.Rebirth);
            rebirthParameter.DbType = DbType.Byte;

            DbParameter gainedParameter = _db.CreateParameter(DbNames.UPDATECHARACTER_GAINEDTP_PARAMETER, ch.GainedTrainings);
            gainedParameter.DbType = DbType.Int16;

            DbParameter tpLevelParameter = _db.CreateParameter(DbNames.CHARACTER_TPLEVEL, ch.TpLevel);
            tpLevelParameter.DbType = DbType.Int16;

            _db.Open();

            _db.ExecuteNonQuery(DbNames.UPDATECHARACTER_STOREDPROC,
              System.Data.CommandType.StoredProcedure,
              idParameter,
              levelParameter,
              mapIdParameter,
              curHpParameter,
              curManaParameter,
              strParameter,
              staParameter,
              dexParameter,
              eneParameter,
              posXParameter,
              posYParameter,
              fameParameter,
              moneyParameter,
              statPointParameter,
              trainingPointParameter,
              currentExpParameter,
              rebirthParameter,
              gainedParameter,
              tpLevelParameter);

            _db.Close();
        }

        public Character GetCharacterByCharacterId(int characterId)
        {
            DbParameter characterIdParameter = _db.CreateParameter(DbNames.GETCHARACTERBYCHARACTERID_CHARACTERID_PARAMETER, characterId);
            characterIdParameter.DbType = DbType.Int32;

            _db.Open();

            DbDataReader reader = _db.ExcecuteReader(DbNames.GETCHARACTERBYCHARACTERID_STOREDPROC, CommandType.StoredProcedure, characterIdParameter);

            int ordinalCHARACTER_ID = reader.GetOrdinal(DbNames.CHARACTER_ID);
            int ordinalCHARACTER_ACCOUNTID = reader.GetOrdinal(DbNames.CHARACTER_ACCOUNTID);
            int ordinalCHARACTER_NAME = reader.GetOrdinal(DbNames.CHARACTER_NAME);
            int ordinalCHARACTER_CLASS = reader.GetOrdinal(DbNames.CHARACTER_CLASS);
            int ordinalCHARACTER_LEVEL = reader.GetOrdinal(DbNames.CHARACTER_LEVEL);
            int ordinalCHARACTER_MAPID = reader.GetOrdinal(DbNames.CHARACTER_MAPID);
            int ordinalCHARACTER_CURRENTHP = reader.GetOrdinal(DbNames.CHARACTER_CURRENTHP);
            int ordinalCHARACTER_CURRENTMANA = reader.GetOrdinal(DbNames.CHARACTER_CURRENTMANA);
            int ordinalCHARACTER_STRENGTH = reader.GetOrdinal(DbNames.CHARACTER_STRENGTH);
            int ordinalCHARACTER_STAMINA = reader.GetOrdinal(DbNames.CHARACTER_STAMINA);
            int ordinalCHARACTER_DEXTERITY = reader.GetOrdinal(DbNames.CHARACTER_DEXTERITY);
            int ordinalCHARACTER_ENERGY = reader.GetOrdinal(DbNames.CHARACTER_ENERGY);
            int ordinalCHARACTER_X = reader.GetOrdinal(DbNames.CHARACTER_X);
            int ordinalCHARACTER_Y = reader.GetOrdinal(DbNames.CHARACTER_Y);
            int ordinalCHARACTER_FAME = reader.GetOrdinal(DbNames.CHARACTER_FAME);
            int ordinalCHARACTER_MONEY = reader.GetOrdinal(DbNames.CHARACTER_MONEY);
            int ordinalCHARACTER_STATPOINT = reader.GetOrdinal(DbNames.CHARACTER_STATPOINT);
            int ordinalCHARACTER_TRAININGPOINT = reader.GetOrdinal(DbNames.CHARACTER_TRAININGPOINT);
            int ordinalCHARACTER_GAINEDTRAININGS = reader.GetOrdinal(DbNames.CHARACTER_GAINEDTRAININGS);
            int ordinalCHARACTER_TPLEVEL = reader.GetOrdinal(DbNames.CHARACTER_TPLEVEL);
            int ordinalCHARACTER_FIVEELEMENTPOINT = reader.GetOrdinal(DbNames.CHARACTER_FIVEELEMENTPOINT);
            int ordinalCHARACTER_REPULATIONPOINT = reader.GetOrdinal(DbNames.CHARACTER_REPULATIONPOINT);
            int ordinalCHARACTER_CURRENTEXP = reader.GetOrdinal(DbNames.CHARACTER_CURRENTEXP);
            int ordinalCHARACTER_CURRENTFEEXP = reader.GetOrdinal(DbNames.CHARACTER_CURRENTFEEXP);
            int ordinalCHARACTER_REBIRTH = reader.GetOrdinal(DbNames.CHARACTER_REBIRTH);

            Character c = null;

            while (reader.Read())
            {
                byte ClassType = reader.GetByte(ordinalCHARACTER_CLASS);

                switch (ClassType)
                {
                    case 1:
                        c = new Swordman();
                        break;
                    case 2:
                        c = new Mage();
                        break;
                    case 3:
                        c = new Warrior();
                        break;
                    case 4:
                        c = new GhostFighter();
                        break;
                    default:
                        break;
                }


                c.CharacterId = reader.GetInt32(ordinalCHARACTER_ID);
                c.AccountId = reader.GetInt32(ordinalCHARACTER_ACCOUNTID);
                c.Name = reader.GetString(ordinalCHARACTER_NAME);
                c.Class = ClassType; //ofc nn for that
                c.Level = reader.GetInt16(ordinalCHARACTER_LEVEL);
                c.MapId = reader.GetInt32(ordinalCHARACTER_MAPID);
                c.CurrentHp = reader.GetInt32(ordinalCHARACTER_CURRENTHP);
                c.CurrentMana = reader.GetInt32(ordinalCHARACTER_CURRENTMANA);
                c.Strength = reader.GetInt16(ordinalCHARACTER_STRENGTH);
                c.Stamina = reader.GetInt16(ordinalCHARACTER_STAMINA);
                c.Dexterity = reader.GetInt16(ordinalCHARACTER_DEXTERITY);
                c.Energy = reader.GetInt16(ordinalCHARACTER_ENERGY);
                c.Position = new Position();
                c.Position.X = reader.GetInt16(ordinalCHARACTER_X);
                c.Position.Y = reader.GetInt16(ordinalCHARACTER_Y);
                c.Fame = reader.GetInt32(ordinalCHARACTER_FAME);
                c.Money = reader.GetInt32(ordinalCHARACTER_MONEY);
                c.StatPoint = reader.GetInt16(ordinalCHARACTER_STATPOINT);
                c.TrainingPoint = reader.GetInt16(ordinalCHARACTER_TRAININGPOINT);
                c.GainedTrainings = reader.GetInt16(ordinalCHARACTER_GAINEDTRAININGS);
                c.TpLevel = reader.GetInt16(ordinalCHARACTER_TPLEVEL);
                c.FiveElementPoint = reader.GetInt16(ordinalCHARACTER_FIVEELEMENTPOINT);
                c.RepulationPoint = reader.GetInt32(ordinalCHARACTER_REPULATIONPOINT);
                c.CurrentExp = reader.GetInt64(ordinalCHARACTER_CURRENTEXP);
                c.CurrentFEExp = reader.GetInt32(ordinalCHARACTER_CURRENTFEEXP);
                c.Rebirth = reader.GetByte(ordinalCHARACTER_REBIRTH);

            }

            reader.Close();
            _db.Close();

            int ShopID = MakeShop(c.CharacterId); // should always return an id as it creates new shop if theres none existing..
            c.Shop = GetShopByID(ShopID);
            c.Map = mapManager.GetMapById(c.MapId);
            c.ExpToLevel = GetExperienceToLevelByLevel(c.Level);
            c.NegativeExp = GetNegativeExpByLevel(c.Level);
            c.ExpToTraining = GetExperienceToTpByTpLevel(c.TpLevel);
            c.QuickSlots = GetQuickSlotByCharacterId(c.CharacterId);
            c.SkillList = GetAllSkillsByCharacterID(c.CharacterId);
            c.FriendList = GetFriendsByCharacterId(c.CharacterId);
      
            c.Online = true;
            c.Alive = true;

            var equips = itemDataManager.GetAllItemsInBag(0, c.CharacterId).Cast<Equipment>().ToList();

            //c.Equipments = GetAllItemsInBag(0, c.CharacterId).Cast<Equipment>().ToList();

            var neck = equips.Where(x => x is Necklace);
            var ring = equips.Where(x => x is Ring);
            var head = equips.Where(x => x is Head);
            var body = equips.Where(x => x is Body);
            var feet = equips.Where(x => x is Feet);
            var hand = equips.Where(x => x is Hand);
            var cape = equips.Where(x => x is Cape);
            var charm = equips.Where(x => x.Slot == (byte)Slot.CrystalMirror);
            var mirror = equips.Where(x => x.Slot == (byte)Slot.Mirror);

            c.Necklace = neck.Count() == 0 ? null : neck.First() as Necklace;
            c.Ring = ring.Count() == 0 ? null : ring.First() as Ring;
            c.Head = head.Count() == 0 ? null : head.First() as Head;
            c.Body = body.Count() == 0 ? null : body.First() as Body;
            c.Feet = feet.Count() == 0 ? null : feet.First() as Feet;
            c.Hand = hand.Count() == 0 ? null : hand.First() as Hand;
            c.Cape = cape.Count() == 0 ? null : cape.First() as Cape;
            c.Charm = charm.Count() == 0 ? null : charm.First() as Equipment;
            c.Mirror = mirror.Count() == 0 ? null : mirror.First() as Mirror;

            c.CalculateTotalStats();

            c.Pet = petManager.GetAlivePetByCharacterID(c.CharacterId);
            if (c.Pet != null && !c.Pet.IsSealed)
            {
                c.Pet.Position = new Position();
                c.Pet.Position.X = c.Position.X;
                c.Pet.Position.Y = c.Position.Y;
            }

            c.Guild = guildManager.GetGuildByCharacterID(c.CharacterId);

            return c;
        }

        public Character[] GetAllCharactersByAccountId(int accountId)
        {
            DbParameter accountIdParameter = _db.CreateParameter(DbNames.GETALLCHARACTERSBYACCOUNTID_ACCOUNTID_PARAMETER, accountId);


            accountIdParameter.DbType = DbType.Int32;

            List<Character> characters = new List<Character>();

            _db.Open();

            DbDataReader reader = _db.ExcecuteReader(DbNames.GETALLCHARACTERSBYACCOUNTID_STOREDPROC, CommandType.StoredProcedure, accountIdParameter);

            int ordinalCHARACTER_ID = reader.GetOrdinal(DbNames.CHARACTER_ID);
            int ordinalCHARACTER_ACCOUNTID = reader.GetOrdinal(DbNames.CHARACTER_ACCOUNTID);
            int ordinalCHARACTER_NAME = reader.GetOrdinal(DbNames.CHARACTER_NAME);
            int ordinalCHARACTER_CLASS = reader.GetOrdinal(DbNames.CHARACTER_CLASS);
            int ordinalCHARACTER_LEVEL = reader.GetOrdinal(DbNames.CHARACTER_LEVEL);
            int ordinalCHARACTER_MAPID = reader.GetOrdinal(DbNames.CHARACTER_MAPID);
            int ordinalCHARACTER_CURRENTHP = reader.GetOrdinal(DbNames.CHARACTER_CURRENTHP);
            int ordinalCHARACTER_CURRENTMANA = reader.GetOrdinal(DbNames.CHARACTER_CURRENTMANA);
            int ordinalCHARACTER_STRENGTH = reader.GetOrdinal(DbNames.CHARACTER_STRENGTH);
            int ordinalCHARACTER_STAMINA = reader.GetOrdinal(DbNames.CHARACTER_STAMINA);
            int ordinalCHARACTER_DEXTERITY = reader.GetOrdinal(DbNames.CHARACTER_DEXTERITY);
            int ordinalCHARACTER_ENERGY = reader.GetOrdinal(DbNames.CHARACTER_ENERGY);

            while (reader.Read())
            {
                Character c = new Character
                {
                    CharacterId = reader.GetInt32(ordinalCHARACTER_ID),
                    AccountId = reader.GetInt32(ordinalCHARACTER_ACCOUNTID),
                    Name = reader.GetString(ordinalCHARACTER_NAME),
                    Class = reader.GetByte(ordinalCHARACTER_CLASS),
                    Level = reader.GetInt16(ordinalCHARACTER_LEVEL),
                    MapId = reader.GetInt32(ordinalCHARACTER_MAPID),
                    CurrentHp = reader.GetInt32(ordinalCHARACTER_CURRENTHP),
                    CurrentMana = reader.GetInt32(ordinalCHARACTER_CURRENTMANA),
                    Strength = reader.GetInt16(ordinalCHARACTER_STRENGTH),
                    Stamina = reader.GetInt16(ordinalCHARACTER_STAMINA),
                    Dexterity = reader.GetInt16(ordinalCHARACTER_DEXTERITY),
                    Energy = reader.GetInt16(ordinalCHARACTER_ENERGY)
                };

                characters.Add(c);
            }

            reader.Close();
            _db.Close();

            foreach (Character c in characters)
            {
                //c.Equipments = GetAllItemsInBag(0, c.CharacterId).Cast<Equipment>().ToList();

                var equips = itemDataManager.GetAllItemsInBag(0, c.CharacterId).Cast<Equipment>().ToList();

                var neck = equips.Where(x => x is Necklace);
                var ring = equips.Where(x => x is Ring);
                var head = equips.Where(x => x is Head);
                var body = equips.Where(x => x is Body);
                var feet = equips.Where(x => x is Feet);
                var hand = equips.Where(x => x is Hand);
                var cape = equips.Where(x => x is Cape);
                var charm = equips.Where(x => x.Slot == (byte)Slot.CrystalMirror);
                var mirror = equips.Where(x => x.Slot == (byte)Slot.Mirror);

                c.Necklace = neck.Count() == 0 ? null : neck.First() as Necklace;
                c.Ring = ring.Count() == 0 ? null : ring.First() as Ring;
                c.Head = head.Count() == 0 ? null : head.First() as Head;
                c.Body = body.Count() == 0 ? null : body.First() as Body;
                c.Feet = feet.Count() == 0 ? null : feet.First() as Feet;
                c.Hand = hand.Count() == 0 ? null : hand.First() as Hand;
                c.Cape = cape.Count() == 0 ? null : cape.First() as Cape;
                c.Charm = charm.Count() == 0 ? null : charm.First() as Equipment;
                c.Mirror = mirror.Count() == 0 ? null : mirror.First() as Mirror;

            }

            return characters.ToArray();
        }

        public int DeleteCharacter(int accountId, int characterId)
        {
            DbParameter returnValueParameter = _db.CreateParameter(DbNames.RETURNVALUE_PARAMETER, DbType.Int32);
            DbParameter accountIdParameter = _db.CreateParameter(DbNames.DELETECHARACTER_ACCOUNTID_PARAMETER, DbType.Int32);
            DbParameter characterIdParameter = _db.CreateParameter(DbNames.DELETECHARACTER_CHARACTERID_PARAMETER, DbType.Int32);

            returnValueParameter.Direction = ParameterDirection.ReturnValue;

            accountIdParameter.Value = accountId;
            characterIdParameter.Value = characterId;

            _db.Open();
            _db.ExecuteNonQuery(DbNames.DELETECHARACTER_STOREDPROC, CommandType.StoredProcedure, returnValueParameter, accountIdParameter, characterIdParameter);
            _db.Close();

            int returnValue = (int)returnValueParameter.Value;

            return returnValue;
        }

        public int ResetCharacterSkills(int accountId, int characterId)
        {
            DbParameter returnValueParameter = _db.CreateParameter(DbNames.RETURNVALUE_PARAMETER, DbType.Int32);
            DbParameter accountIdParameter = _db.CreateParameter(DbNames.RESETCHARACTERSKILLS_ACCOUNTID_PARAMETER, DbType.Int32);
            DbParameter characterIdParameter = _db.CreateParameter(DbNames.RESETCHARACTERSKILLS_CHARACTERID_PARAMETER, DbType.Int32);

            returnValueParameter.Direction = ParameterDirection.ReturnValue;

            accountIdParameter.Value = accountId;
            characterIdParameter.Value = characterId;

            _db.Open();
            _db.ExecuteNonQuery(DbNames.RESETCHARACTERSKILLS_STOREDPROC, CommandType.StoredProcedure, returnValueParameter, accountIdParameter, characterIdParameter);
            _db.Close();

            int returnValue = (int)returnValueParameter.Value;

            return returnValue;
        }

        public int ChangeCharacterNickname(int accountId, int characterId, string newCharName)
        {
            DbParameter returnValueParameter = _db.CreateParameter(DbNames.RETURNVALUE_PARAMETER, DbType.Int32);
            DbParameter accountIdParameter = _db.CreateParameter(DbNames.CHANGECHARACTERNICKNAME_ACCOUNTID_PARAMETER, DbType.Int32);
            DbParameter characterIdParameter = _db.CreateParameter(DbNames.CHANGECHARACTERNICKNAME_CHARACTERID_PARAMETER, DbType.Int32);
            DbParameter newNicknameParameter = _db.CreateParameter(DbNames.CHANGECHARACTERNICKNAME_NEWNICKNAME_PARAMETER, DbType.String);

            returnValueParameter.Direction = ParameterDirection.ReturnValue;

            accountIdParameter.Value = accountId;
            characterIdParameter.Value = characterId;
            newNicknameParameter.Value = newCharName;

            _db.Open();
            _db.ExecuteNonQuery(DbNames.CHANGECHARACTERNICKNAME_STOREDPROC, CommandType.StoredProcedure, returnValueParameter, accountIdParameter, characterIdParameter, newNicknameParameter);
            _db.Close();

            int returnValue = (int)returnValueParameter.Value;

            return returnValue;
        }
    }
}
