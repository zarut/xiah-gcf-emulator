using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace XiahBLL
{
    public class MapItemManager:ManagerBase
    {
        public MapItemManager(string conString, string providerName)
            : base(conString, providerName)
        {
        }

        public void DeleteMapItem(int mapItemId, int ItemId)
        {
            DbParameter mapItemIdParameter = _db.CreateParameter(DbNames.DELETEMAPITEM_MAPITEMID_PARAMETER, mapItemId);
            mapItemIdParameter.DbType = System.Data.DbType.Int32;

            DbParameter ItemIdParameter = _db.CreateParameter(DbNames.DELETEMAPITEM_ITEMID_PARAMETER, ItemId);
            ItemIdParameter.DbType = System.Data.DbType.Int32;

            _db.Open();

            _db.ExecuteNonQuery(DbNames.DELETEMAPITEM_STOREDPROC, System.Data.CommandType.StoredProcedure, mapItemIdParameter, ItemIdParameter);

            _db.Close();
        }

        public int InsertMapItem(int mapId, short mapX, short mapY, short mapZ, byte bType, short visualId, short referenceId, int itemId, short amount, int droppedByPlayerId)
        {
            DbParameter mapIdParameter = _db.CreateParameter(DbNames.INSERTMAPITEM_MAPID_PARAMETER, mapId);
            DbParameter mapXParameter = _db.CreateParameter(DbNames.INSERTMAPITEM_MAPX_PARAMETER, mapX);
            DbParameter mapYParameter = _db.CreateParameter(DbNames.INSERTMAPITEM_MAPY_PARAMETER, mapY);
            DbParameter mapZParameter = _db.CreateParameter(DbNames.INSERTMAPITEM_MAPZ_PARAMETER, mapZ);
            DbParameter bTypeParameter = _db.CreateParameter(DbNames.INSERTMAPITEM_BTYPE_PARAMETER, bType);
            DbParameter visualIdParameter = _db.CreateParameter(DbNames.INSERTMAPITEM_VISUALID_PARAMETER, visualId);
            DbParameter referenceIdParameter = _db.CreateParameter(DbNames.INSERTMAPITEM_REFERENCEID_PARAMETER, referenceId);
            DbParameter itemIdParameter = _db.CreateParameter(DbNames.INSERTMAPITEM_ITEMID_PARAMETER, itemId);
            DbParameter amountParameter = _db.CreateParameter(DbNames.INSERTMAPITEM_AMOUNT_PARAMETER, amount);
            DbParameter droppedByPlayerIdParameter = _db.CreateParameter(DbNames.INSERTMAPITEM_DROPPEDBYPLAYERID, droppedByPlayerId);
            DbParameter mapItemIdParameter = _db.CreateParameter(DbNames.INSERTMAPITEM_MAPITEMID_OUTPUT, System.Data.DbType.Int32);

            mapIdParameter.DbType = System.Data.DbType.Int32;
            mapXParameter.DbType = System.Data.DbType.Int16;
            mapYParameter.DbType = System.Data.DbType.Int16;
            mapZParameter.DbType = System.Data.DbType.Int16;
            bTypeParameter.DbType = System.Data.DbType.Byte;
            visualIdParameter.DbType = System.Data.DbType.Int16;
            referenceIdParameter.DbType = System.Data.DbType.Int16;
            itemIdParameter.DbType = System.Data.DbType.Int32;
            amountParameter.DbType = System.Data.DbType.Int16;
            droppedByPlayerIdParameter.DbType = System.Data.DbType.Int32;
            mapItemIdParameter.Direction = System.Data.ParameterDirection.Output;

            _db.Open();

            _db.ExecuteNonQuery(DbNames.INSERTMAPITEM_STOREDPROC, System.Data.CommandType.StoredProcedure,
                mapIdParameter, mapXParameter, mapYParameter, mapZParameter, bTypeParameter, visualIdParameter, referenceIdParameter, itemIdParameter, amountParameter, droppedByPlayerIdParameter, mapItemIdParameter);

            _db.Close();

            int mapItemId = (int)mapItemIdParameter.Value;

            return mapItemId;
        }

    }
}
