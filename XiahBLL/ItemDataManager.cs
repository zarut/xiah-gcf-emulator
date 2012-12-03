using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using System.Data.Common;
using System.Data;

namespace XiahBLL
{
    public class ItemDataManager : ManagerBase
    {
        public ItemDataManager(string conString, string providerName)
            : base(conString, providerName)
        {

        }

        public BaseItem GetItemByReferenceID(int refId)
        {
            DbParameter itemIdParameter = _db.CreateParameter(DbNames.GETITEMBYREFID_REFID_PARAMETER, refId);
            itemIdParameter.DbType = DbType.Int32;

            BaseItem b = null;

            _db.Open();

            DbDataReader reader = _db.ExcecuteReader(DbNames.GETITEMBYREFID_STOREDPROC, CommandType.StoredProcedure, itemIdParameter);

            int ordinalITEM_REFERENCEID = reader.GetOrdinal(DbNames.ITEM_REFERENCEID);
            int ordinalITEM_BTYPE = reader.GetOrdinal(DbNames.ITEM_BTYPE);
            int ordinalITEM_BKIND = reader.GetOrdinal(DbNames.ITEM_BKIND);
            int ordinalITEM_VISUALID = reader.GetOrdinal(DbNames.ITEM_VISUALID);
            int ordinalITEM_COST = reader.GetOrdinal(DbNames.ITEM_COST);
            int ordinalITEM_CLASS = reader.GetOrdinal(DbNames.ITEM_CLASS);
            int ordinalITEM_LEVEL = reader.GetOrdinal(DbNames.ITEM_LEVEL);
            int ordinalITEM_DEX = reader.GetOrdinal(DbNames.ITEM_DEX);
            int ordinalITEM_STR = reader.GetOrdinal(DbNames.ITEM_STR);
            int ordinalITEM_STA = reader.GetOrdinal(DbNames.ITEM_STA);
            int ordinalITEM_ENE = reader.GetOrdinal(DbNames.ITEM_ENE);
            int ordinalITEM_MAXIMBUES = reader.GetOrdinal(DbNames.ITEM_MAXIMBUES);
            int ordinalITEM_MAXDURA = reader.GetOrdinal(DbNames.DROPITEM_DURABILITY);
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
            int ordinalITEM_POLISHTRIES = reader.GetOrdinal(DbNames.ITEM_POLISHTRIES);
            int ordinalITEM_DAMAGEABSORB = reader.GetOrdinal(DbNames.ITEM_DAMAGEABSORB);
            int ordinalITEM_DEFENSEABSORB = reader.GetOrdinal(DbNames.ITEM_DEFENSEABSORB);
            int ordinalITEM_ATTACKRATINGABSORB = reader.GetOrdinal(DbNames.ITEM_ATTACKRATINGABSORB);
            int ordinalITEM_LIFEABSORB = reader.GetOrdinal(DbNames.ITEM_LIFEABSORB);
            int ordinalITEM_SIZEX = reader.GetOrdinal(DbNames.ITEM_SIZEX);
            int ordinalITEM_SIZEY = reader.GetOrdinal(DbNames.ITEM_SIZEY);

            while (reader.Read())
            {
                int BType = reader.GetByte(ordinalITEM_BTYPE);
                int BKind = reader.GetByte(ordinalITEM_BKIND);

                if (BType == (byte)bType.Weapon || BType == (byte)bType.Clothes || BType == (byte)bType.Hat || BType == (byte)bType.Necklace
                    || BType == (byte)bType.Ring || BType == (byte)bType.Shoes || BType == (byte)bType.Cape || BType == (byte)bType.Mirror)
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
                        c.PolishImbueTries = reader.GetByte(ordinalITEM_POLISHTRIES);
                    }
                    if (BType == (byte)bType.Mirror)
                    {
                        // bkind 4 = mirror, 0 = jar
                        b = new Mirror();
                        Mirror m = b as Mirror;

                        m.LifeAbsorb = reader.GetInt16(ordinalITEM_LIFEABSORB);
                        m.DamageAbsorb = reader.GetInt16(ordinalITEM_DAMAGEABSORB);
                        m.DefenseAbsorb = reader.GetInt16(ordinalITEM_DEFENSEABSORB);
                        m.AttackRatingAbsorb = reader.GetInt16(ordinalITEM_ATTACKRATINGABSORB);
                    }

                    Equipment e = b as Equipment;
                    e.RequiredLevel = reader.GetInt16(ordinalITEM_LEVEL);
                    e.RequiredDexterity = reader.GetInt16(ordinalITEM_DEX);
                    e.RequiredStrength = reader.GetInt16(ordinalITEM_STR);
                    e.RequiredStamina = reader.GetInt16(ordinalITEM_STA);
                    e.RequiredEnergy = reader.GetInt16(ordinalITEM_ENE);
                    e.MaxImbueTries = reader.GetByte(ordinalITEM_MAXIMBUES);
                    e.Durability = reader.GetInt32(ordinalITEM_MAXDURA);
                    e.MaxDurability = reader.GetInt32(ordinalITEM_MAXDURA);
                    e.Damage = reader.GetInt32(ordinalITEM_DAMAGE);
                    e.Defence = reader.GetInt32(ordinalITEM_DEFENCE);
                    e.AttackRating = reader.GetInt32(ordinalITEM_ATTACKRATING);
                    e.AttackSpeed = reader.GetInt16(ordinalITEM_ATTACKSPEED);
                    e.AttackRange = reader.GetInt16(ordinalITEM_ATTACKRANGE);
                    e.IncMaxLife = reader.GetInt16(ordinalITEM_INCMAXLIFE);
                    e.IncMaxMana = reader.GetInt16(ordinalITEM_INCMAXMANA);
                    e.IncLifeRegen = reader.GetInt16(ordinalITEM_LIFEREGEN);
                    e.IncManaRegen = reader.GetInt16(ordinalITEM_MANAREGEN);
                    e.Critical = reader.GetInt16(ordinalITEM_CRITICAL);
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
                    if (BKind == (byte)bKindStones.RbItem)
                    {
                        b = new RbHoleItem();
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
                    if (BKind == (byte)bKindBooks.RebirdBook)
                    {
                        b = new RebirthBook();
                    }
                    if (BKind == (byte)bKindBooks.FourthBook)
                    {
                        b = new FourthBook();
                    }
                    if (BKind == (byte)bKindBooks.FeSkillBook)
                    {
                        b = new FeSkillBook();
                    }
                    if (BKind == (byte)bKindBooks.FeBook)
                    {
                        b = new FiveElementBook();
                    }
                    if (BKind == (byte)bKindBooks.FocusBook)
                    {
                        b = new FocusBook();
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
                if (BType == (byte)bType.StoreTag)
                {
                    b = new StoreTag();

                    StoreTag tag = b as StoreTag;
                    tag.TimeLeft = reader.GetInt16(ordinalITEM_MAXDURA);
                    tag.TimeMax = reader.GetInt16(ordinalITEM_MAXDURA);
                }
                if (BType == (byte)bType.Jeon)
                {
                    b = new Jeon();
                }
                if (BType == (byte)bType.PetItem)
                {
                    if (BKind == (byte)bKindPetItems.Taming)
                        b = new TameItem();
                    if (BKind == (byte)bKindPetItems.Food)
                        b = new PetFood();
                    if (BKind == (byte)bKindPetItems.Potion)
                        b = new PetPotion();
                    if (BKind == (byte)bKindPetItems.Resurect)
                        b = new PetResurrectItem();

                    PetItem p = b as PetItem;
                    p.TameChance = reader.GetInt16(ordinalITEM_IMBUERATE);
                    p.DecreaseWildness = reader.GetInt16(ordinalITEM_IMBUEINCREASE);
                    p.HealLife = reader.GetInt16(ordinalITEM_INCMAXLIFE);
                }
                if (BType == (byte)bType.Pill)
                {
                    if (BKind == (byte)bKindPills.Rebirth)
                        b = new RebirthPill();

                    RebirthPill p = b as RebirthPill;
                    p.RequiredLevel = reader.GetInt16(ordinalITEM_LEVEL);
                    p.RequiredRebirth = reader.GetByte(ordinalITEM_CLASS);
                    p.ToRebirth = (byte)(p.RequiredRebirth + 1);
                    p.IncreaseSp = reader.GetInt16(ordinalITEM_DEX);
                }

                b.ItemID = 0;
                b.OwnerID = 0;
                b.ReferenceID = reader.GetInt16(ordinalITEM_REFERENCEID);
                b.VisualID = reader.GetInt16(ordinalITEM_VISUALID);
                b.Bag = 0;
                b.Slot = 0;
                b.bType = reader.GetByte(ordinalITEM_BTYPE);
                b.bKind = reader.GetByte(ordinalITEM_BKIND);
                b.RequiredClass = reader.GetByte(ordinalITEM_CLASS);
                b.Amount = 1;
                b.SizeX = reader.GetByte(ordinalITEM_SIZEX);
                b.SizeY = reader.GetByte(ordinalITEM_SIZEY);
                b.Price = reader.GetInt32(ordinalITEM_COST);
            }

            reader.Close();
            _db.Close();

            return b;
        }


        public BaseItem GetItemByItemID(int itemID)
        {
            DbParameter itemIdParameter = _db.CreateParameter(DbNames.GETITEMBYITEMID_ITEMID_PARAMETER, itemID);
            itemIdParameter.DbType = DbType.Int32;

            BaseItem b = null;

            _db.Open();

            DbDataReader reader = _db.ExcecuteReader(DbNames.GETITEMBYITEMID_STOREDPROC, CommandType.StoredProcedure, itemIdParameter);

            int ordinalITEM_ITEMID = reader.GetOrdinal(DbNames.ITEM_ITEMID);
            int ordinalITEM_OWNERID = reader.GetOrdinal(DbNames.ITEM_OWNERID);
            int ordinalITEM_REFERENCEID = reader.GetOrdinal(DbNames.ITEM_REFERENCEID);
            int ordinalITEM_BTYPE = reader.GetOrdinal(DbNames.ITEM_BTYPE);
            int ordinalITEM_BKIND = reader.GetOrdinal(DbNames.ITEM_BKIND);
            int ordinalITEM_VISUALID = reader.GetOrdinal(DbNames.ITEM_VISUALID);
            int ordinalITEM_COST = reader.GetOrdinal(DbNames.ITEM_COST);
            int ordinalITEM_CLASS = reader.GetOrdinal(DbNames.ITEM_CLASS);
            int ordinalITEM_AMOUNT = reader.GetOrdinal(DbNames.ITEM_AMOUNT);
            int ordinalITEM_LEVEL = reader.GetOrdinal(DbNames.ITEM_LEVEL);
            int ordinalITEM_DEX = reader.GetOrdinal(DbNames.ITEM_DEX);
            int ordinalITEM_STR = reader.GetOrdinal(DbNames.ITEM_STR);
            int ordinalITEM_STA = reader.GetOrdinal(DbNames.ITEM_STA);
            int ordinalITEM_ENE = reader.GetOrdinal(DbNames.ITEM_ENE);
            int ordinalITEM_MAXIMBUES = reader.GetOrdinal(DbNames.ITEM_MAXIMBUES);
            int ordinalITEM_MAXDURA = reader.GetOrdinal(DbNames.ITEM_MAXDURA);
            int ordinalITEM_CURDURA = reader.GetOrdinal(DbNames.ITEM_CURDURA);
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
            int ordinalITEM_PLUS = reader.GetOrdinal(DbNames.ITEM_PLUS);
            int ordinalITEM_SLVL = reader.GetOrdinal(DbNames.ITEM_SLVL);
            int ordinalITEM_IMBUETRIES = reader.GetOrdinal(DbNames.ITEM_IMBUETRIES);
            int ordinalITEM_DRAGONSUCCESSIMBUETRIES = reader.GetOrdinal(DbNames.ITEM_DRAGONSUCCESSIMBUETRIES);
            int ordinalITEM_DISCOUNTREPAIRFEE = reader.GetOrdinal(DbNames.ITEM_DISCOUNTREPAIRFEE);
            int ordinalITEM_TOTALDRAGONIMBUES = reader.GetOrdinal(DbNames.ITEM_TOTALDRAGONIMBUES);
            int ordinalITEM_DRAGONDAMAGE = reader.GetOrdinal(DbNames.ITEM_DRAGONDAMAGE);
            int ordinalITEM_DRAGONDEFENCE = reader.GetOrdinal(DbNames.ITEM_DRAGONDEFENCE);
            int ordinalITEM_DRAGONATTACKRATING = reader.GetOrdinal(DbNames.ITEM_DRAGONATTACKRATING);
            int ordinalITEM_DRAGONLIFE = reader.GetOrdinal(DbNames.ITEM_DRAGONLIFE);
            int ordinalITEM_MAPPEDSTUFF = reader.GetOrdinal(DbNames.ITEM_MAPPEDSTUFF);
            int ordinalITEM_FORCENUMBER = reader.GetOrdinal(DbNames.ITEM_FORCENUMBER);
            int ordinalITEM_REBIRTHHOLE = reader.GetOrdinal(DbNames.ITEM_REBIRTHHOLE);
            int ordinalITEM_REBIRTHHOLESTAT = reader.GetOrdinal(DbNames.ITEM_REBIRTHHOLESTAT);
            int ordinalITEM_TOMAPID = reader.GetOrdinal(DbNames.ITEM_TOMAPID);
            int ordinalITEM_IMBUERATE = reader.GetOrdinal(DbNames.ITEM_IMBUERATE);
            int ordinalITEM_IMBUEINCREASE = reader.GetOrdinal(DbNames.ITEM_IMBUEINCREASE);
            int ordinalITEM_IMBUEDATA = reader.GetOrdinal(DbNames.ITEM_IMBUEDATA);
            int ordinalITEM_BOOKSKILLID = reader.GetOrdinal(DbNames.ITEM_BOOKSKILLID);
            int ordinalITEM_BOOKSKILLLEVEL = reader.GetOrdinal(DbNames.ITEM_BOOKSKILLLEVEL);
            int ordinalITEM_BOOKSKILLDATA = reader.GetOrdinal(DbNames.ITEM_BOOKSKILLDATA);
            int ordinalITEM_MAXPOLISHTRIES = reader.GetOrdinal(DbNames.ITEM_MAXPOLISHTRIES);
            int ordinalITEM_POLISHTRIES = reader.GetOrdinal(DbNames.ITEM_POLISHTRIES);
            int ordinalITEM_VIGISTAT1 = reader.GetOrdinal(DbNames.ITEM_VIGISTAT1);
            int ordinalITEM_VIGISTAT2 = reader.GetOrdinal(DbNames.ITEM_VIGISTAT2);
            int ordinalITEM_VIGISTAT3 = reader.GetOrdinal(DbNames.ITEM_VIGISTAT3);
            int ordinalITEM_VIGISTAT4 = reader.GetOrdinal(DbNames.ITEM_VIGISTAT4);
            int ordinalITEM_VIGISTATADD1 = reader.GetOrdinal(DbNames.ITEM_VIGISTATADD1);
            int ordinalITEM_VIGISTATADD2 = reader.GetOrdinal(DbNames.ITEM_VIGISTATADD2);
            int ordinalITEM_VIGISTATADD3 = reader.GetOrdinal(DbNames.ITEM_VIGISTATADD3);
            int ordinalITEM_VIGISTATADD4 = reader.GetOrdinal(DbNames.ITEM_VIGISTATADD4);
            int ordinalITEM_PETID = reader.GetOrdinal(DbNames.ITEM_PETID);
            int ordinalITEM_DAMAGEABSORB = reader.GetOrdinal(DbNames.ITEM_DAMAGEABSORB);
            int ordinalITEM_DEFENSEABSORB = reader.GetOrdinal(DbNames.ITEM_DEFENSEABSORB);
            int ordinalITEM_ATTACKRATINGABSORB = reader.GetOrdinal(DbNames.ITEM_ATTACKRATINGABSORB);
            int ordinalITEM_LIFEABSORB = reader.GetOrdinal(DbNames.ITEM_LIFEABSORB);
            int ordinalITEM_BAG = reader.GetOrdinal(DbNames.ITEM_BAG);
            int ordinalITEM_SLOT = reader.GetOrdinal(DbNames.ITEM_SLOT);
            int ordinalITEM_SIZEX = reader.GetOrdinal(DbNames.ITEM_SIZEX);
            int ordinalITEM_SIZEY = reader.GetOrdinal(DbNames.ITEM_SIZEY);

            while (reader.Read())
            {
                int BType = reader.GetByte(ordinalITEM_BTYPE);
                int BKind = reader.GetByte(ordinalITEM_BKIND);

                if (BType == (byte)bType.Weapon || BType == (byte)bType.Clothes || BType == (byte)bType.Hat || BType == (byte)bType.Necklace
                    || BType == (byte)bType.Ring || BType == (byte)bType.Shoes || BType == (byte)bType.Cape || BType == (byte)bType.Mirror)
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
                        c.PolishImbueTries = reader.GetByte(ordinalITEM_POLISHTRIES);
                        c.VigiStat1 = reader.GetInt16(ordinalITEM_VIGISTAT1);
                        c.VigiStatAdd1 = reader.GetInt16(ordinalITEM_VIGISTATADD1);
                        c.VigiStat2 = reader.GetInt16(ordinalITEM_VIGISTAT2);
                        c.VigiStatAdd2 = reader.GetInt16(ordinalITEM_VIGISTATADD2);
                        c.VigiStat3 = reader.GetInt16(ordinalITEM_VIGISTAT3);
                        c.VigiStatAdd3 = reader.GetInt16(ordinalITEM_VIGISTATADD3);
                        c.VigiStat4 = reader.GetInt16(ordinalITEM_VIGISTAT4);
                        c.VigiStatAdd4 = reader.GetInt16(ordinalITEM_VIGISTATADD4);
                    }
                    if (BType == (byte)bType.Mirror)
                    {
                        // bkind 4 = mirror, 0 = jar
                        b = new Mirror();
                        Mirror m = b as Mirror;

                        m.PetID = reader.GetInt32(ordinalITEM_PETID);
                        m.LifeAbsorb = reader.GetInt16(ordinalITEM_LIFEABSORB);
                        m.DamageAbsorb = reader.GetInt16(ordinalITEM_DAMAGEABSORB);
                        m.DefenseAbsorb = reader.GetInt16(ordinalITEM_DEFENSEABSORB);
                        m.AttackRatingAbsorb = reader.GetInt16(ordinalITEM_ATTACKRATINGABSORB);
                    }


                    Equipment e = b as Equipment;
                    e.RequiredLevel = reader.GetInt16(ordinalITEM_LEVEL);
                    e.RequiredDexterity = reader.GetInt16(ordinalITEM_DEX);
                    e.RequiredStrength = reader.GetInt16(ordinalITEM_STR);
                    e.RequiredStamina = reader.GetInt16(ordinalITEM_STA);
                    e.RequiredEnergy = reader.GetInt16(ordinalITEM_ENE);
                    e.MaxImbueTries = reader.GetByte(ordinalITEM_MAXIMBUES);
                    e.Durability = reader.GetInt16(ordinalITEM_CURDURA);
                    e.MaxDurability = reader.GetInt16(ordinalITEM_MAXDURA);
                    e.Damage = reader.GetInt32(ordinalITEM_DAMAGE);
                    e.Defence = reader.GetInt32(ordinalITEM_DEFENCE);
                    e.AttackRating = reader.GetInt32(ordinalITEM_ATTACKRATING);
                    e.AttackSpeed = reader.GetInt16(ordinalITEM_ATTACKSPEED);
                    e.AttackRange = reader.GetInt16(ordinalITEM_ATTACKRANGE);
                    e.IncMaxLife = reader.GetInt16(ordinalITEM_INCMAXLIFE);
                    e.IncMaxMana = reader.GetInt16(ordinalITEM_INCMAXMANA);
                    e.IncLifeRegen = reader.GetInt16(ordinalITEM_LIFEREGEN);
                    e.IncManaRegen = reader.GetInt16(ordinalITEM_MANAREGEN);
                    e.Critical = reader.GetInt16(ordinalITEM_CRITICAL);
                    e.Plus = reader.GetByte(ordinalITEM_PLUS);
                    e.Slvl = reader.GetByte(ordinalITEM_SLVL);
                    e.ImbueTries = reader.GetByte(ordinalITEM_IMBUETRIES);
                    e.DragonSuccessImbueTries = reader.GetInt16(ordinalITEM_DRAGONSUCCESSIMBUETRIES);
                    e.DiscountRepairFee = reader.GetByte(ordinalITEM_DISCOUNTREPAIRFEE);
                    e.TotalDragonImbueTries = reader.GetInt16(ordinalITEM_TOTALDRAGONIMBUES);
                    e.DragonDamage = reader.GetInt32(ordinalITEM_DRAGONDAMAGE);
                    e.DragonDefence = reader.GetInt32(ordinalITEM_DRAGONDEFENCE);
                    e.DragonAttackRating = reader.GetInt32(ordinalITEM_DRAGONATTACKRATING);
                    e.DragonLife = reader.GetInt16(ordinalITEM_DRAGONLIFE);
                    e.MappedData = reader.GetByte(ordinalITEM_MAPPEDSTUFF);
                    e.ForceSlot = reader.GetByte(ordinalITEM_FORCENUMBER);
                    e.RebirthHole = reader.GetInt16(ordinalITEM_REBIRTHHOLE);
                    e.RebirthHoleStat = reader.GetInt16(ordinalITEM_REBIRTHHOLESTAT);
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
                    if (BKind == (byte)bKindStones.RbItem)
                    {
                        b = new RbHoleItem();
                    }

                    ImbueItem im = b as ImbueItem;
                    im.ImbueChance = reader.GetInt16(ordinalITEM_IMBUERATE);
                    im.IncreaseValue = reader.GetInt16(ordinalITEM_IMBUEINCREASE);
                    im.ImbueData = reader.GetByte(ordinalITEM_IMBUEDATA);

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
                    if (BKind == (byte)bKindBooks.RebirdBook)
                    {
                        b = new RebirthBook();
                    }
                    if (BKind == (byte)bKindBooks.FourthBook)
                    {
                        b = new FourthBook();
                    }
                    if (BKind == (byte)bKindBooks.FeSkillBook)
                    {
                        b = new FeSkillBook();
                    }
                    if (BKind == (byte)bKindBooks.FeBook)
                    {
                        b = new FiveElementBook();
                    }
                    if (BKind == (byte)bKindBooks.FocusBook)
                    {
                        b = new FocusBook();
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
                if (BType == (byte)bType.StoreTag)
                {
                    b = new StoreTag();

                    StoreTag tag = b as StoreTag;
                    tag.TimeLeft = reader.GetInt16(ordinalITEM_CURDURA);
                    tag.TimeMax = reader.GetInt16(ordinalITEM_MAXDURA);
                }
                if (BType == (byte)bType.PetItem)
                {
                    if (BKind == (byte)bKindPetItems.Taming)
                        b = new TameItem();
                    if (BKind == (byte)bKindPetItems.Food)
                        b = new PetFood();
                    if (BKind == (byte)bKindPetItems.Potion)
                        b = new PetPotion();
                    if (BKind == (byte)bKindPetItems.Resurect)
                        b = new PetResurrectItem();

                    PetItem p = b as PetItem;
                    p.TameChance = reader.GetInt16(ordinalITEM_IMBUERATE);
                    p.DecreaseWildness = reader.GetInt16(ordinalITEM_IMBUEINCREASE);
                    p.HealLife = reader.GetInt16(ordinalITEM_INCMAXLIFE);
                }
                if (BType == (byte)bType.Pill)
                {
                    if (BKind == (byte)bKindPills.Rebirth)
                        b = new RebirthPill();

                    RebirthPill p = b as RebirthPill;
                    p.RequiredLevel = reader.GetInt16(ordinalITEM_LEVEL);
                    p.RequiredRebirth = reader.GetByte(ordinalITEM_CLASS);
                    p.ToRebirth = (byte)(p.RequiredRebirth + 1);
                    p.IncreaseSp = reader.GetInt16(ordinalITEM_DEX);
                }

                b.ItemID = reader.GetInt32(ordinalITEM_ITEMID);
                b.OwnerID = reader.GetInt32(ordinalITEM_OWNERID);
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
                b.Price = reader.GetInt32(ordinalITEM_COST);
            }

            reader.Close();
            _db.Close();

            return b;
        }

        public void UpdateItem(BaseItem item)
        {
            DbParameter OwnerIdParameter = _db.CreateParameter(DbNames.UPDATEITEM_OWNERID_PARAMETER, DbType.Int32);
            OwnerIdParameter.Value = item.OwnerID;

            DbParameter CostParameter = _db.CreateParameter(DbNames.UPDATEITEM_COST_PARAMETER, DbType.Int32);
            CostParameter.Value = item.Price;

            DbParameter ReqClassParameter = _db.CreateParameter(DbNames.UPDATEITEM_REQCLASS_PARAMETER, DbType.Byte);
            ReqClassParameter.Value = item.RequiredClass;

            DbParameter AmountParameter = _db.CreateParameter(DbNames.UPDATEITEM_AMOUNT_PARAMETER, DbType.Int16);
            AmountParameter.Value = item.Amount;

            DbParameter BagParameter = _db.CreateParameter(DbNames.UPDATEITEM_BAG_PARAMETER, DbType.Byte);
            BagParameter.Value = item.Bag;

            DbParameter SlotParameter = _db.CreateParameter(DbNames.UPDATEITEM_SLOT_PARAMETER, DbType.Byte);
            SlotParameter.Value = item.Slot;

            DbParameter ItemIdParameter = _db.CreateParameter(DbNames.UPDATEITEM_ITEMID_PARAMETER, DbType.Int32);
            ItemIdParameter.Value = item.ItemID;

            DbParameter ReqLevelParameter = _db.CreateParameter(DbNames.UPDATEITEM_REQLEVEL_PARAMETER, DbType.Int16);
            ReqLevelParameter.Value = 0;

            DbParameter ReqDexParameter = _db.CreateParameter(DbNames.UPDATEITEM_REQDEX_PARAMETER, DbType.Int16);
            ReqDexParameter.Value = 0;

            DbParameter ReqStrParameter = _db.CreateParameter(DbNames.UPDATEITEM_REQSTR_PARAMETER, DbType.Int16);
            ReqStrParameter.Value = 0;

            DbParameter ReqStaParameter = _db.CreateParameter(DbNames.UPDATEITEM_REQSTA_PARAMETER, DbType.Int16);
            ReqStaParameter.Value = 0;

            DbParameter ReqEneParameter = _db.CreateParameter(DbNames.UPDATEITEM_REQENE_PARAMETER, DbType.Int16);
            ReqEneParameter.Value = 0;

            DbParameter MaxImbueTriesParameter = _db.CreateParameter(DbNames.UPDATEITEM_MAXIMBUETRIES_PARAMETER, DbType.Byte);
            MaxImbueTriesParameter.Value = 200;

            DbParameter MaxDuraParameter = _db.CreateParameter(DbNames.UPDATEITEM_MAXDURA_PARAMETER, DbType.Int16);
            MaxDuraParameter.Value = 0;

            DbParameter CurDuraParameter = _db.CreateParameter(DbNames.UPDATEITEM_CURDURA_PARAMETER, DbType.Int16);
            CurDuraParameter.Value = 0;

            DbParameter DamageParameter = _db.CreateParameter(DbNames.UPDATEITEM_DAMAGE_PARAMETER, DbType.Int32);
            DamageParameter.Value = 0;

            DbParameter DefenceParameter = _db.CreateParameter(DbNames.UPDATEITEM_DEFENCE_PARAMETER, DbType.Int32);
            DefenceParameter.Value = 0;

            DbParameter AttackRatingParameter = _db.CreateParameter(DbNames.UPDATEITEM_ATTACKRATING_PARAMETER, DbType.Int32);
            AttackRatingParameter.Value = 0;

            DbParameter AttackSpeedParameter = _db.CreateParameter(DbNames.UPDATEITEM_ATTACKSPEED_PARAMETER, DbType.Int16);
            AttackSpeedParameter.Value = 0;

            DbParameter AttackRangeParameter = _db.CreateParameter(DbNames.UPDATEITEM_ATTACKRANGE_PARAMETER, DbType.Int16);
            AttackRangeParameter.Value = 0;

            DbParameter IncMaxLifeParameter = _db.CreateParameter(DbNames.UPDATEITEM_INCMAXLIFE_PARAMETER, DbType.Int16);
            IncMaxLifeParameter.Value = 0;

            DbParameter IncMaxManaParameter = _db.CreateParameter(DbNames.UPDATEITEM_INCMAXMANA_PARAMETER, DbType.Int16);
            IncMaxManaParameter.Value = 0;

            DbParameter LifeRegenParameter = _db.CreateParameter(DbNames.UPDATEITEM_LIFEREGEN_PARAMETER, DbType.Int16);
            LifeRegenParameter.Value = 0;

            DbParameter ManaRegenParameter = _db.CreateParameter(DbNames.UPDATEITEM_MANAREGEN_PARAMETER, DbType.Int16);
            ManaRegenParameter.Value = 0;

            DbParameter CriticalHitParameter = _db.CreateParameter(DbNames.UPDATEITEM_CRITICALHIT_PARAMETER, DbType.Int16);
            CriticalHitParameter.Value = 0;

            DbParameter PlusParameter = _db.CreateParameter(DbNames.UPDATEITEM_PLUS_PARAMETER, DbType.Byte);
            PlusParameter.Value = 0;

            DbParameter SlvlParameter = _db.CreateParameter(DbNames.UPDATEITEM_SLVL_PARAMETER, DbType.Byte);
            SlvlParameter.Value = 0;

            DbParameter ImbueTriesParameter = _db.CreateParameter(DbNames.UPDATEITEM_IMBUETRIES_PARAMETER, DbType.Byte);
            ImbueTriesParameter.Value = 0;

            DbParameter DragonSuccessImbueTriesParameter = _db.CreateParameter(DbNames.UPDATEITEM_DRAGONSUCCESSIMBUETRIES_PARAMETER, DbType.Int16);
            DragonSuccessImbueTriesParameter.Value = 0;

            DbParameter DiscountRepairFeeParameter = _db.CreateParameter(DbNames.UPDATEITEM_DISCOUNTREPAIRFEE_PARAMETER, DbType.Byte);
            DiscountRepairFeeParameter.Value = 0;

            DbParameter TotalDragonImbueTriesParameter = _db.CreateParameter(DbNames.UPDATEITEM_TOTALDRAGONIMBUETRIES_PARAMETER, DbType.Int16);
            TotalDragonImbueTriesParameter.Value = 0;

            DbParameter DragonDmgParameter = _db.CreateParameter(DbNames.UPDATEITEM_DRAGONDMG_PARAMETER, DbType.Int32);
            DragonDmgParameter.Value = 0;

            DbParameter DragonDefParameter = _db.CreateParameter(DbNames.UPDATEITEM_DRAGONDEF_PARAMETER, DbType.Int32);
            DragonDefParameter.Value = 0;

            DbParameter DragonARParameter = _db.CreateParameter(DbNames.UPDATEITEM_DRAGONAR_PARAMETER, DbType.Int32);
            DragonARParameter.Value = 0;

            DbParameter DragonLifeParameter = _db.CreateParameter(DbNames.UPDATEITEM_DRAGONLIFE_PARAMETER, DbType.Int16);
            DragonLifeParameter.Value = 0;
            
            DbParameter MappedStuffParameter = _db.CreateParameter(DbNames.UPDATEITEM_MAPPEDSTUFF_PARAMETER, DbType.Byte);
            MappedStuffParameter.Value = 0;

            DbParameter ForceNumberParameter = _db.CreateParameter(DbNames.UPDATEITEM_FORCENUMBER_PARAMETER, DbType.Byte);
            ForceNumberParameter.Value = 0;

            DbParameter RebirthHoleParameter = _db.CreateParameter(DbNames.UPDATEITEM_REBIRTHHOLE_PARAMETER, DbType.Int16);
            RebirthHoleParameter.Value = 0;

            DbParameter RebirthHoleStatParameter = _db.CreateParameter(DbNames.UPDATEITEM_REBIRTHHOLESTAT_PARAMETER, DbType.Int16);
            RebirthHoleStatParameter.Value = 0;

            DbParameter PolishStoneTriesParameter = _db.CreateParameter(DbNames.UPDATEITEM_POLISHSTONETRIES_PARAMETER, DbType.Byte);
            PolishStoneTriesParameter.Value = 0;
            DbParameter VigiStat1Parameter = _db.CreateParameter(DbNames.UPDATEITEM_VIGISTAT1_PARAMETER, DbType.Int16);
            VigiStat1Parameter.Value = 0;
            DbParameter VigiStat2Parameter = _db.CreateParameter(DbNames.UPDATEITEM_VIGISTAT2_PARAMETER, DbType.Int16);
            VigiStat2Parameter.Value = 0;
            DbParameter VigiStat3Parameter = _db.CreateParameter(DbNames.UPDATEITEM_VIGISTAT3_PARAMETER, DbType.Int16);
            VigiStat3Parameter.Value = 0;
            DbParameter VigiStat4Parameter = _db.CreateParameter(DbNames.UPDATEITEM_VIGISTAT4_PARAMETER, DbType.Int16);
            VigiStat4Parameter.Value = 0;
            DbParameter VigiStatAdd1Parameter = _db.CreateParameter(DbNames.UPDATEITEM_VIGISTATADD1_PARAMETER, DbType.Int16);
            VigiStatAdd1Parameter.Value = 0;
            DbParameter VigiStatAdd2Parameter = _db.CreateParameter(DbNames.UPDATEITEM_VIGISTATADD2_PARAMETER, DbType.Int16);
            VigiStatAdd2Parameter.Value = 0;
            DbParameter VigiStatAdd3Parameter = _db.CreateParameter(DbNames.UPDATEITEM_VIGISTATADD3_PARAMETER, DbType.Int16);
            VigiStatAdd3Parameter.Value = 0;
            DbParameter VigiStatAdd4Parameter = _db.CreateParameter(DbNames.UPDATEITEM_VIGISTATADD4_PARAMETER, DbType.Int16);
            VigiStatAdd4Parameter.Value = 0;
            DbParameter PetIDParameter = _db.CreateParameter(DbNames.UPDATEITEM_PETID_PARAMETER, DbType.Int32);
            PetIDParameter.Value = 0;

            if (item is Equipment)
            {
                Equipment e = item as Equipment;
                ReqLevelParameter.Value = e.RequiredLevel;
                ReqDexParameter.Value = e.RequiredDexterity;
                ReqStrParameter.Value = e.RequiredStrength;
                ReqStaParameter.Value = e.RequiredStamina;
                ReqEneParameter.Value = e.RequiredEnergy;
                MaxImbueTriesParameter.Value = e.MaxImbueTries;
                MaxDuraParameter.Value = e.MaxDurability;
                CurDuraParameter.Value = e.Durability;
                DamageParameter.Value = e.Damage;
                DefenceParameter.Value = e.Defence;
                AttackRatingParameter.Value = e.AttackRating;
                AttackSpeedParameter.Value = e.AttackSpeed;
                AttackRangeParameter.Value = e.AttackRange;
                IncMaxLifeParameter.Value = e.IncMaxLife;
                IncMaxManaParameter.Value = e.IncMaxMana;
                LifeRegenParameter.Value = e.IncLifeRegen;
                ManaRegenParameter.Value = e.IncManaRegen;
                CriticalHitParameter.Value = e.Critical;
                PlusParameter.Value = e.Plus;
                SlvlParameter.Value = e.Slvl;
                ImbueTriesParameter.Value = e.ImbueTries;
                DragonSuccessImbueTriesParameter.Value = e.DragonSuccessImbueTries;
                DiscountRepairFeeParameter.Value = e.DiscountRepairFee;
                TotalDragonImbueTriesParameter.Value = e.TotalDragonImbueTries;
                DragonDmgParameter.Value = e.DragonDamage;
                DragonDefParameter.Value = e.DragonDefence;
                DragonARParameter.Value = e.DragonAttackRating;
                DragonLifeParameter.Value = e.DragonLife;
                MappedStuffParameter.Value = e.MappedData;
                ForceNumberParameter.Value = e.ForceSlot;
                RebirthHoleParameter.Value = e.RebirthHole;
                RebirthHoleStatParameter.Value = e.RebirthHoleStat;
                if (e is Cape)
                {
                    Cape c = e as Cape;
                    PolishStoneTriesParameter.Value = c.PolishImbueTries;
                    VigiStat1Parameter.Value = c.VigiStat1;
                    VigiStat2Parameter.Value = c.VigiStat2;
                    VigiStat3Parameter.Value = c.VigiStat3;
                    VigiStat4Parameter.Value = c.VigiStat4;
                    VigiStatAdd1Parameter.Value = c.VigiStatAdd1;
                    VigiStatAdd2Parameter.Value = c.VigiStatAdd2;
                    VigiStatAdd3Parameter.Value = c.VigiStatAdd3;
                    VigiStatAdd4Parameter.Value = c.VigiStatAdd4;
                }
                if (e is Mirror)
                { 
                    Mirror m = e as Mirror;
                    PetIDParameter.Value = m.PetID;
                }
            }

            DbParameter ImbueRateParameter = _db.CreateParameter(DbNames.UPDATEITEM_IMBUERATE_PARAMETER, DbType.Int16);
            ImbueRateParameter.Value = 0;
            DbParameter ImbueIncreaseParameter = _db.CreateParameter(DbNames.UPDATEITEM_IMBUEINCREASE_PARAMETER, DbType.Int16);
            ImbueIncreaseParameter.Value = 0;
            DbParameter ImbueData = _db.CreateParameter(DbNames.UPDATEITEM_IMBUEDATA_PARAMETER, DbType.Byte);
            ImbueData.Value = 0;

            if (item is ImbueItem)
            {
                ImbueItem i = item as ImbueItem;

                ImbueRateParameter.Value = i.ImbueChance;
                ImbueIncreaseParameter.Value = i.IncreaseValue;
                ImbueData.Value = i.ImbueData;
            }

            if (item is PotionItem)
            {
                PotionItem p = item as PotionItem;

                IncMaxLifeParameter.Value = p.HealHp;
                IncMaxManaParameter.Value = p.HealMana;
            }
            if (item is StoreTag)
            {
                StoreTag tag = item as StoreTag;
                CurDuraParameter.Value = tag.TimeLeft;
                MaxDuraParameter.Value = tag.TimeMax;
            }
            if (item is PetItem)
            {
                PetItem p = item as PetItem;

                ImbueRateParameter.Value = p.TameChance;
                ImbueIncreaseParameter.Value = p.DecreaseWildness;
                IncMaxLifeParameter.Value = p.HealLife;
            }

            _db.Open();

            _db.ExecuteNonQuery(DbNames.UPDATEITEM_STOREDPROC,
              System.Data.CommandType.StoredProcedure,
              OwnerIdParameter,
              CostParameter,
              ReqClassParameter,
              AmountParameter,
              BagParameter,
              SlotParameter,
              ItemIdParameter,
              ReqLevelParameter,
              ReqDexParameter,
              ReqStrParameter,
              ReqStaParameter,
              ReqEneParameter,
              MaxImbueTriesParameter,
              MaxDuraParameter,
              CurDuraParameter,
              DamageParameter,
              DefenceParameter,
              AttackRatingParameter,
              AttackSpeedParameter,
              AttackRangeParameter,
              IncMaxLifeParameter,
              IncMaxManaParameter,
              LifeRegenParameter,
              ManaRegenParameter,
              CriticalHitParameter,
              PlusParameter,
              SlvlParameter,
              ImbueTriesParameter,
              DragonSuccessImbueTriesParameter,
              DiscountRepairFeeParameter,
              TotalDragonImbueTriesParameter,
              DragonDmgParameter,
              DragonDefParameter,
              DragonARParameter,
              DragonLifeParameter,
              MappedStuffParameter,
              ForceNumberParameter,
              RebirthHoleParameter,
              RebirthHoleStatParameter,
              PolishStoneTriesParameter,
              VigiStat1Parameter,
              VigiStat2Parameter,
              VigiStat3Parameter,
              VigiStat4Parameter,
              VigiStatAdd1Parameter,
              VigiStatAdd2Parameter,
              VigiStatAdd3Parameter,
              VigiStatAdd4Parameter,
              PetIDParameter,
              ImbueIncreaseParameter,
              ImbueRateParameter,
              ImbueData
              );

            _db.Close();
        }

        public void DeleteItem(int ItemID)
        {
            DbParameter ItemIdParameter = _db.CreateParameter(DbNames.DELETEITEM_ITEMID_PARAMETER, ItemID);
            ItemIdParameter.DbType = System.Data.DbType.Int32;

            _db.Open();

            _db.ExecuteNonQuery(DbNames.DELETEITEM_STOREDPROC, System.Data.CommandType.StoredProcedure, ItemIdParameter);

            _db.Close();
        }

        public int InsertItem(BaseItem item)
        {
            DbParameter refIdParameter = _db.CreateParameter(DbNames.INSERTITEM_REFERENCEID_PARAMETER, item.ReferenceID);
            refIdParameter.DbType = DbType.Int16;

            DbParameter characterIdParameter = _db.CreateParameter(DbNames.INSERTITEM_CHARACTERID_PARAMETER, item.OwnerID);
            characterIdParameter.DbType = DbType.Int32;

            DbParameter amountParameter = _db.CreateParameter(DbNames.INSERTITEM_AMOUNT_PARAMETER, item.Amount);
            amountParameter.DbType = DbType.Int16;

            DbParameter bagParameter = _db.CreateParameter(DbNames.INSTERITEM_BAG_PARAMETER, item.Bag);
            bagParameter.DbType = DbType.Byte;

            DbParameter slotParameter = _db.CreateParameter(DbNames.INSERTITEM_SLOT_PARAMETER, item.Slot);
            slotParameter.DbType = DbType.Byte;

            int itemID = -1;

            DbParameter newIdParameter = _db.CreateParameter(DbNames.INSERTITEM_ITEMID_PARAMETER, itemID);
            newIdParameter.Direction = ParameterDirection.Output;

            _db.Open();

            _db.ExecuteNonQuery(DbNames.INSERTITEM_STOREDPROC,
              System.Data.CommandType.StoredProcedure,
              refIdParameter,
              characterIdParameter,
              amountParameter,
              bagParameter,
              slotParameter,
              newIdParameter);

            _db.Close();

            itemID = (int)newIdParameter.Value;

            return itemID;
        }

        public List<BaseItem> GetWarehouseItemsByAccountId(int accountId)
        {
            DbParameter accountIdParameter = _db.CreateParameter(DbNames.GETWAREHOUSUEITEMSBYACCOUNTID_ACCOUNT_ID_PARAMETER, accountId);
            accountIdParameter.DbType = DbType.Int32;

            List<BaseItem> items = new List<BaseItem>();

            _db.Open();

            DbDataReader reader = _db.ExcecuteReader(DbNames.GETWAREHOUSUEITEMSBYACCOUNTID_STOREDPROC, CommandType.StoredProcedure, accountIdParameter);

            int ordinalITEM_ITEMID = reader.GetOrdinal(DbNames.ITEM_ITEMID);
            int ordinalITEM_OWNERID = reader.GetOrdinal(DbNames.ITEM_OWNERID);
            int ordinalITEM_REFERENCEID = reader.GetOrdinal(DbNames.ITEM_REFERENCEID);
            int ordinalITEM_BTYPE = reader.GetOrdinal(DbNames.ITEM_BTYPE);
            int ordinalITEM_BKIND = reader.GetOrdinal(DbNames.ITEM_BKIND);
            int ordinalITEM_VISUALID = reader.GetOrdinal(DbNames.ITEM_VISUALID);
            int ordinalITEM_COST = reader.GetOrdinal(DbNames.ITEM_COST);
            int ordinalITEM_CLASS = reader.GetOrdinal(DbNames.ITEM_CLASS);
            int ordinalITEM_AMOUNT = reader.GetOrdinal(DbNames.ITEM_AMOUNT);
            int ordinalITEM_LEVEL = reader.GetOrdinal(DbNames.ITEM_LEVEL);
            int ordinalITEM_DEX = reader.GetOrdinal(DbNames.ITEM_DEX);
            int ordinalITEM_STR = reader.GetOrdinal(DbNames.ITEM_STR);
            int ordinalITEM_STA = reader.GetOrdinal(DbNames.ITEM_STA);
            int ordinalITEM_ENE = reader.GetOrdinal(DbNames.ITEM_ENE);
            int ordinalITEM_MAXIMBUES = reader.GetOrdinal(DbNames.ITEM_MAXIMBUES);
            int ordinalITEM_MAXDURA = reader.GetOrdinal(DbNames.ITEM_MAXDURA);
            int ordinalITEM_CURDURA = reader.GetOrdinal(DbNames.ITEM_CURDURA);
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
            int ordinalITEM_PLUS = reader.GetOrdinal(DbNames.ITEM_PLUS);
            int ordinalITEM_SLVL = reader.GetOrdinal(DbNames.ITEM_SLVL);
            int ordinalITEM_IMBUETRIES = reader.GetOrdinal(DbNames.ITEM_IMBUETRIES);
            int ordinalITEM_DRAGONSUCCESSIMBUETRIES = reader.GetOrdinal(DbNames.ITEM_DRAGONSUCCESSIMBUETRIES);
            int ordinalITEM_DISCOUNTREPAIRFEE = reader.GetOrdinal(DbNames.ITEM_DISCOUNTREPAIRFEE);
            int ordinalITEM_TOTALDRAGONIMBUES = reader.GetOrdinal(DbNames.ITEM_TOTALDRAGONIMBUES);
            int ordinalITEM_DRAGONDAMAGE = reader.GetOrdinal(DbNames.ITEM_DRAGONDAMAGE);
            int ordinalITEM_DRAGONDEFENCE = reader.GetOrdinal(DbNames.ITEM_DRAGONDEFENCE);
            int ordinalITEM_DRAGONATTACKRATING = reader.GetOrdinal(DbNames.ITEM_DRAGONATTACKRATING);
            int ordinalITEM_DRAGONLIFE = reader.GetOrdinal(DbNames.ITEM_DRAGONLIFE);
            int ordinalITEM_MAPPEDSTUFF = reader.GetOrdinal(DbNames.ITEM_MAPPEDSTUFF);
            int ordinalITEM_FORCENUMBER = reader.GetOrdinal(DbNames.ITEM_FORCENUMBER);
            int ordinalITEM_REBIRTHHOLE = reader.GetOrdinal(DbNames.ITEM_REBIRTHHOLE);
            int ordinalITEM_REBIRTHHOLESTAT = reader.GetOrdinal(DbNames.ITEM_REBIRTHHOLESTAT);
            int ordinalITEM_TOMAPID = reader.GetOrdinal(DbNames.ITEM_TOMAPID);
            int ordinalITEM_IMBUERATE = reader.GetOrdinal(DbNames.ITEM_IMBUERATE);
            int ordinalITEM_IMBUEINCREASE = reader.GetOrdinal(DbNames.ITEM_IMBUEINCREASE);
            int ordinalITEM_IMBUEDATA = reader.GetOrdinal(DbNames.ITEM_IMBUEDATA);
            int ordinalITEM_BOOKSKILLID = reader.GetOrdinal(DbNames.ITEM_BOOKSKILLID);
            int ordinalITEM_BOOKSKILLLEVEL = reader.GetOrdinal(DbNames.ITEM_BOOKSKILLLEVEL);
            int ordinalITEM_BOOKSKILLDATA = reader.GetOrdinal(DbNames.ITEM_BOOKSKILLDATA);
            int ordinalITEM_MAXPOLISHTRIES = reader.GetOrdinal(DbNames.ITEM_MAXPOLISHTRIES);
            int ordinalITEM_POLISHTRIES = reader.GetOrdinal(DbNames.ITEM_POLISHTRIES);
            int ordinalITEM_VIGISTAT1 = reader.GetOrdinal(DbNames.ITEM_VIGISTAT1);
            int ordinalITEM_VIGISTAT2 = reader.GetOrdinal(DbNames.ITEM_VIGISTAT2);
            int ordinalITEM_VIGISTAT3 = reader.GetOrdinal(DbNames.ITEM_VIGISTAT3);
            int ordinalITEM_VIGISTAT4 = reader.GetOrdinal(DbNames.ITEM_VIGISTAT4);
            int ordinalITEM_VIGISTATADD1 = reader.GetOrdinal(DbNames.ITEM_VIGISTATADD1);
            int ordinalITEM_VIGISTATADD2 = reader.GetOrdinal(DbNames.ITEM_VIGISTATADD2);
            int ordinalITEM_VIGISTATADD3 = reader.GetOrdinal(DbNames.ITEM_VIGISTATADD3);
            int ordinalITEM_VIGISTATADD4 = reader.GetOrdinal(DbNames.ITEM_VIGISTATADD4);
            int ordinalITEM_PETID = reader.GetOrdinal(DbNames.ITEM_PETID);
            int ordinalITEM_DAMAGEABSORB = reader.GetOrdinal(DbNames.ITEM_DAMAGEABSORB);
            int ordinalITEM_DEFENSEABSORB = reader.GetOrdinal(DbNames.ITEM_DEFENSEABSORB);
            int ordinalITEM_ATTACKRATINGABSORB = reader.GetOrdinal(DbNames.ITEM_ATTACKRATINGABSORB);
            int ordinalITEM_LIFEABSORB = reader.GetOrdinal(DbNames.ITEM_LIFEABSORB);
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
                    || BType == (byte)bType.Ring || BType == (byte)bType.Shoes || BType == (byte)bType.Cape || BType == (byte)bType.Mirror)
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
                        c.PolishImbueTries = reader.GetByte(ordinalITEM_POLISHTRIES);
                        c.VigiStat1 = reader.GetInt16(ordinalITEM_VIGISTAT1);
                        c.VigiStatAdd1 = reader.GetInt16(ordinalITEM_VIGISTATADD1);
                        c.VigiStat2 = reader.GetInt16(ordinalITEM_VIGISTAT2);
                        c.VigiStatAdd2 = reader.GetInt16(ordinalITEM_VIGISTATADD2);
                        c.VigiStat3 = reader.GetInt16(ordinalITEM_VIGISTAT3);
                        c.VigiStatAdd3 = reader.GetInt16(ordinalITEM_VIGISTATADD3);
                        c.VigiStat4 = reader.GetInt16(ordinalITEM_VIGISTAT4);
                        c.VigiStatAdd4 = reader.GetInt16(ordinalITEM_VIGISTATADD4);
                    }
                    if (BType == (byte)bType.Mirror)
                    {
                        // bkind 4 = mirror, 0 = jar
                        b = new Mirror();
                        Mirror m = b as Mirror;

                        m.PetID = reader.GetInt32(ordinalITEM_PETID);
                        m.LifeAbsorb = reader.GetInt16(ordinalITEM_LIFEABSORB);
                        m.DamageAbsorb = reader.GetInt16(ordinalITEM_DAMAGEABSORB);
                        m.DefenseAbsorb = reader.GetInt16(ordinalITEM_DEFENSEABSORB);
                        m.AttackRatingAbsorb = reader.GetInt16(ordinalITEM_ATTACKRATINGABSORB);
                    }


                    Equipment e = b as Equipment;
                    e.RequiredLevel = reader.GetInt16(ordinalITEM_LEVEL);
                    e.RequiredDexterity = reader.GetInt16(ordinalITEM_DEX);
                    e.RequiredStrength = reader.GetInt16(ordinalITEM_STR);
                    e.RequiredStamina = reader.GetInt16(ordinalITEM_STA);
                    e.RequiredEnergy = reader.GetInt16(ordinalITEM_ENE);
                    e.MaxImbueTries = reader.GetByte(ordinalITEM_MAXIMBUES);
                    e.Durability = reader.GetInt16(ordinalITEM_CURDURA);
                    e.MaxDurability = reader.GetInt16(ordinalITEM_MAXDURA);
                    e.Damage = reader.GetInt32(ordinalITEM_DAMAGE);
                    e.Defence = reader.GetInt32(ordinalITEM_DEFENCE);
                    e.AttackRating = reader.GetInt32(ordinalITEM_ATTACKRATING);
                    e.AttackSpeed = reader.GetInt16(ordinalITEM_ATTACKSPEED);
                    e.AttackRange = reader.GetInt16(ordinalITEM_ATTACKRANGE);
                    e.IncMaxLife = reader.GetInt16(ordinalITEM_INCMAXLIFE);
                    e.IncMaxMana = reader.GetInt16(ordinalITEM_INCMAXMANA);
                    e.IncLifeRegen = reader.GetInt16(ordinalITEM_LIFEREGEN);
                    e.IncManaRegen = reader.GetInt16(ordinalITEM_MANAREGEN);
                    e.Critical = reader.GetInt16(ordinalITEM_CRITICAL);
                    e.Plus = reader.GetByte(ordinalITEM_PLUS);
                    e.Slvl = reader.GetByte(ordinalITEM_SLVL);
                    e.ImbueTries = reader.GetByte(ordinalITEM_IMBUETRIES);
                    e.DragonSuccessImbueTries = reader.GetInt16(ordinalITEM_DRAGONSUCCESSIMBUETRIES);
                    e.DiscountRepairFee = reader.GetByte(ordinalITEM_DISCOUNTREPAIRFEE);
                    e.TotalDragonImbueTries = reader.GetInt16(ordinalITEM_TOTALDRAGONIMBUES);
                    e.DragonDamage = reader.GetInt32(ordinalITEM_DRAGONDAMAGE);
                    e.DragonDefence = reader.GetInt32(ordinalITEM_DRAGONDEFENCE);
                    e.DragonAttackRating = reader.GetInt32(ordinalITEM_DRAGONATTACKRATING);
                    e.DragonLife = reader.GetInt16(ordinalITEM_DRAGONLIFE);
                    e.MappedData = reader.GetByte(ordinalITEM_MAPPEDSTUFF);
                    e.ForceSlot = reader.GetByte(ordinalITEM_FORCENUMBER);
                    e.RebirthHole = reader.GetInt16(ordinalITEM_REBIRTHHOLE);
                    e.RebirthHoleStat = reader.GetInt16(ordinalITEM_REBIRTHHOLESTAT);
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
                    if (BKind == (byte)bKindStones.RbItem)
                    {
                        b = new RbHoleItem();
                    }

                    ImbueItem im = b as ImbueItem;
                    im.ImbueChance = reader.GetInt16(ordinalITEM_IMBUERATE);
                    im.IncreaseValue = reader.GetInt16(ordinalITEM_IMBUEINCREASE);
                    im.ImbueData = reader.GetByte(ordinalITEM_IMBUEDATA);
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
                    if (BKind == (byte)bKindBooks.RebirdBook)
                    {
                        b = new RebirthBook();
                    }
                    if (BKind == (byte)bKindBooks.FourthBook)
                    {
                        b = new FourthBook();
                    }
                    if (BKind == (byte)bKindBooks.FeSkillBook)
                    {
                        b = new FeSkillBook();
                    }
                    if (BKind == (byte)bKindBooks.FeBook)
                    {
                        b = new FiveElementBook();
                    }
                    if (BKind == (byte)bKindBooks.FocusBook)
                    {
                        b = new FocusBook();
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
                if (BType == (byte)bType.StoreTag)
                {
                    b = new StoreTag();

                    StoreTag tag = b as StoreTag;
                    tag.TimeLeft = reader.GetInt16(ordinalITEM_CURDURA);
                    tag.TimeMax = reader.GetInt16(ordinalITEM_MAXDURA);
                }
                if (BType == (byte)bType.PetItem)
                {
                    if (BKind == (byte)bKindPetItems.Taming)
                        b = new TameItem();
                    if (BKind == (byte)bKindPetItems.Food)
                        b = new PetFood();
                    if (BKind == (byte)bKindPetItems.Potion)
                        b = new PetPotion();
                    if (BKind == (byte)bKindPetItems.Resurect)
                        b = new PetResurrectItem();

                    PetItem p = b as PetItem;
                    p.TameChance = reader.GetInt16(ordinalITEM_IMBUERATE);
                    p.DecreaseWildness = reader.GetInt16(ordinalITEM_IMBUEINCREASE);
                    p.HealLife = reader.GetInt16(ordinalITEM_INCMAXLIFE);
                }
                if (BType == (byte)bType.Pill)
                {
                    if (BKind == (byte)bKindPills.Rebirth)
                        b = new RebirthPill();

                    RebirthPill p = b as RebirthPill;
                    p.RequiredLevel = reader.GetInt16(ordinalITEM_LEVEL);
                    p.RequiredRebirth = reader.GetByte(ordinalITEM_CLASS);
                    p.ToRebirth = (byte)(p.RequiredRebirth + 1);
                    p.IncreaseSp = reader.GetInt16(ordinalITEM_DEX);
                }

                b.ItemID = reader.GetInt32(ordinalITEM_ITEMID);
                b.OwnerID = reader.GetInt32(ordinalITEM_OWNERID);
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
                b.Price = reader.GetInt32(ordinalITEM_COST);

                items.Add(b);
            }

            reader.Close();
            _db.Close();

            return items;
        }

        public List<BaseItem> GetAllItemsInBag(byte bag, int characterId)
        {
            DbParameter bagIdParameter = _db.CreateParameter(DbNames.GETALLITEMSBYBAGID_BAGID_PARAMETER, bag);
            bagIdParameter.DbType = DbType.Byte;

            DbParameter characterIdParameter = _db.CreateParameter(DbNames.GETALLITEMSBYBAGID_CHARACTERID_PARAMETER, characterId);
            characterIdParameter.DbType = DbType.Int32;

            List<BaseItem> items = new List<BaseItem>();

            _db.Open();

            DbDataReader reader = _db.ExcecuteReader(DbNames.GETALLITEMSBYBAGID_STOREDPROC, CommandType.StoredProcedure, bagIdParameter, characterIdParameter);

            int ordinalITEM_ITEMID = reader.GetOrdinal(DbNames.ITEM_ITEMID);
            int ordinalITEM_OWNERID = reader.GetOrdinal(DbNames.ITEM_OWNERID);
            int ordinalITEM_REFERENCEID = reader.GetOrdinal(DbNames.ITEM_REFERENCEID);
            int ordinalITEM_BTYPE = reader.GetOrdinal(DbNames.ITEM_BTYPE);
            int ordinalITEM_BKIND = reader.GetOrdinal(DbNames.ITEM_BKIND);
            int ordinalITEM_VISUALID = reader.GetOrdinal(DbNames.ITEM_VISUALID);
            int ordinalITEM_COST = reader.GetOrdinal(DbNames.ITEM_COST);
            int ordinalITEM_CLASS = reader.GetOrdinal(DbNames.ITEM_CLASS);
            int ordinalITEM_AMOUNT = reader.GetOrdinal(DbNames.ITEM_AMOUNT);
            int ordinalITEM_LEVEL = reader.GetOrdinal(DbNames.ITEM_LEVEL);
            int ordinalITEM_DEX = reader.GetOrdinal(DbNames.ITEM_DEX);
            int ordinalITEM_STR = reader.GetOrdinal(DbNames.ITEM_STR);
            int ordinalITEM_STA = reader.GetOrdinal(DbNames.ITEM_STA);
            int ordinalITEM_ENE = reader.GetOrdinal(DbNames.ITEM_ENE);
            int ordinalITEM_MAXIMBUES = reader.GetOrdinal(DbNames.ITEM_MAXIMBUES);
            int ordinalITEM_MAXDURA = reader.GetOrdinal(DbNames.ITEM_MAXDURA);
            int ordinalITEM_CURDURA = reader.GetOrdinal(DbNames.ITEM_CURDURA);
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
            int ordinalITEM_PLUS = reader.GetOrdinal(DbNames.ITEM_PLUS);
            int ordinalITEM_SLVL = reader.GetOrdinal(DbNames.ITEM_SLVL);
            int ordinalITEM_IMBUETRIES = reader.GetOrdinal(DbNames.ITEM_IMBUETRIES);
            int ordinalITEM_DRAGONSUCCESSIMBUETRIES = reader.GetOrdinal(DbNames.ITEM_DRAGONSUCCESSIMBUETRIES);
            int ordinalITEM_DISCOUNTREPAIRFEE = reader.GetOrdinal(DbNames.ITEM_DISCOUNTREPAIRFEE);
            int ordinalITEM_TOTALDRAGONIMBUES = reader.GetOrdinal(DbNames.ITEM_TOTALDRAGONIMBUES);
            int ordinalITEM_DRAGONDAMAGE = reader.GetOrdinal(DbNames.ITEM_DRAGONDAMAGE);
            int ordinalITEM_DRAGONDEFENCE = reader.GetOrdinal(DbNames.ITEM_DRAGONDEFENCE);
            int ordinalITEM_DRAGONATTACKRATING = reader.GetOrdinal(DbNames.ITEM_DRAGONATTACKRATING);
            int ordinalITEM_DRAGONLIFE = reader.GetOrdinal(DbNames.ITEM_DRAGONLIFE);
            int ordinalITEM_MAPPEDSTUFF = reader.GetOrdinal(DbNames.ITEM_MAPPEDSTUFF);
            int ordinalITEM_FORCENUMBER = reader.GetOrdinal(DbNames.ITEM_FORCENUMBER);
            int ordinalITEM_REBIRTHHOLE = reader.GetOrdinal(DbNames.ITEM_REBIRTHHOLE);
            int ordinalITEM_REBIRTHHOLESTAT = reader.GetOrdinal(DbNames.ITEM_REBIRTHHOLESTAT);
            int ordinalITEM_TOMAPID = reader.GetOrdinal(DbNames.ITEM_TOMAPID);
            int ordinalITEM_IMBUERATE = reader.GetOrdinal(DbNames.ITEM_IMBUERATE);
            int ordinalITEM_IMBUEINCREASE = reader.GetOrdinal(DbNames.ITEM_IMBUEINCREASE);
            int ordinalITEM_IMBUEDATA = reader.GetOrdinal(DbNames.ITEM_IMBUEDATA);
            int ordinalITEM_BOOKSKILLID = reader.GetOrdinal(DbNames.ITEM_BOOKSKILLID);
            int ordinalITEM_BOOKSKILLLEVEL = reader.GetOrdinal(DbNames.ITEM_BOOKSKILLLEVEL);
            int ordinalITEM_BOOKSKILLDATA = reader.GetOrdinal(DbNames.ITEM_BOOKSKILLDATA);
            int ordinalITEM_MAXPOLISHTRIES = reader.GetOrdinal(DbNames.ITEM_MAXPOLISHTRIES);
            int ordinalITEM_POLISHTRIES = reader.GetOrdinal(DbNames.ITEM_POLISHTRIES);
            int ordinalITEM_VIGISTAT1 = reader.GetOrdinal(DbNames.ITEM_VIGISTAT1);
            int ordinalITEM_VIGISTAT2 = reader.GetOrdinal(DbNames.ITEM_VIGISTAT2);
            int ordinalITEM_VIGISTAT3 = reader.GetOrdinal(DbNames.ITEM_VIGISTAT3);
            int ordinalITEM_VIGISTAT4 = reader.GetOrdinal(DbNames.ITEM_VIGISTAT4);
            int ordinalITEM_VIGISTATADD1 = reader.GetOrdinal(DbNames.ITEM_VIGISTATADD1);
            int ordinalITEM_VIGISTATADD2 = reader.GetOrdinal(DbNames.ITEM_VIGISTATADD2);
            int ordinalITEM_VIGISTATADD3 = reader.GetOrdinal(DbNames.ITEM_VIGISTATADD3);
            int ordinalITEM_VIGISTATADD4 = reader.GetOrdinal(DbNames.ITEM_VIGISTATADD4);
            int ordinalITEM_PETID = reader.GetOrdinal(DbNames.ITEM_PETID);
            int ordinalITEM_DAMAGEABSORB = reader.GetOrdinal(DbNames.ITEM_DAMAGEABSORB);
            int ordinalITEM_DEFENSEABSORB = reader.GetOrdinal(DbNames.ITEM_DEFENSEABSORB);
            int ordinalITEM_ATTACKRATINGABSORB = reader.GetOrdinal(DbNames.ITEM_ATTACKRATINGABSORB);
            int ordinalITEM_LIFEABSORB = reader.GetOrdinal(DbNames.ITEM_LIFEABSORB);
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
                    || BType == (byte)bType.Ring || BType == (byte)bType.Shoes || BType == (byte)bType.Cape || BType == (byte)bType.Mirror)
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
                        c.PolishImbueTries = reader.GetByte(ordinalITEM_POLISHTRIES);
                        c.VigiStat1 = reader.GetInt16(ordinalITEM_VIGISTAT1);
                        c.VigiStatAdd1 = reader.GetInt16(ordinalITEM_VIGISTATADD1);
                        c.VigiStat2 = reader.GetInt16(ordinalITEM_VIGISTAT2);
                        c.VigiStatAdd2 = reader.GetInt16(ordinalITEM_VIGISTATADD2);
                        c.VigiStat3 = reader.GetInt16(ordinalITEM_VIGISTAT3);
                        c.VigiStatAdd3 = reader.GetInt16(ordinalITEM_VIGISTATADD3);
                        c.VigiStat4 = reader.GetInt16(ordinalITEM_VIGISTAT4);
                        c.VigiStatAdd4 = reader.GetInt16(ordinalITEM_VIGISTATADD4);
                    }
                    if (BType == (byte)bType.Mirror)
                    {
                        // bkind 4 = mirror, 0 = jar
                        b = new Mirror();
                        Mirror m = b as Mirror;

                        m.PetID = reader.GetInt32(ordinalITEM_PETID);
                        m.LifeAbsorb = reader.GetInt16(ordinalITEM_LIFEABSORB);
                        m.DamageAbsorb = reader.GetInt16(ordinalITEM_DAMAGEABSORB);
                        m.DefenseAbsorb = reader.GetInt16(ordinalITEM_DEFENSEABSORB);
                        m.AttackRatingAbsorb = reader.GetInt16(ordinalITEM_ATTACKRATINGABSORB);
                    }


                    Equipment e = b as Equipment;
                    e.RequiredLevel = reader.GetInt16(ordinalITEM_LEVEL);
                    e.RequiredDexterity = reader.GetInt16(ordinalITEM_DEX);
                    e.RequiredStrength = reader.GetInt16(ordinalITEM_STR);
                    e.RequiredStamina = reader.GetInt16(ordinalITEM_STA);
                    e.RequiredEnergy = reader.GetInt16(ordinalITEM_ENE);
                    e.MaxImbueTries = reader.GetByte(ordinalITEM_MAXIMBUES);
                    e.Durability = reader.GetInt16(ordinalITEM_CURDURA);
                    e.MaxDurability = reader.GetInt16(ordinalITEM_MAXDURA);
                    e.Damage = reader.GetInt32(ordinalITEM_DAMAGE);
                    e.Defence = reader.GetInt32(ordinalITEM_DEFENCE);
                    e.AttackRating = reader.GetInt32(ordinalITEM_ATTACKRATING);
                    e.AttackSpeed = reader.GetInt16(ordinalITEM_ATTACKSPEED);
                    e.AttackRange = reader.GetInt16(ordinalITEM_ATTACKRANGE);
                    e.IncMaxLife = reader.GetInt16(ordinalITEM_INCMAXLIFE);
                    e.IncMaxMana = reader.GetInt16(ordinalITEM_INCMAXMANA);
                    e.IncLifeRegen = reader.GetInt16(ordinalITEM_LIFEREGEN);
                    e.IncManaRegen = reader.GetInt16(ordinalITEM_MANAREGEN);
                    e.Critical = reader.GetInt16(ordinalITEM_CRITICAL);
                    e.Plus = reader.GetByte(ordinalITEM_PLUS);
                    e.Slvl = reader.GetByte(ordinalITEM_SLVL);
                    e.ImbueTries = reader.GetByte(ordinalITEM_IMBUETRIES);
                    e.DragonSuccessImbueTries = reader.GetInt16(ordinalITEM_DRAGONSUCCESSIMBUETRIES);
                    e.DiscountRepairFee = reader.GetByte(ordinalITEM_DISCOUNTREPAIRFEE);
                    e.TotalDragonImbueTries = reader.GetInt16(ordinalITEM_TOTALDRAGONIMBUES);
                    e.DragonDamage = reader.GetInt32(ordinalITEM_DRAGONDAMAGE);
                    e.DragonDefence = reader.GetInt32(ordinalITEM_DRAGONDEFENCE);
                    e.DragonAttackRating = reader.GetInt32(ordinalITEM_DRAGONATTACKRATING);
                    e.DragonLife = reader.GetInt16(ordinalITEM_DRAGONLIFE);
                    e.MappedData = reader.GetByte(ordinalITEM_MAPPEDSTUFF);
                    e.ForceSlot = reader.GetByte(ordinalITEM_FORCENUMBER);
                    e.RebirthHole = reader.GetInt16(ordinalITEM_REBIRTHHOLE);
                    e.RebirthHoleStat = reader.GetInt16(ordinalITEM_REBIRTHHOLESTAT);
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
                    if (BKind == (byte)bKindStones.RbItem) 
                    {
                        b = new RbHoleItem();
                    }

                    ImbueItem im = b as ImbueItem;
                    im.ImbueChance = reader.GetInt16(ordinalITEM_IMBUERATE);
                    im.IncreaseValue = reader.GetInt16(ordinalITEM_IMBUEINCREASE);
                    im.ImbueData = reader.GetByte(ordinalITEM_IMBUEDATA);
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
                    if (BKind == (byte)bKindBooks.RebirdBook)
                    {
                        b = new RebirthBook();
                    }
                    if (BKind == (byte)bKindBooks.FourthBook)
                    {
                        b = new FourthBook();
                    }
                    if (BKind == (byte)bKindBooks.FeSkillBook)
                    {
                        b = new FeSkillBook();
                    }
                    if (BKind == (byte)bKindBooks.FeBook)
                    {
                        b = new FiveElementBook();
                    }
                    if (BKind == (byte)bKindBooks.FocusBook)
                    {
                        b = new FocusBook();
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
                if (BType == (byte)bType.StoreTag)
                {
                    b = new StoreTag();

                    StoreTag tag = b as StoreTag;
                    tag.TimeLeft = reader.GetInt16(ordinalITEM_CURDURA);
                    tag.TimeMax = reader.GetInt16(ordinalITEM_MAXDURA);
                }
                if (BType == (byte)bType.PetItem)
                {
                    if (BKind == (byte)bKindPetItems.Taming)
                        b = new TameItem();
                    if (BKind == (byte)bKindPetItems.Food)
                        b = new PetFood();
                    if (BKind == (byte)bKindPetItems.Potion)
                        b = new PetPotion();
                    if (BKind == (byte)bKindPetItems.Resurect)
                        b = new PetResurrectItem();

                    PetItem p = b as PetItem;
                    p.TameChance = reader.GetInt16(ordinalITEM_IMBUERATE);
                    p.DecreaseWildness = reader.GetInt16(ordinalITEM_IMBUEINCREASE);
                    p.HealLife = reader.GetInt16(ordinalITEM_INCMAXLIFE);
                }
                if (BType == (byte)bType.Pill)
                {
                    if (BKind == (byte)bKindPills.Rebirth)
                        b = new RebirthPill();

                    RebirthPill p = b as RebirthPill;
                    p.RequiredLevel = reader.GetInt16(ordinalITEM_LEVEL);
                    p.RequiredRebirth = reader.GetByte(ordinalITEM_CLASS);
                    p.ToRebirth = (byte)(p.RequiredRebirth + 1);
                    p.IncreaseSp = reader.GetInt16(ordinalITEM_DEX);
                }

                b.ItemID = reader.GetInt32(ordinalITEM_ITEMID);
                b.OwnerID = reader.GetInt32(ordinalITEM_OWNERID);
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
                b.Price = reader.GetInt32(ordinalITEM_COST);

                items.Add(b);
            }

            reader.Close();
            _db.Close();

            return items;
        }

        public void RemoveShopItemByItemID(int itemid)
        {
            DbParameter itemIdParameter = _db.CreateParameter(DbNames.REMOVESHOPITEMBYITEMID_ITEMID_PARAMETER, itemid);
            itemIdParameter.DbType = DbType.Int32;

            _db.Open();

            _db.ExecuteNonQuery(DbNames.REMOVESHOPITEMBYITEMID_STOREDPROC,
              System.Data.CommandType.StoredProcedure,
              itemIdParameter);

            _db.Close();
        }

        public void UpdateShopByCharacterID(Shop shop)
        {
            DbParameter characterIdParameter = _db.CreateParameter(DbNames.UPDATESHOPBYCHARACTERID_CHARACTERID_PARAMETER, shop.OwnerID);
            characterIdParameter.DbType = DbType.Int32;

            DbParameter nameParameter = _db.CreateParameter(DbNames.UPDATESHOPBYCHARACTERID_NAME_PARAMETER, shop.ShopName);
            nameParameter.DbType = DbType.String;

            DbParameter descParameter = _db.CreateParameter(DbNames.UPDATESHOPBYCHARACTERID_DESC_PARAMETER, shop.ShopDesc);
            descParameter.DbType = DbType.String;

            DbParameter totalmoneyParameter = _db.CreateParameter(DbNames.UPDATESHOPBYCHARACTERID_TOTALMONEY_PARAMETER, shop.TotalMoney);
            totalmoneyParameter.DbType = DbType.Int32;

            _db.Open();

            _db.ExecuteNonQuery(DbNames.UPDATESHOPBYCHARACTERID_STOREDPROC,
              System.Data.CommandType.StoredProcedure,
              characterIdParameter,
              nameParameter,
              descParameter,
              totalmoneyParameter);

            _db.Close();
        }

        public void UpdateShopItemById(BaseItem item)
        {
            DbParameter slotParameter = _db.CreateParameter(DbNames.INSERTSHOPITEM_SLOT_PARAMETER, item.Slot);
            slotParameter.DbType = DbType.Byte;

            DbParameter itemIdParameter = _db.CreateParameter(DbNames.INSERTSHOPITEM_ITEMID_PARAMETER, item.ItemID);
            itemIdParameter.DbType = DbType.Int32;

            DbParameter pricedParameter = _db.CreateParameter(DbNames.INSERTSHOPITEM_PRICE_PARAMETER, item.SellPrice);
            pricedParameter.DbType = DbType.Int32;

            _db.Open();

            _db.ExecuteNonQuery(DbNames.UPDATESHOPITEM_STOREDPROC,
              System.Data.CommandType.StoredProcedure,
              slotParameter,
              itemIdParameter,
              pricedParameter);

            _db.Close();
        }

        public void InsertShopItem(int shopId, byte slot, int itemId, int price)
        {
            DbParameter shopIdParameter = _db.CreateParameter(DbNames.INSERTSHOPITEM_SHOPID_PARAMETER, shopId);
            shopIdParameter.DbType = DbType.Int32;

            DbParameter slotParameter = _db.CreateParameter(DbNames.INSERTSHOPITEM_SLOT_PARAMETER, slot);
            slotParameter.DbType = DbType.Byte;

            DbParameter itemIdParameter = _db.CreateParameter(DbNames.INSERTSHOPITEM_ITEMID_PARAMETER, itemId);
            itemIdParameter.DbType = DbType.Int32;

            DbParameter pricedParameter = _db.CreateParameter(DbNames.INSERTSHOPITEM_PRICE_PARAMETER, price);
            pricedParameter.DbType = DbType.Int32;

            _db.Open();

            _db.ExecuteNonQuery(DbNames.INSERTSHOPITEM_STOREDPROC,
              System.Data.CommandType.StoredProcedure,
              shopIdParameter,
              slotParameter,
              itemIdParameter,
              pricedParameter);

            _db.Close();
        }

        public List<BaseItem> GetShopItemsByOwnerID(int ownerId)
        {
            DbParameter ownerIdParameter = _db.CreateParameter(DbNames.GETSHOPITEMSBYOWNERID_OWNERID_PARAMETER, ownerId);
            ownerIdParameter.DbType = DbType.Int32;

            List<BaseItem> items = new List<BaseItem>();

            _db.Open();

            DbDataReader reader = _db.ExcecuteReader(DbNames.GETSHOPITEMSBYOWNERID_STOREDPROC, CommandType.StoredProcedure, ownerIdParameter);

            int ordinalITEM_ITEMID = reader.GetOrdinal(DbNames.ITEM_ITEMID);
            int ordinalITEM_OWNERID = reader.GetOrdinal(DbNames.ITEM_OWNERID);
            int ordinalITEM_REFERENCEID = reader.GetOrdinal(DbNames.ITEM_REFERENCEID);
            int ordinalITEM_BTYPE = reader.GetOrdinal(DbNames.ITEM_BTYPE);
            int ordinalITEM_BKIND = reader.GetOrdinal(DbNames.ITEM_BKIND);
            int ordinalITEM_VISUALID = reader.GetOrdinal(DbNames.ITEM_VISUALID);
            int ordinalITEM_SHOPPRICE = reader.GetOrdinal(DbNames.SHOP_PRICE);
            int ordinalITEM_COST = reader.GetOrdinal(DbNames.ITEM_COST);
            int ordinalITEM_CLASS = reader.GetOrdinal(DbNames.ITEM_CLASS);
            int ordinalITEM_AMOUNT = reader.GetOrdinal(DbNames.ITEM_AMOUNT);
            int ordinalITEM_LEVEL = reader.GetOrdinal(DbNames.ITEM_LEVEL);
            int ordinalITEM_DEX = reader.GetOrdinal(DbNames.ITEM_DEX);
            int ordinalITEM_STR = reader.GetOrdinal(DbNames.ITEM_STR);
            int ordinalITEM_STA = reader.GetOrdinal(DbNames.ITEM_STA);
            int ordinalITEM_ENE = reader.GetOrdinal(DbNames.ITEM_ENE);
            int ordinalITEM_MAXIMBUES = reader.GetOrdinal(DbNames.ITEM_MAXIMBUES);
            int ordinalITEM_MAXDURA = reader.GetOrdinal(DbNames.ITEM_MAXDURA);
            int ordinalITEM_CURDURA = reader.GetOrdinal(DbNames.ITEM_CURDURA);
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
            int ordinalITEM_PLUS = reader.GetOrdinal(DbNames.ITEM_PLUS);
            int ordinalITEM_SLVL = reader.GetOrdinal(DbNames.ITEM_SLVL);
            int ordinalITEM_IMBUETRIES = reader.GetOrdinal(DbNames.ITEM_IMBUETRIES);
            int ordinalITEM_DRAGONSUCCESSIMBUETRIES = reader.GetOrdinal(DbNames.ITEM_DRAGONSUCCESSIMBUETRIES);
            int ordinalITEM_DISCOUNTREPAIRFEE = reader.GetOrdinal(DbNames.ITEM_DISCOUNTREPAIRFEE);
            int ordinalITEM_TOTALDRAGONIMBUES = reader.GetOrdinal(DbNames.ITEM_TOTALDRAGONIMBUES);
            int ordinalITEM_DRAGONDAMAGE = reader.GetOrdinal(DbNames.ITEM_DRAGONDAMAGE);
            int ordinalITEM_DRAGONDEFENCE = reader.GetOrdinal(DbNames.ITEM_DRAGONDEFENCE);
            int ordinalITEM_DRAGONATTACKRATING = reader.GetOrdinal(DbNames.ITEM_DRAGONATTACKRATING);
            int ordinalITEM_DRAGONLIFE = reader.GetOrdinal(DbNames.ITEM_DRAGONLIFE);
            int ordinalITEM_MAPPEDSTUFF = reader.GetOrdinal(DbNames.ITEM_MAPPEDSTUFF);
            int ordinalITEM_FORCENUMBER = reader.GetOrdinal(DbNames.ITEM_FORCENUMBER);
            int ordinalITEM_REBIRTHHOLE = reader.GetOrdinal(DbNames.ITEM_REBIRTHHOLE);
            int ordinalITEM_REBIRTHHOLESTAT = reader.GetOrdinal(DbNames.ITEM_REBIRTHHOLESTAT);
            int ordinalITEM_TOMAPID = reader.GetOrdinal(DbNames.ITEM_TOMAPID);
            int ordinalITEM_IMBUERATE = reader.GetOrdinal(DbNames.ITEM_IMBUERATE);
            int ordinalITEM_IMBUEINCREASE = reader.GetOrdinal(DbNames.ITEM_IMBUEINCREASE);
            int ordinalITEM_IMBUEDATA = reader.GetOrdinal(DbNames.ITEM_IMBUEDATA);
            int ordinalITEM_BOOKSKILLID = reader.GetOrdinal(DbNames.ITEM_BOOKSKILLID);
            int ordinalITEM_BOOKSKILLLEVEL = reader.GetOrdinal(DbNames.ITEM_BOOKSKILLLEVEL);
            int ordinalITEM_BOOKSKILLDATA = reader.GetOrdinal(DbNames.ITEM_BOOKSKILLDATA);
            int ordinalITEM_MAXPOLISHTRIES = reader.GetOrdinal(DbNames.ITEM_MAXPOLISHTRIES);
            int ordinalITEM_POLISHTRIES = reader.GetOrdinal(DbNames.ITEM_POLISHTRIES);
            int ordinalITEM_VIGISTAT1 = reader.GetOrdinal(DbNames.ITEM_VIGISTAT1);
            int ordinalITEM_VIGISTAT2 = reader.GetOrdinal(DbNames.ITEM_VIGISTAT2);
            int ordinalITEM_VIGISTAT3 = reader.GetOrdinal(DbNames.ITEM_VIGISTAT3);
            int ordinalITEM_VIGISTAT4 = reader.GetOrdinal(DbNames.ITEM_VIGISTAT4);
            int ordinalITEM_VIGISTATADD1 = reader.GetOrdinal(DbNames.ITEM_VIGISTATADD1);
            int ordinalITEM_VIGISTATADD2 = reader.GetOrdinal(DbNames.ITEM_VIGISTATADD2);
            int ordinalITEM_VIGISTATADD3 = reader.GetOrdinal(DbNames.ITEM_VIGISTATADD3);
            int ordinalITEM_VIGISTATADD4 = reader.GetOrdinal(DbNames.ITEM_VIGISTATADD4);
            int ordinalITEM_PETID = reader.GetOrdinal(DbNames.ITEM_PETID);
            int ordinalITEM_DAMAGEABSORB = reader.GetOrdinal(DbNames.ITEM_DAMAGEABSORB);
            int ordinalITEM_DEFENSEABSORB = reader.GetOrdinal(DbNames.ITEM_DEFENSEABSORB);
            int ordinalITEM_ATTACKRATINGABSORB = reader.GetOrdinal(DbNames.ITEM_ATTACKRATINGABSORB);
            int ordinalITEM_LIFEABSORB = reader.GetOrdinal(DbNames.ITEM_LIFEABSORB);
            int ordinalITEM_BAG = reader.GetOrdinal(DbNames.ITEM_BAG);
            int ordinalITEM_SLOT = reader.GetOrdinal(DbNames.SHOP_SLOT);
            int ordinalITEM_SIZEX = reader.GetOrdinal(DbNames.ITEM_SIZEX);
            int ordinalITEM_SIZEY = reader.GetOrdinal(DbNames.ITEM_SIZEY);

            while (reader.Read())
            {
                BaseItem b = null;

                int BType = reader.GetByte(ordinalITEM_BTYPE);
                int BKind = reader.GetByte(ordinalITEM_BKIND);



                if (BType == (byte)bType.Weapon || BType == (byte)bType.Clothes || BType == (byte)bType.Hat || BType == (byte)bType.Necklace
                    || BType == (byte)bType.Ring || BType == (byte)bType.Shoes || BType == (byte)bType.Cape || BType == (byte)bType.Mirror)
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
                        c.PolishImbueTries = reader.GetByte(ordinalITEM_POLISHTRIES);
                        c.VigiStat1 = reader.GetInt16(ordinalITEM_VIGISTAT1);
                        c.VigiStatAdd1 = reader.GetInt16(ordinalITEM_VIGISTATADD1);
                        c.VigiStat2 = reader.GetInt16(ordinalITEM_VIGISTAT2);
                        c.VigiStatAdd2 = reader.GetInt16(ordinalITEM_VIGISTATADD2);
                        c.VigiStat3 = reader.GetInt16(ordinalITEM_VIGISTAT3);
                        c.VigiStatAdd3 = reader.GetInt16(ordinalITEM_VIGISTATADD3);
                        c.VigiStat4 = reader.GetInt16(ordinalITEM_VIGISTAT4);
                        c.VigiStatAdd4 = reader.GetInt16(ordinalITEM_VIGISTATADD4);
                    }
                    if (BType == (byte)bType.Mirror)
                    {
                        // bkind 4 = mirror, 0 = jar
                        b = new Mirror();
                        Mirror m = b as Mirror;

                        m.PetID = reader.GetInt32(ordinalITEM_PETID);
                        m.LifeAbsorb = reader.GetInt16(ordinalITEM_LIFEABSORB);
                        m.DamageAbsorb = reader.GetInt16(ordinalITEM_DAMAGEABSORB);
                        m.DefenseAbsorb = reader.GetInt16(ordinalITEM_DEFENSEABSORB);
                        m.AttackRatingAbsorb = reader.GetInt16(ordinalITEM_ATTACKRATINGABSORB);
                    }


                    Equipment e = b as Equipment;
                    e.RequiredLevel = reader.GetInt16(ordinalITEM_LEVEL);
                    e.RequiredDexterity = reader.GetInt16(ordinalITEM_DEX);
                    e.RequiredStrength = reader.GetInt16(ordinalITEM_STR);
                    e.RequiredStamina = reader.GetInt16(ordinalITEM_STA);
                    e.RequiredEnergy = reader.GetInt16(ordinalITEM_ENE);
                    e.MaxImbueTries = reader.GetByte(ordinalITEM_MAXIMBUES);
                    e.Durability = reader.GetInt16(ordinalITEM_CURDURA);
                    e.MaxDurability = reader.GetInt16(ordinalITEM_MAXDURA);
                    e.Damage = reader.GetInt32(ordinalITEM_DAMAGE);
                    e.Defence = reader.GetInt32(ordinalITEM_DEFENCE);
                    e.AttackRating = reader.GetInt32(ordinalITEM_ATTACKRATING);
                    e.AttackSpeed = reader.GetInt16(ordinalITEM_ATTACKSPEED);
                    e.AttackRange = reader.GetInt16(ordinalITEM_ATTACKRANGE);
                    e.IncMaxLife = reader.GetInt16(ordinalITEM_INCMAXLIFE);
                    e.IncMaxMana = reader.GetInt16(ordinalITEM_INCMAXMANA);
                    e.IncLifeRegen = reader.GetInt16(ordinalITEM_LIFEREGEN);
                    e.IncManaRegen = reader.GetInt16(ordinalITEM_MANAREGEN);
                    e.Critical = reader.GetInt16(ordinalITEM_CRITICAL);
                    e.Plus = reader.GetByte(ordinalITEM_PLUS);
                    e.Slvl = reader.GetByte(ordinalITEM_SLVL);
                    e.ImbueTries = reader.GetByte(ordinalITEM_IMBUETRIES);
                    e.DragonSuccessImbueTries = reader.GetInt16(ordinalITEM_DRAGONSUCCESSIMBUETRIES);
                    e.DiscountRepairFee = reader.GetByte(ordinalITEM_DISCOUNTREPAIRFEE);
                    e.TotalDragonImbueTries = reader.GetInt16(ordinalITEM_TOTALDRAGONIMBUES);
                    e.DragonDamage = reader.GetInt32(ordinalITEM_DRAGONDAMAGE);
                    e.DragonDefence = reader.GetInt32(ordinalITEM_DRAGONDEFENCE);
                    e.DragonAttackRating = reader.GetInt32(ordinalITEM_DRAGONATTACKRATING);
                    e.DragonLife = reader.GetInt16(ordinalITEM_DRAGONLIFE);
                    e.MappedData = reader.GetByte(ordinalITEM_MAPPEDSTUFF);
                    e.ForceSlot = reader.GetByte(ordinalITEM_FORCENUMBER);
                    e.RebirthHole = reader.GetInt16(ordinalITEM_REBIRTHHOLE);
                    e.RebirthHoleStat = reader.GetInt16(ordinalITEM_REBIRTHHOLESTAT);
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
                    if (BKind == (byte)bKindStones.RbItem)
                    {
                        b = new RbHoleItem();
                    }

                    ImbueItem im = b as ImbueItem;
                    im.ImbueChance = reader.GetInt16(ordinalITEM_IMBUERATE);
                    im.IncreaseValue = reader.GetInt16(ordinalITEM_IMBUEINCREASE);
                    im.ImbueData = reader.GetByte(ordinalITEM_IMBUEDATA);
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
                    if (BKind == (byte)bKindBooks.RebirdBook)
                    {
                        b = new RebirthBook();
                    }
                    if (BKind == (byte)bKindBooks.FourthBook)
                    {
                        b = new FourthBook();
                    }
                    if (BKind == (byte)bKindBooks.FeSkillBook)
                    {
                        b = new FeSkillBook();
                    }
                    if (BKind == (byte)bKindBooks.FeBook)
                    {
                        b = new FiveElementBook();
                    }
                    if (BKind == (byte)bKindBooks.FocusBook)
                    {
                        b = new FocusBook();
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
                if (BType == (byte)bType.StoreTag)
                {
                    b = new StoreTag();

                    StoreTag tag = b as StoreTag;
                    tag.TimeLeft = reader.GetInt16(ordinalITEM_CURDURA);
                    tag.TimeMax = reader.GetInt16(ordinalITEM_MAXDURA);
                }
                if (BType == (byte)bType.PetItem)
                {
                    if (BKind == (byte)bKindPetItems.Taming)
                        b = new TameItem();
                    if (BKind == (byte)bKindPetItems.Food)
                        b = new PetFood();
                    if (BKind == (byte)bKindPetItems.Potion)
                        b = new PetPotion();
                    if (BKind == (byte)bKindPetItems.Resurect)
                        b = new PetResurrectItem();

                    PetItem p = b as PetItem;
                    p.TameChance = reader.GetInt16(ordinalITEM_IMBUERATE);
                    p.DecreaseWildness = reader.GetInt16(ordinalITEM_IMBUEINCREASE);
                    p.HealLife = reader.GetInt16(ordinalITEM_INCMAXLIFE);
                }
                if (BType == (byte)bType.Pill)
                {
                    if (BKind == (byte)bKindPills.Rebirth)
                        b = new RebirthPill();

                    RebirthPill p = b as RebirthPill;
                    p.RequiredLevel = reader.GetInt16(ordinalITEM_LEVEL);
                    p.RequiredRebirth = reader.GetByte(ordinalITEM_CLASS);
                    p.ToRebirth = (byte)(p.RequiredRebirth + 1);
                    p.IncreaseSp = reader.GetInt16(ordinalITEM_DEX);
                }

                b.ItemID = reader.GetInt32(ordinalITEM_ITEMID);
                b.OwnerID = reader.GetInt32(ordinalITEM_OWNERID);
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
                b.Price = reader.GetInt32(ordinalITEM_COST);
                b.SellPrice = reader.GetInt32(ordinalITEM_SHOPPRICE);

                items.Add(b);
            }

            reader.Close();
            _db.Close();

            return items;
        }

        public BaseItem GetBookDropItem(Monster m)
        {
            DbParameter monsterTypeParameter = _db.CreateParameter(DbNames.GETBOOKDROPITEM_MONSTERTYPE, m.MonsterType);
            monsterTypeParameter.DbType = DbType.Byte;

            List<BaseItem> items = new List<BaseItem>();

            _db.Open();

            DbDataReader reader = _db.ExcecuteReader(DbNames.GETBOOKDROPITEM_STOREDPROC, CommandType.StoredProcedure, monsterTypeParameter);

            int ordinalITEM_REFERENCEID = reader.GetOrdinal(DbNames.ITEM_REFERENCEID);
            int ordinalITEM_BTYPE = reader.GetOrdinal(DbNames.ITEM_BTYPE);
            int ordinalITEM_BKIND = reader.GetOrdinal(DbNames.ITEM_BKIND);
            int ordinalITEM_VISUALID = reader.GetOrdinal(DbNames.ITEM_VISUALID);
            int ordinalITEM_COST = reader.GetOrdinal(DbNames.ITEM_COST);
            int ordinalITEM_CLASS = reader.GetOrdinal(DbNames.ITEM_CLASS);
            int ordinalITEM_LEVEL = reader.GetOrdinal(DbNames.ITEM_LEVEL);
            int ordinalITEM_DEX = reader.GetOrdinal(DbNames.ITEM_DEX);
            int ordinalITEM_STR = reader.GetOrdinal(DbNames.ITEM_STR);
            int ordinalITEM_STA = reader.GetOrdinal(DbNames.ITEM_STA);
            int ordinalITEM_ENE = reader.GetOrdinal(DbNames.ITEM_ENE);
            int ordinalITEM_MAXIMBUES = reader.GetOrdinal(DbNames.ITEM_MAXIMBUES);
            int ordinalITEM_MAXDURA = reader.GetOrdinal(DbNames.DROPITEM_DURABILITY);
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
            int ordinalITEM_POLISHTRIES = reader.GetOrdinal(DbNames.ITEM_POLISHTRIES);
            int ordinalITEM_SIZEX = reader.GetOrdinal(DbNames.ITEM_SIZEX);
            int ordinalITEM_SIZEY = reader.GetOrdinal(DbNames.ITEM_SIZEY);

            while (reader.Read())
            {
                BaseItem b = null;

                int BType = reader.GetByte(ordinalITEM_BTYPE);
                int BKind = reader.GetByte(ordinalITEM_BKIND);

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
                    if (BKind == (byte)bKindBooks.RebirdBook)
                    {
                        b = new RebirthBook();
                    }
                    if (BKind == (byte)bKindBooks.FourthBook)
                    {
                        b = new FourthBook();
                    }
                    if (BKind == (byte)bKindBooks.FeSkillBook)
                    {
                        b = new FeSkillBook();
                    }
                    if (BKind == (byte)bKindBooks.FeBook)
                    {
                        b = new FiveElementBook();
                    }
                    if (BKind == (byte)bKindBooks.FocusBook)
                    {
                        b = new FocusBook();
                    }

                    BookItem book = b as BookItem;
                    book.RequiredClass = reader.GetByte(ordinalITEM_CLASS);
                    book.RequiredLevel = reader.GetInt16(ordinalITEM_LEVEL);
                    book.SkillID = reader.GetInt32(ordinalITEM_BOOKSKILLID);
                    book.SkillLevel = reader.GetByte(ordinalITEM_BOOKSKILLLEVEL);
                    book.SkillData = reader.GetInt32(ordinalITEM_BOOKSKILLDATA);
                }

                b.ItemID = 0;
                b.OwnerID = 0;
                b.ReferenceID = reader.GetInt16(ordinalITEM_REFERENCEID);
                b.VisualID = reader.GetInt16(ordinalITEM_VISUALID);
                b.Bag = 0;
                b.Slot = 0;
                b.bType = reader.GetByte(ordinalITEM_BTYPE);
                b.bKind = reader.GetByte(ordinalITEM_BKIND);
                b.RequiredClass = reader.GetByte(ordinalITEM_CLASS);
                b.Amount = 1;
                b.SizeX = reader.GetByte(ordinalITEM_SIZEX);
                b.SizeY = reader.GetByte(ordinalITEM_SIZEY);
                b.Price = reader.GetInt32(ordinalITEM_COST);
                items.Add(b);
            }

            reader.Close();
            _db.Close();


            if (items.Count > 0)
            {
                Random rand = new Random();
                int itemPos = rand.Next(0, items.Count);
                return items[itemPos];
            }
            else
                return null;
        }

        public BaseItem GetImbueDropItem()
        {
            List<BaseItem> items = new List<BaseItem>();

            _db.Open();

            DbDataReader reader = _db.ExcecuteReader(DbNames.GETIMBUEDROPITEM_STOREDPROC, CommandType.StoredProcedure);

            int ordinalITEM_REFERENCEID = reader.GetOrdinal(DbNames.ITEM_REFERENCEID);
            int ordinalITEM_BTYPE = reader.GetOrdinal(DbNames.ITEM_BTYPE);
            int ordinalITEM_BKIND = reader.GetOrdinal(DbNames.ITEM_BKIND);
            int ordinalITEM_VISUALID = reader.GetOrdinal(DbNames.ITEM_VISUALID);
            int ordinalITEM_COST = reader.GetOrdinal(DbNames.ITEM_COST);
            int ordinalITEM_CLASS = reader.GetOrdinal(DbNames.ITEM_CLASS);
            int ordinalITEM_LEVEL = reader.GetOrdinal(DbNames.ITEM_LEVEL);
            int ordinalITEM_DEX = reader.GetOrdinal(DbNames.ITEM_DEX);
            int ordinalITEM_STR = reader.GetOrdinal(DbNames.ITEM_STR);
            int ordinalITEM_STA = reader.GetOrdinal(DbNames.ITEM_STA);
            int ordinalITEM_ENE = reader.GetOrdinal(DbNames.ITEM_ENE);
            int ordinalITEM_MAXIMBUES = reader.GetOrdinal(DbNames.ITEM_MAXIMBUES);
            int ordinalITEM_MAXDURA = reader.GetOrdinal(DbNames.DROPITEM_DURABILITY);
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
            int ordinalITEM_POLISHTRIES = reader.GetOrdinal(DbNames.ITEM_POLISHTRIES);
            int ordinalITEM_SIZEX = reader.GetOrdinal(DbNames.ITEM_SIZEX);
            int ordinalITEM_SIZEY = reader.GetOrdinal(DbNames.ITEM_SIZEY);

            while (reader.Read())
            {
                BaseItem b = null;

                int BType = reader.GetByte(ordinalITEM_BTYPE);
                int BKind = reader.GetByte(ordinalITEM_BKIND);

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
                    if (BKind == (byte)bKindStones.RbItem)
                    {
                        b = new RbHoleItem();
                    }

                    ImbueItem im = b as ImbueItem;
                    im.ImbueChance = reader.GetInt16(ordinalITEM_IMBUERATE);
                    im.IncreaseValue = reader.GetInt16(ordinalITEM_IMBUEINCREASE);
                }

                b.ItemID = 0;
                b.OwnerID = 0;
                b.ReferenceID = reader.GetInt16(ordinalITEM_REFERENCEID);
                b.VisualID = reader.GetInt16(ordinalITEM_VISUALID);
                b.Bag = 0;
                b.Slot = 0;
                b.bType = reader.GetByte(ordinalITEM_BTYPE);
                b.bKind = reader.GetByte(ordinalITEM_BKIND);
                b.RequiredClass = reader.GetByte(ordinalITEM_CLASS);
                b.Amount = 1;
                b.SizeX = reader.GetByte(ordinalITEM_SIZEX);
                b.SizeY = reader.GetByte(ordinalITEM_SIZEY);
                b.Price = reader.GetInt32(ordinalITEM_COST);
                items.Add(b);
            }

            reader.Close();
            _db.Close();


            if (items.Count > 0)
            {
                Random rand = new Random();
                int itemPos = rand.Next(0, items.Count);
                return items[itemPos];
            }
            else
                return null;
        }

        public BaseItem GetRebirthPillDrop(Monster m)
        {
            DbParameter levelParameter = _db.CreateParameter(DbNames.GETPILLDROPITEM_LEVEL_PARAMETER, m.Level);
            levelParameter.DbType = DbType.Int32;

            List<BaseItem> items = new List<BaseItem>();

            _db.Open();

            DbDataReader reader = _db.ExcecuteReader(DbNames.GETPILLDROPITEM_STOREDPROC, CommandType.StoredProcedure, levelParameter);

            int ordinalITEM_REFERENCEID = reader.GetOrdinal(DbNames.ITEM_REFERENCEID);
            int ordinalITEM_BTYPE = reader.GetOrdinal(DbNames.ITEM_BTYPE);
            int ordinalITEM_BKIND = reader.GetOrdinal(DbNames.ITEM_BKIND);
            int ordinalITEM_VISUALID = reader.GetOrdinal(DbNames.ITEM_VISUALID);
            int ordinalITEM_COST = reader.GetOrdinal(DbNames.ITEM_COST);
            int ordinalITEM_CLASS = reader.GetOrdinal(DbNames.ITEM_CLASS);
            int ordinalITEM_LEVEL = reader.GetOrdinal(DbNames.ITEM_LEVEL);
            int ordinalITEM_DEX = reader.GetOrdinal(DbNames.ITEM_DEX);
            int ordinalITEM_STR = reader.GetOrdinal(DbNames.ITEM_STR);
            int ordinalITEM_STA = reader.GetOrdinal(DbNames.ITEM_STA);
            int ordinalITEM_ENE = reader.GetOrdinal(DbNames.ITEM_ENE);
            int ordinalITEM_MAXIMBUES = reader.GetOrdinal(DbNames.ITEM_MAXIMBUES);
            int ordinalITEM_MAXDURA = reader.GetOrdinal(DbNames.DROPITEM_DURABILITY);
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
            int ordinalITEM_POLISHTRIES = reader.GetOrdinal(DbNames.ITEM_POLISHTRIES);
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
                        c.PolishImbueTries = reader.GetByte(ordinalITEM_POLISHTRIES);
                    }



                    Equipment e = b as Equipment;
                    e.RequiredLevel = reader.GetInt16(ordinalITEM_LEVEL);
                    e.RequiredDexterity = reader.GetInt16(ordinalITEM_DEX);
                    e.RequiredStrength = reader.GetInt16(ordinalITEM_STR);
                    e.RequiredStamina = reader.GetInt16(ordinalITEM_STA);
                    e.RequiredEnergy = reader.GetInt16(ordinalITEM_ENE);
                    e.MaxImbueTries = reader.GetByte(ordinalITEM_MAXIMBUES);
                    e.Durability = reader.GetInt32(ordinalITEM_MAXDURA);
                    e.MaxDurability = reader.GetInt32(ordinalITEM_MAXDURA);
                    e.Damage = reader.GetInt32(ordinalITEM_DAMAGE);
                    e.Defence = reader.GetInt32(ordinalITEM_DEFENCE);
                    e.AttackRating = reader.GetInt32(ordinalITEM_ATTACKRATING);
                    e.AttackSpeed = reader.GetInt16(ordinalITEM_ATTACKSPEED);
                    e.AttackRange = reader.GetInt16(ordinalITEM_ATTACKRANGE);
                    e.IncMaxLife = reader.GetInt16(ordinalITEM_INCMAXLIFE);
                    e.IncMaxMana = reader.GetInt16(ordinalITEM_INCMAXMANA);
                    e.IncLifeRegen = reader.GetInt16(ordinalITEM_LIFEREGEN);
                    e.IncManaRegen = reader.GetInt16(ordinalITEM_MANAREGEN);
                    e.Critical = reader.GetInt16(ordinalITEM_CRITICAL);
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
                    if (BKind == (byte)bKindStones.RbItem)
                    {
                        b = new RbHoleItem();
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
                    if (BKind == (byte)bKindBooks.RebirdBook)
                    {
                        b = new RebirthBook();
                    }
                    if (BKind == (byte)bKindBooks.FourthBook)
                    {
                        b = new FourthBook();
                    }
                    if (BKind == (byte)bKindBooks.FeSkillBook)
                    {
                        b = new FeSkillBook();
                    }
                    if (BKind == (byte)bKindBooks.FeBook)
                    {
                        b = new FiveElementBook();
                    }
                    if (BKind == (byte)bKindBooks.FocusBook)
                    {
                        b = new FocusBook();
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
                if (BType == (byte)bType.StoreTag)
                {
                    b = new StoreTag();

                    StoreTag tag = b as StoreTag;
                    tag.TimeLeft = reader.GetInt16(ordinalITEM_MAXDURA);
                    tag.TimeMax = reader.GetInt16(ordinalITEM_MAXDURA);
                }
                if (BType == (byte)bType.PetItem)
                {
                    if (BKind == (byte)bKindPetItems.Taming)
                        b = new TameItem();
                    if (BKind == (byte)bKindPetItems.Food)
                        b = new PetFood();
                    if (BKind == (byte)bKindPetItems.Potion)
                        b = new PetPotion();
                    if (BKind == (byte)bKindPetItems.Resurect)
                        b = new PetResurrectItem();

                    PetItem p = b as PetItem;
                    p.TameChance = reader.GetInt16(ordinalITEM_IMBUERATE);
                    p.DecreaseWildness = reader.GetInt16(ordinalITEM_IMBUEINCREASE);
                    p.HealLife = reader.GetInt16(ordinalITEM_INCMAXLIFE);
                } 
                if (BType == (byte)bType.Pill)
                {
                    if (BKind == (byte)bKindPills.Rebirth)
                        b = new RebirthPill();

                    RebirthPill p = b as RebirthPill;
                    p.RequiredLevel = reader.GetInt16(ordinalITEM_LEVEL);
                    p.RequiredRebirth = reader.GetByte(ordinalITEM_CLASS);
                    p.ToRebirth = (byte)(p.RequiredRebirth + 1);
                    p.IncreaseSp = reader.GetInt16(ordinalITEM_DEX);
                }

                b.ItemID = 0;
                b.OwnerID = 0;
                b.ReferenceID = reader.GetInt16(ordinalITEM_REFERENCEID);
                b.VisualID = reader.GetInt16(ordinalITEM_VISUALID);
                b.Bag = 0;
                b.Slot = 0;
                b.bType = reader.GetByte(ordinalITEM_BTYPE);
                b.bKind = reader.GetByte(ordinalITEM_BKIND);
                b.RequiredClass = reader.GetByte(ordinalITEM_CLASS);
                b.Amount = 1;
                b.SizeX = reader.GetByte(ordinalITEM_SIZEX);
                b.SizeY = reader.GetByte(ordinalITEM_SIZEY);
                b.Price = reader.GetInt32(ordinalITEM_COST);
                items.Add(b);
            }

            reader.Close();
            _db.Close();

            if (items.Count > 0)
            {
                Random rand = new Random();
                int itemPos = rand.Next(0, items.Count);
                return items[itemPos];
            }
            else
                return null;
        }


        public BaseItem GetMonsterDropItem(Monster m)
        {
            DbParameter levelParameter = _db.CreateParameter(DbNames.GETMONSTERDROPITEM_MONSTERLEVEL_PARAMETER, m.Level);
            levelParameter.DbType = DbType.Int32;

            List<BaseItem> items = new List<BaseItem>();

            _db.Open();

            DbDataReader reader = _db.ExcecuteReader(DbNames.GETMONSTERDROPITEM_STOREDPROC, CommandType.StoredProcedure, levelParameter);

            int ordinalITEM_REFERENCEID = reader.GetOrdinal(DbNames.ITEM_REFERENCEID);
            int ordinalITEM_BTYPE = reader.GetOrdinal(DbNames.ITEM_BTYPE);
            int ordinalITEM_BKIND = reader.GetOrdinal(DbNames.ITEM_BKIND);
            int ordinalITEM_VISUALID = reader.GetOrdinal(DbNames.ITEM_VISUALID);
            int ordinalITEM_COST = reader.GetOrdinal(DbNames.ITEM_COST);
            int ordinalITEM_CLASS = reader.GetOrdinal(DbNames.ITEM_CLASS);
            int ordinalITEM_LEVEL = reader.GetOrdinal(DbNames.ITEM_LEVEL);
            int ordinalITEM_DEX = reader.GetOrdinal(DbNames.ITEM_DEX);
            int ordinalITEM_STR = reader.GetOrdinal(DbNames.ITEM_STR);
            int ordinalITEM_STA = reader.GetOrdinal(DbNames.ITEM_STA);
            int ordinalITEM_ENE = reader.GetOrdinal(DbNames.ITEM_ENE);
            int ordinalITEM_MAXIMBUES = reader.GetOrdinal(DbNames.ITEM_MAXIMBUES);
            int ordinalITEM_MAXDURA = reader.GetOrdinal(DbNames.DROPITEM_DURABILITY);
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
            int ordinalITEM_POLISHTRIES = reader.GetOrdinal(DbNames.ITEM_POLISHTRIES);
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
                        c.PolishImbueTries = reader.GetByte(ordinalITEM_POLISHTRIES);
                    }



                    Equipment e = b as Equipment;
                    e.RequiredLevel = reader.GetInt16(ordinalITEM_LEVEL);
                    e.RequiredDexterity = reader.GetInt16(ordinalITEM_DEX);
                    e.RequiredStrength = reader.GetInt16(ordinalITEM_STR);
                    e.RequiredStamina = reader.GetInt16(ordinalITEM_STA);
                    e.RequiredEnergy = reader.GetInt16(ordinalITEM_ENE);
                    e.MaxImbueTries = reader.GetByte(ordinalITEM_MAXIMBUES);
                    e.Durability = reader.GetInt32(ordinalITEM_MAXDURA);
                    e.MaxDurability = reader.GetInt32(ordinalITEM_MAXDURA);
                    e.Damage = reader.GetInt32(ordinalITEM_DAMAGE);
                    e.Defence = reader.GetInt32(ordinalITEM_DEFENCE);
                    e.AttackRating = reader.GetInt32(ordinalITEM_ATTACKRATING);
                    e.AttackSpeed = reader.GetInt16(ordinalITEM_ATTACKSPEED);
                    e.AttackRange = reader.GetInt16(ordinalITEM_ATTACKRANGE);
                    e.IncMaxLife = reader.GetInt16(ordinalITEM_INCMAXLIFE);
                    e.IncMaxMana = reader.GetInt16(ordinalITEM_INCMAXMANA);
                    e.IncLifeRegen = reader.GetInt16(ordinalITEM_LIFEREGEN);
                    e.IncManaRegen = reader.GetInt16(ordinalITEM_MANAREGEN);
                    e.Critical = reader.GetInt16(ordinalITEM_CRITICAL);
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
                    if (BKind == (byte)bKindStones.RbItem)
                    {
                        b = new RbHoleItem();
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
                    if (BKind == (byte)bKindBooks.RebirdBook)
                    {
                        b = new RebirthBook();
                    }
                    if (BKind == (byte)bKindBooks.FourthBook)
                    {
                        b = new FourthBook();
                    }
                    if (BKind == (byte)bKindBooks.FeSkillBook)
                    {
                        b = new FeSkillBook();
                    }
                    if (BKind == (byte)bKindBooks.FeBook)
                    {
                        b = new FiveElementBook();
                    }
                    if (BKind == (byte)bKindBooks.FocusBook)
                    {
                        b = new FocusBook();
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
                if (BType == (byte)bType.StoreTag)
                {
                    b = new StoreTag();

                    StoreTag tag = b as StoreTag;
                    tag.TimeLeft = reader.GetInt16(ordinalITEM_MAXDURA);
                    tag.TimeMax = reader.GetInt16(ordinalITEM_MAXDURA);
                }
                if (BType == (byte)bType.PetItem)
                {
                    if (BKind == (byte)bKindPetItems.Taming)
                        b = new TameItem();
                    if (BKind == (byte)bKindPetItems.Food)
                        b = new PetFood();
                    if (BKind == (byte)bKindPetItems.Potion)
                        b = new PetPotion();
                    if (BKind == (byte)bKindPetItems.Resurect)
                        b = new PetResurrectItem();

                    PetItem p = b as PetItem;
                    p.TameChance = reader.GetInt16(ordinalITEM_IMBUERATE);
                    p.DecreaseWildness = reader.GetInt16(ordinalITEM_IMBUEINCREASE);
                    p.HealLife = reader.GetInt16(ordinalITEM_INCMAXLIFE);
                }

                b.ItemID = 0;
                b.OwnerID = 0;
                b.ReferenceID = reader.GetInt16(ordinalITEM_REFERENCEID);
                b.VisualID = reader.GetInt16(ordinalITEM_VISUALID);
                b.Bag = 0;
                b.Slot = 0;
                b.bType = reader.GetByte(ordinalITEM_BTYPE);
                b.bKind = reader.GetByte(ordinalITEM_BKIND);
                b.RequiredClass = reader.GetByte(ordinalITEM_CLASS);
                b.Amount = 1;
                b.SizeX = reader.GetByte(ordinalITEM_SIZEX);
                b.SizeY = reader.GetByte(ordinalITEM_SIZEY);
                b.Price = reader.GetInt32(ordinalITEM_COST);
                items.Add(b);
            }

            reader.Close();
            _db.Close();


            if (items.Count > 0)
            {
                Random rand = new Random();
                int itemPos = rand.Next(0, items.Count);
                return items[itemPos];
            }
            else
                return null;
        }
    }
}