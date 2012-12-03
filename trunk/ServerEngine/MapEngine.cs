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
        public event EventHandler<MonsterMoveInfoEventArgs> MonsterMove;
        public event EventHandler<MonsterAttackInfoEventArgs> MonsterAttack;

        MapManager mapManager;
        MapItemManager mapItemManager;
        ItemDataManager itemDataManager;
        public List<MapItem> mapItems;
     //   public static Random Random = new Random();
        GameEngine gameEngine;
        private MonsterEngine monsterEngine;
        //Map[] maps;

        public Map Map
        {
            get;
            private set;
        }

        public List<BaseEntity> ActiveClientEntities
        {
            get;
            private set;
        }

        public MapEngine(string conString, string providerName, Map map)
        {
            mapManager = new MapManager(conString, providerName);
            itemDataManager = new ItemDataManager(conString, providerName);
            mapItemManager = new MapItemManager(conString, providerName);
            this.Map = map;
            mapItems = new List<MapItem>();
            gameEngine = new GameEngine(conString, providerName);
            ActiveClientEntities = new List<BaseEntity>();
            monsterEngine = new MonsterEngine(map.Monsters, ActiveClientEntities);
            monsterEngine.MonsterMove += new EventHandler<MonsterMoveInfoEventArgs>(monsterEngine_MonsterMove);
            monsterEngine.MonsterAttack += new EventHandler<MonsterAttackInfoEventArgs>(monsterEngine_MonsterAttack);
            monsterEngine.StartEngine();
        }

        void monsterEngine_MonsterAttack(object sender, MonsterAttackInfoEventArgs e)
        {
            if (MonsterAttack != null)
                MonsterAttack(sender, e);
        }

        void monsterEngine_MonsterMove(object sender, MonsterMoveInfoEventArgs e)
        {
            if (MonsterMove != null)
                MonsterMove(sender, e);
        }

        //public Map GetMapById(int mapId)
        //{
        //    return maps.Where(x => x.MapID == mapId).First();
        //}

        //public Map[] Maps { get { return maps; } }

        public MapItem PlayerDropItem(int droppedByCharId ,BaseItem item,short x, short y, byte z, int mapId)
        {
            item.OwnerID = -1;
            item.Bag = 255;
            item.Slot = 255;

            MapItem m = new MapItem
            {
                DropTime = DateTime.Now,
                MapID = mapId,
                MapX = x,
                MapY = y,
                MapZ = z,
                bType =item.bType,
                ItemID = item.ItemID,
                Amount = item.Amount,
                ReferenceID = item.ReferenceID,
                VisualID = item.VisualID,
                DroppedByCharacterID = droppedByCharId
            };

            int mapItemId = mapItemManager.InsertMapItem(m.MapID, m.MapX, m.MapY, m.MapZ, m.bType, m.VisualID, m.ReferenceID, m.ItemID, m.Amount, droppedByCharId);

            m.MapItemID = mapItemId;

            if (item.ItemID != 0)
                itemDataManager.UpdateItem(item);
            mapItems.Add(m);

            return m;
        }

        public void DeleteMapItem(MapItem m)
        {
            mapItemManager.DeleteMapItem(m.MapItemID, m.ItemID);
            mapItems.Remove(m);
        }

        public BaseItem PickupItem(int mapItemId, Client c, short mapItemAmount)
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

            BaseItem item = null;

            if (m.DroppedByCharacterID == 0 || m.DroppedByCharacterID == c.MyCharacter.CharacterId)
            {
                if (m.bType != (byte)bType.Jeon)
                {
                    if (m.ItemID != 0)
                        item = itemDataManager.GetItemByItemID(m.ItemID);
                    else
                        item = itemDataManager.GetItemByReferenceID(m.ReferenceID);


                    BagSlot bagSlot = gameEngine.TryPickToBags(c.MyCharacter.Bags.ToArray(), item);

                    item.Slot = bagSlot.Slot;
                    item.Bag = bagSlot.Bag;

                    mapItemManager.DeleteMapItem(m.MapItemID, 0);
                    mapItems.Remove(m);
                    item.OwnerID = c.MyCharacter.CharacterId;

                    if (item.ItemID != 0)
                        itemDataManager.UpdateItem(item);
                    else
                    {
                        item.ItemID = itemDataManager.InsertItem(item);
                        if (item is Equipment)
                        {
                            // later add chance to get these items blabla
                            Equipment Item = item as Equipment;
                            ImbueStat stat = ImbueStat.None;
                            ImbueItem imbueitem = new ImbueItem
                            {
                                ImbueChance = 1,
                                IncreaseValue = 1,
                            };
                            // Possible plus for drop
                            if (XiahRandom.PercentSuccess(40))
                            {
                                int plus = gameEngine.RandomChance(0, 5);
                                for (int i = 0; i < plus; i++)
                                {
                                    gameEngine.BlackImbue(Item, ref stat, imbueitem, 1);
                                    Item.Plus++;
                                }
                            }

                            if (XiahRandom.PercentSuccess(40))
                            {
                                // Possible slvl for drop
                                int slvl = gameEngine.RandomChance(0, 5);
                                for (int i = 0; i < slvl && !(Item is Cape); i++)
                                {
                                    gameEngine.WhiteImbue(Item, ref stat, imbueitem);
                                    Item.Slvl++;
                                }
                            }

                            item = Item;
                            itemDataManager.UpdateItem(item);
                        }
                    }
                }
                else
                {
                    item = itemDataManager.GetItemByReferenceID(m.ReferenceID);
                    mapItemManager.DeleteMapItem(m.MapItemID, 0);
                    mapItems.Remove(m);

                    item.Amount = mapItemAmount;
                }
            }
            else
            {
                c.Send(PacketEngine.PacketManager.SendPickUpText(0, 0, 0));
                // send pickuperror blabal
            }

            return item;
        }

        public MapItem[] GetNearbyMapItems(Character ch)
        {
            var nearbyMapItems = mapItems.Where(x => (GetDistance(x.MapX, x.MapY, ch.Position.X, ch.Position.Y) < 150));

            return nearbyMapItems.ToArray();
        }

        public Npc[] GetNearbyNpcs(Character ch)
        {
            try
            {
                //var map = maps.Where(x => x.MapID == ch.MapId).First();
                var nearbyNpcs = Map.Npcs.Where(x => (GetDistance(x.Position.X, x.Position.Y, ch.Position.X, ch.Position.Y) < 150));

                return nearbyNpcs.ToArray();
            }
            catch
            {
                return null;
            }
        }

        public Npc GetNpcByID(int npcid, int mapid)
        {
            //var map = maps.Where(x => x.MapID == mapid).First();
            var npc = Map.Npcs.Where(x => x.NpcID == npcid).First();

            return npc;
        }

        public Monster GetMonsterByID(int monsterid, int mapid)
        {
            try
            {
                //var map = maps.Where(x => x.MapID == mapid).First();
                var mob = Map.Monsters.Where(x => x.MonsterID == monsterid).First();

                return mob;
            }
            catch
            {
                return null;
            }
        }

        public Monster[] GetMonstersByRange(Pet pet, int distance)
        {
            try
            {
                //var map = maps.Where(x => x.MapID == pet.MapID).First();
                var monsters = Map.Monsters.Where(x => x.Alive && GetDistance(x.Position.X, x.Position.Y, pet.Position.X, pet.Position.Y) < distance);


                return monsters.ToArray();
            }
            catch
            {
                return null;
            }
        }

        public Monster[] GetMonstersByRange(Character ch, int distance)
        {
            try
            {
                //var map = maps.Where(x => x.MapID == ch.MapId).First();
                var monsters = Map.Monsters.Where(x => x.Alive && GetDistance(x.Position.X, x.Position.Y, ch.Position.X, ch.Position.Y) < distance);


                return monsters.ToArray();
            }
            catch
            {
                return null;
            }
        }

        public Monster[] GetMonstersByRange(Monster m, int distance)
        {
            try
            {
                //var map = maps.Where(x => x.MapID == m.MapID).First();
                var monsters = Map.Monsters.Where(x => x.Alive && GetDistance(x.Position.X, x.Position.Y, m.Position.X, m.Position.Y) < distance);


                return monsters.ToArray();
            }
            catch
            {
                return null;
            }
        }

        public List<Character> GetCharactersInRange(Position pos, int distance)
        {
            List<Character> nearbyChars = ActiveClientEntities.Where(entity => entity is Character && MapEngine.GetDistance(pos.X, pos.Y, entity.Position.X, entity.Position.Y) <= distance).Cast<Character>().ToList();

            return nearbyChars;
        }

        public static bool IsWalkableTile(Character ch)
        {
            return ch.Map.WalkableTiles[(short)ch.Position.X, (short)ch.Position.Y];
        }

        public static int GetDistance(float X, float Y, float X2, float Y2)
        {
            return (int)Math.Max(Math.Abs(X - X2), Math.Abs(Y - Y2));
        }

        //public static bool PercentSuccess(double percent)
        //{
        //    double test = Random.Next(1, 1000000) / 10000;
        //    if (test >= (100 - percent)) ;
        //    return ((double)Random.Next(1, 1000000)) / 10000 >= 100 - percent;
        //}
    }
}
