using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XiahBLL;
using Entities;
using System.Configuration;

namespace ServerEngine
{
    class MapEngine
    {
        MapManager mapManager;
        MapItemManager mapItemManager;
        ItemDataManager itemDataManager;
        List<MapItem> mapItems;
        
        Map[] maps;

        public MapEngine(string conString, string providerName)
        {
            mapManager = new MapManager(conString, providerName);
            itemDataManager = new ItemDataManager(conString, providerName);
            mapItemManager = new MapItemManager(conString, providerName);
            maps = mapManager.GetAllMaps();
            mapItems = new List<MapItem>();
        }

        public Map GetMapById(int mapId)
        {
            return maps.Where(x => x.MapID == mapId).First();
        }

        public MapItem PlayerDropItem(BaseItem item,short x, short y, byte z, int mapId)
        {
            item.OwnerID = -1;
            item.Bag = 255;
            item.Slot = 255;

            MapItem m = new MapItem
            {
                MapID = mapId,
                MapX = x,
                MapY = y,
                MapZ = z,
                bType =item.bType,
                ItemID = item.ItemID,
                Amount = item.Amount,
                ReferenceID = item.ReferenceID,
                VisualID = item.VisualID
            };

            int mapItemId = mapItemManager.InsertMapItem(m.MapID, m.MapX, m.MapY, m.MapZ, m.bType, m.VisualID, m.ReferenceID, m.ItemID, m.Amount);

            m.MapItemID = mapItemId;

            itemDataManager.UpdateItem(item);
            mapItems.Add(m);

            return m;
        }

        public BaseItem PickupItem(int mapItemId, Character c)
        {
            MapItem m = null;

            try
            {
                m = mapItems.Where(x => x.MapItemID == mapItemId).First();
            }
            catch (Exception)
            {
                return null;
            }

            BaseItem item = itemDataManager.GetItemByItemID(m.ItemID);

            BagSlot bagSlot = new BagSlot();

            bool added = false;

            for (int i = 0; i < c.Bags.Count; i++)
            {
                if (!added)
                {
                    if (c.Bags[i].PickItem(item, bagSlot))
                    {
                        added = true;
                        bagSlot.Bag = (byte)(i + 1);
                        break;
                    }
                }
            }

            if (!added)
                throw new BagIsFullException(Messages.BAGISFULLEXCEPTION);

            item.Slot = bagSlot.Slot;
            item.Bag = bagSlot.Bag;

            mapItemManager.DeleteMapItem(m.MapItemID);
            mapItems.Remove(m);
            item.OwnerID = c.CharacterId;
            itemDataManager.UpdateItem(item);

            return item;
        }

        public MapItem[] GetNearbyMapItems(short charX, short charY)
        {
            var nearbyMapItems = mapItems.Where(x => (GetDistance(x.MapX, x.MapY, charX, charY) < 150));

            return nearbyMapItems.ToArray();
        }

        public Npc[] GetNearbyNpcs(Character ch)
        {
            var map = maps.Where(x => x.MapID == ch.MapId).First() ;
            var nearbyNpcs = map.Npcs.Where(x => (GetDistance(x.X, x.Y, ch.X, ch.Y) < 150));

            return nearbyNpcs.ToArray();
        }

        public Npc GetNpcByID(int npcid, int mapid)
        {
            var map = maps.Where(x => x.MapID == mapid).First();
            var npc = map.Npcs.Where(x => x.NpcID == npcid).First();

            return npc;
        }

        public int GetDistance(short X, short Y, short X2, short Y2)
        {
            return (int)Math.Max(Math.Abs(X - X2), Math.Abs(Y - Y2));
        }


    }
}
