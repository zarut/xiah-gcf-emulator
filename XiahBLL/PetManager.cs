using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using System.Data.Common;
using System.Data;

namespace XiahBLL
{
    public class PetManager : ManagerBase
    {
        public PetManager(string conString, string providerName)
            : base(conString, providerName)
        {
        }

        public int InsertPet(Pet pet, bool IsAlive)
        {
            DbParameter ownerIdParameter = _db.CreateParameter(DbNames.INSERTPET_OWNERID_PARAMETER, pet.OwnerID);
            DbParameter mapIdParameter = _db.CreateParameter(DbNames.INSERTPET_MAPID_PARAMETER, pet.MapID);
            DbParameter typeParameter = _db.CreateParameter(DbNames.INSERTPET_TYPE_PARAMETER, pet.PetType);
            DbParameter levelParameter = _db.CreateParameter(DbNames.INSERTPET_LEVEL_PARAMETER, pet.Level);
            DbParameter curHealthParameter = _db.CreateParameter(DbNames.INSERTPET_CURHEALTH_PARAMETER, pet.CurHealth);
            DbParameter maxHealthParameter = _db.CreateParameter(DbNames.INSERTPET_MAXHEALTH_PARAMETER, pet.MaxHealth);
            DbParameter damageParameter = _db.CreateParameter(DbNames.INSERTPET_DAMAGE_PARAMETER, pet.Damage);
            DbParameter defenseParameter = _db.CreateParameter(DbNames.INSERTPET_DEFENSE_PARAMETER, pet.Defense);
            DbParameter attackratingParameter = _db.CreateParameter(DbNames.INSERTPET_ATTACKRATING_PARAMETER, pet.AttackRating);
            DbParameter petIdParameter = _db.CreateParameter(DbNames.INSERTPET_PETID_OUTPUT, System.Data.DbType.Int32);
            DbParameter isaliveParameter = _db.CreateParameter(DbNames.INSERTPET_ISALIVE_PARAMETER, IsAlive);
            DbParameter nameParameter = _db.CreateParameter(DbNames.INSERTPET_NAME_PARAMETER, pet.Name);

            mapIdParameter.DbType = System.Data.DbType.Int32;
            ownerIdParameter.DbType = System.Data.DbType.Int32;
            typeParameter.DbType = System.Data.DbType.Byte;
            levelParameter.DbType = System.Data.DbType.Int16;
            curHealthParameter.DbType = System.Data.DbType.Int32;
            maxHealthParameter.DbType = System.Data.DbType.Int32;
            damageParameter.DbType = System.Data.DbType.Int32;
            defenseParameter.DbType = System.Data.DbType.Int32;
            attackratingParameter.DbType = System.Data.DbType.Int32;
            isaliveParameter.DbType = System.Data.DbType.Boolean;
            nameParameter.DbType = DbType.String;
            petIdParameter.Direction = System.Data.ParameterDirection.Output;
            pet.Alive = IsAlive;

            _db.Open();

            _db.ExecuteNonQuery(DbNames.INSERTPET_STOREDPROC, System.Data.CommandType.StoredProcedure,
                ownerIdParameter, mapIdParameter, typeParameter, levelParameter,nameParameter, curHealthParameter, maxHealthParameter, damageParameter, defenseParameter, attackratingParameter, isaliveParameter, petIdParameter);

            _db.Close();

            int mapItemId = (int)petIdParameter.Value;

            return mapItemId;
        }

        public Pet GetAlivePetByCharacterID(int characterID)
        {
            DbParameter characterIdParameter = _db.CreateParameter(DbNames.GETALIVEPETBYCHARACTERID_CHARACTERID_PARAMETER, characterID);
            characterIdParameter.DbType = DbType.Int32;

            _db.Open();

            DbDataReader reader = _db.ExcecuteReader(DbNames.GETALIVEPETBYCHARACTERID_STOREDPROC, CommandType.StoredProcedure, characterIdParameter);

            int ordinalPET_ID = reader.GetOrdinal(DbNames.PET_ID);
            int ordinalPET_OWNERID = reader.GetOrdinal(DbNames.PET_OWNERID);
            int ordinalPET_MAPID = reader.GetOrdinal(DbNames.PET_MAPID);
            int ordinalPET_TYPE = reader.GetOrdinal(DbNames.PET_TYPE);
            int ordinalPET_BASETYPE = reader.GetOrdinal(DbNames.PET_BASETYPE);
            int ordinalPET_LEVEL = reader.GetOrdinal(DbNames.PET_LEVEL);
            int ordinalPET_NAME = reader.GetOrdinal(DbNames.PET_NAME);
            int ordinalPET_CURHEALTH = reader.GetOrdinal(DbNames.PET_CURHEALTH);
            int ordinalPET_MAXHEALTH = reader.GetOrdinal(DbNames.PET_MAXHEALTH);
            int ordinalPET_DAMAGE = reader.GetOrdinal(DbNames.PET_DAMAGE);
            int ordinalPET_DEFENSE = reader.GetOrdinal(DbNames.PET_DEFENSE);
            int ordinalPET_ATTACKRATING = reader.GetOrdinal(DbNames.PET_ATTACKRATING);
            int ordinalPET_EXPERIENCE = reader.GetOrdinal(DbNames.PET_CURRENTEXPERIENCE);
            int ordinalPET_WILDNESS = reader.GetOrdinal(DbNames.PET_WILDNESS);
            int ordinalPET_EVOLUTION = reader.GetOrdinal(DbNames.PET_EVOLUTION);
            int ordinalPET_ISLEGENDARY = reader.GetOrdinal(DbNames.PET_ISLEGENDARY);
            int ordinalPET_ISALIVE = reader.GetOrdinal(DbNames.PET_ISALIVE);
            int ordinalPET_ISSEALED = reader.GetOrdinal(DbNames.PET_ISSEALED);

            Pet p = null;

            while (reader.Read())
            {
                p = new Pet();
                p.PetID = reader.GetInt32(ordinalPET_ID);
                p.OwnerID = reader.GetInt32(ordinalPET_OWNERID);
                p.MapID = reader.GetInt32(ordinalPET_MAPID);
                p.PetType = reader.GetByte(ordinalPET_TYPE);
                p.PetBaseType = reader.GetByte(ordinalPET_BASETYPE);
                p.Level = reader.GetInt16(ordinalPET_LEVEL);
                p.Name = reader.GetString(ordinalPET_NAME);
                p.CurHealth = reader.GetInt32(ordinalPET_CURHEALTH);
                p.MaxHealth = reader.GetInt32(ordinalPET_MAXHEALTH);
                p.Damage = reader.GetInt32(ordinalPET_DAMAGE);
                p.Defense = reader.GetInt32(ordinalPET_DEFENSE);
                p.AttackRating = reader.GetInt32(ordinalPET_ATTACKRATING);
                p.CurrentExperience = reader.GetInt64(ordinalPET_EXPERIENCE);
                p.Wildness = reader.GetByte(ordinalPET_WILDNESS);
                p.Evolution = reader.GetByte(ordinalPET_EVOLUTION);
                p.IsLegendary = reader.GetBoolean(ordinalPET_ISLEGENDARY);
                p.Alive = reader.GetBoolean(ordinalPET_ISALIVE);
                p.IsSealed = reader.GetBoolean(ordinalPET_ISSEALED);
            }

            reader.Close();
            _db.Close();

            if (p != null)
            {
                p.ExperienceToLevel = GetExperienceToLevelByLevel(p.Level);
                p.NegativeExperience = GetNegativeExpByLevel(p.Level);
                p.EvolutionTable = GetPetEvolutionByType(p.PetBaseType);
            }

            return p;
        }

        public Pet GetPetByID(int petID)
        {
            DbParameter petIdParameter = _db.CreateParameter(DbNames.GETPETBYID_PETID_PARAMETER, petID);
            petIdParameter.DbType = DbType.Int32;

            _db.Open();

            DbDataReader reader = _db.ExcecuteReader(DbNames.GETPETBYID_STOREDPROC, CommandType.StoredProcedure, petIdParameter);

            int ordinalPET_ID = reader.GetOrdinal(DbNames.PET_ID);
            int ordinalPET_OWNERID = reader.GetOrdinal(DbNames.PET_OWNERID);
            int ordinalPET_MAPID = reader.GetOrdinal(DbNames.PET_MAPID);
            int ordinalPET_TYPE = reader.GetOrdinal(DbNames.PET_TYPE);
            int ordinalPET_BASETYPE = reader.GetOrdinal(DbNames.PET_BASETYPE);
            int ordinalPET_LEVEL = reader.GetOrdinal(DbNames.PET_LEVEL);
            int ordinalPET_NAME = reader.GetOrdinal(DbNames.PET_NAME);
            int ordinalPET_CURHEALTH = reader.GetOrdinal(DbNames.PET_CURHEALTH);
            int ordinalPET_MAXHEALTH = reader.GetOrdinal(DbNames.PET_MAXHEALTH);
            int ordinalPET_DAMAGE = reader.GetOrdinal(DbNames.PET_DAMAGE);
            int ordinalPET_DEFENSE = reader.GetOrdinal(DbNames.PET_DEFENSE);
            int ordinalPET_ATTACKRATING = reader.GetOrdinal(DbNames.PET_ATTACKRATING);
            int ordinalPET_EXPERIENCE = reader.GetOrdinal(DbNames.PET_CURRENTEXPERIENCE);
            int ordinalPET_WILDNESS = reader.GetOrdinal(DbNames.PET_WILDNESS);
            int ordinalPET_EVOLUTION = reader.GetOrdinal(DbNames.PET_EVOLUTION);
            int ordinalPET_ISLEGENDARY = reader.GetOrdinal(DbNames.PET_ISLEGENDARY);
            int ordinalPET_ISALIVE = reader.GetOrdinal(DbNames.PET_ISALIVE);
            int ordinalPET_ISSEALED = reader.GetOrdinal(DbNames.PET_ISSEALED);

            Pet p = null;

            while (reader.Read())
            {
                p = new Pet();
                p.PetID = reader.GetInt32(ordinalPET_ID);
                p.OwnerID = reader.GetInt32(ordinalPET_OWNERID);
                p.MapID = reader.GetInt32(ordinalPET_MAPID);
                p.PetType = reader.GetByte(ordinalPET_TYPE);
                p.PetBaseType = reader.GetByte(ordinalPET_BASETYPE);
                p.Level = reader.GetInt16(ordinalPET_LEVEL);
                p.Name = reader.GetString(ordinalPET_NAME);
                p.CurHealth = reader.GetInt32(ordinalPET_CURHEALTH);
                p.MaxHealth = reader.GetInt32(ordinalPET_MAXHEALTH);
                p.Damage = reader.GetInt32(ordinalPET_DAMAGE);
                p.Defense = reader.GetInt32(ordinalPET_DEFENSE);
                p.AttackRating = reader.GetInt32(ordinalPET_ATTACKRATING);
                p.CurrentExperience = reader.GetInt64(ordinalPET_EXPERIENCE);
                p.Wildness = reader.GetByte(ordinalPET_WILDNESS);
                p.Evolution = reader.GetByte(ordinalPET_EVOLUTION);
                p.IsLegendary = reader.GetBoolean(ordinalPET_ISLEGENDARY);
                p.Alive = reader.GetBoolean(ordinalPET_ISALIVE);
                p.IsSealed = reader.GetBoolean(ordinalPET_ISSEALED);
            }

            reader.Close();
            _db.Close();

            if (p != null)
            {
                p.ExperienceToLevel = GetExperienceToLevelByLevel(p.Level);
                p.NegativeExperience = GetNegativeExpByLevel(p.Level);
                p.EvolutionTable = GetPetEvolutionByType(p.PetBaseType);
            }

            return p;
        }

        public void UpdatePet(Pet pet)
        {
            DbParameter petIDParameter = _db.CreateParameter(DbNames.UPDATEPET_PETID_PARAMETER, pet.PetID);
            petIDParameter.DbType = DbType.Int32;
            DbParameter petOwnerIDParameter = _db.CreateParameter(DbNames.UPDATEPET_OWNERID_PARAMETER, pet.OwnerID);
            petOwnerIDParameter.DbType = DbType.Int32;
            DbParameter petMapIDParameter = _db.CreateParameter(DbNames.UPDATEPET_MAPID_PARAMETER, pet.MapID);
            petMapIDParameter.DbType = DbType.Int32;
            DbParameter petTypeParameter = _db.CreateParameter(DbNames.UPDATEPET_TYPE_PARAMETER, pet.PetType);
            petTypeParameter.DbType = DbType.Byte;
            DbParameter petLevelParamater = _db.CreateParameter(DbNames.UPDATEPET_LEVEL_PARAMETER, pet.Level);
            petLevelParamater.DbType = DbType.Int16;
            DbParameter petNameParameter = _db.CreateParameter(DbNames.UPDATEPET_NAME_PARAMETER, pet.Name);
            petNameParameter.DbType = DbType.String;
            DbParameter petCurHealthParameter = _db.CreateParameter(DbNames.UPDATEPET_CURHEALTH_PARAMETER, pet.CurHealth);
            petCurHealthParameter.DbType = DbType.Int32;
            DbParameter petMaxHealthParameter = _db.CreateParameter(DbNames.UPDATEPET_MAXHEALTH_PARAMATER, pet.MaxHealth);
            petMaxHealthParameter.DbType = DbType.Int32;
            DbParameter petDamageParameter = _db.CreateParameter(DbNames.UPDATEPET_DAMAGE_PARAMETER, pet.Damage);
            petDamageParameter.DbType = DbType.Int32;
            DbParameter petDefenseParameter = _db.CreateParameter(DbNames.UPDATEPET_DEFENSE_PARAMETER, pet.Defense);
            petDefenseParameter.DbType = DbType.Int32;
            DbParameter petAttackRatingParameter = _db.CreateParameter(DbNames.UPDATEPET_ATTACKRATING_PARAMETER, pet.AttackRating);
            petAttackRatingParameter.DbType = DbType.Int32;
            DbParameter petCurExpParameter = _db.CreateParameter(DbNames.UPDATEPET_CURRENTEXP_PARAMETER, pet.CurrentExperience);
            petCurExpParameter.DbType = DbType.Int64;
            DbParameter petWildnessParameter = _db.CreateParameter(DbNames.UPDATEPET_WILDNESS_PARAMATER, pet.Wildness);
            petWildnessParameter.DbType = DbType.Byte;
            DbParameter petEvolutionParameter = _db.CreateParameter(DbNames.UPDATEPET_EVOLUTION_PARAMETER, pet.Evolution);
            petEvolutionParameter.DbType = DbType.Byte;
            DbParameter petIsLegendaryParameter = _db.CreateParameter(DbNames.UPDATEPET_ISLEGENDARY_PARAMETER, pet.IsLegendary);
            petIsLegendaryParameter.DbType = DbType.Boolean;
            DbParameter petIsAliveParameter = _db.CreateParameter(DbNames.UPDATEPET_ISALIVE_PARAMETER, pet.Alive); // once monsters attack pets this can be done
            petIsAliveParameter.DbType = DbType.Boolean;                                                      // or release could work aswell
            DbParameter petIsSealedParameter = _db.CreateParameter(DbNames.UPDATEPET_ISSEALED_PARAMETER, pet.IsSealed); // once mirrors is done this will be used
            petIsSealedParameter.DbType = DbType.Boolean;
            DbParameter deathTimeParameter = _db.CreateParameter(DbNames.UPDATEPET_DEATHTIME_PARAMETER, DateTime.Now);
            deathTimeParameter.DbType = DbType.DateTime;

            _db.Open();

            _db.ExecuteNonQuery(DbNames.UPDATEPET_STOREDPROC,
              System.Data.CommandType.StoredProcedure,
              petIDParameter,
              petOwnerIDParameter,
              petMapIDParameter,
              petTypeParameter,
              petLevelParamater,
              petNameParameter,
              petCurHealthParameter,
              petMaxHealthParameter,
              petDamageParameter,
              petDefenseParameter,
              petAttackRatingParameter,
              petCurExpParameter,
              petWildnessParameter,
              petEvolutionParameter,
              petIsLegendaryParameter,
              petIsAliveParameter,
              petIsSealedParameter,
              deathTimeParameter);

            _db.Close();
        }

        public Pet TamePet(Monster m, Character ch)
        {
            Pet pet = new Pet
            {
                OwnerID = ch.CharacterId,
                MapID = ch.MapId,
                Position = m.Position.NormalizedCopy(),
                PetType = m.MonsterType,
                Name = m.Name,
                Level = m.Level,
                CurHealth = m.MaxHealth,
                MaxHealth = m.MaxHealth,
                Damage = m.Damage,
                Defense = m.Defense,
                AttackRating = m.AttackRating,
            };
            pet.PetID = InsertPet(pet, true);
            pet.ExperienceToLevel = GetExperienceToLevelByLevel(pet.Level);
            pet.NegativeExperience = GetNegativeExpByLevel(pet.Level);
            pet.CurrentExperience = pet.NegativeExperience;
            pet.EvolutionTable = GetPetEvolutionByType(pet.PetType);
            pet.Alive = true;
            UpdatePet(pet);
            return pet;
        }

        public List<Pet> GetLastDiedPets(int OwnerID)
        {
            DbParameter ownerIDParameter = _db.CreateParameter(DbNames.GETLASTDIEDPETS_OWNERID_PARAMETER, OwnerID);
            ownerIDParameter.DbType = DbType.Int32;

            _db.Open();

            DbDataReader reader = _db.ExcecuteReader(DbNames.GETLASTDIEDPETS_STOREDPROC, CommandType.StoredProcedure, ownerIDParameter);

            int ordinalPET_ID = reader.GetOrdinal(DbNames.PET_ID);
            int ordinalPET_OWNERID = reader.GetOrdinal(DbNames.PET_OWNERID);
            int ordinalPET_MAPID = reader.GetOrdinal(DbNames.PET_MAPID);
            int ordinalPET_TYPE = reader.GetOrdinal(DbNames.PET_TYPE);
            int ordinalPET_BASETYPE = reader.GetOrdinal(DbNames.PET_BASETYPE);
            int ordinalPET_LEVEL = reader.GetOrdinal(DbNames.PET_LEVEL);
            int ordinalPET_NAME = reader.GetOrdinal(DbNames.PET_NAME);
            int ordinalPET_CURHEALTH = reader.GetOrdinal(DbNames.PET_CURHEALTH);
            int ordinalPET_MAXHEALTH = reader.GetOrdinal(DbNames.PET_MAXHEALTH);
            int ordinalPET_DAMAGE = reader.GetOrdinal(DbNames.PET_DAMAGE);
            int ordinalPET_DEFENSE = reader.GetOrdinal(DbNames.PET_DEFENSE);
            int ordinalPET_ATTACKRATING = reader.GetOrdinal(DbNames.PET_ATTACKRATING);
            int ordinalPET_EXPERIENCE = reader.GetOrdinal(DbNames.PET_CURRENTEXPERIENCE);
            int ordinalPET_WILDNESS = reader.GetOrdinal(DbNames.PET_WILDNESS);
            int ordinalPET_EVOLUTION = reader.GetOrdinal(DbNames.PET_EVOLUTION);
            int ordinalPET_ISLEGENDARY = reader.GetOrdinal(DbNames.PET_ISLEGENDARY);
            int ordinalPET_ISALIVE = reader.GetOrdinal(DbNames.PET_ISALIVE);
            int ordinalPET_ISSEALED = reader.GetOrdinal(DbNames.PET_ISSEALED);

            List<Pet> lastDiedPets = new List<Pet>();

            while (reader.Read())
            {
                Pet p = new Pet();
                p.PetID = reader.GetInt32(ordinalPET_ID);
                p.OwnerID = reader.GetInt32(ordinalPET_OWNERID);
                p.MapID = reader.GetInt32(ordinalPET_MAPID);
                p.PetType = reader.GetByte(ordinalPET_TYPE);
                p.PetBaseType = reader.GetByte(ordinalPET_BASETYPE);
                p.Level = reader.GetInt16(ordinalPET_LEVEL);
                p.Name = reader.GetString(ordinalPET_NAME);
                p.CurHealth = reader.GetInt32(ordinalPET_CURHEALTH);
                p.MaxHealth = reader.GetInt32(ordinalPET_MAXHEALTH);
                p.Damage = reader.GetInt32(ordinalPET_DAMAGE);
                p.Defense = reader.GetInt32(ordinalPET_DEFENSE);
                p.AttackRating = reader.GetInt32(ordinalPET_ATTACKRATING);
                p.CurrentExperience = reader.GetInt64(ordinalPET_EXPERIENCE);
                p.Wildness = reader.GetByte(ordinalPET_WILDNESS);
                p.Evolution = reader.GetByte(ordinalPET_EVOLUTION);
                p.IsLegendary = reader.GetBoolean(ordinalPET_ISLEGENDARY);
                p.Alive = reader.GetBoolean(ordinalPET_ISALIVE);
                lastDiedPets.Add(p);
            }

            reader.Close();
            _db.Close();

            return lastDiedPets;
        }

        public PetEvolution[] GetPetEvolutionByType(byte type)
        {
            DbParameter typeParameter = _db.CreateParameter(DbNames.GETPETEVOLUTION_TYPE_PARAMETER, type);
            typeParameter.DbType = DbType.Byte;

            _db.Open();
            DbDataReader reader = _db.ExcecuteReader(DbNames.GETPETEVOLUTION_STOREDPROC, CommandType.StoredProcedure, typeParameter);

            int ordinalEvolution_Level = reader.GetOrdinal(DbNames.PETEVOLUTION_LEVEL);
            int ordinalEvolution_EvolutionCount = reader.GetOrdinal(DbNames.PETEVOLUTION_COUNT);
            int ordinalEvolution_ToType = reader.GetOrdinal(DbNames.PETEVOLUTION_TOTYPE);

            PetEvolution[] Evolutions = new PetEvolution[3];
            int pos = 0;

            while (reader.Read())
            {
                Evolutions[pos] = new PetEvolution();
                Evolutions[pos].Level = reader.GetInt16(ordinalEvolution_Level);
                Evolutions[pos].Evolution = reader.GetByte(ordinalEvolution_EvolutionCount);
                Evolutions[pos].Type = reader.GetByte(ordinalEvolution_ToType);
                pos++;
            }

            reader.Close();
            _db.Close();

            return Evolutions;
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
    }
}
