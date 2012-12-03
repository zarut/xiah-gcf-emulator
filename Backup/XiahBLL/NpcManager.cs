using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using System.Data.Common;
using System.Data;

namespace XiahBLL
{
    class NpcManager : ManagerBase
    {
        public NpcManager(string conString, string providerName)
            : base(conString, providerName)
        {
        }

        public List<Npc> GetNpcsByMapId(int mapId)
        {
            DbParameter mapIdParameter = _db.CreateParameter(DbNames.GETNPCSBYMAPID_ID_PARAMETER, mapId);
            mapIdParameter.DbType = System.Data.DbType.Int32;

            _db.Open();

            DbDataReader reader = _db.ExcecuteReader(DbNames.GETNPCSBYMAPID_STOREDPROC, System.Data.CommandType.StoredProcedure, mapIdParameter);

            int ordinalNPC_MAPID = reader.GetOrdinal(DbNames.NPC_MAPID);
            int ordinalNPC_NPCID = reader.GetOrdinal(DbNames.NPC_NPCID);
            int ordinalNPC_NPCTYPE = reader.GetOrdinal(DbNames.NPC_NPCTYPE);
            int ordinalNPC_POSX = reader.GetOrdinal(DbNames.NPC_POSX);
            int ordinalNPC_POSY = reader.GetOrdinal(DbNames.NPC_POSY);
            int ordinalNPC_DIRECTION = reader.GetOrdinal(DbNames.NPC_DIRECTION);

            List<Npc> listNpcs = new List<Npc>();

            while (reader.Read())
            {
                Npc p = new Npc
                {
                    NpcID = reader.GetInt32(ordinalNPC_NPCID),
                    MapID = reader.GetInt32(ordinalNPC_MAPID),
                    NpcType = reader.GetByte(ordinalNPC_NPCTYPE),
                    X = reader.GetInt16(ordinalNPC_POSX),
                    Y = reader.GetInt16(ordinalNPC_POSY),
                    Direction = reader.GetInt16(ordinalNPC_DIRECTION)
                };

                listNpcs.Add(p);
            }

            reader.Close();
            _db.Close();

            foreach (Npc n in listNpcs)
            {
                for (byte i = 0; i < 4; i++)
                {
                    var bagItems = GetAllItemsInNpcBag(i, n.NpcID);
                    n.Bags.Add(new Bag(bagItems));
                }
            }

            return listNpcs;
        }


        public List<BaseItem> GetAllItemsInNpcBag(byte bag, int npcId)
        {
            DbParameter bagIdParameter = _db.CreateParameter(DbNames.GETALLNPCITEMSBYBAGID_BAGID_PARAMETER, bag);
            bagIdParameter.DbType = DbType.Byte;

            DbParameter characterIdParameter = _db.CreateParameter(DbNames.GETALLNPCITEMSBYBAGID_NPCID_PARAMETER, npcId);
            characterIdParameter.DbType = DbType.Int32;

            List<BaseItem> items = new List<BaseItem>();

            _db.Open();

            DbDataReader reader = _db.ExcecuteReader(DbNames.GETALLNPCITEMSBYBAGID_STOREDPROC, CommandType.StoredProcedure, bagIdParameter, characterIdParameter);

            int ordinalITEM_REFERENCEID = reader.GetOrdinal(DbNames.ITEM_REFID);
            int ordinalITEM_BTYPE = reader.GetOrdinal(DbNames.ITEM_BTYPE);
            int ordinalITEM_BKIND = reader.GetOrdinal(DbNames.ITEM_BKIND);
            int ordinalITEM_VISUALID = reader.GetOrdinal(DbNames.ITEM_VISUALID);
            int ordinalITEM_CLASS = reader.GetOrdinal(DbNames.ITEM_CLASS);
            int ordinalITEM_AMOUNT = reader.GetOrdinal(DbNames.ITEM_AMOUNT);
            int ordinalITEM_PRICE = reader.GetOrdinal(DbNames.ITEM_PRICE);
            int ordinalITEM_LEVEL = reader.GetOrdinal(DbNames.ITEM_LEVEL);
            int ordinalITEM_DEX = reader.GetOrdinal(DbNames.ITEM_DEX);
            int ordinalITEM_STR = reader.GetOrdinal(DbNames.ITEM_STR);
            int ordinalITEM_STA = reader.GetOrdinal(DbNames.ITEM_STA);
            int ordinalITEM_ENE = reader.GetOrdinal(DbNames.ITEM_ENE);
            int ordinalITEM_DURABILITY = reader.GetOrdinal(DbNames.ITEM_DURABILITY);
            int ordinalITEM_DAMAGE = reader.GetOrdinal(DbNames.ITEM_DAMAGE);
            int ordinalITEM_DEFENCE = reader.GetOrdinal(DbNames.ITEM_DEFENCE);
            int ordinalITEM_ATTACKRATING = reader.GetOrdinal(DbNames.ITEM_ATTACKRATING);
            int ordinalITEM_ATTACKSPEED = reader.GetOrdinal(DbNames.ITEM_ATTACKSPEED);
            int ordinalITEM_ATTACKRANGE = reader.GetOrdinal(DbNames.ITEM_ATTACKRANGE);
            int ordinalITEM_INCMAXLIFE = reader.GetOrdinal(DbNames.ITEM_INCMAXLIFE);
            int ordinalITEM_INCMAXMANA = reader.GetOrdinal(DbNames.ITEM_INCMAXMANA);
            int ordinalITEM_LIFEREGEN = reader.GetOrdinal(DbNames.ITEM_LIFEREGEN);
            int ordinalITEM_MANAREGEN = reader.GetOrdinal(DbNames.ITEM_MANAREGEN);
            int ordinalITEM_CRITICAL = reader.GetOrdinal(DbNames.ITEM_CRITICAL);
            int ordinalITEM_TOMAPID = reader.GetOrdinal(DbNames.ITEM_TOMAPID);
            int ordinalITEM_IMBUERATE = reader.GetOrdinal(DbNames.ITEM_IMBUERATE);
            int ordinalITEM_IMBUEINCREASE = reader.GetOrdinal(DbNames.ITEM_IMBUEINCREASE);
            int ordinalITEM_BOOKSKILLID = reader.GetOrdinal(DbNames.ITEM_BOOKSKILLID);
            int ordinalITEM_BOOKSKILLLEVEL = reader.GetOrdinal(DbNames.ITEM_BOOKSKILLLEVEL);
            int ordinalITEM_BOOKSKILLDATA = reader.GetOrdinal(DbNames.ITEM_BOOKSKILLDATA);
            int ordinalITEM_MAXPOLISHTRIES = reader.GetOrdinal(DbNames.ITEM_MAXPOLISHTRIES);
            int ordinalITEM_MAXIMBUETRIES = reader.GetOrdinal(DbNames.ITEM_MAXIMBUES);
            int ordinalITEM_BAG = reader.GetOrdinal(DbNames.ITEM_BAG);
            int ordinalITEM_SLOT = reader.GetOrdinal(DbNames.ITEM_SLOT);
            int ordinalITEM_SIZEX = reader.GetOrdinal(DbNames.ITEM_SIZEX);
            int ordinalITEM_SIZEY = reader.GetOrdinal(DbNames.ITEM_SIZEY);

            while (reader.Read())
            {
                BaseItem b = null;

                int BType = reader.GetByte(ordinalITEM_BTYPE);
                int BKind = reader.GetByte(ordinalITEM_BKIND);



                if (BType == (byte)bType.Weapon || BType == (byte)bType.Clothes || BType == (byte)bType.Hat || BType == (byte)bType.Necklace
                    || BType == (byte)bType.Ring || BType == (byte)bType.Shoes || BType == (byte)bType.Cape)
                {


                    if (BKind == (byte)bKindWeapons.Sword && BType == (byte)bType.Weapon)
                    {
                        b = new Sword();
                    }
                    if (BKind == (byte)bKindWeapons.Blade && BType == (byte)bType.Weapon)
                    {
                        b = new Blade();
                    }
                    if (BKind == (byte)bKindWeapons.Fan && BType == (byte)bType.Weapon)
                    {
                        b = new Fan();
                    }
                    if (BKind == (byte)bKindWeapons.Brush && BType == (byte)bType.Weapon)
                    {
                        b = new Brush();
                    }
                    if (BKind == (byte)bKindWeapons.Claw && BType == (byte)bType.Weapon)
                    {
                        b = new Claw();
                    }
                    if (BKind == (byte)bKindWeapons.Axe && BType == (byte)bType.Weapon)
                    {
                        b = new Axe();
                    }
                    if (BKind == (byte)bKindWeapons.Talon && BType == (byte)bType.Weapon)
                    {
                        b = new Talon();
                    }
                    if (BKind == (byte)bKindWeapons.Tonfa && BType == (byte)bType.Weapon)
                    {
                        b = new Tonfa();
                    }
                    if(BKind == (byte)bKindWeapons.Hammer && BType == (byte)bType.Weapon)
                    {
                        b = new Hammer();
                    }
                    if (BKind == (byte)bKindArmors.SwordMan && BType == (byte)bType.Clothes)
                    {
                        b = new Clothes();
                    }
                    if (BKind == (byte)bKindArmors.Mage && BType == (byte)bType.Clothes)
                    {
                        b = new Dress();
                    }
                    if (BKind == (byte)bKindArmors.Warrior && BType == (byte)bType.Clothes)
                    {
                        b = new Armor();
                    }
                    if (BKind == (byte)bKindArmors.GhostFighter && BType == (byte)bType.Clothes)
                    {
                        b = new LeatherClothes();
                    }
                    if (BKind == (byte)bKindHats.SwordMan && BType == (byte)bType.Hat)
                    {
                        b = new Hood();
                    }
                    if (BKind == (byte)bKindHats.Mage && BType == (byte)bType.Hat)
                    {
                        b = new Tiara();
                    }
                    if (BKind == (byte)bKindHats.Warrior && BType == (byte)bType.Hat)
                    {
                        b = new Helmet();
                    }
                    if (BKind == (byte)bKindHats.GhostFighter && BType == (byte)bType.Hat)
                    {
                        b = new Hat();
                    }
                    if (BKind == (byte)bKindHats.SwordMan && BType == (byte)bType.Shoes)
                    {
                        b = new SmBoots();
                    }
                    if (BKind == (byte)bKindHats.Mage && BType == (byte)bType.Shoes)
                    {
                        b = new MageBoots();
                    }
                    if (BKind == (byte)bKindHats.Warrior && BType == (byte)bType.Shoes)
                    {
                        b = new WarriorShoes();
                    }
                    if (BKind == (byte)bKindHats.GhostFighter && BType == (byte)bType.Shoes)
                    {
                        b = new GhostFighterShoes();
                    }
                    if (BKind == 0 && BType == (byte)bType.Ring)
                    {
                        b = new Ring();
                    }
                    if (BKind == 0 && BType == (byte)bType.Necklace)
                    {
                        b = new Necklace();
                    }
                    if (BType == (byte)bType.Cape)
                    {
                        b = new Cape();
                        Cape c = b as Cape;
                        c.MaxPolishImbueTries = reader.GetInt16(ordinalITEM_MAXPOLISHTRIES);
                    }



                    Equipment e = b as Equipment;
                    e.RequiredLevel = reader.GetInt16(ordinalITEM_LEVEL);
                    e.RequiredDexterity = reader.GetInt16(ordinalITEM_DEX);
                    e.RequiredStrength = reader.GetInt16(ordinalITEM_STR);
                    e.RequiredStamina = reader.GetInt16(ordinalITEM_STA);
                    e.RequiredEnergy = reader.GetInt16(ordinalITEM_ENE);
                    e.Durability = reader.GetInt32(ordinalITEM_DURABILITY);
                    e.MaxDurability = e.Durability;
                    e.Damage = reader.GetInt16(ordinalITEM_DAMAGE);
                    e.Defence = reader.GetInt16(ordinalITEM_DEFENCE);
                    e.AttackRating = reader.GetInt16(ordinalITEM_ATTACKRATING);
                    e.AttackSpeed = reader.GetInt16(ordinalITEM_ATTACKSPEED);
                    e.AttackRange = reader.GetInt16(ordinalITEM_ATTACKRANGE);
                    e.IncMaxLife = reader.GetInt16(ordinalITEM_INCMAXLIFE);
                    e.IncMaxMana = reader.GetInt16(ordinalITEM_INCMAXMANA);
                    e.IncLifeRegen = reader.GetInt16(ordinalITEM_LIFEREGEN);
                    e.IncManaRegen = reader.GetInt16(ordinalITEM_MANAREGEN);
                    e.Critical = reader.GetInt16(ordinalITEM_CRITICAL);
                    e.MaxImbueTries = reader.GetByte(ordinalITEM_MAXIMBUETRIES);
                }

                if (BType == (byte)bType.ImbueItem)
                {
                    if (BKind == (byte)bKindStones.Black)
                    {
                        b = new Black();
                    }
                    if (BKind == (byte)bKindStones.White)
                    {
                        b = new White();
                    }
                    if (BKind == (byte)bKindStones.Red)
                    {
                        b = new Red();
                    }
                    if (BKind == (byte)bKindStones.Dragon)
                    {
                        b = new Dragon();
                    }

                    ImbueItem im = b as ImbueItem;
                    im.ImbueChance = reader.GetInt16(ordinalITEM_IMBUERATE);
                    im.IncreaseValue = reader.GetInt16(ordinalITEM_IMBUEINCREASE);
                }

                if (BType == (byte)bType.Potion)
                {
                    if (BKind == (byte)bKindPotions.Normal)
                    {
                        b = new Potion();
                    }
                    if (BKind == (byte)bKindPotions.Elixir)
                    {
                        b = new Elixir();
                    }

                    PotionItem pot = b as PotionItem;
                    pot.HealHp = reader.GetInt16(ordinalITEM_INCMAXLIFE);
                    pot.HealMana = reader.GetInt16(ordinalITEM_INCMAXMANA);
                }
                if (BType == (byte)bType.Book)
                {
                    if (BKind == (byte)bKindBooks.SoftBook)
                    {
                        b = new SoftBook();
                    }
                    if (BKind == (byte)bKindBooks.HardBook)
                    {
                        b = new HardBook();
                    }

                    BookItem book = b as BookItem;
                    book.RequiredClass = reader.GetByte(ordinalITEM_CLASS);
                    book.RequiredLevel = reader.GetInt16(ordinalITEM_LEVEL);
                    book.SkillID = reader.GetInt32(ordinalITEM_BOOKSKILLID);
                    book.SkillLevel = reader.GetByte(ordinalITEM_BOOKSKILLLEVEL);
                    book.SkillData = reader.GetInt32(ordinalITEM_BOOKSKILLDATA);
                }
                if (BType == (byte)bType.Bead)
                {
                    if (BKind == (byte)bKindBeads.Normal)
                    {
                        b = new Bead();
                    }
                    BeadItem bead = b as BeadItem;
                    bead.ToMapID = reader.GetInt32(ordinalITEM_TOMAPID);
                }

                b.ReferenceID = reader.GetInt16(ordinalITEM_REFERENCEID);
                b.VisualID = reader.GetInt16(ordinalITEM_VISUALID);
                b.Bag = reader.GetByte(ordinalITEM_BAG);
                b.Slot = reader.GetByte(ordinalITEM_SLOT);
                b.bType = reader.GetByte(ordinalITEM_BTYPE);
                b.bKind = reader.GetByte(ordinalITEM_BKIND);
                b.RequiredClass = reader.GetByte(ordinalITEM_CLASS);
                b.Amount = reader.GetInt16(ordinalITEM_AMOUNT);
                b.SizeX = reader.GetByte(ordinalITEM_SIZEX);
                b.SizeY = reader.GetByte(ordinalITEM_SIZEY);
                b.Price = reader.GetInt32(ordinalITEM_PRICE);

                items.Add(b);
            }

            reader.Close();
            _db.Close();

            return items;
        }
    }
}