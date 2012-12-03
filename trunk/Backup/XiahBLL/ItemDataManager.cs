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
            int ordinalITEM_REBIRTHHOLEITEM = reader.GetOrdinal(DbNames.ITEM_REBIRTHHOLEITEM);
            int ordinalITEM_REBIRTHHOLESTAT = reader.GetOrdinal(DbNames.ITEM_REBIRTHHOLESTAT);
            int ordinalITEM_TOMAPID = reader.GetOrdinal(DbNames.ITEM_TOMAPID);
            int ordinalITEM_IMBUERATE = reader.GetOrdinal(DbNames.ITEM_IMBUERATE);
            int ordinalITEM_IMBUEINCREASE = reader.GetOrdinal(DbNames.ITEM_IMBUEINCREASE);
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
            int ordinalITEM_BAG = reader.GetOrdinal(DbNames.ITEM_BAG);
            int ordinalITEM_SLOT = reader.GetOrdinal(DbNames.ITEM_SLOT);
            int ordinalITEM_SIZEX = reader.GetOrdinal(DbNames.ITEM_SIZEX);
            int ordinalITEM_SIZEY = reader.GetOrdinal(DbNames.ITEM_SIZEY);

            while (reader.Read())
            {
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
                        c.VigiStat1 = reader.GetInt16(ordinalITEM_VIGISTAT1);
                        c.VigiStatAdd1 = reader.GetInt16(ordinalITEM_VIGISTATADD1);
                        c.VigiStat2 = reader.GetInt16(ordinalITEM_VIGISTAT2);
                        c.VigiStatAdd2 = reader.GetInt16(ordinalITEM_VIGISTATADD2);
                        c.VigiStat3 = reader.GetInt16(ordinalITEM_VIGISTAT3);
                        c.VigiStatAdd3 = reader.GetInt16(ordinalITEM_VIGISTATADD3);
                        c.VigiStat4 = reader.GetInt16(ordinalITEM_VIGISTAT4);
                        c.VigiStatAdd4 = reader.GetInt16(ordinalITEM_VIGISTATADD4);
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
                    e.Plus = reader.GetByte(ordinalITEM_PLUS);
                    e.Slvl = reader.GetByte(ordinalITEM_SLVL);
                    e.ImbueTries = reader.GetByte(ordinalITEM_IMBUETRIES);
                    e.DragonSuccessImbueTries = reader.GetInt16(ordinalITEM_DRAGONSUCCESSIMBUETRIES);
                    e.DiscountRepairFee = reader.GetByte(ordinalITEM_DISCOUNTREPAIRFEE);
                    e.TotalDragonImbueTries = reader.GetInt16(ordinalITEM_TOTALDRAGONIMBUES);
                    e.DragonDamage = reader.GetInt16(ordinalITEM_DRAGONDAMAGE);
                    e.DragonDefence = reader.GetInt16(ordinalITEM_DRAGONDEFENCE);
                    e.DragonAttackRating = reader.GetInt16(ordinalITEM_DRAGONATTACKRATING);
                    e.DragonLife = reader.GetInt16(ordinalITEM_DRAGONLIFE);
                    e.MappedData = reader.GetByte(ordinalITEM_MAPPEDSTUFF);
                    e.ForceSlot = reader.GetByte(ordinalITEM_FORCENUMBER);
                    e.RebirthHole = reader.GetByte(ordinalITEM_REBIRTHHOLE);
                    e.RebirthHoleItem = reader.GetByte(ordinalITEM_REBIRTHHOLEITEM);
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
            //DbParameter ownerIdParameter = _db.CreateParameter(DbNames.UPDATEITEM_OWNERID_PARAMETER, item.OwnerID);
            //ownerIdParameter.DbType = DbType.Int32;

            //DbParameter costParameter = _db.CreateParameter(DbNames.UPDATEITEM_COST_PARAMETER, item.Price);
            //costParameter.DbType = DbType.Int32;

            //DbParameter reqClassParameter = _db.CreateParameter(DbNames.UPDATEITEM_REQCLASS_PARAMETER, item.RequiredClass);

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
            DbParameter ReqDexParameter = _db.CreateParameter(DbNames.UPDATEITEM_REQDEX_PARAMETER, DbType.Int16);
            DbParameter ReqStrParameter = _db.CreateParameter(DbNames.UPDATEITEM_REQSTR_PARAMETER, DbType.Int16);
            DbParameter ReqStaParameter = _db.CreateParameter(DbNames.UPDATEITEM_REQSTA_PARAMETER, DbType.Int16);
            DbParameter ReqEneParameter = _db.CreateParameter(DbNames.UPDATEITEM_REQENE_PARAMETER, DbType.Int16);
            DbParameter MaxImbueTriesParameter = _db.CreateParameter(DbNames.UPDATEITEM_MAXIMBUETRIES_PARAMETER, DbType.Byte);
            DbParameter MaxDuraParameter = _db.CreateParameter(DbNames.UPDATEITEM_MAXDURA_PARAMETER, DbType.Int16);
            DbParameter CurDuraParameter = _db.CreateParameter(DbNames.UPDATEITEM_CURDURA_PARAMETER, DbType.Int16);
            DbParameter DamageParameter = _db.CreateParameter(DbNames.UPDATEITEM_DAMAGE_PARAMETER, DbType.Int16);
            DbParameter DefenceParameter = _db.CreateParameter(DbNames.UPDATEITEM_DEFENCE_PARAMETER, DbType.Int16);
            DbParameter AttackRatingParameter = _db.CreateParameter(DbNames.UPDATEITEM_ATTACKRATING_PARAMETER, DbType.Int16);
            DbParameter AttackSpeedParameter = _db.CreateParameter(DbNames.UPDATEITEM_ATTACKSPEED_PARAMETER, DbType.Int16);
            DbParameter AttackRangeParameter = _db.CreateParameter(DbNames.UPDATEITEM_ATTACKRANGE_PARAMETER, DbType.Int16);
            DbParameter IncMaxLifeParameter = _db.CreateParameter(DbNames.UPDATEITEM_INCMAXLIFE_PARAMETER, DbType.Int16);
            DbParameter IncMaxManaParameter = _db.CreateParameter(DbNames.UPDATEITEM_INCMAXMANA_PARAMETER, DbType.Int16);
            DbParameter LifeRegenParameter = _db.CreateParameter(DbNames.UPDATEITEM_LIFEREGEN_PARAMETER, DbType.Int16);
            DbParameter ManaRegenParameter = _db.CreateParameter(DbNames.UPDATEITEM_MANAREGEN_PARAMETER, DbType.Int16);
            DbParameter CriticalHitParameter = _db.CreateParameter(DbNames.UPDATEITEM_CRITICALHIT_PARAMETER, DbType.Int16);
            DbParameter PlusParameter = _db.CreateParameter(DbNames.UPDATEITEM_PLUS_PARAMETER, DbType.Byte);
            DbParameter SlvlParameter = _db.CreateParameter(DbNames.UPDATEITEM_SLVL_PARAMETER, DbType.Byte);
            DbParameter ImbueTriesParameter = _db.CreateParameter(DbNames.UPDATEITEM_IMBUETRIES_PARAMETER, DbType.Byte);
            DbParameter DragonSuccessImbueTriesParameter = _db.CreateParameter(DbNames.UPDATEITEM_DRAGONSUCCESSIMBUETRIES_PARAMETER, DbType.Int16);
            DbParameter DiscountRepairFeeParameter = _db.CreateParameter(DbNames.UPDATEITEM_DISCOUNTREPAIRFEE_PARAMETER, DbType.Byte);
            DbParameter TotalDragonImbueTriesParameter = _db.CreateParameter(DbNames.UPDATEITEM_TOTALDRAGONIMBUETRIES_PARAMETER, DbType.Int16);
            DbParameter DragonDmgParameter = _db.CreateParameter(DbNames.UPDATEITEM_DRAGONDMG_PARAMETER, DbType.Int16);
            DbParameter DragonDefParameter = _db.CreateParameter(DbNames.UPDATEITEM_DRAGONDEF_PARAMETER, DbType.Int16);
            DbParameter DragonARParameter = _db.CreateParameter(DbNames.UPDATEITEM_DRAGONAR_PARAMETER, DbType.Int16);
            DbParameter DragonLifeParameter = _db.CreateParameter(DbNames.UPDATEITEM_DRAGONLIFE_PARAMETER, DbType.Int16);
            DbParameter MappedStuffParameter = _db.CreateParameter(DbNames.UPDATEITEM_MAPPEDSTUFF_PARAMETER, DbType.Byte);
            DbParameter ForceNumberParameter = _db.CreateParameter(DbNames.UPDATEITEM_FORCENUMBER_PARAMETER, DbType.Byte);
            DbParameter RebirthHoleParameter = _db.CreateParameter(DbNames.UPDATEITEM_REBIRTHHOLE_PARAMETER, DbType.Byte);
            DbParameter RebirthHoleItemParameter = _db.CreateParameter(DbNames.UPDATEITEM_REBIRTHHOLEITEM_PARAMETER, DbType.Byte);
            DbParameter RebirthHoleStatParameter = _db.CreateParameter(DbNames.UPDATEITEM_REBIRTHHOLESTAT_PARAMETER, DbType.Int16);

            DbParameter PolishStoneTriesParameter = _db.CreateParameter(DbNames.UPDATEITEM_POLISHSTONETRIES_PARAMETER, DbType.Byte);
            DbParameter VigiStat1Parameter = _db.CreateParameter(DbNames.UPDATEITEM_VIGISTAT1_PARAMETER, DbType.Int16);
            DbParameter VigiStat2Parameter = _db.CreateParameter(DbNames.UPDATEITEM_VIGISTAT2_PARAMETER, DbType.Int16);
            DbParameter VigiStat3Parameter = _db.CreateParameter(DbNames.UPDATEITEM_VIGISTAT3_PARAMETER, DbType.Int16);
            DbParameter VigiStat4Parameter = _db.CreateParameter(DbNames.UPDATEITEM_VIGISTAT4_PARAMETER, DbType.Int16);
            DbParameter VigiStatAdd1Parameter = _db.CreateParameter(DbNames.UPDATEITEM_VIGISTATADD1_PARAMETER, DbType.Int16);
            DbParameter VigiStatAdd2Parameter = _db.CreateParameter(DbNames.UPDATEITEM_VIGISTATADD2_PARAMETER, DbType.Int16);
            DbParameter VigiStatAdd3Parameter = _db.CreateParameter(DbNames.UPDATEITEM_VIGISTATADD3_PARAMETER, DbType.Int16);
            DbParameter VigiStatAdd4Parameter = _db.CreateParameter(DbNames.UPDATEITEM_VIGISTATADD4_PARAMETER, DbType.Int16);

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
                RebirthHoleItemParameter.Value = e.RebirthHoleItem;
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
            }

            DbParameter ImbueRateParameter = _db.CreateParameter(DbNames.UPDATEITEM_IMBUERATE_PARAMETER, DbType.Int16);
            DbParameter ImbueIncreaseParameter = _db.CreateParameter(DbNames.UPDATEITEM_IMBUEINCREASE_PARAMETER, DbType.Int16);

            if (item is ImbueItem)
            {
                ImbueItem i = item as ImbueItem;

                ImbueRateParameter.Value = i.ImbueChance;
                ImbueIncreaseParameter.Value = i.IncreaseValue;
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
              RebirthHoleItemParameter,
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
              ImbueIncreaseParameter,
              ImbueRateParameter
              );

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
            int ordinalITEM_REBIRTHHOLEITEM = reader.GetOrdinal(DbNames.ITEM_REBIRTHHOLEITEM);
            int ordinalITEM_REBIRTHHOLESTAT = reader.GetOrdinal(DbNames.ITEM_REBIRTHHOLESTAT);
            int ordinalITEM_TOMAPID = reader.GetOrdinal(DbNames.ITEM_TOMAPID);
            int ordinalITEM_IMBUERATE = reader.GetOrdinal(DbNames.ITEM_IMBUERATE);
            int ordinalITEM_IMBUEINCREASE = reader.GetOrdinal(DbNames.ITEM_IMBUEINCREASE);
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



                    Equipment e = b as Equipment;
                    e.RequiredLevel = reader.GetInt16(ordinalITEM_LEVEL);
                    e.RequiredDexterity = reader.GetInt16(ordinalITEM_DEX);
                    e.RequiredStrength = reader.GetInt16(ordinalITEM_STR);
                    e.RequiredStamina = reader.GetInt16(ordinalITEM_STA);
                    e.RequiredEnergy = reader.GetInt16(ordinalITEM_ENE);
                    e.MaxImbueTries = reader.GetByte(ordinalITEM_MAXIMBUES);
                    e.Durability = reader.GetInt16(ordinalITEM_CURDURA);
                    e.MaxDurability = reader.GetInt16(ordinalITEM_MAXDURA);
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
                    e.Plus = reader.GetByte(ordinalITEM_PLUS);
                    e.Slvl = reader.GetByte(ordinalITEM_SLVL);
                    e.ImbueTries = reader.GetByte(ordinalITEM_IMBUETRIES);
                    e.DragonSuccessImbueTries = reader.GetInt16(ordinalITEM_DRAGONSUCCESSIMBUETRIES);
                    e.DiscountRepairFee = reader.GetByte(ordinalITEM_DISCOUNTREPAIRFEE);
                    e.TotalDragonImbueTries = reader.GetInt16(ordinalITEM_TOTALDRAGONIMBUES);
                    e.DragonDamage = reader.GetInt16(ordinalITEM_DRAGONDAMAGE);
                    e.DragonDefence = reader.GetInt16(ordinalITEM_DRAGONDEFENCE);
                    e.DragonAttackRating = reader.GetInt16(ordinalITEM_DRAGONATTACKRATING);
                    e.DragonLife = reader.GetInt16(ordinalITEM_DRAGONLIFE);
                    e.MappedData = reader.GetByte(ordinalITEM_MAPPEDSTUFF);
                    e.ForceSlot = reader.GetByte(ordinalITEM_FORCENUMBER);
                    e.RebirthHole = reader.GetByte(ordinalITEM_REBIRTHHOLE);
                    e.RebirthHoleItem = reader.GetByte(ordinalITEM_REBIRTHHOLEITEM);
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
    }
}