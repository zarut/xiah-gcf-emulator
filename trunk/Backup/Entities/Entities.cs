using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Collections;

namespace Entities
{
    public class GameLogin
    {
        public string AccountName { get; set; }
        public string HashCode { get; set; }
        public int Version { get; set; }
    }

    public class UnitLogin
    {
        public int AccountID { get; set; }
        public string Account { get; set; }
        public int Channel { get; set; }
        public int CharacterID { get; set; }
    }

    public class ChannelLogin
    {
        public int CharacterID { get; set; }
        public int AccountID { get; set; }
        public int ChannelID { get; set; }
        public int WorldID { get; set; }
    }

    public class MapRequestInfo
    {
        public int CharacterID { get; set; }
        public int MapID { get; set; }
    }

    public class ChannelChangeInfo
    {
        public byte Something { get; set; }
        public byte Something2 { get; set; }
    }

    public class StatRequestInfo
    {
        public int CharacterID { get; set; }
        public int MapID { get; set; }
    }

    public class SpawnRequestInfo
    {
        public int CharacterID { get; set; }
        public int MapID { get; set; }
    }

    public class MovementInfo
    {
        public int PacketID { get; set; }
        public int CharacterID { get; set; }
        public short FromX { get; set; }
        public short FromY { get; set; }
        public byte FromZ { get; set; }
        public short ToX { get; set; }
        public short ToY { get; set; }
        public byte ToZ { get; set; }
        public int Rotation { get; set; }
    }

    public class WorldInfo
    {
        public int WorldId { get; set; }
        public string WorldName { get; set; }
        public string WorldDesc { get; set; }
        public string IPAddress { get; set; }
        public int Port { get; set; }
    }

    public class ChannelInfo
    {
        public int ChannelId { get; set; }
        public string ChannelName { get; set; }
        public string ChannelDesc { get; set; }
        public string IPAddress { get; set; }
        public int Port { get; set; }
        public bool PK { get; set; }
    }

    public class ChatMessageInfo
    {
        public ChatType Type { get; set; }
        public int TargetID { get; set; }
        public string Message { get; set; }
        public string TargetName { get; set; }
    }

    public class CommandInfo
    {
        public byte Type { get; set; }
        public byte Action { get; set; }
        public byte Something { get; set; }
        public byte Something2 { get; set; }
    }

    public class MoveItemInfo
    {
        public byte FromBag { get; set; }
        public byte FromSlot { get; set; }
        public int ItemID { get; set; }
        public byte ToBag { get; set; }
        public byte ToSlot { get; set; }
        public int ItemIDUnder { get; set; }
    }

    public class DropItemInfo
    {
        public byte FromBag { get; set; }
        public byte FromSlot { get; set; }
        public int ItemID { get; set; }
        public short ToX { get; set; }
        public short ToY { get; set; }
        public byte ToZ { get; set; }
        public int Something { get; set; }
    }

    public class PickItemInfo
    {
        public int MapID { get; set; }
        public int ItemID { get; set; }
        public short FromX { get; set; }
        public short FromY { get; set; }
        public byte FromZ { get; set; }
        public int MapItemID { get; set; }
        public short Amount { get; set; }
    }

    public class ImbueItemInfo
    {
        public int ItemID { get; set; }
        public byte Bag { get; set; }
        public byte Slot { get; set; }
    }

    public class AcceptImbueItem
    {
        public int NpcID { get; set; }
        public int ToImbueItemID { get; set; }
        public byte ToImbueItemBag { get; set; }
        public byte ToImbueItemSlot { get; set; }

        public int ImbueItem1ID { get; set; }
        public byte ImbueItem1Bag { get; set; }
        public byte ImbueItem1Slot { get; set; }

        public int ImbueItem2ID { get; set; }
        public byte ImbueItem2Bag { get; set; }
        public byte ImbueItem2Slot { get; set; }

        public int ImbueItem3ID { get; set; }
        public byte ImbueItem3Bag { get; set; }
        public byte ImbueItem3Slot { get; set; }
    }

    public class NpcTradeInfo
    {
        public int MapID { get; set; }
        public int NpcID { get; set; }
        public byte Bag { get; set; }
    }

    public class BuyItemInfo
    {
        public int NpcID { get; set; }
        public short ReferenceID { get; set; }
        public short Amount { get; set; }
        public byte Bag { get; set; }
    }

    public abstract class BaseItemContainer
    {
        private List<BaseItem> items;

        public int ItemCount { get { return items.Count; } }

        public BaseItem[] Items { get { return items.ToArray(); } }

        int slotCount;

        public int SlotCount { get { return slotCount; } }

        public BaseItemContainer(int slotCount, List<BaseItem> items)
        {
            this.slotCount = slotCount;
            this.items = items;
        }

        private bool HasNoRoom(BaseItem item, byte itemSlot, out List<BaseItem> conflictingItems)
        {
            bool full = false;

            conflictingItems = null;
            IEnumerable<BaseItem> q = null;
            if (item.SizeX > 1)
            {
                q = items.Where(x => x.Slot == itemSlot ||
                                    (x.Slot == itemSlot - 7 && x.SizeX > 1) ||
                                    (x.Slot == itemSlot - 6 && x.SizeX > 1) ||
                                    (x.Slot == itemSlot - 5 && x.SizeX > 1) ||
                                    (x.Slot == itemSlot - 1 && x.SizeX > 1) ||
                                    (x.Slot == itemSlot + 1 && x.SizeX > 1) ||
                                    (x.Slot == itemSlot + 5 && x.SizeX > 1) ||
                                    (x.Slot == itemSlot + 6 && x.SizeX > 1) ||
                                    (x.Slot == itemSlot + 7 && x.SizeX > 1));
                if (q.Count() > 0)
                    full = true;
            }
            else
            {
                q = items.Where(x => x.Slot == itemSlot ||
                                    (x.Slot == itemSlot - 1 && x.SizeX > 1) ||
                                    (x.Slot == itemSlot - 6 && x.SizeX > 1) ||
                                    (x.Slot == itemSlot - 7 && x.SizeX > 1));

                
                
                if (q.Count() > 0)
                    full = true;
            }

            conflictingItems = q.ToList();

            return full;
        }

        private bool IsSlotValid(BaseItem item, byte itemSlot)
        {
            if (item.SizeX > 1)
            {
                int invalidSlotsCount = 6 + ((slotCount / 6) - 1);



                int[] invalidSlots = new int[invalidSlotsCount];

                invalidSlots[0] = slotCount - 1;
                invalidSlots[1] = slotCount - 2;
                invalidSlots[2] = slotCount - 3;
                invalidSlots[3] = slotCount - 4;
                invalidSlots[4] = slotCount - 5;
                invalidSlots[5] = slotCount - 6;

                int invalidSlotNum = 5;

                for (int i = 6; i < invalidSlots.Length; i++)
                {
                    invalidSlots[i] = invalidSlotNum;
                    invalidSlotNum += 6;
                }

                var q = invalidSlots.Where(x => x == itemSlot);

                if (q.Count() > 0)
                {
                    return false;
                }
            }

            return true;
        }

        public bool CheckSlot(BaseItem item, byte slot)
        {
            bool empthy = false;

            List<BaseItem> conflictingItems = null;

            if (!HasNoRoom(item, slot, out conflictingItems))
                empthy = true;

            return empthy;
        }

        public bool MoveItem(BaseItem item, byte bag, byte slot, out BaseItem swappedItem)
        {
            bool moved = false;
            List<BaseItem> conflictingItems = null;
            swappedItem = null;

            if (!IsSlotValid(item, slot))
            {
                throw new InvalidItemSlotException("NO PUT ON EDGE PUJJ");
            }

            if (!HasNoRoom(item, slot, out conflictingItems))
            {
                moved = true;
                if (!this.items.Any(x => x.ItemID == item.ItemID))
                    this.AddItem(item);
            }
            else //later add swapping
            {
                BaseItem conflict = null;

                if (conflictingItems.Count == 1)
                {
                    conflict = conflictingItems.First();

                    if (conflict.Slot == slot)
                    {
                        if (conflict.SizeX == item.SizeX)
                        {
                            this.AddItem(item);
                            this.RemoveItem(conflict);

                            swappedItem = conflict;
                        }
                        else if(item.SizeX > 1 && item.Slot == conflict.Slot)
                        {
                            this.AddItem(item);
                            this.RemoveItem(conflict);

                            swappedItem = conflict;
                        } 
                        else if(item.SizeX==1)
                        {
                            this.AddItem(item);
                            this.RemoveItem(conflict);

                            swappedItem = conflict;
                        }
                    }
                    else
                    {
                        if (item.SizeX > 1)
                        {
                            
                        }
                        else
                        {
                            //no room plx
                        }
                    }
                }
                else
                {
                    var q = conflictingItems.Where(x=>x.Slot == item.Slot ||
                                                                  x.Slot+1 == item.Slot ||
                                                                  x.Slot+6 == item.Slot ||
                                                                  x.Slot+7 == item.Slot);

                    if(conflict.SizeX == 1 && conflict.SizeX >1)
                    {
                    }
                    else
                    {
                        //no room plx
                    }
                }

                
            }

            return moved;
        }

        public void AddItem(BaseItem item)
        {
            items.Add(item);
        }

        public bool FindFreeSlot(BaseItem item, BagSlot newBagSlot)
        {
            bool found = false;

            newBagSlot.Slot = 255;

            List<BaseItem> conflictingItems = null;

            for (byte i = 0; i < slotCount; i++)
            {
                if (!IsSlotValid(item, i))
                {
                    continue;
                }

                if (!HasNoRoom(item, i, out conflictingItems))
                {
                    newBagSlot.Slot = i;
                    found = true;
                    break;
                }

            }

            return found;
        }

        public bool PickItem(BaseItem item, BagSlot newBagSlot)
        {
            bool added = false;


            if (FindFreeSlot(item, newBagSlot))
            {
                added = true;
                items.Add(item);
            }

            return added;
        }

        public void RemoveItem(BaseItem item)
        {
            items.Remove(item);
        }
    }

    public class Bag : BaseItemContainer
    {
        public Bag(List<BaseItem> items)
            : base(36, items)
        {
        }
    }

    [Serializable]
    public class BaseEntity
    {
        public string Name { get; set; }
        public short X { get; set; }
        public short Y { get; set; }
    }

    public class Character : BaseEntity
    {
        protected short strStatModifier;
        protected short staminaStatModifier;
        protected short dexterityStatModifier;
        protected short energyStatModifier;
        protected short lifeLevelModifier;
        protected short manaLevelModifier;

        protected Head head;
        protected Body body;
        protected Hand hand;
        protected Feet feet;
        protected Cape cape;

        protected Ring ring;
        protected Necklace necklace;

        public Ring Ring
        {
            get
            {
                return ring;
            }
            set
            {
                if (value is Ring && this.Level >= value.RequiredLevel || value == null)
                {
                    if (value != null)
                    {
                        value.Bag = 0;
                        value.Slot = (byte)Slot.Ring;
                    }
                    ring = value;
                }
                else
                    throw new RequiredClassException(Messages.REQUIREDCLASSEXCEPTION);
            }
        }

        public Necklace Necklace
        {
            get
            {
                return necklace;
            }
            set
            {
                if (value is Necklace && this.Level >= value.RequiredLevel || value == null)
                {
                    if (value != null)
                    {
                        value.Bag = 0;
                        value.Slot = (byte)Slot.Necklace;
                    }
                    necklace = value;
                }
                else
                    throw new RequiredClassException(Messages.REQUIREDCLASSEXCEPTION);
            }
        }

        List<Bag> bags = new List<Bag>();
        public List<Bag> Bags
        {
            get { return bags; }
        }

        

        public BaseItem GetItemFromBagById(int itemId, int bagId)
        {
            BaseItem item = null;

            //var q = BagOne.Items.Where(x => x.ItemID == itemId);
            //if (q.Count() > 0)
            //    item = q.First();

            //var r = BagTwo.Items.Where(x => x.ItemID == itemId);
            //if (r.Count() > 0)
            //    item = r.First();

            return item;
        }



        //public List<Equipment> Equipments { get; set; }
        public Equipment[] visual { get; set; }
        public int CharacterId { get; set; }
        public int AccountId { get; set; }
        public byte Class { get; set; }
        public short Level { get; set; }
        public int MapId { get; set; }
        public int CurrentHp { get; set; }
        public int MaxHp { get; set; }
        public int CurrentMana { get; set; }
        public int MaxMana { get; set; }
        public short Strength { get; set; }
        public short Stamina { get; set; }
        public short Dexterity { get; set; }
        public short Energy { get; set; }
        public int Fame { get; set; }
        public int Money { get; set; }
        public short StatPoint { get; set; }
        public short TrainingPoint { get; set; }
        public short FiveElementPoint { get; set; }
        public int RepulationPoint { get; set; }
        public long CurrentExp { get; set; }
        public int CurrentFEExp { get; set; }
        public byte Rebirth { get; set; }

        public short Critical { get; set; }
        public byte MovingSpeed { get; set; }
        public int TotalDamage { get; set; }
        public int TotalDefence { get; set; }
        public int TotalAttackRating { get; set; }

        public short StatDamage { get { return (short)(Strength * strStatModifier); } }
        public short StatDefence { get { return (short)(Stamina * staminaStatModifier); } }
        public short StatAttackRating { get { return (short)(Dexterity * dexterityStatModifier); } }
        public int StatHp { get { return Energy * energyStatModifier; } }
        public int LevelHp { get { return Level * lifeLevelModifier; } }
        public short LevelMana { get { return (short)(Level * manaLevelModifier); } }

        public List<Equipment> GetAllEquips()
        {
            List<Equipment> Equipments = new List<Equipment>();
            Equipments.AddRange(new Equipment[] { this.Necklace, this.Ring, this.Head, this.Body, this.Hand, this.Cape, this.Feet, this.Charm, this.Mirror });

            var equips = Equipments.Where(x => x != null);

            return equips.ToList();
        }

        public bool FindFreeSlotInBags(BaseItem item, BagSlot bagSlot)
        {
            bool found = false;

            for (int i = 0; i < Bags.Count; i++)
            {
                if (Bags[i].FindFreeSlot(item, bagSlot))
                {
                    found = true;
                    bagSlot.Bag = (byte)(i + 1);
                    break;
                }
            }

            return found;
        }

        public List<Equipment> GetVisuals()
        {
            bool[] equip = new bool[9];
            List<Equipment> equips = new List<Equipment>();
            List<Equipment> Equipments = GetAllEquips();

            for (int i = 0; i < 9; i++)
            {
                if (!equip[i])
                {
                    Equipment temp = new Equipment();
                    temp.Slot = (byte)i;
                    temp.VisualID = 0;
                    temp.Plus = 0;
                    temp.Slvl = 0;
                    temp.RequiredClass = 0;
                    equips.Add(temp);
                }
            }

            foreach (Equipment i in Equipments)
            {
                if (i != null)
                {
                    equips.RemoveAt(i.Slot);
                    equips.Insert(i.Slot, i);
                    equip[i.Slot] = true;
                }
            }

            return equips;
        }

        public virtual Head Head
        {
            get
            {
                return head;
            }
            set
            {
                if (value == null || this.Level >= value.RequiredLevel && this.Dexterity >= value.RequiredDexterity)
                    head = value;
                else
                    throw new RequiredClassException(Messages.REQUIREDCLASSEXCEPTION);
            }
        }
        public virtual Body Body
        {
            get
            {
                return body;
            }
            set
            {
                if (value == null || this.Level >= value.RequiredLevel && this.Stamina >= value.RequiredStamina)
                {
                    if (value != null)
                    {
                        value.Bag = 0;
                        value.Slot = (byte)Slot.Armor;
                    }
                    body = value;
                }
                else
                    throw new RequiredClassException(Messages.REQUIREDCLASSEXCEPTION);
            }
        }
        public virtual Cape Cape
        {
            get { return cape; }
            set
            {
                if (value == null || this.Level >= value.RequiredLevel && this.Class == value.RequiredClass)
                {
                    if (value != null)
                    {
                        value.Bag = 0;
                        value.Slot = (byte)Slot.Cape;
                    }
                    cape = value;
                }
                else
                    throw new RequiredClassException(Messages.REQUIREDCLASSEXCEPTION);
            }
        }
        public virtual Hand Hand
        {
            get { return hand; }
            set
            {
                if (value == null || this.Level >= value.RequiredLevel && this.Strength >= value.RequiredStrength)
                {
                    if (value != null)
                    {
                        value.Bag = 0;
                        value.Slot = (byte)Slot.Weapon;
                    }
                    hand = value;
                }
                else
                    throw new RequiredClassException(Messages.REQUIREDCLASSEXCEPTION);
            }
        }
        public virtual Feet Feet
        {
            get { return feet; }
            set
            {
                if (value == null || this.Level >= value.RequiredLevel && this.Energy >= value.RequiredEnergy)
                {
                    if (value != null)
                    {
                        value.Bag = 0;
                        value.Slot = (byte)Slot.Shoes;
                    }
                    feet = value;
                }
                else
                    throw new RequiredClassException(Messages.REQUIREDCLASSEXCEPTION);
            }
        }

        public Equipment Charm
        {
            get;
            set;
        }
        public Equipment Mirror
        {
            get;
            set;
        }

        public void CalculateTotalStats()
        {
            this.MovingSpeed = 0;
            this.Critical = 0;
            this.MaxHp = energyStatModifier;
            this.MaxMana = 0;
            this.TotalDamage = 0;
            this.TotalDefence = 0;
            this.TotalAttackRating = 0;

            foreach (Equipment e in this.GetAllEquips())
            {
                if (e.bType == (byte)bType.Shoes)
                {
                    this.MovingSpeed = (byte)e.AttackSpeed;
                    this.Critical += e.Critical;
                }
                else
                {
                    this.Critical += e.Critical;
                    this.MaxHp += e.IncMaxLife;
                    this.MaxMana += e.IncMaxMana;
                    this.TotalDamage += e.Damage;
                    this.TotalDefence += e.Defence;
                    this.TotalAttackRating += e.AttackRating;
                }
            }

            //LATER ADD SOFT SKILL DATA AND HARD SKILL OFC TY PLOX

            this.TotalDamage += this.StatDamage;
            this.TotalDefence += this.StatDefence;
            this.TotalAttackRating += this.StatAttackRating;
            if (this.Level > 1)
            {
                this.MaxHp += this.LevelHp + this.StatHp;
                this.MaxMana += this.LevelMana;
            }
        }
    }

    public class Swordman : Character
    {
        public Swordman()
        {
            strStatModifier = 1;
            staminaStatModifier = 2;
            dexterityStatModifier = 1;
            energyStatModifier = 8;
            lifeLevelModifier = 8;
            manaLevelModifier = 4;
        }



        public override Head Head
        {
            get
            {
                return base.Head;
            }

            set
            {
                if (value is Hood || value == null)
                    base.Head = value;
                else
                    throw new RequiredClassException(Messages.REQUIREDCLASSEXCEPTION);
            }

        }

        public override Body Body
        {
            get
            {
                return base.Body;
            }
            set
            {
                if (value is Clothes || value == null)
                    base.Body = value;
                else
                    throw new RequiredClassException(Messages.REQUIREDCLASSEXCEPTION);
            }
        }

        public override Cape Cape
        {
            get
            {
                return base.Cape;
            }

            set
            {
                base.Cape = value;
            }
        }

        public override Feet Feet
        {
            get
            {
                return base.Feet;
            }

            set
            {
                if (value == null || value is SmBoots)
                    base.Feet = value;
                else
                    throw new RequiredClassException(Messages.REQUIREDCLASSEXCEPTION);
            }
        }

        public override Hand Hand
        {
            get
            {
                return base.Hand;
            }

            set
            {
                if (value == null || value is Sword || value is Blade && this.Energy >= value.RequiredEnergy)
                    base.Hand = value;
                else
                    throw new RequiredClassException(Messages.REQUIREDCLASSEXCEPTION);
            }
        }
    }

    public class Mage : Character
    {
        public Mage()
        {
            strStatModifier = 1;
            staminaStatModifier = 2;
            dexterityStatModifier = 2;
            energyStatModifier = 4;
            lifeLevelModifier = 4;
            manaLevelModifier = 5;
        }

        public override Head Head
        {
            get
            {
                return base.Head;
            }

            set
            {
                if (value == null || value is Tiara)
                    base.Head = value;
                else
                    throw new RequiredClassException(Messages.REQUIREDCLASSEXCEPTION);
            }

        }

        public override Body Body
        {
            get
            {
                return base.Body;
            }
            set
            {
                if (value == null || value is Dress)
                    base.Body = value;
                else
                    throw new RequiredClassException(Messages.REQUIREDCLASSEXCEPTION);
            }
        }

        public override Cape Cape
        {
            get
            {
                return base.Cape;
            }

            set
            {
                base.Cape = value;
            }
        }

        public override Feet Feet
        {
            get
            {
                return base.Feet;
            }

            set
            {
                if (value == null || value is MageBoots)
                    base.Feet = value;
                else
                    throw new RequiredClassException(Messages.REQUIREDCLASSEXCEPTION);
            }
        }

        public override Hand Hand
        {
            get
            {
                return base.Hand;
            }

            set
            {
                if (value == null || value is Fan && this.Dexterity >= value.RequiredDexterity || value is Brush && this.Dexterity >= value.RequiredDexterity)
                    base.Hand = value;
                else
                    throw new RequiredClassException(Messages.REQUIREDCLASSEXCEPTION);
            }
        }
    }

    public class Warrior : Character
    {
        public Warrior()
        {
            strStatModifier = 1;
            staminaStatModifier = 3;
            dexterityStatModifier = 1;
            energyStatModifier = 6;
            lifeLevelModifier = 6;
            manaLevelModifier = 4;
        }

        public override Head Head
        {
            get
            {
                return base.Head;
            }

            set
            {
                if (value == null || value is Helmet)
                    base.Head = value;
                else
                    throw new RequiredClassException(Messages.REQUIREDCLASSEXCEPTION);
            }

        }

        public override Body Body
        {
            get
            {
                return base.Body;
            }
            set
            {
                if (value == null || value is Armor)
                    base.Body = value;
                else
                    throw new RequiredClassException(Messages.REQUIREDCLASSEXCEPTION);
            }
        }

        public override Cape Cape
        {
            get
            {
                return base.Cape;
            }

            set
            {
                base.Cape = value;
            }
        }

        public override Feet Feet
        {
            get
            {
                return base.Feet;
            }

            set
            {
                if (value == null || value is WarriorShoes)
                    base.Feet = value;
                else
                    throw new RequiredClassException(Messages.REQUIREDCLASSEXCEPTION);
            }
        }

        public override Hand Hand
        {
            get
            {
                return base.Hand;
            }

            set
            {
                if (value == null || value is Claw && this.Stamina >= value.RequiredStamina || value is Axe && this.Stamina >= value.RequiredStamina)
                    base.Hand = value;
                else
                    throw new RequiredClassException(Messages.REQUIREDCLASSEXCEPTION);
            }
        }
    }

    public class GhostFighter : Character
    {
        public GhostFighter()
        {
            strStatModifier = 2;
            staminaStatModifier = 1;
            dexterityStatModifier = 1;
            energyStatModifier = 5;
            lifeLevelModifier = 6;
            manaLevelModifier = 4;
        }

        public override Head Head
        {
            get
            {
                return base.Head;
            }

            set
            {
                if (value == null || value is Hat)
                    base.Head = value;
                else
                    throw new RequiredClassException(Messages.REQUIREDCLASSEXCEPTION);
            }

        }

        public override Body Body
        {
            get
            {
                return base.Body;
            }
            set
            {
                if (value == null || value is LeatherClothes)
                    base.Body = value;
                else
                    throw new RequiredClassException(Messages.REQUIREDCLASSEXCEPTION);
            }
        }

        public override Cape Cape
        {
            get
            {
                return base.Cape;
            }

            set
            {
                base.Cape = value;
            }
        }

        public override Feet Feet
        {
            get
            {
                return base.Feet;
            }

            set
            {
                if (value == null || value is GhostFighterShoes)
                    base.Feet = value;
                else
                    throw new RequiredClassException(Messages.REQUIREDCLASSEXCEPTION);
            }
        }

        public override Hand Hand
        {
            get
            {
                return base.Hand;
            }

            set
            {
                if (value == null || value is Talon || value is Tonfa)
                    base.Hand = value;
                else
                    throw new RequiredClassException(Messages.REQUIREDCLASSEXCEPTION);
            }
        }
    }

    public class Npc : BaseEntity
    {
        List<Bag> bags = new List<Bag>();
        public List<Bag> Bags
        {
            get { return bags; }
        }
        public int MapID { get; set; }
        public int NpcID { get; set; }
        public byte NpcType { get; set; }
        public short Direction { get; set; }
    }

    public class ChannelRequest
    {
        public int WorldID { get; set; }
    }

    public class AddStatInfo
    {
        public int Stat { get; set; }
        public int Amount { get; set; }
    }

    public class Map
    {
        public List<Portal> Portals { get; set; }
        public List<Npc> Npcs { get; set; }

        public int MapID { get; set; }
        public string Name { get; set; }
        public int MultiplyValue { get; set; }
        public int SpawnX { get; set; }
        public int SpawnY { get; set; }
    }

    public class Portal
    {
        public int MapID { get; set; }
        public int ToMapID { get; set; }
        public short Width { get; set; }
        public short Height { get; set; }
        public short ToX { get; set; }
        public short ToY { get; set; }
        public short FromX { get; set; }
        public short FromY { get; set; }
    }

    public class BagSlot
    {
        public byte Bag { get; set; }
        public byte Slot { get; set; }
    }

    [Serializable]
    public class BaseItem : BaseEntity
    {
        public int ItemID { get; set; }
        public int OwnerID { get; set; }
        public short ReferenceID { get; set; }
        public short VisualID { get; set; }
        public byte Bag { get; set; }
        public byte Slot { get; set; }
        public byte bType { get; set; }
        public byte bKind { get; set; }
        public byte RequiredClass { get; set; }
        public short Amount { get; set; }
        public byte SizeX { get; set; }
        public byte SizeY { get; set; }
        public int Price { get; set; }
    }

    public class MapItem
    {
        public int MapID { get; set; }
        public short MapX { get; set; }
        public short MapY { get; set; }
        public byte MapZ { get; set; }
        public int MapItemID { get; set; }
        public byte bType { get; set; }
        public short VisualID { get; set; }
        public short ReferenceID { get; set; }
        public int ItemID { get; set; }
        public short Amount { get; set; }

    }

    [Serializable]
    public class BeadItem : BaseItem
    {
        public int ToMapID { get; set; }
    }

    [Serializable]
    public class Bead : BeadItem
    {
    }

    #region BookItem
    [Serializable]
    public class BookItem : BaseItem
    {
        public short RequiredLevel { get; set; }
        public int SkillID { get; set; }
        public byte SkillLevel { get; set; }
        public int SkillData { get; set; }
    }

    [Serializable]
    public class SoftBook : BookItem
    {
    }

    [Serializable]
    public class HardBook : BookItem
    {
    }
    #endregion

    #region PotionItem
    [Serializable]
    public class PotionItem : BaseItem
    {
        public short HealHp { get; set; }
        public short HealMana { get; set; }
    }

    [Serializable]
    public class Potion : PotionItem
    {
    }

    [Serializable]
    public class Elixir : PotionItem
    {
    }
    #endregion

    #region ImbueItem
    [Serializable]
    public class ImbueItem : BaseItem
    {
        public short ImbueChance { get; set; }
        public short IncreaseValue { get; set; }

    }

    [Serializable]
    public class Black : ImbueItem
    {
    }

    [Serializable]
    public class White : ImbueItem
    {
    }

    [Serializable]
    public class Red : ImbueItem
    {
    }

    [Serializable]
    public class Dragon : ImbueItem
    {
    }
    #endregion

    #region Equipment
    [Serializable]
    public class Equipment : BaseItem
    {
        public short RequiredLevel { get; set; }
        public short RequiredDexterity { get; set; }
        public short RequiredStrength { get; set; }
        public short RequiredStamina { get; set; }
        public short RequiredEnergy { get; set; }
        public byte MaxImbueTries { get; set; }
        public int Durability { get; set; }
        public int MaxDurability { get; set; }
        public short Damage { get; set; }
        public short Defence { get; set; }
        public short AttackRating { get; set; }
        public short AttackSpeed { get; set; }
        public short AttackRange { get; set; }
        public short IncMaxLife { get; set; }
        public short IncMaxMana { get; set; }
        public short IncLifeRegen { get; set; }
        public short IncManaRegen { get; set; }
        public short Critical { get; set; }
        public byte Plus { get; set; }
        public byte Slvl { get; set; }
        public byte ImbueTries { get; set; }
        public short DragonSuccessImbueTries { get; set; }
        public byte DiscountRepairFee { get; set; }
        public short TotalDragonImbueTries { get; set; }
        public short DragonDamage { get; set; }
        public short DragonDefence { get; set; }
        public short DragonAttackRating { get; set; }
        public short DragonLife { get; set; }
        public byte MappedData { get; set; }
        public byte ForceSlot { get; set; }
        public byte RebirthHole { get; set; }
        public byte RebirthHoleItem { get; set; }
        public short RebirthHoleStat { get; set; }
    }

    [Serializable]
    public class Hand : Equipment
    {
    }

    [Serializable]
    public class Head : Equipment
    {
    }

    [Serializable]
    public class Body : Equipment
    {
    }

    [Serializable]
    public class Feet : Equipment
    {
    }

    [Serializable]
    public class Cape : Equipment
    {
        public short MaxPolishImbueTries { get; set; }
        public byte PolishImbueTries { get; set; }
        public short VigiStat1 { get; set; }
        public short VigiStatAdd1 { get; set; }
        public short VigiStat2 { get; set; }
        public short VigiStatAdd2 { get; set; }
        public short VigiStat3 { get; set; }
        public short VigiStatAdd3 { get; set; }
        public short VigiStat4 { get; set; }
        public short VigiStatAdd4 { get; set; }
    }

    [Serializable]
    public class Sword : Hand
    {
    }

    [Serializable]
    public class Blade : Hand
    {
    }

    [Serializable]
    public class Fan : Hand
    {
    }

    [Serializable]
    public class Brush : Hand
    {
    }

    public class Claw : Hand
    {
    }

    [Serializable]
    public class Axe : Hand
    {
    }

    [Serializable]
    public class Talon : Hand
    {
    }

    [Serializable]
    public class Tonfa : Hand
    {
    }

    [Serializable]
    public class Hammer : Hand
    {
    }

    [Serializable]
    public class Clothes : Body
    {
    }

    [Serializable]
    public class Dress : Body
    {
    }

    [Serializable]
    public class Armor : Body
    {
    }

    [Serializable]
    public class LeatherClothes : Body
    {
    }

    [Serializable]
    public class Hood : Head
    {
    }

    [Serializable]
    public class Tiara : Head
    {
    }

    [Serializable]
    public class Helmet : Head
    {
    }

    [Serializable]
    public class Hat : Head
    {
    }

    [Serializable]
    public class SmBoots : Feet
    {
    }

    [Serializable]
    public class MageBoots : Feet
    {
    }

    [Serializable]
    public class WarriorShoes : Feet
    {
    }

    [Serializable]
    public class GhostFighterShoes : Feet
    {
    }

    [Serializable]
    public class Ring : Equipment
    {
    }

    [Serializable]
    public class Necklace : Equipment
    {
    }
    #endregion


}
