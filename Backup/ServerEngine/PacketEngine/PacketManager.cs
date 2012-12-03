using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;

namespace ServerEngine.PacketEngine
{
    public class PacketManager
    {
        #region All Servers

        /// <summary>
        /// Send public cryption key, first packet on each server when client connects
        /// </summary>
        /// <param name="key">Cryption key</param>
        /// <returns></returns>
        public static byte[] SendKey(int key)
        {
            Packet p = new Packet(5);
            p.WriteByte(key);
            return p.GetWrittenBuffer(PacketIds.SendKey);
        }

        #endregion

        #region Login Server

        #region Login Sends
        /// <summary>
        /// Send list of worlds to client
        /// </summary>
        /// <returns></returns>
        public static byte[] SendWorldList(WorldInfo[] worldInfos)
        {
            Packet p = new Packet(100);

            p.WriteByte(worldInfos.Length);
            for (int i = 0; i < worldInfos.Length; i++)
            {
                p.WriteByte(worldInfos[i].WorldId);
                p.WriteString(worldInfos[i].WorldName);
                p.WriteString(worldInfos[i].WorldDesc);
            }

            return p.GetWrittenBuffer(PacketIds.SendWorldList);
        }


        /// <summary>
        /// Send LoginState to client
        /// </summary>
        /// <param name="ls">State of login</param>
        /// <returns></returns>
        public static byte[] SendLoginState(LoginState ls, int ID)
        {
            Packet p = new Packet(100);
            if (ls == LoginState.Success)
            {
                p.WriteByte((short)ls);
                p.WriteShort(ID); // the total connection amounts from db or somewhere, used in gameserver
                p.WriteHexString("01 00 14");
            }
            else
            {
                p.WriteByte((short)ls);
                p.WriteHexString("00 00 00 00 00"); // if its error just logintype and rest zeroes
            }
            return p.GetWrittenBuffer(PacketIds.SendLoginState);
        }

        /// <summary>
        /// Send chosed world ip to client
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static byte[] SendWorldIP(string ip, int port)
        {
            Packet p = new Packet(100);
            p.WriteString(ip); // world ip, if theres multiple worlds request will send the world id in order to get right ones ip
            p.WriteInt(port);
            return p.GetWrittenBuffer(PacketIds.SendWorldIP);
        }
        #endregion

        #region Login Receives

        /// <summary>
        /// Handle Game Login packet from client
        /// </summary>
        /// <param name="p"></param>
        /// <param name="c"></param>
        public static GameLogin RecvGameLogin(Packet p)
        {
            p.Skip(2);
            string account = p.ReadString();
            string hash = p.ReadString();
            int version = p.ReadShort();

            GameLogin g = new GameLogin
            {
                AccountName = account,
                HashCode = hash,
                Version = version
            };

            return g;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="c"></param>
        /// <returns>World Id</returns>
        public static int RecvConnectWorld(Packet p)
        {
            p.Skip(2);
            int worldID = p.ReadByte(); // if theres plenty of worlds, it would get the ip & port from worldlist[worldID] i guess

            return worldID;
        }

        #endregion

        #endregion

        #region World Server

        #region World Sends

        /// <summary>
        /// Send Unit Server login state to client
        /// </summary>
        /// <param name="uls"></param>
        /// <param name="PlayerID"></param>
        /// <returns></returns>
        public static byte[] SendUnitLoginState(LoginState uls, int PlayerID)
        {
            Packet p = new Packet(100);
            if (uls == LoginState.Success)
            {
                p.WriteShort((short)uls);
                p.WriteInt(PlayerID); // if client were ingame before, else its just 00 00 00 00 
            }
            else
            {
                p.WriteShort((short)uls);
                p.WriteHexString("00 00 00 00"); // if its error just logintype and rest zeroes
            }

            return p.GetWrittenBuffer(PacketIds.SendUnitLoginState);
        }

        /// <summary>
        /// Send character list to client
        /// </summary>
        /// <returns></returns>
        public static byte[] SendCharacterList(Character[] characters)
        {

            Packet p = new Packet(200);
            p.WriteByte(characters.Length);

            foreach (Character c in characters)
            {
                p.WriteInt(c.MapId);
                p.WriteInt(c.CharacterId);
                p.WriteString(c.Name);
                p.WriteByte(c.Class);
                p.WriteShort(c.Level);
                p.WriteInt(c.CurrentHp);
                p.WriteInt(c.MaxHp);
                p.WriteInt(c.MaxMana);
                p.WriteShort(c.Strength);
                p.WriteShort(c.Stamina);
                p.WriteShort(c.Dexterity);
                p.WriteShort(c.Energy);
                p.WriteHexString("00 00 00 00 02 00 00 00 00 00 00 00"); // 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");
                foreach (Equipment e in c.GetVisuals())
                {
                    p.WriteShort(e.VisualID);
                    p.WriteByte(e.Plus);
                    p.WriteByte(e.Slvl);
                }

                p.WriteHexString("01 00"); // ending bytes 
            }

            return p.GetWrittenBuffer(PacketIds.SendCharacterList);
        }

        /// <summary>
        /// Create character
        /// </summary>
        /// <param name="ms">State of character making</param>
        /// <param name="playerid">ID of character that was made</param>
        /// <returns></returns>
        public static byte[] SendCreateCharacter(CharCreationState ms, int playerid)
        {
            Packet p = new Packet(500);
            p.WriteByte((byte)ms);
            if (ms == CharCreationState.Success)
                p.WriteInt(playerid);
            else
                p.WriteHexString("00 00 00 00");
            return p.GetWrittenBuffer(PacketIds.SendCreateCharacter);
        }

        /// <summary>
        /// Send channel to client
        /// </summary>
        /// <param name="CI">Channel Info, containing id, name, desc, ip address and port</param>
        /// <param name="TotalChannels"></param>
        /// <returns></returns>
        public static byte[] SendChannel(ChannelInfo CI, int TotalChannels)
        {
            Packet p = new Packet(500);
            p.WriteByte(TotalChannels); // total channels
            p.WriteByte(CI.ChannelId); // channel id
            p.WriteString(CI.ChannelName); // name
            p.WriteString(CI.ChannelDesc); // desc
            p.WriteHexString("12 F4 01");
            p.WriteString(CI.IPAddress); // ip
            p.WriteShort(CI.Port); // port
            p.WriteHexString("00 00 00");
            return p.GetWrittenBuffer(PacketIds.SendChannelList);
        }

        public static byte[] SendChannelStatus()
        {
            Packet p = new Packet(200);
            p.WriteHexString("02 00 01 01 01 32 00 01 02 01 08 00");
            return p.GetWrittenBuffer(PacketIds.SendChannelStatus);
        }

        #endregion

        #region World Recvs

        /// <summary>
        /// Generates the unit server login information from packet
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static UnitLogin RecvUnitLogin(Packet p)
        {
            p.Skip(2);
            string account = p.ReadString();
            int hash = p.ReadShort();
            int map = p.ReadShort();
            int channel = p.ReadByte();
            p.Skip(6);
            int characterID = p.ReadInt();

            UnitLogin u = new UnitLogin()
            {
                Account = account,
                AccountID = hash,
                Channel = channel,
                CharacterID = characterID
            };

            return u;
        }

        public static Character RecvCreateCharacter(Packet p)
        {
            p.Skip(2);
            byte cClass = p.ReadByte();
            string name = p.ReadString();

            Character c = new Character()
            {
                Class = cClass,
                Name = name
            };

            return c;
        }

        public static ChannelRequest RecvChannelRequest(Packet p)
        {
            p.Skip(2);
            int worldID = p.ReadByte();

            ChannelRequest c = new ChannelRequest()
            {
                WorldID = worldID
            };

            return c;
        }

        #endregion


        #endregion

        #region Channel Server

        #region Channel Sends

        public static byte[] SendChannelChange(int something, int something2)
        {
            Packet p = new Packet(200);
            p.WriteByte(0);
            p.WriteByte(something);
            p.WriteByte(something2);
            return p.GetWrittenBuffer(PacketIds.SendChannelChange);
        }

        public static byte[] SendRemoveCharacter(Character ch)
        {
            Packet p = new Packet(200);
            p.WriteHexString("00");
            p.WriteInt(ch.CharacterId);
            p.WriteHexString("01");
            p.WriteInt(ch.MapId);
            p.WriteHexString("00");
            return p.GetWrittenBuffer(PacketIds.SendRemoveCharacter);
        }

        public static byte[] SendPortal(Portal portal)
        {
            Packet p = new Packet(200);
            p.WriteHexString("00");
            p.WriteInt(portal.ToMapID);
            p.WriteShort(portal.ToX);
            p.WriteShort(portal.ToY);
            p.WriteHexString("01");
            return p.GetWrittenBuffer(PacketIds.SendPortal);
        }

        public static byte[] SendMapID(int mapId)
        {
            Packet p = new Packet(200);
            p.WriteHexString("00");
            p.WriteInt(mapId);

            return p.GetWrittenBuffer(PacketIds.SendMapId);
        }

        public static byte[] SendMapTime(int year, int month, int day, int hour)
        {
            Packet p = new Packet(200);
            p.WriteShort(year);
            p.WriteByte(month);
            p.WriteByte(day);
            p.WriteByte(hour);
            return p.GetWrittenBuffer(PacketIds.SendMapTime);
        }

        public static byte[] SendMapLoad2()
        {
            Packet p = new Packet(200);
            p.WriteHexString("38 35 01 00 00");
            return p.GetWrittenBuffer();
        }

        public static byte[] SendMapLoad3()
        {
            Packet p = new Packet(200);
            p.WriteHexString("23 3E 04 00 00 00 00 00");
            return p.GetWrittenBuffer();
        }

        public static byte[] SendMap(Map mh)
        {
            Packet p = new Packet(200);
            p.WriteByte(0);
            p.WriteInt(mh.MapID);
            p.WriteByte(0);
            p.WriteShort(mh.MultiplyValue);
            p.WriteByte(mh.MultiplyValue);
            p.WriteByte(1);
            p.WriteByte(mh.Portals.Count);
            foreach (Portal portal in mh.Portals)
            {
                p.WriteInt(portal.ToMapID);
                p.WriteShort(portal.FromX);
                p.WriteShort(portal.FromY);
                p.WriteShort(portal.Width);
                p.WriteShort(portal.Height);
                p.WriteByte(1); // type
            }
            //  p.WriteHexString("01 02 01 00 00 00 21 03 2A 03 40 00 40 00 01 0B 00 00 00 91 00 75 00 20 00 20 00 01");
            return p.GetWrittenBuffer(PacketIds.SendMap);
        }

        public static byte[] SendMap2()
        {
            Packet p = new Packet(200);
            p.WriteHexString("17 32 25 00 04 00 00 00 00 00 A0 05 00 00 01 00 00 00 00 A0 05 00 00 02 00 00 00 00 A0 05 00 00 03 00 00 00 00 A0 05 00 00");
            return p.GetWrittenBuffer();
        }

        public static byte[] SendMap3()
        {
            Packet p = new Packet(200);
            p.WriteHexString("18 32 01 00 00");
            return p.GetWrittenBuffer();
        }

        public static byte[] SendSpawnStats(Character ch) // something about repulation, current hp/mana, stats
        {
            Packet p = new Packet(200);
            p.WriteShort(ch.Level);
            p.WriteShort(ch.Strength);
            p.WriteShort(ch.Stamina);
            p.WriteShort(ch.Dexterity);
            p.WriteShort(ch.Energy);
            p.WriteHexString("01 01 01 01"); // unknown :(, maybe base stats?: D
            p.WriteShort(ch.MaxMana);
            p.WriteShort(ch.CurrentMana);
            p.WriteInt(ch.MaxHp);
            p.WriteInt(ch.CurrentHp);
            p.WriteLong(ch.CurrentExp); // cur exp
            p.WriteHexString("00 00 00 00 00 00 00 00"); // curxp to next training
            p.WriteLong(1200000); // exp to level
            p.WriteHexString("00 00 00 00 00 00 00 00"); // xp to next training
            p.WriteHexString("00 00");
            p.WriteHexString("04 00 00 00");
            p.WriteHexString("00 00");
            p.WriteHexString("08 00 00 00");
            p.WriteShort(ch.StatPoint); // stat points
            p.WriteHexString("6B 00 00 00"); // total trainings?
            p.WriteShort(ch.TrainingPoint); // current trainings
            p.WriteHexString("7C 00"); // damage from stats
            p.WriteHexString("C5 00 00 00"); // total damage (from items, skills etc)
            p.WriteHexString("11 00"); // defence from stats 
            p.WriteHexString("12 00 00 00"); // total defence (from items, skills etc)
            p.WriteHexString("13 00"); // attack rating from stats
            p.WriteHexString("14 00 00 00"); // total attack rating (from items, skills etc) 
            p.WriteHexString("07 00"); // attack range or speed 
            p.WriteHexString("0B 0B"); // animation speed (the speed hax thingy in xiah 2827)
            p.WriteHexString("0B"); // movement speed, from shoes ^_ ^ 
            p.WriteHexString("00 00 00 00 00");
            p.WriteInt(ch.Money);
            p.WriteHexString("02 00");  // Critical
            p.WriteHexString("1A 00");
            p.WriteInt(ch.Fame);
            p.WriteShort(ch.FiveElementPoint); // five element points (unused) 
            p.WriteInt(ch.CurrentFEExp); // current fe experience
            p.WriteHexString("02 00 00 00"); // fe experience to point, 40000 each level
            p.WriteHexString("00 00 00 00"); // that other five element skill experience :S
            p.WriteHexString("00 00"); // fire fe level 
            p.WriteHexString("00 00"); // water fe level
            p.WriteHexString("00 00"); // wood fe level
            p.WriteHexString("00 00"); // steel fe level
            p.WriteHexString("00 00"); // soil fe level
            p.WriteByte(ch.Rebirth); // rebirths
            p.WriteInt(ch.RepulationPoint); // Repulation points, the points near stat ones :U

            return p.GetWrittenBuffer(PacketIds.SendSpawnStats);
        }

        public static byte[] SendShoftSkillList()
        {
            Packet p = new Packet(200);
            p.WriteHexString("1C 31 01 00 00");
            return p.GetWrittenBuffer();
        }

        public static byte[] SendHardSkillList()
        {
            Packet p = new Packet(200);
            p.WriteHexString("1D 31 01 00 00");
            return p.GetWrittenBuffer();
        }

        public static byte[] SendBagItems(List<BaseItem> items, int bag)
        {
            Packet p = new Packet(200);
            //    p.WriteHexString("18 31 A4 00 00 02 00 73 83 D0 00 2A 4E 01 02 B0 04 00 00 00 00 00 00 02 00 00 04 00 05 00 06 00 07 00 08 00 C0 C8 00 C8 00 05 00 06 00 07 00 08 00 09 00 0A 00 0B 00 0C 00 0D 00 0E 00 0F 08 00 00 00 04 00 05 00 07 00 0A 00 0B 00 0C 00 0D 00 00 00 00 01 01 01 02 02 74 83 D0 00 35 4E 02 01 34 08 00 00 00 00 00 00 02 01 00 01 00 00 00 00 00 01 00 00 00 C8 64 00 64 00 00 00 03 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");
            p.WriteByte(bag);
            p.WriteByte(items.Count);
            foreach (BaseItem i in items)
            {
                p.WriteByte(i.Slot);
                p.WriteInt(i.ItemID);
                p.WriteShort(i.ReferenceID);
                p.WriteByte(i.bType);
                p.WriteByte(i.bKind);
                p.WriteShort(i.VisualID);
                p.WriteInt(i.Price);
                p.WriteHexString("00 00");
                if (i is Equipment)
                {
                    Equipment e = i as Equipment;
                    p.WriteByte(e.RequiredClass);
                    p.WriteShort(e.Amount);
                    p.WriteShort(e.RequiredLevel);
                    p.WriteShort(e.RequiredDexterity);
                    p.WriteShort(e.RequiredStrength);
                    p.WriteShort(e.RequiredStamina);
                    p.WriteShort(e.RequiredEnergy);
                    p.WriteByte(e.MaxImbueTries);
                    p.WriteShort(e.Durability);
                    p.WriteShort(e.MaxDurability);
                    p.WriteShort(e.Damage);
                    p.WriteShort(e.Defence);
                    p.WriteShort(e.AttackRating);
                    p.WriteShort(e.AttackSpeed);
                    p.WriteShort(e.AttackRange);
                    p.WriteShort(e.IncMaxLife);
                    p.WriteShort(e.IncMaxMana);
                    p.WriteShort(e.IncLifeRegen);
                    p.WriteShort(e.IncManaRegen);
                    p.WriteShort(e.Critical);
                    p.WriteByte(e.Plus);
                    p.WriteByte(e.Slvl);
                    p.WriteHexString("00"); // unknown
                    p.WriteByte(e.ImbueTries);
                    p.WriteHexString("00"); // unknown
                    p.WriteShort(e.DragonSuccessImbueTries);
                    p.WriteByte(e.DiscountRepairFee);
                    p.WriteShort(e.TotalDragonImbueTries);
                    p.WriteShort(e.DragonDamage);
                    p.WriteShort(e.DragonDefence);
                    p.WriteShort(e.DragonAttackRating);
                    p.WriteShort(e.DragonLife);
                    p.WriteByte(e.MappedData);
                    if (!(e is Ring) && !(e is Necklace) && !(e is Cape))
                        p.WriteByte(e.ForceSlot);
                    p.WriteHexString("00 00"); // unknown
                    if (e is Cape)
                    {
                        Cape c = e as Cape;
                        p.WriteHexString("00 00"); // required guild position 
                        p.WriteHexString("00 00 00 00 00 00 00");
                        p.WriteHexString("01 00"); // decrease times of durability
                        p.WriteByte(c.PolishImbueTries); // polish imbue tries
                        p.WriteShort(c.MaxPolishImbueTries); // polish max tries
                        p.WriteShort(c.VigiStat1); // stat1
                        p.WriteShort(c.VigiStat2); // stat2
                        p.WriteShort(c.VigiStat3); // stat3
                        p.WriteShort(c.VigiStat4); // stat4
                        p.WriteHexString("00 00"); // stat5
                        p.WriteShort(c.VigiStatAdd1); // stat1 increase
                        p.WriteShort(c.VigiStatAdd2); // stat2 increase
                        p.WriteShort(c.VigiStatAdd3); // stat3 increase
                        p.WriteShort(c.VigiStatAdd4); // stat4 increase
                        p.WriteHexString("00 00"); // stat5 increase
                        p.WriteHexString("00 00"); // unknown
                    }
                    if (!(e is Ring) && !(e is Necklace) && !(e is Cape))
                    {
                        p.WriteByte(e.RebirthHole);
                        p.WriteByte(e.RebirthHoleItem);
                        p.WriteShort(e.RebirthHoleStat);
                    }
                }
                if (i is ImbueItem)
                {
                    ImbueItem im = i as ImbueItem;
                    p.WriteByte(im.RequiredClass);
                    p.WriteShort(i.Amount);
                    p.WriteHexString("00"); // imbuestat, eg dec level, increase value etc
                    p.WriteShort(im.ImbueChance);
                    p.WriteShort(im.IncreaseValue);
                    p.WriteHexString("00 00"); // could be lowest lvl who knows
                    p.WriteHexString("00 00"); // maxlevel of usage, for dragon stones 
                    p.WriteHexString("00 00"); // unknown
                }
                if (i is PotionItem)
                {
                    PotionItem pot = i as PotionItem;
                    p.WriteByte(pot.RequiredClass);
                    p.WriteShort(pot.Amount);
                    p.WriteHexString("00 00 00 00");
                    p.WriteShort(pot.HealHp);
                    p.WriteShort(pot.HealMana);
                    p.WriteHexString("00 20 4E 00 00 00 00");
                }
                if (i is BookItem)
                {
                    BookItem book = i as BookItem;
                    p.WriteByte(book.RequiredClass);
                    p.WriteShort(book.Amount);
                    p.WriteShort(book.RequiredLevel);
                    p.WriteInt(book.SkillID);
                    p.WriteByte(book.SkillLevel);
                    p.WriteInt(book.SkillData);
                }
                if (i is BeadItem)
                {
                    BeadItem bead = i as BeadItem;
                    p.WriteByte(bead.RequiredClass);
                    p.WriteShort(bead.Amount);
                    p.WriteInt(bead.ToMapID);
                    p.WriteHexString("01 00 00 00 00 00 00");
                }
            }

            return p.GetWrittenBuffer(PacketIds.SendItemList);
        }

        public static byte[] SendAddItem(BaseItem i)
        {
            Packet p = new Packet(500);
            p.WriteByte(i.Bag);
            p.WriteByte(i.Slot);
            p.WriteInt(i.ItemID);
            p.WriteShort(i.ReferenceID);
            p.WriteByte(i.bType);
            p.WriteByte(i.bKind);
            p.WriteShort(i.VisualID);
            p.WriteInt(i.Price);
            p.WriteHexString("00 00");
            if (i is Equipment)
            {
                Equipment e = i as Equipment;
                p.WriteByte(e.RequiredClass);
                p.WriteShort(e.Amount);
                p.WriteShort(e.RequiredLevel);
                p.WriteShort(e.RequiredDexterity);
                p.WriteShort(e.RequiredStrength);
                p.WriteShort(e.RequiredStamina);
                p.WriteShort(e.RequiredEnergy);
                p.WriteByte(e.MaxImbueTries);
                p.WriteShort(e.Durability);
                p.WriteShort(e.MaxDurability);
                p.WriteShort(e.Damage);
                p.WriteShort(e.Defence);
                p.WriteShort(e.AttackRating);
                p.WriteShort(e.AttackSpeed);
                p.WriteShort(e.AttackRange);
                p.WriteShort(e.IncMaxLife);
                p.WriteShort(e.IncMaxMana);
                p.WriteShort(e.IncLifeRegen);
                p.WriteShort(e.IncManaRegen);
                p.WriteShort(e.Critical);
                p.WriteByte(e.Plus);
                p.WriteByte(e.Slvl);
                p.WriteHexString("00"); // unknown
                p.WriteByte(e.ImbueTries);
                p.WriteHexString("00"); // unknown
                p.WriteShort(e.DragonSuccessImbueTries);
                p.WriteByte(e.DiscountRepairFee);
                p.WriteShort(e.TotalDragonImbueTries);
                p.WriteShort(e.DragonDamage);
                p.WriteShort(e.DragonDefence);
                p.WriteShort(e.DragonAttackRating);
                p.WriteShort(e.DragonLife);
                p.WriteByte(e.MappedData);
                if (!(e is Ring) && !(e is Necklace) && !(e is Cape))
                    p.WriteByte(e.ForceSlot);
                p.WriteHexString("00 00"); // unknown
                if (e is Cape)
                {
                    Cape c = e as Cape;
                    p.WriteHexString("00 00"); // required guild position 
                    p.WriteHexString("00 00 00 00 00 00 00");
                    p.WriteHexString("01 00"); // decrease times of durability
                    p.WriteByte(c.PolishImbueTries); // polish imbue tries
                    p.WriteShort(c.MaxPolishImbueTries); // polish max tries
                    p.WriteShort(c.VigiStat1); // stat1
                    p.WriteShort(c.VigiStat2); // stat2
                    p.WriteShort(c.VigiStat3); // stat3
                    p.WriteShort(c.VigiStat4); // stat4
                    p.WriteHexString("00 00"); // stat5
                    p.WriteShort(c.VigiStatAdd1); // stat1 increase
                    p.WriteShort(c.VigiStatAdd2); // stat2 increase
                    p.WriteShort(c.VigiStatAdd3); // stat3 increase
                    p.WriteShort(c.VigiStatAdd4); // stat4 increase
                    p.WriteHexString("00 00"); // stat5 increase
                    p.WriteHexString("00 00"); // unknown
                }
                if (!(e is Ring) && !(e is Necklace) && !(e is Cape))
                {
                    p.WriteByte(e.RebirthHole);
                    p.WriteByte(e.RebirthHoleItem);
                    p.WriteShort(e.RebirthHoleStat);
                }
            }
            if (i is ImbueItem)
            {
                ImbueItem im = i as ImbueItem;
                p.WriteByte(im.RequiredClass);
                p.WriteShort(i.Amount);
                p.WriteHexString("00"); // imbuestat, eg dec level, increase value etc
                p.WriteShort(im.ImbueChance);
                p.WriteShort(im.IncreaseValue);
                p.WriteHexString("00 00"); // could be lowest lvl who knows
                p.WriteHexString("00 00"); // maxlevel of usage, for dragon stones 
                p.WriteHexString("00 00"); // unknown
            }
            if (i is PotionItem)
            {
                PotionItem pot = i as PotionItem;
                p.WriteByte(pot.RequiredClass);
                p.WriteShort(pot.Amount);
                p.WriteHexString("00 00 00 00");
                p.WriteShort(pot.HealHp);
                p.WriteShort(pot.HealMana);
                p.WriteHexString("00 20 4E 00 00 00 00");
            }
            if (i is BookItem)
            {
                BookItem book = i as BookItem;
                p.WriteByte(book.RequiredClass);
                p.WriteShort(book.Amount);
                p.WriteShort(book.RequiredLevel);
                p.WriteInt(book.SkillID);
                p.WriteByte(book.SkillLevel);
                p.WriteInt(book.SkillData);
            }
            if (i is BeadItem)
            {
                BeadItem bead = i as BeadItem;
                p.WriteByte(bead.RequiredClass);
                p.WriteShort(bead.Amount);
                p.WriteInt(bead.ToMapID);
                p.WriteHexString("01 00 00 00 00 00 00");
            }

            return p.GetWrittenBuffer(PacketIds.SendAddItem);
        }

        public static byte[] SendAcceptImbueItem(ImbueState i)
        {
            Packet p = new Packet(5);
            p.WriteByte((byte)i);
            return p.GetWrittenBuffer(PacketIds.SendAcceptImbueItem);
        }

        public static byte[] SendImbueMessage(ImbueError error, ImbueStat stat, int value, Equipment item)
        {
            Packet p = new Packet(5);
            p.WriteByte((byte)error); // Error message
            p.WriteByte((byte)stat); // Stat
            p.WriteInt(value);
            p.WriteHexString("00");
            p.WriteShort(item.ReferenceID);
            p.WriteByte(item.Plus);
            p.WriteByte(item.Slvl);
            return p.GetWrittenBuffer(PacketIds.SendImbueMessage);
        }

        public static byte[] SendImbuedItem(BaseItem i)
        {
            Packet p = new Packet(500);
            p.WriteByte(i.Bag);
            p.WriteByte(i.Slot);
            p.WriteInt(i.ItemID);
            p.WriteShort(i.ReferenceID);
            p.WriteByte(i.bType);
            p.WriteByte(i.bKind);
            p.WriteShort(i.VisualID);
            p.WriteInt(i.Price);
            p.WriteHexString("00 00");
            Equipment e = i as Equipment;
            p.WriteByte(e.RequiredClass);
            p.WriteShort(e.Amount);
            p.WriteShort(e.RequiredLevel);
            p.WriteShort(e.RequiredDexterity);
            p.WriteShort(e.RequiredStrength);
            p.WriteShort(e.RequiredStamina);
            p.WriteShort(e.RequiredEnergy);
            p.WriteByte(e.MaxImbueTries);
            p.WriteShort(e.Durability);
            p.WriteShort(e.MaxDurability);
            p.WriteShort(e.Damage);
            p.WriteShort(e.Defence);
            p.WriteShort(e.AttackRating);
            p.WriteShort(e.AttackSpeed);
            p.WriteShort(e.AttackRange);
            p.WriteShort(e.IncMaxLife);
            p.WriteShort(e.IncMaxMana);
            p.WriteShort(e.IncLifeRegen);
            p.WriteShort(e.IncManaRegen);
            p.WriteShort(e.Critical);
            p.WriteByte(e.Plus);
            p.WriteByte(e.Slvl);
            p.WriteHexString("00"); // unknown
            p.WriteByte(e.ImbueTries);
            p.WriteHexString("00"); // unknown
            p.WriteShort(e.DragonSuccessImbueTries);
            p.WriteByte(e.DiscountRepairFee);
            p.WriteShort(e.TotalDragonImbueTries);
            p.WriteShort(e.DragonDamage);
            p.WriteShort(e.DragonDefence);
            p.WriteShort(e.DragonAttackRating);
            p.WriteShort(e.DragonLife);
            p.WriteByte(e.MappedData);
            if (!(e is Ring) && !(e is Necklace) && !(e is Cape))
                p.WriteByte(e.ForceSlot);
            p.WriteHexString("00 00"); // unknown
            if (e is Cape)
            {
                Cape c = e as Cape;
                p.WriteHexString("00 00"); // required guild position 
                p.WriteHexString("00 00 00 00 00 00 00");
                p.WriteHexString("01 00"); // decrease times of durability
                p.WriteByte(c.PolishImbueTries); // polish imbue tries
                p.WriteShort(c.MaxPolishImbueTries); // polish max tries
                p.WriteShort(c.VigiStat1); // stat1
                p.WriteShort(c.VigiStat2); // stat2
                p.WriteShort(c.VigiStat3); // stat3
                p.WriteShort(c.VigiStat4); // stat4
                p.WriteHexString("00 00"); // stat5
                p.WriteShort(c.VigiStatAdd1); // stat1 increase
                p.WriteShort(c.VigiStatAdd2); // stat2 increase
                p.WriteShort(c.VigiStatAdd3); // stat3 increase
                p.WriteShort(c.VigiStatAdd4); // stat4 increase
                p.WriteHexString("00 00"); // stat5 increase
                p.WriteHexString("00 00"); // unknown
            }
            if (!(e is Ring) && !(e is Necklace) && !(e is Cape))
            {
                p.WriteByte(e.RebirthHole);
                p.WriteByte(e.RebirthHoleItem);
                p.WriteShort(e.RebirthHoleStat);
            }
            return p.GetWrittenBuffer(PacketIds.SendImbuedItem);
        }
        

        public static byte[] SendPickUpText(short referenceId, int amount)
        {
            Packet p = new Packet(200);
            p.WriteHexString("01 00"); // type maybe
            p.WriteShort(referenceId);
            p.WriteShort(amount);
            p.WriteHexString("00 00"); 
            return p.GetWrittenBuffer(PacketIds.SendPickUpText);
        }

        public static byte[] SendRemoveMapItem(int mapitemId)
        {
            Packet p = new Packet(200);
            p.WriteInt(mapitemId);
            return p.GetWrittenBuffer(PacketIds.SendRemoveMapItem);
        }

        public static byte[] SendMoveItem(MoveItemInfo m, byte error)
        {
            Packet p = new Packet(200);
            p.WriteByte(error);
            p.WriteByte(m.FromBag);
            p.WriteByte(m.FromSlot);
            p.WriteByte(m.ToBag);
            p.WriteByte(m.ToSlot);
            return p.GetWrittenBuffer(PacketIds.SendMoveItem);
        }

        public static byte[] SendBagIsFull()
        {
            Packet p = new Packet(200);
            p.WriteHexString("02 33 01 00 01");
            return p.GetWrittenBuffer();
        }

        public static byte[] SendRemoveItem(byte bag, byte slot)
        {
            Packet p = new Packet(200);
            p.WriteByte(bag);
            p.WriteByte(slot);
            p.WriteHexString("00");
            return p.GetWrittenBuffer(PacketIds.SendRemoveItem);
        }

        public static byte[] SendItemOnGround(MapItem e)
        {
            Packet p = new Packet(200);
            p.WriteHexString("00 01 00");
            p.WriteInt(e.MapID);
            p.WriteShort(e.MapX);
            p.WriteShort(e.MapY);
            p.WriteByte(e.MapZ);
            p.WriteInt(e.MapItemID); // DROP ITEM NEED NEW CLASS DROP ITEM PROBABLY
            p.WriteByte(e.bType);
            p.WriteShort(e.VisualID);
            p.WriteShort(e.ReferenceID);
            p.WriteInt(e.ItemID);
            p.WriteShort(e.Amount);
            p.WriteHexString("00 00 00 00 00 00 01 00 00");
            return p.GetWrittenBuffer(PacketIds.SendItemOnGround);
        }

        public static byte[] SendSomething()
        {
            Packet p = new Packet(200);
            p.WriteHexString("2E 31 7B 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 02 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 03 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");
            return p.GetWrittenBuffer();
        }

        public static byte[] SendFriendList()
        {
            Packet p = new Packet(200);
            p.WriteHexString("56 3B 01 00 00");
            return p.GetWrittenBuffer();
        }

        public static byte[] SendGuildList()
        {
            Packet p = new Packet(200);
            p.WriteHexString("35 3B 01 00 00");
            return p.GetWrittenBuffer();
        }

        public static byte[] SendSomething2()
        {
            Packet p = new Packet(200);
            p.WriteHexString("04 3C 0A 00 01 01 01 00 00 00 00 00 00 00");
            return p.GetWrittenBuffer();
        }

        public static byte[] SendShowMap()

        {
            Packet p = new Packet(200);
            p.WriteHexString("02 34 01 00 00");
            return p.GetWrittenBuffer();
        }

        public static byte[] SendSpawn10(Character Char)
        {
            Packet p = new Packet(200);
            p.WriteByte(0);
            p.WriteInt(Char.MapId);
            p.WriteInt(Char.CharacterId);
            p.WriteByte(Char.Class);
            p.WriteShort(Char.X);
            p.WriteShort(Char.Y);
            p.WriteHexString("00 00 00 00 13 00 00 00 00 00 00 00 00");
            return p.GetWrittenBuffer(PacketIds.SendSpawn);
        }

        public static byte[] SendSpawn11()
        {
            Packet p = new Packet(200);
            p.WriteHexString("2E 40 B6 00 00 0B 00 00 00 05 00 E4 4E F8 07 75 D2 06 2D 00 A8 03 00 ED 00 E4 4E 00 00 14 BE 06 8B 03 00 45 0C 00 00 45 0C 00 00 0B 80 FF E4 4E F5 07 75 B8 06 2D 00 8B 03 00 65 00 E4 4E 00 00 14 B5 06 97 03 00 45 0C 00 00 45 0C 00 00 0B 80 FF E4 4E F6 07 75 E2 06 2D 00 A0 03 00 50 00 E4 4E 00 00 18 E2 06 A0 03 00 45 0C 00 00 45 0C 00 00 0B 80 FF E4 4E F9 07 75 96 06 2D 00 8E 03 00 63 00 E4 4E 00 00 02 96 06 8E 03 00 45 0C 00 00 45 0C 00 00 0B 80 FF E4 4E F7 07 75 C8 06 2D 00 89 03 00 09 01 E4 4E 00 00 14 C3 06 5B 03 00 45 0C 00 00 45 0C 00 00 0B 80 FF");
            return p.GetWrittenBuffer();
        }

        public static byte[] SendSpawn12(Client[] Characters)
        {
            Packet p = new Packet(200);

            p.WriteByte(0);
            p.WriteByte(Characters.Length);

            for (int i = 0; i < Characters.Length; i++)
            {
                p.WriteInt(Characters[i].MyCharacter.CharacterId);
                p.WriteHexString("01 00"); // entity type, like 1 for players blabla
            }

            return p.GetWrittenBuffer(PacketIds.SendPlayerIdsSomething);
        }

        public static byte[] SendStartMoving(MovementInfo mo, byte movingspeed)
        {
            Packet p = new Packet(200);
            p.WriteByte(0);
            p.WriteInt(mo.CharacterID);
            p.WriteShort(mo.FromX);
            p.WriteShort(mo.FromY);
            p.WriteByte(mo.FromZ);
            p.WriteShort(mo.ToX);
            p.WriteShort(mo.ToY);
            p.WriteByte(mo.ToZ);
            p.WriteShort(mo.Rotation);
            p.WriteHexString("00");
            p.WriteByte(11 + movingspeed);
            p.WriteHexString("00");

            return p.GetWrittenBuffer(PacketIds.SendStartMoving);
        }

        public static byte[] SendKeepMoving(MovementInfo mo, byte movingspeed)
        {
            Packet p = new Packet(200);
            p.WriteByte(0);
            p.WriteInt(mo.CharacterID);
            p.WriteShort(mo.FromX);
            p.WriteShort(mo.FromY);
            p.WriteByte(mo.FromZ);
            p.WriteShort(mo.ToX);
            p.WriteShort(mo.ToY);
            p.WriteByte(mo.ToZ);
            p.WriteShort(mo.Rotation);
            p.WriteHexString("00");
            p.WriteByte(11 + movingspeed);
            p.WriteHexString("00 00");
            return p.GetWrittenBuffer(PacketIds.SendKeepMoving);
        }

        public static byte[] SendStopMoving(MovementInfo mo)
        {
            Packet p = new Packet(200);
            p.WriteByte(0);
            p.WriteInt(mo.CharacterID);
            p.WriteShort(mo.FromX);
            p.WriteShort(mo.FromY);
            p.WriteByte(mo.FromZ);
            p.WriteByte(0);
            return p.GetWrittenBuffer(PacketIds.SendStopMoving);
        }

        public static byte[] SpawnMonster()
        {
            Packet p = new Packet(200);
            p.WriteHexString("2C 40"); // id 
            p.WriteHexString("28 00");  // length
            p.WriteHexString("00"); // something 
            p.WriteHexString("39 05 00 00"); // monsterid
            p.WriteHexString("C0"); // monstertype, also visual
            p.WriteHexString("06 00 00 00"); // mapid 
            p.WriteHexString("9C 01"); // posx, // spawnx
            p.WriteHexString("C8 00"); // level
            p.WriteHexString("67 02"); // posy, // spawny 
            p.WriteHexString("00"); // unknown
            p.WriteHexString("00 00"); // direction 
            p.WriteHexString("56 54 00 00"); // monsterwrefid, for name 
            p.WriteHexString("14 1F 03 FF 03 00 96 00 00 00 96 00 00 00 07 80 FF");
            return p.GetWrittenBuffer();
        }

        public static byte[] SendMonsterMove(MovementInfo mi)
        {
            Packet p = new Packet(200);
            p.WriteHexString("0A 40 16 00 00 39 05 00 00 03");
            p.WriteShort(mi.FromX);
            p.WriteShort(mi.FromY);
            p.WriteByte(mi.FromZ);
            p.WriteShort(mi.ToX);
            p.WriteShort(mi.ToY);
            p.WriteByte(mi.ToZ);
            p.WriteShort(mi.Rotation);
            p.WriteHexString("15 03 00 00");
            return p.GetWrittenBuffer();
        }

        public static byte[] SendMonsterStop(MovementInfo mi)
        {
            Packet p = new Packet(200);
            p.WriteHexString("0C 40 0D 00 00 39 05 00 00 03");
            p.WriteShort(mi.FromX);
            p.WriteShort(mi.FromY);
            p.WriteHexString("00 15 03");
            return p.GetWrittenBuffer();
        }

        public static byte[] SendSpawnNpc(List<Npc> npcs, int mapid)
        {
            Packet p = new Packet(200);
            p.WriteHexString("00"); // unknown
            p.WriteInt(mapid);
            p.WriteShort(npcs.Count);
            foreach (Npc n in npcs)
            {
                p.WriteInt(n.NpcID);
                p.WriteByte(n.NpcType);
                p.WriteByte(n.NpcType);
                p.WriteHexString("01"); // unknown
                p.WriteShort(n.X);
                p.WriteShort(n.Y);
                p.WriteHexString("00"); // unknown
                p.WriteShort(n.Direction);
                p.WriteHexString("00 00 00 00 00 00 00 00 00"); // unknown
            }
            return p.GetWrittenBuffer(PacketIds.SendNpcSpawn);
        }

        public static byte[] SendNpcItemList(Npc n, byte bag)
        {
            Packet p = new Packet(200);
            p.WriteHexString("00"); // unknown
            p.WriteInt(n.NpcID);
            p.WriteByte(bag); // bag
            p.WriteShort(n.Bags[bag].ItemCount);
            foreach (BaseItem i in n.Bags[bag].Items)
            {
                p.WriteShort(i.ReferenceID);
                p.WriteByte(i.bType);
                p.WriteByte(i.bKind);
                p.WriteShort(i.VisualID);
                p.WriteInt(i.Price);
                p.WriteHexString("00 00"); // some quest item, only guild master thingy 
                p.WriteByte(i.RequiredClass);
                p.WriteShort(i.Amount);
                p.WriteByte(i.Slot);
                p.WriteHexString("00");

                if (i is Equipment)
                {
                    Equipment e = i as Equipment;
                    p.WriteShort(e.RequiredLevel);
                    p.WriteShort(e.RequiredDexterity);
                    p.WriteShort(e.RequiredStrength);
                    p.WriteShort(e.RequiredStamina);
                    p.WriteShort(e.RequiredEnergy);
                    p.WriteByte(e.MaxImbueTries);
                    p.WriteHexString("00"); // something
                    p.WriteShort(e.Durability);
                    p.WriteShort(e.Durability);
                    p.WriteShort(e.Damage);
                    p.WriteShort(e.Defence);
                    p.WriteShort(e.AttackRating);
                    if (e is Cape)
                    {
                        Cape c = e as Cape;
                        p.WriteHexString("00 00"); // faster run
                        p.WriteHexString("01 00"); // decrease times of durability
                        p.WriteByte(c.PolishImbueTries); // polish imbue tries
                        p.WriteHexString("00"); // something
                        p.WriteShort(c.MaxPolishImbueTries); // polish max tries
                        p.WriteHexString("00 00 00 00 00 00 00 00 00 00 00 00 00 00"); // something
                    }
                    else
                    {
                        p.WriteShort(e.AttackSpeed);
                        p.WriteShort(e.AttackRange);
                        p.WriteShort(e.IncMaxLife);
                        p.WriteShort(e.IncMaxMana);
                        p.WriteShort(e.IncLifeRegen);
                        p.WriteShort(e.IncManaRegen);
                        p.WriteShort(e.Critical);
                        p.WriteHexString("00 00 00 00 00 00 00 00");
                    }
                }
                if (i is PotionItem)
                {
                    PotionItem pot = i as PotionItem;
                    p.WriteShort(pot.HealHp);
                    p.WriteShort(pot.HealMana);
                }
            }

            return p.GetWrittenBuffer(PacketIds.SendNpcItemList);
        }

        public static byte[] SendBuyItemState(BuyItemState b)
        {
            Packet p = new Packet(20);
            p.WriteByte((byte)b);

            return p.GetWrittenBuffer(PacketIds.SendBuyItemState);
        }

        public static byte[] SendMoneyLeft(int money)
        {
            Packet p = new Packet(20);
            p.WriteInt(money);
            p.WriteHexString("00"); // something

            return p.GetWrittenBuffer(PacketIds.SendMoneyLeft);
        }

        public static byte[] SendSpawnPlayers(Character ch)
        {
            Packet p = new Packet(200);
            p.WriteHexString("00");
            p.WriteInt(ch.MapId);
            p.WriteHexString("01 00");
            p.WriteInt(ch.CharacterId);
            p.WriteShort(ch.X);
            p.WriteShort(ch.Y);
            p.WriteHexString("76");
            p.WriteShort(ch.X);
            p.WriteShort(ch.Y);
            p.WriteHexString("76");
            p.WriteHexString("16 B2 00 00");
            p.WriteByte(ch.Class);
            p.WriteString(ch.Name);
            p.WriteInt(ch.Fame);
            p.WriteHexString("00"); // bool selling
            p.WriteString(""); // shopname
            p.WriteString(""); // shopdesc
            p.WriteHexString("00"); // purple name
            p.WriteHexString("00"); // fe
            p.WriteHexString("00"); // fe ring
            p.WriteHexString("00"); // maybe has guild thingy
            //if Character has guild
            //p.WriteHexString("00 00 00 00"); // guild id
            //p.WriteString(""); // guild name
            //p.WriteHexString("00 00 00 00"); // maybe guild position
            //p.WriteHexString("00 00 00 00"); // maybe guild position
            //p.WriteHexString("00 00 00 00 00 00 00 00");
            //p.WriteHexString("00"); // something
            p.WriteHexString("00 00 00 00 00 00 00 00 00"); // else write this
            foreach (Equipment e in ch.GetVisuals())
            {
                p.WriteShort(e.VisualID);
                p.WriteByte(e.Plus);
                p.WriteByte(e.Slvl);
                p.WriteByte(e.RequiredClass);
            }
            p.WriteByte(ch.Rebirth); // rebirth
            p.WriteHexString("00 00 00 00 00");
            return p.GetWrittenBuffer(PacketIds.SendSpawnPlayers);
        }

        public static byte[] SendStats(Character ch)
        {
            Packet p = new Packet(200);
            // p.WriteHexString("02 3A 52 00 01 00 02 00 03 00 04 00 05 00 06 00 00 00 00 00 07 00 00 00 08 00 00 00 09 00 00 00 00 0B 05 00 00 00 06 00 00 00 07 00 08 00 09 00 01 00 02 00 02 00 0C 07 00 02 03 00 00 00 00 00 80 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");
            p.WriteShort(ch.Level);
            p.WriteShort(ch.Strength);
            p.WriteShort(ch.Stamina);
            p.WriteShort(ch.Dexterity);
            p.WriteShort(ch.Energy);
            p.WriteShort(ch.StatPoint); // stat points

            //PLOX FUNCTION OF STATS HERE
            ch.CalculateTotalStats();

            p.WriteHexString("00 00 00 00"); // unknown
            p.WriteInt(ch.TotalDamage); // total damage
            p.WriteInt(ch.TotalDefence); // total defence
            p.WriteInt(ch.TotalAttackRating); // total attack rating


            p.WriteHexString("00 0B"); // unknown
            p.WriteInt(ch.CurrentHp);
            p.WriteInt(ch.MaxHp);
            p.WriteShort(ch.CurrentMana);
            p.WriteShort(ch.MaxMana);

            p.WriteShort(ch.Critical); // critical hit

            p.WriteShort(ch.StatDamage); // damage from stats
            p.WriteShort(ch.StatDefence); // defence from stats
            p.WriteShort(ch.StatAttackRating); // attack rating from stats

            p.WriteHexString("0C 07 01"); // unknown
            p.WriteByte(ch.MovingSpeed); // movement speed
            p.WriteShort(ch.TrainingPoint); // trainings
            p.WriteHexString("00 00 00 00"); // unknown
            p.WriteInt(ch.Fame);
            p.WriteHexString("00"); // gives you green name, luls
            p.WriteByte(ch.Rebirth); // rebirths
            p.WriteHexString("00 00 00 00 00 00 00 00 00 00 00 00"); // unknown
            p.WriteInt(ch.RepulationPoint); // repulation points
            return p.GetWrittenBuffer(PacketIds.SendStats);
        }

        public static byte[] SendAddStat(int error, AddStatInfo asi, int pointsleft, int statamount)
        {
            Packet p = new Packet(200);
            p.WriteHexString("1E 35 07 00");
            p.WriteByte(error);
            p.WriteByte(asi.Stat);
            p.WriteByte(asi.Amount);
            p.WriteShort(pointsleft);
            p.WriteShort(statamount);
            return p.GetWrittenBuffer();
        }

        public static byte[] SendSpawnPlayerSomething2()
        {
            Packet p = new Packet(200);
            p.WriteHexString("36 31 01 00 00");
            return p.GetWrittenBuffer();
        }

        public static byte[] SendPlayerID(int id)
        {
            Packet p = new Packet(200);
            p.WriteHexString("20 3A 05 00");
            p.WriteInt(id);
            p.WriteByte(0);
            return p.GetWrittenBuffer();
        }

        public static byte[] SendChatMessage(Character ch, ChatMessageInfo cmi)
        {
            Packet p = new Packet(200);
            p.WriteInt(ch.CharacterId);
            p.WriteByte((byte)cmi.Type); // chat type probably
            p.WriteString(cmi.Message);
            p.WriteString(ch.Name);
            if (cmi.Type == ChatType.Whisper)
            {
                p.WriteInt(cmi.TargetID);
                p.WriteString(cmi.TargetName);
            }
            return p.GetWrittenBuffer(PacketIds.SendChatMessage);
        }

        public static byte[] SendCommand(Character ch, CommandInfo ci)
        {
            Packet p = new Packet(200);
            p.WriteInt(ch.CharacterId);
            p.WriteByte(ci.Type);
            p.WriteByte(ci.Action);
            p.WriteByte(ci.Something);
            p.WriteByte(ci.Something2);
            return p.GetWrittenBuffer(PacketIds.SendCommand);
        }

        #endregion



        #region Channel Recvs

        public static ChannelLogin RecvChannelLogin(Packet p)
        {
            p.Skip(2);
            int characterID = p.ReadInt();
            int accountID = p.ReadShort();
            int channelID = p.ReadShort();
            int worldID = p.ReadByte();

            ChannelLogin u = new ChannelLogin()
            {
                CharacterID = characterID,
                AccountID = accountID,
                ChannelID = channelID,
                WorldID = worldID
            };

            return u;
        }

        public static ChannelChangeInfo RecvChannelChange(Packet p)
        {
            p.Skip(2);
            byte something1 = p.ReadByte();
            byte something2 = p.ReadByte();

            ChannelChangeInfo cci = new ChannelChangeInfo
            {
                Something = something1,
                Something2 = something2
            };

            return cci;
        }

        public static MapRequestInfo RecvMapRequest(Packet p)
        {
            p.Skip(2);
            int mapID = p.ReadInt();

            MapRequestInfo mr = new MapRequestInfo
            {
                MapID = mapID
            };

            return mr;
        }

        public static int RecvSkillListRequest(Packet p)
        {
            p.Skip(2);
            int skill = p.ReadByte();
            return skill;
        }

        public static int RecvInventoryRequest(Packet p)
        {
            p.Skip(2);
            int inventory = p.ReadByte();
            return inventory;
        }

        public static StatRequestInfo RecvStatRequest(Packet p)
        {
            p.Skip(2);
            int charid = p.ReadInt();
            int mapid = p.ReadInt();

            StatRequestInfo sr = new StatRequestInfo
            {
                CharacterID = charid,
                MapID = mapid
            };

            return sr;
        }

        public static SpawnRequestInfo RecvSpawnRequest(Packet p)
        {
            p.Skip(2);
            int charid = p.ReadInt();
            int mapid = p.ReadInt();

            SpawnRequestInfo sr = new SpawnRequestInfo
            {
                CharacterID = charid,
                MapID = mapid
            };

            return sr;
        }

        public static AddStatInfo RecvAddStatRequest(Packet p)
        {
            p.Skip(2);
            byte stat = p.ReadByte();
            byte amount = p.ReadByte();

            AddStatInfo asi = new AddStatInfo
            {
                Stat = stat,
                Amount = amount
            };

            return asi;
        }

        public static SpawnRequestInfo RecvSpawnOtherPlayerRequest(Packet p)
        {
            p.Skip(2);
            int charid = p.ReadInt();
            int mapid = p.ReadInt();

            SpawnRequestInfo sr = new SpawnRequestInfo
            {
                CharacterID = charid,
                MapID = mapid
            };

            return sr;
        }

        public static SpawnRequestInfo RecvPlayerIDRequest(Packet p)
        {
            p.Skip(2);
            int charid = p.ReadInt();

            SpawnRequestInfo sr = new SpawnRequestInfo
            {
                CharacterID = charid
            };

            return sr;
        }

        public static SpawnRequestInfo RecvSpawnPlayer(Packet p)
        {
            p.Skip(2);
            int charid = p.ReadInt();
            int mapid = p.ReadInt();

            SpawnRequestInfo sr = new SpawnRequestInfo
            {
                CharacterID = charid,
                MapID = mapid
            };

            return sr;
        }

        public static MovementInfo RecvStartMoving(Packet p)
        {
            p.Skip(2);
            int charid = p.ReadInt();

            short from_x = p.ReadShort();
            short from_y = p.ReadShort();
            byte from_z = p.ReadByte();

            short to_x = p.ReadShort();
            short to_y = p.ReadShort();
            byte to_z = p.ReadByte();
            short direction = p.ReadShort();

            MovementInfo mi = new MovementInfo
            {
                PacketID = 1,
                CharacterID = charid,
                FromX = from_x,
                FromY = from_y,
                FromZ = from_z,
                ToX = to_x,
                ToY = to_y,
                ToZ = to_z,
                Rotation = direction
            };

            return mi;
        }

        public static MovementInfo RecvKeepMoving(Packet p)
        {
            p.Skip(2);
            int charid = p.ReadInt();

            short from_x = p.ReadShort();
            short from_y = p.ReadShort();
            byte from_z = p.ReadByte();

            short to_x = p.ReadShort();
            short to_y = p.ReadShort();
            byte to_z = p.ReadByte();
            short direction = p.ReadShort();

            MovementInfo mi = new MovementInfo
            {
                PacketID = 2,
                CharacterID = charid,
                FromX = from_x,
                FromY = from_y,
                FromZ = from_z,
                ToX = to_x,
                ToY = to_y,
                ToZ = to_z,
                Rotation = direction
            };

            return mi;
        }

        public static MovementInfo RecvStopMoving(Packet p)
        {
            p.Skip(2);
            int charid = p.ReadInt();

            short from_x = p.ReadShort();
            short from_y = p.ReadShort();
            byte from_z = p.ReadByte();

            MovementInfo mi = new MovementInfo
            {
                PacketID = 3,
                CharacterID = charid,
                FromX = from_x,
                FromY = from_y,
                FromZ = from_z,
            };

            return mi;
        }

        public static int RecvWalkToPortal(Packet p)
        {
            p.Skip(2);
            int mapid = p.ReadInt();
            return mapid;
        }

        public static ChatMessageInfo RecvChatMessage(Packet p)
        {
            p.Skip(2);
            byte type = p.ReadByte();
            int targetId = p.ReadInt();
            string message = p.ReadString();
            string targetName = p.ReadString(); // if you dont have id

            ChatMessageInfo cmi = new ChatMessageInfo
            {
                Type = (ChatType)type,
                TargetID = targetId,
                Message = message,
                TargetName = targetName
            };

            return cmi;
        }

        public static CommandInfo RecvCommand(Packet p)
        {
            p.Skip(2);
            byte type = p.ReadByte();
            byte action = p.ReadByte();
            byte something = p.ReadByte();
            byte something2 = p.ReadByte();

            CommandInfo c = new CommandInfo
            {
                Type = type,
                Action = action,
                Something = something,
                Something2 = something2
            };

            return c;
        }

        public static MoveItemInfo RecvMoveItem(Packet p)
        {
            p.Skip(2);
            byte frombag = p.ReadByte();
            byte fromslot = p.ReadByte();
            int itemid = p.ReadInt();
            byte tobag = p.ReadByte();
            byte toslot = p.ReadByte();
            int itemunder = p.ReadInt();

            MoveItemInfo mii = new MoveItemInfo
            {
                FromBag = frombag,
                FromSlot = fromslot,
                ItemID = itemid,
                ToBag = tobag,
                ToSlot = toslot,
                ItemIDUnder = itemunder
            };

            return mii;
        }

        public static DropItemInfo RecvDropItem(Packet p)
        {
            p.Skip(2);
            byte frombag = p.ReadByte();
            byte fromslot = p.ReadByte();
            int itemid = p.ReadInt();
            short tox = p.ReadShort();
            short toy = p.ReadShort();
            byte toz = p.ReadByte();
            int something = p.ReadInt();

            DropItemInfo drop = new DropItemInfo
            {
                FromBag = frombag,
                FromSlot = fromslot,
                ItemID = itemid,
                ToX = tox,
                ToY = toy,
                ToZ = toz,
                Something = something
            };

            return drop;
        }

        public static PickItemInfo RecvPickItem(Packet p)
        {
            p.Skip(2);
            int mapid = p.ReadInt();
            int itemid = p.ReadInt();
            short fromx = p.ReadShort();
            short fromy = p.ReadShort();
            byte fromz = p.ReadByte();
            int mapitemid = p.ReadInt();
            short amount = p.ReadShort();

            PickItemInfo pi = new PickItemInfo
            {
                MapID = mapid,
                ItemID = itemid,
                FromX = fromx,
                FromY = fromy,
                FromZ = fromz,
                MapItemID = mapitemid,
                Amount = amount
            };

            return pi;
        }

        public static ImbueItemInfo RecvImbueItem(Packet p)
        {
            p.Skip(2);
            int itemid = p.ReadInt();
            byte bag = p.ReadByte();
            byte slot = p.ReadByte();

            ImbueItemInfo i = new ImbueItemInfo
            {
                ItemID = itemid,
                Bag = bag,
                Slot = slot
            };

            return i;
        }
        public static AcceptImbueItem RecvAcceptImbueItem(Packet p)
        {
            p.Skip(2);
            int NpcID = p.ReadInt();

            int toImbueID = p.ReadInt();
            byte toImbueBag = p.ReadByte();
            byte toImbueSlot = p.ReadByte();

            int imbueItem1ID = p.ReadInt();
            byte imbueItem1Bag = p.ReadByte();
            byte imbueItem1Slot = p.ReadByte();

            int imbueItem2ID = p.ReadInt();
            byte imbueItem2Bag = p.ReadByte();
            byte imbueItem2Slot = p.ReadByte();

            int imbueItem3ID = p.ReadInt();
            byte imbueItem3Bag = p.ReadByte();
            byte imbueItem3Slot = p.ReadByte();

            AcceptImbueItem i = new AcceptImbueItem
            {
                NpcID = NpcID,
                ToImbueItemID = toImbueID,
                ToImbueItemBag = toImbueBag,
                ToImbueItemSlot = toImbueSlot,
                ImbueItem1ID = imbueItem1ID,
                ImbueItem1Bag = imbueItem1Bag,
                ImbueItem1Slot = imbueItem1Slot,
                ImbueItem2ID = imbueItem2ID,
                ImbueItem2Bag = imbueItem2Bag,
                ImbueItem2Slot = imbueItem2Slot,
                ImbueItem3ID = imbueItem3ID,
                ImbueItem3Bag = imbueItem3Bag,
                ImbueItem3Slot = imbueItem3Slot
            };

            return i;
        }

        public static NpcTradeInfo RecvNpcTrade(Packet p)
        {
            p.Skip(2);
            int map = p.ReadInt();
            int npc = p.ReadInt();
            byte bag = p.ReadByte();

            NpcTradeInfo nti = new NpcTradeInfo
            {
                MapID = map,
                NpcID = npc,
                Bag = bag
            };

            return nti;
        }

        public static BuyItemInfo RecvBuyItem(Packet p)
        {
            p.Skip(2);
            int npcid = p.ReadInt();
            short wref = p.ReadShort();
            p.Skip(2);
            short amount = p.ReadShort();
            p.Skip(2);
            byte bag = p.ReadByte();

            BuyItemInfo i = new BuyItemInfo
            {
                NpcID = npcid,
                ReferenceID = wref,
                Amount = amount,
                Bag = bag
            };

            return i;
        }

        #endregion

        #endregion
    }
}