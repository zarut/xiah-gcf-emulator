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
    public class GuildManager : ManagerBase
    {
        public GuildManager(string conString, string providerName)
            : base(conString, providerName)
        {

        }

        public Guild CreateGuild(string name, int owner)
        {
            DbParameter nameIdParameter = _db.CreateParameter(DbNames.CREATEGUILD_NAME, name);
            nameIdParameter.DbType = DbType.String;

            DbParameter ownerIdParameter = _db.CreateParameter(DbNames.CREATEGUILD_OWNERID, owner);
            ownerIdParameter.DbType = DbType.Int32;

            DbParameter guildIdParameter = _db.CreateParameter(DbNames.CREATEGUILD_GUILDID, DbType.Int32);
            guildIdParameter.Direction = System.Data.ParameterDirection.Output;

            DbParameter returnValueParameter = _db.CreateParameter(DbNames.RETURNVALUE_PARAMETER, DbType.Int32);
            returnValueParameter.DbType = System.Data.DbType.Int32;
            returnValueParameter.Direction = System.Data.ParameterDirection.ReturnValue;

            _db.Open();

            Guild g = null;

            _db.ExecuteNonQuery(DbNames.CREATEGUILD,
                System.Data.CommandType.StoredProcedure,
                returnValueParameter,
                nameIdParameter,
                ownerIdParameter,
                guildIdParameter);

            _db.Close();

            int returnValue = (int)returnValueParameter.Value;

            if (returnValue == 1)
            {
                g = GetGuildByCharacterID(owner);
            }

            return g;
        }

        public void DeleteGuild(int guildID)
        {
            DbParameter guildIdParameter = _db.CreateParameter(DbNames.DELETEGUILD_ID_PARAMETER, guildID);
            guildIdParameter.DbType = DbType.Int32;

            _db.Open();
            _db.ExecuteNonQuery(DbNames.DELETEGUILD, System.Data.CommandType.StoredProcedure, guildIdParameter);
            _db.Close();
        }

        public void DeleteGuildMember(int memberID)
        {
            DbParameter memberIdParameter = _db.CreateParameter(DbNames.DELETEGUILDMEMBER_ID_PARAMETER, memberID);
            memberIdParameter.DbType = DbType.Int32;

            _db.Open();
            _db.ExecuteNonQuery(DbNames.DELETEGUILDMEMBER, System.Data.CommandType.StoredProcedure, memberIdParameter);
            _db.Close();
        }

        public Guild GetGuildByCharacterID(int characterID)
        {
            DbParameter characterIdParameter = _db.CreateParameter(DbNames.GETGUILDBYCHARACTERID_CHARACTERID, characterID);
            characterIdParameter.DbType = DbType.Int32;

            Guild g = null;

            _db.Open();

            DbDataReader reader = _db.ExcecuteReader(DbNames.GETGUILDBYCHARACTERID, CommandType.StoredProcedure, characterIdParameter);

            int ordinalGuild_id = reader.GetOrdinal(DbNames.GUILD_ID);
            int ordinalGuild_name = reader.GetOrdinal(DbNames.GUILD_NAME);
            int ordinalGuild_notice = reader.GetOrdinal(DbNames.GUILD_NOTICE);
            int ordinalGuild_fame = reader.GetOrdinal(DbNames.GUILD_FAME);
            int ordinalGuild_donations = reader.GetOrdinal(DbNames.GUILD_DONATIONS);

            while (reader.Read())
            {
                g = new Guild();
                g.ID = reader.GetInt32(ordinalGuild_id);
                g.Fame = reader.GetInt32(ordinalGuild_fame);
                g.Donations = reader.GetInt32(ordinalGuild_donations);
                g.Name = reader.GetString(ordinalGuild_name);
                g.Notice = reader.GetString(ordinalGuild_notice);
            }

            reader.Close();
            _db.Close();

            // get member list, get member with rank 1 as leader.
            if (g != null)
            {
                g.Members = GetGuildMembersByGuildID(g.ID);
                GuildMember master = g.Members.Single(x => x.Rank == GuildOrder.Guild_Master);
                if (master != null)
                    g.Leader = master;
            }

            return g;
        }

        public void InsertGuildMember(GuildMember gm, Guild g)
        {
            DbParameter guildIdParameter = _db.CreateParameter(DbNames.INSERTGUILDMEMBER_GUILDID, g.ID);

            DbParameter characterIdParameter = _db.CreateParameter(DbNames.INSERTGUILDMEMBER_CHARACTERID, gm.Character.CharacterId);
            characterIdParameter.DbType = DbType.Int32;

            DbParameter titleParameter = _db.CreateParameter(DbNames.INSERTGUILDMEMBER_TITLE, gm.Title);
            titleParameter.DbType = DbType.String;

            DbParameter rankParameter = _db.CreateParameter(DbNames.INSERTGUILDMEMBER_RANK, gm.Rank);
            rankParameter.DbType = DbType.Int32;

            _db.Open();
            _db.ExecuteNonQuery(DbNames.INSERTGUILDMEMBER, CommandType.StoredProcedure, guildIdParameter, characterIdParameter, titleParameter, rankParameter);
            _db.Close();
        }

        public void UpdateGuildMember(GuildMember gm)
        {
            DbParameter characterIdParameter = _db.CreateParameter(DbNames.UPDATEGUILDMEMBER_CHARACTERID, gm.Character.CharacterId);
            characterIdParameter.DbType = DbType.Int32;

            DbParameter titleParameter = _db.CreateParameter(DbNames.UPDATEGUILDMEMBER_TITLE, gm.Title);
            titleParameter.DbType = DbType.String;

            DbParameter rankParameter = _db.CreateParameter(DbNames.UPDATEGUILDMEMBER_RANK, gm.Rank);
            rankParameter.DbType = DbType.Int32;

            _db.Open();
            _db.ExecuteNonQuery(DbNames.UPDATEGUILDMEMBER, CommandType.StoredProcedure, characterIdParameter, titleParameter, rankParameter);
            _db.Close();
        }

        public void UpdateGuild(Guild g)
        {
            DbParameter guildIdParameter = _db.CreateParameter(DbNames.UPDATEGUILD_ID, g.ID);
            guildIdParameter.DbType = DbType.Int32;

            DbParameter noticeParameter = _db.CreateParameter(DbNames.UPDATEGUILD_NOTICE, g.Notice);
            noticeParameter.DbType = DbType.String;

            DbParameter fameParameter = _db.CreateParameter(DbNames.UPDATEGUILD_FAME, g.Fame);
            fameParameter.DbType = DbType.Int32;

            DbParameter donationsParameter = _db.CreateParameter(DbNames.UPDATEGUILD_DONATIONS, g.Donations);
            donationsParameter.DbType = DbType.Int32;

            _db.Open();
            _db.ExecuteNonQuery(DbNames.UPDATEGUILD, CommandType.StoredProcedure, guildIdParameter, noticeParameter, fameParameter, donationsParameter);
            _db.Close();
        }


        public List<GuildMember> GetGuildMembersByGuildID(int guildID)
        {
            DbParameter guildIdParameter = _db.CreateParameter(DbNames.GETGUILDMEMBERSBYID_GUILDID, guildID);
            guildIdParameter.DbType = DbType.Int32;

            List<GuildMember> members = new List<GuildMember>();

            _db.Open();

            DbDataReader reader = _db.ExcecuteReader(DbNames.GETGUILDMEMBERSBYID, CommandType.StoredProcedure, guildIdParameter);

            int ordinalGUILDMEMBER_TITLE = reader.GetOrdinal(DbNames.GUILDMEMBER_TITLE);
            int ordinalGUILDMEMBER_RANK = reader.GetOrdinal(DbNames.GUILDMEMBER_RANK);
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


            while (reader.Read())
            {
                Character c = new Character();

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

                string title = reader.GetString(ordinalGUILDMEMBER_TITLE);
                GuildOrder rank = (GuildOrder)reader.GetInt32(ordinalGUILDMEMBER_RANK);

                members.Add(new GuildMember(title, rank, c));
            }

            reader.Close();
            _db.Close();

            return members;
        }
    }
}
