using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using System.Data.Common;
using System.Data;

namespace XiahBLL
{
    public class CharacterManager : ManagerBase
    {
        ItemDataManager itemDataManager;

        public CharacterManager(string conString, string providerName)
            : base(conString, providerName)
        {
            itemDataManager = new ItemDataManager(conString, providerName);
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
                c.X = reader.GetInt16(ordinalCHARACTER_X);
                c.Y = reader.GetInt16(ordinalCHARACTER_Y);
                c.Fame = reader.GetInt32(ordinalCHARACTER_FAME);
                c.Money = reader.GetInt32(ordinalCHARACTER_MONEY);
                c.StatPoint = reader.GetInt16(ordinalCHARACTER_STATPOINT);
                c.TrainingPoint = reader.GetInt16(ordinalCHARACTER_TRAININGPOINT);
                c.FiveElementPoint = reader.GetInt16(ordinalCHARACTER_FIVEELEMENTPOINT);
                c.RepulationPoint = reader.GetInt32(ordinalCHARACTER_REPULATIONPOINT);
                c.CurrentExp = reader.GetInt64(ordinalCHARACTER_CURRENTEXP);
                c.CurrentFEExp = reader.GetInt32(ordinalCHARACTER_CURRENTFEEXP);
                c.Rebirth = reader.GetByte(ordinalCHARACTER_REBIRTH);

            }

            reader.Close();
            _db.Close();

            

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
            c.Mirror = mirror.Count() == 0 ? null : mirror.First() as Equipment;

            c.CalculateTotalStats();

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
                c.Mirror = mirror.Count() == 0 ? null : mirror.First() as Equipment;

            }

            return characters.ToArray();
        }
    }
}
