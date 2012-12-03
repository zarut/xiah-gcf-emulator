using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using System.Data.Common;
using System.Data;

namespace XiahBLL
{
    class MonsterManager : ManagerBase
    {
        private Random rand;
        public MonsterManager(string conString, string providerName)
            : base(conString, providerName)
        {
            rand = new Random();
        }

        private int totalMonstersLoaded = 0;

        public List<Monster> GetMonstersByMapId(int mapId)
        {
            DbParameter mapIdParameter = _db.CreateParameter(DbNames.GETMONSTERSBYMAPID_ID_PARAMETER, mapId);
            mapIdParameter.DbType = System.Data.DbType.Int32;
           
            _db.Open();

            DbDataReader reader = _db.ExcecuteReader(DbNames.GETMONSTERSBYMAPID_STOREDPROC, System.Data.CommandType.StoredProcedure, mapIdParameter);

            int ordinalMonster_count = reader.GetOrdinal(DbNames.MONSTER_COUNT);
            int ordinalMonster_type = reader.GetOrdinal(DbNames.MONSTER_TYPE);
            int ordinalMonster_level = reader.GetOrdinal(DbNames.MONSTER_LEVEL);
            int ordinalMonster_name = reader.GetOrdinal(DbNames.MONSTER_NAME);
            int ordinalMonster_health = reader.GetOrdinal(DbNames.MONSTER_HEALTH);
            int ordinalMonster_damage = reader.GetOrdinal(DbNames.MONSTER_DAMAGE);
            int ordinalMonster_defense = reader.GetOrdinal(DbNames.MONSTER_DEFENSE);
            int ordinalMonster_attackrating = reader.GetOrdinal(DbNames.MONSTER_ATTACKRATING);
            int ordinalMonster_walkspeed = reader.GetOrdinal(DbNames.MONSTER_WALKSPEED);
            int ordinalMonster_sightrange = reader.GetOrdinal(DbNames.MONSTER_SIGHTRANGE);
            int ordinalMonster_wanderrange = reader.GetOrdinal(DbNames.MONSTER_WANDERRANGE);
            int ordinalMonster_attackrange = reader.GetOrdinal(DbNames.MONSTER_ATTACKRANGE);
            int ordinalMonster_experience = reader.GetOrdinal(DbNames.MONSTER_EXPERIENCE);
            int ordinalMonster_healpoint = reader.GetOrdinal(DbNames.MONSTER_HEALPOINT);
            int ordinalMonster_regeneration = reader.GetOrdinal(DbNames.MONSTER_REGENERATION);
            int ordinalMonster_id = reader.GetOrdinal(DbNames.MONSTER_ID); // monster ref id
            int ordinalMonster_mapid = reader.GetOrdinal(DbNames.MONSTER_MAPID);
            int ordinalMonster_spawnx = reader.GetOrdinal(DbNames.MONSTER_SPAWNX);
            int ordinalMonster_spawny = reader.GetOrdinal(DbNames.MONSTER_SPAWNY);
            int ordinalMonster_groupid = reader.GetOrdinal(DbNames.MONSTER_GROUPID);
            int ordinalMonster_grouporder = reader.GetOrdinal(DbNames.MONSTER_GROUPORDER);
            int ordinalMonster_leaderid = reader.GetOrdinal(DbNames.MONSTER_LEADERID);
            int ordinalMonster_healthinc = reader.GetOrdinal(DbNames.MONSTER_HEALTHINC);
            int ordinalMonster_damageinc = reader.GetOrdinal(DbNames.MONSTER_DAMAGEINC);
            int ordinalMonster_defenseinc = reader.GetOrdinal(DbNames.MONSTER_DEFENSEINC);
            int ordinalMonster_attackratinginc = reader.GetOrdinal(DbNames.MONSTER_ATTACKRATINGINC);
            int ordinalMonster_attackrangeinc = reader.GetOrdinal(DbNames.MONSTER_ATTACKRANGEINC);
            int ordinalMonster_expinc = reader.GetOrdinal(DbNames.MONSTER_EXPINC);
            int ordinalMonster_istameable = reader.GetOrdinal(DbNames.MONSTER_ISTAMEABLE);

            List<Monster> listMonsters = new List<Monster>();

            while (reader.Read())
            {
                int count = reader.GetInt32(ordinalMonster_count);

                for (int i = 0; i < count; i++)
                {
                    Monster p = new Monster
                    {
                        MonsterType = reader.GetByte(ordinalMonster_type),
                        Level = reader.GetInt16(ordinalMonster_level),
                        Name = reader.GetString(ordinalMonster_name),
                        MaxHealth = reader.GetInt32(ordinalMonster_health),
                        HealthInc = reader.GetInt32(ordinalMonster_healthinc),
                        Damage = reader.GetInt32(ordinalMonster_damage),
                        DamageInc = reader.GetInt16(ordinalMonster_damageinc),
                        Defense = reader.GetInt32(ordinalMonster_defense),
                        DefenseInc = reader.GetInt16(ordinalMonster_defenseinc),
                        AttackRating = reader.GetInt32(ordinalMonster_attackrating),
                        AttackRatingInc = reader.GetInt16(ordinalMonster_attackratinginc),
                        AttackRange = reader.GetInt16(ordinalMonster_attackrange),
                        AttackRangeInc = reader.GetInt16(ordinalMonster_attackrangeinc),
                        MovementSpeed = reader.GetByte(ordinalMonster_walkspeed),
                        SightRange = reader.GetInt16(ordinalMonster_sightrange),
                        WanderRange = reader.GetInt16(ordinalMonster_wanderrange),
                        Experience = reader.GetInt32(ordinalMonster_experience),
                        ExperienceInc = reader.GetInt32(ordinalMonster_expinc),
                        HealPoint = reader.GetInt32(ordinalMonster_healpoint),
                        Regeneration = reader.GetInt16(ordinalMonster_regeneration),
                        MonsterID = reader.GetInt32(ordinalMonster_id),
                        MapID = reader.GetInt32(ordinalMonster_mapid),
                        SpawnX = reader.GetInt16(ordinalMonster_spawnx),
                        SpawnY = reader.GetInt16(ordinalMonster_spawny),
                        GroupID = reader.GetInt16(ordinalMonster_groupid),
                        GroupOrder = reader.GetByte(ordinalMonster_grouporder),
                        LeaderID = reader.GetInt32(ordinalMonster_leaderid),
                        IsTameable = reader.GetBoolean(ordinalMonster_istameable)
                    };

                    totalMonstersLoaded++;

                    // it loads the monster id as reference id so keh
                    p.MonsterReferenceID = p.MonsterID;

                    // so we can have multiple monsters for one reference id :)
                    p.MonsterID = totalMonstersLoaded;

                    p.CurHealth = p.MaxHealth;

                    // so all wont stack together now they have small distances between others
                    p.SpawnX = (short)rand.Next(p.SpawnX, p.SpawnX + p.WanderRange);
                    p.SpawnY = (short)rand.Next(p.SpawnY, p.SpawnY + p.WanderRange);

                    p.Position = new Position();
                    p.Position.X = p.SpawnX;
                    p.Position.Y = p.SpawnY;
                    p.Alive = true;

                    listMonsters.Add(p);
                }
            }

            reader.Close();
            _db.Close();

            return listMonsters;
        }
    }
}
