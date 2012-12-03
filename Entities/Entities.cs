using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Collections;
using System.Timers;
using System.Xml.Serialization;

namespace Entities
{
    public class MonsterMoveInfo
    {
        public Position CurrentPosition { get; set; }
        public Position Destination { get; set; }
        public short Rotation { get; set; }
        public MonsterMoveStatus Status { get; set; }
    }

    public class MonsterAttackInfo
    {
        public BaseEntity Target { get; set; }
        public bool UseSkill { get; set; }
    }

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

    public class PetRequestInfo
    {
        public int CharacterID { get; set; }
    }

    public class OnSeeEntityInfo
    {
        public int TargetID { get; set; }
        public bool Moving { get; set; }
        public bool Attacking { get; set; }
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

    public class AttackInfo
    {
        public int PacketID { get; set; }
        public byte AttackerType { get; set; }
        public int AttackerID { get; set; }
        public short TargetX { get; set; }
        public short TargetY { get; set; }
        public byte TargetZ { get; set; }
        public byte TargetType { get; set; }
        public int TargetID { get; set; }
        public int Damage { get; set; }
        public AttackType Type { get; set; }
        public int Experience { get; set; }
        public bool Critical { get; set; }
        public bool Dead { get; set; }

        public bool PetDamaged { get; set; }
        public bool PetDied { get; set; }

        public bool DoRefDamage { get; set; }
    }

    public class SelectSkillInfo
    {
        public byte Row { get; set; }
        public int SkillID { get; set; }
        public byte Slot { get; set; }
    }

    public class CastSkillInfo
    {
        public int PacketID { get; set; }
        public int SkillID { get; set; }
        public byte CasterType { get; set; }
        public int CasterID { get; set; }
        public short CasterX { get; set; }
        public short CasterY { get; set; }
        public short CasterZ { get; set; }
        public byte TargetType { get; set; }
        public int TargetID { get; set; }
        public short TargetX { get; set; }
        public short TargetY { get; set; }
        public byte TargetZ { get; set; }
        public AttackType Type { get; set; }
        public int Experience { get; set; }
        public int Damage { get; set; }
        public bool Dead { get; set; }
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
        public byte Slot { get; set; }
        public byte Bag { get; set; }
    }

    public class SellItemInfo
    {
        public int NpcID { get; set; }
        public int ItemID { get; set; }
        public byte Bag { get; set; }
        public byte Slot { get; set; }
    }

    public class UseItemInfo
    {
        public byte Bag { get; set; }
        public byte Slot { get; set; }
        public int ItemID { get; set; }
    }

    public class UpdateQuickSlotInfo
    {
        public short ValueID { get; set; }
        public byte Slot { get; set; }
        public byte Page { get; set; }
    }

    public class OpenWarehouseInfo
    {
        public int NpcID { get; set; }
    }

    public class AddItemToWarehouseInfo
    {
        public int CharacterID { get; set; }
        public int ItemID { get; set; }
        public byte FromBag { get; set; }
        public byte FromSlot { get; set; }
        public byte ToBag { get; set; }
        public byte ToSlot { get; set; }
    }

    public class MoveWarehouseItemInfo
    {
        public byte FromSlot { get; set; }
        public int ItemID { get; set; }
        public byte ToSlot { get; set; }
        public int ItemUnderID { get; set; }
    }

    public class MoveWarehouseItemToBagInfo
    {
        public int CharacterID { get; set; }
        public int ItemID { get; set; }
        public byte FromSlot { get; set; }
        public byte ToBag { get; set; }
        public byte ToSlot { get; set; }
    }

    public class AddItemToShopInfo
    {
        public byte FromBag { get; set; }
        public byte FromSlot { get; set; }
        public int ItemID { get; set; }
        public byte Slot { get; set; }
        public int Price { get; set; }
    }

    public class MoveShopItemInfo
    {
        public byte FromSlot { get; set; }
        public int ItemID { get; set; }
        public byte ToSlot { get; set; }
        public int ItemUnderID { get; set; }
    }

    public class ChangeShopInfo
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class MoveShopItemToBagInfo
    {
        public byte FromSlot { get; set; }
        public int ItemID { get; set; }
        public byte ToBag { get; set; }
        public byte ToSlot { get; set; }
    }

    public class ShopStartSellingInfo
    {
        public bool Selling { get; set; }
    }

    public class ShopTakeMoneyInfo
    {
        public int Amount { get; set; }
    }

    public class OpenOtherPlayerShopInfo
    {
        public int CharacterID { get; set; }
    }

    public class BuyOtherPlayerShopItem
    {
        public int CharacterID { get; set; }
        public byte FromSlot { get; set; }
        public int ItemID { get; set; }
        public byte ToBag { get; set; }
        public byte ToSlot { get; set; }
        public int Price { get; set; }
    }

    public class TradePlayerInfo
    {
        public TradePlayerError Error { get; set; }
        public int CharacterID { get; set; }
        public int CharacterID2 { get; set; }
    }

    public class TradePlayerCommandsInfo
    {
        public TradePlayerCommands Error { get; set; }
        public int CharacterID { get; set; }
    }

    public class TradeAddMoneyInfo
    {
        public int PlayerID { get; set; }
        public int Amount { get; set; }
    }

    public class TradeAddItemInfo
    {
        public byte FromBag { get; set; }
        public byte FromSlot { get; set; }
        public byte ToBag { get; set; }
        public byte ToSlot { get; set; }
        public int ItemID { get; set; }
        public short Amount { get; set; }
        public int PlayerID { get; set; }
    }

    public class FriendAddInfo
    {
        public FriendAddTypes Type { get; set; }
        public FriendAddAnswers Answer { get; set; }
        public int AskerID { get; set; }
        public int TargetID { get; set; }
    }

    public class UseTeleporterInfo
    {
        public int NpcID { get; set; }
        public int ToMap { get; set; }
    }

    public class CombineItemsInfo
    {
        public int ItemID { get; set; }
        public byte Bag { get; set; }
        public byte Slot { get; set; }
    }

    public class LearnSkillInfo
    {
        public int SkillID { get; set; }
    }

    public class MonsterSpawnRequestInfo
    {
        public int MonsterId { get; set; }
        public int MapId { get; set; }
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

        public bool HasRoom(BaseItem item)
        {
            IEnumerable<BaseItem> q = null;

            q = items.Where(x => x.Slot == item.Slot && x != item ||
                                   (x.Slot == item.Slot + 1 && x != item) ||                //  _ _ _ _ _ 
                                   (x.Slot == item.Slot + 5 && x != item && x.SizeX > 1) || // | | | | | |
                                   (x.Slot == item.Slot + 6 && x != item) ||                // | |o|x|o| | // x being item
                                   (x.Slot == item.Slot + 7 && x != item) ||                // | |o|o|o| |
                                   (x.Slot == item.Slot - 1 && x != item && x.SizeX > 1));// | | | | | |
            if (q.Count() > 0)
                return false;

            return true;
        }

        public bool CheckTradeSlot(BaseItem item, byte itemSlot)
        {
            if (IsSlotValid(item, itemSlot))
            {
                IEnumerable<BaseItem> q = null;

                if (item.SizeX > 1)
                {
                    q = items.Where(x => x.TradeSlot == itemSlot && x != item ||
                                           (x.TradeSlot == itemSlot + 1 && x != item) ||                //  _ _ _ _ _ 
                                           (x.TradeSlot == itemSlot + 5 && x != item && x.SizeX > 1) || // | |o|o|o| |
                                           (x.TradeSlot == itemSlot + 6 && x != item) ||                // | |o|x|o| | // x being item
                                           (x.TradeSlot == itemSlot + 7 && x != item) ||                // | |o|o|o| |
                                           (x.TradeSlot == itemSlot - 1 && x != item && x.SizeX > 1) ||  // | | | | | |
                                           (x.TradeSlot == itemSlot - 7 && x != item && x.SizeX > 1) ||
                                           (x.TradeSlot == itemSlot - 6 && x != item && x.SizeX > 1) ||
                                           (x.TradeSlot == itemSlot - 5 && x != item && x.SizeX > 1));
                    if (q.Count() > 0)
                        return true;
                }
                else
                {
                    q = items.Where(x => x.TradeSlot == itemSlot && x != item ||
                                        (x.TradeSlot == itemSlot - 1 && x.SizeX > 1 && x != item) ||
                                        (x.TradeSlot == itemSlot - 6 && x.SizeX > 1 && x != item) ||
                                        (x.TradeSlot == itemSlot - 7 && x.SizeX > 1 && x != item));
                    if (q.Count() > 0)
                        return true;
                }

                return false;
            }

            return true;
        }

        public bool CheckSlot(BaseItem item, byte itemSlot)
        {
            IEnumerable<BaseItem> q = null;

            if (item.SizeX > 1)
            {
                q = items.Where(x => x.Slot == itemSlot && x != item ||
                                       (x.Slot == itemSlot + 1 && x != item) ||                //  _ _ _ _ _ 
                                       (x.Slot == itemSlot + 5 && x != item && x.SizeX > 1) || // | |o|o|o| |
                                       (x.Slot == itemSlot + 6 && x != item) ||                // | |o|x|o| | // x being item
                                       (x.Slot == itemSlot + 7 && x != item) ||                // | |o|o|o| |
                                       (x.Slot == itemSlot - 1 && x != item && x.SizeX > 1) ||  // | | | | | |
                                       (x.Slot == itemSlot - 7 && x != item && x.SizeX > 1) ||
                                       (x.Slot == itemSlot - 6 && x != item && x.SizeX > 1) ||
                                       (x.Slot == itemSlot - 5 && x != item && x.SizeX > 1));
                if (q.Count() > 0)
                    return true;
            }
            else
            {
                q = items.Where(x => x.Slot == itemSlot && x != item ||
                                    (x.Slot == itemSlot - 1 && x.SizeX > 1 && x != item) ||
                                    (x.Slot == itemSlot - 6 && x.SizeX > 1 && x != item) ||
                                    (x.Slot == itemSlot - 7 && x.SizeX > 1 && x != item));
                if (q.Count() > 0)
                    return true;
            }


            return false;
        }

        private bool HasNoRoom(BaseItem item, byte itemSlot, out List<BaseItem> conflictingItems)
        {
            bool full = false;

            conflictingItems = null;
            IEnumerable<BaseItem> q = null;
            if (item.SizeX > 1)
            {
                q = items.Where(x => x.Slot == itemSlot ||
                                    (x.Slot == itemSlot + 1) ||
                                    (x.Slot == itemSlot + 6) ||
                                    (x.Slot == itemSlot + 7) ||
                                    (x.Slot == itemSlot - 5 && x != item && x.SizeX > 1));
                if (q.Count() > 0)
                    full = true;
            }
            else
            {
                q = items.Where(x => x.Slot == itemSlot && x != item ||
                                (x.Slot == itemSlot - 1 && x.SizeX > 1 && x != item) ||
                                (x.Slot == itemSlot - 6 && x.SizeX > 1 && x != item) ||
                                (x.Slot == itemSlot - 7 && x.SizeX > 1 && x != item));

                //q = items.Where(x => x.Slot == itemSlot ||
                //                    (x.Slot == itemSlot - 1 && x.SizeX > 1) ||
                //                    (x.Slot == itemSlot - 6 && x.SizeX > 1) ||
                //                    (x.Slot == itemSlot - 7 && x.SizeX > 1));



                if (q.Count() > 0)
                    full = true;
            }

            conflictingItems = q.ToList();

            return full;
        }


        public bool IsSlotValid(BaseItem item, byte itemSlot)
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

        private bool IsOnEdge(BaseItem item, byte itemSlot)
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
                return true;
            }

            return false;
        }

        public bool MoveItem(Character ch, BaseItem item, byte slot, out BaseItem swappedItem)
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
                //if (!this.items.Any(x => x.ItemID == item.ItemID))
                //    this.AddItem(item);
            }
            else //later add swapping
            {
                BaseItem conflict = null;

                if (conflictingItems.Count == 1)
                {
                    conflict = conflictingItems.First();

                    if (conflict == item)
                        moved = true;

                    if (conflict.Slot == slot)
                    {
                        if (conflict.SizeX == item.SizeX)
                        {
                            moved = true;
                            swappedItem = conflict;
                        }
                        else
                        {
                            moved = SwapDifferentSize(ch, item, conflict, slot);
                            swappedItem = conflict;
                        }
                    }
                    else
                    {
                        if (item.SizeX > 1)
                        {
                            if (slot + 1 != conflict.Slot && slot + 6 != conflict.Slot && slot + 7 != conflict.Slot && slot - 5 != conflict.Slot) // so it wont let you overlay stuff from left to right 
                                moved = true;
                        }
                        else
                        {
                            if (slot - 1 != conflict.Slot && slot - 6 != conflict.Slot && slot - 7 != conflict.Slot) // so it wont let you overlay stuff from left to right 
                                moved = true;

                            //    moved = false;
                            //no room plx
                        }
                    }
                }
                else
                {
                    var q = conflictingItems.Where(x => x.Slot == item.Slot ||
                                                                  x.Slot + 1 == item.Slot ||
                                                                  x.Slot + 6 == item.Slot ||
                                                                  x.Slot + 7 == item.Slot);

                    moved = false;

                    //if(conflict.SizeX == 1 && conflict.SizeX >1)
                    //{
                    //}
                    //else
                    //{
                    //    //no room plx
                    //}
                }


            }

            return moved;
        }

        private bool SwapDifferentSize(Character ch, BaseItem one, BaseItem two, int slot)
        {
            bool moved = false;
            int test = Math.Abs(one.Slot - slot);
            if (!IsOnEdge(one, one.Slot)) // so it wont let you stack it with the item
            {
                if (one.Bag == two.Bag)
                {
                    if (test != 1 && test != 6 && test != 7)
                    {
                        if (HasRoom(one))
                        {
                            moved = true;
                        }
                        else
                            moved = false;
                    }
                    else
                        moved = false;
                }
                else
                {
                    if (ch.Bags[one.Bag - 1].HasRoom(one) && ch.Bags[two.Bag - 1].HasRoom(two))
                    {
                        moved = true;
                    }
                    else
                        moved = false;
                }
            }

            return moved;
        }

        public void AddItem(BaseItem item)
        {
            items.Add(item);
        }

        public bool FindFreeTradeSlot(BaseItem item, BagSlot newBagSlot)
        {
            bool found = false;

            newBagSlot.Slot = 255;

            for (byte i = 0; i < slotCount; i++)
            {
                if (!IsSlotValid(item, i))
                {
                    continue;
                }

                if (!CheckTradeSlot(item, i))
                {
                    newBagSlot.Slot = i;
                    found = true;
                    break;
                }

            }

            return found;
        }

        public bool FindFreeSlot(BaseItem item, BagSlot newBagSlot)
        {
            bool found = false;

            newBagSlot.Slot = 255;

            for (byte i = 0; i < slotCount; i++)
            {
                if (!IsSlotValid(item, i))
                {
                    continue;
                }

                if (!CheckSlot(item, i))
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

    public class Warehouse : BaseItemContainer
    {
        public Warehouse(List<BaseItem> items)
            : base(64, items)
        {
        }
    }

    public class TradeWindow : BaseItemContainer
    {
        public bool Active { get; set; }
        public bool Accepted { get; set; }
        public int Money { get; set; }

        public TradeWindow(List<BaseItem> items)
            : base(24, items)
        {
        }
    }

    public class Shop : BaseItemContainer
    {
        public bool Active { get; set; }
        public string ShopName { get; set; }
        public string ShopDesc { get; set; }
        public int ShopID { get; set; }
        public int OwnerID { get; set; }
        public int TotalMoney { get; set; }
        public Shop(string Name, string Desc, int ID, int Money, int OwnerId, List<BaseItem> items)
            : base(36, items)
        {
            this.ShopName = Name;
            this.ShopDesc = Desc;
            this.ShopID = ID;
            this.TotalMoney = Money;
            this.OwnerID = OwnerId;
        }
    }

    public class BaseSkill
    {
        public short SkillID { get; set; }
        public byte SkillLevel { get; set; }
        public byte RequiredLevel { get; set; }
        public short RequiredStrength { get; set; }
        public short RequiredStamina { get; set; }
        public short RequiredDexterity { get; set; }
        public short RequiredEnergy { get; set; }
        public short ManaCost { get; set; }
        public short PreDelay { get; set; }
        public short PostDelay { get; set; }
        public short RequiredTraining { get; set; }
        public byte ReadOnlyBook { get; set; }
        public short IncDamage { get; set; }
        public short IncDamagePerc { get; set; }
        public short IncDefense { get; set; }
        public short IncDefensePerc { get; set; }
        public short IncAttackRating { get; set; }
        public short IncAttackRatingPerc { get; set; }
        public short IncHpMax { get; set; }
        public short IncHpCur { get; set; }
        public short IncHpCurPerc { get; set; }
        public short RecoverHp { get; set; }
        public short RecoverHpPerc { get; set; }
        public short IncManaMax { get; set; }
        public short IncManaCur { get; set; }
        public short IncManaCurPerc { get; set; }
        public short RecoverMana { get; set; }
        public short RecoverManaPerc { get; set; }
        public short IncCritical { get; set; }
        public short IncCriticalPerc { get; set; }
        public short IncPKPerc { get; set; }
        public int KeepUpTime { get; set; }
        public short Distance { get; set; }
        public short SuccessChance { get; set; }
        public int nEtc1 { get; set; }
        public int nEtc2 { get; set; }
        public int nEtc3 { get; set; }
    }

    public class HardSkill : BaseSkill
    {
    }

    public class SoftSkill : BaseSkill
    {
    }

    public class ActiveSkill
    {
        public ActiveSkill(DateTime casted, BaseSkill skill, BaseEntity[] targets, Character caster)
        {
            this.CastTime = casted;
            this.Skill = skill;
            this.Caster = caster;
            this.Targets = targets;
        }
        public DateTime CastTime { get; set; }
        public BaseSkill Skill { get; set; }
        public Character Caster { get; set; }
        public BaseEntity[] Targets { get; set; }

        public virtual void DoEffect()
        {
        }
    }

    public class PetSkill
    {
        public PetSkill(DateTime casted, BaseSkill skill, Pet target, Character caster)
        {
            this.CastTime = casted;
            this.Skill = skill;
            this.Caster = caster;
            this.Target = target;
        }

        public DateTime CastTime { get; set; }
        public BaseSkill Skill { get; set; }
        public Character Caster { get; set; }
        public Pet Target { get; set; }

        public virtual void DoEffect()
        {
        }
    }

    public class StrengthenMonster : PetSkill
    {
        public StrengthenMonster(DateTime casted, BaseSkill skill, Pet target, Character caster)
            : base(casted, skill, target, caster)
        {
        }

        public EventHandler StrMonsterCasted;
        public EventHandler StrMonsterEnded;

        public override void DoEffect()
        {
            float damageInc = Skill.IncDamagePerc - 100;
            damageInc = damageInc / 100;
            float atkratingInc = Skill.IncAttackRatingPerc - 100;
            atkratingInc = atkratingInc / 100;
            float defenseInc = Skill.IncDefensePerc - 100;
            defenseInc = defenseInc / 100;
            float lifeInc = Skill.IncHpCurPerc - 100;
            lifeInc = lifeInc / 100;

            Target.DamageInc = damageInc;
            Target.DefenseInc = defenseInc;
            Target.AttackRatingInc = atkratingInc;
            Target.LifeInc = lifeInc;

            Target.IsUsingBuff = true;

            if (StrMonsterCasted != null)
                StrMonsterCasted(this, new EventArgs());

            SkillTimer = new Timer(Skill.KeepUpTime * 1000);
            SkillTimer.Enabled = true;
            SkillTimer.AutoReset = false;
            SkillTimer.Elapsed += new ElapsedEventHandler(SkillTimer_Elapsed);
            SkillTimer.Start();
        }

        void SkillTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            SkillTimer.Stop();

            Target.DamageInc = 0;
            Target.DefenseInc = 0;
            Target.AttackRatingInc = 0;
            Target.LifeInc = 0;
            Target.IsUsingBuff = false;

            if (StrMonsterEnded != null)
                StrMonsterEnded(this, new EventArgs());
        }

        Timer SkillTimer;
    }

    public class EmpathySkill : PetSkill
    {
        public EmpathySkill(DateTime casted, BaseSkill skill, Pet target, Character caster)
            : base(casted, skill, target, caster)
        {
        }

        public EventHandler EmpathyEnded;
        public EventHandler EmpathyCasted;

        public override void DoEffect()
        {
            float expInc = Skill.nEtc1 - 100;
            expInc = expInc / 100;

            Target.BonusExperience = expInc;
            Target.IsUsingEmpathy = true;

            if (EmpathyCasted != null)
                EmpathyCasted(this, new EventArgs());

            SkillTimer = new Timer(Skill.KeepUpTime * 1000);
            SkillTimer.Enabled = true;
            SkillTimer.AutoReset = false;
            SkillTimer.Elapsed += new ElapsedEventHandler(SkillTimer_Elapsed);
            SkillTimer.Start();
        }

        void SkillTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            SkillTimer.Stop();

            Target.BonusExperience = 0;
            Target.IsUsingEmpathy = false;

            if (EmpathyEnded != null)
                EmpathyEnded(this, new EventArgs());
        }

        Timer SkillTimer;
    }

    public class PUTM : PetSkill
    {
        public PUTM(DateTime casted, BaseSkill skill, Pet target, Character caster)
            : base(casted, skill, target, caster)
        {

        }

        public EventHandler PUTMEnded;
        public EventHandler PUTMCasted;

        public override void DoEffect()
        {
            float damageInc = Skill.IncDamagePerc - 100;
            damageInc = damageInc / 100;
            float atkratingInc = Skill.IncAttackRatingPerc - 100;
            atkratingInc = atkratingInc / 100;

            Target.DamageInc = damageInc;
            Target.AttackRatingInc = atkratingInc;

            //Target.BonusDamage += (int)bonusDmg;
            //Target.BonusAttackRating += (int)bonusAr;
            Target.IsUsingBuff = true;

            if (PUTMCasted != null)
                PUTMCasted(this, new EventArgs());

            SkillTimer = new Timer(Skill.KeepUpTime * 1000);
            SkillTimer.Enabled = true;
            SkillTimer.AutoReset = false;
            SkillTimer.Elapsed += new ElapsedEventHandler(SkillTimer_Elapsed);
            SkillTimer.Start();
        }


        void SkillTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            SkillTimer.Stop();
            Target.AttackRatingInc = 0;
            Target.DamageInc = 0;
            Target.IsUsingBuff = false;
            if (PUTMEnded != null)
                PUTMEnded(this, new EventArgs());
        }

        Timer SkillTimer;
    }

    public class HazeSkill : ActiveSkill
    {
        public HazeSkill(DateTime hit, BaseSkill skill, BaseEntity[] targets, Character caster, BaseEntity hitTarget)
            : base(hit, skill, targets, caster)
        {
            this.hitTarget = hitTarget;
        }
        public event EventHandler<HazeDamageCastedEventArgs> HazeDamageCasted;
        public event EventHandler HazeEnded;

        private BaseEntity hitTarget;
        //may be characters as well on pvp
        int passedInterval = 0;

        public BaseEntity HitTarget
        {
            get
            {
                return hitTarget;
            }
        }

        void dmgTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (passedInterval == Skill.KeepUpTime)
            {
                dmgTimer.Stop();
                dmgTimer.AutoReset = false;
                if (HazeEnded != null)
                    HazeEnded(this, new EventArgs());
                return;
            }


            if (HazeDamageCasted != null)
                HazeDamageCasted(this, new HazeDamageCastedEventArgs(Targets, Caster));

            passedInterval += 2;
        }

        public override void DoEffect()
        {
            dmgTimer = new Timer(2000);
            dmgTimer.Enabled = true;
            dmgTimer.AutoReset = true;
            dmgTimer.Elapsed += new ElapsedEventHandler(dmgTimer_Elapsed);
            dmgTimer.Start();
        }

        public void EndHaze()
        {
            dmgTimer.Stop();
            dmgTimer.AutoReset = false;
            if (HazeEnded != null)
                HazeEnded(this, new EventArgs());
            return;
        }

        Timer dmgTimer;
    }

    public class PartyRequestInfo
    {
        public int AskerID { get; set; }
        public int TargetID { get; set; }
        public PartyError Error { get; set; }
        public PartyType Type { get; set; }
    }

    public class AddToPartyRequestInfo
    {
        public int AskerID { get; set; }
        public int TargetID { get; set; }
        public PartyError Error { get; set; }
        public PartyType Type { get; set; }
    }

    public class LeavePartyInfo
    {
        public int PartyID { get; set; }
    }

    public class ChangePartyLeaderInfo
    {
        public int OldLeader { get; set; }
        public int NewLeader { get; set; }
    }

    public class Party
    {
        public int PartyID { get; set; }
        public PartyType Type { get; set; }
        public List<Character> Members { get; set; }
        public Character Leader { get; set; }
        public int MemberCount { get { return Members.Count; } }
    }

    public class Guild
    {
        public int ID { get; set; }
        public int Donations { get; set; }
        public int Fame { get; set; }
        public string Name { get; set; }
        public string Notice { get; set; }

        public List<GuildMember> Members { get; set; }
        public GuildMember Leader { get; set; }
    }

    public class GuildTown : BaseEntity
    {
        public int ID { get; set; }
        public int MapID { get; set; }
        public byte Grade { get; set; }
        public int CurDura { get; set; }
        public int MaxDura { get; set; }
        public int OwnerID { get; set; }
    }

    public class GuildMember
    {
        public string Title { get; set; }
        public Character Character { get; set; }
        public GuildOrder Rank { get; set; }
        public bool Online { get; set; }

        public GuildMember(string title, GuildOrder rank, Character ch)
        {
            Title = title;
            Rank = rank;
            Character = ch;
        }
    }

    public class Friend
    {
        public int FriendID { get; set; }
        public FriendAddTypes RelationType { get; set; }
        public string FriendName { get; set; }
        public bool FriendOnline { get; set; }
    }

    public class QuickSlot
    {
        public QuickSlot(int slot, int value)
        {
            this.Slot = slot;
            this.Value = value;
        }

        public int Slot { get; set; }
        public int Value { get; set; }
    }

    public class KillPet
    {
        public byte Type { get; set; }
        public int PetID { get; set; }
        public byte Action { get; set; }
    }

    public class BaitPetInfo
    {
        public byte Bag { get; set; }
        public byte Slot { get; set; }
        public int ItemID { get; set; }
        public byte MonsterType { get; set; }
        public int MonsterID { get; set; }
    }

    public class RequestPetStats
    {
        public int MapID { get; set; }
        public int PetID { get; set; }
    }

    public class RequestSpawnOtherPet
    {
        public int PetID { get; set; }
        public int MapID { get; set; }
    }

    public class ResurrectPetInfo
    {
        public int PetID { get; set; }
        public byte Bag { get; set; }
        public byte Slot { get; set; }
    }

    public class RenamePetInfo
    {
        public int MapID { get; set; }
        public int PetID { get; set; }
        public string NewName { get; set; }
    }

    public class SendPetToMeInfo
    {
        public int PetID { get; set; }
        public int MapID { get; set; }
    }

    public class TradePetInfo
    {
        public int TargetID { get; set; }
        public int PetID { get; set; }
    }

    public class TradePetAmountInfo
    {
        public PetTradeAction Action { get; set; }
        public int OwnerID { get; set; }
        public int TargetID { get; set; }
        public int PetID { get; set; }
        public int MoneyWanted { get; set; }
    }

    public class SealPetInfo
    {
        public int PetID { get; set; }
        public byte Bag { get; set; }
        public byte Slot { get; set; }
    }

    public class UnSealPetInfo
    {
        public byte Bag { get; set; }
        public byte Slot { get; set; }
    }

    public class StackItemInfo
    {
        public byte Bag { get; set; }
        public byte Slot { get; set; }
        public int ItemID { get; set; }
        public byte StackBag { get; set; }
        public byte StackSlot { get; set; }
        public int StackItemID { get; set; }
    }


    public class CreateGuildInfo
    {
        public string GuildName { get; set; }
    }

    public class RequestGuildInfo
    {
        public int GuildID { get; set; }
    }

    public class RequestGuildMemberStatsInfo
    {
        public int MemberID { get; set; }
    }

    public class RequestGuildMemberChangeRankInfo
    {
        public int MemberID { get; set; }
        public GuildOrder OldRank { get; set; }
        public GuildOrder NewRank { get; set; }
    }

    public class RequestGuildMemberChangeTitleInfo
    {
        public int GuildID { get; set; }
        public int MemberID { get; set; }
        public string NewTitle { get; set; }
    }

    public class RequestGuildChangeNoticeInfo
    {
        public string NewNotice { get; set; }
    }

    public class RequestJoinGuildInfo
    {
        public GuildJoinAnswer Answer { get; set; }
        public int AskerID { get; set; }
        public int TargetID { get; set; }
    }

    public class RequestLeaveGuildInfo
    {
        public int PlayerID { get; set; }
        public int Something { get; set; }
    }

    public class GuildChatInfo
    {
        public byte Type { get; set; }
        public string Message { get; set; }
    }

    public class UseRebirthPillInfo
    {
        public byte Bag { get; set; }
        public byte Slot { get; set; }
        public int ItemID { get; set; }
    }

    [Serializable]
    public class BaseEntity
    {
        public string Name { get; set; }
        //public short X { get; set; }
        //public short Y { get; set; }
        public Position Position {get;set;}

        public virtual AttackInfo OnAttack(Monster m) { return null; }
        public virtual AttackInfo OnAttack(Character ch) { return null; }
        public virtual AttackInfo OnAttack(Pet pet) { return null; }

        public virtual CastSkillInfo OnCast(Monster m) { return null; }
        public virtual CastSkillInfo OnCast(Character ch, BaseSkill skill) { return null; }
        public virtual CastSkillInfo OnCast(Pet pet) { return null; }

        public virtual MovementInfo OnMove(Character target) { return null; }

        public virtual OnSeeEntityInfo OnSeeEntity(Character ch) { return null; } // THESE EXECUTE MOVE; ETC TOWARDS THE ENTITY TO BEAT
        //public virtual void OnSeeEntity(Pet pet);      // THE SHIT OUT OF HIM ^_^

        public bool PercentSuccess(double percent)
        {
            return ((double)Random().Next(1, 1000000)) / 10000 >= 100 - percent;
        }
        public int GetDistance(float X, float Y, float X2, float Y2)
        {
            return (int)Math.Max(Math.Abs(X - X2), Math.Abs(Y - Y2));
        }

        public Random Random()
        {
            return new Random();
        }
    }

    public class PetEvolution
    {
        public short Level {get;set;}
        public byte Evolution { get; set; }
        public byte Type { get; set; }
    }

    public class Pet : BaseEntity
    {
        public EntityType Type { get { return EntityType.Pet; } }
        public PetEvolution[] EvolutionTable { get; set; }

        public bool Alive { get; set; }
        public int PetID { get; set; }
        public int OwnerID { get; set; }
        public int MapID { get; set; }
        public byte PetType { get; set; }
        public byte PetBaseType { get; set; }
        public short Level { get; set; }

        public int CurHealth { get; set; }
        public int MaxHealth { get; set; }
        public int Damage { get; set; }
        public int Defense { get; set; }
        public int AttackRating { get; set; }

        public float BonusHealth { get { return (float)(MaxHealth * LifeInc); } }
        public float BonusDamage { get { return (float)(Damage * DamageInc); } }
        public float BonusAttackRating { get { return (float)(AttackRating * AttackRatingInc); } }
        public float BonusDefense { get { return (float)(Defense * DefenseInc); } }
        public float BonusExperience { get; set; }

        public float DamageInc { get; set; }
        public float DefenseInc { get; set; }
        public float LifeInc { get; set; }
        public float AttackRatingInc { get; set; }

        public int TotalHealth { get { return MaxHealth + (int)BonusHealth; } }
        public int TotalDamage { get { return Damage + (int)BonusDamage; } }
        public int TotalDefense { get { return Defense + (int)BonusDefense; } }
        public int TotalAttackRating { get { return AttackRating + (int)BonusAttackRating; } }

        public long CurrentExperience { get; set; }
        public long NegativeExperience { get; set; }
        public long ExperienceToLevel { get; set; }
        public byte Wildness { get; set; }
        public byte Evolution { get; set; }
        public bool IsLegendary { get; set; }
        public bool IsSealed { get; set; }
        public bool IsUsingEmpathy { get; set; }
        public bool IsUsingBuff { get; set; }
        

        public override AttackInfo OnAttack(Monster m)
        {
            if (m.Alive)
            {
                AttackInfo info = new AttackInfo();
                info.Damage = 0;
                info.Critical = false;
                info.Type = AttackType.Miss;

                info.TargetID = PetID;
                info.TargetType = (byte)Type;
                info.TargetX = (short)Position.X;
                info.TargetY = (short)Position.Y;
                info.TargetZ = 0;

                info.AttackerID = m.MonsterID;
                info.AttackerType = (byte)m.Type;

                if (m.AttackRating > TotalAttackRating)
                {
                    info.Type = AttackType.Hit;
                    if (PercentSuccess(10))
                        info.Critical = true;
                }
                else
                {
                    int chance = Random().Next(1, TotalAttackRating);
                    if (chance <= m.AttackRating)
                        info.Type = AttackType.Hit;
                    else
                        info.Type = AttackType.Miss;
                }
                if (info.Type == AttackType.Hit)
                {
                    info.Damage = m.Damage - TotalDefense / 3;
                    if (info.Damage <= 0)
                        info.Damage = 1;
                    if (info.Critical)
                        info.Damage *= 6;
                    if (info.Damage <= CurHealth)
                    {
                        CurHealth -= info.Damage;
                    }
                    else if (info.Damage >= CurHealth)
                    {
                        Alive = false;
                        CurHealth = 0;
                        info.Dead = true;
                    }
                }

                return info;
            }
            else
                return base.OnAttack(m);
        }

        public override AttackInfo OnAttack(Character ch)
        {
            if (ch.Alive)
            {
                AttackInfo info = new AttackInfo();
                info.Damage = 0;
                info.Critical = false;
                info.Type = AttackType.Miss;

                info.TargetID = PetID;
                info.TargetType = (byte)Type;
                info.TargetX = (short)Position.X;
                info.TargetY = (short)Position.Y;
                info.TargetZ = 0;

                info.AttackerID = ch.CharacterId;
                info.AttackerType = (byte)ch.Type;

                if (ch.TotalAttackRating > TotalAttackRating)
                {
                    info.Type = AttackType.Hit;
                    if (PercentSuccess(ch.Critical))
                        info.Critical = true;
                }
                else
                {
                    int chance = Random().Next(1, TotalAttackRating);
                    if (chance <= ch.TotalAttackRating)
                        info.Type = AttackType.Hit;
                    else
                        info.Type = AttackType.Miss;
                }
                if (info.Type == AttackType.Hit)
                {
                    info.Damage = ch.TotalDamage - TotalDefense / 3;
                    if (info.Damage <= 0)
                        info.Damage = 1;
                    if (info.Critical)
                        info.Damage *= 6;
                    if (info.Damage <= CurHealth)
                    {
                        CurHealth -= info.Damage;
                    }
                    else if (info.Damage >= CurHealth)
                    {
                        Alive = false;
                        CurHealth = 0;
                        info.Dead = true;
                    }
                }

                return info;
            }
            else
                return base.OnAttack(ch);
        }
    }
    public class Monster : BaseEntity
    {
        public EntityType Type { get { return EntityType.Monster; } }
        protected Movement m_Movement;
        public Movement Movement
        {
            get
            {
                if (m_Movement == null)
                {
                    m_Movement = new Movement(this);
                }
                return m_Movement;
            }
        }

        public bool Attacking { get; set; }
        public BaseEntity Target { get; set; }
        public bool IsMoving { get; set; }
        public Position Destination { get; set; }

        public DateTime DeathTime;
        public DateTime LastMoveTime;
        public DateTime LastAttack;
        public bool Alive { get; set; }
        public float Time { get; set; }
        public int MonsterID { get; set; }
        public byte MonsterType { get; set; }
        public int MapID { get; set; }
        public short Level { get; set; }
        public short SpawnX { get; set; }
        public short SpawnY { get; set; }
        public short Direction { get; set; }
        public int MonsterReferenceID { get; set; }
        public int CurHealth { get; set; }
        public int MaxHealth { get; set; }
        public int Damage { get; set; }
        public int Defense { get; set; }
        public int AttackRating { get; set; }
        public byte MovementSpeed { get; set; }
        public short SightRange { get; set; }
        public short WanderRange { get; set; }
        public short AttackRange { get; set; }
        public int Experience { get; set; }
        public int HealPoint { get; set; }
        public short Regeneration { get; set; }
        public short GroupID { get; set; }
        public byte GroupOrder { get; set; }
        public int LeaderID { get; set; }
        public int HealthInc { get; set; }
        public short DamageInc { get; set; }
        public short DefenseInc { get; set; }
        public short AttackRatingInc { get; set; }
        public short AttackRangeInc { get; set; }
        public int ExperienceInc { get; set; }
        public bool IsTameable { get; set; }

        public float GetDistance(ref Position pt)
        {
            float dx = (pt.X - Position.X);
            float dy = (pt.Y - Position.Y);
            float dz = (pt.Z - Position.Z);

            return (float)Math.Sqrt((dx * dx) + (dy * dy) + (dz * dz));
        }

        public override AttackInfo OnAttack(Pet pet)
        {
            if (Alive)
            {
                AttackInfo info = new AttackInfo();
                info.Damage = 0;
                info.Critical = false;
                info.Type = AttackType.Miss;

                info.TargetID = MonsterID;
                info.TargetType = (byte)Type;
                info.TargetX = (short)Position.X;
                info.TargetY = (short)Position.Y;
                info.TargetZ = 0;

                info.AttackerID = pet.PetID;
                info.AttackerType = (byte)pet.Type;

                if (pet.TotalAttackRating > AttackRating)
                {
                    info.Type = AttackType.Hit;
                    if (PercentSuccess(10))
                        info.Critical = true;
                }
                else
                {
                    int chance = Random().Next(1, AttackRating);
                    if (chance <= pet.TotalAttackRating)
                        info.Type = AttackType.Hit;
                    else
                        info.Type = AttackType.Miss;
                }
                if (info.Type == AttackType.Hit)
                {
                    info.Damage = pet.TotalDamage - Defense / 3;
                    if (info.Damage <= 0)
                        info.Damage = 1;
                    if (info.Critical)
                        info.Damage *= 6;
                    if (info.Damage <= CurHealth)
                    {
                        CurHealth -= info.Damage;
                    }
                    else if (info.Damage >= CurHealth)
                    {
                        Alive = false;
                        CurHealth = 0;
                        DeathTime = DateTime.Now;
                        info.Dead = true;
                        info.Experience = Experience;
                    }
                }

                return info;
            }
            else
                return base.OnAttack(pet);
        }
        public override AttackInfo OnAttack(Character ch)
        {
            if (Alive && ch.Alive)
            {
                AttackInfo info = new AttackInfo();
                info.Damage = 0;
                info.Critical = false;
                info.Type = AttackType.Miss;

                info.TargetID = MonsterID;
                info.TargetType = (byte)Type;
                info.TargetX = (short)Position.X;
                info.TargetY = (short)Position.Y;
                info.TargetZ = 0;

                info.AttackerID = ch.CharacterId;
                info.AttackerType = (byte)ch.Type;

                if (ch.TotalAttackRating > AttackRating)
                {
                    info.Type = AttackType.Hit;
                    if (PercentSuccess(ch.Critical))
                        info.Critical = true;
                }
                else
                {
                    int chance = Random().Next(1, AttackRating);
                    if (chance <= ch.TotalAttackRating)
                        info.Type = AttackType.Hit;
                    else
                        info.Type = AttackType.Miss;
                }
                if (info.Type == AttackType.Hit)
                {
                    info.Damage = ch.TotalDamage - Defense / 3;
                    if (info.Damage <= 0)
                        info.Damage = 1;
                    if (info.Critical)
                        info.Damage *= 6;
                    if (info.Damage <= CurHealth)
                    {
                        CurHealth -= info.Damage;
                    }
                    else if (info.Damage >= CurHealth)
                    {
                        Alive = false;
                        CurHealth = 0;
                        DeathTime = DateTime.Now;
                        info.Dead = true;
                        info.Experience = Experience;
                    }
                }

                return info;
            }
            else
                return base.OnAttack(ch);
        }

        public override CastSkillInfo OnCast(Character ch, BaseSkill skill)
        {
            if (Alive && ch.Alive)
            {
                CastSkillInfo info = new CastSkillInfo();

                info.Dead = false;
                info.Type = AttackType.Miss;
                info.Damage = 0;

                info.CasterType = (byte)ch.Type;
                info.CasterID = ch.CharacterId;
                info.CasterX = (short)ch.Position.X;
                info.CasterY = (short)ch.Position.Y;
                info.CasterZ = 0;

                info.TargetID = MonsterID;
                info.TargetType = (byte)Type;
                info.TargetX = (short)Position.X;
                info.TargetY = (short)Position.Y;
                info.TargetZ = 0;

                if (ch.TotalAttackRating > AttackRating)
                {
                    info.Type = AttackType.Hit;
                }
                else
                {
                    int chance = Random().Next(1, AttackRating);
                    if (chance <= ch.TotalAttackRating)
                        info.Type = AttackType.Hit;
                    else
                        info.Type = AttackType.Miss;
                }
                if (info.Type == AttackType.Hit)
                {
                    info.Damage = ch.TotalDamage - Defense / 3;
                    if (info.Damage <= 0)
                        info.Damage = 1;

                    if (skill.IncDamage != 0)
                    {
                        info.Damage += skill.IncDamage;
                    }

                    if (skill.IncDamagePerc != 0)
                    {
                        float DamageInc = skill.IncDamagePerc;
                        float increase = (DamageInc / 100);
                        info.Damage = (int)(info.Damage * increase);
                    }

                    if (info.Damage <= CurHealth)
                    {
                        CurHealth -= info.Damage;
                    }
                    else if (info.Damage >= CurHealth)
                    {
                        Alive = false;
                        CurHealth = 0;
                        DeathTime = DateTime.Now;
                        info.Dead = true;
                        info.Experience = Experience;
                    }
                }

                return info;
            }
            else
                return base.OnCast(ch, skill);
        }

        public override MovementInfo OnMove(Character target)
        {
            if (Alive && target.Alive)
            {
                if (GetDistance(Position.X, Position.Y, target.Position.X, target.Position.Y) <= AttackRange + 15)
                {
                    Attacking = true;
                }

                MovementInfo move = new MovementInfo();

                move.FromX = (short)Position.X;
                move.FromY = (short)Position.Y;
                move.FromZ = 0;

                move.CharacterID = MonsterID;
                move.ToX = (short)target.Position.X;
                move.ToY = (short)target.Position.Y;
                move.ToZ = 0;

                double rotation = Math.Atan2(move.ToY - move.FromX, move.ToX - move.FromX);
                rotation = rotation * 180 / Math.PI;
                move.Rotation = (int)Math.Abs(rotation);

                return move;
            }
            else
                return base.OnMove(target);
        }

        public override OnSeeEntityInfo OnSeeEntity(Character ch)
        {
            if (Alive && ch.Alive)
            {
                OnSeeEntityInfo i = new OnSeeEntityInfo();
                if (GetDistance(Position.Y, Position.X, ch.Position.X, ch.Position.Y) <= AttackRange + 15)
                {
                    Attacking = true;
                    i.Attacking = true;
                }
                else
                {
                    i.Moving = true;
                }


                //TargetID = ch.CharacterId;
                //i.TargetID = TargetID;

                return i;
            }
            else
                return base.OnSeeEntity(ch);
        }
    }

    public class Character : BaseEntity
    {
        public override AttackInfo OnAttack(Pet pet)
        {
            if (Alive)
            {
                AttackInfo info = new AttackInfo();
                info.Damage = 0;
                info.Critical = false;
                info.Type = AttackType.Miss;

                info.TargetID = CharacterId;
                info.TargetType = (byte)Type;
                info.TargetX = (short)Position.X;
                info.TargetY = (short)Position.Y;
                info.TargetZ = 0;

                info.AttackerID = pet.PetID;
                info.AttackerType = (byte)pet.Type;

                if (pet.TotalAttackRating > TotalAttackRating)
                {
                    info.Type = AttackType.Hit;
                    if (PercentSuccess(10))
                        info.Critical = true;
                }
                else
                {
                    int chance = Random().Next(1, TotalAttackRating);
                    if (chance <= pet.TotalAttackRating)
                        info.Type = AttackType.Hit;
                    else
                        info.Type = AttackType.Miss;
                }
                if (info.Type == AttackType.Hit)
                {
                    info.Damage = pet.TotalDamage - TotalDefence / 3;

                    if (info.Damage <= 0)
                        info.Damage = 1;
                    if (info.Critical)
                        info.Damage *= 6;

                    if (UsingReflection())
                    {
                        float damageInc = ReflectPerc;
                        float increase = (damageInc / 100);
                        int amount = (int)(info.Damage * increase);
                        info.Damage -= amount;
                        if (info.Damage <= 0)
                            info.Damage = 1;

                        info.DoRefDamage = true;
                    }

                    if (UsingShadow())
                    {
                        if (info.Damage <= tempPet.CurHealth)
                        {
                            tempPet.CurHealth -= info.Damage;
                            info.PetDamaged = true;
                        }
                        else if (info.Damage >= tempPet.CurHealth)
                        {
                            info.PetDamaged = true;
                            info.PetDied = true;
                        }
                    }
                    else
                    {
                        if (info.Damage <= CurrentHp)
                        {
                            CurrentHp -= info.Damage;
                        }
                        else if (info.Damage >= CurrentHp)
                        {
                            Alive = false;
                            CurrentHp = 0;
                            DeathTime = DateTime.Now;
                            info.Dead = true;
                        }
                    }
                }

                return info;
            }
            else
                return base.OnAttack(pet);
        }
        public override AttackInfo OnAttack(Monster m)
        {
            if (m.Alive)
            {
                AttackInfo info = new AttackInfo();
                info.Damage = 0;
                info.Critical = false;
                info.Type = AttackType.Miss;

                info.TargetID = CharacterId;
                info.TargetType = (byte)Type;
                info.TargetX = (short)Position.X;
                info.TargetY = (short)Position.Y;
                info.TargetZ = 0;

                info.AttackerID = m.MonsterID;
                info.AttackerType = (byte)m.Type;

                if (m.AttackRating > TotalAttackRating)
                {
                    info.Type = AttackType.Hit;
                    if (PercentSuccess(10))
                        info.Critical = true;
                }
                else
                {
                    int chance = Random().Next(1, TotalAttackRating);
                    if (chance <= m.AttackRating)
                        info.Type = AttackType.Hit;
                    else
                        info.Type = AttackType.Miss;
                }
                if (info.Type == AttackType.Hit)
                {
                    info.Damage = m.Damage - TotalDefence / 3;
                    if (info.Damage <= 0)
                        info.Damage = 1;
                    if (info.Critical)
                        info.Damage *= 6;

                    if (UsingReflection())
                    {
                        float damageInc = ReflectPerc;
                        float increase = (damageInc / 100);
                        int amount = (int)(info.Damage * increase);
                        info.Damage -= amount;
                        if (info.Damage <= 0)
                            info.Damage = 1;

                        info.DoRefDamage = true;
                    }

                    if (UsingShadow())
                    {
                        if (info.Damage <= tempPet.CurHealth)
                        {
                            tempPet.CurHealth -= info.Damage;
                            info.PetDamaged = true;
                        }
                        else if (info.Damage >= tempPet.CurHealth)
                        {
                            info.PetDamaged = true;
                            info.PetDied = true;
                        }
                    }
                    else
                    {
                        if (info.Damage <= CurrentHp)
                        {
                            CurrentHp -= info.Damage;
                        }
                        else if (info.Damage >= CurrentHp)
                        {
                            Alive = false;
                            CurrentHp = 0;
                            DeathTime = DateTime.Now;
                            info.Dead = true;
                        }
                    }
                }

                return info;
            }
            else
                return base.OnAttack(m);
        }
        public override AttackInfo OnAttack(Character ch)
        {
            if (Alive && ch.Alive)
            {
                AttackInfo info = new AttackInfo();
                info.Damage = 0;
                info.Critical = false;
                info.Type = AttackType.Miss;

                info.TargetID = CharacterId;
                info.TargetType = (byte)Type;
                info.TargetX = (short)Position.X;
                info.TargetY = (short)Position.Y;
                info.TargetZ = 0;

                info.AttackerID = ch.CharacterId;
                info.AttackerType = (byte)ch.Type;

                if (ch.TotalAttackRating > TotalAttackRating)
                {
                    info.Type = AttackType.Hit;
                    if (PercentSuccess(ch.Critical))
                        info.Critical = true;
                }
                else
                {
                    int chance = Random().Next(1, TotalAttackRating);
                    if (chance <= ch.TotalAttackRating)
                        info.Type = AttackType.Hit;
                    else
                        info.Type = AttackType.Miss;
                }
                if (info.Type == AttackType.Hit)
                {
                    info.Damage = ch.TotalDamage - TotalDefence / 3;
                    if (info.Damage <= 0)
                        info.Damage = 1;
                    if (info.Critical)
                        info.Damage *= 6;

                    if (UsingReflection())
                    {
                        float damageInc = ReflectPerc;
                        float increase = (damageInc / 100);
                        int amount = (int)(info.Damage * increase);
                        info.Damage -= amount;
                        if (info.Damage <= 0)
                            info.Damage = 1;

                        info.DoRefDamage = true;
                    }

                    if (UsingShadow())
                    {
                        if (info.Damage <= tempPet.CurHealth)
                        {
                            tempPet.CurHealth -= info.Damage;
                            info.PetDamaged = true;
                        }
                        else if (info.Damage >= tempPet.CurHealth)
                        {
                            info.PetDamaged = true;
                            info.PetDied = true;
                        }
                    }
                    else
                    {
                        if (info.Damage <= CurrentHp)
                        {
                            CurrentHp -= info.Damage;
                        }
                        else if (info.Damage >= CurrentHp)
                        {
                            Alive = false;
                            CurrentHp = 0;
                            DeathTime = DateTime.Now;
                            info.Dead = true;
                        }
                    }
                }

                return info;
            }
            else
                return base.OnAttack(ch);
        }

        public override CastSkillInfo OnCast(Character ch, BaseSkill skill)
        {
            if (Alive && ch.Alive)
            {
                CastSkillInfo info = new CastSkillInfo();

                info.Dead = false;
                info.Type = AttackType.Miss;
                info.Damage = 0;

                info.CasterType = (byte)ch.Type;
                info.CasterID = ch.CharacterId;
                info.CasterX = (short)ch.Position.X;
                info.CasterY = (short)ch.Position.Y;
                info.CasterZ = 0;

                info.TargetID = CharacterId;
                info.TargetType = (byte)Type;
                info.TargetX = (short)Position.X;
                info.TargetY = (short)Position.Y;
                info.TargetZ = 0;

                if (ch.TotalAttackRating > TotalAttackRating)
                {
                    info.Type = AttackType.Hit;
                }
                else
                {
                    int chance = Random().Next(1, TotalAttackRating);
                    if (chance <= ch.TotalAttackRating)
                        info.Type = AttackType.Hit;
                    else
                        info.Type = AttackType.Miss;
                }
                if (info.Type == AttackType.Hit)
                {
                    info.Damage = ch.TotalDamage - TotalDefence / 3;
                    if (info.Damage <= 0)
                        info.Damage = 1;

                    if (skill.IncDamage != 0)
                    {
                        info.Damage += skill.IncDamage;
                    }

                    if (skill.IncDamagePerc != 0)
                    {
                        float DamageInc = skill.IncDamagePerc;
                        float increase = (DamageInc / 100);
                        info.Damage = (int)(info.Damage * increase);
                    }

                    if (info.Damage <= CurrentHp)
                    {
                        CurrentHp -= info.Damage;
                    }
                    else if (info.Damage >= CurrentHp)
                    {
                        Alive = false;
                        CurrentHp = 0;
                        DeathTime = DateTime.Now;
                        info.Dead = true;
                    }
                }

                return info;
            }
            else
                return base.OnCast(ch, skill);
        }

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
        protected Mirror mirror;

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



        public BaseItem GetItemFromBagById(int ReferenceId)
        {
            BaseItem item = null;

            try
            {
                var q = Bags[0].Items.Where(x => x.ReferenceID == ReferenceId);
                if (q.Count() > 0)
                    item = q.First();

                var r = Bags[1].Items.Where(x => x.ReferenceID == ReferenceId);
                if (r.Count() > 0)
                    item = r.First();

                return item;
            }
            catch
            {
                return null;
            }
        }

        public DateTime ManaRegenTime { get; set; }
        public DateTime HealthRegenTime { get; set; }
        public DateTime DeathTime { get; set; }
        public int HealthRegenInvertal = 15000;
        public int ManaRegenInvertal = 15000;
        public int HealthRegen = 12;
        public int ManaRegen = 5;

        public EntityType Type { get { return EntityType.Player; } }

        public Pet tempPet { get; set; } // summon
        public Pet Pet { get; set; }
        public Party Party { get; set; }
        public Guild Guild { get; set; }
        public TradeWindow TradeWindow { get; set; }
        public Shop Shop { get; set; }
        public Warehouse Warehouse { get; set; }
        public List<Friend> FriendList { get; set; }
        public List<BaseSkill> SkillList { get; set; }
        public QuickSlot[] QuickSlots { get; set; }
        public Equipment[] visual { get; set; }
        public ImperialSet ImperialSet { get; set; }
        public bool Online { get; set; }
        public bool Alive { get; set; }
        public int CharacterId { get; set; }
        public int AccountId { get; set; }
        public byte Class { get; set; }
        public short Level { get; set; }
        public int OldMapId { get; set; }
        public int MapId { get; set; }
        public Map Map { get; set; }
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
        public short GainedTrainings { get; set; }
        public short TpLevel { get; set; }

        public short FiveElementPoint { get; set; }
        public int RepulationPoint { get; set; }

        public long NegativeExp { get; set; }
        public long CurrentExp { get; set; }
        public long ExpToLevel { get; set; }

        public long ExpToTraining { get; set; }

        public int CurrentFEExp { get; set; }
        public int ExpTofePoint { get; set; }
        public byte Rebirth { get; set; }

        public short Critical { get; set; }
        public byte MovingSpeed { get; set; }
        public short AttackRange { get; set; }
        public int TotalDamage { get; set; }
        public int TotalDefence { get; set; }
        public int TotalAttackRating { get; set; }

        public int StatDamage { get { return Math.Abs(Strength * strStatModifier); } }
        public int StatDefence { get { return Math.Abs(Stamina * staminaStatModifier); } }
        public int StatAttackRating { get { return Math.Abs(Dexterity * dexterityStatModifier); } }
        public int StatHp { get { return Math.Abs(Energy * energyStatModifier); } }
        public int LevelHp { get { return Math.Abs(Level * lifeLevelModifier); } }
        public short LevelMana { get { return (short)Math.Abs(Level * manaLevelModifier); } }

        public int GetStatPointsPerLevel()
        {
            int test = this.Level;
            int test2 = 1; // to loop all levels
            int statpoint = 3;

            for (int i = 1; i < 101; i++)
            {
                if (i > test)
                    break;

                if (i == 41)
                    test2 = i;
                if (i == 81)
                    test2 = i;
                if (i == 99)
                    test2 = i;

                if (i < 41 && (i - test2) == 4)
                {
                    i++;
                    test2 = i;
                    statpoint++;
                }
                else if (i < 80 && i > 41 && (i - test2) == 3)
                {
                    test2 = i + 1;
                    statpoint++;
                }
                else if (i < 99 && i > 81 && (i - test2) == 2)
                {
                    i++;
                    test2 = i;
                    statpoint++;
                }
            }

            return statpoint;
        }

        public BaseItem ShopContainsItem(int itemId, byte slot)
        {
            try
            {
                BaseItem i = Shop.Items.First(x => x.ItemID == itemId && x.Slot == slot);
                return i;
            }
            catch
            {
                return null;
            }
        }

        public BaseSkill FindSkill(int skillid)
        {
            foreach (BaseSkill s in SkillList)
            {
                if (s.SkillID == skillid)
                    return s;
            }

            return null;
        }

        public bool ContainSkill(int skillid, int skilllevel)
        {
            foreach (BaseSkill s in SkillList)
            {
                if (s.SkillID == skillid && s.SkillLevel >= skilllevel)
                    return true;
            }

            return false;
        }

        public void UpdateSkill(BaseSkill skill)
        {
            BaseSkill igetsdeleted = null;

            foreach (BaseSkill s in SkillList)
            {
                if (s.SkillID == skill.SkillID)
                    igetsdeleted = s;
            }

            if (igetsdeleted != null)
            {
                SkillList.Remove(igetsdeleted);
                SkillList.Add(skill);
            }
        }

        public List<SoftSkill> GetSoftSkills()
        {
            List<SoftSkill> softs = new List<SoftSkill>();
            foreach (BaseSkill s in SkillList)
            {
                if (s is SoftSkill)
                    softs.Add((SoftSkill)s);
            }

            return softs;
        }

        public List<HardSkill> GetHardSkills()
        {
            List<HardSkill> hards = new List<HardSkill>();
            foreach (BaseSkill s in SkillList)
            {
                if (s is HardSkill)
                    hards.Add((HardSkill)s);
            }

            return hards;
        }

        public List<ActiveSkill> ActiveSkills = new List<ActiveSkill>();

        public bool UsingFastMovement()
        {
            foreach (ActiveSkill s in ActiveSkills)
            {
                if (s.Skill.SkillID == (int)HardSkills.Fast_Movement || s.Skill.SkillID == (int)HardSkills.Fast_Step || s.Skill.SkillID == (int)HardSkills.Cloud_Walk || s.Skill.SkillID == (int)HardSkills.Wind_Walk)
                {
                    return true;
                }
            }

            return false;
        }

        public bool UsingShadow()
        {
            foreach (ActiveSkill s in ActiveSkills)
            {
                if (s.Skill.SkillID == (int)HardSkills.Shadow_Strike || s.Skill.SkillID == (int)AbsorbSkills.Shadow_Focus || s.Skill.SkillID == (int)RebirthSkills.Duplicate_Attack)
                {
                    return true;
                }
            }

            return false;
        }

        public bool RemoveActiveSkill(int SkillID)
        {
            try
            {
                ActiveSkill s = ActiveSkills.Where(x => x.Skill.SkillID == SkillID).First();
                ActiveSkills.Remove(s);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UsingReflection()
        {
            foreach (ActiveSkill s in ActiveSkills)
            {
                if (s.Skill.SkillID == (int)HardSkills.Reflection)
                {
                    return true;
                }
            }

            return false;
        }
        public int ReflectPerc { get; set; }

        public bool NotCastedYet(BaseSkill skill)
        {
            foreach (ActiveSkill s in ActiveSkills)
            {
                if (s.Skill.SkillID == skill.SkillID)
                    return false;
            }

            return true;
        }

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

        public bool FindFreeSlotInWarehouse(BaseItem item, BagSlot bagSlot)
        {
            bool found = false;

            if (Warehouse.FindFreeSlot(item, bagSlot))
            {
                found = true;
                bagSlot.Bag = 5;
            }

            return found;
        }

        public bool FindFreeSlotInShop(BaseItem item, BagSlot bagSlot)
        {
            bool found = false;

            if (Shop.FindFreeSlot(item, bagSlot))
            {
                found = true;
                bagSlot.Bag = 6;
            }

            return found;
        }

        public bool FindFreeSlotInTradeWindow(BaseItem item, BagSlot bagSlot)
        {
            bool found = false;

            if (TradeWindow.FindFreeTradeSlot(item, bagSlot))
            {
                found = true;
                bagSlot.Bag = 3;
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
                {
                    if (value != null)
                    {
                        value.Bag = 0;
                        value.Slot = (byte)Slot.Hat;
                    }
                    head = value;
                }
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
        public Mirror Mirror
        {
            get { return mirror; }
            set
            {
                if (value == null || (this.Level >= value.RequiredLevel - 10) && value.PetID != 0)
                {
                    if (value != null)
                    {
                        value.Bag = 0;
                        value.Slot = (byte)Slot.Mirror;
                    }
                    mirror = value;
                }
                else
                    throw new RequiredClassException(Messages.REQUIREDCLASSEXCEPTION);
            }
        }

        public void CalculateTotalStats()
        {
            this.MovingSpeed = 0;
            this.AttackRange = 5;
            this.Critical = 0;
            this.MaxHp = energyStatModifier;
            this.MaxMana = 0;
            this.TotalDamage = 0;
            this.TotalDefence = 0;
            this.TotalAttackRating = 0;
            this.ManaRegen = 5;
            this.HealthRegen = 12;
            this.ReflectPerc = 0;
            this.ImperialSet = GetSetBonus();

            foreach (Equipment e in this.GetAllEquips())
            {
                if (e.bType == (byte)bType.Weapon)
                    this.AttackRange += e.AttackRange;
                if (e.bType == (byte)bType.Shoes)
                {
                    this.MovingSpeed = (byte)e.AttackSpeed;
                    this.Critical += e.Critical;
                    this.HealthRegen += e.IncLifeRegen;
                    this.ManaRegen += e.IncManaRegen;
                    this.MaxHp += e.IncMaxLife;
                    this.MaxMana += e.IncMaxMana;
                    this.TotalDamage += e.Damage;
                    this.TotalDefence += e.Defence;
                    this.TotalAttackRating += e.AttackRating;
                    this.TotalDamage += e.DragonDamage;
                    this.TotalDefence += e.DragonDefence;
                    this.TotalAttackRating += e.DragonAttackRating;
                }
                else
                {
                    this.Critical += e.Critical;
                    this.MaxHp += e.IncMaxLife;
                    this.MaxMana += e.IncMaxMana;
                    this.TotalDamage += e.Damage;
                    this.TotalDefence += e.Defence;
                    this.TotalAttackRating += e.AttackRating;
                    this.TotalDamage += e.DragonDamage;
                    this.TotalDefence += e.DragonDefence;
                    this.TotalAttackRating += e.DragonAttackRating;
                    this.HealthRegen += e.IncLifeRegen;
                    this.ManaRegen += e.IncManaRegen;
                }

                // fiqure way to get the 10% for mirrors more 

                if (e.RebirthHole > 1)
                {
                    RebirthHoleItems i = (RebirthHoleItems)e.RebirthHole;
                    switch (i)
                    {
                        case RebirthHoleItems.Damage:
                            this.TotalDamage += e.RebirthHoleStat;
                            break;

                        case RebirthHoleItems.Defense:
                            this.TotalDefence += e.RebirthHoleStat;
                            break;

                        case RebirthHoleItems.AttackRating:
                            this.TotalAttackRating += e.RebirthHoleStat;
                            break;

                        case RebirthHoleItems.Critical:
                            this.Critical += e.RebirthHoleStat;
                            break;

                        case RebirthHoleItems.MaxLife:
                            this.MaxHp += e.RebirthHoleStat;
                            break;

                        case RebirthHoleItems.MaxMana:
                            this.MaxMana += e.RebirthHoleStat;
                            break;

                        case RebirthHoleItems.LifeReg:
                            this.HealthRegen += e.RebirthHoleStat;
                            break;

                        case RebirthHoleItems.ManaReg:
                            this.ManaRegen += e.RebirthHoleStat;
                            break;
                    }
                }
            }

            this.TotalDamage += this.StatDamage;
            this.TotalDefence += this.StatDefence;
            this.TotalAttackRating += this.StatAttackRating;

            foreach (SoftSkill s in this.GetSoftSkills())
            {
                #region Focuses
                if (s.SkillID == (int)SoftSkills.Attack_Focus)
                {
                    TotalDamage += s.IncDamage;
                }
                if (s.SkillID == (int)SoftSkills.Defense_Focus)
                {
                    TotalDefence += s.IncDefense;
                }
                if (s.SkillID == (int)SoftSkills.Accuracy_Focus)
                {
                    TotalAttackRating += s.IncAttackRating;
                }
                if (s.SkillID == (int)SoftSkills.Life_Increase)
                {
                    MaxHp += s.IncHpMax;
                }
                if (s.SkillID == (int)SoftSkills.Mana_Increase)
                {
                    MaxMana += s.IncManaMax;
                }
                if (s.SkillID == (int)SoftSkills.Fast_Healing)
                {
                    this.HealthRegenInvertal = s.nEtc1;
                    this.HealthRegen += s.RecoverHp;
                }
                if (s.SkillID == (int)SoftSkills.Fast_Mana_Refill)
                {
                    this.ManaRegenInvertal = s.nEtc2;
                    this.ManaRegen += s.RecoverMana;
                }

                #endregion

                #region SwordMan Masteries
                if (s.SkillID == (int)SoftSkills.Sword_Mastery)
                {
                    float DamageInc = s.IncDamagePerc;
                    float increase = (DamageInc / 100) - 1.0f;

                    float DefInc = s.IncDefensePerc;
                    float defIncrease = (DefInc / 100) - 1.0f;

                    float ArInc = s.IncAttackRatingPerc;
                    float arIncrease = (ArInc / 100) - 1.0f;

                    if (this.hand is Sword)
                    {
                        TotalDamage += (int)(hand.Damage * increase);

                        if (hand.Defence > 0)
                            TotalDefence += (int)(hand.Defence * defIncrease);

                        if (hand.AttackRating > 0)
                            TotalAttackRating += (int)(hand.AttackRating * arIncrease);
                    }
                }
                if (s.SkillID == (int)SoftSkills.Blade_Mastery)
                {
                    float DamageInc = s.IncDamagePerc;
                    float increase = (DamageInc / 100) - 1.0f;

                    float DefInc = s.IncDefensePerc;
                    float defIncrease = (DefInc / 100) - 1.0f;

                    float ArInc = s.IncAttackRatingPerc;
                    float arIncrease = (ArInc / 100) - 1.0f;

                    if (this.hand is Blade)
                    {
                        TotalDamage += (int)(hand.Damage * increase);

                        if (hand.Defence > 0)
                            TotalDefence += (int)(hand.Defence * defIncrease);

                        if (hand.AttackRating > 0)
                            TotalAttackRating += (int)(hand.AttackRating * arIncrease);
                    }
                }
                #endregion

                #region Mage Masteries
                if (s.SkillID == (int)SoftSkills.Fan_Mastery)
                {
                    float DamageInc = s.IncDamagePerc;
                    float increase = (DamageInc / 100) - 1.0f;

                    float DefInc = s.IncDefensePerc;
                    float defIncrease = (DefInc / 100) - 1.0f;

                    float ArInc = s.IncAttackRatingPerc;
                    float arIncrease = (ArInc / 100) - 1.0f;

                    if (this.hand is Fan)
                    {
                        TotalDamage += (int)(hand.Damage * increase);

                        if (hand.Defence > 0)
                            TotalDefence += (int)(hand.Defence * defIncrease);

                        if (hand.AttackRating > 0)
                            TotalAttackRating += (int)(hand.AttackRating * arIncrease);
                    }
                }
                if (s.SkillID == (int)SoftSkills.Brush_Mastery)
                {
                    float DamageInc = s.IncDamagePerc;
                    float increase = (DamageInc / 100) - 1.0f;

                    float DefInc = s.IncDefensePerc;
                    float defIncrease = (DefInc / 100) - 1.0f;

                    float ArInc = s.IncAttackRatingPerc;
                    float arIncrease = (ArInc / 100) - 1.0f;

                    if (this.hand is Brush)
                    {
                        TotalDamage += (int)(hand.Damage * increase);

                        if (hand.Defence > 0)
                            TotalDefence += (int)(hand.Defence * defIncrease);

                        if (hand.AttackRating > 0)
                            TotalAttackRating += (int)(hand.AttackRating * arIncrease);
                    }
                }
                #endregion

                #region Warrior Masteries
                if (s.SkillID == (int)SoftSkills.Claw_Mastery)
                {
                    float DamageInc = s.IncDamagePerc;
                    float increase = (DamageInc / 100) - 1.0f;

                    float DefInc = s.IncDefensePerc;
                    float defIncrease = (DefInc / 100) - 1.0f;

                    float ArInc = s.IncAttackRatingPerc;
                    float arIncrease = (ArInc / 100) - 1.0f;

                    if (this.hand is Claw)
                    {
                        TotalDamage += (int)(hand.Damage * increase);

                        if (hand.Defence > 0)
                            TotalDefence += (int)(hand.Defence * defIncrease);

                        if (hand.AttackRating > 0)
                            TotalAttackRating += (int)(hand.AttackRating * arIncrease);
                    }
                }
                if (s.SkillID == (int)SoftSkills.Axe_Mastery)
                {
                    float DamageInc = s.IncDamagePerc;
                    float increase = (DamageInc / 100) - 1.0f;

                    float DefInc = s.IncDefensePerc;
                    float defIncrease = (DefInc / 100) - 1.0f;

                    float ArInc = s.IncAttackRatingPerc;
                    float arIncrease = (ArInc / 100) - 1.0f;

                    if (this.hand is Axe)
                    {
                        TotalDamage += (int)(hand.Damage * increase);

                        if (hand.Defence > 0)
                            TotalDefence += (int)(hand.Defence * defIncrease);

                        if (hand.AttackRating > 0)
                            TotalAttackRating += (int)(hand.AttackRating * arIncrease);
                    }
                }
                #endregion

                #region GhostFighter Masteries
                if (s.SkillID == (int)SoftSkills.Talon_Mastery)
                {
                    float DamageInc = s.IncDamagePerc;
                    float increase = (DamageInc / 100) - 1.0f;

                    float DefInc = s.IncDefensePerc;
                    float defIncrease = (DefInc / 100) - 1.0f;

                    float ArInc = s.IncAttackRatingPerc;
                    float arIncrease = (ArInc / 100) - 1.0f;

                    if (this.hand is Talon)
                    {
                        TotalDamage += (int)(hand.Damage * increase);

                        if (hand.Defence > 0)
                            TotalDefence += (int)(hand.Defence * defIncrease);

                        if (hand.AttackRating > 0)
                            TotalAttackRating += (int)(hand.AttackRating * arIncrease);
                    }
                }
                if (s.SkillID == (int)SoftSkills.Tonfa_Mastery) // Tonfa mastery
                {
                    float DamageInc = s.IncDamagePerc;
                    float increase = (DamageInc / 100) - 1.0f;

                    float DefInc = s.IncDefensePerc;
                    float defIncrease = (DefInc / 100) - 1.0f;

                    float ArInc = s.IncAttackRatingPerc;
                    float arIncrease = (ArInc / 100) - 1.0f;

                    if (this.hand is Tonfa)
                    {
                        TotalDamage += (int)(hand.Damage * increase);

                        if (hand.Defence > 0)
                            TotalDefence += (int)(hand.Defence * defIncrease);

                        if (hand.AttackRating > 0)
                            TotalAttackRating += (int)(hand.AttackRating * arIncrease);
                    }
                }
                #endregion
            }

            foreach (ActiveSkill s in this.ActiveSkills)
            {
                #region Swordman

                // Critical Hit
                if (s.Skill.SkillID == (int)HardSkills.Critical_Hit)
                {
                    Critical += s.Skill.IncCritical;
                }

                // IronBody
                if (s.Skill.SkillID == (int)HardSkills.Iron_Body)
                {
                    TotalDefence += s.Skill.IncDefense;
                }

                // Rage
                if (s.Skill.SkillID == (int)HardSkills.Rage)
                {
                    TotalDamage += s.Skill.IncDamage;
                }

                #endregion

                #region Mage
                if (s.Skill.SkillID == (int)HardSkills.War_Cry)
                {
                    TotalDamage += s.Skill.IncDamage;
                    TotalAttackRating += s.Skill.IncAttackRating;
                }
                #endregion

                #region Warrior
                if (s.Skill.SkillID == (int)HardSkills.Frenzy)
                {
                    float damageInc = s.Skill.IncDefensePerc;
                    float increase = (damageInc / 100);
                    int amount = (int)(TotalDefence * increase);
                    TotalDefence -= amount;
                    TotalDamage += amount;
                }

                if (s.Skill.SkillID == (int)HardSkills.Reflection)
                {
                    this.ReflectPerc = s.Skill.nEtc1;
                }
                #endregion

                #region Ghost Fighter
                if (s.Skill.SkillID == (int)HardSkills.Poison_Power_Up)
                {
                    float DamageInc = s.Skill.IncDamagePerc;
                    float increase = (DamageInc / 100);
                    TotalDamage = (int)(TotalDamage * increase);
                    TotalAttackRating += s.Skill.IncAttackRating;
                }
                #endregion

                #region Absorb Focuses

                if (s.Skill.SkillID == (int)AbsorbSkills.Ironbody_Focus)
                {
                    TotalDefence += s.Skill.IncDefense;
                }

                if (s.Skill.SkillID == (int)AbsorbSkills.Poison_Power_Up_Focus)
                {
                    float DamageInc = s.Skill.IncDamagePerc;
                    float increase = (DamageInc / 100);
                    TotalDamage = (int)(TotalDamage * increase);
                    TotalAttackRating += s.Skill.IncAttackRating;
                }

                if (s.Skill.SkillID == (int)AbsorbSkills.Warcry_Focus)
                {
                    TotalDamage += s.Skill.IncDamage;
                    TotalAttackRating += s.Skill.IncAttackRating;
                }

                #endregion

                #region Rebirth Skills

                if (s.Skill.SkillID == (int)RebirthSkills.Absorb_Force)
                {
                    float defInc = s.Skill.nEtc2;
                    float increase = (defInc / 100);
                    int amount = (int)(TotalDefence * increase);
                    TotalDefence += amount;
                }

                if (s.Skill.SkillID == (int)RebirthSkills.Extend_Range)
                {
                    float defInc = s.Skill.nEtc2;
                    float increase = (defInc / 100);
                    int amount = (int)(TotalDefence * increase);
                    TotalDefence += amount;
                }

                if (s.Skill.SkillID == (int)RebirthSkills.Strong_Defense)
                {
                    float defInc = s.Skill.IncDefensePerc;
                    float increase = (defInc / 100);
                    int amount = (int)(TotalDefence * increase);
                    TotalDefence += amount;
                }

                if (s.Skill.SkillID == (int)RebirthSkills.Once_Attack)
                {
                    float defInc = s.Skill.IncDefensePerc;
                    float increase = (defInc / 100);
                    int amount = (int)(TotalDefence * increase);
                    TotalDefence += amount;
                }


                #endregion
            }

            if (this.ImperialSet != Entities.ImperialSet.None)
            {
                switch (this.ImperialSet)
                {
                    case Entities.ImperialSet.IncreaseAttack:
                        this.TotalDamage += 300;
                        break;

                    case Entities.ImperialSet.IncreaseDefence:
                        this.TotalDefence += 300;
                        break;

                    case Entities.ImperialSet.IncreaseAccuracy:
                        this.TotalAttackRating += 300;
                        break;

                    case Entities.ImperialSet.IncreaseDmgDefAr:
                        this.TotalDamage += 300;
                        this.TotalDefence += 300;
                        this.TotalAttackRating += 300;
                        break;

                    case Entities.ImperialSet.IncreaseLife:
                        this.MaxHp += 800;
                        break;

                    case Entities.ImperialSet.IncreaseWeaponDamage:
                        float increase = 0.20f;
                        this.TotalDamage += (int)(hand.Damage * increase);
                        break;

                    case Entities.ImperialSet.IncreaseClothesDefence:
                        increase = 0.20f;
                        this.TotalDefence += (int)(body.Defence * increase);
                        break;

                    case Entities.ImperialSet.IncreaseHatAccuracy:
                        increase = 0.20f;
                        this.TotalAttackRating += (int)(head.AttackRating * increase);
                        break;
                }
            }

            if (this.Level > 1)
            {
                this.MaxHp += this.LevelHp + this.StatHp;
                this.MaxMana += this.LevelMana;
            }
            if (this.CurrentHp > this.MaxHp)
                this.CurrentHp = this.MaxHp;
            if (this.CurrentMana > this.MaxMana)
                this.CurrentMana = this.MaxMana;
        }

        private ImperialSet GetSetBonus()
        {
            #region Increase attack by 300 = Mystic Apricot Pendant - Sakura Pendant - Bright moon Lu - Tomb Grass Lu
            if (this.head != null && this.head.RebirthHole == 100 || this.feet != null && this.feet.RebirthHole == 100 || this.hand != null && this.hand.RebirthHole == 100
                || this.body != null && this.body.RebirthHole == 100)
            {
                if (this.head != null && this.head.RebirthHole == 300 || this.feet != null && this.feet.RebirthHole == 300 || this.hand != null && this.hand.RebirthHole == 300
                    || this.body != null && this.body.RebirthHole == 300)
                {
                    if (this.head != null && this.head.RebirthHole == 500 || this.feet != null && this.feet.RebirthHole == 500 || this.hand != null && this.hand.RebirthHole == 500
                        || this.body != null && this.body.RebirthHole == 500)
                    {
                        if (this.head != null && this.head.RebirthHole == 700 || this.feet != null && this.feet.RebirthHole == 700 || this.hand != null && this.hand.RebirthHole == 700
                            || this.body != null && this.body.RebirthHole == 700)
                        {
                            return ImperialSet.IncreaseAttack;
                        }
                    }
                }
            }
            #endregion

            #region Increase Defence by 300 = Deep Aroma Pendant - White Lotus Pendant - Fallen Snow Lu - Missing Crow Lu
            if (this.head != null && this.head.RebirthHole == 200 || this.feet != null && this.feet.RebirthHole == 200 || this.hand != null && this.hand.RebirthHole == 200
                || this.body != null && this.body.RebirthHole == 200)
            {
                if (this.head != null && this.head.RebirthHole == 400 || this.feet != null && this.feet.RebirthHole == 400 || this.hand != null && this.hand.RebirthHole == 400
                    || this.body != null && this.body.RebirthHole == 400)
                {
                    if (this.head != null && this.head.RebirthHole == 600 || this.feet != null && this.feet.RebirthHole == 600 || this.hand != null && this.hand.RebirthHole == 600
                        || this.body != null && this.body.RebirthHole == 600)
                    {
                        if (this.head != null && this.head.RebirthHole == 800 || this.feet != null && this.feet.RebirthHole == 800 || this.hand != null && this.hand.RebirthHole == 800
                            || this.body != null && this.body.RebirthHole == 800)
                        {
                            return ImperialSet.IncreaseDefence;
                        }
                    }
                }
            }
            #endregion

            #region Increase Accuracy by 300 - Sakura Pendent - Bright Moon Lu - Tomb Grass Lu - Golden Silkworm Thread
            if (this.head != null && this.head.RebirthHole == 300 || this.feet != null && this.feet.RebirthHole == 300 || this.hand != null && this.hand.RebirthHole == 300
                || this.body != null && this.body.RebirthHole == 300)
            {
                if (this.head != null && this.head.RebirthHole == 500 || this.feet != null && this.feet.RebirthHole == 500 || this.hand != null && this.hand.RebirthHole == 500
                    || this.body != null && this.body.RebirthHole == 500)
                {
                    if (this.head != null && this.head.RebirthHole == 700 || this.feet != null && this.feet.RebirthHole == 700 || this.hand != null && this.hand.RebirthHole == 700
                        || this.body != null && this.body.RebirthHole == 700)
                    {
                        if (this.head != null && this.head.RebirthHole == 900 || this.feet != null && this.feet.RebirthHole == 900 || this.hand != null && this.hand.RebirthHole == 900
                            || this.body != null && this.body.RebirthHole == 900)
                        {
                            return ImperialSet.IncreaseAccuracy;
                        }
                    }
                }
            }
            #endregion

            #region Increase Life by 800 - White Lotus Pendent - Fallen Snow Lu - Missing Crow Lu - Silver Silkworm Thread
            if (this.head != null && this.head.RebirthHole == 400 || this.feet != null && this.feet.RebirthHole == 400 || this.hand != null && this.hand.RebirthHole == 400
                || this.body != null && this.body.RebirthHole == 400)
            {
                if (this.head != null && this.head.RebirthHole == 600 || this.feet != null && this.feet.RebirthHole == 600 || this.hand != null && this.hand.RebirthHole == 600
                    || this.body != null && this.body.RebirthHole == 600)
                {
                    if (this.head != null && this.head.RebirthHole == 800 || this.feet != null && this.feet.RebirthHole == 800 || this.hand != null && this.hand.RebirthHole == 800
                        || this.body != null && this.body.RebirthHole == 800)
                    {
                        if (this.head != null && this.head.RebirthHole == 1000 || this.feet != null && this.feet.RebirthHole == 1000 || this.hand != null && this.hand.RebirthHole == 1000
                            || this.body != null && this.body.RebirthHole == 1000)
                        {
                            return ImperialSet.IncreaseLife;
                        }
                    }
                }
            }
            #endregion

            #region Additional 20% Increase Weaponary Attack = Bright Moon Lu - Tomb Grass Lu - Golden Silkworm Thread - Blue Ghost Card
            if (this.head != null && this.head.RebirthHole == 500 || this.feet != null && this.feet.RebirthHole == 500 || this.hand != null && this.hand.RebirthHole == 500
                || this.body != null && this.body.RebirthHole == 500)
            {
                if (this.head != null && this.head.RebirthHole == 700 || this.feet != null && this.feet.RebirthHole == 700 || this.hand != null && this.hand.RebirthHole == 700
                    || this.body != null && this.body.RebirthHole == 700)
                {
                    if (this.head != null && this.head.RebirthHole == 900 || this.feet != null && this.feet.RebirthHole == 900 || this.hand != null && this.hand.RebirthHole == 900
                        || this.body != null && this.body.RebirthHole == 900)
                    {
                        if (this.head != null && this.head.RebirthHole == 1100 || this.feet != null && this.feet.RebirthHole == 1100 || this.hand != null && this.hand.RebirthHole == 1100
                            || this.body != null && this.body.RebirthHole == 1100)
                        {
                            return ImperialSet.IncreaseWeaponDamage;
                        }
                    }
                }
            }
            #endregion

            #region Additional 20% increase in clothes defence - Fallen Snow Lu - Missing Crow Lu - Silver Silkworm Thread - Red Ghost Card
            if (this.head != null && this.head.RebirthHole == 600 || this.feet != null && this.feet.RebirthHole == 600 || this.hand != null && this.hand.RebirthHole == 600
                || this.body != null && this.body.RebirthHole == 600)
            {
                if (this.head != null && this.head.RebirthHole == 800 || this.feet != null && this.feet.RebirthHole == 800 || this.hand != null && this.hand.RebirthHole == 800
                    || this.body != null && this.body.RebirthHole == 800)
                {
                    if (this.head != null && this.head.RebirthHole == 1000 || this.feet != null && this.feet.RebirthHole == 1000 || this.hand != null && this.hand.RebirthHole == 1000
                        || this.body != null && this.body.RebirthHole == 1000)
                    {
                        if (this.head != null && this.head.RebirthHole == 1101 || this.feet != null && this.feet.RebirthHole == 1101 || this.hand != null && this.hand.RebirthHole == 1101
                            || this.body != null && this.body.RebirthHole == 1101)
                        {
                            return ImperialSet.IncreaseClothesDefence;
                        }
                    }
                }
            }
            #endregion

            #region Additional 20% Increase in Hat accuracy = Mystic Apricot Pendent - Tomb Grass Lu - Golden Silkworm Thread - Blue Ghost Card
            if (this.head != null && this.head.RebirthHole == 100 || this.feet != null && this.feet.RebirthHole == 100 || this.hand != null && this.hand.RebirthHole == 100
                || this.body != null && this.body.RebirthHole == 100)
            {
                if (this.head != null && this.head.RebirthHole == 700 || this.feet != null && this.feet.RebirthHole == 700 || this.hand != null && this.hand.RebirthHole == 700
                    || this.body != null && this.body.RebirthHole == 700)
                {
                    if (this.head != null && this.head.RebirthHole == 900 || this.feet != null && this.feet.RebirthHole == 900 || this.hand != null && this.hand.RebirthHole == 900
                        || this.body != null && this.body.RebirthHole == 900)
                    {
                        if (this.head != null && this.head.RebirthHole == 1100 || this.feet != null && this.feet.RebirthHole == 1100 || this.hand != null && this.hand.RebirthHole == 1100
                            || this.body != null && this.body.RebirthHole == 1100)
                        {
                            return ImperialSet.IncreaseHatAccuracy;
                        }
                    }
                }
            }
            #endregion

            #region Additional 20% Increase In Experience Points = Deep Aroma Pendent - Mission Crow Lu - Silver Silkworm Thread - Read Ghost Card 
            if (this.head != null && this.head.RebirthHole == 200 || this.feet != null && this.feet.RebirthHole == 200 || this.hand != null && this.hand.RebirthHole == 200
                    || this.body != null && this.body.RebirthHole == 200)
            {
                if (this.head != null && this.head.RebirthHole == 800 || this.feet != null && this.feet.RebirthHole == 800 || this.hand != null && this.hand.RebirthHole == 800
                    || this.body != null && this.body.RebirthHole == 800)
                {
                    if (this.head != null && this.head.RebirthHole == 1000 || this.feet != null && this.feet.RebirthHole == 1000 || this.hand != null && this.hand.RebirthHole == 1000
                        || this.body != null && this.body.RebirthHole == 1000)
                    {
                        if (this.head != null && this.head.RebirthHole == 1101 || this.feet != null && this.feet.RebirthHole == 1101 || this.hand != null && this.hand.RebirthHole == 1101
                            || this.body != null && this.body.RebirthHole == 1101)
                        {
                            return ImperialSet.IncreaseExperience1;
                        }
                    }
                }
            } 
            #endregion

            #region Additional 10% gain in Five Element experience points = Mystic Apricot Pendent - Sakura Pendent - Golden Silkworm Thread - Blue Ghost Card.
            if (this.head != null && this.head.RebirthHole == 100 || this.feet != null && this.feet.RebirthHole == 100 || this.hand != null && this.hand.RebirthHole == 100
                || this.body != null && this.body.RebirthHole == 100)
            {
                if (this.head != null && this.head.RebirthHole == 300 || this.feet != null && this.feet.RebirthHole == 300 || this.hand != null && this.hand.RebirthHole == 300
                    || this.body != null && this.body.RebirthHole == 300)
                {
                    if (this.head != null && this.head.RebirthHole == 900 || this.feet != null && this.feet.RebirthHole == 900 || this.hand != null && this.hand.RebirthHole == 900
                        || this.body != null && this.body.RebirthHole == 900)
                    {
                        if (this.head != null && this.head.RebirthHole == 1100 || this.feet != null && this.feet.RebirthHole == 1100 || this.hand != null && this.hand.RebirthHole == 1100
                            || this.body != null && this.body.RebirthHole == 1100)
                        {
                            return ImperialSet.IncreaseFExperience1;
                        }
                    }
                }
            }
            #endregion

            #region Additional 10% gain in Five Element Efficiency = Deep Aroma Pendent - White Lotus Pendant - Silver Silkworm Thread - Red Ghost Card
            if (this.head != null && this.head.RebirthHole == 200 || this.feet != null && this.feet.RebirthHole == 200 || this.hand != null && this.hand.RebirthHole == 200
                || this.body != null && this.body.RebirthHole == 200)
            {
                if (this.head != null && this.head.RebirthHole == 400 || this.feet != null && this.feet.RebirthHole == 400 || this.hand != null && this.hand.RebirthHole == 400
                    || this.body != null && this.body.RebirthHole == 400)
                {
                    if (this.head != null && this.head.RebirthHole == 1000 || this.feet != null && this.feet.RebirthHole == 1000 || this.hand != null && this.hand.RebirthHole == 1000
                        || this.body != null && this.body.RebirthHole == 1000)
                    {
                        if (this.head != null && this.head.RebirthHole == 1101 || this.feet != null && this.feet.RebirthHole == 1101 || this.hand != null && this.hand.RebirthHole == 1101
                            || this.body != null && this.body.RebirthHole == 1101)
                        {
                            return ImperialSet.IncreaseFEBonus;
                        }
                    }
                }
            }
            #endregion

            #region Additional 10% gain in effiency of sealed items = Mystic Apricot Pendent - Sakura Pendent - Bright Moon Lu - Blue Ghost Card.
            if (this.head != null && this.head.RebirthHole == 100 || this.feet != null && this.feet.RebirthHole == 100 || this.hand != null && this.hand.RebirthHole == 100
                || this.body != null && this.body.RebirthHole == 100)
            {
                if (this.head != null && this.head.RebirthHole == 300 || this.feet != null && this.feet.RebirthHole == 300 || this.hand != null && this.hand.RebirthHole == 300
                    || this.body != null && this.body.RebirthHole == 300)
                {
                    if (this.head != null && this.head.RebirthHole == 500 || this.feet != null && this.feet.RebirthHole == 500 || this.hand != null && this.hand.RebirthHole == 500
                        || this.body != null && this.body.RebirthHole == 500)
                    {
                        if (this.head != null && this.head.RebirthHole == 1100 || this.feet != null && this.feet.RebirthHole == 1100 || this.hand != null && this.hand.RebirthHole == 1100
                            || this.body != null && this.body.RebirthHole == 1100)
                        {
                            return ImperialSet.IncreaseSealBonus;
                        }
                    }
                }
            }
            #endregion

            #region Additional 30% gain in Pet Experience Points = Deep Aroma Pendent - White Lotus Pendent - Fallen Snow Lu - Red Ghost Card.
            if (this.head != null && this.head.RebirthHole == 200 || this.feet != null && this.feet.RebirthHole == 200 || this.hand != null && this.hand.RebirthHole == 200
                || this.body != null && this.body.RebirthHole == 200)
            {
                if (this.head != null && this.head.RebirthHole == 400 || this.feet != null && this.feet.RebirthHole == 400 || this.hand != null && this.hand.RebirthHole == 400
                    || this.body != null && this.body.RebirthHole == 400)
                {
                    if (this.head != null && this.head.RebirthHole == 600 || this.feet != null && this.feet.RebirthHole == 600 || this.hand != null && this.hand.RebirthHole == 600
                        || this.body != null && this.body.RebirthHole == 600)
                    {
                        if (this.head != null && this.head.RebirthHole == 1101 || this.feet != null && this.feet.RebirthHole == 1101 || this.hand != null && this.hand.RebirthHole == 1101
                            || this.body != null && this.body.RebirthHole == 1101)
                        {
                            return ImperialSet.IncreasePetExperience;
                        }
                    }
                }
            }
            #endregion

            #region Increase Attack, Defence and Accuracy by 300 = Blue Ghost Card - Red Ghost Card - White Ghost Card - Black Ghost Card.
            if (this.head != null && this.head.RebirthHole == 1100 || this.feet != null && this.feet.RebirthHole == 1100 || this.hand != null && this.hand.RebirthHole == 1100
                || this.body != null && this.body.RebirthHole == 1100)
            {
                if (this.head != null && this.head.RebirthHole == 1101 || this.feet != null && this.feet.RebirthHole == 1101 || this.hand != null && this.hand.RebirthHole == 1101
                    || this.body != null && this.body.RebirthHole == 1101)
                {
                    if (this.head != null && this.head.RebirthHole == 1102 || this.feet != null && this.feet.RebirthHole == 1102 || this.hand != null && this.hand.RebirthHole == 1102
                        || this.body != null && this.body.RebirthHole == 1102)
                    {
                        if (this.head != null && this.head.RebirthHole == 1103 || this.feet != null && this.feet.RebirthHole == 1103 || this.hand != null && this.hand.RebirthHole == 1103
                            || this.body != null && this.body.RebirthHole == 1103)
                        {
                            return ImperialSet.IncreaseDmgDefAr;
                        }
                    }
                }
            }
            #endregion

            #region Additionial 30% Gain in Experience Points = Golden Silkworm Thread - Blue Ghost Card - White Ghost Card - White Ghost Card.
            if (this.head != null && this.head.RebirthHole == 900 || this.feet != null && this.feet.RebirthHole == 900 || this.hand != null && this.hand.RebirthHole == 900
                || this.body != null && this.body.RebirthHole == 900)
            {
                if (this.head != null && this.head.RebirthHole == 1100 || this.feet != null && this.feet.RebirthHole == 1100 || this.hand != null && this.hand.RebirthHole == 1100
                    || this.body != null && this.body.RebirthHole == 1100)
                {
                    if (this.head != null && this.head.RebirthHole == 1102 || this.feet != null && this.feet.RebirthHole == 1102 || this.hand != null && this.hand.RebirthHole == 1102
                        || this.body != null && this.body.RebirthHole == 1102)
                    {
                        if (this.head != null && this.head.RebirthHole == 1102 || this.feet != null && this.feet.RebirthHole == 1102 || this.hand != null && this.hand.RebirthHole == 1102
                            || this.body != null && this.body.RebirthHole == 1102)
                        {
                            return ImperialSet.IncreaseExperience2;
                        }
                    }
                }
            }
            #endregion

            #region Additional 30% gain in Five Element Experience Points = Golden Silkworm Thread - Red Ghost Card - Black Ghost Card - Black Ghost Card.
            if (this.head != null && this.head.RebirthHole == 900 || this.feet != null && this.feet.RebirthHole == 900 || this.hand != null && this.hand.RebirthHole == 900
                || this.body != null && this.body.RebirthHole == 900)
            {
                if (this.head != null && this.head.RebirthHole == 1101 || this.feet != null && this.feet.RebirthHole == 1101 || this.hand != null && this.hand.RebirthHole == 1101
                    || this.body != null && this.body.RebirthHole == 1101)
                {
                    if (this.head != null && this.head.RebirthHole == 1103 || this.feet != null && this.feet.RebirthHole == 1103 || this.hand != null && this.hand.RebirthHole == 1103
                        || this.body != null && this.body.RebirthHole == 1103)
                    {
                        if (this.head != null && this.head.RebirthHole == 1103 || this.feet != null && this.feet.RebirthHole == 1103 || this.hand != null && this.hand.RebirthHole == 1103
                            || this.body != null && this.body.RebirthHole == 1103)
                        {
                            return ImperialSet.IncreaseFExperience2;
                        }
                    }
                }
            }
            #endregion

            #region Additional 30% gain in experience Points = White Ghost Card - White Ghost Card - Black Ghost Card - Black Ghost Card
            if (this.head != null && this.head.RebirthHole == 1102 || this.feet != null && this.feet.RebirthHole == 1102 || this.hand != null && this.hand.RebirthHole == 1102
                || this.body != null && this.body.RebirthHole == 1102)
            {
                if (this.head != null && this.head.RebirthHole == 1102 || this.feet != null && this.feet.RebirthHole == 1102 || this.hand != null && this.hand.RebirthHole == 1102
                    || this.body != null && this.body.RebirthHole == 1102)
                {
                    if (this.head != null && this.head.RebirthHole == 1103 || this.feet != null && this.feet.RebirthHole == 1103 || this.hand != null && this.hand.RebirthHole == 1103
                        || this.body != null && this.body.RebirthHole == 1103)
                    {
                        if (this.head != null && this.head.RebirthHole == 1103 || this.feet != null && this.feet.RebirthHole == 1103 || this.hand != null && this.hand.RebirthHole == 1103
                            || this.body != null && this.body.RebirthHole == 1103)
                        {
                            return ImperialSet.IncreaseExperience3;
                        }
                    }
                }
            }
            #endregion

            #region Increase Damage at PvP by 100% = Blue Ghost Card - White Ghost Card - White Ghost Card - Black Ghost Card
            if (this.head != null && this.head.RebirthHole == 1100 || this.feet != null && this.feet.RebirthHole == 1100 || this.hand != null && this.hand.RebirthHole == 1100
                || this.body != null && this.body.RebirthHole == 1100)
            {
                if (this.head != null && this.head.RebirthHole == 1102 || this.feet != null && this.feet.RebirthHole == 1102 || this.hand != null && this.hand.RebirthHole == 1102
                    || this.body != null && this.body.RebirthHole == 1102)
                {
                    if (this.head != null && this.head.RebirthHole == 1102 || this.feet != null && this.feet.RebirthHole == 1102 || this.hand != null && this.hand.RebirthHole == 1102
                        || this.body != null && this.body.RebirthHole == 1102)
                    {
                        if (this.head != null && this.head.RebirthHole == 1103 || this.feet != null && this.feet.RebirthHole == 1103 || this.hand != null && this.hand.RebirthHole == 1103
                            || this.body != null && this.body.RebirthHole == 1103)
                        {
                            return ImperialSet.IncreasePvpDamage;
                        }
                    }
                }
            }
            #endregion

            #region Decrease Damage at PvP by 100% = Red Ghost Card - White Ghost Card - Black Ghost Card - Black Ghost Card
            if (this.head != null && this.head.RebirthHole == 1101 || this.feet != null && this.feet.RebirthHole == 1101 || this.hand != null && this.hand.RebirthHole == 1101
                || this.body != null && this.body.RebirthHole == 1101)
            {
                if (this.head != null && this.head.RebirthHole == 1102 || this.feet != null && this.feet.RebirthHole == 1102 || this.hand != null && this.hand.RebirthHole == 1102
                    || this.body != null && this.body.RebirthHole == 1102)
                {
                    if (this.head != null && this.head.RebirthHole == 1103 || this.feet != null && this.feet.RebirthHole == 1103 || this.hand != null && this.hand.RebirthHole == 1103
                        || this.body != null && this.body.RebirthHole == 1103)
                    {
                        if (this.head != null && this.head.RebirthHole == 1103 || this.feet != null && this.feet.RebirthHole == 1103 || this.hand != null && this.hand.RebirthHole == 1103
                            || this.body != null && this.body.RebirthHole == 1103)
                        {
                            return ImperialSet.DecreasePvpDamage;
                        }
                    }
                }
            }
            #endregion

            return ImperialSet.None;
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
        public EntityType Type { get { return EntityType.Npc; } }
        List<Bag> bags = new List<Bag>();
        public List<Bag> Bags
        {
            get { return bags; }
        }

        public BaseItem FindItem(int ReferenceId, int Slot)
        {
            foreach (Bag b in bags)
            {
                foreach (BaseItem i in b.Items)
                {
                    if (i.ReferenceID == ReferenceId && i.Slot == Slot)
                        return i;
                }
            }

            return null;
        }

        public bool IsEliteShop
        {
            get
            {
                if (NpcID == 68 || NpcID == 70 || NpcID == 61 || NpcID == 62 ||
                    NpcID == 43 || NpcID == 75 || NpcID == 66 || NpcID == 74 ||
                    NpcID == 67)
                    return true;
                else
                    return false;
            }
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
        public bool[,] WalkableTiles { get; set; }
        public List<Portal> Portals { get; set; }
        public List<Npc> Npcs { get; set; }
        public List<Monster> Monsters { get; set; }

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
    public class BaseItem
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
        public int SellPrice { get; set; }
        public byte TradeSlot { get; set; }
    }

    public class MapItem
    {
        public DateTime DropTime { get; set; }
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
        public int DroppedByCharacterID { get; set; }
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

    #region Pill Item

    public class RebirthPill : BaseItem
    {
        public short RequiredLevel { get; set; }
        public byte RequiredRebirth { get; set; }
        public byte ToRebirth { get; set; }
        public short IncreaseSp { get; set; }
    }

    #endregion

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

    [Serializable]
    public class RebirthBook : BookItem
    {
    }

    [Serializable]
    public class AbsorbBook : BookItem
    {
    }

    [Serializable]
    public class FiveElementBook : BookItem
    {
    }

    [Serializable]
    public class FeSkillBook : BookItem
    {
    }

    [Serializable]
    public class FourthBook : BookItem
    {
    }

    [Serializable]
    public class FocusBook : BookItem
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

    #region Jeon
    public class Jeon : BaseItem
    {
    }
    #endregion

    #region Store Tag
    [Serializable]
    public class StoreTag : BaseItem
    {
        public short TimeLeft { get; set; }
        public short TimeMax { get; set; }
    }
    #endregion

    #region ImbueItem
    [Serializable]
    public class ImbueItem : BaseItem
    {
        public short ImbueChance { get; set; }
        public short IncreaseValue { get; set; }
        public byte ImbueData { get; set; }
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

    [Serializable]
    public class RbHoleItem : ImbueItem
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
        public int Damage { get; set; }
        public int Defence { get; set; }
        public int AttackRating { get; set; }
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
        public int DragonDamage { get; set; }
        public int DragonDefence { get; set; }
        public int DragonAttackRating { get; set; }
        public short DragonLife { get; set; }
        public byte MappedData { get; set; }
        public byte ForceSlot { get; set; }
        public short RebirthHole { get; set; }
        public short RebirthHoleStat { get; set; }
    }

    [Serializable]
    public class Hand : Equipment
    {
        public ImbueStat NotWithWhite = ImbueStat.Damage;
    }

    [Serializable]
    public class Head : Equipment
    {
        public ImbueStat NotWithWhite = ImbueStat.AttackRating;
    }

    [Serializable]
    public class Body : Equipment
    {
        public ImbueStat NotWithWhite = ImbueStat.Defense;
    }

    [Serializable]
    public class Feet : Equipment
    {
    }

    [Serializable]
    public class Cape : Equipment
    {
        public ImbueStat NotWithWhite = ImbueStat.DefenseAndDamage;
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

    [Serializable]
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

    [Serializable]
    public class Mirror : Equipment
    {
        public int PetID { get; set; }

        public short LifeAbsorb { get; set; }
        public short DamageAbsorb { get; set; }
        public short DefenseAbsorb { get; set; }
        public short AttackRatingAbsorb { get; set; }
    }
    #endregion


    #region Quest Stuff

    [Serializable()]
    public class QuestReward
    {
        [XmlIgnore]
        public QuestObjectState State { get; set; }

        [XmlElement("AtObjective")]
        public byte AtObjective { get; set; }

        [XmlElement("Type")]
        public QuestRewardType Type { get; set; }

        [XmlElement("ID")]
        public int ID { get; set; }

        [XmlElement("Amount")]
        public int Amount { get; set; }

        public QuestReward(byte atObjective, int id, int amount, QuestRewardType type)
        {
            AtObjective = atObjective;
            Type = type;
            Amount = amount;
            ID = id;
        }

        public QuestReward()
        {
        }
    }

    [Serializable()]
    public class QuestObject
    {
        [XmlIgnore]
        public QuestObjectState State { get; set; }

        [XmlElement("Type")]
        public QuestObjectType Type { get; set; }
        [XmlElement("ID")]
        public int ID { get; set; }
        [XmlElement("TimesRequired")]
        public int AmountRequired { get; set; }
        [XmlElement("AtObjective")]
        public byte AtObjective { get; set; }

        [XmlIgnore]
        public int CurrentAmount { get; set; }

        public QuestObject(QuestObjectType type, int id, int required, byte atObjective)
        {
            Type = type;
            ID = id;
            AmountRequired = required;
            AtObjective = atObjective;
        }

        public QuestObject()
        {
        }
    }

    [Serializable()]
    public class BaseQuest
    {
        [XmlElement("QuestName")]
        public string Name { get; set; }
        [XmlElement("QuestID")]
        public int ID { get; set; }

        [XmlElement("QuestObjectives")]
        public List<QuestObject> Objectives { get; set; }

        [XmlElement("QuestRewards")]
        public List<QuestReward> Rewards { get; set; }
    }

    public class Quest : BaseQuest
    {
        public QuestState State { get; set; }
        public byte TimesDone { get; set; }
        public byte CurrentObjective { get; set; }
    }

    #endregion

#region Tame Item
    [Serializable]
    public class PetItem : BaseItem
    {
        public short TameChance { get; set; }
        public int HealLife { get; set; }
        public short DecreaseWildness { get; set; }
    }

    [Serializable]
    public class TameItem : PetItem
    {
    }
    
    [Serializable]
    public class PetPotion : PetItem
    {
    }
    
    [Serializable]
    public class PetFood : PetItem
    {
    }

    [Serializable]
    public class PetResurrectItem : PetItem
    {
    }
#endregion
}
