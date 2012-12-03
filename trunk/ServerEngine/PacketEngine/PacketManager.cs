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

        public static byte[] SendRemoveCharacter(Character ch, RemoveCharacterEffect effect)
        {
            Packet p = new Packet(200);
            p.WriteHexString("00");
            p.WriteInt(ch.CharacterId);
            p.WriteHexString("01");
            p.WriteInt(ch.MapId);
            p.WriteByte((byte)effect);
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

        public static byte[] SendPlayerDie1(Character ch)
        {
            Packet p = new Packet(200);
            p.WriteHexString("01");
            p.WriteInt(ch.CharacterId);
            return p.GetWrittenBuffer(PacketIds.SendPlayerDie1);
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
            p.WriteLong(ch.NegativeExp);
            p.WriteLong(ch.ExpToLevel + ch.NegativeExp);
            p.WriteLong(ch.ExpToTraining + ch.NegativeExp);
            //p.WriteHexString("00 00 00 00 00 00 00 00"); // xp to next training
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

        public static byte[] SendTradeAcceptFinished(byte error)
        {
            Packet p = new Packet(20);
            p.WriteByte(error); // probably stuff like no room in bag etc..
            return p.GetWrittenBuffer(PacketIds.SendTradeAcceptFinished);
        }

        public static byte[] SendTradeAddItem(TradeAddItemInfo info, BaseItem i)
        {
            Packet p = new Packet(20);
            p.WriteHexString("00");
            p.WriteInt(i.OwnerID);
            p.WriteByte(info.FromBag);
            p.WriteByte(info.FromSlot);
            p.WriteByte(info.ToBag);
            p.WriteByte(i.TradeSlot);
            p.WriteInt(info.ItemID);
            p.WriteShort(info.Amount);
            p.WriteHexString("00 00");

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
                p.WriteShort(e.MaxImbueTries);
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
                p.WriteShort(e.ImbueTries);
                p.WriteHexString("00"); // unknown
                p.WriteShort(e.DragonSuccessImbueTries);
                p.WriteByte(e.DiscountRepairFee);
                p.WriteShort(e.TotalDragonImbueTries);
                p.WriteShort(e.DragonDamage);
                p.WriteShort(e.DragonDefence);
                p.WriteShort(e.DragonAttackRating);
                p.WriteShort(e.DragonLife);
                if (e is Mirror)
                {
                    Mirror m = e as Mirror;
                    p.WriteInt(m.PetID);
                    p.WriteShort(m.LifeAbsorb);
                    p.WriteShort(m.DamageAbsorb);
                    p.WriteShort(m.DefenseAbsorb);
                    p.WriteShort(m.AttackRatingAbsorb);
                    p.WriteHexString("00 00 00 00 00 00 00 00 00 00"); // pet skill stuff yo
                }
                else
                {
                    p.WriteByte(e.MappedData);
                    if (!(e is Ring) && !(e is Necklace) && !(e is Cape))
                    {
                        p.WriteByte(e.ForceSlot);
                        p.WriteHexString("00 00"); // otherslots
                    }
                    else
                        p.WriteHexString("00 00");
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
                        p.WriteShort(e.RebirthHole);
                        p.WriteShort(e.RebirthHoleStat);
                    }
                }
            }
            if (i is ImbueItem)
            {
                ImbueItem im = i as ImbueItem;
                p.WriteByte(im.RequiredClass);
                p.WriteShort(i.Amount);
                if (i is RbHoleItem)
                {
                    p.WriteByte(0);
                    p.WriteInt(im.ImbueChance);
                    p.WriteInt(im.IncreaseValue);
                    p.WriteHexString("00 00");
                }
                else
                {
                    p.WriteByte(im.ImbueData);
                    p.WriteShort(im.ImbueChance);
                    p.WriteShort(im.IncreaseValue);
                    p.WriteHexString("00 00"); // could be lowest lvl who knows
                    p.WriteHexString("00 00"); // maxlevel of usage, for dragon stones 
                    p.WriteHexString("00 00"); // unknown
                }
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
            if (i is StoreTag)
            {
                StoreTag tag = i as StoreTag;
                p.WriteByte(tag.RequiredClass);
                p.WriteShort(tag.Amount);
                p.WriteHexString("00 00");
                p.WriteShort(tag.TimeLeft);
                p.WriteShort(tag.TimeMax);
                p.WriteHexString("00 00 00 00 00 00");
            }
            if (i is PetItem)
            {
                PetItem pet = i as PetItem;
                p.WriteByte(pet.RequiredClass);
                p.WriteShort(pet.Amount);
                p.WriteByte(pet.TameChance);
                p.WriteHexString("00 00 00 00"); // unknown
                p.WriteByte(pet.DecreaseWildness);
                p.WriteInt(pet.HealLife);
            }
            if (i is RebirthPill)
            {
                RebirthPill pill = i as RebirthPill;
                p.WriteHexString("00");
                p.WriteShort(pill.Amount);
                p.WriteShort(pill.RequiredRebirth);
                p.WriteHexString("00");
                p.WriteShort(pill.RequiredLevel);
                p.WriteHexString("00 00");
            }

            return p.GetWrittenBuffer(PacketIds.SendTradeAddItem);
        }

        public static byte[] SendTradeAddMoney(int characterid, int amount)
        {
            Packet p = new Packet(20);
            p.WriteHexString("00");
            p.WriteInt(characterid);
            p.WriteInt(amount);
            return p.GetWrittenBuffer(PacketIds.SendTradeAddMoney);
        }

        public static byte[] SendTradePlayerAccept(TradePlayerError error, int playerId)
        {
            Packet p = new Packet(20);
            p.WriteByte((byte)error);
            p.WriteInt(playerId);
            return p.GetWrittenBuffer(PacketIds.SendTradePlayerAccept);
        }

        public static byte[] SendTradePlayerError(byte error, int characterId)
        {
            Packet p = new Packet(20);
            p.WriteByte((byte)error);
            p.WriteInt(characterId);
            return p.GetWrittenBuffer(PacketIds.SendTradePlayerError);
        }

        public static byte[] SendTradePlayer(TradePlayerError error, int player1, int player2)
        {
            Packet p = new Packet(20);
            p.WriteByte((byte)error);
            p.WriteInt(player1);
            p.WriteInt(player2);
            return p.GetWrittenBuffer(PacketIds.SendTradePlayer);
        }

        public static byte[] SendShoftSkillList(List<SoftSkill> skills)
        {
            Packet p = new Packet(200);
            p.WriteByte(skills.Count);
            foreach (SoftSkill s in skills)
            {
                p.WriteInt(s.SkillID);
                p.WriteHexString("00");
                p.WriteByte(s.SkillLevel);
                p.WriteHexString("00");
            }
            return p.GetWrittenBuffer(PacketIds.SendSoftSkillList);
        }

        public static byte[] SendHardSkillList(List<HardSkill> skills)
        {
            Packet p = new Packet(200);
            p.WriteByte(skills.Count);
            foreach (HardSkill s in skills)
            {
                p.WriteInt(s.SkillID);
                p.WriteHexString("00");
                p.WriteByte(s.SkillLevel);
                p.WriteHexString("00");
            }
            return p.GetWrittenBuffer(PacketIds.SendHardSkillList);
        }

        public static byte[] SendLearnSkill(SkillLearnError error, short SkillID, byte TeachLevel, short TrainingCost, short TrainingsLeft)
        {
            Packet p = new Packet(200);
            p.WriteByte((byte)error);
            p.WriteShort(SkillID);
            p.WriteHexString("00 00"); // something
            p.WriteByte(TeachLevel);
            p.WriteShort(TrainingCost);
            p.WriteShort(TrainingsLeft);

            return p.GetWrittenBuffer(PacketIds.SendLearnSkill);
        }

        public static byte[] SendSkillSomething()
        {
            Packet p = new Packet(200);
            p.WriteHexString("01 34 05 00 09 00 0A 0A 08");
            return p.GetWrittenBuffer();
        }

        public static byte[] SendSkillError(SkillLearnError error)
        {
            Packet p = new Packet(200);
            p.WriteHexString("1E 33 13 00");
            p.WriteByte((byte)error);
            p.WriteHexString("0E 00 84 BD E0 C2 A9 AC 20 00 08 CD 1C C1 00 00 1E 27");
            return p.GetWrittenBuffer();
        }

        public static byte[] SendAddItemToWarehouse(byte cost, BaseItem i)
        {
            Packet p = new Packet(500);
            p.WriteByte(cost); // its for warehouse bag thing
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
                p.WriteShort(e.MaxImbueTries);
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
                p.WriteShort(e.ImbueTries);
                p.WriteHexString("00"); // unknown
                p.WriteShort(e.DragonSuccessImbueTries);
                p.WriteByte(e.DiscountRepairFee);
                p.WriteShort(e.TotalDragonImbueTries);
                p.WriteShort(e.DragonDamage);
                p.WriteShort(e.DragonDefence);
                p.WriteShort(e.DragonAttackRating);
                p.WriteShort(e.DragonLife);
                if (e is Mirror)
                {
                    Mirror m = e as Mirror;
                    p.WriteInt(m.PetID);
                    p.WriteShort(m.LifeAbsorb);
                    p.WriteShort(m.DamageAbsorb);
                    p.WriteShort(m.DefenseAbsorb);
                    p.WriteShort(m.AttackRatingAbsorb);
                    p.WriteHexString("00 00 00 00 00 00 00 00 00 00"); // pet skill stuff yo
                }
                else
                {
                    p.WriteByte(e.MappedData);
                    if (!(e is Ring) && !(e is Necklace) && !(e is Cape))
                    {
                        p.WriteByte(e.ForceSlot);
                        p.WriteHexString("00 00"); // otherslots
                    }
                    else
                        p.WriteHexString("00 00");
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
                        //p.WriteHexString("00 00"); // unknown
                    }
                    if (!(e is Ring) && !(e is Necklace) && !(e is Cape))
                    {
                        p.WriteShort(e.RebirthHole);
                        p.WriteShort(e.RebirthHoleStat);
                    }
                }
            }
            if (i is ImbueItem)
            {
                ImbueItem im = i as ImbueItem;
                p.WriteByte(im.RequiredClass);
                p.WriteShort(i.Amount);
                if (i is RbHoleItem)
                {
                    p.WriteByte(0);
                    p.WriteInt(im.ImbueChance);
                    p.WriteInt(im.IncreaseValue);
                }
                else
                {
                    p.WriteByte(im.ImbueData);
                    p.WriteShort(im.ImbueChance);
                    p.WriteShort(im.IncreaseValue);
                    p.WriteHexString("00 00"); // could be lowest lvl who knows
                    p.WriteHexString("00 00"); // maxlevel of usage, for dragon stones 
                }
            }
            if (i is PotionItem)
            {
                PotionItem pot = i as PotionItem;
                p.WriteByte(pot.RequiredClass);
                p.WriteShort(pot.Amount);
                p.WriteHexString("00 00 00 00");
                p.WriteShort(pot.HealHp);
                p.WriteShort(pot.HealMana);
                p.WriteHexString("00 20 4E 00 00");
            }
            if (i is BookItem)
            {
                BookItem book = i as BookItem;
                p.WriteByte(book.RequiredClass);
                p.WriteShort(book.Amount);
                p.WriteShort(book.RequiredLevel);
                p.WriteInt(book.SkillID);
                p.WriteByte(book.SkillLevel);
                p.WriteShort(book.SkillData);
            }
            if (i is BeadItem)
            {
                BeadItem bead = i as BeadItem;
                p.WriteByte(bead.RequiredClass);
                p.WriteShort(bead.Amount);
                p.WriteInt(bead.ToMapID);
                p.WriteHexString("01 00 00 00 00");
            }
            if (i is StoreTag)
            {
                StoreTag tag = i as StoreTag;
                p.WriteByte(tag.RequiredClass);
                p.WriteShort(tag.Amount);
                p.WriteHexString("00 00");
                p.WriteShort(tag.TimeLeft);
                p.WriteShort(tag.TimeMax);
                p.WriteHexString("00 00 00 00");
            }
            if (i is PetItem)
            {
                PetItem pet = i as PetItem;
                p.WriteByte(pet.RequiredClass);
                p.WriteShort(pet.Amount);
                p.WriteByte(pet.TameChance);
                p.WriteHexString("00 00"); // unknown
                p.WriteByte(pet.DecreaseWildness);
                p.WriteInt(pet.HealLife);
            }
            if (i is RebirthPill)
            {
                RebirthPill pill = i as RebirthPill;
                p.WriteHexString("00");
                p.WriteShort(pill.Amount);
                p.WriteShort(pill.RequiredRebirth);
                p.WriteHexString("00");
                p.WriteShort(pill.RequiredLevel);
            }

            return p.GetWrittenBuffer(PacketIds.SendAddItemToWarehouse);
        }

        public static byte[] SendWarehouseRemoveItem(byte slot, int itemid)
        {
            Packet p = new Packet(20);
            p.WriteByte(slot);
            p.WriteInt(itemid);
            p.WriteHexString("01 00 00 00"); // maybe amount who knows
            return p.GetWrittenBuffer(PacketIds.SendRemoveWarehouseItem);
        }

        public static byte[] SendWarehouseMoveItemError(WarehouseError error)
        {
            Packet p = new Packet(20);
            p.WriteByte((byte)error);
            p.WriteHexString("00 00");
            return p.GetWrittenBuffer(PacketIds.SendWarehouseMoveItemError);
        }

        public static byte[] SendWarehouseError(WarehouseError error)
        {
            Packet p = new Packet(20);
            p.WriteByte((byte)error);
            return p.GetWrittenBuffer(PacketIds.SendWarehouseError);
        }

        public static byte[] SendWarehouseItems(List<BaseItem> items, int bag)
        {
            Packet p = new Packet(500);
            // p.WriteHexString("9E 38 58 00 01 00 00 00 01 01 00 00 00 00 40 72 F0 00 B8 4E 01 02 B9 04 DD E3 00 00 01 00 02 01 00 28 00 7C 00 08 00 00 00 00 00 C8 F0 0A F0 0A BE 00 00 00 00 00 04 00 03 00 00 00 00 00 00 00 00 00 00 00 04 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");
            p.WriteHexString("01 00 00 00");
            p.WriteHexString("01");
            p.WriteByte(items.Count);
            p.WriteHexString("00 00 00");
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
                    p.WriteShort(e.MaxImbueTries);
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
                    p.WriteShort(e.ImbueTries);
                    p.WriteHexString("00"); // unknown
                    p.WriteShort(e.DragonSuccessImbueTries);
                    p.WriteByte(e.DiscountRepairFee);
                    p.WriteShort(e.TotalDragonImbueTries);
                    p.WriteShort(e.DragonDamage);
                    p.WriteShort(e.DragonDefence);
                    p.WriteShort(e.DragonAttackRating);
                    p.WriteShort(e.DragonLife);
                    if (e is Mirror)
                    {
                        Mirror m = e as Mirror;
                        p.WriteInt(m.PetID);
                        p.WriteShort(m.LifeAbsorb);
                        p.WriteShort(m.DamageAbsorb);
                        p.WriteShort(m.DefenseAbsorb);
                        p.WriteShort(m.AttackRatingAbsorb);
                        p.WriteHexString("00 00 00 00 00 00 00 00 00 00"); // pet skill stuff yo
                    }
                    else
                    {
                        p.WriteByte(e.MappedData);
                        if (!(e is Ring) && !(e is Necklace) && !(e is Cape))
                        {
                            p.WriteByte(e.ForceSlot);
                            p.WriteHexString("00 00"); // other slots
                        }
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
                            p.WriteShort(e.RebirthHole);
                            //p.WriteShort(e.RebirthHoleStat);
                        }
                    }
                }
                if (i is ImbueItem)
                {
                    ImbueItem im = i as ImbueItem;
                    p.WriteByte(im.RequiredClass);
                    p.WriteShort(i.Amount);
                    if (i is RbHoleItem)
                    {
                        p.WriteByte(0);
                        p.WriteInt(im.ImbueChance);
                        p.WriteInt(im.IncreaseValue);
                    }
                    else
                    {
                        p.WriteByte(im.ImbueData);
                        p.WriteShort(im.ImbueChance);
                        p.WriteShort(im.IncreaseValue);
                        p.WriteHexString("00 00"); // could be lowest lvl who knows
                        p.WriteHexString("00 00"); // maxlevel of usage, for dragon stones 
                    }
                }
                if (i is PotionItem)
                {
                    PotionItem pot = i as PotionItem;
                    p.WriteByte(pot.RequiredClass);
                    p.WriteShort(pot.Amount);
                    p.WriteHexString("00 00 00 00");
                    p.WriteShort(pot.HealHp);
                    p.WriteShort(pot.HealMana);
                    p.WriteHexString("00 20 4E 00 00");
                }
                if (i is BookItem)
                {
                    BookItem book = i as BookItem;
                    p.WriteByte(book.RequiredClass);
                    p.WriteShort(book.Amount);
                    p.WriteShort(book.RequiredLevel);
                    p.WriteInt(book.SkillID);
                    p.WriteByte(book.SkillLevel);
                    p.WriteShort(book.SkillData);
                }
                if (i is BeadItem)
                {
                    BeadItem bead = i as BeadItem;
                    p.WriteByte(bead.RequiredClass);
                    p.WriteShort(bead.Amount);
                    p.WriteInt(bead.ToMapID);
                    p.WriteHexString("01 00 00 00 00");
                }
                if (i is StoreTag)
                {
                    StoreTag tag = i as StoreTag;
                    p.WriteByte(tag.RequiredClass);
                    p.WriteShort(tag.Amount);
                    p.WriteHexString("00 00");
                    p.WriteShort(tag.TimeLeft);
                    p.WriteShort(tag.TimeMax);
                    p.WriteHexString("00 00 00 00");
                }
                if (i is PetItem)
                {
                    PetItem pet = i as PetItem;
                    p.WriteByte(pet.RequiredClass);
                    p.WriteShort(pet.Amount);
                    p.WriteByte(pet.TameChance);
                    p.WriteHexString("00 00"); // unknown
                    p.WriteByte(pet.DecreaseWildness);
                    p.WriteInt(pet.HealLife);
                }
                if (i is RebirthPill)
                {
                    RebirthPill pill = i as RebirthPill;
                    p.WriteHexString("00");
                    p.WriteShort(pill.Amount);
                    p.WriteShort(pill.RequiredRebirth);
                    p.WriteHexString("00");
                    p.WriteShort(pill.RequiredLevel);
                }
            }

            return p.GetWrittenBuffer(PacketIds.SendWarehouseItemList);
        }

        public static byte[] SendBuyOtherPlayerShopItemError(byte error)
        {
            Packet p = new Packet(20);
            p.WriteByte(error);
            return p.GetWrittenBuffer(PacketIds.SendBuyOtherPlayerShopItemError);
        }

        public static byte[] SendOtherPlayerShop(Character ch)
        {
            Packet p = new Packet(200);
            p.WriteHexString("00");
            p.WriteInt(ch.CharacterId);
            p.WriteByte(ch.Shop.ItemCount);
            foreach (BaseItem i in ch.Shop.Items)
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
                    p.WriteShort(e.MaxImbueTries);
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
                    p.WriteShort(e.ImbueTries);
                    p.WriteHexString("00"); // unknown
                    p.WriteShort(e.DragonSuccessImbueTries);
                    p.WriteByte(e.DiscountRepairFee);
                    p.WriteShort(e.TotalDragonImbueTries);
                    p.WriteShort(e.DragonDamage);
                    p.WriteShort(e.DragonDefence);
                    p.WriteShort(e.DragonAttackRating);
                    p.WriteShort(e.DragonLife);
                    if (e is Mirror)
                    {
                        Mirror m = e as Mirror;
                        p.WriteInt(m.PetID);
                        p.WriteShort(m.LifeAbsorb);
                        p.WriteShort(m.DamageAbsorb);
                        p.WriteShort(m.DefenseAbsorb);
                        p.WriteShort(m.AttackRatingAbsorb);
                        p.WriteHexString("00 00 00 00 00 00 00 00"); // pet skill stuff yo
                    }
                    else
                    {
                        p.WriteByte(e.MappedData);
                        if (!(e is Ring) && !(e is Necklace) && !(e is Cape))
                        {
                            p.WriteByte(e.ForceSlot);
                            p.WriteHexString("00 00"); // otherslots
                        }
                        else
                            p.WriteHexString("00 00");
                        if (e is Cape)
                        {
                            Cape c = e as Cape;
                            p.WriteHexString("00 00 00 00"); // required guild position 
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
                        }
                        if (!(e is Ring) && !(e is Necklace) && !(e is Cape))
                        {
                            p.WriteShort(e.RebirthHole);
                           // p.WriteByte(e.RebirthHoleStat);
                        }
                    }
                }
                if (i is ImbueItem)
                {
                    ImbueItem im = i as ImbueItem;
                    p.WriteByte(im.RequiredClass);
                    p.WriteShort(i.Amount);
                    p.WriteByte(im.ImbueData);
                    p.WriteShort(im.ImbueChance);
                    p.WriteShort(im.IncreaseValue);
                    p.WriteHexString("00 00"); // could be lowest lvl who knows
                    p.WriteHexString("00 00"); // maxlevel of usage, for dragon stones 
                }
                if (i is PotionItem)
                {
                    PotionItem pot = i as PotionItem;
                    p.WriteByte(pot.RequiredClass);
                    p.WriteShort(pot.Amount);
                    p.WriteHexString("00 00 00 00");
                    p.WriteShort(pot.HealHp);
                    p.WriteShort(pot.HealMana);
                    p.WriteHexString("00 20 4E 00 00");
                }
                if (i is BookItem)
                {
                    BookItem book = i as BookItem;
                    p.WriteByte(book.RequiredClass);
                    p.WriteShort(book.Amount);
                    p.WriteShort(book.RequiredLevel);
                    p.WriteInt(book.SkillID);
                    p.WriteByte(book.SkillLevel);
                    p.WriteShort(book.SkillData);
                }
                if (i is BeadItem)
                {
                    BeadItem bead = i as BeadItem;
                    p.WriteByte(bead.RequiredClass);
                    p.WriteShort(bead.Amount);
                    p.WriteInt(bead.ToMapID);
                    p.WriteHexString("01 00 00 00 00");
                }
                if (i is StoreTag)
                {
                    StoreTag tag = i as StoreTag;
                    p.WriteByte(tag.RequiredClass);
                    p.WriteShort(tag.Amount);
                    p.WriteHexString("00 00");
                    p.WriteShort(tag.TimeLeft);
                    p.WriteShort(tag.TimeMax);
                    p.WriteHexString("00 00 00 00 00 00");
                }
                if (i is PetItem)
                {
                    PetItem pet = i as PetItem;
                    p.WriteByte(pet.RequiredClass);
                    p.WriteShort(pet.Amount);
                    p.WriteByte(pet.TameChance);
                    p.WriteHexString("00 00"); // unknown
                    p.WriteByte(pet.DecreaseWildness);
                    p.WriteInt(pet.HealLife);
                }
                if (i is RebirthPill)
                {
                    RebirthPill pill = i as RebirthPill;
                    p.WriteHexString("00");
                    p.WriteShort(pill.Amount);
                    p.WriteShort(pill.RequiredRebirth);
                    p.WriteHexString("00");
                    p.WriteShort(pill.RequiredLevel);
                }

                p.WriteInt(i.SellPrice);
                if (i is Equipment)
                {
                    Equipment e = i as Equipment;
                    if (e.RebirthHoleStat > 0)
                        p.WriteShort(e.RebirthHoleStat);
                    else
                        p.WriteHexString("00 00");
                }
                else
                    p.WriteHexString("00 00");
            }

            return p.GetWrittenBuffer(PacketIds.SendOpenOtherPlayerShop);
        }

        public static byte[] SendPlayerShop(Character ch, short timeleft)
        {
            Packet p = new Packet(20);
            p.WriteHexString("00");
            p.WriteInt(ch.CharacterId);
            p.WriteHexString("00");
            p.WriteString(ch.Shop.ShopName);
            p.WriteString(ch.Shop.ShopDesc);
            p.WriteInt(ch.Shop.TotalMoney);
            p.WriteShort(timeleft);
            p.WriteByte(ch.Shop.ItemCount);
            foreach (BaseItem i in ch.Shop.Items)
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
                    p.WriteShort(e.MaxImbueTries);
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
                    p.WriteShort(e.ImbueTries);
                    p.WriteHexString("00"); // unknown
                    p.WriteShort(e.DragonSuccessImbueTries);
                    p.WriteByte(e.DiscountRepairFee);
                    p.WriteShort(e.TotalDragonImbueTries);
                    p.WriteShort(e.DragonDamage);
                    p.WriteShort(e.DragonDefence);
                    p.WriteShort(e.DragonAttackRating);
                    p.WriteShort(e.DragonLife);
                    if (e is Mirror)
                    {
                        Mirror m = e as Mirror;
                        p.WriteInt(m.PetID);
                        p.WriteShort(m.LifeAbsorb);
                        p.WriteShort(m.DamageAbsorb);
                        p.WriteShort(m.DefenseAbsorb);
                        p.WriteShort(m.AttackRatingAbsorb);
                        p.WriteHexString("00 00 00 00 00 00 00 00"); // pet skill stuff yo
                    }
                    else
                    {
                        p.WriteByte(e.MappedData);
                        if (!(e is Ring) && !(e is Necklace) && !(e is Cape))
                        {
                            p.WriteByte(e.ForceSlot);
                            p.WriteHexString("00 00"); // otherslots
                        }
                        else
                            p.WriteHexString("00 00");
                        if (e is Cape)
                        {
                            Cape c = e as Cape;
                            p.WriteHexString("00 00 00 00"); // required guild position 
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
                        }
                        if (!(e is Ring) && !(e is Necklace) && !(e is Cape))
                        {
                            p.WriteShort(e.RebirthHole);
                            //p.WriteByte(e.RebirthHoleStat);
                        }
                    }
                }
                if (i is ImbueItem)
                {
                    ImbueItem im = i as ImbueItem;
                    p.WriteByte(im.RequiredClass);
                    p.WriteShort(i.Amount);
                    if (i is RbHoleItem)
                    {
                        p.WriteByte(0);
                        p.WriteInt(im.ImbueChance);
                        p.WriteInt(im.IncreaseValue);
                    }
                    else
                    {
                        p.WriteByte(im.ImbueData);
                        p.WriteShort(im.ImbueChance);
                        p.WriteShort(im.IncreaseValue);
                        p.WriteHexString("00 00"); // could be lowest lvl who knows
                        p.WriteHexString("00 00"); // maxlevel of usage, for dragon stones 
                    }
                }
                if (i is PotionItem)
                {
                    PotionItem pot = i as PotionItem;
                    p.WriteByte(pot.RequiredClass);
                    p.WriteShort(pot.Amount);
                    p.WriteHexString("00 00 00 00");
                    p.WriteShort(pot.HealHp);
                    p.WriteShort(pot.HealMana);
                    p.WriteHexString("00 20 4E 00 00");
                }
                if (i is BookItem)
                {
                    BookItem book = i as BookItem;
                    p.WriteByte(book.RequiredClass);
                    p.WriteShort(book.Amount);
                    p.WriteShort(book.RequiredLevel);
                    p.WriteInt(book.SkillID);
                    p.WriteByte(book.SkillLevel);
                    p.WriteShort(book.SkillData);
                }
                if (i is BeadItem)
                {
                    BeadItem bead = i as BeadItem;
                    p.WriteByte(bead.RequiredClass);
                    p.WriteShort(bead.Amount);
                    p.WriteInt(bead.ToMapID);
                    p.WriteHexString("01 00 00 00 00");
                }
                if (i is StoreTag)
                {
                    StoreTag tag = i as StoreTag;
                    p.WriteByte(tag.RequiredClass);
                    p.WriteShort(tag.Amount);
                    p.WriteHexString("00 00");
                    p.WriteShort(tag.TimeLeft);
                    p.WriteShort(tag.TimeMax);
                    p.WriteHexString("00 00 00 00 00 00");
                }
                if (i is PetItem)
                {
                    PetItem pet = i as PetItem;
                    p.WriteByte(pet.RequiredClass);
                    p.WriteShort(pet.Amount);
                    p.WriteByte(pet.TameChance);
                    p.WriteHexString("00 00"); // unknown
                    p.WriteByte(pet.DecreaseWildness);
                    p.WriteInt(pet.HealLife);
                }
                if (i is RebirthPill)
                {
                    RebirthPill pill = i as RebirthPill;
                    p.WriteHexString("00");
                    p.WriteShort(pill.Amount);
                    p.WriteShort(pill.RequiredRebirth);
                    p.WriteHexString("00");
                    p.WriteShort(pill.RequiredLevel);
                }

                p.WriteInt(i.SellPrice);
                if (i is Equipment)
                {
                    Equipment e = i as Equipment;
                    if (e.RebirthHoleStat > 0)
                        p.WriteShort(e.RebirthHoleStat);
                    else
                        p.WriteHexString("00 00");
                }
                else
                    p.WriteHexString("00 00");
            }

            return p.GetWrittenBuffer(PacketIds.SendPlayerShop);
        }

        public static byte[] SendShopStartSelling(byte state)
        {
            Packet p = new Packet(10);
            p.WriteHexString("00");
            p.WriteByte(state);
            return p.GetWrittenBuffer(PacketIds.SendShopStartSelling);
        }

        public static byte[] SendShopTimeLeft(int money, short timeleft)
        {
            Packet p = new Packet(40);
            p.WriteInt(money);
            p.WriteShort(timeleft);
            return p.GetWrittenBuffer(PacketIds.SendShopTimeLeft);
        }

        public static byte[] SendChangeShopInfo(Shop shop)
        {
            Packet p = new Packet(50);
            p.WriteByte(2); // no idea
            p.WriteInt(shop.OwnerID);
            p.WriteByte(Convert.ToByte(shop.Active));
            p.WriteHexString("00 00 00 00 00 00 00 00 00 00 00");
            if (shop.ShopName == "")
                p.WriteString("Welcome Xiah GCF by xiahdev.org");
            else
                p.WriteString(shop.ShopName);
            p.WriteString(shop.ShopDesc);
            p.WriteHexString("02 00 00 00");
            return p.GetWrittenBuffer(PacketIds.SendChangeShopInfo);
        }

        public static byte[] SendShopTaxesAmount(Shop shop)
        {
            Packet p = new Packet(50);
            p.WriteInt(shop.TotalMoney);
            p.WriteInt(0); // something
            p.WriteInt(0); // something
            p.WriteShort(4); // something ^_^ 
            p.WriteInt(39); // something
            return p.GetWrittenBuffer(PacketIds.SendShopTaxAmount);
        }

        public static byte[] SendMoveShopItemToBagError(byte error)
        {
            Packet p = new Packet(10);
            p.WriteByte(error);
            return p.GetWrittenBuffer(PacketIds.SendMoveShopItemToBag);
        }

        public static byte[] SendChangeShopInfoError(byte error)
        {
            Packet p = new Packet(10);
            p.WriteByte(error);
            return p.GetWrittenBuffer(PacketIds.SendChangeShopInfoError);
        }

        public static byte[] SendRemoveShopItem(byte slot, int itemid, int amount)
        {
            Packet p = new Packet(20);
            p.WriteByte(slot);
            p.WriteInt(itemid);
            p.WriteInt(amount);
            return p.GetWrittenBuffer(PacketIds.SendRemoveShopItem);
        }

        public static byte[] SendAddItemToShopError(AddItemToShopError error, int price)
        {
            Packet p = new Packet(20);
            p.WriteByte((byte)error);
            p.WriteInt(price);

            return p.GetWrittenBuffer(PacketIds.SendAddItemToShopError);
        }

        public static byte[] SendAddItemToShop(BaseItem i, int price)
        {
            Packet p = new Packet(500);

            p.WriteByte(2);
            p.WriteByte(i.Slot);
            p.WriteInt(price);
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
                p.WriteShort(e.MaxImbueTries);
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
                p.WriteShort(e.ImbueTries);
                p.WriteHexString("00"); // unknown
                p.WriteShort(e.DragonSuccessImbueTries);
                p.WriteByte(e.DiscountRepairFee);
                p.WriteShort(e.TotalDragonImbueTries);
                p.WriteShort(e.DragonDamage);
                p.WriteShort(e.DragonDefence);
                p.WriteShort(e.DragonAttackRating);
                p.WriteShort(e.DragonLife);
                if (e is Mirror)
                {
                    Mirror m = e as Mirror;
                    p.WriteInt(m.PetID);
                    p.WriteShort(m.LifeAbsorb);
                    p.WriteShort(m.DamageAbsorb);
                    p.WriteShort(m.DefenseAbsorb);
                    p.WriteShort(m.AttackRatingAbsorb);
                    p.WriteHexString("00 00 00 00 00 00 00 00 00 00"); // pet skill stuff yo
                }
                else
                {
                    p.WriteByte(e.MappedData);
                    if (!(e is Ring) && !(e is Necklace) && !(e is Cape))
                    {
                        p.WriteByte(e.ForceSlot);
                        p.WriteHexString("00 00"); // otherslots
                    }
                    else
                        p.WriteHexString("00 00");
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
                        p.WriteShort(e.RebirthHole);
                        p.WriteShort(e.RebirthHoleStat);
                    }
                }
            }
            if (i is ImbueItem)
            {
                ImbueItem im = i as ImbueItem;
                p.WriteByte(im.RequiredClass);
                p.WriteShort(i.Amount);
                if (i is RbHoleItem)
                {
                    p.WriteByte(0);
                    p.WriteInt(im.ImbueChance);
                    p.WriteInt(im.IncreaseValue);
                    p.WriteHexString("00 00");
                }
                else
                {
                    p.WriteByte(im.ImbueData);
                    p.WriteShort(im.ImbueChance);
                    p.WriteShort(im.IncreaseValue);
                    p.WriteHexString("00 00"); // could be lowest lvl who knows
                    p.WriteHexString("00 00"); // maxlevel of usage, for dragon stones 
                    p.WriteHexString("00 00"); // unknown
                }
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
            if (i is StoreTag)
            {
                StoreTag tag = i as StoreTag;
                p.WriteByte(tag.RequiredClass);
                p.WriteShort(tag.Amount);
                p.WriteHexString("00 00");
                p.WriteShort(tag.TimeLeft);
                p.WriteShort(tag.TimeMax);
                p.WriteHexString("00 00 00 00 00 00");
            }
            if (i is PetItem)
            {
                PetItem pet = i as PetItem;
                p.WriteByte(pet.RequiredClass);
                p.WriteShort(pet.Amount);
                p.WriteByte(pet.TameChance);
                p.WriteHexString("00 00 00 00"); // unknown
                p.WriteByte(pet.DecreaseWildness);
                p.WriteInt(pet.HealLife);
            }
            if (i is RebirthPill)
            {
                RebirthPill pill = i as RebirthPill;
                p.WriteHexString("00");
                p.WriteShort(pill.Amount);
                p.WriteShort(pill.RequiredRebirth);
                p.WriteHexString("00");
                p.WriteShort(pill.RequiredLevel);
                p.WriteHexString("00 00");
            }

            return p.GetWrittenBuffer(PacketIds.SendAddItemToShop);
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
                    p.WriteShort(e.MaxImbueTries);
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
                    p.WriteShort(e.ImbueTries);
                    p.WriteHexString("00"); // unknown
                    p.WriteShort(e.DragonSuccessImbueTries);
                    p.WriteByte(e.DiscountRepairFee);
                    p.WriteShort(e.TotalDragonImbueTries);
                    p.WriteShort(e.DragonDamage);
                    p.WriteShort(e.DragonDefence);
                    p.WriteShort(e.DragonAttackRating);
                    p.WriteShort(e.DragonLife);
                    if (e is Mirror)
                    {
                        Mirror m = e as Mirror;
                        p.WriteInt(m.PetID);
                        p.WriteShort(m.LifeAbsorb);
                        p.WriteShort(m.DamageAbsorb);
                        p.WriteShort(m.DefenseAbsorb);
                        p.WriteShort(m.AttackRatingAbsorb);
                        p.WriteHexString("00 00 00 00 00 00 00 00 00 00"); // pet skill stuff yo
                    }
                    else
                    {
                        p.WriteByte(e.MappedData);
                        if (!(e is Ring) && !(e is Necklace) && !(e is Cape))
                        {
                            p.WriteByte(e.ForceSlot);
                            p.WriteHexString("00 00"); // other force slots
                        }
                        else
                        {
                            p.WriteHexString("00 00");
                        }
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
                            p.WriteShort(e.RebirthHole);
                            p.WriteShort(e.RebirthHoleStat);
                        }
                    }
                }
                if (i is ImbueItem)
                {
                    ImbueItem im = i as ImbueItem;
                    p.WriteByte(im.RequiredClass);
                    p.WriteShort(i.Amount);
                    if (i is RbHoleItem)
                    {
                        p.WriteByte(0);
                        p.WriteInt(im.ImbueChance);
                        p.WriteInt(im.IncreaseValue);
                        p.WriteHexString("00 00");
                    }
                    else
                    {
                        p.WriteByte(im.ImbueData);
                        p.WriteShort(im.ImbueChance);
                        p.WriteShort(im.IncreaseValue);
                        p.WriteHexString("00 00"); // could be lowest lvl who knows
                        p.WriteHexString("00 00"); // maxlevel of usage, for dragon stones 
                        p.WriteHexString("00 00"); // unknown
                    }
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
                if (i is StoreTag)
                {
                    StoreTag tag = i as StoreTag;
                    p.WriteByte(tag.RequiredClass);
                    p.WriteShort(tag.Amount);
                    p.WriteHexString("00 00");
                    p.WriteShort(tag.TimeLeft);
                    p.WriteShort(tag.TimeMax);
                    p.WriteHexString("00 00 00 00 00 00");
                }
                if (i is PetItem)
                {
                    PetItem pet = i as PetItem;
                    p.WriteByte(pet.RequiredClass);
                    p.WriteShort(pet.Amount);
                    p.WriteByte(pet.TameChance);
                    p.WriteHexString("00 00 00 00"); // unknown
                    p.WriteByte(pet.DecreaseWildness);
                    p.WriteInt(pet.HealLife);
                }
                if (i is RebirthPill)
                {
                    RebirthPill pill = i as RebirthPill;
                    p.WriteHexString("00");
                    p.WriteShort(pill.Amount);
                    p.WriteShort(pill.RequiredRebirth);
                    p.WriteHexString("00");
                    p.WriteShort(pill.RequiredLevel);
                    p.WriteHexString("00 00");
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
                p.WriteShort(e.MaxImbueTries);
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
                p.WriteShort(e.ImbueTries);
                p.WriteHexString("00"); // unknown
                p.WriteShort(e.DragonSuccessImbueTries);
                p.WriteByte(e.DiscountRepairFee);
                p.WriteShort(e.TotalDragonImbueTries);
                p.WriteShort(e.DragonDamage);
                p.WriteShort(e.DragonDefence);
                p.WriteShort(e.DragonAttackRating);
                p.WriteShort(e.DragonLife);
                if (e is Mirror)
                {
                    Mirror m = e as Mirror;
                    p.WriteInt(m.PetID);
                    p.WriteShort(m.LifeAbsorb);
                    p.WriteShort(m.DamageAbsorb);
                    p.WriteShort(m.DefenseAbsorb);
                    p.WriteShort(m.AttackRatingAbsorb);
                    p.WriteHexString("00 00 00 00 00 00 00 00 00 00"); // pet skill stuff yo
                }
                else
                {
                    p.WriteByte(e.MappedData);
                    if (!(e is Ring) && !(e is Necklace) && !(e is Cape))
                    {
                        p.WriteByte(e.ForceSlot);
                        p.WriteHexString("00 00"); // otherslots
                    }
                    else
                        p.WriteHexString("00 00");
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
                        p.WriteShort(e.RebirthHole);
                        p.WriteShort(e.RebirthHoleStat);
                    }
                }
            }
            if (i is ImbueItem)
            {
                ImbueItem im = i as ImbueItem;
                p.WriteByte(im.RequiredClass);
                p.WriteShort(i.Amount);
                if (i is RbHoleItem)
                {
                    p.WriteByte(0);
                    p.WriteInt(im.ImbueChance);
                    p.WriteInt(im.IncreaseValue);
                    p.WriteHexString("00 00");
                }
                else
                {
                    p.WriteByte(im.ImbueData);
                    p.WriteShort(im.ImbueChance);
                    p.WriteShort(im.IncreaseValue);
                    p.WriteHexString("00 00"); // could be lowest lvl who knows
                    p.WriteHexString("00 00"); // maxlevel of usage, for dragon stones 
                    p.WriteHexString("00 00"); // unknown
                }
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
            if (i is StoreTag)
            {
                StoreTag tag = i as StoreTag;
                p.WriteByte(tag.RequiredClass);
                p.WriteShort(tag.Amount);
                p.WriteHexString("00 00");
                p.WriteShort(tag.TimeLeft);
                p.WriteShort(tag.TimeMax);
                p.WriteHexString("00 00 00 00 00 00");
            }
            if (i is PetItem)
            {
                PetItem pet = i as PetItem;
                p.WriteByte(pet.RequiredClass);
                p.WriteShort(pet.Amount);
                p.WriteShort(pet.TameChance);
                p.WriteHexString("00 00 00 00"); // unknown
                p.WriteByte(pet.DecreaseWildness);
                p.WriteInt(pet.HealLife);
            }
            if (i is RebirthPill)
            {
                RebirthPill pill = i as RebirthPill;
                p.WriteHexString("00");
                p.WriteShort(pill.Amount);
                p.WriteShort(pill.RequiredRebirth);
                p.WriteHexString("00");
                p.WriteShort(pill.RequiredLevel);
                p.WriteHexString("00 00");
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
                p.WriteShort(e.MaxImbueTries);
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
                p.WriteShort(e.ImbueTries);
                p.WriteHexString("00"); // unknown
                p.WriteShort(e.DragonSuccessImbueTries);
                p.WriteByte(e.DiscountRepairFee);
                p.WriteShort(e.TotalDragonImbueTries);
                p.WriteShort(e.DragonDamage);
                p.WriteShort(e.DragonDefence);
                p.WriteShort(e.DragonAttackRating);
                p.WriteShort(e.DragonLife);
                if (e is Mirror)
                {
                    Mirror m = e as Mirror;
                    p.WriteInt(m.PetID);
                    p.WriteShort(m.LifeAbsorb);
                    p.WriteShort(m.DamageAbsorb);
                    p.WriteShort(m.DefenseAbsorb);
                    p.WriteShort(m.AttackRatingAbsorb);
                    p.WriteHexString("00 00 00 00 00 00 00 00 00 00"); // pet skill stuff yo
                }
                else
                {
                    if (!(e is Ring) && !(e is Necklace) && !(e is Cape))
                    {
                        p.WriteByte(e.ForceSlot);
                        p.WriteHexString("00 00 00"); // otherslots
                    }
                    else
                        p.WriteHexString("00 00");
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
                        p.WriteShort(e.RebirthHole);
                        p.WriteShort(e.RebirthHoleStat);
                    }
                }
            }
            if (i is ImbueItem)
            {
                ImbueItem im = i as ImbueItem;
                p.WriteByte(im.RequiredClass);
                p.WriteShort(i.Amount);
                if (i is RbHoleItem)
                {
                    p.WriteByte(0);
                    p.WriteInt(im.ImbueChance);
                    p.WriteInt(im.IncreaseValue);
                    p.WriteHexString("00 00");
                }
                else
                {
                    p.WriteByte(im.ImbueData);
                    p.WriteShort(im.ImbueChance);
                    p.WriteShort(im.IncreaseValue);
                    p.WriteHexString("00 00"); // could be lowest lvl who knows
                    p.WriteHexString("00 00"); // maxlevel of usage, for dragon stones 
                    p.WriteHexString("00 00"); // unknown
                }
            }
            return p.GetWrittenBuffer(PacketIds.SendImbuedItem);
        }

        public static byte[] SendCombineMessage(byte error)
        {
            Packet p = new Packet(20);
            p.WriteByte(error);
            return p.GetWrittenBuffer(PacketIds.SendCombineMessage);
        }

        public static byte[] SendCombineMessage2()
        {
            Packet p = new Packet(20);
            p.WriteHexString("2B 31 06 00 97 DD 5E 00 0A 00");
            return p.GetWrittenBuffer();
        }

        public static byte[] SendPickUpText(short referenceId, int amount, byte error)
        {
            Packet p = new Packet(200);
            p.WriteByte(error); 
            p.WriteHexString("00");
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

        public static byte[] SendSelectedSkill(SelectSkillInfo i)
        {
            Packet p = new Packet(20);
            p.WriteHexString("00");
            p.WriteByte(i.Row);
            p.WriteInt(i.SkillID);
            p.WriteByte(i.Slot);
            return p.GetWrittenBuffer(PacketIds.SendSelectedSkill);
        }

        public static byte[] SendQuickSlots(Character ch)
        {
            Packet p = new Packet(200);

            p.WriteByte(1); // list 1, later add all 3 listes somehow
            for (int i = 0; i < ch.QuickSlots.Length; i++)
            {
                p.WriteShort(ch.QuickSlots[i].Value);
                p.WriteHexString("00 00");
            }

          //  p.WriteHexString("2E 31 7B 00 01 20 00 00 00 28 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 02 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 03 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");
            p.WriteHexString("02 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 03 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");
            return p.GetWrittenBuffer(PacketIds.SendQuickSlotList);
        }

        public static byte[] SendUpdateQuickSlot(byte slot, short visual)
        {
            Packet p = new Packet(20);
            p.WriteHexString("00");
            p.WriteByte(slot);
            p.WriteShort(visual);
            p.WriteHexString("00 00");

            return p.GetWrittenBuffer(PacketIds.SendUpdateQuickSlot);
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
            p.WriteShort((short)Char.Position.X);
            p.WriteShort((short)Char.Position.Y);
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

        public static byte[] SendStartMoving(MovementInfo mo, Character ch)
        {
            Packet p = new Packet(200);
            p.WriteByte(0);
            p.WriteInt(ch.CharacterId);
            p.WriteShort(mo.FromX);
            p.WriteShort(mo.FromY);
            p.WriteByte(mo.FromZ);
            p.WriteShort(mo.ToX);
            p.WriteShort(mo.ToY);
            p.WriteByte(mo.ToZ);
            p.WriteShort(mo.Rotation);
            p.WriteHexString("00");
            p.WriteByte(11 + ch.MovingSpeed);
            if (ch.UsingFastMovement())
                p.WriteHexString("06");
            else
                p.WriteHexString("00");

            return p.GetWrittenBuffer(PacketIds.SendStartMoving);
        }

        public static byte[] SendKeepMoving(MovementInfo mo, Character ch)
        {
            Packet p = new Packet(200);
            p.WriteByte(0);
            p.WriteInt(ch.CharacterId);
            p.WriteShort(mo.FromX);
            p.WriteShort(mo.FromY);
            p.WriteByte(mo.FromZ);
            p.WriteShort(mo.ToX);
            p.WriteShort(mo.ToY);
            p.WriteByte(mo.ToZ);
            p.WriteShort(mo.Rotation);
            p.WriteHexString("00");
            p.WriteByte(11 + ch.MovingSpeed);
            if (ch.UsingFastMovement())
                p.WriteHexString("06");
            else
                p.WriteHexString("00");
            p.WriteHexString("00");
            return p.GetWrittenBuffer(PacketIds.SendKeepMoving);
        }

        public static byte[] SendStopMoving(MovementInfo mo, Character ch)
        {
            Packet p = new Packet(200);
            p.WriteByte(0);
            p.WriteInt(ch.CharacterId);
            p.WriteShort(mo.FromX);
            p.WriteShort(mo.FromY);
            p.WriteByte(mo.FromZ);
            p.WriteByte(0);
            return p.GetWrittenBuffer(PacketIds.SendStopMoving);
        }

        public static byte[] SendSpawnMonster(Monster m)
        {
            Packet p = new Packet(200);
            p.WriteHexString("00"); // something 
            p.WriteInt(m.MonsterID);
            p.WriteByte(m.MonsterType);
            p.WriteInt(m.MapID);
            p.WriteShort((short)m.Position.X);
            p.WriteShort(m.Level);
            p.WriteShort((short)m.Position.Y);
            p.WriteHexString("00"); // unknown
            p.WriteShort(m.Direction);
            p.WriteInt(m.MonsterReferenceID);
            p.WriteHexString("14");
            p.WriteShort((short)m.Position.X);
            p.WriteShort((short)m.Position.Y);
            p.WriteHexString("00");
            p.WriteInt(m.MaxHealth);
            p.WriteInt(m.CurHealth);
            p.WriteByte(m.MovementSpeed);
            p.WriteByte(m.LeaderID);
            p.WriteHexString("FF 00 00 00 00");
            p.WriteHexString("00 00 00 00");
            return p.GetWrittenBuffer(PacketIds.SendSpawnMonster);
        }

        public static byte[] SendSealPet(Pet pet)
        {
            Packet p = new Packet(20);
            p.WriteHexString("00");
            p.WriteInt(pet.PetID);
            return p.GetWrittenBuffer(PacketIds.SendSealPet);
        }

        public static byte[] SendUnSealPet(Pet pet)
        {
            Packet p = new Packet(200);
            p.WriteHexString("00");
            p.WriteInt(pet.PetID);
            p.WriteInt(pet.OwnerID);
            p.WriteInt(pet.MapID);
            p.WriteByte(pet.PetType);
            p.WriteString(pet.Name);
            p.WriteShort(pet.Level);
            p.WriteShort((short)pet.Position.X);
            p.WriteShort((short)pet.Position.Y);
            p.WriteHexString("4E 4E");
            p.WriteHexString("01 78 02 4E C6 00");
            p.WriteInt(pet.MaxHealth);
            p.WriteInt(pet.CurHealth);
            p.WriteShort(pet.Damage);
            p.WriteShort(pet.Defense);
            p.WriteShort(pet.AttackRating);
            p.WriteHexString("01 00 09 00 00 00 00 00 25 32 00 00 01");
            p.WriteLong(pet.CurrentExperience);
            p.WriteLong(pet.NegativeExperience);
            p.WriteLong(pet.ExperienceToLevel + pet.NegativeExperience);
            p.WriteByte(pet.Evolution);
            p.WriteByte(pet.Wildness);
            p.WriteByte(Convert.ToByte(pet.IsLegendary));
            p.WriteHexString("00 00 00 00");
            p.WriteHexString("00 00 00 00 00 00 00 00");
            return p.GetWrittenBuffer(PacketIds.SendUnSealPet);
        }

        public static byte[] SendSummonSpawn(Pet pet)
        {
            Packet p = new Packet(200);
            p.WriteHexString("00");
            p.WriteInt(pet.PetID);
            p.WriteInt(pet.OwnerID);
            p.WriteInt(pet.MapID);
         //   pet.PetType = 2;
            p.WriteByte(pet.PetType); // type, 250 = summon, 251 = sm shadow, 252 = mage shadow, 253 = warrior shadow, 254 = gf shadow
            p.WriteHexString("06 00 2C D2 B0 C6 00 00");
            p.WriteShort(pet.Level);
            p.WriteShort(pet.Position.X);
            p.WriteShort(pet.Position.Y);
            p.WriteHexString("00");
            p.WriteShort(pet.Position.X);
            p.WriteShort(pet.Position.Y);
            p.WriteHexString("00");
            p.WriteHexString("BB 00");
            p.WriteInt(pet.CurHealth);
            p.WriteInt(pet.MaxHealth);
            p.WriteShort(pet.Damage);
            p.WriteShort(pet.Defense);
            p.WriteShort(pet.AttackRating);
            p.WriteShort(pet.AttackRating);
            p.WriteHexString("00 14 00 14 00 14 00 00 00 00 02 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");
            return p.GetWrittenBuffer(PacketIds.SendSummonPet);
        }

        public static byte[] SendMonsterKeepMoving(BaseEntity ent, MovementInfo mi)
        {
            Pet pet = null;
            Packet p = new Packet(200);
            if (ent is Pet)
            {
                pet = ent as Pet;
                p.WriteHexString("00");
                p.WriteInt(pet.PetID);
                p.WriteByte((byte)pet.Type);
                p.WriteShort(mi.FromX);
                p.WriteShort(mi.FromY);
                p.WriteByte(mi.FromZ);
                p.WriteShort(mi.ToX);
                p.WriteShort(mi.ToY);
                p.WriteByte(mi.ToZ);
                p.WriteInt(mi.Rotation);
                p.WriteHexString("00 00");
            }
            return p.GetWrittenBuffer(PacketIds.SendMonsterKeepMoving);
        }

        public static byte[] SendMonsterKeepMoving(BaseEntity ent, MonsterMoveInfo mi)
        {
            Monster m = null;
            Packet p = new Packet(200);
            if (ent is Monster)
            {
                m = ent as Monster;
                p.WriteHexString("00");
                p.WriteInt(m.MonsterID);
                p.WriteByte((byte)m.Type);
                p.WriteShort(mi.CurrentPosition.X);
                p.WriteShort(mi.CurrentPosition.Y);
                p.WriteByte(0);
                p.WriteShort(mi.Destination.X);
                p.WriteShort(mi.Destination.Y);
                p.WriteByte(0);
                p.WriteShort((short)mi.Rotation);
                p.WriteHexString("14");
                p.WriteByte(m.MovementSpeed);
                p.WriteHexString("00 00");
            }
            return p.GetWrittenBuffer(PacketIds.SendMonsterKeepMoving);
        }

        public static byte[] SendMonsterStopMoving(BaseEntity ent, MonsterMoveInfo mi)
        {
            Monster m = null;
            Packet p = new Packet(200);
            if (ent is Monster)
            {
                m = ent as Monster;
                p.WriteHexString("00");
                p.WriteInt(m.MonsterID);
                p.WriteByte((byte)m.Type);
                p.WriteShort(mi.CurrentPosition.X);
                p.WriteShort(mi.CurrentPosition.Y);
                p.WriteHexString("00 14");
                p.WriteByte(m.MovementSpeed);
            }
  
            return p.GetWrittenBuffer(PacketIds.SendMonsterStopMoving);
        }

        public static byte[] SendMonsterStopMoving(BaseEntity ent , MovementInfo mi)
        {
            Pet pet = null;
            Monster m = null;
            Packet p = new Packet(200);
            if (ent is Monster)
            {
                m = ent as Monster;
                p.WriteHexString("00");
                p.WriteInt(m.MonsterID);
                p.WriteByte((byte)m.Type);
                p.WriteShort(mi.FromX);
                p.WriteShort(mi.FromY);
                p.WriteHexString("00 14");
                p.WriteByte(m.MovementSpeed);
            }
            if (ent is Pet)
            {
                pet = ent as Pet;
                p.WriteHexString("00");
                p.WriteInt(pet.PetID);
                p.WriteByte((byte)pet.Type);
                p.WriteShort(mi.FromX);
                p.WriteShort(mi.FromY);
                p.WriteHexString("00 00 03");
            }
            return p.GetWrittenBuffer(PacketIds.SendMonsterStopMoving);
        }

        public static byte[] SendMonsterStartMoving(BaseEntity ent, MonsterMoveInfo mi)
        {
            Monster m = null;
            Packet p = new Packet(200);
            if (ent is Monster)
            {
                m = ent as Monster;
                p.WriteHexString("00");
                p.WriteInt(m.MonsterID);
                p.WriteByte((byte)m.Type);
                p.WriteShort(mi.CurrentPosition.X);
                p.WriteShort(mi.CurrentPosition.Y);
                p.WriteByte(0);
                p.WriteShort(mi.Destination.X);
                p.WriteShort(mi.Destination.Y);
                p.WriteByte(0);
                p.WriteShort((short)mi.Rotation);
                p.WriteHexString("14");
                p.WriteByte(m.MovementSpeed);
            }

            return p.GetWrittenBuffer(PacketIds.SendMonsterStartMoving);
        }

        public static byte[] SendMonsterStartMoving(BaseEntity ent, MovementInfo mi)
        {
            Pet pet = null;
            Monster m = null;
            Packet p = new Packet(200);
            if (ent is Monster)
            {
                m = ent as Monster;
                p.WriteHexString("00");
                p.WriteInt(m.MonsterID);
                p.WriteByte((byte)m.Type);
                p.WriteShort(mi.FromX);
                p.WriteShort(mi.FromY);
                p.WriteByte(mi.FromZ);
                p.WriteShort(mi.ToX);
                p.WriteShort(mi.ToY);
                p.WriteByte(mi.ToZ);
                p.WriteShort(mi.Rotation);
                p.WriteHexString("14");
                p.WriteByte(m.MovementSpeed);
            }
            if (ent is Pet)
            {
                pet = ent as Pet;
                p.WriteHexString("00");
                p.WriteInt(pet.PetID);
                p.WriteByte((byte)pet.Type);
                p.WriteShort(mi.FromX);
                p.WriteShort(mi.FromY);
                p.WriteByte(mi.FromZ);
                p.WriteShort(mi.ToX);
                p.WriteShort(mi.ToY);
                p.WriteByte(mi.ToZ);
                p.WriteInt(mi.Rotation);
            }
            return p.GetWrittenBuffer(PacketIds.SendMonsterStartMoving);
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
                p.WriteShort(n.Position.X);
                p.WriteShort(n.Position.Y);
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
                        if (e is Mirror)
                        {
                            Mirror m = e as Mirror;
                            p.WriteShort(m.LifeAbsorb);
                            p.WriteShort(m.DamageAbsorb);
                            p.WriteShort(m.DefenseAbsorb);
                            p.WriteShort(m.AttackRatingAbsorb);
                        }
                    }
                }
                if (i is PotionItem)
                {
                    PotionItem pot = i as PotionItem;
                    p.WriteShort(pot.HealHp);
                    p.WriteShort(pot.HealMana);
                }
                if (i is StoreTag)
                {
                    StoreTag tag = i as StoreTag;
                    p.WriteHexString("00 00");
                    p.WriteShort(tag.TimeLeft);
                    p.WriteShort(tag.TimeMax);
                    p.WriteHexString("00 00 00 00");
                }
                if (i is PetItem)
                {
                    PetItem pet = i as PetItem;
                    p.WriteShort(pet.TameChance);
                    p.WriteHexString("00 00 00 00"); // unknown
                    p.WriteShort(pet.DecreaseWildness);
                    p.WriteShort(pet.HealLife);
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

        public static byte[] SendSellState(byte error)
        {
            Packet p = new Packet(20);
            p.WriteByte(error);
            return p.GetWrittenBuffer(PacketIds.SendSellState);
        }

        public static byte[] SendItemsLeft(byte Bag, byte Slot, int itemId, short AmountLeft)
        {
            Packet p = new Packet(200);
            p.WriteByte(Bag);
            p.WriteByte(Slot);
            p.WriteInt(itemId);
            p.WriteShort(AmountLeft);
            return p.GetWrittenBuffer(PacketIds.SendItemsLeft);
        }

        public static byte[] SendHealMana(int maxHealth, int curHealth, int maxMana, int curMana, byte effect)
        {
            Packet p = new Packet(200);
            p.WriteInt(maxHealth);
            p.WriteInt(curHealth);
            p.WriteShort(maxMana);
            p.WriteShort(curMana);
            p.WriteByte(effect);
            return p.GetWrittenBuffer(PacketIds.SendHealMana);
        }

        public static byte[] SendPlayerDie(Character ch)
        {
            Packet p = new Packet(200);
            p.WriteInt(ch.CharacterId);
            p.WriteHexString("05 16 00");
            p.WriteShort(ch.Map.SpawnX);
            p.WriteShort(ch.Map.SpawnY);
            return p.GetWrittenBuffer(PacketIds.SendPlayerDie);
        }

        public static byte[] SendPlayerActiveSkills(Character ch)
        {
            Packet p = new Packet(200);
            p.WriteHexString("00");
            p.WriteInt(ch.CharacterId);
            p.WriteByte((byte)ch.Type);
            p.WriteByte(ch.ActiveSkills.Count);
            foreach (ActiveSkill s in ch.ActiveSkills)
            {
                p.WriteInt(s.Skill.SkillID);
                p.WriteByte(s.Skill.SkillLevel);
            }
            return p.GetWrittenBuffer(PacketIds.SendPlayerActiveSkills);
        }

        public static byte[] SendSpawnPlayer(Character ch)
        {
            Packet p = new Packet(200);
            p.WriteHexString("00");
            p.WriteInt(ch.CharacterId);
            p.WriteInt(ch.MapId);
            p.WriteShort(ch.Position.X);
            p.WriteShort(ch.Position.Y);
            p.WriteHexString("76");
            p.WriteShort(ch.Position.X);
            p.WriteShort(ch.Position.Y);
            p.WriteHexString("76");
            p.WriteHexString("0B C5 00 00");
            p.WriteByte(ch.Class);
            p.WriteString(ch.Name);
            p.WriteInt(ch.Fame);
            p.WriteByte(Convert.ToByte(ch.Shop.Active));
            p.WriteString(ch.Shop.ShopName);
            p.WriteString(ch.Shop.ShopDesc);
            p.WriteHexString("00 00 00 00 00 00 00 00 00");
            p.WriteInt(ch.Party.PartyID);
            p.WriteInt(ch.Party.Leader.CharacterId);
            p.WriteHexString("00 00 00 00");
            foreach (Equipment e in ch.GetVisuals())
            {
                p.WriteShort(e.VisualID);
                p.WriteByte(e.Plus);
                p.WriteByte(e.Slvl);
                p.WriteByte(e.RequiredClass);
            }
            p.WriteHexString("00 00 00 00 00 00");

            return p.GetWrittenBuffer(PacketIds.SendSpawnPlayer);
        }

        public static byte[] SendSpawnPlayers(Character ch)
        {
            Packet p = new Packet(200);
            p.WriteHexString("00");
            p.WriteInt(ch.MapId);
            p.WriteHexString("01 00");
            p.WriteInt(ch.CharacterId);
            p.WriteShort(ch.Position.X);
            p.WriteShort(ch.Position.Y);
            p.WriteHexString("76");
            p.WriteShort(ch.Position.X);
            p.WriteShort(ch.Position.Y);
            p.WriteHexString("76");
            p.WriteHexString("16 B2 00 00");
            p.WriteByte(ch.Class);
            p.WriteString(ch.Name);
            p.WriteInt(ch.Fame);
            p.WriteByte(Convert.ToByte(ch.Shop.Active));
            p.WriteString(ch.Shop.ShopName);
            p.WriteString(ch.Shop.ShopDesc);
            p.WriteHexString("00"); // purple name
            p.WriteHexString("00"); // fe
            p.WriteHexString("00"); // fe ring
            p.WriteHexString("00 00");
            if (ch.Guild != null)
            {
                p.WriteInt(ch.Guild.ID);
                p.WriteString(ch.Guild.Name);
                GuildMember member = ch.Guild.Members.Single(x => x.Character.CharacterId == ch.CharacterId);
                p.WriteInt((int)member.Rank);
                p.WriteString(member.Title);
                p.WriteHexString("00 00 00 00"); // picture
            }
            else
                p.WriteHexString("00 00 00");
            p.WriteHexString("00");
            if (ch.Party != null)
            {
                p.WriteInt(ch.Party.PartyID);
                p.WriteInt(ch.Party.Leader.CharacterId);
            }
            p.WriteHexString("00 00 00 00");
            foreach (Equipment e in ch.GetVisuals())
            {
                p.WriteShort(e.VisualID);
                p.WriteByte(e.Plus);
                p.WriteByte(e.Slvl);
                p.WriteByte(e.RequiredClass);
            }
            p.WriteByte(ch.Rebirth); // rebirth
            p.WriteHexString("00 00 00 00 00");
            p.WriteHexString("00 00 00 00"); // title
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


            p.WriteHexString("00"); // unknown
            p.WriteByte(ch.MovingSpeed + 11);
            p.WriteInt(ch.CurrentHp);
            p.WriteInt(ch.MaxHp);
            p.WriteShort(ch.CurrentMana);
            p.WriteShort(ch.MaxMana);

            p.WriteShort(ch.Critical); // critical hit

            p.WriteShort(ch.StatDamage); // damage from stats
            p.WriteShort(ch.StatDefence); // defence from stats
            p.WriteShort(ch.StatAttackRating); // attack rating from stats

            p.WriteHexString("0C"); // unknown
            p.WriteShort(ch.AttackRange);
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

        public static byte[] SendPetBaitTaken(Monster m, Pet pet)
        {
            Packet p = new Packet(200);

            p.WriteInt(m.MonsterID);
            p.WriteByte((byte)m.Type);
            p.WriteHexString("00 00 00 00");
            p.WriteInt(pet.PetID);
            p.WriteByte((byte)pet.Type);
            p.WriteInt(pet.OwnerID);
            p.WriteString(pet.Name);
            return p.GetWrittenBuffer(PacketIds.SendPetBaitTaken);
        }

        public static byte[] SendPetRenamed(Pet pet)
        {
            Packet p = new Packet(200);
            p.WriteHexString("00");
            p.WriteInt(pet.PetID);
            p.WriteString(pet.Name);
            return p.GetWrittenBuffer(PacketIds.SendPetRenamed);
        }

        public static byte[] SendResurrectPet(Pet pet)
        {
            Packet p = new Packet(200);
            p.WriteInt(pet.OwnerID);
            p.WriteHexString("01");
            p.WriteInt(pet.PetID);
            p.WriteInt(pet.OwnerID);
            p.WriteInt(pet.MapID);
            p.WriteByte(pet.PetType);
            p.WriteString(pet.Name);
            p.WriteShort(pet.Level);
            p.WriteShort(pet.Position.X);
            p.WriteShort(pet.Position.Y);
            p.WriteHexString("00");
            p.WriteShort(pet.Position.X);
            p.WriteShort(pet.Position.Y);
            p.WriteHexString("00 FE 00");
            p.WriteInt(pet.MaxHealth);
            p.WriteInt(pet.CurHealth);
            p.WriteShort(pet.Damage);
            p.WriteShort(pet.Defense);
            p.WriteShort(pet.AttackRating);
            p.WriteHexString("01 00 09 00 00 00 00 00 83 4E 00 00 01");
            p.WriteLong(pet.CurrentExperience);
            p.WriteLong(pet.NegativeExperience);
            p.WriteLong(pet.ExperienceToLevel + pet.NegativeExperience);
            p.WriteByte(pet.Evolution);
            p.WriteByte(pet.Wildness);
            p.WriteByte(Convert.ToByte(pet.IsLegendary));
            p.WriteHexString("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");
            return p.GetWrittenBuffer(PacketIds.SendResurrectPet);
        }

        public static byte[] SendResurrectPet2(Pet pet)
        {
            Packet p = new Packet(200);
            p.WriteHexString("00");
            p.WriteInt(pet.PetID);
            p.WriteInt(pet.OwnerID);
            p.WriteInt(pet.MapID);
            p.WriteByte(pet.PetType);
            p.WriteString(pet.Name);
            p.WriteShort(pet.Level);
            p.WriteShort(pet.Position.X);
            p.WriteShort(pet.Position.Y);
            p.WriteHexString("00");
            p.WriteShort(pet.Position.X);
            p.WriteShort(pet.Position.Y);
            p.WriteHexString("00 FE 00");
            p.WriteInt(pet.MaxHealth);
            p.WriteInt(pet.CurHealth);
            p.WriteShort(pet.Damage);
            p.WriteShort(pet.Defense);
            p.WriteShort(pet.AttackRating);
            p.WriteHexString("01 00 00 00 00 00 00 00 83 4E 00 00 01");
            p.WriteLong(pet.CurrentExperience);
            p.WriteLong(pet.NegativeExperience);
            p.WriteLong(pet.ExperienceToLevel + pet.NegativeExperience);
            p.WriteByte(pet.Evolution);
            p.WriteByte(pet.Wildness);
            p.WriteByte(Convert.ToByte(pet.IsLegendary));
            p.WriteHexString("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");
            return p.GetWrittenBuffer(PacketIds.SendResurrectPet2);
        }

        public static byte[] SendLastDiedPets(List<Pet> pets)
        {
            Packet p = new Packet(200);
            p.WriteHexString("00");
            p.WriteByte(pets.Count);
            foreach (Pet pet in pets)
            {
                p.WriteInt(pet.PetID);
                p.WriteByte(pet.PetType);
                p.WriteString(pet.Name);
                p.WriteShort(pet.Level);
                p.WriteInt(pet.MaxHealth);
                p.WriteShort(pet.Damage);
                p.WriteShort(pet.Defense);
                p.WriteShort(pet.AttackRating);
                p.WriteHexString("01 00"); // ending bytes
            }
            return p.GetWrittenBuffer(PacketIds.SendLastDiedPets);
        }

        public static byte[] SendPetTamedMessage(Pet pet, PetTamedError error)
        {
            Packet p = new Packet(200);
            p.WriteByte((byte)error);
            if (error == PetTamedError.None)
            {
                p.WriteInt(pet.PetID);
                p.WriteByte((byte)pet.Type);
            }
            else
            {
                p.WriteInt(0);
                p.WriteByte(0);
            }
            return p.GetWrittenBuffer(PacketIds.SendPetTamed);
        }

        public static byte[] SendTradePetError()
        {
            Packet p = new Packet(10);
            p.WriteByte(0); // error
            return p.GetWrittenBuffer(PacketIds.SendTradePetError);
        }

        public static byte[] SendTradePetOfferError(byte error)
        {
            Packet p = new Packet(10);
            p.WriteByte((byte)error);
            return p.GetWrittenBuffer(PacketIds.SendTradePetOfferError);
        }

        public static byte[] SendTradePetOffer(Pet pet, int MoneyWanted, int targetid)
        {
            Packet p = new Packet(200);
            p.WriteHexString("00");
            p.WriteInt(pet.OwnerID);
            p.WriteInt(targetid);
            p.WriteInt(pet.PetID);
            p.WriteInt(MoneyWanted);
            p.WriteString("Pet"); // type of the pet, could be like beast, undead etc
            p.WriteString(pet.Name);
            p.WriteShort(pet.Level);
            p.WriteInt(pet.MaxHealth);
            p.WriteShort(pet.Damage);
            p.WriteShort(pet.Defense);
            p.WriteShort(pet.AttackRating);
            p.WriteHexString("01 00 00 00");
            return p.GetWrittenBuffer(PacketIds.SendTradePetOffer);
        }

        public static byte[] SendTradedPet(Pet pet)
        {
            Packet p = new Packet(200);
            p.WriteHexString("00");
            p.WriteInt(pet.PetID);
            p.WriteInt(pet.OwnerID);
            p.WriteInt(pet.MapID);
            p.WriteByte(pet.PetType);
            p.WriteString(pet.Name);
            p.WriteShort(pet.Level);
            p.WriteShort(pet.Position.X);
            p.WriteShort(pet.Position.Y);
            p.WriteHexString("00");
            p.WriteShort(pet.Position.X);
            p.WriteShort(pet.Position.Y);
            p.WriteHexString("6D 19 00");
            p.WriteInt(pet.MaxHealth);
            p.WriteInt(pet.CurHealth);
            p.WriteShort(pet.Damage);
            p.WriteShort(pet.Defense);
            p.WriteShort(pet.AttackRating);
            p.WriteHexString("01 00 00 00 00 00 00 00 83 4E 00 00 01");
            p.WriteLong(pet.CurrentExperience);
            p.WriteLong(pet.NegativeExperience);
            p.WriteLong(pet.ExperienceToLevel + pet.NegativeExperience);
            p.WriteByte(pet.Evolution);
            p.WriteByte(pet.Wildness);
            p.WriteByte(Convert.ToByte(pet.IsLegendary));
            p.WriteHexString("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");
            return p.GetWrittenBuffer(PacketIds.SendTradedPet);
        }

        public static byte[] SendRemovePet(Pet pet)
        {
            Packet p = new Packet(200);
            p.WriteByte(0);
            p.WriteInt(pet.PetID);
            p.WriteByte((byte)pet.Type);
            p.WriteInt(pet.MapID);
            return p.GetWrittenBuffer(PacketIds.SendRemovePet);
        }

        public static byte[] SendPetOtherPlayerSpawn(Pet pet)
        {
            Packet p = new Packet(200);
            p.WriteHexString("00");
            p.WriteInt(pet.MapID);
            p.WriteByte(pet.Evolution);
            p.WriteByte(pet.PetType);
            p.WriteInt(pet.PetID);
            p.WriteHexString("00");
            p.WriteInt(pet.MapID);
            p.WriteShort(pet.Position.X);
            p.WriteShort(pet.Position.Y);
            p.WriteHexString("4E 58 01");
            p.WriteString(pet.Name);
            p.WriteHexString("14 51 01 B4 00 4E 24 00 00 00 24 00 00 00 09");
            p.WriteInt(pet.OwnerID);
            p.WriteHexString("02 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");
            return p.GetWrittenBuffer(PacketIds.SendPetOtherPlayerSpawn);
        }

        public static byte[] SendPetOtherPlayerSpawn2(Pet pet)
        {
            Packet p = new Packet(200);
            p.WriteHexString("00");
            p.WriteInt(pet.PetID);
            p.WriteByte(pet.PetType);
            p.WriteInt(pet.MapID);
            p.WriteShort(pet.Position.X);
            p.WriteShort(pet.Position.Y);
            p.WriteHexString("00 00 00");
            p.WriteString(pet.Name);
            p.WriteHexString("00");
            p.WriteShort(pet.Position.X);
            p.WriteShort(pet.Position.Y);
            p.WriteHexString("00");
            p.WriteInt(pet.MaxHealth);
            p.WriteInt(pet.CurHealth);
            p.WriteHexString("00");
            p.WriteInt(pet.OwnerID);
            p.WriteHexString("00");
            p.WriteByte(Convert.ToByte(pet.IsLegendary));
            p.WriteHexString("00"); // mytholy pill :S
            p.WriteHexString("00 00"); // pet skill1
            p.WriteHexString("00 00"); // pet skill1 amount
            p.WriteHexString("00 00"); // pet skill2
            p.WriteHexString("00 00"); // pet skill2 amount
            p.WriteHexString("00"); // pet closeness
            p.WriteHexString("00 00 00 00 00 00 00 00 00 00 00 00");
        
            return p.GetWrittenBuffer(PacketIds.SendPetOtherPlayerSpawn2);
        }

        public static byte[] SendPetOtherPlayerHasPet(Pet pet)
        {
            Packet p = new Packet(200);
            p.WriteHexString("00");
            p.WriteInt(pet.MapID);
            p.WriteInt(pet.PetID);
            p.WriteByte((byte)pet.Type);
            p.WriteShort(pet.Position.X);
            p.WriteShort(pet.Position.Y);
            p.WriteHexString("4E 1D 01 00 02");
            return p.GetWrittenBuffer(PacketIds.SendPetOtherPlayerHasPet);
        }

        public static byte[] SendPetStats(Pet pet)
        {
            Packet p = new Packet(200);
            p.WriteHexString("00");
            p.WriteInt(pet.PetID);
            p.WriteInt(pet.OwnerID);
            p.WriteInt(pet.MapID);
            p.WriteByte(pet.PetType);
            p.WriteString(pet.Name);
            p.WriteShort(pet.Level);
            p.WriteShort(pet.Position.X);
            p.WriteShort(pet.Position.Y);
            p.WriteHexString("00");
            p.WriteShort(pet.Position.X);
            p.WriteShort(pet.Position.Y);
            p.WriteHexString("00 8B 00");
            p.WriteInt(pet.TotalHealth);
            p.WriteInt(pet.CurHealth);
            p.WriteShort(pet.TotalDamage);
            p.WriteShort(pet.TotalDefense);
            p.WriteShort(pet.TotalAttackRating);
            p.WriteHexString("00 00 04 04 00 00 00 00 83 4E 00 00 01");
            p.WriteLong(pet.CurrentExperience);
            p.WriteLong(pet.NegativeExperience);
            p.WriteLong(pet.ExperienceToLevel + pet.NegativeExperience);
            p.WriteByte(pet.Evolution);
            p.WriteByte(pet.Wildness);
            p.WriteByte(Convert.ToByte(pet.IsLegendary));
            p.WriteHexString("00 00 00 00 00 00 00 00 00 00");
            return p.GetWrittenBuffer(PacketIds.SendPetStats);
        }

        public static byte[] SendPlayerPet(Character ch)
        {
            Packet p = new Packet(200);
            Pet pet = ch.Pet;
            p.WriteInt(ch.CharacterId);
            if (pet != null)
            {
                p.WriteHexString("01");
                p.WriteInt(pet.PetID);
                p.WriteInt(pet.OwnerID);
                p.WriteInt(pet.MapID);
                p.WriteByte(pet.PetType);
                p.WriteString(pet.Name);
                p.WriteShort(pet.Level);
                p.WriteShort(pet.Position.X);
                p.WriteShort(pet.Position.Y);
                p.WriteHexString("38 D7 03 96 04 38 CE 00");
                p.WriteInt(pet.TotalHealth);
                p.WriteInt(pet.CurHealth);
                p.WriteShort(pet.TotalDamage);
                p.WriteShort(pet.TotalDefense);
                p.WriteShort(pet.TotalAttackRating);
                p.WriteHexString("01 00 09 00 00 00 00 00 56 2A 00 00 01 ");
                p.WriteLong(pet.CurrentExperience);
                p.WriteLong(pet.NegativeExperience);
                p.WriteLong(pet.ExperienceToLevel + pet.NegativeExperience);
                p.WriteByte(pet.Evolution);
                p.WriteByte(pet.Wildness);
                p.WriteByte(Convert.ToByte(pet.IsLegendary));
                p.WriteHexString("00 00 00 00 00 00 00 00 00 00 00 00");
                p.WriteHexString("00 00 00 00 00 00 00 00 00 00");
            }
            else
                p.WriteHexString("00"); // no pet

            return p.GetWrittenBuffer(PacketIds.SendPetOnLogin);
        }

        public static byte[] SendPetExperienceGained(Pet pet, int exp)
        {
            Packet p = new Packet(50);
            p.WriteInt(pet.PetID);
            p.WriteInt(exp);
            return p.GetWrittenBuffer(PacketIds.SendPetExperienceGained);
        }

        public static byte[] SendPetLeveled(Pet pet)
        {
            Packet p = new Packet(200);
            p.WriteInt(pet.OwnerID);
            p.WriteInt(pet.PetID);
            p.WriteByte(pet.Level);
            p.WriteShort(pet.TotalDamage);
            p.WriteShort(pet.TotalDefense);
            p.WriteShort(pet.TotalAttackRating);
            p.WriteHexString("00 00");
            p.WriteInt(pet.MaxHealth);
            p.WriteInt(pet.CurHealth);
            p.WriteLong(pet.CurrentExperience);
            p.WriteLong(pet.NegativeExperience);
            p.WriteLong(pet.ExperienceToLevel + pet.NegativeExperience);
            return p.GetWrittenBuffer(PacketIds.SendPetLeveled);
        }

        public static byte[] SendPetEvolution(Pet pet)
        {
            Packet p = new Packet(200);
            p.WriteInt(pet.OwnerID);
            p.WriteInt(pet.PetID);
            p.WriteByte(pet.PetType);
            p.WriteByte(pet.Evolution);
            p.WriteHexString("00 00 00 00 00 00 00"); // some transforming stuff like when using a pill i guess
            return p.GetWrittenBuffer(PacketIds.SendPetEvolved);
        }

        public static byte[] SendFriendOnlineStatus(Friend friend)
        {
            Packet p = new Packet(20);
            p.WriteInt(friend.FriendID);
            p.WriteHexString("02 04 00 00 00");
            p.WriteByte(Convert.ToByte(friend.FriendOnline));
            return p.GetWrittenBuffer(PacketIds.SendFriendOnlinestatus);
        }

        public static byte[] SendFriendList(List<Friend> friends)
        {
            Packet p = new Packet(200);
            p.WriteByte(friends.Count);
            foreach (Friend f in friends)
            {
                p.WriteByte((byte)f.RelationType);
                p.WriteInt(f.FriendID);
                p.WriteString(f.FriendName);
                p.WriteHexString("02 04 00 00 00");
                p.WriteByte(Convert.ToByte(f.FriendOnline));
            }
            return p.GetWrittenBuffer(PacketIds.SendFriendList);
        }

        public static byte[] SendAddFriendStatus(FriendAddTypes type, Character ch)
        {
            Packet p = new Packet(20);
            p.WriteByte((byte)type);
            p.WriteInt(ch.CharacterId);
            p.WriteString(ch.Name);
            p.WriteByte(1); // bool online probably..
            return p.GetWrittenBuffer(PacketIds.SendFriendStatus);
        }

        public static byte[] SendAddFriend(FriendAddInfo info)
        {
            Packet p = new Packet(20);
            p.WriteByte((byte)info.Type);
            p.WriteByte((byte)info.Answer);
            p.WriteInt(info.AskerID);
            p.WriteInt(info.TargetID);
            return p.GetWrittenBuffer(PacketIds.SendAddFriend);
        }

        public static byte[] SendPartyAnswer(PartyRequestInfo i)
        {
            Packet p = new Packet(20);
            p.WriteInt(i.AskerID);
            p.WriteInt(i.TargetID);
            p.WriteByte((byte)i.Error);
            p.WriteByte((byte)i.Type);

            return p.GetWrittenBuffer(PacketIds.SendPartyAnswer);
        }

        public static byte[] SendAddToPartyAnswer(AddToPartyRequestInfo i)
        {
            Packet p = new Packet(20);
            p.WriteInt(i.AskerID);
            p.WriteInt(i.TargetID);
            p.WriteByte((byte)i.Error);

            return p.GetWrittenBuffer(PacketIds.SendAddToPartyAnswer);
        }

        public static byte[] SendPartyLeader(PartyRequestInfo i)
        {
            Packet p = new Packet(20);
            p.WriteInt(i.AskerID); // partyID
            p.WriteInt(i.AskerID);
            p.WriteByte((byte)i.Type);

            return p.GetWrittenBuffer(PacketIds.SendPartyLeader);
        }

        public static byte[] SendAddPartyMember(Party party, Character member)
        {
            Packet p = new Packet(20);
            p.WriteInt(party.PartyID); // party ID
            p.WriteInt(member.CharacterId);
            p.WriteByte(member.Class);
            p.WriteString(member.Name);
            p.WriteShort(member.Level);
            p.WriteShort((short)member.Position.X);
            p.WriteShort((short)member.Position.Y);
            p.WriteInt(member.CurrentHp);
            p.WriteInt(member.MaxHp);

            return p.GetWrittenBuffer(PacketIds.SendAddPartyMember);
        }

        public static byte[] SendParty(Party party)
        {
            Packet p = new Packet(20);
            p.WriteInt(party.PartyID);
            p.WriteByte(party.MemberCount);
            p.WriteByte(1); // unknown
            foreach (Character ch in party.Members)
            {
                p.WriteInt(ch.CharacterId);
                p.WriteByte(2); // unknown
                p.WriteString(ch.Name);
                p.WriteShort(ch.Level);
                p.WriteShort(ch.Position.X);
                p.WriteShort(ch.Position.Y);
                p.WriteInt(ch.CurrentHp);
                p.WriteInt(ch.MaxHp);
                p.WriteInt(ch.MapId);
            }

            return p.GetWrittenBuffer(PacketIds.SendParty);
        }

        public static byte[] SendLeaveParty(Party party, Character ch)
        {
            Packet p = new Packet(20);
            p.WriteInt(party.PartyID);
            p.WriteInt(ch.CharacterId);
            p.WriteByte((byte)party.Type);

            return p.GetWrittenBuffer(PacketIds.SendLeaveParty);
        }

        public static byte[] SendChangePartyLeader(Party party)
        {
            Packet p = new Packet(20);
            p.WriteInt(party.PartyID);
            p.WriteInt(party.Leader.CharacterId);
            p.WriteByte((byte)party.Type);
            return p.GetWrittenBuffer(PacketIds.SendChangePartyLeader);
        }

        public static byte[] SendKillParty(Party party)
        {
            Packet p = new Packet(20);
            p.WriteInt(party.PartyID);

            return p.GetWrittenBuffer(PacketIds.SendKillParty);
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

        public static byte[] SendPetHealth(Pet pet)
        {
            Packet p = new Packet(20);
            p.WriteInt(pet.PetID);
            p.WriteInt(pet.CurHealth);
            p.WriteInt(pet.MaxHealth);
            p.WriteByte(0);
            return p.GetWrittenBuffer(PacketIds.SendPetHealth);
        }

        public static byte[] SendMonsterDie(BaseEntity entity)
        {
            Packet p = new Packet(20);
            Pet pet = null;
            Monster m = null;
            if (entity is Monster)
            {
                m = entity as Monster;
                p.WriteByte((byte)m.Type);
                p.WriteInt(m.MonsterID);
            }
            if (entity is Pet)
            {
                pet = entity as Pet;
                p.WriteByte((byte)pet.Type);
                p.WriteInt(pet.PetID);
            }
            p.WriteHexString("03 00 00 FF");
            return p.GetWrittenBuffer(PacketIds.SendMonsterDie);
        }

        public static byte[] SendExperience(Character ch, int experienceGained, short feGained, bool leveled, bool gettraining)
        {
            Packet p = new Packet(200);

            p.WriteInt(experienceGained);
            p.WriteLong(ch.CurrentExp);

            p.WriteByte(Convert.ToByte(leveled));
            p.WriteByte(Convert.ToByte(gettraining));

            p.WriteHexString("00"); // not sure
            p.WriteLong(ch.ExpToLevel + ch.NegativeExp);
            p.WriteLong(ch.ExpToTraining + ch.NegativeExp);

            p.WriteShort(ch.FiveElementPoint);
            p.WriteInt(ch.CurrentFEExp);
            p.WriteHexString("40 9C 00 00"); // fe exp to level 4000 all levels or 40k not sure too lazy check kthxbye

            p.WriteHexString("00 00 00 00"); // fe special skill current exp
            p.WriteHexString("00 00 00 00"); // bonus exp
            p.WriteShort(feGained);

            return p.GetWrittenBuffer(PacketIds.SendExperience);
        }

        public static byte[] SendExpEffectPacket(Monster m)
        {
            Packet p = new Packet(200);
            p.WriteByte((byte)m.Type);
            p.WriteInt(m.MonsterID);
            p.WriteInt(m.Experience);
            return p.GetWrittenBuffer(PacketIds.SendExpEffectPacket);
        }

        public static byte[] SendAttack1(AttackInfo i)
        {
            Packet p = new Packet(200);
            p.WriteByte(i.AttackerType);
            p.WriteInt(i.AttackerID);
            p.WriteShort(i.TargetX);
            p.WriteShort(i.TargetY);
            p.WriteByte(i.TargetZ);
            p.WriteByte(i.TargetType);
            p.WriteInt(i.TargetID);
            p.WriteHexString("00 0D");
            return p.GetWrittenBuffer(PacketIds.SendAttack1);
        }

        public static byte[] SendAttack2(AttackInfo i, BaseEntity target)
        {
            Packet p = new Packet(200);
            p.WriteByte((byte)i.Type);
            p.WriteByte(i.AttackerType);
            p.WriteInt(i.AttackerID);
            p.WriteShort(i.TargetX);
            p.WriteShort(i.TargetY);
            p.WriteByte(i.TargetZ);
            p.WriteByte(i.TargetType);
            p.WriteInt(i.TargetID);
            if (target is Monster)
            {
                Monster m = target as Monster;
                p.WriteInt(m.MaxHealth);
                p.WriteInt(m.CurHealth);
            }
            if (target is Character)
            {
                Character ch = target as Character;
                p.WriteInt(ch.MaxHp);
                p.WriteInt(ch.CurrentHp);
            }
            if (target is Pet)
            {
                Pet pet = target as Pet;
                p.WriteInt(pet.MaxHealth);
                p.WriteInt(pet.CurHealth);
            }
            p.WriteInt(i.Damage);
            p.WriteInt(i.Experience);
            p.WriteHexString("00");
            p.WriteByte(Convert.ToByte(i.Critical));
            return p.GetWrittenBuffer(PacketIds.SendAttack2);
        }

        public static byte[] SendCast1(BaseSkill skill, CastSkillInfo i, byte error)
        {
            Packet p = new Packet(500);
            p.WriteByte(error);
            p.WriteInt(skill.SkillID);
            p.WriteByte(skill.SkillLevel);
            p.WriteHexString("01");
            p.WriteInt(i.CasterID);
            p.WriteShort(i.CasterX);
            p.WriteShort(i.CasterY);
            p.WriteByte(i.CasterZ);
            p.WriteByte(i.TargetType);
            p.WriteInt(i.TargetID);
            p.WriteShort(i.TargetX);
            p.WriteShort(i.TargetY);
            p.WriteShort(i.TargetZ);
            p.WriteHexString("00 00 00");

            return p.GetWrittenBuffer(PacketIds.SendCast1);
        }

        public static byte[] SendCast2(BaseSkill skill, CastSkillInfo i, Monster m)
        {
            Packet p = new Packet(500);
            p.WriteByte((byte)i.Type);
            p.WriteInt(skill.SkillID);
            p.WriteByte(skill.SkillLevel);
            p.WriteByte(i.CasterType);
            p.WriteInt(i.CasterID);
            p.WriteShort(i.CasterX);
            p.WriteShort(i.CasterY);
            p.WriteByte(i.CasterZ);
            if (m != null)
            {
                p.WriteByte(3);
                p.WriteInt(m.MonsterID);
                p.WriteInt(m.MaxHealth);
                p.WriteInt(m.CurHealth);
                p.WriteInt(i.Damage);
                p.WriteInt(i.Experience);
                p.WriteHexString("00");
            }
            else
            {
                p.WriteByte(i.TargetType);
                p.WriteInt(i.TargetID);
                p.WriteHexString("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");
            }
            return p.GetWrittenBuffer(PacketIds.SendCast2);
        }

        public static byte[] SendSkillEffect(BaseEntity ent, BaseSkill skill)
        {
            Packet p = new Packet(20);
            p.WriteHexString("2C 35 0B 00 00");
            if (ent is Character)
            {
                Character ch = ent as Character;
                p.WriteInt(ch.CharacterId);
                p.WriteByte((byte)ch.Type);
            }
            if (ent is Monster) 
            {
                Monster m = ent as Monster;
                p.WriteInt(m.MonsterID);
                p.WriteByte((byte)m.Type);
            }
            if (ent is Pet)
            {
                Pet pet = ent as Pet;
                p.WriteInt(pet.PetID);
                p.WriteByte((byte)pet.Type);
            }
            p.WriteInt(skill.SkillID);
            p.WriteByte(skill.SkillLevel);
            return p.GetWrittenBuffer();
        }

        public static byte[] SendEndSkill(BaseEntity ent, BaseSkill skill)
        {
            Packet p = new Packet(20);
            p.WriteHexString("2E 35 0B 00 00");
            if (ent is Character)
            {
                Character ch = ent as Character;
                p.WriteInt(ch.CharacterId);
                p.WriteByte((byte)ch.Type);
            }
            if (ent is Monster)
            {
                Monster m = ent as Monster;
                p.WriteInt(m.MonsterID);
                p.WriteByte((byte)m.Type);
            }
            if (ent is Pet)
            {
                Pet pet = ent as Pet;
                p.WriteInt(pet.PetID);
                p.WriteByte((byte)pet.Type);
            }
            p.WriteInt(skill.SkillID);
            p.WriteByte(skill.SkillLevel);
            return p.GetWrittenBuffer();
        }

        public static byte[] SendGuildCreate(Guild g)
        {
            Packet p = new Packet(20);
            p.WriteHexString("00");
            p.WriteInt(g.ID);
            p.WriteString(g.Name);
            return p.GetWrittenBuffer(PacketIds.SendGuildCreate);
        }

        public static byte[] SendGuildJoin(Character c)
        {
            Packet p = new Packet(20);
            p.WriteHexString("00");
            p.WriteHexString("45 1A 00 00"); // guild id
            p.WriteHexString("01 00 00 00");
            p.WriteInt(c.CharacterId);
            p.WriteString(c.Name);
            return p.GetWrittenBuffer(PacketIds.SendGuildJoin);
        }

        public static byte[] SendGuildMember(Guild g, GuildMember m)
        {
            Packet p = new Packet(20);
            p.WriteInt(m.Character.CharacterId);
            p.WriteInt(g.ID);
            p.WriteString(g.Name);
            p.WriteInt((int)m.Rank);
            p.WriteString(m.Title);
            p.WriteString(m.Character.Name); // owner name
            p.WriteHexString("00 00 00 00 00");
            p.WriteHexString("02 00 00 00");
            p.WriteHexString("00 00 00 00"); // more stats?
            p.WriteHexString("00 00 00 00"); // guild mark
            return p.GetWrittenBuffer(PacketIds.SendGuildOwner);
        }

        public static byte[] SendGuildNotice(Guild g)
        {
            Packet p = new Packet(20);
            p.WriteInt(g.Leader.Character.CharacterId);
            p.WriteHexString("09");
            p.WriteString(g.Notice);
            return p.GetWrittenBuffer(PacketIds.SendGuildNotice);
        }

        public static byte[] SendGuildChat(Character ch, string message)
        {
            Packet p = new Packet(20);
            p.WriteInt(ch.CharacterId);
            p.WriteHexString("06");
            p.WriteString(message);
            return p.GetWrittenBuffer(PacketIds.SendGuildNotice);
        }

        public static byte[] SendGuild(Guild g)
        {
            Packet p = new Packet(200);
            p.WriteInt(g.ID);
            p.WriteString(g.Name);
            p.WriteInt(g.Members.Count);

            foreach (GuildMember c in g.Members)
            {
                p.WriteInt((int)c.Rank);
                p.WriteInt(c.Character.CharacterId);
                p.WriteString(c.Character.Name);
                p.WriteString(c.Title);
                p.WriteByte(Convert.ToByte(c.Online));
                p.WriteHexString("01");
            }

            return p.GetWrittenBuffer(PacketIds.SendGuild);
        }

        public static byte[] SendGuildMemberInfo(GuildMember g)
        {
            Packet p = new Packet(200);
            p.WriteInt(g.Character.CharacterId);
            p.WriteString(g.Character.Name);
            p.WriteInt((int)g.Rank);
            p.WriteString(g.Title);
            p.WriteByte(Convert.ToByte(g.Online));
            p.WriteByte(g.Character.Level);
            p.WriteHexString("00"); // some stats
            p.WriteByte(g.Character.Class);
            return p.GetWrittenBuffer(PacketIds.SendGuildMemberInfo);
        }

        public static byte[] SendGuildStats(Guild g)
        {
            Packet p = new Packet(200);
            p.WriteHexString("00");
            p.WriteInt(g.ID);
            p.WriteString(g.Name);
            p.WriteInt(g.Leader.Character.CharacterId);
            p.WriteString(g.Leader.Character.Name);
            p.WriteHexString("00 00 00 00 00 00 00 00 00 00 00 00 00 14 00 00 00 00 00 00 00 00 02 00 00 00 02 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 02 00 00 00 02 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 04 05 00 00 00 00");
            return p.GetWrittenBuffer(PacketIds.SendGuildStats);
        }

        public static byte[] SendGuildChangeMemberRank(GuildMember m, GuildMemberError e)
        {
            Packet p = new Packet(20);
            p.WriteByte((byte)e);
            if (e == GuildMemberError.Success)
            {
                p.WriteInt(m.Character.CharacterId);
                p.WriteInt((int)m.Rank);
            }
            else
            {
                p.WriteInt(0);
                p.WriteInt(0);
            }
            return p.GetWrittenBuffer(PacketIds.SendGuildChangeMemberRank);
        }

        public static byte[] SendGuildChangeMemberTitle(Guild g, GuildMember m, GuildMemberError e)
        {
            Packet p = new Packet(20);
            p.WriteByte((byte)e); // error proly
            if (e == GuildMemberError.Success)
            {
                p.WriteInt(g.ID);
                p.WriteInt(m.Character.CharacterId);
                p.WriteString(m.Title);
            }
            else
            {
                p.WriteInt(0);
                p.WriteInt(0);
                p.WriteString("");
            }
            return p.GetWrittenBuffer(PacketIds.SendGuildChangeMemberTitle);
        }

        public static byte[] SendGuildChangeNoticeError(GuildNoticeError error)
        {
            Packet p = new Packet(20);
            p.WriteByte((byte)error);
            return p.GetWrittenBuffer(PacketIds.SendGuildChangeNoticeError);
        }

        public static byte[] SendGuildJoinAnswer(RequestJoinGuildInfo answer)
        {
            Packet p = new Packet(20);
            p.WriteByte((byte)answer.Answer);
            p.WriteInt(answer.AskerID);
            p.WriteInt(answer.TargetID);
            return p.GetWrittenBuffer(PacketIds.SendJoinGuildAnswer);
        }

        public static byte[] SendCloseGuild()
        {
            Packet p = new Packet(20);
            p.WriteHexString("00");
            return p.GetWrittenBuffer(PacketIds.SendCloseGuild);
        }

        public static byte[] SendLeaveGuild(int ch, Guild g)
        {
            Packet p = new Packet(20);
            p.WriteHexString("00");
            p.WriteInt(g.ID);
            p.WriteHexString("00 00 00 00"); // unknown
            p.WriteInt(ch);
            return p.GetWrittenBuffer(PacketIds.SendLeaveGuild);
        }

        public static byte[] SendGuildTown(GuildTown Town)
        {
            Packet p = new Packet(200);
            p.WriteByte(0);
            p.WriteInt(Town.MapID); // map id prolly
            p.WriteHexString("01 00"); // status, 1 = free, rest sold?
            p.WriteInt(Town.ID); // guild townID
            p.WriteString(Town.Name); // name
            p.WriteByte(Town.Grade); // town level/grade
            p.WriteShort(Town.Position.X); // x
            p.WriteShort(Town.Position.Y); // y
            p.WriteHexString("00 00 00"); // something
            p.WriteInt(Town.CurDura); // dura
            p.WriteInt(Town.MaxDura); // dura
            p.WriteInt(Town.OwnerID); // owner guildID
            return p.GetWrittenBuffer(PacketIds.SendGuildTown);
        }

        public static byte[] SendQuestList(Quest q)
        {
            Packet p = new Packet(20);
            p.WriteHexString("80 00 00 00"); // player fame
            p.WriteHexString("01"); // total quests
            p.WriteInt(q.ID);
            p.WriteByte((byte)q.State); // status, paused, started etc
            p.WriteHexString("00"); // maybe times finished..
            p.WriteByte(q.CurrentObjective);
            p.WriteByte(q.Objectives.Count);
            p.WriteByte(q.Rewards.Count);
            for (int i = 0; i < q.Objectives.Count; i++)
            {
                QuestObjectState state = QuestObjectState.Hidden;
                if (q.CurrentObjective >= q.Objectives[i].AtObjective)
                {
                    if (q.Objectives[i].CurrentAmount == q.Objectives[i].AmountRequired)
                        state = QuestObjectState.Obtained;
                    else
                        state = QuestObjectState.NotObtained;
                }
                q.Objectives[i].State = state;
                p.WriteByte((byte)q.Objectives[i].State); // object state acquired, hidden etc.
                p.WriteByte(q.Objectives[i].AtObjective);
                p.WriteByte((byte)q.Objectives[i].Type); // object type..
                p.WriteInt(q.Objectives[i].ID);
                p.WriteInt(q.Objectives[i].CurrentAmount);
                p.WriteInt(q.Objectives[i].AmountRequired);
            }
            for (int i = 0; i < q.Rewards.Count; i++)
            {
                QuestObjectState state = QuestObjectState.Hidden;
                if (q.CurrentObjective >= q.Rewards[i].AtObjective)
                    state = QuestObjectState.NotObtained;
                q.Rewards[i].State = state;
                p.WriteByte((byte)q.Rewards[i].State);

                p.WriteByte(q.Rewards[i].AtObjective);
                p.WriteByte((byte)q.Rewards[i].Type);
                if (q.Rewards[i].Type == QuestRewardType.Item || q.Rewards[i].Type == QuestRewardType.PlusItem)
                {
                    p.WriteInt(q.Rewards[i].ID);
                    p.WriteInt(q.Rewards[i].Amount);
                }
                else
                {
                    p.WriteInt(q.Rewards[i].Amount);
                    p.WriteInt(0); //unknown.
                }
            }
            return p.GetWrittenBuffer(PacketIds.SendQuestList);
        }

        public static byte[] SendRebirth(RebirthError error, Character ch)
        {
            Packet p = new Packet(200);
            p.WriteByte((byte)error);
            if (error == RebirthError.Success)
            {
                p.WriteInt(ch.CharacterId);
                p.WriteByte(ch.Rebirth);
            }
            return p.GetWrittenBuffer(PacketIds.SendRebirth);
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

        public static OpenWarehouseInfo RecvOpenWarehouse(Packet p)
        {
            p.Skip(2);
            int npcid = p.ReadInt();

            OpenWarehouseInfo i = new OpenWarehouseInfo
            {
                NpcID = npcid
            };

            return i;
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

        public static PetRequestInfo RecvPlayerPetRequest(Packet p)
        {
            p.Skip(2);
            int charid = p.ReadInt();

            PetRequestInfo sr = new PetRequestInfo
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

        public static StackItemInfo RecvStackItem(Packet p)
        {
            p.Skip(2);
            byte bag, slot, stackbag, stackslot;
            int itemid, stackitemid;

            bag = p.ReadByte();
            slot = p.ReadByte();
            itemid = p.ReadInt();
            stackbag = p.ReadByte();
            stackslot = p.ReadByte();
            stackitemid = p.ReadInt();

            StackItemInfo i = new StackItemInfo
            {
                Bag = bag,
                Slot = slot,
                ItemID = itemid,
                StackBag = stackbag,
                StackSlot = stackslot,
                StackItemID = stackitemid
            };

            return i;
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
            p.Skip(3);
            byte slot = p.ReadByte();
            byte bag = p.ReadByte();

            BuyItemInfo i = new BuyItemInfo
            {
                NpcID = npcid,
                ReferenceID = wref,
                Amount = amount,
                Slot = slot,
                Bag = bag
            };

            return i;
        }

        public static SellItemInfo RecvSellItem(Packet p)
        {
            p.Skip(2);
            int npcid = p.ReadInt();
            int itemid = p.ReadInt();
            byte bag = p.ReadByte();
            byte slot = p.ReadByte();

            SellItemInfo i = new SellItemInfo
            {
                NpcID = npcid,
                ItemID = itemid,
                Bag = bag,
                Slot = slot
            };

            return i;
        }

        public static UseItemInfo RecvUseItem(Packet p)
        {
            p.Skip(2);
            byte bag = p.ReadByte();
            byte slot = p.ReadByte();
            int itemId = p.ReadInt();

            UseItemInfo i = new UseItemInfo
            {
                Bag = bag,
                Slot = slot,
                ItemID = itemId
            };

            return i;
        }

        public static UseRebirthPillInfo RecvUseRebirthPill(Packet p)
        {
            p.Skip(2);
            byte bag = p.ReadByte();
            byte slot = p.ReadByte();
            int itemId = p.ReadInt();

            UseRebirthPillInfo i = new UseRebirthPillInfo
            {
                Bag = bag,
                Slot = slot,
                ItemID = itemId
            };

            return i;
        }

        public static UpdateQuickSlotInfo RecvUpdateQuickSlot(Packet p)
        {
            p.Skip(2);
            short valueId = p.ReadShort();
            p.Skip(2);
            byte slot = p.ReadByte();
            byte page = p.ReadByte();

            UpdateQuickSlotInfo i = new UpdateQuickSlotInfo
            {
                ValueID = valueId,
                Slot = slot,
                Page = page
            };

            return i;
        }

        public static AddItemToWarehouseInfo RecvAddItemToWarehouse(Packet p)
        {
            p.Skip(2);
            int charid = p.ReadInt();
            int itemid = p.ReadInt();
            byte frombag = p.ReadByte();
            byte fromslot = p.ReadByte();
            byte toslot = p.ReadByte();
            byte tobag = p.ReadByte();

            AddItemToWarehouseInfo i = new AddItemToWarehouseInfo
            {
                CharacterID = charid,
                ItemID = itemid,
                FromBag = frombag,
                FromSlot = fromslot,
                ToBag = tobag,
                ToSlot = toslot
            };

            return i;
        }

        public static MoveWarehouseItemToBagInfo RecvMoveWarehouseItemToBag(Packet p)
        {
            p.Skip(2);
            int charid = p.ReadInt();
            int itemid = p.ReadInt();
            byte fromslot = p.ReadByte();
            byte tobag = p.ReadByte();
            byte toslot = p.ReadByte();

            MoveWarehouseItemToBagInfo i = new MoveWarehouseItemToBagInfo
            {
                CharacterID = charid,
                ItemID = itemid,
                FromSlot = fromslot,
                ToBag = tobag,
                ToSlot = toslot
            };

            return i;
        }

        public static MoveWarehouseItemInfo RecvMoveWarehouseItem(Packet p)
        {
            p.Skip(2);
            byte fromslot = p.ReadByte();
            int itemid = p.ReadInt();
            byte toslot = p.ReadByte();
            int itemunderid = p.ReadInt();

            MoveWarehouseItemInfo i = new MoveWarehouseItemInfo
            {
                FromSlot = fromslot,
                ItemID = itemid,
                ToSlot = toslot,
                ItemUnderID = itemunderid
            };

            return i;
        }

        public static AddItemToShopInfo RecvAddItemToShop(Packet p)
        {
            p.Skip(2);
            byte frombag = p.ReadByte();
            byte fromslot = p.ReadByte();
            int itemid = p.ReadInt();
            byte slot = p.ReadByte();
            int price = p.ReadInt();

            AddItemToShopInfo i = new AddItemToShopInfo
            {
                FromBag = frombag,
                FromSlot = fromslot,
                ItemID = itemid,
                Slot = slot,
                Price = price
            };

            return i;
        }

        public static MoveShopItemInfo RecvMoveShopItem(Packet p)
        {
            p.Skip(2);
            byte fromslot = p.ReadByte();
            int itemid = p.ReadInt();
            byte toslot = p.ReadByte();
            int itemunderid = p.ReadInt();

            MoveShopItemInfo i = new MoveShopItemInfo
            {
                FromSlot = fromslot,
                ItemID = itemid,
                ToSlot = toslot,
                ItemUnderID = itemunderid
            };

            return i;
        }

        public static ChangeShopInfo RecvChangeShopInfo(Packet p)
        {
            p.Skip(2);
            string name = p.ReadString();
            string desc = p.ReadString();

            ChangeShopInfo i = new ChangeShopInfo
            {
                Name = name,
                Description = desc
            };

            return i;
        }

        public static MoveShopItemToBagInfo RecvMoveShopItemToBag(Packet p)
        {
            p.Skip(2);
            byte fromslot = p.ReadByte();
            int itemid = p.ReadInt();
            byte tobag = p.ReadByte();
            byte toslot = p.ReadByte();

            MoveShopItemToBagInfo i = new MoveShopItemToBagInfo
            {
                FromSlot = fromslot,
                ItemID = itemid,
                ToBag = tobag,
                ToSlot = toslot
            };

            return i;
        }

        public static ShopStartSellingInfo RecvShopStartSelling(Packet p)
        {
            p.Skip(2);
            byte selling = p.ReadByte();

            ShopStartSellingInfo i = new ShopStartSellingInfo
            {
                Selling = Convert.ToBoolean(selling)
            };

            return i;
        }

        public static OpenOtherPlayerShopInfo RecvOpenOtherPlayerShop(Packet p)
        {
            p.Skip(2);
            int playerid = p.ReadInt();

            OpenOtherPlayerShopInfo i = new OpenOtherPlayerShopInfo
            {
                CharacterID = playerid
            };

            return i;
        }

        public static BuyOtherPlayerShopItem RecvBuyOtherPlayerShopItem(Packet p)
        {
            p.Skip(2);
            int playerid = p.ReadInt();
            byte fromslot = p.ReadByte();
            int itemid = p.ReadInt();
            byte tobag = p.ReadByte();
            byte toslot = p.ReadByte();
            int price = p.ReadInt();

            BuyOtherPlayerShopItem i = new BuyOtherPlayerShopItem
            {
                CharacterID = playerid,
                FromSlot = fromslot,
                ItemID = itemid,
                ToBag = tobag,
                ToSlot = toslot,
                Price = price
            };

            return i;
        }

        public static ShopTakeMoneyInfo RecvShopTakeMoney(Packet p)
        {
            p.Skip(2);
            int amount = p.ReadInt();

            ShopTakeMoneyInfo i = new ShopTakeMoneyInfo
            {
                Amount = amount
            };

            return i;
        }

        public static TradePlayerInfo RecvTradePlayer(Packet p)
        {
            p.Skip(2);
            byte error = p.ReadByte();
            int player1 = p.ReadInt();
            int player2 = p.ReadInt();

            TradePlayerInfo i = new TradePlayerInfo
            {
                Error = (TradePlayerError)error,
                CharacterID = player1,
                CharacterID2 = player2
            };

            return i;
        }

        public static TradePlayerCommandsInfo RecvTradePlayerCommands(Packet p)
        {
            p.Skip(2);
            byte error = p.ReadByte();
            int player1 = p.ReadInt();

            TradePlayerCommandsInfo i = new TradePlayerCommandsInfo
            {
                Error = (TradePlayerCommands)error,
                CharacterID = player1
            };

            return i;
        }

        public static TradeAddMoneyInfo RecvTradeAddMoney(Packet p)
        {
            p.Skip(2);
            int player1 = p.ReadInt();
            int amount = p.ReadInt();

            TradeAddMoneyInfo i = new TradeAddMoneyInfo
            {
                PlayerID = player1,
                Amount = amount
            };

            return i;
        }

        public static TradeAddItemInfo RecvTradeAdditem(Packet p)
        {
            p.Skip(2);
            byte frombag = p.ReadByte();
            byte fromslot = p.ReadByte();
            byte tobag = p.ReadByte();
            byte toslot = p.ReadByte();
            int itemid = p.ReadInt();
            short amount = p.ReadShort();
            p.Skip(2);
            int charid = p.ReadInt();

            TradeAddItemInfo i = new TradeAddItemInfo
            {
                FromBag = frombag,
                FromSlot = fromslot,
                ToBag = tobag,
                ToSlot = toslot,
                ItemID = itemid,
                Amount = amount,
                PlayerID = charid
            };

            return i;
        }

        public static LearnSkillInfo RecvLearnSkill(Packet p)
        {
            p.Skip(2);
            int skillid = p.ReadInt();

            LearnSkillInfo i = new LearnSkillInfo
            {
                SkillID = skillid
            };

            return i;
        }

        public static FriendAddInfo RecvFriendAdd(Packet p)
        {
            p.Skip(2);
            byte type = p.ReadByte();
            byte answer = p.ReadByte();
            int askerid = p.ReadInt();
            int targetid = p.ReadInt();

            FriendAddInfo i = new FriendAddInfo
            {
                Type = (FriendAddTypes)type,
                Answer = (FriendAddAnswers)answer,
                AskerID = askerid,
                TargetID = targetid
            };

            return i;
        }

        public static MonsterSpawnRequestInfo RecvMonsterSpawnRequest(Packet p)
        {
            p.Skip(2);
            int monsterId = p.ReadInt();
            int mapId = p.ReadInt();

            MonsterSpawnRequestInfo i = new MonsterSpawnRequestInfo
            {
                MonsterId = monsterId,
                MapId = mapId
            };

            return i;
        }

        public static AttackInfo RecvAttack1(Packet p)
        {
            p.Skip(2);
            byte attackertype = p.ReadByte();
            int attackerid = p.ReadInt();
            short targetx = p.ReadShort();
            short targety = p.ReadShort();
            byte targetz = p.ReadByte();
            byte targettype = p.ReadByte();
            int targetid = p.ReadInt();

            AttackInfo i = new AttackInfo
            {
                PacketID = 1,
                AttackerType = attackertype,
                AttackerID = attackerid,
                TargetX = targetx,
                TargetY = targety,
                TargetZ = targetz,
                TargetType = targettype,
                TargetID = targetid
            };

            return i;
        }


        public static AttackInfo RecvAttack2(Packet p)
        {
            p.Skip(2);
            byte attackertype = p.ReadByte();
            int attackerid = p.ReadInt();
            short targetx = p.ReadShort();
            short targety = p.ReadShort();
            byte targetz = p.ReadByte();
            byte targettype = p.ReadByte();
            int targetid = p.ReadInt();

            AttackInfo i = new AttackInfo
            {
                PacketID = 2,
                AttackerType = attackertype,
                AttackerID = attackerid,
                TargetX = targetx,
                TargetY = targety,
                TargetZ = targetz,
                TargetType = targettype,
                TargetID = targetid
            };

            return i;
        }

        public static CastSkillInfo RecvCastSkill1(Packet p)
        {
            p.Skip(2);
            int skillid = p.ReadInt();
            byte castertype = p.ReadByte();
            int casterid = p.ReadInt();
            short casterx = p.ReadShort();
            short castery = p.ReadShort();
            byte casterz = p.ReadByte();
            byte targettype = p.ReadByte();
            int targetid = p.ReadInt();
            short targetx = p.ReadShort();
            short targety = p.ReadShort();
            byte targetz = p.ReadByte();

            CastSkillInfo i = new CastSkillInfo
            {
                PacketID = 1,
                SkillID = skillid,
                CasterType = castertype,
                CasterID = casterid,
                CasterX = casterx,
                CasterY = castery,
                CasterZ = casterz,
                TargetType = targettype,
                TargetID = targetid,
                TargetX = targetx,
                TargetY = targety,
                TargetZ = targetz
            };

            return i;
        }

        public static CastSkillInfo RecvCastSkill2(Packet p)
        {
            p.Skip(2);
            int skillid = p.ReadInt();
            byte castertype = p.ReadByte();
            int casterid = p.ReadInt();
            short casterx = p.ReadShort();
            short castery = p.ReadShort();
            byte casterz = p.ReadByte();
            byte targettype = p.ReadByte();
            int targetid = p.ReadInt();
            short targetx = p.ReadShort();
            short targety = p.ReadShort();
            byte targetz = p.ReadByte();

            CastSkillInfo i = new CastSkillInfo
            {
                PacketID = 2,
                SkillID = skillid,
                CasterType = castertype,
                CasterID = casterid,
                CasterX = casterx,
                CasterY = castery,
                CasterZ = casterz,
                TargetType = targettype,
                TargetID = targetid,
                TargetX = targetx,
                TargetY = targety,
                TargetZ = targetz
            };

            return i;
        }

        public static SelectSkillInfo RecvSelectSkill(Packet p)
        {
            p.Skip(2);
            byte row = p.ReadByte();
            int skillid = p.ReadInt();
            byte slot = p.ReadByte();

            SelectSkillInfo i = new SelectSkillInfo
            {
                Row = row,
                SkillID = skillid,
                Slot = slot
            };

            return i;
        }

        public static UseTeleporterInfo RecvUseTeleporter(Packet p)
        {
            p.Skip(2);
            int id = p.ReadInt();
            int map = p.ReadInt();

            UseTeleporterInfo i = new UseTeleporterInfo
            {
                NpcID = id,
                ToMap = map
            };

            return i;
        }

        public static CombineItemsInfo RecvCombineItems(Packet p)
        {
            p.Skip(2);
            byte something = p.ReadByte();
            byte something2 = p.ReadByte();
            int itemId = p.ReadInt();
            byte bag = p.ReadByte();
            byte slot = p.ReadByte();

            CombineItemsInfo i = new CombineItemsInfo
            {
                ItemID = itemId,
                Bag = bag,
                Slot = slot
            };

            return i;
        }

        public static PartyRequestInfo RecvPartyRequest(Packet p)
        {
            p.Skip(2);
            int asker = p.ReadInt();
            int target = p.ReadInt();
            PartyError error = (PartyError)p.ReadByte();
            PartyType type = (PartyType)p.ReadByte();

            PartyRequestInfo i = new PartyRequestInfo
            {
                AskerID = asker,
                TargetID = target,
                Error = error,
                Type = type
            };

            return i;
        }

        public static AddToPartyRequestInfo RecvAddToPartyRequest(Packet p)
        {
            p.Skip(2);
            int asker = p.ReadInt();
            int target = p.ReadInt();
            PartyError error = (PartyError)p.ReadByte();
            PartyType type = (PartyType)p.ReadByte();

            AddToPartyRequestInfo i = new AddToPartyRequestInfo
            {
                AskerID = asker,
                TargetID = target,
                Error = error,
                Type = type
            };

            return i;
        }

        public static LeavePartyInfo RecvLeaveParty(Packet p)
        {
            p.Skip(2);
            int partyid = p.ReadInt();

            LeavePartyInfo i = new LeavePartyInfo
            {
                PartyID = partyid
            };

            return i;
        }

        public static ChangePartyLeaderInfo RecvChangePartyLeader(Packet p)
        {
            p.Skip(2);
            int oldLeader = p.ReadInt();
            int newLeader = p.ReadInt();

            ChangePartyLeaderInfo i = new ChangePartyLeaderInfo
            {
                OldLeader = oldLeader,
                NewLeader = newLeader
            };

            return i;
        }

        public static MovementInfo RecvPetStartMoving(Packet p)
        {
            p.Skip(2);
            int charid = p.ReadInt();

            short from_x = p.ReadShort();
            short from_y = p.ReadShort();
            byte from_z = p.ReadByte();

            short to_x = p.ReadShort();
            short to_y = p.ReadShort();
            byte to_z = p.ReadByte();
            int direction = p.ReadInt();

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

        public static MovementInfo RecvPetKeepMoving(Packet p)
        {
            p.Skip(2);
            int charid = p.ReadInt();

            short from_x = p.ReadShort();
            short from_y = p.ReadShort();
            byte from_z = p.ReadByte();

            short to_x = p.ReadShort();
            short to_y = p.ReadShort();
            byte to_z = p.ReadByte();
            int direction = p.ReadInt();

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

        public static MovementInfo RecvPetStopMoving(Packet p)
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

        public static KillPet RecvKillPet(Packet p)
        {
            p.Skip(2);
            byte type = p.ReadByte();
            int id = p.ReadInt();
            byte action = p.ReadByte();

            KillPet i = new KillPet
            {
                Type = type,
                PetID = id,
                Action = action
            };

            return i;
        }

        public static BaitPetInfo RecvBaitPet(Packet p)
        {
            p.Skip(2);
            byte bag = p.ReadByte();
            byte slot = p.ReadByte();
            int itemid = p.ReadInt();
            byte monstertype = p.ReadByte();
            int monsterid = p.ReadInt();

            BaitPetInfo i = new BaitPetInfo
            {
                Bag = bag,
                Slot = slot,
                ItemID = itemid,
                MonsterType = monstertype,
                MonsterID = monsterid
            };

            return i;
        }

        public static RequestPetStats RecvPetStatRequest(Packet p)
        {
            p.Skip(2);
            int mapid = p.ReadInt();
            int petid = p.ReadInt();

            RequestPetStats i = new RequestPetStats
            {
                MapID = mapid,
                PetID = petid
            };

            return i;
        }

        public static RequestSpawnOtherPet RecvSpawnOtherPet(Packet p)
        {
            p.Skip(2);
            int petid = p.ReadInt();
            int mapid = p.ReadInt();

            RequestSpawnOtherPet i = new RequestSpawnOtherPet
            {
                MapID = mapid,
                PetID = petid
            };

            return i;
        }

        public static ResurrectPetInfo RecvResurrectPet(Packet p)
        {
            p.Skip(2);
            int petid = p.ReadInt();
            byte bag = p.ReadByte();
            byte slot = p.ReadByte();

            ResurrectPetInfo i = new ResurrectPetInfo
            {
                PetID = petid,
                Bag = bag,
                Slot = slot
            };

            return i;
        }

        public static RenamePetInfo RecvRenamePet(Packet p)
        {
            p.Skip(2);
            int mapid = p.ReadInt();
            int petid = p.ReadInt();
            string newname = p.ReadString();

            RenamePetInfo i = new RenamePetInfo
            {
                MapID = mapid,
                PetID = petid,
                NewName = newname
            };

            return i;
        }

        public static SendPetToMeInfo RecvSendPetToMe(Packet p)
        {
            p.Skip(2);
            int petid = p.ReadInt();
            int mapid = p.ReadInt();

            SendPetToMeInfo i = new SendPetToMeInfo
            {
                PetID = petid,
                MapID = mapid
            };

            return i;
        }

        public static TradePetInfo RecvTradePet(Packet p)
        {
            p.Skip(2);
            int targetid = p.ReadInt();
            int petid = p.ReadInt();

            TradePetInfo i = new TradePetInfo
            {
                TargetID = targetid,
                PetID = petid
            };

            return i;
        }

        public static TradePetAmountInfo RecvTradePetAmount(Packet p)
        {
            p.Skip(2);
            PetTradeAction action = (PetTradeAction)p.ReadByte();
            int ownerid = p.ReadInt();
            int targetid = p.ReadInt();
            int petid = p.ReadInt();
            int amount = p.ReadInt();

            TradePetAmountInfo i = new TradePetAmountInfo
            {
                Action = action,
                OwnerID = ownerid,
                TargetID = targetid,
                PetID = petid,
                MoneyWanted = amount
            };

            return i;
        }

        public static SealPetInfo RecvSealPet(Packet p)
        {
            p.Skip(2);
            int petid = p.ReadInt();
            byte bag = p.ReadByte();
            byte slot = p.ReadByte();

            SealPetInfo i = new SealPetInfo
            {
                PetID = petid,
                Bag = bag,
                Slot = slot
            };

            return i;
        }

        public static UnSealPetInfo RecvUnSealPet(Packet p)
        {
            p.Skip(2);
            byte bag = p.ReadByte();
            byte slot = p.ReadByte();

            UnSealPetInfo i = new UnSealPetInfo
            {
                Bag = bag,
                Slot = slot
            };

            return i;
        }

        public static CreateGuildInfo RecvCreateGuild(Packet p)
        {
            p.Skip(2);
            string GuildName = p.ReadString();

            CreateGuildInfo i = new CreateGuildInfo
            {
                GuildName = GuildName
            };

            return i;
        }

        public static RequestGuildInfo RecvRequestGuildInfo(Packet p)
        {
            p.Skip(3);
            int guildID = p.ReadInt();

            RequestGuildInfo i = new RequestGuildInfo
            {
                GuildID = guildID
            };

            return i;
        }

        public static RequestGuildMemberStatsInfo RecvRequestGuildMemberStats(Packet p)
        {
            p.Skip(2);
            int guildMemberID = p.ReadInt();

            RequestGuildMemberStatsInfo i = new RequestGuildMemberStatsInfo
            {
                MemberID = guildMemberID
            };

            return i;
        }

        public static RequestGuildMemberChangeRankInfo RecvRequestChangeGuildMemberRank(Packet p)
        {
            p.Skip(2);
            int memberId = p.ReadInt();
            GuildOrder oldRank = (GuildOrder)p.ReadInt();
            GuildOrder newRank = (GuildOrder)p.ReadInt();

            RequestGuildMemberChangeRankInfo i = new RequestGuildMemberChangeRankInfo
            {
                MemberID = memberId,
                OldRank = oldRank,
                NewRank = newRank
            };

            return i;
        }

        public static RequestGuildMemberChangeTitleInfo RecvRequestChangeGuildMemberTitle(Packet p)
        {
            p.Skip(2);
            int guildID = p.ReadInt();
            int memberID = p.ReadInt();
            string newTitle = p.ReadString();

            RequestGuildMemberChangeTitleInfo i = new RequestGuildMemberChangeTitleInfo
            {
                GuildID = guildID,
                MemberID = memberID,
                NewTitle = newTitle
            };

            return i;
        }

        public static RequestGuildChangeNoticeInfo RecvRequestChangeGuildNotice(Packet p)
        {
            p.Skip(2);
            string newNotice = p.ReadString();

            RequestGuildChangeNoticeInfo i = new RequestGuildChangeNoticeInfo
            {
                NewNotice = newNotice
            };

            return i;
        }

        public static RequestJoinGuildInfo RecvRequestJoinGuild(Packet p)
        {
            p.Skip(2);
            GuildJoinAnswer answer = (GuildJoinAnswer)p.ReadByte();
            int joiner = p.ReadInt();
            int target = p.ReadInt();

            RequestJoinGuildInfo i = new RequestJoinGuildInfo
            {
                Answer = answer,
                AskerID = joiner,
                TargetID = target
            };

            return i;
        }

        public static RequestLeaveGuildInfo RecvRequestLeaveGuild(Packet p)
        {
            p.Skip(2);
            int playerid = p.ReadInt();
            int something = p.ReadInt();

            RequestLeaveGuildInfo i = new RequestLeaveGuildInfo
            {
                PlayerID = playerid,
                Something = something
            };

            return i;
        }

        public static GuildChatInfo RecvGuildChat(Packet p)
        {
            p.Skip(2);
            byte type = p.ReadByte();
            string message = p.ReadString();

            GuildChatInfo i = new GuildChatInfo
            {
                Type = type,
                Message = message
            };

            return i;
        }

        #endregion

        #endregion
    }
}