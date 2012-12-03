﻿using System; using System.Collections.Generic; using System.Linq; using System.Text; using Entities; using System.Data.Common;  namespace XiahBLL {     public class MapManager : ManagerBase     {         PortalManager portalManager;         NpcManager npcManager;          public MapManager(string conString, string providerName)             : base(conString, providerName)         {             portalManager = new PortalManager(conString, providerName);             npcManager = new NpcManager(conString, providerName);         }          public Map[] GetAllMaps()         {             _db.Open();              DbDataReader reader = _db.ExcecuteReader(DbNames.GETALLMAPS_STOREDPROC, System.Data.CommandType.StoredProcedure, null);              int ordinalId = reader.GetOrdinal(DbNames.MAP_ID);             int ordinalName = reader.GetOrdinal(DbNames.MAP_NAME);             int ordinalMultiplyValue = reader.GetOrdinal(DbNames.MAP_MULTIPLYVALUE);             int ordinalSpawnX = reader.GetOrdinal(DbNames.MAP_SPAWNX);             int ordinalSpawnY = reader.GetOrdinal(DbNames.MAP_SPAWNY);              List<Map> maps = new List<Map>();              while (reader.Read())             {                 Map m = new Map()                 {                     MapID = reader.GetInt32(ordinalId),                     Name = reader.GetString(ordinalName),                     MultiplyValue = reader.GetInt32(ordinalMultiplyValue),                     SpawnX = reader.GetInt32(ordinalSpawnX),                     SpawnY = reader.GetInt32(ordinalSpawnY)                 };                  maps.Add(m);             }              reader.Close();             _db.Close();              foreach (Map map in maps)             {                 map.Portals = portalManager.GetPortalsByMapId(map.MapID);                 map.Npcs = npcManager.GetNpcsByMapId(map.MapID);             }                           return maps.ToArray();         }          public Map GetMapById(int mapId)         {             DbParameter mapIdParameter = _db.CreateParameter(DbNames.GETMAPBYID_ID_PARAMETER, mapId);              mapIdParameter.DbType = System.Data.DbType.Int32;              _db.Open();              DbDataReader reader = _db.ExcecuteReader(DbNames.GETMAPBYID_STOREDPROC, System.Data.CommandType.StoredProcedure, mapIdParameter);              int ordinalId = reader.GetOrdinal(DbNames.MAP_ID);             int ordinalName = reader.GetOrdinal(DbNames.MAP_NAME);             int ordinalMultiplyValue = reader.GetOrdinal(DbNames.MAP_MULTIPLYVALUE);             int ordinalSpawnX = reader.GetOrdinal(DbNames.MAP_SPAWNX);             int ordinalSpawnY = reader.GetOrdinal(DbNames.MAP_SPAWNY);              Map m = null;              while (reader.Read())             {                 m = new Map                 {                     MapID = reader.GetInt32(ordinalId),                     Name = reader.GetString(ordinalName),                     MultiplyValue = reader.GetInt32(ordinalMultiplyValue),                     SpawnX = reader.GetInt32(ordinalSpawnX),                     SpawnY = reader.GetInt32(ordinalSpawnY)                 };             }              reader.Close();             _db.Close();              m.Portals = portalManager.GetPortalsByMapId(mapId);             m.Npcs = npcManager.GetNpcsByMapId(mapId);              return m;         }                                            } } 