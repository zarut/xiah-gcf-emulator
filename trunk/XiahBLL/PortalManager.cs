using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using System.Data.Common;

namespace XiahBLL
{
    class PortalManager:ManagerBase
    {
        public PortalManager(string conString, string providerName)
            : base(conString, providerName)
        {
        }

        public List<Portal> GetPortalsByMapId(int mapId)
        {
            DbParameter mapIdParameter = _db.CreateParameter(DbNames.GETPORTALSBYMAPID_ID_PARAMETER, mapId);

            mapIdParameter.DbType = System.Data.DbType.Int32;

            _db.Open();

            DbDataReader reader = _db.ExcecuteReader(DbNames.GETPORTALSBYMAPID_STOREDPROC, System.Data.CommandType.StoredProcedure, mapIdParameter);

            int ordinalMapId = reader.GetOrdinal(DbNames.PORTAL_MAPID);
            int ordinalToMapId = reader.GetOrdinal(DbNames.PORTAL_TOMAPID);
            int ordinalWidth = reader.GetOrdinal(DbNames.PORTAL_WIDTH);
            int ordinalHeight = reader.GetOrdinal(DbNames.PORTAL_HEIGHT);
            int ordinalToX = reader.GetOrdinal(DbNames.PORTAL_TOX);
            int ordinalToY = reader.GetOrdinal(DbNames.PORTAL_TOY);
            int ordinalFromX = reader.GetOrdinal(DbNames.PORTAL_FROMX);
            int ordinalFromY = reader.GetOrdinal(DbNames.PORTAL_FROMY);

            List<Portal> listPortals = new List<Portal>();

            while (reader.Read())
            {
                Portal p = new Portal
                {
                    MapID = reader.GetInt32(ordinalMapId),
                    ToMapID = reader.GetInt32(ordinalToMapId),
                    Width = reader.GetInt16(ordinalWidth),
                    Height = reader.GetInt16(ordinalHeight),
                    ToX = reader.GetInt16(ordinalToX),
                    ToY = reader.GetInt16(ordinalToY),
                    FromX = reader.GetInt16(ordinalFromX),
                    FromY = reader.GetInt16(ordinalFromY)
                };

                listPortals.Add(p);
            }

            reader.Close();
            _db.Close();

            return listPortals;
        }
    }
}
