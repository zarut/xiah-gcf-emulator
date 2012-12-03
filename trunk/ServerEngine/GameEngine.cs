using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;

namespace ServerEngine
{
    public class GameEngine
    {
        public static Random Random = new Random();

        public GameEngine(string conString, string providerName)
        {

        }

        public int RandomChance(int Min, int Max)
        {
            int temp;
            temp = Random.Next(Min * (Min + 1) / 2, Max * (Max + 1) / 2);
            for (int i = 0; i <= Max; i++)
            {
                if (temp <= i * (i + 1) / 2)
                    return (Max - i);
            }
            return Max;
        }

        public int BlackImbue(Equipment Item, ref ImbueStat stat, ImbueItem imbueitem, int success)
        {
            int value = (int)(Item.RequiredLevel) + imbueitem.IncreaseValue;
            value += (int)(((value * 0.1) * Item.Plus) + ((imbueitem.ImbueChance / 5) * Item.Plus));
            value *= success;

            if (Item is Hand)
            {
                Item.Damage += (short)value;
                stat = ImbueStat.Damage;
            }
            if (Item is Cape)
            {
                if (Item.Damage > 0 && Item.Defence > 0)
                {
                    Item.Defence += (short)value;
                    Item.Damage += (short)value;
                    stat = ImbueStat.DefenseAndDamage;
                }
                else
                {
                    Item.Defence += (short)value;
                    stat = ImbueStat.Defense;
                }
            }
            if (Item is Head)
            {
                Item.AttackRating += (short)value;
                stat = ImbueStat.AttackRating;
            }
            if (Item is Body)
            {
                Item.Defence += (short)value;
                stat = ImbueStat.Defense;
            }
            if (Item is Feet)
            {
                value = (int)(Math.Round((double)(value / 50))) + 1;
                Item.Critical += (short)value;
                stat = ImbueStat.CriticalHit;
            }
            if (Item is Ring)
            {
                Item.IncMaxLife += (short)value;
                stat = ImbueStat.MaxLife;
            }
            if (Item is Necklace)
            {
                Item.IncMaxMana += (short)value;
                stat = ImbueStat.MaxMana;
            }

            return value;
        }

        public int WhiteImbue(Equipment Item, ref ImbueStat stat, ImbueItem imbueitem)
        {
            int value = (int)(Item.RequiredLevel);
            value += (int)(((value * 0.1) * Item.Slvl) + ((imbueitem.ImbueChance / 5) * Item.Slvl));

            List<ImbueStat> WhiteStats = new List<ImbueStat>();
            WhiteStats.Add(ImbueStat.Damage);
            WhiteStats.Add(ImbueStat.Defense);
            WhiteStats.Add(ImbueStat.AttackRating);
            WhiteStats.Add(ImbueStat.MaxLife);
            WhiteStats.Add(ImbueStat.MaxMana);
            WhiteStats.Add(ImbueStat.LifeReg);
            WhiteStats.Add(ImbueStat.ManaReg);
            WhiteStats.Add(ImbueStat.CriticalHit);

            if (Item is Cape)
            {
                WhiteStats.Remove(ImbueStat.Damage);
                WhiteStats.Remove(ImbueStat.Defense);
            }
            if (Item is Hand)
            {
                Hand weapon = Item as Hand;
                WhiteStats.Remove(weapon.NotWithWhite);
            }
            if (Item is Head)
            {
                Head hat = Item as Head;
                WhiteStats.Remove(hat.NotWithWhite);
            }
            if (Item is Body)
            {
                Body armor = Item as Body;
                WhiteStats.Remove(armor.NotWithWhite);
            }
            if (Item is Ring)
            {
                Ring ring = Item as Ring;
                value *= (int)2.94;
            }
            if (Item is Necklace)
            {
                Necklace neck = Item as Necklace;
                value *= (int)1.85;
            }

            if (imbueitem.ImbueData == 0)
            {
                int randomStat = Random.Next(0, WhiteStats.Count);
                stat = WhiteStats[randomStat];
            }
            else
            {
                switch (imbueitem.ImbueData)
                {
                    case 2:
                        stat = ImbueStat.Damage;
                        break;
                    case 3:
                        stat = ImbueStat.Defense;
                        break;

                    case 4:
                        stat = ImbueStat.AttackRating;
                        break;

                    case 5:
                        stat = ImbueStat.CriticalHit;
                        break;

                    case 6:
                        stat = ImbueStat.MaxLife;
                        break;

                    case 7:
                        stat = ImbueStat.MaxMana;
                        break;

                    case 8:
                        stat = ImbueStat.LifeReg;
                        break;

                    case 9:
                        stat = ImbueStat.ManaReg;
                        break;

                    default:
                        stat = ImbueStat.CriticalHit;
                        break;
                }
            }
            switch (stat)
            {
                case ImbueStat.Damage:
                    Item.Damage += (short)value;
                    break;

                case ImbueStat.Defense:
                    Item.Defence += (short)value;
                    break;

                case ImbueStat.AttackRating:
                    Item.AttackRating += (short)value;
                    break;

                case ImbueStat.MaxLife:
                    Item.IncMaxLife += (short)value;
                    break;

                case ImbueStat.MaxMana:
                    Item.IncMaxMana += (short)value;
                    break;

                case ImbueStat.LifeReg:
                    Item.IncLifeRegen += (short)value;
                    break;

                case ImbueStat.ManaReg:
                    Item.IncManaRegen += (short)value;
                    break;

                case ImbueStat.CriticalHit:
                    value = (int)(Math.Round((double)(value / 50))) + 1;
                    Item.Critical += (short)value;
                    break;
            }

            return value;
        }

        public ImbueStat ChooseStat(int stat)
        {
            switch (stat)
            {
                case 8: return (ImbueStat.RequiredLevel);
                case 9: return (ImbueStat.RequiredStrength);
                case 10: return (ImbueStat.RequiredDexterity);
                case 11: return (ImbueStat.RequiredStamina);
                case 12: return (ImbueStat.RequiredEnergy);
            }
            return (ImbueStat.None);
        }

        public void RedImbue(Equipment Item, ImbueStat stat, ref int value, bool success, ref ImbueError error)
        {
            if (success)
                error = ImbueError.Success;
            else
            {
                error = ImbueError.FailedToRemake;
                if (PercentSuccess(30))
                {
                    value *= -1;
                }
                else
                {
                    value *= 0;
                }
            }

            switch (stat)
            {
                case ImbueStat.RequiredLevel:
                    if (Item.RequiredLevel >= -value)
                        Item.RequiredLevel += (short)value;
                    else
                        Item.RequiredLevel = 0;
                    return;
                case ImbueStat.RequiredStrength:
                    if (Item.RequiredStrength >= -value)
                        Item.RequiredStrength += (short)value;
                    else
                        Item.RequiredStrength = 0;
                    return;
                case ImbueStat.RequiredStamina:
                    if (Item.RequiredStamina >= -value)
                        Item.RequiredStamina += (short)value;
                    else
                        Item.RequiredStamina = 0;
                    return;
                case ImbueStat.RequiredDexterity:
                    if (Item.RequiredDexterity >= -value)
                        Item.RequiredDexterity += (short)value;
                    else
                        Item.RequiredDexterity = 0;
                    return;
                case ImbueStat.RequiredEnergy:
                    if (Item.RequiredEnergy >= -value)
                        Item.RequiredEnergy += (short)value;
                    else
                        Item.RequiredEnergy = 0;
                    return;
                default:
                    stat = ImbueStat.None;
                    return;
            }

        }

        public bool PercentSuccess(double percent)
        {
            return ((double)Random.Next(1, 1000000)) / 10000 >= 100 - percent;
        }

        public BagSlot TryPickToBags(Bag[] playerBags, BaseItem item)
        {
            BagSlot bagSlot = new BagSlot();

            bool added = false;

            for (int i = 0; i < playerBags.Length; i++)
            {
                if (!added)
                {
                    if (playerBags[i].PickItem(item, bagSlot))
                    {
                        added = true;
                        bagSlot.Bag = (byte)(i + 1);
                        break;
                    }
                }
            }

            if (!added)
                throw new BagIsFullException(Messages.BAGISFULLEXCEPTION);
            return bagSlot;
        }
    }
}
