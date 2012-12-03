using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data.SqlClient;
using DAL;
using Entities;
using System.Data;

namespace XiahBLL
{
    public class WorldManager : ManagerBase
    {
        public WorldManager(string conString, string providerName)
            : base(conString, providerName)
        {

        }

        public WorldInfo[] GetAllWorlds()
        {
            _db.Open();

            List<WorldInfo> worlds = new List<WorldInfo>();

            DbDataReader reader = _db.ExcecuteReader(DbNames.GETALLWORLDS_STOREDPROC, System.Data.CommandType.StoredProcedure, null);

            int ordinalId = reader.GetOrdinal(DbNames.WORLDINFO_ID);
            int ordinalName = reader.GetOrdinal(DbNames.WORLDINFO_NAME);
            int ordinalDesc = reader.GetOrdinal(DbNames.WORLDINFO_DESC);
            int ordinalIP = reader.GetOrdinal(DbNames.WORLDINFO_IP);
            int ordinalPort = reader.GetOrdinal(DbNames.WORLDINFO_PORT);

            while (reader.Read())
            {
                WorldInfo world = new WorldInfo
                {
                    WorldId = reader.GetInt32(ordinalId),
                    WorldName = reader.GetString(ordinalName),
                    WorldDesc = reader.GetString(ordinalDesc),
                    IPAddress = reader.GetString(ordinalIP),
                    Port = reader.GetInt32(ordinalPort)
                };

                worlds.Add(world);
            }

            reader.Close();
            _db.Close();

            return worlds.ToArray();
        }     
    }
}
