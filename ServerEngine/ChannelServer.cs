using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XiahBLL;
using Entities;
using ServerEngine.PacketEngine;
using System.Configuration;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ServerEngine
{
    public class ChannelServer : ServerBase
    {
        private int ExperienceMultiper = 1;
        private int ItemDropChance = 15;
        private int MoneyDropChance = 20;
        private int StoneDropChance = 20;
        private int BookDropChance = 5;
        private int PillDropChance = 5;
        private int StoneCombineChance = 50;

        private ChannelInfo[] channelInfos;
        private ChannelManager channelManager;

        private GameEngine gameEngine;
        //private MapEngine mapEngine;
        private List<MapEngine> mapEngines;
        private AccountManager accountManager;
        private CharacterManager characterManager;
        private ItemDataManager itemDataManager;
        private PetManager petManager;
        private MapManager mapManager;
        private GuildManager guildManager;

        #region Base Events
        public event EventHandler<ClientUnitConnectionInfoEventArgs> ClientUnitConnection;
        public event EventHandler<UnknownPacketEventArgs> ClientUnknownPacket;
        public event EventHandler<LogPacketEventArgs> ClientLogPacket;
        public event EventHandler<PlayerConnectionChangedEventArgs> ClientConnectionChanged;
        #endregion

        //#region Entity Threads
        //public void StartMonsters()
        //{
        //    EntityThread MobThread = new EntityThread();
        //    MobThread.Execute += new Execute(MobThread_Execute);
        //    MobThread.Start(100);

        //}

        public void StartRegenThread()
        {
            EntityThread RegenThread = new EntityThread();
            RegenThread.Execute += new Execute(RegenThread_Execute);
            RegenThread.Start(100);
        }

        public void StartMapItemThread()
        {
            EntityThread mapItemThread = new EntityThread();
            mapItemThread.Execute += new Execute(MapItemThread_Execute);
            mapItemThread.Start(100);
        }

        private void MapItemThread_Execute()
        {
            //foreach (MapItem item in mapEngine.mapItems)
            //{
            //    if (item != null)
            //    {
            //        if (item.DroppedByCharacterID != 0 && DateTime.Now >= item.DropTime.AddSeconds(10))
            //        {
            //            item.DroppedByCharacterID = 0;
            //            item.DropTime = DateTime.Now;
            //        }
            //        if (DateTime.Now >= item.DropTime.AddSeconds(30))
            //        {
            //            SendInRange(item.MapX, item.MapY, item.MapID, PacketManager.SendRemoveMapItem(item.MapItemID));
            //            mapEngine.DeleteMapItem(item);
            //        }
            //    }
            //}

            for (int i = 0; i < mapEngines.Count; i++)
            {
                MapEngine mapEngine = mapEngines[i];

                for (int j = 0; j < mapEngine.mapItems.Count; j++)
                {
                    MapItem item = mapEngine.mapItems[j];
                    Client[] clients = GetClientsForChars(mapEngine.GetCharactersInRange(new Position(item.MapX, item.MapY, item.MapZ), 150).ToArray());

                    if (item != null)
                    {
                        if (item.DroppedByCharacterID != 0 && DateTime.Now >= item.DropTime.AddSeconds(10))
                        {
                            item.DroppedByCharacterID = 0;
                            item.DropTime = DateTime.Now;
                        }
                        if (DateTime.Now >= item.DropTime.AddSeconds(30))
                        {
                            SendToClients(PacketManager.SendRemoveMapItem(item.MapItemID), clients);
                            mapEngine.DeleteMapItem(item);
                        }
                    }
                }
            }
        }

        private void RegenThread_Execute()
        {
            foreach (Client c in clients)
            {
                if (c.MyCharacter != null)
                {
                    if (c.MyCharacter.Alive)
                    {
                        bool sendHpMana = false;
                        if (DateTime.Now >= c.MyCharacter.HealthRegenTime.AddSeconds(c.MyCharacter.HealthRegenInvertal / 1000))
                        {
                            c.MyCharacter.HealthRegenTime = DateTime.Now;
                            if (c.MyCharacter.CurrentHp + c.MyCharacter.HealthRegen >= c.MyCharacter.MaxHp)
                                c.MyCharacter.CurrentHp = c.MyCharacter.MaxHp;
                            else
                                c.MyCharacter.CurrentHp += c.MyCharacter.HealthRegen;

                            sendHpMana = true;
                        }
                        if (DateTime.Now >= c.MyCharacter.ManaRegenTime.AddSeconds(c.MyCharacter.ManaRegenInvertal / 1000))
                        {
                            c.MyCharacter.ManaRegenTime = DateTime.Now;
                            if (c.MyCharacter.CurrentMana + c.MyCharacter.ManaRegen >= c.MyCharacter.MaxMana)
                                c.MyCharacter.CurrentMana = c.MyCharacter.MaxMana;
                            else
                                c.MyCharacter.CurrentMana += c.MyCharacter.ManaRegen;

                            sendHpMana = true;
                        }

                        if (sendHpMana)
                        {
                            byte[] SendHealMana = PacketManager.SendHealMana(c.MyCharacter.MaxHp, c.MyCharacter.CurrentHp, c.MyCharacter.MaxMana, c.MyCharacter.CurrentMana, 0);
                            c.Send(SendHealMana);

                            characterManager.UpdateCharacter(c.MyCharacter);
                        }
                    }
                    foreach (ActiveSkill skill in c.MyCharacter.ActiveSkills)
                    {
                        if (DateTime.Now >= skill.CastTime.AddSeconds(skill.Skill.KeepUpTime))
                        {
                            MapEngine mapEngine = GetMapEngine(c.MyCharacter.MapId);
                            Client[] Clients = GetClientsForChars(mapEngine.GetCharactersInRange(c.MyCharacter.Position, 150).ToArray());

                            if (skill.Skill.SkillID == (int)HardSkills.Summon_Monster || skill.Skill.SkillID == (int)HardSkills.Shadow_Strike || skill.Skill.SkillID == (int)AbsorbSkills.Shadow_Focus || skill.Skill.SkillID == (int)RebirthSkills.Duplicate_Attack)
                            {
                                if (c.MyCharacter.tempPet != null)
                                {
                                    byte[] KillShadow = PacketManager.SendMonsterDie(c.MyCharacter.tempPet);
                                    SendToClients(KillShadow, Clients);

                                    c.MyCharacter.tempPet = null;
                                }
                            }

                            byte[] EndSkill = PacketManager.SendEndSkill(c.MyCharacter, skill.Skill);
                            SendToClients(EndSkill, Clients);
                            c.MyCharacter.ActiveSkills.Remove(skill);

                            byte[] SendStats = PacketManager.SendStats(c.MyCharacter);
                            c.Send(SendStats);
                        }
                    }
                }
            }
        }

        //private void MobThread_Execute()
        //{
        //    foreach (Map m in mapEngine.Maps)
        //    {
        //        foreach (Monster mo in m.Monsters)
        //        {
        //            if (mo.Alive)
        //            {
        //                foreach (Client c in clients)
        //                {
        //                    if (c.MyCharacter != null && c.MyCharacter.MapId == mo.MapID && c.MyCharacter.Alive)
        //                    {
        //                        if (mapEngine.GetDistance(mo.X, mo.Y, c.MyCharacter.Position.X, c.MyCharacter.Position.Y) <= mo.SightRange + 15)
        //                        {
        //                            OnSeeEntityInfo info = mo.OnSeeEntity(c.MyCharacter);
        //                            if (info != null && info.TargetID != 0)
        //                            {
        //                                if (info.Moving)
        //                                {
        //                                    if (mo.MoveInfo == null)
        //                                        mo.Status = MonsterMoveStatus.StartMoving;

        //                                    if (mo.Status == MonsterMoveStatus.StartMoving )
        //                                    {
        //                                        mo.MoveInfo = mo.OnMove(c.MyCharacter);

        //                                        int distance = mapEngine.GetDistance(mo.X, mo.Y, mo.MoveInfo.ToX, mo.MoveInfo.ToY);
        //                                        mo.Time = (distance / mo.MovementSpeed) * 1000;
        //                                        mo.LastStart = DateTime.Now;
        //                                        mo.LastKeep = DateTime.Now;
        //                                        mo.Status = MonsterMoveStatus.KeepMoving;

        //                                        if (mapEngine.GetDistance(mo.X, mo.Y, c.MyCharacter.Position.X, c.MyCharacter.Position.Y) <= mo.AttackRange)
        //                                        {
        //                                            mo.Attacking = true;
        //                                            mo.TargetID = c.MyCharacter.CharacterId;
        //                                            info.Attacking = true;
        //                                        }

        //                                        SendInRange(mo.X, mo.Y, mo.MapID, PacketManager.SendMonsterStartMoving(mo, mo.MoveInfo));
        //                                    }
        //                                    else if (mo.Status == MonsterMoveStatus.KeepMoving && DateTime.Now >= mo.LastKeep.AddMilliseconds(mo.Time / 2))
        //                                    {
        //                                        mo.Status = MonsterMoveStatus.StopMoving;
        //                                        mo.LastStart = DateTime.Now;

        //                                        if (mapEngine.GetDistance(mo.X, mo.Y, c.MyCharacter.Position.X, c.MyCharacter.Position.Y) <= mo.AttackRange)
        //                                        {
        //                                            mo.Attacking = true;
        //                                            mo.TargetID = c.MyCharacter.CharacterId;
        //                                            info.Attacking = true;
        //                                        }
        //                                    }
        //                                    else if (mo.Status == MonsterMoveStatus.StopMoving && DateTime.Now >= mo.LastStart.AddMilliseconds(mo.Time))
        //                                    {
        //                                        mo.X = mo.MoveInfo.ToX;
        //                                        mo.Y = mo.MoveInfo.ToY;
        //                                        mo.MoveInfo.FromX = mo.X;
        //                                        mo.MoveInfo.FromY = mo.Y;
        //                                        mo.Status = MonsterMoveStatus.StartMoving;
        //                                        mo.LastStart = DateTime.Now;

        //                                        if (mapEngine.GetDistance(mo.X, mo.Y, c.MyCharacter.Position.X, c.MyCharacter.Position.Y) <= mo.AttackRange)
        //                                        {
        //                                            mo.Attacking = true;
        //                                            mo.TargetID = c.MyCharacter.CharacterId;
        //                                            info.Attacking = true;
        //                                        }

        //                                        SendInRange(mo.X, mo.Y, mo.MapID, PacketManager.SendMonsterStopMoving(mo, mo.MoveInfo));
        //                                    }

        //                                }
        //                                if (info.Attacking)
        //                                {
        //                                    if (DateTime.Now >= mo.LastAttack.AddSeconds(2))
        //                                    {
        //                                        Client target = null;
        //                                        target = GetClientByCharacterID(mo.TargetID);
        //                                        if (target == null)
        //                                        {
        //                                            mo.TargetID = 0;
        //                                            mo.Attacking = false;
        //                                        }
        //                                        if (target.MyCharacter.Alive && mo.Alive)
        //                                        {
        //                                            Client[] nearbyClients = GetNearByClients(target);

        //                                            int distance = mapEngine.GetDistance(mo.X, mo.Y, target.MyCharacter.X, target.MyCharacter.Y);

        //                                            if (distance < mo.AttackRange + 15)
        //                                            {
        //                                                AttackInfo atk = target.MyCharacter.OnAttack(mo);

        //                                                if (atk != null)
        //                                                {
        //                                                    byte[] SendTargetHealMana = PacketManager.SendHealMana(target.MyCharacter.MaxHp, target.MyCharacter.CurrentHp, target.MyCharacter.MaxMana, target.MyCharacter.CurrentMana, 0);
        //                                                    target.Send(SendTargetHealMana);

        //                                                    byte[] SendAttack1 = PacketManager.SendAttack1(atk);
        //                                                    SendSpecific(nearbyClients, SendAttack1);

        //                                                    byte[] SendAttack2 = PacketManager.SendAttack2(atk, target.MyCharacter);
        //                                                    SendSpecific(nearbyClients, SendAttack2);

        //                                                    if (atk.DoRefDamage && !atk.Dead)
        //                                                    {
        //                                                        BaseSkill skill = target.MyCharacter.FindSkill((int)HardSkills.Reflection);
        //                                                        CastSkillInfo RefInfo = mo.OnCast(target.MyCharacter, skill);
        //                                                        RefInfo.CasterType = 1;

        //                                                        byte[] SendRefdamage = PacketManager.SendCast2(skill, RefInfo, mo);
        //                                                        SendSpecific(nearbyClients, SendRefdamage);

        //                                                        if (RefInfo.Dead)
        //                                                        {
        //                                                            CharacterGainExperience(target, mo, target.MyCharacter, false);

        //                                                            MonsterDropItem(nearbyClients, mo, target.MyCharacter);

        //                                                            byte[] SendMonsterDie = PacketManager.SendMonsterDie(mo);
        //                                                            SendSpecific(nearbyClients, SendMonsterDie);
        //                                                        }
        //                                                    }

        //                                                    if (atk.PetDamaged)
        //                                                    {
        //                                                        if (target.MyCharacter.tempPet != null)
        //                                                        {
        //                                                            byte[] SendPetHealth = PacketManager.SendPetHealth(target.MyCharacter.tempPet);
        //                                                            target.Send(SendPetHealth);

        //                                                            if (atk.PetDied)
        //                                                            {
        //                                                                BaseSkill skill = new BaseSkill();
        //                                                                if (target.MyCharacter.RemoveActiveSkill((int)HardSkills.Shadow_Strike))
        //                                                                {
        //                                                                    skill.SkillID = (int)HardSkills.Shadow_Strike;
        //                                                                    skill.SkillLevel = 1;
        //                                                                    target.Send(PacketManager.SendEndSkill(target.MyCharacter, skill));
        //                                                                }
        //                                                                if (target.MyCharacter.RemoveActiveSkill((int)AbsorbSkills.Shadow_Focus))
        //                                                                {
        //                                                                    skill.SkillID = (int)AbsorbSkills.Shadow_Focus;
        //                                                                    skill.SkillLevel = 1;
        //                                                                    target.Send(PacketManager.SendEndSkill(target.MyCharacter, skill));
        //                                                                }
        //                                                                if (target.MyCharacter.RemoveActiveSkill((int)RebirthSkills.Duplicate_Attack))
        //                                                                {
        //                                                                    skill.SkillID = (int)RebirthSkills.Duplicate_Attack;
        //                                                                    skill.SkillLevel = 1;
        //                                                                    target.Send(PacketManager.SendEndSkill(target.MyCharacter, skill));
        //                                                                }


        //                                                                byte[] RemovePet = PacketManager.SendMonsterDie(target.MyCharacter.tempPet);
        //                                                                SendInRange(target.MyCharacter.tempPet.X, target.MyCharacter.tempPet.Y, target.MyCharacter.tempPet.MapID, RemovePet);
        //                                                                target.MyCharacter.tempPet = null;
        //                                                            }
        //                                                        }
        //                                                    }

        //                                                    if (atk.Dead)
        //                                                    {
        //                                                        byte[] SendPlayerDead = PacketManager.SendPlayerDie(target.MyCharacter);
        //                                                        SendSpecific(nearbyClients, SendPlayerDead);
        //                                                    }
        //                                                }

        //                                                mo.LastAttack = DateTime.Now;
        //                                            }
        //                                            else
        //                                            {
        //                                                mo.TargetID = 0;
        //                                                mo.Attacking = false;
        //                                            }
        //                                        }
        //                                        else
        //                                            mo.Attacking = false;
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //                if (!mo.Attacking)
        //                {
        //                    if (mo.Status == MonsterMoveStatus.StartMoving && DateTime.Now >= mo.LastStart.AddSeconds(2))
        //                    {
        //                        int randomDirection = mapEngine.Random.Next(0, 4);
        //                        switch (randomDirection)
        //                        {
        //                            case 0:
        //                                mo.ToX = (short)mapEngine.Random.Next(mo.SpawnX - mo.WanderRange, mo.SpawnX + mo.WanderRange);
        //                                mo.StepX = (short)((mo.ToX - mo.SpawnX) / 2);
        //                                break;

        //                            case 1:
        //                                mo.ToX = (short)mapEngine.Random.Next(mo.SpawnX - mo.WanderRange, mo.SpawnX + mo.WanderRange);
        //                                mo.StepX = (short)((mo.ToX - mo.SpawnX) / 2);
        //                                break;

        //                            case 2:
        //                                mo.ToY = (short)mapEngine.Random.Next(mo.SpawnY - mo.WanderRange, mo.SpawnY + mo.WanderRange);
        //                                mo.StepY = (short)((mo.ToY - mo.SpawnY) / 2);
        //                                break;

        //                            case 3:
        //                                mo.ToY = (short)mapEngine.Random.Next(mo.SpawnY - mo.WanderRange, mo.SpawnY + mo.WanderRange);
        //                                mo.StepY = (short)((mo.ToY - mo.SpawnY) / 2);
        //                                break;
        //                        }

        //                        int distance = mapEngine.GetDistance(mo.X, mo.Y, mo.ToX, mo.ToY);
        //                        mo.Time = (distance / mo.MovementSpeed) * 1000;

        //                        MovementInfo mi = new MovementInfo
        //                        {
        //                            FromX = mo.X,
        //                            FromY = mo.Y,
        //                            ToX = mo.ToX,
        //                            ToY = mo.ToY,
        //                            FromZ = 0,
        //                            ToZ = 0,
        //                            Rotation = 0,
        //                        };

        //                        double rotation = Math.Atan2(mo.ToY - mo.Y, mo.ToX - mo.X);
        //                        rotation = rotation * 180 / Math.PI;
        //                        mi.Rotation = (int)Math.Abs(rotation);
        //                        mo.LastStart = DateTime.Now;
        //                        mo.LastKeep = DateTime.Now;
        //                        mo.Status = MonsterMoveStatus.KeepMoving;

        //                        SendInRange(mo.X, mo.Y, mo.MapID, PacketManager.SendMonsterStartMoving(mo, mi));
        //                    }
        //                    else if (mo.Status == MonsterMoveStatus.KeepMoving && DateTime.Now >= mo.LastKeep.AddMilliseconds(mo.Time / 2))
        //                    {
        //                        mo.Status = MonsterMoveStatus.StopMoving;
        //                        mo.LastStart = DateTime.Now;
        //                    }
        //                    else if (mo.Status == MonsterMoveStatus.StopMoving && DateTime.Now >= mo.LastStart.AddMilliseconds(mo.Time))
        //                    {
        //                        mo.X = mo.ToX;
        //                        mo.Y = mo.ToY;

        //                        MovementInfo mi = new MovementInfo
        //                        {
        //                            FromX = mo.X,
        //                            FromY = mo.Y,
        //                            ToX = mo.X,
        //                            ToY = mo.Y,
        //                            FromZ = 0,
        //                            ToZ = 0,
        //                            Rotation = 0,
        //                        };

        //                        double rotation = Math.Atan2(mo.ToY - mo.Y, mo.ToX - mo.X);
        //                        rotation = rotation * 180 / Math.PI;
        //                        mi.Rotation = (int)Math.Abs(rotation);
        //                        mo.Status = MonsterMoveStatus.StartMoving;
        //                        mo.LastStart = DateTime.Now;

        //                        SendInRange(mo.X, mo.Y, mo.MapID, PacketManager.SendMonsterStopMoving(mo, mi));
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                int respawntime = 15;
        //                if (mo.MonsterType == 112 || mo.MonsterType == 187 || mo.MonsterType == 188) // king, phx, gb
        //                    respawntime = 3600;
        //                if (mo.MonsterType == 113) // emp
        //                    respawntime = 7200;

        //                if (DateTime.Now >= mo.DeathTime.AddSeconds(respawntime))
        //                {
        //                    mo.X = mo.SpawnX;
        //                    mo.Y = mo.SpawnY;
        //                    mo.Alive = true;
        //                    mo.CurHealth = mo.MaxHealth;
        //                    // send maybe spawn or something here
        //                }
        //            }
        //        }
        //    }
        //}
        

        public ChannelServer(string ipAddress, int port, int expRate, int itemDropRate, int moneyDropRate, int stoneDropRate, int bookDropRate, int pillDropRate, int stoneCombineChance)
            : base(ipAddress, port)
        {
            gameEngine = new GameEngine(ConfigurationManager.ConnectionStrings["XiahDb"].ConnectionString, ConfigurationManager.ConnectionStrings["XiahDb"].ProviderName);
            //mapEngine = new MapEngine(ConfigurationManager.ConnectionStrings["XiahDb"].ConnectionString, ConfigurationManager.ConnectionStrings["XiahDb"].ProviderName);
            accountManager = new AccountManager(ConfigurationManager.ConnectionStrings["XiahDb"].ConnectionString, ConfigurationManager.ConnectionStrings["XiahDb"].ProviderName);
            characterManager = new CharacterManager(ConfigurationManager.ConnectionStrings["XiahDb"].ConnectionString, ConfigurationManager.ConnectionStrings["XiahDb"].ProviderName);
            itemDataManager = new ItemDataManager(ConfigurationManager.ConnectionStrings["XiahDb"].ConnectionString, ConfigurationManager.ConnectionStrings["XiahDb"].ProviderName);
            channelManager = new ChannelManager(ConfigurationManager.ConnectionStrings["XiahDb"].ConnectionString, ConfigurationManager.ConnectionStrings["XiahDb"].ProviderName);
            petManager = new PetManager(ConfigurationManager.ConnectionStrings["XiahDb"].ConnectionString, ConfigurationManager.ConnectionStrings["XiahDb"].ProviderName);
            mapManager = new MapManager(ConfigurationManager.ConnectionStrings["XiahDb"].ConnectionString, ConfigurationManager.ConnectionStrings["XiahDb"].ProviderName);
            guildManager = new GuildManager(ConfigurationManager.ConnectionStrings["XiahDb"].ConnectionString, ConfigurationManager.ConnectionStrings["XiahDb"].ProviderName);
            InitiateMapEngines();
            InitiateChannels();
            //StartMonsters();
            StartRegenThread();
            StartMapItemThread();

            ExperienceMultiper = expRate;
            ItemDropChance = itemDropRate;
            MoneyDropChance = moneyDropRate;
            StoneDropChance = stoneDropRate;
            BookDropChance = bookDropRate;
            PillDropChance = pillDropRate;
            StoneCombineChance = stoneCombineChance;
        }

        private void InitiateMapEngines()
        {
            Map[] maps = mapManager.GetAllMaps();
            mapEngines = new List<MapEngine>();

            for (int i = 0; i < maps.Length; i++)
            {
                MapEngine mapEngine = new MapEngine(ConfigurationManager.ConnectionStrings["XiahDb"].ConnectionString, ConfigurationManager.ConnectionStrings["XiahDb"].ProviderName, maps[i]);
                mapEngine.MonsterMove += new EventHandler<MonsterMoveInfoEventArgs>(mapEngine_MonsterMove);
                mapEngine.MonsterAttack += new EventHandler<MonsterAttackInfoEventArgs>(mapEngine_MonsterAttack);
                mapEngines.Add(mapEngine);
            }
        }

        void mapEngine_MonsterAttack(object sender, MonsterAttackInfoEventArgs e)
        {
            MonsterAI monsterAI = sender as MonsterAI;
            MapEngine mapEngine = GetMapEngine(monsterAI.Monster.MapID);
            Client[] clients = GetClientsForChars(mapEngine.GetCharactersInRange(monsterAI.Monster.Position, 150).ToArray());
            BaseEntity Target = e.Info.Target;
            Character ch = null;
            Pet pet = null;         
            Client c = null;
            if (Target == null)
            {
                monsterAI.Monster.Attacking = false;
                monsterAI.Monster.Target = null;
                return;
            }

            if (Target is Character)
            {
                ch = Target as Character;
                c = GetClientByCharacterID(ch.CharacterId);
            }
            if (Target is Pet)
            {
                pet = Target as Pet;
                c = GetClientByCharacterID(pet.OwnerID);
            }

            if (ch != null && ch.Alive && monsterAI.Monster.Alive)
            {
                AttackInfo atk = ch.OnAttack(monsterAI.Monster);
                if (atk != null)
                {
                    byte[] SendTargetHealMana = PacketManager.SendHealMana(ch.MaxHp, ch.CurrentHp, ch.MaxMana, ch.CurrentMana, 0);
                    c.Send(SendTargetHealMana);

                    byte[] SendAttack1 = PacketManager.SendAttack1(atk);
                    SendToClients(SendAttack1, clients);

                    byte[] SendAttack2 = PacketManager.SendAttack2(atk, c.MyCharacter);
                    SendToClients(SendAttack2, clients);


                    if (atk.DoRefDamage && !atk.Dead)
                    {
                        BaseSkill skill = c.MyCharacter.FindSkill((int)HardSkills.Reflection);
                        CastSkillInfo RefInfo = monsterAI.Monster.OnCast(c.MyCharacter, skill);
                        RefInfo.CasterType = 1;

                        byte[] SendRefdamage = PacketManager.SendCast2(skill, RefInfo, monsterAI.Monster);
                        SendToClients(SendRefdamage, clients);

                        if (RefInfo.Dead)
                        {
                            CharacterGainExperience(c, monsterAI.Monster, c.MyCharacter, false);

                            MonsterDropItem(monsterAI.Monster, c.MyCharacter);

                            byte[] SendMonsterDie = PacketManager.SendMonsterDie(monsterAI.Monster);
                            SendToClients(SendMonsterDie, clients);
                        }
                    }

                    if (atk.PetDamaged)
                    {
                        if (c.MyCharacter.tempPet != null)
                        {
                            byte[] SendPetHealth = PacketManager.SendPetHealth(c.MyCharacter.tempPet);
                            c.Send(SendPetHealth);

                            if (atk.PetDied)
                            {
                                BaseSkill skill = new BaseSkill();
                                if (c.MyCharacter.RemoveActiveSkill((int)HardSkills.Shadow_Strike))
                                {
                                    skill.SkillID = (int)HardSkills.Shadow_Strike;
                                    skill.SkillLevel = 1;
                                    c.Send(PacketManager.SendEndSkill(c.MyCharacter, skill));
                                }
                                if (c.MyCharacter.RemoveActiveSkill((int)AbsorbSkills.Shadow_Focus))
                                {
                                    skill.SkillID = (int)AbsorbSkills.Shadow_Focus;
                                    skill.SkillLevel = 1;
                                    c.Send(PacketManager.SendEndSkill(c.MyCharacter, skill));
                                }
                                if (c.MyCharacter.RemoveActiveSkill((int)RebirthSkills.Duplicate_Attack))
                                {
                                    skill.SkillID = (int)RebirthSkills.Duplicate_Attack;
                                    skill.SkillLevel = 1;
                                    c.Send(PacketManager.SendEndSkill(c.MyCharacter, skill));
                                }


                                byte[] RemovePet = PacketManager.SendMonsterDie(c.MyCharacter.tempPet);
                                SendToClients(RemovePet, clients);

                                c.MyCharacter.tempPet = null;
                            }
                        }
                    }


                    if (atk.Dead)
                    {
                        mapEngine.ActiveClientEntities.Remove(c.MyCharacter);
                        byte[] SendPlayerDead = PacketManager.SendPlayerDie(c.MyCharacter);
                        SendToClients(SendPlayerDead, clients);
                    }

                    monsterAI.Monster.LastAttack = DateTime.Now;
                }
            }
            else if (pet != null && pet.Alive && monsterAI.Monster.Alive)
            {
                AttackInfo atk = pet.OnAttack(monsterAI.Monster);
                if (atk != null)
                {
                    byte[] SendPetHealth = PacketManager.SendPetHealth(pet);
                    c.Send(SendPetHealth);

                    byte[] SendAttack1 = PacketManager.SendAttack1(atk);
                    SendToClients(SendAttack1, clients);

                    byte[] SendAttack2 = PacketManager.SendAttack2(atk, Target);
                    SendToClients(SendAttack2, clients);

                    if (atk.Dead)
                    {
                        byte[] SendPetDie = PacketManager.SendMonsterDie(pet);
                        SendToClients(SendPetDie, clients);

                        pet.Alive = false;
                        petManager.UpdatePet(pet);
                        c.MyCharacter.Pet = null;
                        mapEngine.ActiveClientEntities.Remove(pet);
                    }

                    monsterAI.Monster.LastAttack = DateTime.Now;
                }
            }
        }

        void mapEngine_MonsterMove(object sender, MonsterMoveInfoEventArgs e)
        {
            MonsterAI monsterAI = sender as MonsterAI;
            MapEngine mapEngine = GetMapEngine(monsterAI.Monster.MapID);

            byte[] movementPacket = null;

            switch (e.Info.Status)
            {
                case MonsterMoveStatus.StartMoving:
                    movementPacket = PacketManager.SendMonsterStartMoving(monsterAI.Monster, e.Info);
                    break;

                case MonsterMoveStatus.KeepMoving:
                    movementPacket = PacketManager.SendMonsterKeepMoving(monsterAI.Monster, e.Info);
                    break;

                case MonsterMoveStatus.StopMoving:
                    movementPacket = PacketManager.SendMonsterStopMoving(monsterAI.Monster, e.Info);
                    break;
            }


            Client[] clients = GetClientsForChars(mapEngine.GetCharactersInRange(monsterAI.Monster.Position, 150).ToArray());
            SendToClients(movementPacket, clients);
        }

        private static void SendToClients(byte[] packet, Client[] clients)
        {
            for (int i = 0; i < clients.Length; i++)
            {
                if (clients[i] != null)
                    clients[i].Send(packet);
            }
        }

        private void InitiateChannels()
        {
            channelInfos = channelManager.GetAllChannels();
        }

        #region Base Stuff

        protected override void OnClientConnected(ClientConnectionEventArgs e)
        {
            base.OnClientConnected(e);

            Client c = e.Client;
            c.ClientUnknownPacket += new EventHandler<UnknownPacketEventArgs>(c_ClientUnknownPacket);
            c.ClientDisposed += new EventHandler(serverClient_ClientDisposed);

            c.ClientUnitConnectionInfo += new EventHandler<ClientUnitConnectionInfoEventArgs>(c_ClientUnitConnectionInfo);
            c.ClientChannelRequestInfo += new EventHandler<ClientChannelRequestEventArgs>(c_ClientChannelRequestInfo);
            c.ClientCreateCharacterInfo += new EventHandler<ClientCharacterCreateInfoEventArgs>(c_ClientCreateCharacterInfo);

            c.ClientLogPacket += new EventHandler<LogPacketEventArgs>(c_ClientLogPacket);

            c.ClientChannelLoginInfo += new EventHandler<ClientChannelLoginInfoEventArgs>(c_ClientChannelLoginInfo);
            c.ClientChannelChangeInfo += new EventHandler<ClientChannelChangeEventArgs>(c_ClientChannelChangeInfo);
            c.ClientChannelStuffInfo += new EventHandler<ClientChannelChangeStuffEventArgs>(c_ClientChannelStuffInfo);

            c.ClientWalkToPortalInfo += new EventHandler<ClientWalkToPortalEventArgs>(c_ClientWalkToPortalInfo);
            c.ClientMapLoadInfo += new EventHandler<ClientMapLoadEventArgs>(c_ClientMapLoadInfo);
            c.ClientSkillLoadInfo += new EventHandler<ClientSkillLoadEvenArgs>(c_ClientSkillLoadInfo);

            c.ClientMoveWarehouseItemToBagInfo += new EventHandler<ClientMoveWarehouseItemToBagEventArgs>(c_ClientMoveWarehouseItemToBagInfo);
            c.ClientOpenWarehouseInfo += new EventHandler<ClientOpenWarehouseEventArgs>(c_ClientOpenWarehouseInfo);
            c.ClientAddItemToWarehouseInfo += new EventHandler<ClientAddItemToWarehouseEventargs>(c_ClientAddItemToWarehouseInfo);
            c.ClientMoveWarehouseItemInfo += new EventHandler<ClientMoveWarehouseItemEventArgs>(c_ClientMoveWarehouseItemInfo);

            c.ClientInventoryLoadInfo += new EventHandler<ClientInventoryLoadEvenArgs>(c_ClientInventoryLoadInfo);
            c.ClientMoveItemInfo += new EventHandler<ClientMoveItemEventArgs>(c_ClientMoveItemInfo);
            c.ClientDropItemInfo += new EventHandler<ClientDropItemEventArgs>(c_ClientDropItemInfo);
            c.ClientPickItemInfo += new EventHandler<ClientPickItemEventArgs>(c_ClientPickItemInfo);
            c.ClientStackItemInfo += new EventHandler<ClientStackItemInfoEventArgs>(c_ClientStackItemInfo);
            c.ClientImbueItemInfo += new EventHandler<ClientImbueItemEventArgs>(c_ClientImbueItemInfo);
            c.ClientAcceptImbueItemInfo += new EventHandler<ClientAcceptImbueItemEventArgs>(c_ClientAcceptImbueItemInfo);

            c.ClientNpcTradeInfo += new EventHandler<ClientNpcTradeEventArgs>(c_ClientNpcTradeInfo);
            c.ClientBuyItemInfo += new EventHandler<ClientBuyItemEventArgs>(c_ClientBuyItemInfo);
            c.ClientSellItemInfo += new EventHandler<ClientSellItemEventArgs>(c_ClientSellItemInfo);
            c.ClientUseItemInfo += new EventHandler<ClientUseItemEventArgs>(c_ClientUseItemInfo);

            c.ClientUseRebirthPillInfo += new EventHandler<ClientUseRebirthPillEventArgs>(c_ClientUseRebirthPillInfo);

            c.ClientAddItemToShopInfo += new EventHandler<ClientAddItemToShopEventArgs>(c_ClientAddItemToShopInfo);
            c.ClientMoveShopItemInfo += new EventHandler<ClientMoveShopItemEventArgs>(c_ClientMoveShopItemInfo);
            c.ClientChangeShopInfo += new EventHandler<ClientChangeShopInfoEventArgs>(c_ClientChangeShopInfo);
            c.ClientMoveShopItemToBagInfo += new EventHandler<ClientMoveShopItemToBagEventArgs>(c_ClientMoveShopItemToBagInfo);
            c.ClientShopStartSellingInfo += new EventHandler<ClientShopStartSellingEventArgs>(c_ClientShopStartSellingInfo);
            c.ClientOpenOtherPlayerShopInfo += new EventHandler<ClientOpenOtherPlayerShopEventArgs>(c_ClientOpenOtherPlayerShopInfo);
            c.ClientBuyOtherPlayerItemInfo += new EventHandler<ClientBuyOtherPlayerShopItemEventArgs>(c_ClientBuyOtherPlayerItemInfo);
            c.ClientShopTakeMoneyInfo += new EventHandler<ClientShopTakeMoneyInfoEventArgs>(c_ClientShopTakeMoneyInfo);

            c.ClientTradePlayerInfo += new EventHandler<ClientTradePlayerEventArgs>(c_ClientTradePlayerInfo);
            c.ClientTradePlayerCommandsInfo += new EventHandler<ClientTradePlayerCommandsEventArgs>(c_ClientTradePlayerCommandsInfo);
            c.ClientTradeAddMoneyInfo += new EventHandler<ClientTradeAddMoneyEventArgs>(c_ClientTradeAddMoneyInfo);
            c.ClientTradeAddItemInfo += new EventHandler<ClientTradeAddItemEventArgs>(c_ClientTradeAddItemInfo);

            c.ClientUpdateQuickSlotInfo += new EventHandler<ClientUpdateQuickSlotEventArgs>(c_ClientUpdateQuickSlotInfo);
            c.ClientSelectSkillInfo += new EventHandler<ClientSelectSkillEventArgs>(c_ClientSelectSkillInfo);

            c.ClientLearnSkillInfo += new EventHandler<ClientLearnSkillEventArgs>(c_ClientLearnSkillInfo);

            c.ClientSpawnStatLoadInfo += new EventHandler<ClientStatLoadEventArgs>(c_ClientSpawnStatLoadInfo);
            c.ClientSpawnLoadInfo += new EventHandler<ClientSpawnLoadEventArgs>(c_ClientSpawnLoadInfo);
            c.ClientRequestStatsLoadInfo += new EventHandler<ClientRequestStatsLoadInfo>(c_ClientRequestStatsLoadInfo);
            c.ClientAddStatInfo += new EventHandler<ClientAddStatEventArgs>(c_ClientAddStatInfo);

            c.ClientSendPetInfo += new EventHandler<ClientSendPetEventArgs>(c_ClientSendPlayerPetInfo);
            c.ClientSpawnOtherPlayerInfo += new EventHandler<ClientSpawnOtherPlayerEventArgs>(c_ClientSpawnOtherPlayerInfo);

            c.ClientMovementInfo += new EventHandler<ClientMovementInfoEventArgs>(c_ClientMovementInfo);

            c.ClientAddFriendInfo += new EventHandler<ClientAddFriendEventArgs>(c_ClientAddFriendInfo);
            c.ClientRequestFriendListInfo += new EventHandler<ClientRequestFriendListEventArgs>(c_ClientRequestFriendListInfo);

            c.ClientChatMessageInfo += new EventHandler<ClientChatMessageEventArgs>(c_ClientChatMessageInfo);
            c.ClientCommandInfo += new EventHandler<ClientCommandInfoEventArgs>(c_ClientCommandInfo);

            c.ClientMonsterSpawnRequestInfo += new EventHandler<ClientMonsterSpawnRequestEventArgs>(c_ClientMonsterSpawnRequestInfo);

            c.ClientAttackInfo += new EventHandler<ClientAttackEventArgs>(c_ClientAttackInfo);
            c.ClientCastSkillInfo += new EventHandler<ClientCastSkillEventArgs>(c_ClientCastSkillInfo);

            c.ClientUseTeleporterInfo += new EventHandler<ClientUseTeleporterEventArgs>(c_ClientUseTeleporterInfo);

            c.ClientCombineItemsInfo += new EventHandler<ClientCombineItemsEvenArgs>(c_ClientCombineItemsInfo);

            c.ClientPartyRequestInfo += new EventHandler<ClientPartyRequestEventArgs>(c_ClientPartyRequestInfo);
            c.ClientAddToPartyRequestInfo += new EventHandler<ClientAddToPartyRequestEventArgs>(c_ClientAddToPartyRequestInfo);
            c.ClientLeavePartyInfo += new EventHandler<ClientLeavePartyEventArgs>(c_ClientLeavePartyInfo);
            c.ClientChangePartyLeaderInfo += new EventHandler<ClientChangePartyLeaderEventArgs>(c_ClientChangePartyLeaderInfo);

            c.ClientPetMovementInfo += new EventHandler<ClientPetMovementInfoEventArgs>(c_ClientPetMovementInfo);
            c.ClientKillPetInfo += new EventHandler<ClientKillPetInfoEventArgs>(c_ClientKillPetInfo);
            c.ClientBaitPetInfo += new EventHandler<ClientBaitPetInfoEventArgs>(c_ClientBaitPetInfo);
            c.ClientRequestPetStatsInfo += new EventHandler<ClientRequestPetStatsInfoEventArgs>(c_ClientRequestPetStatsInfo);
            c.ClientRequestSpawnOtherPetInfo += new EventHandler<ClientRequestSpawnOtherPetInfoEventArgs>(c_ClientRequestSpawnOtherPetInfo);
            c.ClientResurrectPetInfo += new EventHandler<ClientResurrectPetInfoEventArgs>(c_ClientResurrectPetInfo);
            c.ClientRenamePetInfo += new EventHandler<ClientRenamePetInfoEventArgs>(c_ClientRenamePetInfo);
            c.ClientSendPetToMeInfo += new EventHandler<ClientSendPetToMeInfoEventArgs>(c_ClientSendPetToMeInfo);
            c.ClientTradePetInfo += new EventHandler<ClientTradePetInfoEventArgs>(c_ClientTradePetInfo);
            c.ClientTradePetAmountInfo += new EventHandler<ClientTradePetAmountInfoEventArgs>(c_ClientTradePetAmountInfo);
            c.ClientSealPetInfo += new EventHandler<ClientSealPetInfoEventArgs>(c_ClientSealPetInfo);
            c.ClientUnSealPetInfo += new EventHandler<ClientUnSealPetInfoEventArgs>(c_ClientUnSealPetInfo);

            c.ClientRequestDeath += new EventHandler<ClientRequestDeath>(c_ClientRequestDeath);

            c.ClientCreateGuildInfo += new EventHandler<ClientCreateGuildInfoEventArgs>(c_ClientCreateGuildInfo);
            c.ClientRequestGuildInfo += new EventHandler<ClientRequestGuildEventArgs>(c_ClientRequestGuildInfo);
            c.ClientRequestGuildInformation += new EventHandler<ClientRequestGuildInfoEventArgs>(c_ClientRequestGuildInformation);
            c.ClientRequestGuildMemberStatsInfo += new EventHandler<ClientRequestGuildMemberStatsEventArgs>(c_ClientRequestGuildMemberStatsInfo);
            c.ClientRequestGuildMemberChangeRankInfo += new EventHandler<ClientRequestGuildMemberRankChangeEventArgs>(c_ClientRequestGuildMemberChangeRankInfo);
            c.ClientRequestGuildMemberChangeTitleInfo += new EventHandler<ClientRequestGuildMemberChangeTitleEventArgs>(c_ClientRequestGuildMemberChangeTitleInfo);
            c.ClientRequestGuildChangeNoticeInfo += new EventHandler<ClientRequestGuildChangeNoticeEventArgs>(c_ClientRequestGuildChangeNoticeInfo);
            c.ClientRequestJoinGuildInfo += new EventHandler<ClientRequestJoinGuildEventArgs>(c_ClientRequestJoinGuildInfo);
            c.ClientRequestCloseGuildInfo += new EventHandler<ClientRequestCloseGuildEventArgs>(c_ClientRequestCloseGuildInfo);
            c.ClientRequestLeaveGuildInfo += new EventHandler<ClientRequestLeaveGuildEventArgs>(c_ClientRequestLeaveGuildInfo);
            c.ClientGuildChatInfo += new EventHandler<ClientGuildChatEventArgs>(c_ClientGuildChatInfo);
        }

        protected override void serverClient_ClientDisposed(object sender, EventArgs e)
        {
            Client c = (Client)sender;
            if (c.MyCharacter != null && c.MyCharacter.Online)
            {
                c.MyCharacter.Online = false;

                MapEngine engine = GetMapEngine(c.MyCharacter.MapId);
                engine.ActiveClientEntities.Remove(c.MyCharacter);
                Client[] clients = GetClientsForChars(engine.GetCharactersInRange(c.MyCharacter.Position, 150).ToArray());

                accountManager.ExitGame(c.AccountID);
                UpdateFriendsStatus(c);

                if (c.MyCharacter.Guild != null)
                {
                    GuildMember g = c.MyCharacter.Guild.Members.Single(x => x.Character.CharacterId == c.MyCharacter.CharacterId);
                    g.Online = false;
                    SendToGuild(c.MyCharacter.Guild, PacketManager.SendGuildMemberInfo(g));
                }
                if (c.MyCharacter.Party != null)
                {
                    LeavePartyInfo i = new LeavePartyInfo
                    {
                        PartyID = c.MyCharacter.Party.PartyID
                    };
                    ClientLeaveParty(c);
                }

                ClientConnectionChanged(null, new PlayerConnectionChangedEventArgs(c.MyCharacter, c.MyCharacter.Online));

                byte[] bufferRemoveCharacter = PacketManager.SendRemoveCharacter(c.MyCharacter, RemoveCharacterEffect.None);
                SendToClients(bufferRemoveCharacter, clients);

                if (c.MyCharacter.Pet != null && !c.MyCharacter.Pet.IsSealed)
                {
                    byte[] bufferRemovePet = PacketManager.SendRemovePet(c.MyCharacter.Pet);
                    SendToClients(bufferRemovePet, clients);

                    petManager.UpdatePet(c.MyCharacter.Pet);
                }
            }

            base.serverClient_ClientDisposed(sender, e);
        }

        void c_ClientUnitConnectionInfo(object sender, ClientUnitConnectionInfoEventArgs e)
        {
            if (ClientUnitConnection != null)
                ClientUnitConnection(sender, e);

            Client c = (Client)sender;
            UnitLogin u = e.UnitLoginDetails;

            int accountId = u.AccountID;

            if (u.Channel == 0)
            {
                byte[] bufferUniLoginState = PacketManager.SendUnitLoginState(LoginState.Success, u.CharacterID);
                c.Send(bufferUniLoginState);

                Character[] charList = characterManager.GetAllCharactersByAccountId(accountId);

                byte[] bufferCharacterList = PacketManager.SendCharacterList(charList);
                c.Send(bufferCharacterList);
            }
            else
            {
                if (accountManager.ConnectVerification(accountId))
                {
                    byte[] bufferUniLoginState = PacketManager.SendUnitLoginState(LoginState.Success, u.CharacterID);
                    c.Send(bufferUniLoginState);
                }
                else
                {
                    Client[] otherPlayer = clients.Where(x => (x.AccountID == u.AccountID) && (x != c)).ToArray();
                    for (int i = 0; i < otherPlayer.Length; i++)
                    {
                        otherPlayer[i].Send(PacketManager.SendUnitLoginState(LoginState.OtherConnecting, 0));
                        otherPlayer[i].Dispose();
                    }

                    byte[] bufferUniLoginState = PacketManager.SendUnitLoginState(LoginState.Success, u.CharacterID);
                    c.Send(bufferUniLoginState);
                }
            }
        }

        void c_ClientCreateCharacterInfo(object sender, ClientCharacterCreateInfoEventArgs e)
        {
            Client c = (Client)sender;
            Character cc = e.CreateCharacterDetails;

            int characterID = -1;
            if (characterManager.CreateCharacter(cc.Name, cc.Class, c.AccountID, ref characterID))
            {
                // needs checks for name if its in use, if success return with playerid and sends it with this packet
                byte[] bufferCreateCharacter = PacketManager.SendCreateCharacter(CharCreationState.Success, characterID);
                c.Send(bufferCreateCharacter);

                // if it success creating character, needs to load new list and send it to client
                Character[] charList = characterManager.GetAllCharactersByAccountId(c.AccountID);

                byte[] bufferCharacterList = PacketManager.SendCharacterList(charList);
                c.Send(bufferCharacterList);
            }
            else
            {
                byte[] bufferCreateCharacter = PacketManager.SendCreateCharacter(CharCreationState.NameInUse, characterID);
                c.Send(bufferCreateCharacter);
            }
        }


        void c_ClientChannelRequestInfo(object sender, ClientChannelRequestEventArgs e)
        {
            Client c = (Client)sender;
            ChannelRequest cr = e.ChannelRequestDetails;

            foreach (ChannelInfo ci in channelInfos)
            {
                byte[] bufferChannelList = PacketManager.SendChannel(ci, channelInfos.Length);
                c.Send(bufferChannelList);
            }


            byte[] bufferChannelStatus = PacketManager.SendChannelStatus();
            c.Send(bufferChannelStatus);
        }

        private Client GetClientByName(string name)
        {
            foreach (Client c in clients)
            {
                if (c.MyCharacter.Name.ToLower() == name.ToLower() && c.MyCharacter.Online)
                    return c;
            }
            return null;
        }

        private Client GetClientByCharacterID(int characterId)
        {
            foreach (Client c in clients)
            {
                if (c.MyCharacter != null && c.MyCharacter.CharacterId == characterId && c.MyCharacter.Online)
                    return c;
            }
            return null;
        }

        public Client[] GetClientsForChars(Character[] chars)
        {
            List<Client> nearbyClients = new List<Client>();

            for (int i = 0; i < chars.Length; i++)
            {
                Client client = clients.FirstOrDefault(tempClient => tempClient.MyCharacter == chars[i]);
                nearbyClients.Add(client);
            }

            return nearbyClients.ToArray();
        }

        public Client[] GetNearClientsByMonster(Monster m)
        {
            try
            {
                var nearbyClients = clients.Where(x => (MapEngine.GetDistance(m.Position.X, m.Position.Y, x.MyCharacter.Position.X, x.MyCharacter.Position.Y) <= m.SightRange + 15) && x.Map.MapID == m.MapID);

                return nearbyClients.ToArray();
            }
            catch
            {
                return null;
            }
        }

        private Client[] GetNearByClients(Monster m)
        {
            try
            {
                var nearbyClients = clients.Where(x => (x.MyCharacter.MapId == m.MapID && MapEngine.GetDistance(x.MyCharacter.Position.X, x.MyCharacter.Position.Y, m.Position.X, m.Position.Y) < 150));

                return nearbyClients.ToArray();
            }
            catch
            {
                return null;
            }
        }

        private void UpdateFriendsStatus(Client c)
        {
            Friend me = new Friend
            {
                FriendID = c.MyCharacter.CharacterId,
                FriendName = c.MyCharacter.Name,
                FriendOnline = c.MyCharacter.Online
            };

            Client friend;
            byte[] SendFriendOnline = null;
            foreach (Friend f in c.MyCharacter.FriendList)
            {
                friend = GetClientByCharacterID(f.FriendID);
                if (friend != null)
                {
                    f.FriendOnline = true; // could add friend connected here or something
                    SendFriendOnline = PacketManager.SendFriendOnlineStatus(me);
                    friend.Send(SendFriendOnline);
                }
                else
                    f.FriendOnline = false;
            }
        }

        private Portal GetNearbyPortal(Client c)
        {
            try
            {
                var portal = c.Map.Portals.Where(x => MapEngine.GetDistance(c.MyCharacter.Position.X, c.MyCharacter.Position.Y, x.FromX, x.FromY) <= x.Width).First();
                return portal;
            }
            catch
            {
                return null;
            }
        }

        void c_ClientLogPacket(object sender, LogPacketEventArgs e)
        {
            if (ClientLogPacket != null)
                ClientLogPacket(sender, e);
        }

        void c_ClientUnknownPacket(object sender, UnknownPacketEventArgs e)
        {
            if (ClientUnknownPacket != null)
                ClientUnknownPacket(sender, e);
        }

        #endregion

        #region Packet Stuff

        void c_ClientChannelLoginInfo(object sender, ClientChannelLoginInfoEventArgs e)
        {
            Client c = (Client)sender;
            ChannelLogin ch = e.ChannelLogin;

            Character cha = characterManager.GetCharacterByCharacterId(e.ChannelLogin.CharacterID);
            c.MyCharacter = cha;

            ClientConnectionChanged(null, new PlayerConnectionChangedEventArgs(c.MyCharacter, c.MyCharacter.Online));

            byte[] bufferSendMapID = PacketManager.SendMapID(cha.MapId);
            c.Send(bufferSendMapID);

            byte[] bufferSendMapTime = PacketManager.SendMapTime(7, 5, 25, 0);
            c.Send(bufferSendMapTime);

            byte[] bufferSendMapLoad2 = PacketManager.SendMapLoad2();
            c.Send(bufferSendMapLoad2);

            byte[] bufferSendMapLoad3 = PacketManager.SendMapLoad3();
            c.Send(bufferSendMapLoad3);
        }

        void c_ClientChannelChangeInfo(object sender, ClientChannelChangeEventArgs e)
        {
            Client c = (Client)sender;
            ChannelChangeInfo cci = e.Info;

            if (c.MyCharacter != null && c.MyCharacter.Online)
            {
                MapEngine mapEngine = GetMapEngine(c.MyCharacter.MapId);
                Client[] clients = GetClientsForChars(mapEngine.GetCharactersInRange(c.MyCharacter.Position, 150).ToArray());

                switch (cci.Something2)
                {
                    case 1:
                        c.MyCharacter.Online = false;
                        mapEngine.ActiveClientEntities.Remove(c.MyCharacter);
                        UpdateFriendsStatus(c);
                        if (c.MyCharacter.Guild != null)
                        {
                            GuildMember g = c.MyCharacter.Guild.Members.Single(x => x.Character.CharacterId == c.MyCharacter.CharacterId);
                            g.Online = false;
                            SendToGuild(c.MyCharacter.Guild, PacketManager.SendGuildMemberInfo(g));
                        }
                        if (c.MyCharacter.Party != null)
                        {
                            LeavePartyInfo i = new LeavePartyInfo
                            {
                                PartyID = c.MyCharacter.Party.PartyID
                            };

                            ClientLeaveParty(c);
                        }

                        ClientConnectionChanged(null, new PlayerConnectionChangedEventArgs(c.MyCharacter, false));
                        // send friends i has quit the game

                        byte[] bufferSendChannelChange = PacketManager.SendChannelChange(cci.Something, cci.Something2);
                        c.Send(bufferSendChannelChange);

                        byte[] bufferRemoveCharacter = null;

                        bufferRemoveCharacter = PacketManager.SendRemoveCharacter(c.MyCharacter, RemoveCharacterEffect.None);
                        SendToClients(bufferRemoveCharacter, clients);

                        if (c.MyCharacter.Pet != null && !c.MyCharacter.Pet.IsSealed)
                        {
                            mapEngine.ActiveClientEntities.Remove(c.MyCharacter.Pet);
                            byte[] bufferRemovePet = PacketManager.SendRemovePet(c.MyCharacter.Pet);
                            SendToClients(bufferRemovePet, clients);
                        }

                        break;

                    case 0:
                        c.MyCharacter.Online = false;
                        mapEngine.ActiveClientEntities.Remove(c.MyCharacter);
                        UpdateFriendsStatus(c);
                        if (c.MyCharacter.Guild != null)
                        {
                            GuildMember g = c.MyCharacter.Guild.Members.Single(x => x.Character.CharacterId == c.MyCharacter.CharacterId);
                            g.Online = false;
                            SendToGuild(c.MyCharacter.Guild, PacketManager.SendGuildMemberInfo(g));
                        }
                        if (c.MyCharacter.Party != null)
                        {
                            LeavePartyInfo i = new LeavePartyInfo
                            {
                                PartyID = c.MyCharacter.Party.PartyID
                            };
                            ClientLeaveParty(c);
                        }

                        ClientConnectionChanged(null, new PlayerConnectionChangedEventArgs(c.MyCharacter, false));
                        // send friends i has quit the game

                        accountManager.ExitGame(c.AccountID);


                        bufferRemoveCharacter = PacketManager.SendRemoveCharacter(c.MyCharacter, RemoveCharacterEffect.None);
                        SendToClients(bufferRemoveCharacter, clients);

                        if (c.MyCharacter.Pet != null && !c.MyCharacter.Pet.IsSealed)
                        {
                            mapEngine.ActiveClientEntities.Remove(c.MyCharacter.Pet);
                            byte[] bufferRemovePet = PacketManager.SendRemovePet(c.MyCharacter.Pet);
                            SendToClients(bufferRemovePet, clients);
                        }

                        break;
                }
            }
        }

        void c_ClientChannelStuffInfo(object sender, ClientChannelChangeStuffEventArgs e)
        {
            Client c = (Client)sender;
            Character[] charlist = characterManager.GetAllCharactersByAccountId(c.AccountID);

            byte[] bufferSendChannelChange2 = PacketManager.SendChannelChange(2, 0);
            c.Send(bufferSendChannelChange2);

            byte[] bufferSendCharList = PacketManager.SendCharacterList(charlist);
            c.Send(bufferSendCharList);

            if (c.MyCharacter != null)
            {
                MapEngine mapEngine = GetMapEngine(c.MyCharacter.MapId);
                Client[] clients = GetClientsForChars(mapEngine.GetCharactersInRange(c.MyCharacter.Position, 150).ToArray());
                mapEngine.ActiveClientEntities.Remove(c.MyCharacter);
                byte[] bufferRemoveCharacter = PacketManager.SendRemoveCharacter(c.MyCharacter, RemoveCharacterEffect.None);
                SendToClients(bufferRemoveCharacter, clients);

                if (c.MyCharacter.Pet != null && !c.MyCharacter.Pet.IsSealed)
                {
                    mapEngine.ActiveClientEntities.Remove(c.MyCharacter.Pet);
                    byte[] bufferRemovePet = PacketManager.SendRemovePet(c.MyCharacter.Pet);
                    SendToClients(bufferRemovePet, clients);
                }
            }
        }

        void c_ClientMapLoadInfo(object sender, ClientMapLoadEventArgs e)
        {
            Client c = (Client)sender;
            MapEngine newMap = GetMapEngine(e.Map.MapID);
            newMap.ActiveClientEntities.Add(c.MyCharacter);
            if (c.MyCharacter.Pet != null && !c.MyCharacter.Pet.IsSealed)
                newMap.ActiveClientEntities.Add(c.MyCharacter.Pet);
            //if (c.MyCharacter.tempPet != null) GOTTA FIX LIKE SKILLS YO BEFORE THESE
            //    newMap.ActiveClientEntities.Add(c.MyCharacter.tempPet);

            if (c.MyCharacter.OldMapId != 0)
            {
                MapEngine oldMap = GetMapEngine(c.MyCharacter.OldMapId);
                oldMap.ActiveClientEntities.Remove(c.MyCharacter);
                if (c.MyCharacter.Pet != null && !c.MyCharacter.Pet.IsSealed)
                    oldMap.ActiveClientEntities.Remove(c.MyCharacter.Pet);
                //if (c.MyCharacter.tempPet != null) GOTTA FIX LIKE SKILLS YO BEFORE THESE
                //    oldMap.ActiveClientEntities.Remove(c.MyCharacter.tempPet);
            }

            Map curMap = GetMapEngine(e.Map.MapID).Map;
            c.Map = curMap;
            c.MyCharacter.Map = curMap;
         
            characterManager.UpdateCharacter(c.MyCharacter);

            ClientConnectionChanged(null, new PlayerConnectionChangedEventArgs(c.MyCharacter, c.MyCharacter.Online));

            //c.MyCharacter.Position.X = (short)curMap.SpawnX;
            //c.MyCharacter.Position.Y = (short)curMap.SpawnY;

            byte[] bufferSendMap = PacketManager.SendMap(curMap);
            c.Send(bufferSendMap);

            byte[] bufferSendMap2 = PacketManager.SendMap2();
            c.Send(bufferSendMap2);
        }

        void c_ClientSkillLoadInfo(object sender, ClientSkillLoadEvenArgs e)
        {
            Client c = (Client)sender;
            int skill = e.SkillList;
            switch (skill)
            {
                case 0:
                    byte[] bufferSoftSkillList = PacketManager.SendShoftSkillList(c.MyCharacter.GetSoftSkills());
                    c.Send(bufferSoftSkillList);
                    break;

                case 1:
                    byte[] bufferHardSkillList = PacketManager.SendHardSkillList(c.MyCharacter.GetHardSkills());
                    c.Send(bufferHardSkillList);
                    break;
            }
        }

        void c_ClientInventoryLoadInfo(object sender, ClientInventoryLoadEvenArgs e)
        {
            Client c = (Client)sender;
            int inv = e.Inventory;


            switch (inv)
            {
                case 0:

                    byte[] bufferSendEquips = PacketManager.SendBagItems(c.MyCharacter.GetAllEquips().Cast<BaseItem>().ToList(), inv);
                    c.Send(bufferSendEquips);
                    break;

                case 1:
                    //byte[] bufferSendBag1 = PacketManager.SendTempBag1();
                    //c.Send(bufferSendBag1);
                    var bagOne = itemDataManager.GetAllItemsInBag(1, c.MyCharacter.CharacterId);
                    c.MyCharacter.Bags.Add(new Bag(bagOne));
                    if (c.MyCharacter.Bags[0].ItemCount > 0)
                    {
                        byte[] bufferSendBag1 = PacketManager.SendBagItems(c.MyCharacter.Bags[0].Items.Cast<BaseItem>().ToList(), inv);
                        c.Send(bufferSendBag1);
                    }
                    break;

                case 2:
                    var bagTwo = itemDataManager.GetAllItemsInBag(2, c.MyCharacter.CharacterId);
                    c.MyCharacter.Bags.Add(new Bag(bagTwo));
                    if (c.MyCharacter.Bags[1].ItemCount > 0)
                    {
                        byte[] bufferSendBag2 = PacketManager.SendBagItems(c.MyCharacter.Bags[1].Items.Cast<BaseItem>().ToList(), inv);
                        c.Send(bufferSendBag2);
                    }
                    break;

                case 3:
                    break;
            }
        }


        void c_ClientTradeAddItemInfo(object sender, ClientTradeAddItemEventArgs e)
        {
            Client c = (Client)sender;
            TradeAddItemInfo i = e.Info;
            Client p = null;
            BaseItem item = null;

            if (i.PlayerID != c.MyCharacter.CharacterId)
                p = GetClientByCharacterID(i.PlayerID);

            try
            {
                item = c.MyCharacter.Bags[i.FromBag - 1].Items.First(x => x.ItemID == i.ItemID);

                if (i.ToSlot != 255)
                {
                    item.TradeSlot = i.ToSlot;

                    if (!c.MyCharacter.TradeWindow.CheckTradeSlot(item, item.TradeSlot))
                    {
                        c.MyCharacter.TradeWindow.AddItem(item);
                    }
                    else
                    {
                        BagSlot bagSlot = new BagSlot();
                        if (!c.MyCharacter.FindFreeSlotInTradeWindow(item, bagSlot))
                        {
                            throw new BagIsFullException("BAG FULL");
                            //Throw exception here
                        }

                        item.TradeSlot = bagSlot.Slot;

                        c.MyCharacter.TradeWindow.AddItem(item);
                    }

                    c.Send(PacketManager.SendRemoveItem(i.FromBag, i.FromSlot));

                    byte[] SendTradeAddItem = PacketManager.SendTradeAddItem(i, item);
                    c.Send(SendTradeAddItem);
                    p.Send(SendTradeAddItem);

                    c.Send(PacketManager.SendAddItem(item));
                }
            }
            catch
            {
            }
        }

        void c_ClientTradeAddMoneyInfo(object sender, ClientTradeAddMoneyEventArgs e)
        {
            Client c = (Client)sender;
            TradeAddMoneyInfo i = e.Info;
            Client p = null;

            if (i.PlayerID != c.MyCharacter.CharacterId)
                p = GetClientByCharacterID(i.PlayerID);

            c.MyCharacter.TradeWindow.Money += i.Amount;

            c.Send(PacketManager.SendTradeAddMoney(c.MyCharacter.CharacterId, i.Amount));
            p.Send(PacketManager.SendTradeAddMoney(c.MyCharacter.CharacterId, i.Amount));
        }

        void c_ClientTradePlayerCommandsInfo(object sender, ClientTradePlayerCommandsEventArgs e)
        {
            Client c = (Client)sender;
            TradePlayerCommandsInfo i = e.Info;
            Client p = null;

            if (i.CharacterID != c.MyCharacter.CharacterId)
                p = GetClientByCharacterID(i.CharacterID);

            if (i.Error == TradePlayerCommands.Accept)
            {
                c.MyCharacter.TradeWindow.Accepted = true;

                if (c.MyCharacter.TradeWindow.Accepted && p.MyCharacter.TradeWindow.Accepted)
                {
                    // do item stuff here 
                    BaseItem[] Player1FakeTradeItems = null;
                    BaseItem[] Player2FakeTradeItems = null;

                    Bag[] Player1FakeBags = new Bag[]
                    {
                        new Bag(c.MyCharacter.Bags[0].Items.Select(x=>new BaseItem { ItemID = x.ItemID, SizeX = x.SizeX, SizeY = x.SizeY, Slot = x.Slot, Bag = x.Bag }).ToList()),
                        new Bag(c.MyCharacter.Bags[1].Items.Select(x=>new BaseItem { ItemID = x.ItemID, SizeX = x.SizeX, SizeY = x.SizeY, Slot = x.Slot, Bag = x.Bag }).ToList())
                    };

                    Bag[] Player2FakeBags = new Bag[]
                    {
                        new Bag(p.MyCharacter.Bags[0].Items.Select(x=>new BaseItem { ItemID = x.ItemID, SizeX = x.SizeX, SizeY = x.SizeY, Slot = x.Slot, Bag = x.Bag }).ToList()),
                        new Bag(p.MyCharacter.Bags[1].Items.Select(x=>new BaseItem { ItemID = x.ItemID, SizeX = x.SizeX, SizeY = x.SizeY, Slot = x.Slot, Bag = x.Bag }).ToList())
                    };

                    if (c.MyCharacter.TradeWindow.Items.Length > 0)
                    {
                        Player1FakeTradeItems = c.MyCharacter.TradeWindow.Items.Select(x => new BaseItem { ItemID = x.ItemID, SizeX = x.SizeX, SizeY = x.SizeY, Bag = x.Bag, Slot = x.Slot }).ToArray();
                        for (int j = 0; j < Player1FakeTradeItems.Length; j++)
                        {
                            BaseItem itemToRemove = Player1FakeBags[Player1FakeTradeItems[j].Bag - 1].Items.Where(x => x.ItemID == Player1FakeTradeItems[j].ItemID).FirstOrDefault();
                            if (itemToRemove != null)
                                Player1FakeBags[Player1FakeTradeItems[j].Bag - 1].RemoveItem(itemToRemove);
                        }
                    }
                    if (p.MyCharacter.TradeWindow.Items.Length > 0)
                    {
                        Player2FakeTradeItems = p.MyCharacter.TradeWindow.Items.Select(x => new BaseItem { ItemID = x.ItemID, SizeX = x.SizeX, SizeY = x.SizeY, Bag = x.Bag, Slot = x.Slot }).ToArray();
                        for (int j = 0; j < Player2FakeTradeItems.Length; j++)
                        {
                            BaseItem itemToRemove = Player2FakeBags[Player2FakeTradeItems[j].Bag - 1].Items.Where(x => x.ItemID == Player2FakeTradeItems[j].ItemID).FirstOrDefault();
                            if (itemToRemove != null)
                                Player2FakeBags[Player2FakeTradeItems[j].Bag - 1].RemoveItem(itemToRemove);
                        }
                    }


                    try
                    {
                        if (Player1FakeTradeItems != null)
                        {
                            for (int j = 0; j < Player1FakeTradeItems.Length; j++)
                            {
                                BagSlot newBagSlot = gameEngine.TryPickToBags(Player2FakeBags, Player1FakeTradeItems[j]);
                                Player1FakeTradeItems[j].Bag = newBagSlot.Bag;
                                Player1FakeTradeItems[j].Slot = newBagSlot.Slot;
                            }
                        }
                        if (Player2FakeTradeItems != null)
                        {
                            for (int j = 0; j < Player2FakeTradeItems.Length; j++)
                            {
                                BagSlot newBagSlot = gameEngine.TryPickToBags(Player1FakeBags, Player2FakeTradeItems[j]);
                                Player2FakeTradeItems[j].Bag = newBagSlot.Bag;
                                Player2FakeTradeItems[j].Slot = newBagSlot.Slot;
                            }
                        }

                        if (Player1FakeTradeItems != null)
                        {
                            if (Player2FakeTradeItems != null)
                            {
                                for (int j = 0; j < p.MyCharacter.TradeWindow.Items.Length; j++)
                                {
                                    p.MyCharacter.Bags[p.MyCharacter.TradeWindow.Items[j].Bag - 1].RemoveItem(p.MyCharacter.TradeWindow.Items[j]);

                                }
                            }

                            for (int j = 0; j < c.MyCharacter.TradeWindow.Items.Length; j++)
                            {

                                BaseItem tradedItem = c.MyCharacter.TradeWindow.Items[j];
                                byte oldPlayerBag = tradedItem.Bag;
                                byte oldPlayerSlot = tradedItem.Slot;
                                BagSlot newBagSlot = gameEngine.TryPickToBags(p.MyCharacter.Bags.ToArray(), tradedItem);

                                c.MyCharacter.Bags[oldPlayerBag - 1].RemoveItem(tradedItem);
                                byte[] SendRemoveFromBag = PacketManager.SendRemoveItem(oldPlayerBag, oldPlayerSlot);
                                c.Send(SendRemoveFromBag);

                                tradedItem.Bag = newBagSlot.Bag;
                                tradedItem.Slot = newBagSlot.Slot;
                                tradedItem.OwnerID = p.MyCharacter.CharacterId;

                                itemDataManager.UpdateItem(tradedItem);

                                byte[] SendAddItemToBag = PacketManager.SendAddItem(tradedItem);
                                p.Send(SendAddItemToBag);
                            }
                        }
                        if (Player2FakeTradeItems != null)
                        {
                            if (Player1FakeTradeItems != null)
                            {
                                for (int j = 0; j < c.MyCharacter.TradeWindow.Items.Length; j++)
                                {
                                    c.MyCharacter.Bags[c.MyCharacter.TradeWindow.Items[j].Bag - 1].RemoveItem(c.MyCharacter.TradeWindow.Items[j]);
                                }
                            }

                            for (int j = 0; j < p.MyCharacter.TradeWindow.Items.Length; j++)
                            {
                                BaseItem tradedItem = p.MyCharacter.TradeWindow.Items[j];
                                byte oldPlayerBag = tradedItem.Bag;
                                byte oldPlayerSlot = tradedItem.Slot;
                                BagSlot newBagSlot = gameEngine.TryPickToBags(c.MyCharacter.Bags.ToArray(), tradedItem);

                                p.MyCharacter.Bags[oldPlayerBag - 1].RemoveItem(tradedItem);
                                byte[] SendRemoveFromBag = PacketManager.SendRemoveItem(oldPlayerBag, oldPlayerSlot);
                                p.Send(SendRemoveFromBag);

                                tradedItem.Bag = newBagSlot.Bag;
                                tradedItem.Slot = newBagSlot.Slot;
                                tradedItem.OwnerID = c.MyCharacter.CharacterId;

                                itemDataManager.UpdateItem(tradedItem);
                                //c adds
                                byte[] SendAddItemToBag = PacketManager.SendAddItem(tradedItem);
                                c.Send(SendAddItemToBag);
                            }

                        }
                    }
                    catch (BagIsFullException)
                    {
                        //Send no room in bag and cancel trade
                        c.MyCharacter.TradeWindow = null;
                        p.MyCharacter.TradeWindow = null;
                        c.Send(PacketManager.SendTradeAcceptFinished(1));
                        p.Send(PacketManager.SendTradeAcceptFinished(1));
                        c.Send(PacketManager.SendTradePlayerError((byte)TradePlayerError.Cancel, p.MyCharacter.CharacterId));
                        p.Send(PacketManager.SendTradePlayerError((byte)TradePlayerError.Cancel, c.MyCharacter.CharacterId));
                        return;
                    };

                    c.MyCharacter.Money -= c.MyCharacter.TradeWindow.Money;
                    c.MyCharacter.Money += p.MyCharacter.TradeWindow.Money;

                    p.MyCharacter.Money -= p.MyCharacter.TradeWindow.Money;
                    p.MyCharacter.Money += c.MyCharacter.TradeWindow.Money;

                    c.MyCharacter.TradeWindow = null;
                    p.MyCharacter.TradeWindow = null;

                    c.Send(PacketManager.SendMoneyLeft(c.MyCharacter.Money));
                    p.Send(PacketManager.SendMoneyLeft(p.MyCharacter.Money));

                    c.Send(PacketManager.SendTradeAcceptFinished(0));
                    p.Send(PacketManager.SendTradeAcceptFinished(0));
                }
            }

            if (i.Error == TradePlayerCommands.Cancel)
            {
                c.MyCharacter.TradeWindow.Active = false;
                p.MyCharacter.TradeWindow.Active = false;
                c.MyCharacter.TradeWindow = null;
                p.MyCharacter.TradeWindow = null;
            }

            c.Send(PacketManager.SendTradePlayerError((byte)i.Error, p.MyCharacter.CharacterId));
            p.Send(PacketManager.SendTradePlayerError((byte)i.Error, c.MyCharacter.CharacterId));
        }

        void c_ClientTradePlayerInfo(object sender, ClientTradePlayerEventArgs e)
        {
            Client c = (Client)sender;
            TradePlayerInfo i = e.Info;
            Client p = null;

            if (i.CharacterID == c.MyCharacter.CharacterId)
                p = GetClientByCharacterID(i.CharacterID2);
            else
                p = GetClientByCharacterID(i.CharacterID);

            if (i.Error == TradePlayerError.None)
                p.Send(PacketManager.SendTradePlayer(i.Error, c.MyCharacter.CharacterId, p.MyCharacter.CharacterId)); // send request to other player for trade

            if (i.Error == TradePlayerError.Cancel)
            {
                c.Send(PacketManager.SendTradePlayerError((byte)i.Error, 0));
                c.Send(PacketManager.SendTradePlayer(i.Error, i.CharacterID, i.CharacterID2));

                p.Send(PacketManager.SendTradePlayerError((byte)i.Error, 0));
                p.Send(PacketManager.SendTradePlayer(i.Error, i.CharacterID, i.CharacterID2));
            }
            else if (i.Error == TradePlayerError.Accept)
            {
                c.Send(PacketManager.SendTradePlayerAccept(i.Error, p.MyCharacter.CharacterId));
                p.Send(PacketManager.SendTradePlayerAccept(i.Error, c.MyCharacter.CharacterId));

                c.MyCharacter.TradeWindow = new TradeWindow(new List<BaseItem>());
                p.MyCharacter.TradeWindow = new TradeWindow(new List<BaseItem>());
                c.MyCharacter.TradeWindow.Active = true;
                p.MyCharacter.TradeWindow.Active = true;
            }

        }

        void c_ClientAddItemToShopInfo(object sender, ClientAddItemToShopEventArgs e)
        {
            Client c = (Client)sender;
            AddItemToShopInfo i = e.Info;
            BaseItem item = null;


            if (i.Slot <= 36)
            {
                try
                {
                    item = c.MyCharacter.Bags[i.FromBag - 1].Items.First(x => x.ItemID == i.ItemID);

                    if (!c.MyCharacter.Shop.CheckSlot(item, i.Slot))
                    {
                        item.Slot = i.Slot;

                        c.MyCharacter.Bags[i.FromBag - 1].RemoveItem(item);
                        c.MyCharacter.Shop.AddItem(item);
                    }
                    else
                    {
                        BagSlot bagSlot = new BagSlot();
                        if (!c.MyCharacter.FindFreeSlotInShop(item, bagSlot))
                        {
                            throw new BagIsFullException("BAG FULL");
                            //Throw exception here
                        }

                        item.Slot = bagSlot.Slot;
                        item.Bag = bagSlot.Bag;

                        c.MyCharacter.Bags[i.FromBag - 1].RemoveItem(item);
                        c.MyCharacter.Shop.AddItem(item);
                        //   itemDataManager.UpdateItem(item);
                    }

                    item.SellPrice = i.Price;

                    byte[] SendAddItemToShopError = PacketManager.SendAddItemToShopError(AddItemToShopError.Success, item.SellPrice);
                    c.Send(SendAddItemToShopError);

                    byte[] SendRemoveItem = PacketManager.SendRemoveItem(i.FromBag, i.FromSlot);
                    c.Send(SendRemoveItem);

                    byte[] SendAddItemToShop = PacketManager.SendAddItemToShop(item, item.SellPrice);
                    c.Send(SendAddItemToShop);


                    itemDataManager.InsertShopItem(c.MyCharacter.Shop.ShopID, item.Slot, item.ItemID, item.SellPrice);
                }
                catch
                {
                    byte[] SendAddItemToShopError = PacketManager.SendAddItemToShopError(AddItemToShopError.CannotRegisterAnymoreBecauseYourStoreBagIsFull, 0);
                    c.Send(SendAddItemToShopError);
                }
            }
        }

        void c_ClientBuyOtherPlayerItemInfo(object sender, ClientBuyOtherPlayerShopItemEventArgs e)
        {
            Client c = (Client)sender;
            BuyOtherPlayerShopItem i = e.Info;
            Client ch = null;
            BaseItem item = null;

            try
            {
                ch = GetClientByCharacterID(i.CharacterID);
                item = ch.MyCharacter.ShopContainsItem(i.ItemID, i.FromSlot);
                BuyOtherPlayerShopItemError error = BuyOtherPlayerShopItemError.Success;

                if (i.ToSlot <= 36)
                {
                    if (item != null)
                    {
                        if (c.MyCharacter.Money >= item.SellPrice && i.Price == item.SellPrice)
                        {
                            if (!c.MyCharacter.Bags[i.ToBag - 1].CheckSlot(item, i.ToSlot))
                            {
                                ch.MyCharacter.Shop.TotalMoney += item.SellPrice;
                                byte[] SendRemoveShopItem = PacketManager.SendRemoveShopItem(item.Slot, item.ItemID, item.Amount);
                                ch.Send(SendRemoveShopItem);

                                byte[] SendShopTimeLeft = PacketManager.SendShopTimeLeft(ch.MyCharacter.Shop.TotalMoney, 50);
                                ch.Send(SendShopTimeLeft);

                                c.MyCharacter.Money -= item.SellPrice;
                                item.Slot = (byte)(i.ToSlot);
                                item.Bag = i.ToBag;
                                item.OwnerID = c.MyCharacter.CharacterId;

                                byte[] SendMoneyLeft = PacketManager.SendMoneyLeft(c.MyCharacter.Money);
                                c.Send(SendMoneyLeft);

                                byte[] SendAddItemToBag = PacketManager.SendAddItem(item);
                                c.Send(SendAddItemToBag);

                                ch.MyCharacter.Shop.RemoveItem(item);
                                c.MyCharacter.Bags[i.ToBag - 1].AddItem(item);
                                itemDataManager.UpdateItem(item);
                                itemDataManager.UpdateShopByCharacterID(ch.MyCharacter.Shop);
                                itemDataManager.RemoveShopItemByItemID(item.ItemID);
                            }
                        }
                        else
                        {
                            if (i.Price == item.SellPrice)
                                error = BuyOtherPlayerShopItemError.NoMoney;
                            else
                                error = BuyOtherPlayerShopItemError.PriceChanged;
                            // no money pl0x
                        }
                    }
                    else
                        error = BuyOtherPlayerShopItemError.ItemInfoHasBeenChanged;

                    byte[] SendBuyOtherPlayerItemError = PacketManager.SendBuyOtherPlayerShopItemError((byte)error);
                    c.Send(SendBuyOtherPlayerItemError);
                }
            }
            catch
            {
            }

        }

        void c_ClientOpenOtherPlayerShopInfo(object sender, ClientOpenOtherPlayerShopEventArgs e)
        {
            Client c = (Client)sender;
            OpenOtherPlayerShopInfo i = e.Info;
            Client ch = null;

            ch = GetClientByCharacterID(i.CharacterID);
            byte[] SendOtherPlayerShopItems = PacketManager.SendOtherPlayerShop(ch.MyCharacter);
            c.Send(SendOtherPlayerShopItems);
        }

        void c_ClientShopStartSellingInfo(object sender, ClientShopStartSellingEventArgs e)
        {
            Client c = (Client)sender;
            ShopStartSellingInfo i = e.Info;
            MapEngine mapEngine = GetMapEngine(c.MyCharacter.MapId);
            Client[] clients = GetClientsForChars(mapEngine.GetCharactersInRange(c.MyCharacter.Position, 150).ToArray());
    
            c.MyCharacter.Shop.Active = i.Selling;

            // it does send these both packets for both starting/ending selling
            byte[] SendShopTimeLeft = PacketManager.SendShopTimeLeft(c.MyCharacter.Shop.TotalMoney, 50); // need later add the items time left for this
            c.Send(SendShopTimeLeft);

            byte[] SendStartSelling = PacketManager.SendShopStartSelling(Convert.ToByte(c.MyCharacter.Shop.Active));
            c.Send(SendStartSelling);

            byte[] SendChangeShopInfo = PacketManager.SendChangeShopInfo(c.MyCharacter.Shop);
            SendToClients(SendChangeShopInfo, clients);
        }

        void c_ClientShopTakeMoneyInfo(object sender, ClientShopTakeMoneyInfoEventArgs e)
        {
            Client c = (Client)sender;
            ShopTakeMoneyInfo i = e.Info;

            if (c.MyCharacter.Shop.TotalMoney == i.Amount)
            {
                // blabla add money stored proc thing

                c.MyCharacter.Money += c.MyCharacter.Shop.TotalMoney;
                byte[] SendTotalMoney = PacketManager.SendMoneyLeft(c.MyCharacter.Money);
                c.Send(SendTotalMoney);

                // useless feature for now
                //byte[] SendTaxes = PacketManager.SendShopTaxesAmount(c.MyCharacter.Shop);
                //c.Send(SendTaxes);

                c.MyCharacter.Shop.TotalMoney = 0;

                characterManager.UpdateCharacter(c.MyCharacter);
                itemDataManager.UpdateShopByCharacterID(c.MyCharacter.Shop);

                byte[] SendUpdatedShop = PacketManager.SendPlayerShop(c.MyCharacter, 50);
                c.Send(SendUpdatedShop);
            }
        }


        void c_ClientChangeShopInfo(object sender, ClientChangeShopInfoEventArgs e)
        {
            Client c = (Client)sender;
            ChangeShopInfo i = e.Info;
            MapEngine mapEngine = GetMapEngine(c.MyCharacter.MapId);
            Client[] clients = GetClientsForChars(mapEngine.GetCharactersInRange(c.MyCharacter.Position, 150).ToArray());

            byte[] SendChangeShopInfoError = PacketManager.SendChangeShopInfoError(0);
            c.Send(SendChangeShopInfoError);

            c.MyCharacter.Shop.ShopName = i.Name;
            c.MyCharacter.Shop.ShopDesc = i.Description;

            itemDataManager.UpdateShopByCharacterID(c.MyCharacter.Shop);

            byte[] SendChangeShopInfo = PacketManager.SendChangeShopInfo(c.MyCharacter.Shop);
            SendToClients(SendChangeShopInfo, clients);
        }

        void c_ClientMoveShopItemToBagInfo(object sender, ClientMoveShopItemToBagEventArgs e)
        {
            Client c = (Client)sender;
            MoveShopItemToBagInfo m = e.Info;
            BaseItem item = null;

            item = c.MyCharacter.Shop.Items.First(x => x.ItemID == m.ItemID);

            if (m.ToSlot <= 36)
            {
                if (!c.MyCharacter.Bags[m.ToBag - 1].CheckSlot(item, m.ToSlot))
                {
                    item.Slot = (byte)(m.ToSlot);
                    item.Bag = m.ToBag;

                    byte[] SendRemoveShopItem = PacketManager.SendRemoveShopItem(m.FromSlot, item.ItemID, item.Amount);
                    c.Send(SendRemoveShopItem);

                    byte[] SendMoveShopItemToBagError = PacketManager.SendMoveShopItemToBagError(0);
                    c.Send(SendMoveShopItemToBagError);

                    byte[] SendAddItemToBag = PacketManager.SendAddItem(item);
                    c.Send(SendAddItemToBag);

                    c.MyCharacter.Shop.RemoveItem(item);
                    c.MyCharacter.Bags[m.ToBag - 1].AddItem(item);
                    itemDataManager.UpdateItem(item);
                    itemDataManager.RemoveShopItemByItemID(item.ItemID);
                }
            }
        }

        void c_ClientMoveShopItemInfo(object sender, ClientMoveShopItemEventArgs e)
        {
            Client c = (Client)sender;
            MoveShopItemInfo m = e.Info;
            BaseItem item = null;

            try
            {
                item = c.MyCharacter.Shop.Items.First(x => x.ItemID == m.ItemID);

                if (MoveShopItem(c, m, item))
                {
                }
                else
                {
                    byte[] SendNotEnoughtSpace = PacketManager.SendAddItemToShopError(AddItemToShopError.ItemDoesNotExist, 0);
                    c.Send(SendNotEnoughtSpace);
                }
            }
            catch
            {
            }
        }

        private bool MoveShopItem(Client c, MoveShopItemInfo m, BaseItem item)
        {
            BaseItem itemunder = null;
            if (m.ItemUnderID != 0)
                itemunder = c.MyCharacter.Shop.Items.First(x => x.ItemID == m.ItemUnderID);

            if (m.ToSlot <= 36)
            {
                if (c.MyCharacter.Shop.MoveItem(c.MyCharacter, item, m.ToSlot, out itemunder))
                {
                    if (itemunder == null)
                    {
                        item.Slot = (byte)(m.ToSlot);

                        itemDataManager.UpdateShopItemById(item);
                    }
                    else
                    {
                        int tempslot;
                        tempslot = item.Slot;
                        item.Slot = itemunder.Slot;
                        itemunder.Slot = (byte)tempslot;

                        itemDataManager.UpdateShopItemById(item);
                        itemDataManager.UpdateShopItemById(itemunder);
                    }

                    byte[] SendRemoveShopItem = PacketManager.SendRemoveShopItem(m.FromSlot, item.ItemID, item.Amount);
                    c.Send(SendRemoveShopItem);

                    if (itemunder != null)
                    {
                        byte[] SendRemoveShopItemUnder = PacketManager.SendRemoveShopItem(m.ToSlot, itemunder.ItemID, itemunder.Amount);
                        c.Send(SendRemoveShopItemUnder);

                        byte[] SendAddShopItemUnder = PacketManager.SendAddItemToShop(itemunder, itemunder.SellPrice);
                        c.Send(SendAddShopItemUnder);
                    }

                    byte[] SendAddShopItem = PacketManager.SendAddItemToShop(item, item.SellPrice);
                    c.Send(SendAddShopItem);
                    return true;
                }
            }

            return false;
        }

        void c_ClientMoveWarehouseItemToBagInfo(object sender, ClientMoveWarehouseItemToBagEventArgs e)
        {
            Client c = (Client)sender;
            MoveWarehouseItemToBagInfo m = e.Info;
            BaseItem item = null;

            item = c.MyCharacter.Warehouse.Items.First(x => x.ItemID == m.ItemID);

            if (m.ToSlot <= 36)
            {
                if (!c.MyCharacter.Bags[m.ToBag - 1].CheckSlot(item, m.ToSlot))
                {
                    item.Slot = (byte)(m.ToSlot);
                    item.Bag = m.ToBag;
                    item.OwnerID = c.MyCharacter.CharacterId;

                    byte[] SendRemoveWarehouseItem = PacketManager.SendWarehouseRemoveItem(m.FromSlot, item.ItemID);
                    c.Send(SendRemoveWarehouseItem);

                    byte[] SendAddItemToBag = PacketManager.SendAddItem(item);
                    c.Send(SendAddItemToBag);

                    c.MyCharacter.Warehouse.RemoveItem(item);
                    c.MyCharacter.Bags[m.ToBag - 1].AddItem(item);
                    itemDataManager.UpdateItem(item);
                }
            }
        }

        void c_ClientOpenWarehouseInfo(object sender, ClientOpenWarehouseEventArgs e)
        {
            Client c = (Client)sender;
            OpenWarehouseInfo i = e.Info;


            if (c.MyCharacter.Warehouse == null)
            {
                var warehouse = itemDataManager.GetWarehouseItemsByAccountId(c.MyCharacter.AccountId);
                c.MyCharacter.Warehouse = new Warehouse(warehouse);
            }

            byte[] SendWarehouseItems = PacketManager.SendWarehouseItems(c.MyCharacter.Warehouse.Items.ToList(), 1);
            c.Send(SendWarehouseItems);
        }


        void c_ClientAddItemToWarehouseInfo(object sender, ClientAddItemToWarehouseEventargs e)
        {
            Client c = (Client)sender;
            AddItemToWarehouseInfo i = e.Info;

            if (i.ToSlot <= 65)
            {
                try
                {
                    BaseItem item = c.MyCharacter.Bags[i.FromBag - 1].Items.First(x => x.ItemID == i.ItemID);

                    int storagecost = Math.Abs(item.Price / 100);

                    if (c.MyCharacter.Money >= storagecost)
                    {

                        if (!c.MyCharacter.Warehouse.CheckSlot(item, i.ToSlot))
                        {
                            item.Slot = (byte)(i.ToSlot);
                            item.Bag = 5;

                            c.MyCharacter.Bags[i.FromBag - 1].RemoveItem(item);
                            c.MyCharacter.Warehouse.AddItem(item);
                            itemDataManager.UpdateItem(item);
                        }
                        else
                        {
                            BagSlot bagSlot = new BagSlot();
                            if (!c.MyCharacter.FindFreeSlotInWarehouse(item, bagSlot))
                            {
                                throw new BagIsFullException("BAG FULL");
                                //Throw exception here
                            }

                            item.Slot = bagSlot.Slot;
                            item.Bag = bagSlot.Bag;

                            c.MyCharacter.Bags[i.FromBag - 1].RemoveItem(item);
                            c.MyCharacter.Warehouse.AddItem(item);
                            itemDataManager.UpdateItem(item);
                        }


                        c.MyCharacter.Money -= storagecost;

                        c.Send(PacketManager.SendRemoveItem(i.FromBag, i.FromSlot));
                        c.Send(PacketManager.SendAddItemToWarehouse(1, item));
                        c.Send(PacketManager.SendMoneyLeft(c.MyCharacter.Money));
                    }
                    else
                    {
                        c.Send(PacketManager.SendBuyOtherPlayerShopItemError((byte)BuyOtherPlayerShopItemError.NoMoney));
                    }
                }
                catch (BagIsFullException)
                {
                    byte[] SendWarehouseError = PacketManager.SendWarehouseError(WarehouseError.YourStashIsFull);
                    c.Send(SendWarehouseError);
                }

            }
        }

        void c_ClientMoveWarehouseItemInfo(object sender, ClientMoveWarehouseItemEventArgs e)
        {
            Client c = (Client)sender;
            MoveWarehouseItemInfo m = e.Info;
            BaseItem item = null;

            try
            {
                item = c.MyCharacter.Warehouse.Items.First(x => x.ItemID == m.ItemID);

                if (MoveWarehouseItem(c, m, item))
                {
                }
                else
                {
                    byte[] SendNotEnoughtSpace = PacketManager.SendWarehouseMoveItemError(WarehouseError.NotEnoughtSpace);
                    c.Send(SendNotEnoughtSpace);
                }
            }
            catch
            {
            }
        }

        private bool MoveWarehouseItem(Client c, MoveWarehouseItemInfo m, BaseItem item)
        {
            BaseItem itemunder = null;
            if (m.ItemUnderID != 0)
                itemunder = c.MyCharacter.Warehouse.Items.First(x => x.ItemID == m.ItemUnderID);

            if (m.ToSlot <= 65)
            {
                if (c.MyCharacter.Warehouse.MoveItem(c.MyCharacter, item, m.ToSlot, out itemunder))
                {
                    if (itemunder == null)
                    {
                        item.Slot = (byte)(m.ToSlot);

                        itemDataManager.UpdateItem(item);
                    }
                    else
                    {
                        int tempslot;
                        tempslot = item.Slot;
                        item.Slot = itemunder.Slot;
                        itemunder.Slot = (byte)tempslot;

                        itemDataManager.UpdateItem(item);
                        itemDataManager.UpdateItem(itemunder);
                    }

                    byte[] SendRemoveWarehouseItem = PacketManager.SendWarehouseRemoveItem(m.FromSlot, item.ItemID);
                    c.Send(SendRemoveWarehouseItem);

                    byte[] SendAddWarehouseItem = PacketManager.SendAddItemToWarehouse(2, item);
                    c.Send(SendAddWarehouseItem);

                    if (itemunder != null)
                    {
                        byte[] SendRemoveWarehouseItemUnder = PacketManager.SendWarehouseRemoveItem(m.ToSlot, itemunder.ItemID);
                        c.Send(SendRemoveWarehouseItemUnder);

                        byte[] SendAddWarehouseItemUnder = PacketManager.SendAddItemToWarehouse(2, itemunder);
                        c.Send(SendAddWarehouseItemUnder);
                    }

                    return true;
                }
            }

            return false;
        }

        void c_ClientMoveItemInfo(object sender, ClientMoveItemEventArgs e)
        {
            Client c = (Client)sender;
            MoveItemInfo m = e.Info;
            BaseItem item = null;

            try
            {
                if (m.FromBag > 0)
                {
                    item = c.MyCharacter.Bags[m.FromBag - 1].Items.First(x => x.ItemID == m.ItemID);
                }
                else
                {
                    switch (m.FromSlot)
                    {
                        case (byte)Slot.Weapon: // Hand
                            item = c.MyCharacter.Hand;
                            break;

                        case (byte)Slot.Hat: // Hat
                            item = c.MyCharacter.Head;
                            break;

                        case (byte)Slot.Armor: // Armor
                            item = c.MyCharacter.Body;
                            break;

                        case (byte)Slot.Shoes: // Shoes
                            item = c.MyCharacter.Feet;
                            break;

                        case (byte)Slot.CrystalMirror: // crystalmirror
                            item = c.MyCharacter.Charm;
                            break;

                        case (byte)Slot.Ring:
                            item = c.MyCharacter.Ring;
                            break;

                        case (byte)Slot.Necklace:
                            item = c.MyCharacter.Necklace;
                            break;

                        case (byte)Slot.Cape:
                            item = c.MyCharacter.Cape;
                            break;

                        case (byte)Slot.Mirror:
                            item = c.MyCharacter.Mirror;
                            break;
                        default:
                            break;
                    }
                }
            }
            catch
            {
            }
            // ADD MOVE ITEM FUNC TO THAT BAG CLASS BLABLA

            try
            {
                if (m.ToBag > 0 && m.FromBag != 0)
                {
                    if (HandleItemMove(c, m, item))
                    {
                        if (item.Slot != 255)
                        {
                            byte[] SendMoveItem = PacketManager.SendMoveItem(m, 0);
                            c.Send(SendMoveItem);
                        }
                    }
                    else
                    {
                        byte[] SendError = PacketManager.SendMoveItem(m, 2);
                        c.Send(SendError);
                    }
                }
                else
                {
                    if (m.FromBag != 0)
                        HandleEquipItem(c, m, item);
                    else
                        HandleUnEquipItem(c, m, item);
                }
            }
            catch (InvalidItemSlotException)
            {

            }
        }

        private void HandleUnEquipItem(Client c, MoveItemInfo m, BaseItem item)
        {
            try
            {
                MapEngine mapEngine = GetMapEngine(c.MyCharacter.MapId);
                Client[] clients = GetClientsForChars(mapEngine.GetCharactersInRange(c.MyCharacter.Position, 150).ToArray());

                if (m.ToBag != 255 && m.ToSlot != 255)
                {
                    if (!c.MyCharacter.Bags[m.ToBag - 1].IsSlotValid(item, m.ToSlot))
                    {
                        throw new InvalidItemSlotException("NO PUT ON EDGE PUJJ");
                    }

                    if (!c.MyCharacter.Bags[m.ToBag - 1].CheckSlot(item, m.ToSlot))
                    {
                        switch (item.Slot)
                        {
                            case (byte)Slot.Weapon: // Hand
                                c.MyCharacter.Hand = null;
                                break;

                            case (byte)Slot.Hat: // Hat
                                c.MyCharacter.Head = null;
                                break;

                            case (byte)Slot.Armor: // Armor
                                c.MyCharacter.Body = null;
                                break;

                            case (byte)Slot.Shoes: // Shoes
                                c.MyCharacter.Feet = null;
                                break;

                            case (byte)Slot.Ring:
                                c.MyCharacter.Ring = null;
                                break;

                            case (byte)Slot.Necklace:
                                c.MyCharacter.Necklace = null;
                                break;

                            case (byte)Slot.Cape:
                                c.MyCharacter.Cape = null;
                                break;

                            case (byte)Slot.Mirror:
                                c.MyCharacter.Mirror = null;
                                break;

                            default:
                                break;
                        }

                        item.Bag = m.ToBag;
                        item.Slot = m.ToSlot;
                        c.MyCharacter.Bags[m.ToBag - 1].AddItem(item);
                        itemDataManager.UpdateItem(item);

                        byte[] SendMoveItem = PacketManager.SendMoveItem(m, 0);
                        c.Send(SendMoveItem);

                        byte[] SendNewStats = PacketManager.SendStats(c.MyCharacter);
                        c.Send(SendNewStats);

                        byte[] SendNewVisuals = PacketManager.SendSpawnPlayers(c.MyCharacter);
                        SendToClients(SendNewVisuals, clients);
                    }
                    else
                    {
                        byte[] SendError = PacketManager.SendMoveItem(m, 2);
                        c.Send(SendError);
                    }
                }
            }
            catch
            {
                byte[] SendError = PacketManager.SendMoveItem(m, 2);
                c.Send(SendError);
            }
        }

        private void HandleEquipItem(Client c, MoveItemInfo m, BaseItem item)
        {
            try
            {
                BaseItem conflict = null;
                MapEngine mapEngine = GetMapEngine(c.MyCharacter.MapId);
                Client[] clients = GetClientsForChars(mapEngine.GetCharactersInRange(c.MyCharacter.Position, 150).ToArray());

                switch (m.ToSlot)
                {
                    case (byte)Slot.Weapon: // Hand
                        conflict = c.MyCharacter.Hand;
                        if (conflict != null)
                        {
                            conflict.Slot = item.Slot;
                            conflict.Bag = item.Bag;
                        }
                        c.MyCharacter.Hand = (Hand)item;
                        break;

                    case (byte)Slot.Hat: // Hat
                        conflict = c.MyCharacter.Head;
                        if (conflict != null)
                        {
                            conflict.Slot = item.Slot;
                            conflict.Bag = item.Bag;
                        }
                        c.MyCharacter.Head = (Head)item;
                        break;

                    case (byte)Slot.Armor: // Armor
                        conflict = c.MyCharacter.Body;
                        if (conflict != null)
                        {
                            conflict.Slot = item.Slot;
                            conflict.Bag = item.Bag;
                        }
                        c.MyCharacter.Body = (Body)item;
                        break;

                    case (byte)Slot.Shoes: // Shoes
                        conflict = c.MyCharacter.Feet;
                        if (conflict != null)
                        {
                            conflict.Slot = item.Slot;
                            conflict.Bag = item.Bag;
                        }
                        c.MyCharacter.Feet = (Feet)item;
                        break;

                    case (byte)Slot.Ring:
                        conflict = c.MyCharacter.Ring;
                        if (conflict != null)
                        {
                            conflict.Slot = item.Slot;
                            conflict.Bag = item.Bag;
                        }
                        c.MyCharacter.Ring = (Ring)item;
                        break;

                    case (byte)Slot.Necklace:
                        conflict = c.MyCharacter.Necklace;
                        if (conflict != null)
                        {
                            conflict.Slot = item.Slot;
                            conflict.Bag = item.Bag;
                        }
                        c.MyCharacter.Necklace = (Necklace)item;
                        break;

                    case (byte)Slot.Cape:
                        conflict = c.MyCharacter.Cape;
                        if (conflict != null)
                        {
                            conflict.Slot = item.Slot;
                            conflict.Bag = item.Bag;
                        }
                        c.MyCharacter.Cape = (Cape)item;
                        break;

                    case (byte)Slot.Mirror:
                        conflict = c.MyCharacter.Mirror;
                        if (conflict != null)
                        {
                            conflict.Slot = item.Slot;
                            conflict.Bag = item.Bag;
                        }
                        c.MyCharacter.Mirror = (Mirror)item;
                        break;
                    default:
                        break;
                }

                if (conflict != null)
                {
                    c.MyCharacter.Bags[m.FromBag - 1].AddItem(conflict);
                    itemDataManager.UpdateItem(conflict);
                }

                c.MyCharacter.Bags[m.FromBag - 1].RemoveItem(item);
                itemDataManager.UpdateItem(item);

                byte[] SendEquipItem = PacketManager.SendMoveItem(m, 0);
                c.Send(SendEquipItem);

                byte[] SendNewStats = PacketManager.SendStats(c.MyCharacter);
                c.Send(SendNewStats);

                if (c.MyCharacter.ImperialSet != ImperialSet.None)
                {
                    SendCommandHelp(string.Format("Wearing set: {0}", c.MyCharacter.ImperialSet.ToString()), c);
                }

                byte[] SendNewVisuals = PacketManager.SendSpawnPlayers(c.MyCharacter);
                SendToClients(SendNewVisuals, clients);
            }
            catch (RequiredClassException)
            {
                byte[] SendEquipError = PacketManager.SendMoveItem(m, 3);
                c.Send(SendEquipError);
            }
        }

        private bool HandleItemMove(Client c, MoveItemInfo m, BaseItem item)
        {
            BaseItem test;

            try
            {
                if (m.ItemIDUnder != 0)
                    test = c.MyCharacter.Bags[m.ToBag - 1].Items.First(x => x.ItemID == m.ItemIDUnder);


                if (m.ToSlot != 255)
                {
                    if (c.MyCharacter.Bags[m.ToBag - 1].MoveItem(c.MyCharacter, item, m.ToSlot, out test))
                    {
                        if (test == null)
                        {
                            item.Slot = (byte)(m.ToSlot);
                            item.Bag = (byte)(m.ToBag);

                            itemDataManager.UpdateItem(item);
                        }
                        else
                        {
                            int tempslot, tempbag;
                            tempslot = item.Slot;
                            tempbag = item.Bag;
                            item.Slot = test.Slot;
                            item.Bag = test.Bag;
                            test.Slot = (byte)tempslot;
                            test.Bag = (byte)tempbag;

                            //if (m.FromBag != m.ToBag && m.FromBag > 0)
                            //{
                            //    c.MyCharacter.Bags[m.FromBag - 1].RemoveItem(item);
                            //    c.MyCharacter.Bags[m.ToBag - 1].RemoveItem(test);
                            //    c.MyCharacter.Bags[m.ToBag - 1].AddItem(item);
                            //    c.MyCharacter.Bags[m.FromBag - 1].AddItem(test);
                            //}

                            itemDataManager.UpdateItem(item);
                            itemDataManager.UpdateItem(test);
                        }

                        if (m.FromBag != m.ToBag && m.FromBag > 0)
                        {
                            c.MyCharacter.Bags[m.FromBag - 1].RemoveItem(item);
                            c.MyCharacter.Bags[m.ToBag - 1].AddItem(item);

                            if (test != null)
                            {
                                c.MyCharacter.Bags[m.ToBag - 1].RemoveItem(test);
                                c.MyCharacter.Bags[m.FromBag - 1].AddItem(test);
                            }
                        }

                        return true;
                    }
                }

                return false;
            }
            catch
            {
                return false;
            }
        }


        void c_ClientDropItemInfo(object sender, ClientDropItemEventArgs e)
        {
            Client c = (Client)sender;
            DropItemInfo d = e.Info;
            MapEngine mapEngine = GetMapEngine(c.MyCharacter.MapId);
            try
            {

                //Equipment item = channelManager.GetItemByItemID(d.ItemID);

                BaseItem item = c.MyCharacter.Bags[d.FromBag - 1].Items.Where(x => x.ItemID == d.ItemID).First();

                MapItem m = mapEngine.PlayerDropItem(c.MyCharacter.CharacterId, item, d.ToX, d.ToY, d.ToZ, c.Map.MapID);

                c.MyCharacter.Bags[d.FromBag - 1].RemoveItem(item);

                Client[] clients = GetClientsForChars(mapEngine.GetCharactersInRange(c.MyCharacter.Position, 150).ToArray());

                byte[] SendRemoveItem = PacketManager.SendRemoveItem(d.FromBag, d.FromSlot);
                c.Send(SendRemoveItem);

                byte[] SendItemOnGround = PacketManager.SendItemOnGround(m);
                SendToClients(SendItemOnGround, clients);
            }
            catch
            {
            }
        }

        void c_ClientPickItemInfo(object sender, ClientPickItemEventArgs e)
        {
            Client c = (Client)sender;
            PickItemInfo p = e.Info;

            //Equipment item = (Equipment)channelManager.GetItemByItemID(p.ItemID);
            BaseItem item = null;
            MapEngine mapEngine = GetMapEngine(c.MyCharacter.MapId);
            Client[] clients = GetClientsForChars(mapEngine.GetCharactersInRange(c.MyCharacter.Position, 150).ToArray());
            try
            {
                item = mapEngine.PickupItem(p.MapItemID, c, p.Amount);
            }
            catch (BagIsFullException)
            {
                byte[] bufferBagIsFull = PacketManager.SendBagIsFull();
                c.Send(bufferBagIsFull);
                //send bag is full
            }

            //.MyCharacter.Bags[0].AddItem(item);
            if (item == null)
            {
                //send item not exist
                return;
            }

            if (item is Jeon)
            {
                c.MyCharacter.Money += item.Amount;
                characterManager.UpdateCharacter(c.MyCharacter);

                byte[] SendMoneyLeft = PacketManager.SendMoneyLeft(c.MyCharacter.Money);
                c.Send(SendMoneyLeft);
            }
            else
            {
                // will crash if this is send when you pick up money so better do it like this..
                byte[] SendPickUp = PacketManager.SendAddItem(item);
                c.Send(SendPickUp);
            }

            byte[] SendPickUpText = PacketManager.SendPickUpText(item.ReferenceID, item.Amount, 1);
            c.Send(SendPickUpText);

            byte[] SendRemoveMapItem = PacketManager.SendRemoveMapItem(p.MapItemID);
            SendToClients(SendRemoveMapItem, clients);
        }

        void c_ClientStackItemInfo(object sender, ClientStackItemInfoEventArgs e)
        {
            Client c = (Client)sender;
            StackItemInfo i = e.Info;

            BaseItem item = c.MyCharacter.Bags[i.Bag - 1].Items.Single(x => (x.ItemID == i.ItemID && x.Slot == i.Slot));
            BaseItem stackItem = c.MyCharacter.Bags[i.StackBag - 1].Items.Single(x => (x.ItemID == i.StackItemID && x.Slot == i.StackSlot));

            if (item is Potion && stackItem is Potion)
            {
                if (item.ReferenceID == stackItem.ReferenceID)
                {
                    if (stackItem.Amount + item.Amount <= 100)
                    {
                        // allow stack
                        stackItem.Amount += item.Amount;

                        c.Send(PacketManager.SendItemsLeft(stackItem.Bag, stackItem.Slot, stackItem.ItemID, stackItem.Amount));

                        c.Send(PacketManager.SendRemoveItem(item.Bag, item.Slot));

                        c.MyCharacter.Bags[i.Bag - 1].RemoveItem(item);
                        itemDataManager.DeleteItem(item.ItemID);
                        itemDataManager.UpdateItem(stackItem);
                    }
                }
            }
            if (item is Bead && stackItem is Bead)
            {
                if (item.ReferenceID == stackItem.ReferenceID)
                {
                    if (stackItem.Amount + item.Amount <= 100)
                    {
                        stackItem.Amount += item.Amount;

                        c.Send(PacketManager.SendItemsLeft(stackItem.Bag, stackItem.Slot, stackItem.ItemID, stackItem.Amount));

                        c.Send(PacketManager.SendRemoveItem(item.Bag, item.Slot));

                        c.MyCharacter.Bags[i.Bag - 1].RemoveItem(item);
                        itemDataManager.DeleteItem(item.ItemID);
                        itemDataManager.UpdateItem(stackItem);
                    }
                }
            }
        }

        void c_ClientImbueItemInfo(object sender, ClientImbueItemEventArgs e)
        {
            Client c = (Client)sender;
            ImbueItemInfo i = e.Info;
            BaseItem item = null;
            try
            {
                item = c.MyCharacter.Bags[i.Bag - 1].Items.Single(x => (x.ItemID == i.ItemID && x.Slot == i.Slot) && (x.bType <= 7));

                byte[] SendAcceptImbueItem = PacketManager.SendAcceptImbueItem(ImbueState.Success);
                if (item != null)
                    c.Send(SendAcceptImbueItem);
            }
            catch
            {
                byte[] SendAcceptImbueItem = PacketManager.SendAcceptImbueItem(ImbueState.ThisItemCannotBeImbued);
                c.Send(SendAcceptImbueItem);
            }
        }

        #region OlderImbueCode
        //void c_ClientAcceptImbueItemInfo(object sender, ClientAcceptImbueItemEventArgs e)
        //{
        //    Client c = (Client)sender;
        //    AcceptImbueItem i = e.Info;
        //    Equipment Item = null;
        //    BaseItem item = null;
        //    BaseItem imbueItem = null;
        //    ImbueError error = ImbueError.FailedToRemake;
        //    ImbueStat stat = ImbueStat.CriticalHit;
        //    List<ImbueStat> WhiteStats = null;

        //    try
        //    {
        //        imbueItem = c.MyCharacter.Bags[i.ImbueItem1Bag - 1].Items.Single(x => (x.ItemID == i.ImbueItem1ID));

        //        if (imbueItem is ImbueItem)
        //        {
        //            item = c.MyCharacter.Bags[i.ToImbueItemBag - 1].Items.Single(x => (x.ItemID == i.ToImbueItemID && x.Slot == i.ToImbueItemSlot));
        //            if (item is Equipment)
        //                Item = item as Equipment;

        //            double SuccessChance = 33;
        //            int increaseAbility = 0;

        //            if (Item.Plus == 0 && imbueItem is Black || Item.Slvl == 0 && imbueItem is White)
        //                SuccessChance = 100;

        //            if (imbueItem is ImbueItem)
        //            {
        //                ImbueItem imbueitem = imbueItem as ImbueItem;
        //                SuccessChance += (double)imbueitem.ImbueChance;
        //                increaseAbility += imbueitem.IncreaseValue;
        //            }
        //            else
        //            {
        //                // give error etc cannt imbue with potion or something...
        //            }

        //            int value = (int)(Item.RequiredLevel * 0.33 + increaseAbility);

        //            if (imbueItem is Black)
        //            {
        //                double increaseFromPlus = 0;

        //                if (Item.Plus > 0)
        //                    increaseFromPlus += (value * 0.05) * Item.Plus;

        //                if (Item.Plus > 30)
        //                    increaseFromPlus *= 2.5;

        //                value += (int)increaseFromPlus;

        //                if (mapEngine.PercentSuccess(SuccessChance))
        //                {
        //                    if (Item is Hand)
        //                    {
        //                        Item.Damage += (short)value;
        //                        stat = ImbueStat.Damage;
        //                    }
        //                    if (Item is Cape)
        //                    {
        //                        Item.Defence += (short)value;
        //                        Item.Damage += (short)value;
        //                        stat = ImbueStat.DefenseAndDamage;
        //                    }
        //                    if (Item is Head)
        //                    {
        //                        Item.AttackRating += (short)value;
        //                        stat = ImbueStat.AttackRating;
        //                    }
        //                    if (Item is Body)
        //                    {
        //                        Item.Defence += (short)value;
        //                        stat = ImbueStat.Defense;
        //                    }
        //                    if (Item is Feet)
        //                    {
        //                        value = 2;
        //                        Item.Critical += (short)value;
        //                        stat = ImbueStat.CriticalHit;
        //                    }
        //                    if (Item is Ring)
        //                    {
        //                        Item.IncMaxLife += (short)value;
        //                        stat = ImbueStat.MaxLife;
        //                    }
        //                    if (Item is Necklace)
        //                    {
        //                        Item.IncMaxMana += (short)value;
        //                        stat = ImbueStat.MaxMana;
        //                    }


        //                    Item.Plus++;
        //                    error = ImbueError.Success;
        //                }
        //                else
        //                {
        //                    error = ImbueError.FailedToRemake;
        //                    stat = ImbueStat.None;
        //                    value = -value;

        //                    if (Item.Plus >= 4 && mapEngine.PercentSuccess(20))
        //                    {
        //                        if (Item is Hand)
        //                        {
        //                            Item.Damage += (short)value;
        //                            stat = ImbueStat.Damage;
        //                        }
        //                        if (Item is Cape)
        //                        {
        //                            Item.Defence += (short)value;
        //                            Item.Damage += (short)value;
        //                            stat = ImbueStat.DefenseAndDamage;
        //                        }
        //                        if (Item is Head)
        //                        {
        //                            Item.AttackRating += (short)value;
        //                            stat = ImbueStat.AttackRating;
        //                        }
        //                        if (Item is Body)
        //                        {
        //                            Item.Defence += (short)value;
        //                            stat = ImbueStat.Defense;
        //                        }
        //                        if (Item is Feet)
        //                        {
        //                            value = -2;
        //                            Item.Critical += (short)value;
        //                            stat = ImbueStat.CriticalHit;
        //                        }
        //                        if (Item is Ring)
        //                        {
        //                            Item.IncMaxLife += (short)value;
        //                            stat = ImbueStat.MaxLife;
        //                        }
        //                        if (Item is Necklace)
        //                        {
        //                            Item.IncMaxMana += (short)value;
        //                            stat = ImbueStat.MaxMana;
        //                        }

        //                        Item.Plus--;
        //                    }
        //                }
        //            }
        //            if (imbueItem is White)
        //            {
        //                WhiteStats = new List<ImbueStat>();
        //                WhiteStats.Add(ImbueStat.Damage);
        //                WhiteStats.Add(ImbueStat.Defense);
        //                WhiteStats.Add(ImbueStat.AttackRating);
        //                WhiteStats.Add(ImbueStat.MaxLife);
        //                WhiteStats.Add(ImbueStat.MaxMana);
        //                WhiteStats.Add(ImbueStat.LifeReg);
        //                WhiteStats.Add(ImbueStat.ManaReg);
        //                WhiteStats.Add(ImbueStat.CriticalHit);

        //                if (Item is Cape)
        //                {
        //                    WhiteStats.Remove(ImbueStat.Damage);
        //                    WhiteStats.Remove(ImbueStat.Defense);
        //                }
        //                if (Item is Hand)
        //                {
        //                    Hand weapon = Item as Hand;
        //                    WhiteStats.Remove(weapon.NotWithWhite);
        //                }
        //                if (Item is Head)
        //                {
        //                    Head hat = Item as Head;
        //                    WhiteStats.Remove(hat.NotWithWhite);
        //                }
        //                if (Item is Body)
        //                {
        //                    Body armor = Item as Body;
        //                    WhiteStats.Remove(armor.NotWithWhite);
        //                }
        //                if (Item is Ring)
        //                {
        //                    Ring ring = Item as Ring;
        //                    value *= (int)2.94;
        //                }
        //                if (Item is Necklace)
        //                {
        //                    Necklace neck = Item as Necklace;
        //                    value *= (int)1.85;
        //                }

        //                int randomStat = mapEngine.Random.Next(0, WhiteStats.Count);
        //                stat = WhiteStats[randomStat];

        //                if (Item is Ring || Item is Necklace)
        //                    value += 0;
        //                else
        //                    value /= (int)2.5;

        //                double increaseFromSlvl = 0;

        //                if (Item.Slvl > 0)
        //                    increaseFromSlvl += (value * 0.05) * Item.Slvl;

        //                if (Item.Slvl > 30)
        //                    increaseFromSlvl *= 2.5;

        //                value += (int)increaseFromSlvl;

        //                if (stat == ImbueStat.CriticalHit)
        //                    value = 2;

        //                if (mapEngine.PercentSuccess(SuccessChance))
        //                {
        //                    switch (stat)
        //                    {
        //                        case ImbueStat.Damage:
        //                            Item.Damage += (short)value;
        //                            break;

        //                        case ImbueStat.Defense:
        //                            Item.Defence += (short)value;
        //                            break;

        //                        case ImbueStat.AttackRating:
        //                            Item.AttackRating += (short)value;
        //                            break;

        //                        case ImbueStat.MaxLife:
        //                            Item.IncMaxLife += (short)value;
        //                            break;

        //                        case ImbueStat.MaxMana:
        //                            Item.IncMaxMana += (short)value;
        //                            break;

        //                        case ImbueStat.LifeReg:
        //                            Item.IncLifeRegen += (short)value;
        //                            break;

        //                        case ImbueStat.ManaReg:
        //                            Item.IncManaRegen += (short)value;
        //                            break;

        //                        case ImbueStat.CriticalHit:
        //                            Item.Critical += (short)value;
        //                            break;
        //                    }

        //                    Item.Slvl++;
        //                    error = ImbueError.Success;
        //                }
        //                else
        //                {
        //                    error = ImbueError.FailedToRemake;
        //                    stat = ImbueStat.None;

        //                    if (Item.Slvl >= 4 && mapEngine.PercentSuccess(20))
        //                    {
        //                        Item.Slvl--;
        //                        if (Item is Cape)
        //                        {
        //                            if (Item.AttackRating == 0)
        //                                WhiteStats.Remove(ImbueStat.AttackRating);
        //                            if (Item.IncMaxLife == 0)
        //                                WhiteStats.Remove(ImbueStat.MaxLife);
        //                            if (Item.IncMaxMana == 0)
        //                                WhiteStats.Remove(ImbueStat.MaxMana);
        //                            if (Item.IncLifeRegen == 0)
        //                                WhiteStats.Remove(ImbueStat.LifeReg);
        //                            if (Item.IncManaRegen == 0)
        //                                WhiteStats.Remove(ImbueStat.ManaReg);
        //                            if (Item.Critical == 0)
        //                                WhiteStats.Remove(ImbueStat.CriticalHit);
        //                        }
        //                        if (Item is Hand)
        //                        {
        //                            if (Item.Defence == 0)
        //                                WhiteStats.Remove(ImbueStat.Defense);
        //                            if (Item.AttackRating == 0)
        //                                WhiteStats.Remove(ImbueStat.AttackRating);
        //                            if (Item.IncMaxLife == 0)
        //                                WhiteStats.Remove(ImbueStat.MaxLife);
        //                            if (Item.IncMaxMana == 0)
        //                                WhiteStats.Remove(ImbueStat.MaxMana);
        //                            if (Item.IncLifeRegen == 0)
        //                                WhiteStats.Remove(ImbueStat.LifeReg);
        //                            if (Item.IncManaRegen == 0)
        //                                WhiteStats.Remove(ImbueStat.ManaReg);
        //                            if (Item.Critical == 0)
        //                                WhiteStats.Remove(ImbueStat.CriticalHit);
        //                        }
        //                        if (Item is Head)
        //                        {
        //                            if (Item.Damage == 0)
        //                                WhiteStats.Remove(ImbueStat.Damage);
        //                            if (Item.Defence == 0)
        //                                WhiteStats.Remove(ImbueStat.Defense);
        //                            if (Item.IncMaxLife == 0)
        //                                WhiteStats.Remove(ImbueStat.MaxLife);
        //                            if (Item.IncMaxMana == 0)
        //                                WhiteStats.Remove(ImbueStat.MaxMana);
        //                            if (Item.IncLifeRegen == 0)
        //                                WhiteStats.Remove(ImbueStat.LifeReg);
        //                            if (Item.IncManaRegen == 0)
        //                                WhiteStats.Remove(ImbueStat.ManaReg);
        //                            if (Item.Critical == 0)
        //                                WhiteStats.Remove(ImbueStat.CriticalHit);
        //                        }
        //                        if (Item is Body)
        //                        {
        //                            if (Item.Damage == 0)
        //                                WhiteStats.Remove(ImbueStat.Damage);
        //                            if (Item.AttackRating == 0)
        //                                WhiteStats.Remove(ImbueStat.AttackRating);
        //                            if (Item.IncMaxLife == 0)
        //                                WhiteStats.Remove(ImbueStat.MaxLife);
        //                            if (Item.IncMaxMana == 0)
        //                                WhiteStats.Remove(ImbueStat.MaxMana);
        //                            if (Item.IncLifeRegen == 0)
        //                                WhiteStats.Remove(ImbueStat.LifeReg);
        //                            if (Item.IncManaRegen == 0)
        //                                WhiteStats.Remove(ImbueStat.ManaReg);
        //                            if (Item.Critical == 0)
        //                                WhiteStats.Remove(ImbueStat.CriticalHit);
        //                        }
        //                        if (Item is Feet)
        //                        {
        //                            if (Item.Defence == 0)
        //                                WhiteStats.Remove(ImbueStat.Defense);
        //                            if (Item.Damage == 0)
        //                                WhiteStats.Remove(ImbueStat.Damage);
        //                            if (Item.AttackRating == 0)
        //                                WhiteStats.Remove(ImbueStat.AttackRating);
        //                            if (Item.IncMaxLife == 0)
        //                                WhiteStats.Remove(ImbueStat.MaxLife);
        //                            if (Item.IncMaxMana == 0)
        //                                WhiteStats.Remove(ImbueStat.MaxMana);
        //                            if (Item.IncLifeRegen == 0)
        //                                WhiteStats.Remove(ImbueStat.LifeReg);
        //                            if (Item.IncManaRegen == 0)
        //                                WhiteStats.Remove(ImbueStat.ManaReg);
        //                            if (Item.Critical == 0)
        //                                WhiteStats.Remove(ImbueStat.CriticalHit);
        //                        }
        //                        if (Item is Ring)
        //                        {
        //                            if (Item.Defence == 0)
        //                                WhiteStats.Remove(ImbueStat.Defense);
        //                            if (Item.Damage == 0)
        //                                WhiteStats.Remove(ImbueStat.Damage);
        //                            if (Item.AttackRating == 0)
        //                                WhiteStats.Remove(ImbueStat.AttackRating);
        //                            if (Item.IncMaxLife == 0)
        //                                WhiteStats.Remove(ImbueStat.MaxLife);
        //                            if (Item.IncMaxMana == 0)
        //                                WhiteStats.Remove(ImbueStat.MaxMana);
        //                            if (Item.IncLifeRegen == 0)
        //                                WhiteStats.Remove(ImbueStat.LifeReg);
        //                            if (Item.IncManaRegen == 0)
        //                                WhiteStats.Remove(ImbueStat.ManaReg);
        //                            if (Item.Critical == 0)
        //                                WhiteStats.Remove(ImbueStat.CriticalHit);
        //                        }
        //                        if (Item is Necklace)
        //                        {
        //                            if (Item.Defence == 0)
        //                                WhiteStats.Remove(ImbueStat.Defense);
        //                            if (Item.Damage == 0)
        //                                WhiteStats.Remove(ImbueStat.Damage);
        //                            if (Item.AttackRating == 0)
        //                                WhiteStats.Remove(ImbueStat.AttackRating);
        //                            if (Item.IncMaxLife == 0)
        //                                WhiteStats.Remove(ImbueStat.MaxLife);
        //                            if (Item.IncMaxMana == 0)
        //                                WhiteStats.Remove(ImbueStat.MaxMana);
        //                            if (Item.IncLifeRegen == 0)
        //                                WhiteStats.Remove(ImbueStat.LifeReg);
        //                            if (Item.IncManaRegen == 0)
        //                                WhiteStats.Remove(ImbueStat.ManaReg);
        //                            if (Item.Critical == 0)
        //                                WhiteStats.Remove(ImbueStat.CriticalHit);
        //                        }

        //                        randomStat = mapEngine.Random.Next(0, WhiteStats.Count);
        //                        stat = WhiteStats[randomStat];

        //                        switch (stat)
        //                        {
        //                            case ImbueStat.Damage:
        //                                value = Item.Damage;
        //                                Item.Damage = 0;
        //                                break;

        //                            case ImbueStat.Defense:
        //                                value = Item.Defence;
        //                                Item.Defence = 0;
        //                                break;

        //                            case ImbueStat.AttackRating:
        //                                value = Item.AttackRating;
        //                                Item.AttackRating = 0;
        //                                break;

        //                            case ImbueStat.MaxLife:
        //                                value = Item.IncMaxLife;
        //                                Item.IncMaxLife = 0;
        //                                break;

        //                            case ImbueStat.MaxMana:
        //                                value = Item.IncMaxMana;
        //                                Item.IncMaxMana = 0;
        //                                break;

        //                            case ImbueStat.LifeReg:
        //                                value = Item.IncLifeRegen;
        //                                Item.IncLifeRegen = 0;
        //                                break;

        //                            case ImbueStat.ManaReg:
        //                                value = Item.IncManaRegen;
        //                                Item.IncManaRegen = 0;
        //                                break;

        //                            case ImbueStat.CriticalHit:
        //                                value = Item.Critical;
        //                                Item.Critical = 0;
        //                                break;
        //                        }

        //                        value = -value;
        //                    }
        //                }
        //            }
        //            if (imbueItem is Red)
        //            {
        //                value = -increaseAbility;
        //                stat = ImbueStat.RequiredLevel;

        //                if (mapEngine.PercentSuccess(SuccessChance))
        //                {
        //                    if (Item.RequiredLevel > increaseAbility)
        //                    {
        //                        error = ImbueError.Success;
        //                        Item.RequiredLevel -= (short)increaseAbility;
        //                    }
        //                }
        //                else
        //                {
        //                    stat = ImbueStat.None;
        //                    error = ImbueError.FailedToRemake;
        //                }
        //            }

        //            c.Send(PacketManager.SendImbueMessage(error, stat, value, Item));

        //            c.Send(PacketManager.SendRemoveItem(i.ImbueItem1Bag, i.ImbueItem1Slot));

        //            itemDataManager.DeleteItem(imbueItem.ItemID);
        //            c.MyCharacter.Bags[i.ImbueItem1Bag - 1].RemoveItem(imbueItem);

        //            Item.ImbueTries++;
        //            c.Send(PacketManager.SendImbuedItem(Item));
        //            itemDataManager.UpdateItem(Item);
        //        }
        //    }
        //    catch
        //    {
        //    }
        //} 
        #endregion

        #region OldImbueCode
        //void c_ClientAcceptImbueItemInfo(object sender, ClientAcceptImbueItemEventArgs e)
        //{
        //    Client c = (Client)sender;
        //    AcceptImbueItem i = e.Info;
        //    int itemCount = 1;
        //    BaseItem item = null;
        //    BaseItem imbueItem = null, imbueItem2 = null, imbueItem3 = null;  // Stone\Crystal\Jewel
        //    ImbueError error = ImbueError.FailedToRemake;   // Default error = fail
        //    ImbueStat stat = ImbueStat.CriticalHit;     // Default stat to imbue = critical
        //    List<ImbueStat> WhiteStats = null;      // List of possible stats to imbue

        //    try
        //    {
        //        imbueItem = c.MyCharacter.Bags[i.ImbueItem1Bag - 1].Items.Single(x => (x.ItemID == i.ImbueItem1ID));
        //        if (!(imbueItem is ImbueItem))      // If not using imbue item
        //        {
        //            // Output error - not imbue item
        //            return;
        //        }
        //        ImbueItem imbueitem = imbueItem as ImbueItem;

        //        item = c.MyCharacter.Bags[i.ToImbueItemBag - 1].Items.Single(x => (x.ItemID == i.ToImbueItemID && x.Slot == i.ToImbueItemSlot));
        //        if (!(item is Equipment))      // See if item can be imbued
        //        {
        //            // Output error - not imbuable item
        //            return;
        //        }
        //        Equipment Item = item as Equipment;

        //        int SuccessChance = 75;     // Success rate offset
        //        int increaseAbility = 0;    // Mehhh - to be removed

        //        if (i.ImbueItem2ID != 0)
        //        {
        //            imbueItem2 = c.MyCharacter.Bags[i.ImbueItem2Bag - 1].Items.Single(x => (x.ItemID == i.ImbueItem2ID));
        //            itemCount++;
        //            if (i.ImbueItem3ID != 0)
        //            {
        //                imbueItem3 = c.MyCharacter.Bags[i.ImbueItem3Bag - 1].Items.Single(x => (x.ItemID == i.ImbueItem3ID));
        //                itemCount++;
        //            }
        //        }

        //        SuccessChance += imbueitem.ImbueChance * itemCount;
        //        increaseAbility += imbueitem.IncreaseValue;     // To be removed too

        //        int value = (int)(Item.RequiredLevel);        // Base increase value is req level

        //        if (imbueItem is Black)     // Using a black imbue item
        //        {
        //            // Increase value increases according to plus and type of rock
        //            value += (int)(((value * 0.1) * Item.Plus) + ((imbueitem.ImbueChance / 5) * Item.Plus));

        //            if (mapEngine.PercentSuccess(SuccessChance - Item.Plus * 4))      // Attempt imbue
        //            {
        //                gameEngine.BlackImbue(Item, ref stat, imbueitem, 1);        // Imbue the item
        //                Item.Plus++;        // Increase plus by 1
        //                error = ImbueError.Success;     // Return success message
        //            }
        //            else   // Failed to imbue
        //            {
        //                error = ImbueError.FailedToRemake;      // Error failed to imbue
        //                stat = ImbueStat.None;      // No stat imbued
        //                if (Item.Plus >= 4 && mapEngine.PercentSuccess(Item.Plus * 2))    // Drop?
        //                {
        //                    gameEngine.BlackImbue(Item, ref stat, imbueitem, -1);       // Lose value
        //                    Item.Plus--;        // Reduce plus by 1
        //                }
        //                value *= -1;
        //            }
        //        }
        //        else if (imbueItem is White)
        //        {
        //            value += (int)(((value * 0.1) * Item.Slvl) + ((imbueitem.ImbueChance / 5) * Item.Slvl));
        //            WhiteStats = new List<ImbueStat>();
        //            WhiteStats.Add(ImbueStat.Damage);
        //            WhiteStats.Add(ImbueStat.Defense);
        //            WhiteStats.Add(ImbueStat.AttackRating);
        //            WhiteStats.Add(ImbueStat.MaxLife);
        //            WhiteStats.Add(ImbueStat.MaxMana);
        //            WhiteStats.Add(ImbueStat.LifeReg);
        //            WhiteStats.Add(ImbueStat.ManaReg);
        //            WhiteStats.Add(ImbueStat.CriticalHit);

        //            if (Item is Cape)
        //            {
        //                WhiteStats.Remove(ImbueStat.Damage);
        //                WhiteStats.Remove(ImbueStat.Defense);
        //            }
        //            if (Item is Hand)
        //            {
        //                Hand weapon = Item as Hand;
        //                WhiteStats.Remove(weapon.NotWithWhite);
        //            }
        //            if (Item is Head)
        //            {
        //                Head hat = Item as Head;
        //                WhiteStats.Remove(hat.NotWithWhite);
        //            }
        //            if (Item is Body)
        //            {
        //                Body armor = Item as Body;
        //                WhiteStats.Remove(armor.NotWithWhite);
        //            }
        //            if (Item is Ring)
        //            {
        //                Ring ring = Item as Ring;
        //                value *= (int)2.94;
        //            }
        //            if (Item is Necklace)
        //            {
        //                Necklace neck = Item as Necklace;
        //                value *= (int)1.85;
        //            }

        //            int randomStat = mapEngine.Random.Next(0, WhiteStats.Count);
        //            stat = WhiteStats[randomStat];

        //            if (mapEngine.PercentSuccess(SuccessChance - Item.Slvl * 4))
        //            {
        //                gameEngine.WhiteImbue(Item, stat, value);
        //                Item.Slvl++;
        //                error = ImbueError.Success;
        //            }
        //            else
        //            {
        //                error = ImbueError.FailedToRemake;
        //                stat = ImbueStat.None;

        //                if (Item.Slvl >= 4 && mapEngine.PercentSuccess(Item.Slvl * 2))
        //                {
        //                    Item.Slvl--;
        //                    if (Item is Cape)
        //                    {
        //                        if (Item.AttackRating == 0)
        //                            WhiteStats.Remove(ImbueStat.AttackRating);
        //                        if (Item.IncMaxLife == 0)
        //                            WhiteStats.Remove(ImbueStat.MaxLife);
        //                        if (Item.IncMaxMana == 0)
        //                            WhiteStats.Remove(ImbueStat.MaxMana);
        //                        if (Item.IncLifeRegen == 0)
        //                            WhiteStats.Remove(ImbueStat.LifeReg);
        //                        if (Item.IncManaRegen == 0)
        //                            WhiteStats.Remove(ImbueStat.ManaReg);
        //                        if (Item.Critical == 0)
        //                            WhiteStats.Remove(ImbueStat.CriticalHit);
        //                    }
        //                    if (Item is Hand)
        //                    {
        //                        if (Item.Defence == 0)
        //                            WhiteStats.Remove(ImbueStat.Defense);
        //                        if (Item.AttackRating == 0)
        //                            WhiteStats.Remove(ImbueStat.AttackRating);
        //                        if (Item.IncMaxLife == 0)
        //                            WhiteStats.Remove(ImbueStat.MaxLife);
        //                        if (Item.IncMaxMana == 0)
        //                            WhiteStats.Remove(ImbueStat.MaxMana);
        //                        if (Item.IncLifeRegen == 0)
        //                            WhiteStats.Remove(ImbueStat.LifeReg);
        //                        if (Item.IncManaRegen == 0)
        //                            WhiteStats.Remove(ImbueStat.ManaReg);
        //                        if (Item.Critical == 0)
        //                            WhiteStats.Remove(ImbueStat.CriticalHit);
        //                    }
        //                    if (Item is Head)
        //                    {
        //                        if (Item.Damage == 0)
        //                            WhiteStats.Remove(ImbueStat.Damage);
        //                        if (Item.Defence == 0)
        //                            WhiteStats.Remove(ImbueStat.Defense);
        //                        if (Item.IncMaxLife == 0)
        //                            WhiteStats.Remove(ImbueStat.MaxLife);
        //                        if (Item.IncMaxMana == 0)
        //                            WhiteStats.Remove(ImbueStat.MaxMana);
        //                        if (Item.IncLifeRegen == 0)
        //                            WhiteStats.Remove(ImbueStat.LifeReg);
        //                        if (Item.IncManaRegen == 0)
        //                            WhiteStats.Remove(ImbueStat.ManaReg);
        //                        if (Item.Critical == 0)
        //                            WhiteStats.Remove(ImbueStat.CriticalHit);
        //                    }
        //                    if (Item is Body)
        //                    {
        //                        if (Item.Damage == 0)
        //                            WhiteStats.Remove(ImbueStat.Damage);
        //                        if (Item.AttackRating == 0)
        //                            WhiteStats.Remove(ImbueStat.AttackRating);
        //                        if (Item.IncMaxLife == 0)
        //                            WhiteStats.Remove(ImbueStat.MaxLife);
        //                        if (Item.IncMaxMana == 0)
        //                            WhiteStats.Remove(ImbueStat.MaxMana);
        //                        if (Item.IncLifeRegen == 0)
        //                            WhiteStats.Remove(ImbueStat.LifeReg);
        //                        if (Item.IncManaRegen == 0)
        //                            WhiteStats.Remove(ImbueStat.ManaReg);
        //                        if (Item.Critical == 0)
        //                            WhiteStats.Remove(ImbueStat.CriticalHit);
        //                    }
        //                    if (Item is Feet)
        //                    {
        //                        if (Item.Defence == 0)
        //                            WhiteStats.Remove(ImbueStat.Defense);
        //                        if (Item.Damage == 0)
        //                            WhiteStats.Remove(ImbueStat.Damage);
        //                        if (Item.AttackRating == 0)
        //                            WhiteStats.Remove(ImbueStat.AttackRating);
        //                        if (Item.IncMaxLife == 0)
        //                            WhiteStats.Remove(ImbueStat.MaxLife);
        //                        if (Item.IncMaxMana == 0)
        //                            WhiteStats.Remove(ImbueStat.MaxMana);
        //                        if (Item.IncLifeRegen == 0)
        //                            WhiteStats.Remove(ImbueStat.LifeReg);
        //                        if (Item.IncManaRegen == 0)
        //                            WhiteStats.Remove(ImbueStat.ManaReg);
        //                        if (Item.Critical == 0)
        //                            WhiteStats.Remove(ImbueStat.CriticalHit);
        //                    }
        //                    if (Item is Ring)
        //                    {
        //                        if (Item.Defence == 0)
        //                            WhiteStats.Remove(ImbueStat.Defense);
        //                        if (Item.Damage == 0)
        //                            WhiteStats.Remove(ImbueStat.Damage);
        //                        if (Item.AttackRating == 0)
        //                            WhiteStats.Remove(ImbueStat.AttackRating);
        //                        if (Item.IncMaxLife == 0)
        //                            WhiteStats.Remove(ImbueStat.MaxLife);
        //                        if (Item.IncMaxMana == 0)
        //                            WhiteStats.Remove(ImbueStat.MaxMana);
        //                        if (Item.IncLifeRegen == 0)
        //                            WhiteStats.Remove(ImbueStat.LifeReg);
        //                        if (Item.IncManaRegen == 0)
        //                            WhiteStats.Remove(ImbueStat.ManaReg);
        //                        if (Item.Critical == 0)
        //                            WhiteStats.Remove(ImbueStat.CriticalHit);
        //                    }
        //                    if (Item is Necklace)
        //                    {
        //                        if (Item.Defence == 0)
        //                            WhiteStats.Remove(ImbueStat.Defense);
        //                        if (Item.Damage == 0)
        //                            WhiteStats.Remove(ImbueStat.Damage);
        //                        if (Item.AttackRating == 0)
        //                            WhiteStats.Remove(ImbueStat.AttackRating);
        //                        if (Item.IncMaxLife == 0)
        //                            WhiteStats.Remove(ImbueStat.MaxLife);
        //                        if (Item.IncMaxMana == 0)
        //                            WhiteStats.Remove(ImbueStat.MaxMana);
        //                        if (Item.IncLifeRegen == 0)
        //                            WhiteStats.Remove(ImbueStat.LifeReg);
        //                        if (Item.IncManaRegen == 0)
        //                            WhiteStats.Remove(ImbueStat.ManaReg);
        //                        if (Item.Critical == 0)
        //                            WhiteStats.Remove(ImbueStat.CriticalHit);
        //                    }

        //                    randomStat = mapEngine.Random.Next(0, WhiteStats.Count);
        //                    stat = WhiteStats[randomStat];

        //                    if (mapEngine.PercentSuccess(50))
        //                    {
        //                        switch (stat)
        //                        {
        //                            case ImbueStat.Damage:
        //                                value = Item.Damage;
        //                                Item.Damage = 0;
        //                                break;

        //                            case ImbueStat.Defense:
        //                                value = Item.Defence;
        //                                Item.Defence = 0;
        //                                break;

        //                            case ImbueStat.AttackRating:
        //                                value = Item.AttackRating;
        //                                Item.AttackRating = 0;
        //                                break;

        //                            case ImbueStat.MaxLife:
        //                                value = Item.IncMaxLife;
        //                                Item.IncMaxLife = 0;
        //                                break;

        //                            case ImbueStat.MaxMana:
        //                                value = Item.IncMaxMana;
        //                                Item.IncMaxMana = 0;
        //                                break;

        //                            case ImbueStat.LifeReg:
        //                                value = Item.IncLifeRegen;
        //                                Item.IncLifeRegen = 0;
        //                                break;

        //                            case ImbueStat.ManaReg:
        //                                value = Item.IncManaRegen;
        //                                Item.IncManaRegen = 0;
        //                                break;

        //                            case ImbueStat.CriticalHit:
        //                                value = Item.Critical;
        //                                Item.Critical = 0;
        //                                break;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        switch (stat)
        //                        {
        //                            case ImbueStat.Damage:
        //                                Item.Damage -= (Item.Damage < (short)value) ? Item.Damage : (short)value;
        //                                break;

        //                            case ImbueStat.Defense:
        //                                Item.Defence -= (Item.Defence < (short)value) ? Item.Defence : (short)value;
        //                                break;

        //                            case ImbueStat.AttackRating:
        //                                Item.AttackRating -= (Item.AttackRating < (short)value) ? Item.AttackRating : (short)value;
        //                                break;

        //                            case ImbueStat.MaxLife:
        //                                Item.IncMaxLife -= (Item.IncMaxLife < (short)value) ? Item.IncMaxLife : (short)value;
        //                                break;

        //                            case ImbueStat.MaxMana:
        //                                Item.IncMaxMana -= (Item.IncMaxMana < (short)value) ? Item.IncMaxMana : (short)value;
        //                                break;

        //                            case ImbueStat.LifeReg:
        //                                Item.IncLifeRegen -= (Item.IncLifeRegen < (short)value) ? Item.IncLifeRegen : (short)value;
        //                                break;

        //                            case ImbueStat.ManaReg:
        //                                Item.IncManaRegen -= (Item.IncManaRegen < (short)value) ? Item.IncManaRegen : (short)value;
        //                                break;

        //                            case ImbueStat.CriticalHit:
        //                                value = (int)(Math.Round((double)(value / 50))) + 1;
        //                                Item.Critical -= (Item.Critical < (short)value) ? Item.Critical : (short)value;
        //                                break;
        //                        }
        //                    }
        //                    value *= -1;
        //                }
        //            }
        //        }
        //        if (imbueItem is Red)
        //        {
        //            value = -imbueitem.IncreaseValue;
        //            //SuccessChance -= Item.Durability / 100 - Item.RequiredLevel;
        //            int randStat = 0;
        //            stat = ImbueStat.None;
        //            while (stat == ImbueStat.None)
        //            {
        //                randStat = mapEngine.Random.Next(8, 12);
        //                stat = gameEngine.ChooseStat(randStat);
        //                if (stat == ImbueStat.RequiredLevel && Item.RequiredLevel > 1)
        //                    error = gameEngine.RedImbue(Item, stat, value);
        //                else if (stat == ImbueStat.RequiredStrength && Item.RequiredStrength > 1)
        //                    error = gameEngine.RedImbue(Item, stat, value);
        //                else if (stat == ImbueStat.RequiredStamina && Item.RequiredStamina > 1)
        //                    error = gameEngine.RedImbue(Item, stat, value);
        //                else if (stat == ImbueStat.RequiredDexterity && Item.RequiredDexterity > 1)
        //                    error = gameEngine.RedImbue(Item, stat, value);
        //                else if (stat == ImbueStat.RequiredEnergy && Item.RequiredEnergy > 1)
        //                    error = gameEngine.RedImbue(Item, stat, value);
        //                else
        //                    stat = ImbueStat.None;
        //            }

        //            /*if (mapEngine.PercentSuccess(SuccessChance))
        //            {
        //                if (Item.RequiredLevel > increaseAbility)
        //                {
        //                    error = ImbueError.Success;
        //                    Item.RequiredLevel -= (short)increaseAbility;
        //                }
        //            }
        //            else
        //            {
        //                stat = ImbueStat.None;
        //                error = ImbueError.FailedToRemake;
        //            }*/
        //        }

        //        c.Send(PacketManager.SendImbueMessage(error, stat, value, Item));

        //        c.Send(PacketManager.SendRemoveItem(i.ImbueItem1Bag, i.ImbueItem1Slot));

        //        itemDataManager.DeleteItem(imbueItem.ItemID);
        //        c.MyCharacter.Bags[i.ImbueItem1Bag - 1].RemoveItem(imbueItem);

        //        if (imbueItem2 != null)
        //        {
        //            c.Send(PacketManager.SendRemoveItem(i.ImbueItem2Bag, i.ImbueItem2Slot));
        //            itemDataManager.DeleteItem(imbueItem2.ItemID);
        //            c.MyCharacter.Bags[i.ImbueItem2Bag - 1].RemoveItem(imbueItem2);
        //            if (imbueItem3 != null)
        //            {
        //                c.Send(PacketManager.SendRemoveItem(i.ImbueItem3Bag, i.ImbueItem3Slot));
        //                itemDataManager.DeleteItem(imbueItem3.ItemID);
        //                c.MyCharacter.Bags[i.ImbueItem3Bag - 1].RemoveItem(imbueItem3);
        //            }
        //        }

        //        Item.ImbueTries++;
        //        c.Send(PacketManager.SendImbuedItem(Item));
        //        itemDataManager.UpdateItem(Item);

        //    }
        //    catch
        //    {
        //    }
        //} 
        #endregion

        void c_ClientAcceptImbueItemInfo(object sender, ClientAcceptImbueItemEventArgs e)
        {
            Client c = (Client)sender;
            AcceptImbueItem i = e.Info;
            int itemCount = 1;
            BaseItem item = null;
            BaseItem imbueItem = null, imbueItem2 = null, imbueItem3 = null;  // Stone\Crystal\Jewel
            ImbueError error = ImbueError.FailedToRemake;   // Default error = fail
            ImbueStat stat = ImbueStat.CriticalHit;     // Default stat to imbue = critical
            List<ImbueStat> WhiteStats = null;      // List of possible stats to imbue
            List<ImbueStat> RedStats = null;

            try
            {
                imbueItem = c.MyCharacter.Bags[i.ImbueItem1Bag - 1].Items.Single(x => (x.ItemID == i.ImbueItem1ID));
                if (!(imbueItem is ImbueItem))      // If not using imbue item
                {
                    // Output error - not imbue item
                    return;
                }
                ImbueItem imbueitem = imbueItem as ImbueItem;
                

                item = c.MyCharacter.Bags[i.ToImbueItemBag - 1].Items.Single(x => (x.ItemID == i.ToImbueItemID && x.Slot == i.ToImbueItemSlot));
                if (!(item is Equipment))      // See if item can be imbued
                {
                    // Output error - not imbuable item
                    return;
                }
                Equipment Item = item as Equipment;

                int SuccessChance = 75;     // Success rate offset
                int increaseAbility = 0;

                if (i.ImbueItem2ID != 0)
                {
                    imbueItem2 = c.MyCharacter.Bags[i.ImbueItem2Bag - 1].Items.Single(x => (x.ItemID == i.ImbueItem2ID));
                    itemCount++;
                    if (i.ImbueItem3ID != 0)
                    {
                        imbueItem3 = c.MyCharacter.Bags[i.ImbueItem3Bag - 1].Items.Single(x => (x.ItemID == i.ImbueItem3ID));
                        itemCount++;
                    }
                }

                SuccessChance += imbueitem.ImbueChance * itemCount;
                increaseAbility += imbueitem.IncreaseValue;

                int value = (int)(Item.RequiredLevel) + increaseAbility;        // Base increase value is req level

                if (imbueItem is Black)     // Using a black imbue item
                {
                    // Increase value increases according to plus and type of rock
                    value += (int)(((value * 0.1) * Item.Plus) + ((imbueitem.ImbueChance / 5) * Item.Plus));
                    if(Item is Feet)
                        value = (int)(Math.Round((double)(value / 50))) + 1;

                    if (XiahRandom.PercentSuccess(SuccessChance - Item.Plus * 4))      // Attempt imbue
                    {
                        gameEngine.BlackImbue(Item, ref stat, imbueitem, 1);        // Imbue the item
                        if (!(Item is Feet))
                            if(Item.RebirthHole == (short)(RebirthHoleItems.BlackImbueTier1) || 
                                Item.RebirthHole == (short)(RebirthHoleItems.BlackImbueTier2) || 
                                Item.RebirthHole == (short)(RebirthHoleItems.BlackImbueTier3))
                            {
                                value += Item.RebirthHoleStat;
                            }
                        Item.Plus++;        // Increase plus by 1
                        error = ImbueError.Success;     // Return success message
                    }
                    else   // Failed to imbue
                    {
                        error = ImbueError.FailedToRemake;      // Error failed to imbue
                        stat = ImbueStat.None;      // No stat imbued
                        if (Item.Plus >= 4 && XiahRandom.PercentSuccess(Item.Plus * 2))    // Drop?
                        {
                            value = gameEngine.BlackImbue(Item, ref stat, imbueitem, -1);       // Lose value
                            Item.Plus--;        // Reduce plus by 1
                        }
                        value *= -1;
                    }
                }
                else if (imbueItem is White)
                {
                    if (Item is Cape)
                    {
                        byte[] cannotbeImbued = PacketManager.SendImbueMessage(ImbueError.ImbueImpossible, ImbueStat.None, 0, Item);
                        c.Send(cannotbeImbued);
                        return;
                    }

                    value += (int)(((value * 0.1) * Item.Slvl) + ((imbueitem.ImbueChance / 5) * Item.Slvl));
                    WhiteStats = new List<ImbueStat>();
                    WhiteStats.Add(ImbueStat.Damage);
                    WhiteStats.Add(ImbueStat.Defense);
                    WhiteStats.Add(ImbueStat.AttackRating);
                    WhiteStats.Add(ImbueStat.MaxLife);
                    WhiteStats.Add(ImbueStat.MaxMana);
                    WhiteStats.Add(ImbueStat.LifeReg);
                    WhiteStats.Add(ImbueStat.ManaReg);
                    WhiteStats.Add(ImbueStat.CriticalHit);

                    if (XiahRandom.PercentSuccess(SuccessChance - Item.Slvl * 4))
                    {
                        value = gameEngine.WhiteImbue(Item, ref stat, imbueitem);
                        Item.Slvl++;
                        error = ImbueError.Success;
                    }
                    else
                    {
                        error = ImbueError.FailedToRemake;
                        stat = ImbueStat.None;

                        if (Item.Slvl >= 4 && XiahRandom.PercentSuccess(Item.Slvl * 2))
                        {
                            Item.Slvl--;
                            if (Item is Cape)
                            {
                                WhiteStats.Remove(ImbueStat.Damage);
                                WhiteStats.Remove(ImbueStat.Defense);

                                if (Item.AttackRating == 0)
                                    WhiteStats.Remove(ImbueStat.AttackRating);
                                if (Item.IncMaxLife == 0)
                                    WhiteStats.Remove(ImbueStat.MaxLife);
                                if (Item.IncMaxMana == 0)
                                    WhiteStats.Remove(ImbueStat.MaxMana);
                                if (Item.IncLifeRegen == 0)
                                    WhiteStats.Remove(ImbueStat.LifeReg);
                                if (Item.IncManaRegen == 0)
                                    WhiteStats.Remove(ImbueStat.ManaReg);
                                if (Item.Critical == 0)
                                    WhiteStats.Remove(ImbueStat.CriticalHit);
                            }
                            if (Item is Hand)
                            {
                                WhiteStats.Remove(ImbueStat.Damage);

                                if (Item.Defence == 0)
                                    WhiteStats.Remove(ImbueStat.Defense);
                                if (Item.AttackRating == 0)
                                    WhiteStats.Remove(ImbueStat.AttackRating);
                                if (Item.IncMaxLife == 0)
                                    WhiteStats.Remove(ImbueStat.MaxLife);
                                if (Item.IncMaxMana == 0)
                                    WhiteStats.Remove(ImbueStat.MaxMana);
                                if (Item.IncLifeRegen == 0)
                                    WhiteStats.Remove(ImbueStat.LifeReg);
                                if (Item.IncManaRegen == 0)
                                    WhiteStats.Remove(ImbueStat.ManaReg);
                                if (Item.Critical == 0)
                                    WhiteStats.Remove(ImbueStat.CriticalHit);
                            }
                            if (Item is Head)
                            {
                                WhiteStats.Remove(ImbueStat.AttackRating);

                                if (Item.Damage == 0)
                                    WhiteStats.Remove(ImbueStat.Damage);
                                if (Item.Defence == 0)
                                    WhiteStats.Remove(ImbueStat.Defense);
                                if (Item.IncMaxLife == 0)
                                    WhiteStats.Remove(ImbueStat.MaxLife);
                                if (Item.IncMaxMana == 0)
                                    WhiteStats.Remove(ImbueStat.MaxMana);
                                if (Item.IncLifeRegen == 0)
                                    WhiteStats.Remove(ImbueStat.LifeReg);
                                if (Item.IncManaRegen == 0)
                                    WhiteStats.Remove(ImbueStat.ManaReg);
                                if (Item.Critical == 0)
                                    WhiteStats.Remove(ImbueStat.CriticalHit);
                            }
                            if (Item is Body)
                            {
                                WhiteStats.Remove(ImbueStat.Defense);

                                if (Item.Damage == 0)
                                    WhiteStats.Remove(ImbueStat.Damage);
                                if (Item.AttackRating == 0)
                                    WhiteStats.Remove(ImbueStat.AttackRating);
                                if (Item.IncMaxLife == 0)
                                    WhiteStats.Remove(ImbueStat.MaxLife);
                                if (Item.IncMaxMana == 0)
                                    WhiteStats.Remove(ImbueStat.MaxMana);
                                if (Item.IncLifeRegen == 0)
                                    WhiteStats.Remove(ImbueStat.LifeReg);
                                if (Item.IncManaRegen == 0)
                                    WhiteStats.Remove(ImbueStat.ManaReg);
                                if (Item.Critical == 0)
                                    WhiteStats.Remove(ImbueStat.CriticalHit);
                            }
                            if (Item is Feet)
                            {
                                WhiteStats.Remove(ImbueStat.CriticalHit);

                                if (Item.Defence == 0)
                                    WhiteStats.Remove(ImbueStat.Defense);
                                if (Item.Damage == 0)
                                    WhiteStats.Remove(ImbueStat.Damage);
                                if (Item.AttackRating == 0)
                                    WhiteStats.Remove(ImbueStat.AttackRating);
                                if (Item.IncMaxLife == 0)
                                    WhiteStats.Remove(ImbueStat.MaxLife);
                                if (Item.IncMaxMana == 0)
                                    WhiteStats.Remove(ImbueStat.MaxMana);
                                if (Item.IncLifeRegen == 0)
                                    WhiteStats.Remove(ImbueStat.LifeReg);
                                if (Item.IncManaRegen == 0)
                                    WhiteStats.Remove(ImbueStat.ManaReg);
                                if (Item.Critical == 0)
                                    WhiteStats.Remove(ImbueStat.CriticalHit);
                            }
                            if (Item is Ring)
                            {
                                WhiteStats.Remove(ImbueStat.MaxLife);

                                if (Item.Defence == 0)
                                    WhiteStats.Remove(ImbueStat.Defense);
                                if (Item.Damage == 0)
                                    WhiteStats.Remove(ImbueStat.Damage);
                                if (Item.AttackRating == 0)
                                    WhiteStats.Remove(ImbueStat.AttackRating);
                                if (Item.IncMaxLife == 0)
                                    WhiteStats.Remove(ImbueStat.MaxLife);
                                if (Item.IncMaxMana == 0)
                                    WhiteStats.Remove(ImbueStat.MaxMana);
                                if (Item.IncLifeRegen == 0)
                                    WhiteStats.Remove(ImbueStat.LifeReg);
                                if (Item.IncManaRegen == 0)
                                    WhiteStats.Remove(ImbueStat.ManaReg);
                                if (Item.Critical == 0)
                                    WhiteStats.Remove(ImbueStat.CriticalHit);
                            }
                            if (Item is Necklace)
                            {
                                WhiteStats.Remove(ImbueStat.MaxMana);

                                if (Item.Defence == 0)
                                    WhiteStats.Remove(ImbueStat.Defense);
                                if (Item.Damage == 0)
                                    WhiteStats.Remove(ImbueStat.Damage);
                                if (Item.AttackRating == 0)
                                    WhiteStats.Remove(ImbueStat.AttackRating);
                                if (Item.IncMaxLife == 0)
                                    WhiteStats.Remove(ImbueStat.MaxLife);
                                if (Item.IncMaxMana == 0)
                                    WhiteStats.Remove(ImbueStat.MaxMana);
                                if (Item.IncLifeRegen == 0)
                                    WhiteStats.Remove(ImbueStat.LifeReg);
                                if (Item.IncManaRegen == 0)
                                    WhiteStats.Remove(ImbueStat.ManaReg);
                                if (Item.Critical == 0)
                                    WhiteStats.Remove(ImbueStat.CriticalHit);
                            }

                            int randomStat = XiahRandom.Next(0, WhiteStats.Count);
                            stat = WhiteStats[randomStat];

                            switch (stat)
                            {
                                case ImbueStat.Damage:
                                    value = Item.Damage;
                                    Item.Damage = 0;
                                    break;

                                case ImbueStat.Defense:
                                    value = Item.Defence;
                                    Item.Defence = 0;
                                    break;

                                case ImbueStat.AttackRating:
                                    value = Item.AttackRating;
                                    Item.AttackRating = 0;
                                    break;

                                case ImbueStat.MaxLife:
                                    value = Item.IncMaxLife;
                                    Item.IncMaxLife = 0;
                                    break;

                                case ImbueStat.MaxMana:
                                    value = Item.IncMaxMana;
                                    Item.IncMaxMana = 0;
                                    break;

                                case ImbueStat.LifeReg:
                                    value = Item.IncLifeRegen;
                                    Item.IncLifeRegen = 0;
                                    break;

                                case ImbueStat.ManaReg:
                                    value = Item.IncManaRegen;
                                    Item.IncManaRegen = 0;
                                    break;

                                case ImbueStat.CriticalHit:
                                    value = Item.Critical;
                                    Item.Critical = 0;
                                    break;
                            }

                            #region drop less than all stats
                            //else THIS IS IF WE WANNA DROP LIKE 30 FROM WHITE FAILURE BUT FUCK THAT ITS ALL OR NOTHING MANG (H)
                            //{
                            //    switch (stat)
                            //    {
                            //        case ImbueStat.Damage:
                            //            Item.Damage -= (Item.Damage < (short)value) ? Item.Damage : (short)value;
                            //            break;

                            //        case ImbueStat.Defense:
                            //            Item.Defence -= (Item.Defence < (short)value) ? Item.Defence : (short)value;
                            //            break;

                            //        case ImbueStat.AttackRating:
                            //            Item.AttackRating -= (Item.AttackRating < (short)value) ? Item.AttackRating : (short)value;
                            //            break;

                            //        case ImbueStat.MaxLife:
                            //            Item.IncMaxLife -= (Item.IncMaxLife < (short)value) ? Item.IncMaxLife : (short)value;
                            //            break;

                            //        case ImbueStat.MaxMana:
                            //            Item.IncMaxMana -= (Item.IncMaxMana < (short)value) ? Item.IncMaxMana : (short)value;
                            //            break;

                            //        case ImbueStat.LifeReg:
                            //            Item.IncLifeRegen -= (Item.IncLifeRegen < (short)value) ? Item.IncLifeRegen : (short)value;
                            //            break;

                            //        case ImbueStat.ManaReg:
                            //            Item.IncManaRegen -= (Item.IncManaRegen < (short)value) ? Item.IncManaRegen : (short)value;
                            //            break;

                            //        case ImbueStat.CriticalHit:
                            //            value = (int)(Math.Round((double)(value / 50))) + 1;
                            //            Item.Critical -= (Item.Critical < (short)value) ? Item.Critical : (short)value;
                            //            break;
                            //    }
                            //}
                            #endregion

                            value *= -1;
                        }
                    }
                }
                if (imbueItem is Red)
                {
                    value = -imbueitem.IncreaseValue;
                    int randStat = 0;
                    BaseItem temp = itemDataManager.GetItemByReferenceID(item.ReferenceID);
                    Equipment Temp = temp as Equipment;
                    stat = ImbueStat.None;

                    RedStats = new List<ImbueStat>();
                    RedStats.Add(ImbueStat.RequiredLevel);
                    RedStats.Add(ImbueStat.RequiredEnergy);
                    RedStats.Add(ImbueStat.RequiredDexterity);
                    RedStats.Add(ImbueStat.RequiredStamina);
                    RedStats.Add(ImbueStat.RequiredStrength);

                    if (Item.RequiredLevel == 0)
                        RedStats.Remove(ImbueStat.RequiredLevel);
                    if (Item.RequiredEnergy == 0)
                        RedStats.Remove(ImbueStat.RequiredEnergy);
                    if (Item.RequiredDexterity == 0)
                        RedStats.Remove(ImbueStat.RequiredDexterity);
                    if (Item.RequiredStamina == 0)
                        RedStats.Remove(ImbueStat.RequiredStamina);
                    if (Item.RequiredStrength == 0)
                        RedStats.Remove(ImbueStat.RequiredStrength);

                    if (RedStats.Count == 0) // no stats left blabla
                        error = ImbueError.ImbueImpossible;
                    else // theres one stat to remove :) 
                    {
                        if (imbueitem.ImbueData == 0)
                        {
                            if (RedStats.Count == 1)
                                stat = RedStats[0];
                            else
                            {
                                randStat = XiahRandom.Next(0, RedStats.Count);
                                stat = RedStats[randStat];
                            }
                        }
                        else
                        {
                            switch (imbueitem.ImbueData)
                            {
                                case 2:
                                    stat = ImbueStat.RequiredLevel;
                                    break;
                                case 3:
                                    stat = ImbueStat.RequiredStrength;
                                    break;
                                case 4:
                                    stat = ImbueStat.RequiredStamina;
                                    break;
                                case 5:
                                    stat = ImbueStat.RequiredDexterity;
                                    break;
                                case 6:
                                    stat = ImbueStat.RequiredEnergy;
                                    break;
                                default:
                                    stat = ImbueStat.RequiredLevel;
                                    break;
                            }
                        }

                        if (stat == ImbueStat.RequiredLevel)
                        {

                            if (XiahRandom.PercentSuccess(SuccessChance - (Temp.RequiredLevel - Item.RequiredLevel) * 4))
                                gameEngine.RedImbue(Item, stat, ref value, true, ref error); //if succeeded, pass the success value as 1, as well as pass the error
                            else
                                gameEngine.RedImbue(Item, stat, ref value, false, ref error); //if failed, random between -1 and 0, as well as pass the error
                        }
                        else if (stat == ImbueStat.RequiredStrength)
                        {
                            if (XiahRandom.PercentSuccess(SuccessChance - (Temp.RequiredStrength - Item.RequiredStrength) * 4))

                                gameEngine.RedImbue(Item, stat, ref value, true, ref error);
                            else
                                gameEngine.RedImbue(Item, stat, ref value, false, ref error);
                        }
                        else if (stat == ImbueStat.RequiredStamina)
                        {
                            if (XiahRandom.PercentSuccess(SuccessChance - (Temp.RequiredStamina - Item.RequiredStamina) * 4))
                                gameEngine.RedImbue(Item, stat, ref value, true, ref error);
                            else
                                gameEngine.RedImbue(Item, stat, ref value, false, ref error);
                        }
                        else if (stat == ImbueStat.RequiredDexterity)
                        {
                            if (XiahRandom.PercentSuccess(SuccessChance - (Temp.RequiredDexterity - Item.RequiredDexterity) * 4))
                                gameEngine.RedImbue(Item, stat, ref value, true, ref error);
                            else
                                gameEngine.RedImbue(Item, stat, ref value, false, ref error);
                        }
                        else if (stat == ImbueStat.RequiredEnergy)
                        {
                            if (XiahRandom.PercentSuccess(SuccessChance - (Temp.RequiredEnergy - Item.RequiredEnergy) * 4))
                                gameEngine.RedImbue(Item, stat, ref value, true, ref error);
                            else
                                gameEngine.RedImbue(Item, stat, ref value, false, ref error);
                        }
                    }
                }
                if (imbueitem is RbHoleItem)
                {
                    if (Item.RebirthHole == 1)
                    {
                        if (c.MyCharacter.Money >= (imbueitem.Price * 10))
                        {
                            c.MyCharacter.Money -= (imbueitem.Price * 10);
                            Item.RebirthHole = imbueitem.ImbueChance;
                            Item.RebirthHoleStat = imbueitem.IncreaseValue;

                            if (imbueitem.ImbueChance == (short)RebirthHoleItems.Durability ||
                                imbueitem.ImbueChance == (short)RebirthHoleItems.LevelReq)
                            {
                                if (imbueitem.ImbueChance == (short)RebirthHoleItems.Durability)
                                {
                                    float curdur = (float)Item.MaxDurability;
                                    float incDur = curdur * 0.20f;
                                    Item.MaxDurability += (short)incDur;
                                    Item.Durability = Item.MaxDurability;
                                }
                                else
                                {
                                    Item.RequiredLevel -= 5;
                                }
                            }

                            c.Send(PacketManager.SendMoneyLeft(c.MyCharacter.Money));

                            stat = (ImbueStat)255;
                            value = 0;
                            error = ImbueError.Success;
                        }
                        else
                        {
                            c.Send(PacketManager.SendImbueMessage(ImbueError.NotEnoughtMoney, 0, 0, Item));
                            return;
                        }
                    }
                    else
                    {
                        c.Send(PacketManager.SendImbueMessage(ImbueError.ImbueImpossible, 0, 0, Item));
                        return;
                    }
                }

                if (imbueitem is Red && error == ImbueError.FailedToRemake && value == 0)
                {
                    stat = ImbueStat.None;
                    // just send failed to remake blabla
                }
                else if (imbueitem is Red && error == ImbueError.FailedToRemake && value < 0)
                {
                    value = -value; // so it shows increasement
                }

                c.Send(PacketManager.SendImbueMessage(error, stat, value, Item));

                c.Send(PacketManager.SendRemoveItem(i.ImbueItem1Bag, i.ImbueItem1Slot));

                itemDataManager.DeleteItem(imbueItem.ItemID);
                c.MyCharacter.Bags[i.ImbueItem1Bag - 1].RemoveItem(imbueItem);

                if (imbueItem2 != null)
                {
                    c.Send(PacketManager.SendRemoveItem(i.ImbueItem2Bag, i.ImbueItem2Slot));
                    itemDataManager.DeleteItem(imbueItem2.ItemID);
                    c.MyCharacter.Bags[i.ImbueItem2Bag - 1].RemoveItem(imbueItem2);
                    if (imbueItem3 != null)
                    {
                        c.Send(PacketManager.SendRemoveItem(i.ImbueItem3Bag, i.ImbueItem3Slot));
                        itemDataManager.DeleteItem(imbueItem3.ItemID);
                        c.MyCharacter.Bags[i.ImbueItem3Bag - 1].RemoveItem(imbueItem3);
                    }
                }

                Item.ImbueTries++;
                c.Send(PacketManager.SendImbuedItem(Item));

                itemDataManager.UpdateItem(Item);
            }
            catch
            {
            }
        }

        void c_ClientNpcTradeInfo(object sender, ClientNpcTradeEventArgs e)
        {
            Client c = (Client)sender;
            NpcTradeInfo n = e.Info;
            MapEngine mapEngine = GetMapEngine(c.MyCharacter.MapId);
            Npc npc = mapEngine.GetNpcByID(n.NpcID, n.MapID);

            byte[] SendNpcItemList = PacketManager.SendNpcItemList(npc, n.Bag);
            c.Send(SendNpcItemList);
        }

        /// <summary>
        /// SELL ITEM PRICE FOR PLAIN ITEMS IS ITEM.PRICE / 2.5
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void c_ClientBuyItemInfo(object sender, ClientBuyItemEventArgs e)
        {
            Client c = (Client)sender;
            BuyItemInfo i = e.Info;
            BaseItem buyitem = null;
            BaseItem npcitem = null;
            Npc npc = null;

            MapEngine mapEngine = GetMapEngine(c.MyCharacter.MapId);
            try
            {
                npc = mapEngine.GetNpcByID(i.NpcID, c.MyCharacter.MapId);

                npcitem = npc.FindItem(i.ReferenceID, i.Slot);
                int price = npcitem.Price * i.Amount;

                if (c.MyCharacter.Money >= price)
                {
                    BagSlot bagSlot = new BagSlot();

                    if (!c.MyCharacter.FindFreeSlotInBags(npcitem, bagSlot))
                    {
                        throw new BagIsFullException("BAG FULL");
                        //Throw exception here
                    }

                    c.MyCharacter.Money -= price;
                    c.Send(PacketManager.SendMoneyLeft(c.MyCharacter.Money));
                    c.Send(PacketManager.SendBuyItemState(BuyItemState.Success));

                    MemoryStream stream = new MemoryStream();

                    BinaryFormatter formatter = new BinaryFormatter();

                    formatter.Serialize(stream, npcitem);

                    stream.Position = 0;

                    buyitem = (BaseItem)formatter.Deserialize(stream);

                    stream.Close();

                    buyitem.Bag = bagSlot.Bag;
                    buyitem.Slot = bagSlot.Slot;
                    buyitem.OwnerID = c.MyCharacter.CharacterId;
                    buyitem.Amount = i.Amount;

                    buyitem.ItemID = itemDataManager.InsertItem(buyitem);

                    if (npc.IsEliteShop)
                    {
                        if (buyitem is Equipment)
                        {
                            // later add chance to get these items blabla
                            Equipment Item = buyitem as Equipment;
                            ImbueStat stat = ImbueStat.None;
                            ImbueItem imbueitem = new ImbueItem
                            {
                                ImbueChance = 1,
                                IncreaseValue = 1,
                            };

                            if (XiahRandom.PercentSuccess(40))
                            {
                                // Possible plus for drop
                                int plus = gameEngine.RandomChance(0, 9);
                                for (int a = 0; a < plus; a++)
                                {
                                    gameEngine.BlackImbue(Item, ref stat, imbueitem, 1);
                                    Item.Plus++;
                                }
                            }

                            if (XiahRandom.PercentSuccess(40))
                            {
                                // Possible slvl for drop
                                int slvl = gameEngine.RandomChance(0, 9);
                                for (int a = 0; a < slvl && !(Item is Cape); a++)
                                {
                                    gameEngine.WhiteImbue(Item, ref stat, imbueitem);
                                    Item.Slvl++;
                                }
                                buyitem = Item;
                            }

                            itemDataManager.UpdateItem(buyitem);
                        }
                    }

                    c.MyCharacter.Bags[buyitem.Bag - 1].AddItem(buyitem);

                    characterManager.UpdateCharacter(c.MyCharacter);
                    c.Send(PacketManager.SendAddItem(buyitem));
                }
                else
                {
                    c.Send(PacketManager.SendBuyItemState(BuyItemState.NoMoney));
                }

            }
            catch (BagIsFullException)
            {
                c.Send(PacketManager.SendBuyItemState(BuyItemState.BagFull));
            }
        }

        void c_ClientSellItemInfo(object sender, ClientSellItemEventArgs e)
        {
            Client c = (Client)sender;
            SellItemInfo i = e.Info;
            BaseItem item = null;

            try
            {
                item = c.MyCharacter.Bags[i.Bag - 1].Items.Single(x => x.ItemID == i.ItemID);

                if (item != null)
                {
                    // needs check if you have the item in bag/slot, also remove from db and so on
                    byte error = 1;

                    if (item.Bag == i.Bag && item.Slot == i.Slot)
                        error = 0;
                    else
                        error = 1;

                    byte[] RemoveItem = PacketManager.SendRemoveItem(item.Bag, item.Slot);
                    byte[] SellState = PacketManager.SendSellState(error);

                    if (error == 0)
                    {
                        c.MyCharacter.Money += (int)Math.Abs((item.Price / 2.5) * item.Amount);
                        byte[] SendMoneyLeft = PacketManager.SendMoneyLeft(c.MyCharacter.Money);
                        characterManager.UpdateCharacter(c.MyCharacter);
                        c.Send(SendMoneyLeft);
                        c.Send(RemoveItem);
                        c.MyCharacter.Bags[i.Bag - 1].RemoveItem(item);
                        itemDataManager.DeleteItem(item.ItemID);
                    }

                    c.Send(SellState);
                }
            }
            catch
            {
            }
        }

        void c_ClientUseItemInfo(object sender, ClientUseItemEventArgs e)
        {
            Client c = (Client)sender;
            UseItemInfo i = e.Info;
            BaseItem item = null;
            MapEngine mapEngine = GetMapEngine(c.MyCharacter.MapId);
            Client[] clients = GetClientsForChars(mapEngine.GetCharactersInRange(c.MyCharacter.Position, 150).ToArray());
            bool removeItem = false;

            try
            {
                item = c.MyCharacter.Bags[i.Bag - 1].Items.Single(x => x.ItemID == i.ItemID);

                if (item != null && c.MyCharacter.Alive)
                {
                    if (item is Potion)
                    {
                        Potion p = item as Potion;
                        byte effect = 0;
                        if (c.MyCharacter.CurrentHp == c.MyCharacter.MaxHp && p.HealHp > 0 || c.MyCharacter.CurrentMana == c.MyCharacter.MaxMana && p.HealMana > 0)
                            return; // so it wont let you use potions when you dont need to use them. AKA u wont lose potion for nothing

                        if (p.HealHp != 0)
                        {
                            if (c.MyCharacter.CurrentHp + p.HealHp < c.MyCharacter.MaxHp)
                                c.MyCharacter.CurrentHp += p.HealHp;
                            else
                                c.MyCharacter.CurrentHp = c.MyCharacter.MaxHp;
                            effect = 1;
                        }
                        else
                        {
                            if (c.MyCharacter.CurrentMana + p.HealMana < c.MyCharacter.MaxMana)
                                c.MyCharacter.CurrentMana += p.HealMana;
                            else
                                c.MyCharacter.CurrentMana = c.MyCharacter.MaxMana;
                            effect = 2;
                        }

                        characterManager.UpdateCharacter(c.MyCharacter);
                        byte[] SendHealMana = PacketManager.SendHealMana(c.MyCharacter.MaxHp, c.MyCharacter.CurrentHp, c.MyCharacter.MaxMana, c.MyCharacter.CurrentMana, effect);
                        c.Send(SendHealMana);

                        removeItem = true;
                    }

                    if (item is Bead)
                    {
                        Bead b = item as Bead;

                        Map tomap = null;

                        if (b.ToMapID != 0)
                        {
                            MapEngine oldMap = GetMapEngine(c.MyCharacter.MapId);
   
                            tomap = GetMapEngine(b.ToMapID).Map; // its real bead which has mapid etc
                        }
                        else
                        {
                            tomap = GetMapEngine(c.MyCharacter.MapId).Map; // its either town portal scroll or bugged so its win/win for us
                        }

                        Portal tempPort = new Portal
                        {
                            ToMapID = tomap.MapID,
                            ToX = (short)tomap.SpawnX,
                            ToY = (short)tomap.SpawnY
                        };

                        if (b.ReferenceID == 21975) // BDC base bead
                        {
                            tempPort.ToX = 820;
                            tempPort.ToY = 816;
                        }

                        byte[] SendRemoveCharacter = PacketManager.SendRemoveCharacter(c.MyCharacter, RemoveCharacterEffect.Bead);
                        SendToClients(SendRemoveCharacter, clients);

                        c.MyCharacter.Position.X = tempPort.ToX;
                        c.MyCharacter.Position.Y = tempPort.ToY;
                        c.MyCharacter.OldMapId = c.MyCharacter.MapId;
                        c.MyCharacter.MapId = tempPort.ToMapID;

                        if (c.MyCharacter.Pet != null && !c.MyCharacter.Pet.IsSealed)
                        {
                            byte[] bufferRemovePet = PacketManager.SendRemovePet(c.MyCharacter.Pet);
                            SendToClients(bufferRemovePet, clients);

                            c.MyCharacter.Pet.Position.X = c.MyCharacter.Position.X;
                            c.MyCharacter.Pet.Position.Y = c.MyCharacter.Position.Y;
                            c.MyCharacter.Pet.MapID = c.MyCharacter.MapId;
                        }
 
                        c.MyCharacter.Map = mapEngine.Map;
                        characterManager.UpdateCharacter(c.MyCharacter);

                        byte[] SendPortal = PacketManager.SendPortal(tempPort);
                        c.Send(SendPortal);

                        removeItem = true;
                    }

                    if (item is BookItem)
                    {
                        BookItem book = item as BookItem;
                        BaseSkill skill = characterManager.GetSkillByLevelAndID(book.SkillID, book.SkillLevel);


                        // Need add so it saves the skill using LearnSkill procedure etc..
                        if (book is RebirthBook)
                        {
                            if (c.MyCharacter.Rebirth > 0)
                            {
                                RebirthBook rb = book as RebirthBook;
                                removeItem = LearnSkill(c, item, removeItem, skill, rb);
                            }
                            else
                            {
                                c.Send(PacketManager.SendSkillError(SkillLearnError.TooLowLevel));
                                return;
                            }
                        }
                        if (book is FocusBook)
                        {
                            if (c.MyCharacter.Rebirth > 6)
                            {
                                FocusBook focus = book as FocusBook;
                                removeItem = LearnSkill(c, item, removeItem, skill, focus);
                            }
                            else
                            {
                                c.Send(PacketManager.SendSkillError(SkillLearnError.TooLowLevel));
                                return;
                            }
                        }
                        if (book is HardBook)
                        {
                            HardBook hard = book as HardBook;
                            removeItem = LearnSkill(c, item, removeItem, skill, hard);
                        }
                        else
                        {
                            SoftBook soft = book as SoftBook;
                            removeItem = LearnSkill(c, item, removeItem, skill, soft);

                            byte[] SendStats = PacketManager.SendStats(c.MyCharacter);
                            c.Send(SendStats); // as it needs to update like dmg, def, ar, regens etc
                        }
                    }

                    if (item is StoreTag)
                    {
                        StoreTag tag = item as StoreTag;

                        byte[] SendPlayerShop = PacketManager.SendPlayerShop(c.MyCharacter, tag.TimeLeft);
                        c.Send(SendPlayerShop);
                    }

                    if (item is PetResurrectItem)
                    {
                        PetResurrectItem res = item as PetResurrectItem;

                        List<Pet> deadPets = petManager.GetLastDiedPets(c.MyCharacter.CharacterId);

                        byte[] SendLastDiedPets = PacketManager.SendLastDiedPets(deadPets);
                        c.Send(SendLastDiedPets);
                    }

                    if (removeItem) // just so it wont remove stuff like items from inventory just right clicking :(
                    {
                        if (item.Amount > 1)
                        {
                            item.Amount--;

                            byte[] SendRecvItemsLeft = PacketManager.SendItemsLeft(i.Bag, i.Slot, item.ItemID, item.Amount);
                            c.Send(SendRecvItemsLeft);

                            itemDataManager.UpdateItem(item);
                        }
                        else
                        {
                            byte[] SendRemoveItem = PacketManager.SendRemoveItem(item.Bag, item.Slot);
                            c.Send(SendRemoveItem);

                            c.MyCharacter.Bags[item.Bag - 1].RemoveItem(item);
                            itemDataManager.DeleteItem(item.ItemID);
                        }
                    }

                }
            }
            catch
            {
            }
        }

        private bool LearnSkill(Client c, BaseItem item, bool removeItem, BaseSkill skill, BookItem book)
        {
            short test = 0;

            if (!c.MyCharacter.ContainSkill(skill.SkillID, skill.SkillLevel) && c.MyCharacter.TrainingPoint >= skill.RequiredTraining && c.MyCharacter.Level >= skill.RequiredLevel && c.MyCharacter.Strength >= skill.RequiredStrength && c.MyCharacter.Stamina >= skill.RequiredStamina && c.MyCharacter.Dexterity >= skill.RequiredDexterity && c.MyCharacter.Energy >= skill.RequiredEnergy)
            {
                if (item.RequiredClass == 0 || c.MyCharacter.Class == item.RequiredClass)
                {
                    if (book.SkillLevel == 1)
                    {
                        characterManager.LearnSkill(c.MyCharacter.CharacterId, skill.SkillID, skill.SkillLevel, ref test);
                        c.MyCharacter.TrainingPoint = test;

                        characterManager.UpdateCharacter(c.MyCharacter);

                        c.MyCharacter.SkillList.Add(skill);

                        byte[] LearnSkill = PacketManager.SendLearnSkill(0, skill.SkillID, skill.SkillLevel, skill.RequiredTraining, c.MyCharacter.TrainingPoint);
                        c.Send(LearnSkill);

                        c.Send(PacketManager.SendSkillSomething());

                        removeItem = true;
                    }
                    else
                    {
                        if (c.MyCharacter.ContainSkill(skill.SkillID, skill.SkillLevel - 1))
                        {
                            characterManager.LearnSkill(c.MyCharacter.CharacterId, skill.SkillID, skill.SkillLevel, ref test);
                            c.MyCharacter.UpdateSkill(skill);
                            c.MyCharacter.TrainingPoint = test;

                            characterManager.UpdateCharacter(c.MyCharacter);

                            byte[] LearnSkill = PacketManager.SendLearnSkill(0, skill.SkillID, skill.SkillLevel, skill.RequiredTraining, c.MyCharacter.TrainingPoint);
                            c.Send(LearnSkill);

                            c.Send(PacketManager.SendSkillSomething());

                            removeItem = true;
                        }
                        else
                        {
                            c.Send(PacketManager.SendSkillError(SkillLearnError.NeedMartialArts));
                        }
                    }
                }
                else
                {
                    c.Send(PacketManager.SendSkillError(SkillLearnError.WrongClass));
                }
            }
            else
            {
                byte error = 0;

                if (c.MyCharacter.TrainingPoint <= skill.RequiredTraining)
                    error = 9;
                if (c.MyCharacter.Level <= skill.RequiredLevel)
                    error = 5;
                if (c.MyCharacter.Strength <= skill.RequiredStrength)
                    error = 2;
                if (c.MyCharacter.Dexterity <= skill.RequiredDexterity)
                    error = 1;
                if (c.MyCharacter.Stamina <= skill.RequiredStamina)
                    error = 3;
                if (c.MyCharacter.Energy <= skill.RequiredEnergy)
                    error = 4;
                if (c.MyCharacter.Class != item.RequiredClass && item.RequiredClass != 0)
                    error = 8;
                if (c.MyCharacter.ContainSkill(skill.SkillID, skill.SkillLevel))
                    error = 6;

                c.Send(PacketManager.SendSkillError((SkillLearnError)error));
                // send not enought stats/tps 
            }
            return removeItem;
        }

        void c_ClientLearnSkillInfo(object sender, ClientLearnSkillEventArgs e)
        {
            Client c = (Client)sender;
            LearnSkillInfo i = e.Info;
            BaseSkill skill = null;
            short test = 0;

            try
            {
                skill = c.MyCharacter.FindSkill(i.SkillID);
                if (skill != null)
                {
                    BaseSkill tobelearned = null;
                    tobelearned = characterManager.GetSkillByLevelAndID(skill.SkillID, skill.SkillLevel + 1);

                    if (tobelearned.ReadOnlyBook != 1)
                    {
                        if (c.MyCharacter.TrainingPoint >= tobelearned.RequiredTraining && c.MyCharacter.Level >= tobelearned.RequiredLevel && c.MyCharacter.Strength >= tobelearned.RequiredStrength && c.MyCharacter.Stamina >= tobelearned.RequiredStamina && c.MyCharacter.Dexterity >= tobelearned.RequiredDexterity && c.MyCharacter.Energy >= tobelearned.RequiredEnergy)
                        {
                            characterManager.LearnSkill(c.MyCharacter.CharacterId, tobelearned.SkillID, tobelearned.SkillLevel, ref test);
                            c.MyCharacter.UpdateSkill(tobelearned);
                            c.MyCharacter.TrainingPoint = test;

                            characterManager.UpdateCharacter(c.MyCharacter);

                            byte[] LearnSkill = PacketManager.SendLearnSkill(0, tobelearned.SkillID, tobelearned.SkillLevel, tobelearned.RequiredTraining, c.MyCharacter.TrainingPoint);
                            c.Send(LearnSkill);

                            byte[] SendStats = PacketManager.SendStats(c.MyCharacter);
                            c.Send(SendStats);
                        }
                        else
                        {
                            SkillLearnError error = SkillLearnError.WrongClass;

                            if (c.MyCharacter.TrainingPoint <= skill.RequiredTraining)
                                error = SkillLearnError.NeedMoreTps;
                            if (c.MyCharacter.Level <= skill.RequiredLevel)
                                error = SkillLearnError.TooLowLevel;
                            if (c.MyCharacter.Strength <= skill.RequiredStrength)
                                error = SkillLearnError.NeedMoreStr;
                            if (c.MyCharacter.Dexterity <= skill.RequiredDexterity)
                                error = SkillLearnError.NeedMoreDex;
                            if (c.MyCharacter.Stamina <= skill.RequiredStamina)
                                error = SkillLearnError.NeedMoreSta;
                            if (c.MyCharacter.Energy <= skill.RequiredEnergy)
                                error = SkillLearnError.NeedMoreEne;

                            byte[] LearnSkill = PacketManager.SendLearnSkill(error, tobelearned.SkillID, tobelearned.SkillLevel, tobelearned.RequiredTraining, c.MyCharacter.TrainingPoint);
                            c.Send(LearnSkill);
                        }
                    }
                    else
                    {
                        byte[] LearnSkill = PacketManager.SendLearnSkill(SkillLearnError.AlreadyTrained, tobelearned.SkillID, tobelearned.SkillLevel, tobelearned.RequiredTraining, c.MyCharacter.TrainingPoint);
                        c.Send(LearnSkill);
                    }
                }
                else
                {
                    byte[] LearnSkill = PacketManager.SendLearnSkill(SkillLearnError.AlreadyTrained, 0, 0, 0, c.MyCharacter.TrainingPoint);
                    c.Send(LearnSkill);
                }
            }
            catch
            {
            }
        }

        void c_ClientSelectSkillInfo(object sender, ClientSelectSkillEventArgs e)
        {
            Client c = (Client)sender;
            SelectSkillInfo i = e.Info;

            byte[] SendSelectedSkill = PacketManager.SendSelectedSkill(i);
            c.Send(SendSelectedSkill);
        }

        void c_ClientUpdateQuickSlotInfo(object sender, ClientUpdateQuickSlotEventArgs e)
        {
            Client c = (Client)sender;
            UpdateQuickSlotInfo i = e.Info;
            BaseItem test = null;
            short ValueID = -1;

            if (i.ValueID > 300)
            {
                test = c.MyCharacter.GetItemFromBagById(i.ValueID);
                if (test != null && test is PotionItem)
                {
                    characterManager.UpdateQuickSlot(c.MyCharacter.CharacterId, i.Slot, test.VisualID);
                    ValueID = test.VisualID;
                }
                else
                    return;
            }
            else
            {
                characterManager.UpdateQuickSlot(c.MyCharacter.CharacterId, i.Slot, i.ValueID);
                ValueID = i.ValueID;
            }

            c.MyCharacter.QuickSlots[i.Slot - 1] = new QuickSlot(i.Slot, ValueID);

            byte[] SendUpdateQuickSlot = PacketManager.SendUpdateQuickSlot(i.Slot, ValueID);
            c.Send(SendUpdateQuickSlot);
        }


        void c_ClientSpawnStatLoadInfo(object sender, ClientStatLoadEventArgs e)
        {
            Client c = (Client)sender;
            StatRequestInfo sr = e.Info;

            //Character ch = channelManager.GetCharacterByCharacterId(sr.CharacterID);
            byte[] bufferSendStats = PacketManager.SendSpawnStats(c.MyCharacter);
            c.Send(bufferSendStats);
        }

        void c_ClientRequestStatsLoadInfo(object sender, ClientRequestStatsLoadInfo e)
        {
            Client c = (Client)sender;
            SpawnRequestInfo sr = e.Info;

            byte[] bufferSendStats = PacketManager.SendStats(c.MyCharacter);
            c.Send(bufferSendStats);

            byte[] bufferSendOtherPlayer2 = PacketManager.SendSpawnPlayerSomething2();
            c.Send(bufferSendOtherPlayer2);
        }

        void c_ClientAddStatInfo(object sender, ClientAddStatEventArgs e)
        {
            Client c = (Client)sender;
            AddStatInfo statinfo = e.Info;

            if (c.MyCharacter.StatPoint >= statinfo.Amount)
            {
                int pointsleft = -1;
                int statafter = -1;

                switch (statinfo.Stat)
                {
                    case 1: // dex
                        c.MyCharacter.StatPoint -= 1;
                        c.MyCharacter.Dexterity += 1;
                        statafter = c.MyCharacter.Dexterity;
                        break;

                    case 2: // str
                        c.MyCharacter.StatPoint -= 1;
                        c.MyCharacter.Strength += 1;
                        statafter = c.MyCharacter.Strength;
                        break;

                    case 3: // sta

                        c.MyCharacter.StatPoint -= 1;
                        c.MyCharacter.Stamina += 1;
                        statafter = c.MyCharacter.Stamina;
                        break;

                    case 4: // energy
                        c.MyCharacter.StatPoint -= 1;
                        c.MyCharacter.Energy += 1;
                        statafter = c.MyCharacter.Energy;
                        break;
                }
                pointsleft = c.MyCharacter.StatPoint;

                MapEngine mapEngine = GetMapEngine(c.MyCharacter.MapId);
                c.MyCharacter.Map = mapEngine.Map;
                characterManager.UpdateCharacter(c.MyCharacter);

                byte[] bufferSendAddStat = PacketManager.SendAddStat(0, statinfo, pointsleft, statafter);
                c.Send(bufferSendAddStat);
            }
            else
            {
                byte[] bufferSendStatError = PacketManager.SendAddStat(3, statinfo, 0, 0);
                c.Send(bufferSendStatError);
            }

            byte[] bufferSendStats = PacketManager.SendStats(c.MyCharacter);
            c.Send(bufferSendStats);
        }

        void c_ClientSpawnLoadInfo(object sender, ClientSpawnLoadEventArgs e)
        {
            Client c = (Client)sender;
            SpawnRequestInfo sr = e.Info;
            MapEngine mapEngine = GetMapEngine(c.MyCharacter.MapId);
            Client[] clients = GetClientsForChars(mapEngine.GetCharactersInRange(c.MyCharacter.Position, 150).ToArray());
            MapItem[] nearbyMapItems = mapEngine.GetNearbyMapItems(c.MyCharacter);

            c.MyCharacter.Online = true;
            c.MyCharacter.Alive = true;

            byte[] bufferSendShowMap = PacketManager.SendShowMap();
            c.Send(bufferSendShowMap);

            byte[] bufferSendSpawn10 = PacketManager.SendSpawn10(c.MyCharacter);
            // c.Send(bufferSendSpawn10);
            SendToClients(bufferSendSpawn10, clients);

            for (int i = 0; i < clients.Length; i++)
            {
                if (clients[i].MyCharacter.CharacterId != c.MyCharacter.CharacterId)
                {
                    byte[] bufferSendMeToThem = PacketManager.SendSpawnPlayers(c.MyCharacter);
                    clients[i].Send(bufferSendMeToThem);

                    byte[] SendActiveSkills = PacketManager.SendPlayerActiveSkills(clients[i].MyCharacter);
                    c.Send(SendActiveSkills);

                    byte[] bufferSendPlayers = PacketManager.SendSpawnPlayers(clients[i].MyCharacter);
                    c.Send(bufferSendPlayers);

                    if (clients[i].MyCharacter.Pet != null && !clients[i].MyCharacter.Pet.IsSealed)
                    {
                        byte[] bufferSendOtherPlayerPet = PacketManager.SendPetOtherPlayerSpawn2(clients[i].MyCharacter.Pet);
                        c.Send(bufferSendOtherPlayerPet);
                    }
                }
            }

            for (int i = 0; i < nearbyMapItems.Length; i++)
            {
                byte[] bufferSendMapItems = PacketManager.SendItemOnGround(nearbyMapItems[i]);
                c.Send(bufferSendMapItems);
            }

            // this should send everything in range, players,monsters etc
            byte[] bufferSendSpawn12 = PacketManager.SendSpawn12(clients);
            c.Send(bufferSendSpawn12);

            Npc[] nearbyNpcs = mapEngine.GetNearbyNpcs(c.MyCharacter);

            if (nearbyNpcs != null)
            {
                byte[] bufferSendNpcList = PacketManager.SendSpawnNpc(nearbyNpcs.Cast<Npc>().ToList(), c.Map.MapID);
                c.Send(bufferSendNpcList);
            }
        }

        void c_ClientSendPlayerPetInfo(object sender, ClientSendPetEventArgs e)
        {
            Client c = (Client)sender;
            PetRequestInfo sr = e.Info;
            MapEngine mapEngine = GetMapEngine(c.MyCharacter.MapId);
            Client[] clients = GetClientsForChars(mapEngine.GetCharactersInRange(c.MyCharacter.Position, 150).ToArray());

            if (c.MyCharacter.Pet != null && !c.MyCharacter.Pet.IsSealed)
            {
                c.MyCharacter.Pet.MapID = c.MyCharacter.MapId;
                c.MyCharacter.Pet.Position.X = c.MyCharacter.Position.X;
                c.MyCharacter.Pet.Position.Y = c.MyCharacter.Position.Y;

                byte[] bufferSendPlayerID = PacketManager.SendPlayerPet(c.MyCharacter);
                c.Send(bufferSendPlayerID);

                SendToClients(PacketManager.SendPetOtherPlayerHasPet(c.MyCharacter.Pet), clients);
                SendToClients(PacketManager.SendPetOtherPlayerSpawn2(c.MyCharacter.Pet), clients);
            }
        }

        void c_ClientSpawnOtherPlayerInfo(object sender, ClientSpawnOtherPlayerEventArgs e)
        {
            Client c = (Client)sender;
            SpawnRequestInfo sr = e.Info;

            try
            {
                Client OtherClient = GetClientByCharacterID(sr.CharacterID);

                byte[] bufferSendOtherPlayers = PacketManager.SendSpawnPlayers(OtherClient.MyCharacter);
                c.Send(bufferSendOtherPlayers);

                byte[] SendPlayerActiveSkills = PacketManager.SendPlayerActiveSkills(OtherClient.MyCharacter);
                c.Send(SendPlayerActiveSkills);

                byte[] bufferSendOtherPlayers2 = PacketManager.SendSpawnPlayers(c.MyCharacter);
                OtherClient.Send(bufferSendOtherPlayers2);

                if (OtherClient.MyCharacter.Pet != null && !OtherClient.MyCharacter.Pet.IsSealed)
                {
                    byte[] buffersSendHeHasPet = PacketManager.SendPetOtherPlayerHasPet(OtherClient.MyCharacter.Pet);
                    c.Send(buffersSendHeHasPet);
                }
            }
            catch
            {
            }
        }


        void c_ClientUnSealPetInfo(object sender, ClientUnSealPetInfoEventArgs e)
        {
            Client c = (Client)sender;
            UnSealPetInfo i = e.Info;
            BaseItem item = c.MyCharacter.Bags[i.Bag - 1].Items.Single(x => x.Slot == i.Slot);

            if (item != null && item is Mirror)
            {
                Mirror m = item as Mirror;
                Pet pet = c.MyCharacter.Pet;

                m.RequiredLevel = 0;
                m.PetID = 0;
                m.Damage = 0;
                m.Defence = 0;
                m.AttackRating = 0;
                m.IncMaxLife = 0;

                pet.IsSealed = false;
                pet.MapID = c.MyCharacter.MapId;
                pet.Position = c.MyCharacter.Position.NormalizedCopy();

                MapEngine engine = GetMapEngine(pet.MapID);
                Client[] clients = GetClientsForChars(engine.GetCharactersInRange(c.MyCharacter.Pet.Position, 150).ToArray());
                engine.ActiveClientEntities.Add(pet);

                petManager.UpdatePet(pet);
                itemDataManager.UpdateItem(m);

                byte[] SendUnSealPet = PacketManager.SendUnSealPet(pet);
                c.Send(SendUnSealPet);

                byte[] SendMirrorWithoutStats = PacketManager.SendImbuedItem(m);
                c.Send(SendMirrorWithoutStats);

                byte[] SendStats = PacketManager.SendStats(c.MyCharacter);
                c.Send(SendStats);

                byte[] SendIHasPet = PacketManager.SendPetOtherPlayerHasPet(pet);
                SendToClients(SendIHasPet, clients);

                SendToClients(PacketManager.SendPetOtherPlayerSpawn2(pet), clients);
            }
        }

        void c_ClientSealPetInfo(object sender, ClientSealPetInfoEventArgs e)
        {
            Client c = (Client)sender;
            SealPetInfo i = e.Info;
            BaseItem item = c.MyCharacter.Bags[i.Bag - 1].Items.Single(x => x.Slot == i.Slot);

            if (item != null && item is Mirror)
            {
                Pet pet = c.MyCharacter.Pet;

                MapEngine engine = GetMapEngine(pet.MapID);
                engine.ActiveClientEntities.Remove(pet);
                Client[] clients = GetClientsForChars(engine.GetCharactersInRange(c.MyCharacter.Position, 150).ToArray());

                byte[] SendSealPet = PacketManager.SendSealPet(pet);
                c.Send(SendSealPet);

                Mirror m = item as Mirror;

                float lifeAbsorb = (float)m.LifeAbsorb / 100;
                float lifeAmount = (float)pet.MaxHealth * lifeAbsorb;

                float damageAbsorb = (float)m.DamageAbsorb / 100;
                float damageAmount = (float)pet.Damage * damageAbsorb;

                float defenseAbsorb = (float)m.DefenseAbsorb / 100;
                float defenseAmount = (float)pet.Defense * defenseAbsorb;

                float atkratingAbsorb = (float)m.AttackRatingAbsorb / 100;
                float atkratingAmount = (float)pet.AttackRating * atkratingAbsorb;

                m.RequiredLevel = pet.Level;
                m.IncMaxLife = (short)lifeAmount;
                m.Damage = (short)damageAmount;
                m.Defence = (short)defenseAmount;
                m.AttackRating = (short)atkratingAmount;
                m.PetID = pet.PetID;
                pet.IsSealed = true;

                byte[] SendMirrorWithStats = PacketManager.SendImbuedItem(m);
                c.Send(SendMirrorWithStats);

                byte[] SendCharacterStats = PacketManager.SendStats(c.MyCharacter);
                c.Send(SendCharacterStats);

                byte[] SendRemovePet = PacketManager.SendRemovePet(pet);
                SendToClients(SendRemovePet, clients);

                petManager.UpdatePet(pet);
                itemDataManager.UpdateItem(m);
            }
        }

        void c_ClientTradePetAmountInfo(object sender, ClientTradePetAmountInfoEventArgs e)
        {
            Client c = (Client)sender;
            TradePetAmountInfo i = e.Info;
            Client target = null;
            MapEngine mapEngine = GetMapEngine(c.MyCharacter.MapId);
            Client[] clients = GetClientsForChars(mapEngine.GetCharactersInRange(c.MyCharacter.Position, 150).ToArray());

            if (i.TargetID != c.MyCharacter.CharacterId)
                target = GetClientByCharacterID(i.TargetID);
            else
                target = GetClientByCharacterID(i.OwnerID);

            switch (i.Action)
            {
                case PetTradeAction.AmountWanted:
                    // send offer to sell pet to target
                    target.Send(PacketManager.SendTradePetOffer(c.MyCharacter.Pet, i.MoneyWanted, i.TargetID));
                    break;

                case PetTradeAction.Accept:
                    if (c.MyCharacter.Money >= i.MoneyWanted)
                        target.Send(PacketManager.SendTradePetOffer(target.MyCharacter.Pet, i.MoneyWanted, i.TargetID));
                    else
                    {
                        c.Send(PacketManager.SendTradePetOfferError(9));
                        target.Send(PacketManager.SendTradePetOfferError(9));
                    }
                    break;

                case PetTradeAction.Ok:

                    target.MyCharacter.Money -= i.MoneyWanted;
                    c.MyCharacter.Money += i.MoneyWanted;

                    c.Send(PacketManager.SendMoneyLeft(c.MyCharacter.Money));
                    target.Send(PacketManager.SendMoneyLeft(target.MyCharacter.Money));
                    characterManager.UpdateCharacter(c.MyCharacter);
                    characterManager.UpdateCharacter(target.MyCharacter);

                    SendToClients(PacketManager.SendMonsterDie(c.MyCharacter.Pet), clients);

                    c.Send(PacketManager.SendTradePetOfferError(0));
                    target.Send(PacketManager.SendTradePetOfferError(0));

                    SendToClients(PacketManager.SendRemovePet(c.MyCharacter.Pet), clients);

                    Pet pet = c.MyCharacter.Pet;
                    c.MyCharacter.Pet = null;

                    pet.OwnerID = target.MyCharacter.CharacterId;
                    pet.MapID = target.MyCharacter.MapId;
                    pet.Position.X = target.MyCharacter.Position.X;
                    pet.Position.Y = target.MyCharacter.Position.Y;
                    target.MyCharacter.Pet = pet;
                    petManager.UpdatePet(target.MyCharacter.Pet);

                    SendToClients(PacketManager.SendTradedPet(pet), clients);

                    break;

                case PetTradeAction.Cancel:
                    c.Send(PacketManager.SendTradePetOfferError(9));
                    target.Send(PacketManager.SendTradePetOfferError(9));
                    break;
            }
        }

        void c_ClientTradePetInfo(object sender, ClientTradePetInfoEventArgs e)
        {
            Client c = (Client)sender;
            TradePetInfo i = e.Info;
            Client target = GetClientByCharacterID(i.TargetID);

            if (target.MyCharacter.Pet != null)
            {
                // send error
            }
            else
            {
                byte[] SendTradePetError = PacketManager.SendTradePetError();
                c.Send(SendTradePetError);
                // send trade to him 
                // send the successfull trade packet to trader
            }

        }

        void c_ClientKillPetInfo(object sender, ClientKillPetInfoEventArgs e)
        {
            Client c = (Client)sender;
            KillPet i = e.Info;
            MapEngine mapEngine = GetMapEngine(c.MyCharacter.MapId);
            Client[] clients = GetClientsForChars(mapEngine.GetCharactersInRange(c.MyCharacter.Position, 150).ToArray());

            if (c.MyCharacter.tempPet != null && i.Action == 3)
            {
                BaseSkill skill = new BaseSkill();
                if (c.MyCharacter.RemoveActiveSkill((int)HardSkills.Shadow_Strike))
                {
                    skill.SkillID = (int)HardSkills.Shadow_Strike;
                    skill.SkillLevel = 1;
                    c.Send(PacketManager.SendEndSkill(c.MyCharacter, skill));
                }
                if (c.MyCharacter.RemoveActiveSkill((int)AbsorbSkills.Shadow_Focus))
                {
                    skill.SkillID = (int)AbsorbSkills.Shadow_Focus;
                    skill.SkillLevel = 1;
                    c.Send(PacketManager.SendEndSkill(c.MyCharacter, skill));
                }
                if (c.MyCharacter.RemoveActiveSkill((int)RebirthSkills.Duplicate_Attack))
                {
                    skill.SkillID = (int)RebirthSkills.Duplicate_Attack;
                    skill.SkillLevel = 1;
                    c.Send(PacketManager.SendEndSkill(c.MyCharacter, skill));
                }
                if (c.MyCharacter.RemoveActiveSkill((int)HardSkills.Summon_Monster))
                {
                    skill.SkillID = (int)HardSkills.Summon_Monster;
                    skill.SkillLevel = 1;
                    c.Send(PacketManager.SendEndSkill(c.MyCharacter, skill));
                }


                byte[] RemovePet = PacketManager.SendMonsterDie(c.MyCharacter.tempPet);
                SendToClients(RemovePet, clients);
                c.MyCharacter.tempPet = null;
            }
            if (c.MyCharacter.Pet != null && i.PetID == c.MyCharacter.Pet.PetID && i.Action == 3)
            {
                byte[] RemovePet = PacketManager.SendMonsterDie(c.MyCharacter.Pet);
                SendToClients(RemovePet, clients);

                c.MyCharacter.Pet.Alive = false;
                petManager.UpdatePet(c.MyCharacter.Pet);
                c.MyCharacter.Pet = null;
            }
        }

        void c_ClientBaitPetInfo(object sender, ClientBaitPetInfoEventArgs e)
        {
            try
            {
                Client c = (Client)sender;
                BaitPetInfo i = e.Info;
                MapEngine mapEngine = GetMapEngine(c.MyCharacter.MapId);
                Monster m = mapEngine.GetMonsterByID(i.MonsterID, c.MyCharacter.MapId);
                BaseItem item = c.MyCharacter.Bags[i.Bag - 1].Items.Where(x => x.ItemID == i.ItemID).Single();
                TameItem it = null;
                if (item is TameItem)
                    it = item as TameItem;

                // so cant tame two pets at time, OR CAN YOU?!
                if (c.MyCharacter.Pet == null && m != null && m.IsTameable && m.LeaderID == 0)
                {
                    int tameChance = 20; // max 100% with snake tail..
                    tameChance *= it.TameChance;

                    if (XiahRandom.PercentSuccess(tameChance))
                    {
                        c.MyCharacter.Pet = petManager.TamePet(m, c.MyCharacter);

                        byte[] SendBaitTaken = PacketManager.SendPetBaitTaken(m, c.MyCharacter.Pet);
                        c.Send(SendBaitTaken);

                        byte[] SendPetTamed = PacketManager.SendPetTamedMessage(c.MyCharacter.Pet, PetTamedError.None);
                        c.Send(SendPetTamed);
                    }
                    else
                    {
                        byte[] SendPetTamed = PacketManager.SendPetTamedMessage(c.MyCharacter.Pet, PetTamedError.FailedToTame);
                        c.Send(SendPetTamed);
                    }
                }
                else
                {
                    byte[] SendPetTamed = PacketManager.SendPetTamedMessage(c.MyCharacter.Pet, PetTamedError.FailedToTame);
                    c.Send(SendPetTamed);
                }

                byte[] SendRemoveItem = PacketManager.SendRemoveItem(item.Bag, item.Slot);
                c.Send(SendRemoveItem);

                c.MyCharacter.Bags[item.Bag - 1].RemoveItem(item);
                itemDataManager.DeleteItem(item.ItemID);
            }
            catch
            {
                // send no item are
            }
        }


        void c_ClientRequestPetStatsInfo(object sender, ClientRequestPetStatsInfoEventArgs e)
        {
            Client c = (Client)sender;
            RequestPetStats i = e.Info;

            byte[] SendPetStats = PacketManager.SendPetStats(c.MyCharacter.Pet);
            c.Send(SendPetStats);
        }

        void c_ClientRequestSpawnOtherPetInfo(object sender, ClientRequestSpawnOtherPetInfoEventArgs e)
        {
            Client c = (Client)sender;
            RequestSpawnOtherPet i = e.Info;
            Pet pet = petManager.GetPetByID(i.PetID);
            pet.Position = new Position();

            byte[] SendOtherPlayerPet = PacketManager.SendPetOtherPlayerSpawn2(pet);
            c.Send(SendOtherPlayerPet);
        }


        void c_ClientSendPetToMeInfo(object sender, ClientSendPetToMeInfoEventArgs e)
        {
            Client c = (Client)sender;
            SendPetToMeInfo i = e.Info;
            MapEngine mapEngine = GetMapEngine(c.MyCharacter.MapId);
            Client[] clients = GetClientsForChars(mapEngine.GetCharactersInRange(c.MyCharacter.Position, 150).ToArray());

            if (c.MyCharacter.Pet != null && c.MyCharacter.Pet.PetID == i.PetID && !c.MyCharacter.Pet.IsSealed)
            {
                c.MyCharacter.Pet.Position.X = c.MyCharacter.Position.X;
                c.MyCharacter.Pet.Position.Y = c.MyCharacter.Pet.Position.Y;
                c.MyCharacter.Pet.MapID = c.MyCharacter.MapId;
                petManager.UpdatePet(c.MyCharacter.Pet);

                byte[] SendPetToMe = PacketManager.SendPetOtherPlayerSpawn2(c.MyCharacter.Pet);
                SendToClients(SendPetToMe, clients);
            }
        }


        void c_ClientRenamePetInfo(object sender, ClientRenamePetInfoEventArgs e)
        {
            Client c = (Client)sender;
            RenamePetInfo i = e.Info;

            if (c.MyCharacter.Pet != null && c.MyCharacter.Pet.PetID == i.PetID && !c.MyCharacter.Pet.IsSealed)
            {
                c.MyCharacter.Pet.Name = i.NewName;
                petManager.UpdatePet(c.MyCharacter.Pet);

                byte[] SendNewName = PacketManager.SendPetRenamed(c.MyCharacter.Pet);
                c.Send(SendNewName);
            }
        }

        void c_ClientResurrectPetInfo(object sender, ClientResurrectPetInfoEventArgs e)
        {
            try
            {
                Client c = (Client)sender;
                ResurrectPetInfo i = e.Info;
                Pet pet = petManager.GetPetByID(i.PetID);
                BaseItem item = c.MyCharacter.Bags[i.Bag - 1].Items.Single(x => x.Slot == i.Slot);

                if (c.MyCharacter.Pet == null)
                {
                    pet.Alive = true;
                    pet.MapID = c.MyCharacter.MapId;
                    pet.Position = new Position();
                    pet.Position.X = c.MyCharacter.Position.X;
                    pet.Position.Y = c.MyCharacter.Position.Y;
                    pet.CurHealth = pet.MaxHealth;

                    MapEngine engine = GetMapEngine(pet.MapID);
                    engine.ActiveClientEntities.Add(pet);

                    c.MyCharacter.Pet = pet;
                    petManager.UpdatePet(pet);

                    byte[] SendResurrectedPet = PacketManager.SendResurrectPet(pet);
                    c.Send(SendResurrectedPet);

                    byte[] SendRemoveItem = PacketManager.SendRemoveItem(item.Bag, item.Slot);
                    c.Send(SendRemoveItem);

                    c.MyCharacter.Bags[item.Bag - 1].RemoveItem(item);
                    itemDataManager.DeleteItem(item.ItemID);


                    //  byte[] SendResurrectPet2 = PacketManager.SendResurrectPet(pet);
                    //  c.Send(SendResurrectPet2); some odd reason if send both the pet wont be yours..
                }
            }
            catch
            {
                // send no item are
            }
        }


        void c_ClientPetMovementInfo(object sender, ClientPetMovementInfoEventArgs e)
        {
            Client c = (Client)sender;
            MovementInfo mi = e.Info;
            Pet pet = null;
            MapEngine mapEngine = GetMapEngine(c.MyCharacter.MapId);
            Client[] clients = GetClientsForChars(mapEngine.GetCharactersInRange(c.MyCharacter.Position, 150).ToArray());

            try
            {
                if (c.MyCharacter.tempPet != null && mi.CharacterID == c.MyCharacter.tempPet.PetID)
                    pet = c.MyCharacter.tempPet;
                if (c.MyCharacter.Pet != null && mi.CharacterID == c.MyCharacter.Pet.PetID && !c.MyCharacter.Pet.IsSealed)
                    pet = c.MyCharacter.Pet;

                pet.Position.X = mi.FromX;
                pet.Position.Y = mi.FromY;
                pet.MapID = c.MyCharacter.MapId;

                switch (mi.PacketID)
                {
                    case 1:
                        byte[] SendStartMoving = PacketManager.SendMonsterStartMoving(pet, mi);
                        SendToClients(SendStartMoving, clients);
                        break;

                    case 2:
                        byte[] SendKeepMoving = PacketManager.SendMonsterKeepMoving(pet, mi);
                        SendToClients(SendKeepMoving, clients);
                        break;

                    case 3:
                        byte[] SendStopMoving = PacketManager.SendMonsterStopMoving(pet, mi);
                        SendToClients(SendStopMoving, clients);
                        break;
                }
            }
            catch
            {
            }
        }

        void c_ClientMovementInfo(object sender, ClientMovementInfoEventArgs e)
        {
            Client c = (Client)sender;
            MovementInfo mi = e.Info;

            MapEngine mapEngine = GetMapEngine(c.MyCharacter.MapId);
            Client[] clients = GetClientsForChars(mapEngine.GetCharactersInRange(c.MyCharacter.Position, 150).ToArray());

            try
            {
                foreach (Client client in clients)
                {
                    if (client != c && client.MyCharacter.MapId == c.MyCharacter.MapId)
                    {
                        client.Send(PacketManager.SendSpawnPlayers(c.MyCharacter));
                        c.Send(PacketManager.SendSpawnPlayers(client.MyCharacter));
                        c.Send(PacketManager.SendPlayerActiveSkills(client.MyCharacter));
                        if (client.MyCharacter.Pet != null && !client.MyCharacter.Pet.IsSealed)
                        {
                            c.Send(PacketManager.SendPetOtherPlayerSpawn2(client.MyCharacter.Pet));
                        }
                    }
                }


                c.MyCharacter.Position.X = mi.FromX;
                c.MyCharacter.Position.Y = mi.FromY;
                //GuildTown t = new GuildTown
                //{
                //    ID = 1,
                //    Grade = 1,
                //    CurDura = 1,
                //    MaxDura = 1,
                //    MapID = 7,
                //    Name = "Test MunpaTown",
                //    OwnerID = 6,
                //    Position = new Position(400, 1600, 0)
                //};
                //c.Send(PacketManager.SendGuildTown(t));

                // SAY NO TO WALLHACKERS IN OUR SERVER!!, though this increases ram cost alot :/
                if (!MapEngine.IsWalkableTile(c.MyCharacter))
                {
                    SendChatToAll(string.Format("{0} has been kicked for wallhacking!!", c.MyCharacter.Name));
                    c.Dispose();
                    return;
                }

                Npc[] nearbyNpcs = mapEngine.GetNearbyNpcs(c.MyCharacter);

                if (nearbyNpcs != null)
                {
                    byte[] bufferSendNpcList = PacketManager.SendSpawnNpc(nearbyNpcs.Cast<Npc>().ToList(), c.Map.MapID);
                    c.Send(bufferSendNpcList);
                }

                switch (mi.PacketID)
                {
                    case 1:
                        byte[] SendStartMoving = PacketManager.SendStartMoving(mi, c.MyCharacter);
                        SendToClients(SendStartMoving, clients);
                        break;

                    case 2:
                        byte[] SendKeepMoving = PacketManager.SendKeepMoving(mi, c.MyCharacter);
                        SendToClients(SendKeepMoving, clients);
                        break;

                    case 3:
                        byte[] SendStopMoving = PacketManager.SendStopMoving(mi, c.MyCharacter);
                        SendToClients(SendStopMoving, clients);
                        break;
                }
            }
            catch
            {
            }
        }

        void c_ClientWalkToPortalInfo(object sender, ClientWalkToPortalEventArgs e)
        {
            Client c = (Client)sender;
            int MapID = e.MapID;
            Map m = GetMapEngine(MapID).Map;
            MapEngine mapEngine = GetMapEngine(c.MyCharacter.MapId);
            Client[] clients = GetClientsForChars(mapEngine.GetCharactersInRange(c.MyCharacter.Position, 150).ToArray());

            Portal p = GetNearbyPortal(c);

            if (p != null)
            {
                c.MyCharacter.Position.X = p.ToX;
                c.MyCharacter.Position.Y = p.ToY;
                c.MyCharacter.OldMapId = c.MyCharacter.MapId;
                c.MyCharacter.MapId = p.ToMapID;
                c.MyCharacter.Map = m;

                characterManager.UpdateCharacter(c.MyCharacter);

                byte[] bufferRemoveMe = PacketManager.SendRemoveCharacter(c.MyCharacter, RemoveCharacterEffect.None);
                SendToClients(bufferRemoveMe, clients);

                if (c.MyCharacter.Pet != null && !c.MyCharacter.Pet.IsSealed)
                {
                    byte[] bufferRemovePet = PacketManager.SendRemovePet(c.MyCharacter.Pet);
                    SendToClients(bufferRemovePet, clients);


                    c.MyCharacter.Pet.Position.X = c.MyCharacter.Position.X;
                    c.MyCharacter.Pet.Position.Y = c.MyCharacter.Position.Y;
                    c.MyCharacter.Pet.MapID = c.MyCharacter.MapId;
                }

                byte[] bufferSendPortal = PacketManager.SendPortal(p);
                c.Send(bufferSendPortal);
            }
            else if (p == null && !c.MyCharacter.Alive)
            {
                Portal dead = new Portal()
                {
                    ToMapID = c.MyCharacter.MapId,
                    ToX = (short)c.MyCharacter.Map.SpawnX,
                    ToY = (short)c.MyCharacter.Map.SpawnY
                };

                c.MyCharacter.Position.X = (short)c.MyCharacter.Map.SpawnX;
                c.MyCharacter.Position.Y = (short)c.MyCharacter.Map.SpawnY;
                c.MyCharacter.Map = m;

           
                characterManager.UpdateCharacter(c.MyCharacter);

                byte[] bufferRemoveMe = PacketManager.SendRemoveCharacter(c.MyCharacter, RemoveCharacterEffect.None);
                SendToClients(bufferRemoveMe, clients);

                if (c.MyCharacter.Pet != null && !c.MyCharacter.Pet.IsSealed)
                {
                    byte[] bufferRemovePet = PacketManager.SendRemovePet(c.MyCharacter.Pet);
                    SendToClients(bufferRemovePet, clients);

                    c.MyCharacter.Pet.Position.X = c.MyCharacter.Position.X;
                    c.MyCharacter.Pet.Position.Y = c.MyCharacter.Position.Y;
                    c.MyCharacter.Pet.MapID = c.MyCharacter.MapId;
                }

                byte[] bufferSendPortal = PacketManager.SendPortal(dead);
                c.Send(bufferSendPortal);
            }
        }


        void c_ClientRequestFriendListInfo(object sender, ClientRequestFriendListEventArgs e)
        {
            Client c = (Client)sender;

            UpdateFriendsStatus(c);

            byte[] SendFriendList = PacketManager.SendFriendList(c.MyCharacter.FriendList);
            c.Send(SendFriendList);
        }


        void c_ClientAddFriendInfo(object sender, ClientAddFriendEventArgs e)
        {
            Client c = (Client)sender;
            FriendAddInfo i = e.Info;
            Client p = null;

            if (c.MyCharacter.CharacterId != i.TargetID)
                p = GetClientByCharacterID(i.TargetID);
            else
                p = GetClientByCharacterID(i.AskerID);

            if (i.Type == FriendAddTypes.TeacherStudent)
                if (c.MyCharacter.Class != p.MyCharacter.Class)
                {
                    i.Answer = FriendAddAnswers.CannotTeacherStudentDifClass;
                    c.Send(PacketManager.SendAddFriend(i));
                    return;
                }

            if (i.Answer == FriendAddAnswers.Request)
                p.Send(PacketManager.SendAddFriend(i));

            if (i.Answer == FriendAddAnswers.Cancel)
                p.Send(PacketManager.SendAddFriend(i)); // only send to asker stuff like has refused to add you as lover/friend/studen..

            if (i.Answer == FriendAddAnswers.Accept)
            {
                i.Answer = FriendAddAnswers.Accepted;
                p.Send(PacketManager.SendAddFriend(i));

                if (characterManager.AddRelation(c.MyCharacter.CharacterId, i.Type, p.MyCharacter.CharacterId, p.MyCharacter.Name) && characterManager.AddRelation(p.MyCharacter.CharacterId, i.Type, c.MyCharacter.CharacterId, c.MyCharacter.Name))
                {
                    Friend c_add = new Friend()
                    {
                        FriendID = p.MyCharacter.CharacterId,
                        FriendName = p.MyCharacter.Name,
                        FriendOnline = p.MyCharacter.Online,
                        RelationType = i.Type
                    };

                    Friend p_add = new Friend()
                    {
                        FriendID = c.MyCharacter.CharacterId,
                        FriendName = c.MyCharacter.Name,
                        FriendOnline = c.MyCharacter.Online,
                        RelationType = i.Type
                    };

                    c.MyCharacter.FriendList.Add(c_add);
                    p.MyCharacter.FriendList.Add(p_add);
                    c.Send(PacketManager.SendAddFriendStatus(i.Type, p.MyCharacter));
                    p.Send(PacketManager.SendAddFriendStatus(i.Type, c.MyCharacter));
                }
                else
                {
                    // send error etc
                }
            }
        }

        void c_ClientChatMessageInfo(object sender, ClientChatMessageEventArgs e)
        {
            Client c = (Client)sender;
            ChatMessageInfo cmi = e.Info;

            MapEngine mapEngine = GetMapEngine(c.MyCharacter.MapId);
            Client[] Clients = GetClientsForChars(mapEngine.GetCharactersInRange(c.MyCharacter.Position, 150).ToArray());
            Client target = null;

            if (cmi.Type == ChatType.Whisper)
            {
                if (cmi.TargetName != "")
                    target = GetClientByName(cmi.TargetName);
                else
                    target = GetClientByCharacterID(cmi.TargetID);

                if (target != null)
                {
                    cmi.TargetID = target.MyCharacter.CharacterId;
                    byte[] bufferSendMessage = PacketManager.SendChatMessage(c.MyCharacter, cmi);
                    target.Send(bufferSendMessage);
                }
                else
                {
                    cmi.TargetID = 0;
                }

                byte[] bufferSendMessage2 = PacketManager.SendChatMessage(c.MyCharacter, cmi);
                c.Send(bufferSendMessage2);
            }
            if (cmi.Type == ChatType.Party)
            {
                if (c.MyCharacter.Party != null)
                {
                    byte[] bufferSendPartyMessage = PacketManager.SendChatMessage(c.MyCharacter, cmi);
                    foreach (Character ch in c.MyCharacter.Party.Members)
                    {
                        Client member = GetClientByCharacterID(ch.CharacterId);
                        if (member != null)
                        {
                            member.Send(bufferSendPartyMessage);
                        }
                    }
                }
            }
            if (cmi.Type == ChatType.General)
            {
                bool send = true;

                // COMMANDS FOR EVERYONE
                if (cmi.Message.StartsWith("."))
                {
                    string command = cmi.Message.Substring(1).ToLower();
                    string[] commands = command.Split(' ');

                    switch (commands[0])
                    {
                        case "add":
                            #region Add stats command
                            send = false;
                            if (commands.Length < 3)
                            {
                                SendCommandHelp("Usage: .add <stat> <amount>", c);
                                SendCommandHelp("Example: .add str 50", c);
                            }
                            else
                            {
                                try
                                {
                                    string stat = commands[1];
                                    short amount = Convert.ToInt16(commands[2]);
                                    if (amount < 0)
                                    {
                                        SendCommandHelp("Cannot input negative values", c);
                                        return;
                                    }
                                    if (amount > c.MyCharacter.StatPoint)
                                    {
                                        SendCommandHelp("Not enought stat points", c);
                                        return;
                                    }

                                    switch (stat)
                                    {
                                        case "str":
                                            c.MyCharacter.Strength += amount;
                                            c.MyCharacter.StatPoint -= amount;
                                            break;

                                        case "dex":
                                            c.MyCharacter.Dexterity += amount;
                                            c.MyCharacter.StatPoint -= amount;
                                            break;

                                        case "sta":
                                            c.MyCharacter.Stamina += amount;
                                            c.MyCharacter.StatPoint -= amount;
                                            break;

                                        case "ene":
                                            c.MyCharacter.Energy += amount;
                                            c.MyCharacter.StatPoint -= amount;
                                            break;

                                        default:
                                            SendCommandHelp("Stats available: str, dex, sta, ene", c);
                                            break;
                                    }

                                    c.Send(PacketManager.SendStats(c.MyCharacter));
                                    characterManager.UpdateCharacter(c.MyCharacter);
                                }
                                catch
                                {
                                }
                            }
                            #endregion
                            break;
                    }
                }

                // GM COMMANDS
                if (cmi.Message.StartsWith("!") && accountManager.IsGM(c.MyCharacter.AccountId))
                {
                    string command = cmi.Message.Substring(1).ToLower();
                    string[] commands = command.Split(' ');

                    switch (commands[0])
                    {
                        case "help":
                            #region Help Command
                            send = false;
                            SendCommandHelp("Commands:", c);
                            SendCommandHelp("! - sends text to everyone in game after !", c);
                            SendCommandHelp("!help - displays all commands", c);
                            SendCommandHelp("!levelup - gives enought exp to level up", c);
                            SendCommandHelp("!sp <value> - gives the amount of statpoints you enter", c);
                            SendCommandHelp("!tp <value> - gives the amount of trainingpoints you enter", c);
                            SendCommandHelp("!kick <player> - kicks the player", c);
                            SendCommandHelp("!goto <player> - warps next to the player", c);
                            #endregion
                            break;

                        case "levelup":
                            #region Levelup Command
                            send = false;
                            c.MyCharacter.CurrentExp += c.MyCharacter.ExpToLevel;
                            c.Send(PacketManager.SendStats(c.MyCharacter));
                            c.Send(PacketManager.SendExperience(c.MyCharacter, (int)c.MyCharacter.ExpToLevel, 0, false, false));
                            characterManager.UpdateCharacter(c.MyCharacter);
                            SendCommandHelp("Kill monster to level up!", c);
                            #endregion
                            break;

                        case "sp":
                            #region Statpoint Command
                            send = false;
                            if (commands.Length < 2)
                            {
                                SendCommandHelp("Usage: !sp <value>", c);
                            }
                            else
                            {
                                try
                                {
                                    short amount = Convert.ToInt16(commands[1]);
                                    if (c.MyCharacter.StatPoint + amount < 0)
                                        c.MyCharacter.StatPoint = 0;
                                    else
                                        c.MyCharacter.StatPoint += (short)amount;

                                    characterManager.UpdateCharacter(c.MyCharacter);
                                    c.Send(PacketManager.SendStats(c.MyCharacter));
                                }
                                catch
                                {
                                    SendCommandHelp("Too large input, max amount of statpoints is 32 500", c);
                                }
                            }
                            #endregion
                            break;

                        case "tp":
                            #region Tp Command
                            send = false;
                            if (commands.Length < 2)
                            {
                                SendCommandHelp("Usage: !tp <value>", c);
                            }
                            else
                            {
                                try
                                {
                                    short amount = Convert.ToInt16(commands[1]);
                                    if (c.MyCharacter.TrainingPoint + amount < 0)
                                        c.MyCharacter.TrainingPoint = 0;
                                    else
                                        c.MyCharacter.TrainingPoint += (short)amount;

                                    characterManager.UpdateCharacter(c.MyCharacter);
                                    c.Send(PacketManager.SendStats(c.MyCharacter));
                                }
                                catch
                                {
                                    SendCommandHelp("Too large input, max amount of tps is 32 500", c);
                                }
                            }
                            #endregion
                            break;

                        case "kick":
                            #region Kick Player Command
                            send = false;
                            if (commands.Length < 2)
                            {
                                SendCommandHelp("Usage: !kick <player>", c);
                            }
                            else
                            {
                                try
                                {
                                    Client tobeKicked = GetClientByName(commands[1]);
                                    if (tobeKicked == null)
                                        SendCommandHelp(string.Format("Player {0} does not exist", commands[1]), c);
                                    else
                                    {
                                        KickPlayer(tobeKicked.MyCharacter.Name);
                                        SendCommandHelp(string.Format("Player {0} has been kicked", commands[1]), c);
                                    }
                                }
                                catch
                                {
                                }
                            }
                            #endregion
                            break;

                        case "goto":
                            #region Goto Command
                            send = false;
                            if (commands.Length < 2)
                            {
                                SendCommandHelp("Usage: !goto <player>", c);
                            }
                            else
                            {
                                try
                                {
                                    Map tomap = null;
                                    Client player = GetClientByName(commands[1]);
                                    if (player != null)
                                    {
                                        if (player.MyCharacter.MapId != 0)
                                            tomap = GetMapEngine(player.MyCharacter.MapId).Map; // its real bead which has mapid etc
                                        else
                                            tomap = GetMapEngine(c.MyCharacter.MapId).Map; // its either town portal scroll or bugged so its win/win for us

                                        Portal tempPort = new Portal
                                        {
                                            ToMapID = tomap.MapID,
                                            ToX = (short)player.MyCharacter.Position.X,
                                            ToY = (short)player.MyCharacter.Position.Y
                                        };

                                        byte[] SendRemoveCharacter = PacketManager.SendRemoveCharacter(c.MyCharacter, RemoveCharacterEffect.Bead);
                                        SendToClients(SendRemoveCharacter, Clients);

                                        c.MyCharacter.Position.X = (short)tempPort.ToX;
                                        c.MyCharacter.Position.Y = (short)tempPort.ToY;
                                        c.MyCharacter.OldMapId = c.MyCharacter.MapId;
                                        c.MyCharacter.MapId = tomap.MapID;

                                        c.MyCharacter.Map = mapEngine.Map;
                                        characterManager.UpdateCharacter(c.MyCharacter);

                                        byte[] SendPortal = PacketManager.SendPortal(tempPort);
                                        c.Send(SendPortal);
                                    }
                                    else
                                        SendCommandHelp(string.Format("Player {0} is not online", commands[1]), c);
                                }
                                catch
                                {
                                }
                            }
                            #endregion
                            break;

                        case "money":
                            #region Money Command
                            send = false;
                            if (commands.Length < 2)
                            {
                                SendCommandHelp("Usage: !money <value>", c);
                            }
                            else
                            {
                                try
                                {
                                    int amount = Convert.ToInt32(commands[1]);
                                    if (c.MyCharacter.Money + amount < 0)
                                        c.MyCharacter.Money = 0;
                                    else
                                        c.MyCharacter.Money += amount;

                                    characterManager.UpdateCharacter(c.MyCharacter);
                                    c.Send(PacketManager.SendMoneyLeft(c.MyCharacter.Money));
                                }
                                catch
                                {
                                    SendCommandHelp("Too large input, max amount of money is 2,000,000,000", c);
                                }
                            }
                            #endregion
                            break;

                        case "random":
                            send = false;
                            SendCommandHelp(string.Format("Random test {0} {1}", XiahRandom.Next(0, 100), XiahRandom.Next(500, 900)), c);
                            break;

                        default:
                            cmi.Type = ChatType.Notice;
                            cmi.Message = string.Format("{0}: {1}", c.MyCharacter.Name, cmi.Message.Substring(1)); break;
                    }
                }

                if (send)
                {
                    byte[] bufferSendMessage = PacketManager.SendChatMessage(c.MyCharacter, cmi);
                    SendToClients(bufferSendMessage, clients.ToArray());
                }
            }
        }

        void c_ClientCommandInfo(object sender, ClientCommandInfoEventArgs e)
        {
            Client c = (Client)sender;
            CommandInfo ci = e.Info;
            MapEngine mapEngine = GetMapEngine(c.MyCharacter.MapId);
            Client[] clients = GetClientsForChars(mapEngine.GetCharactersInRange(c.MyCharacter.Position, 150).ToArray());

            byte[] bufferSendCommand = PacketManager.SendCommand(c.MyCharacter, ci);
            SendToClients(bufferSendCommand, clients);
        }

        void c_ClientMonsterSpawnRequestInfo(object sender, ClientMonsterSpawnRequestEventArgs e)
        {
            Client c = (Client)sender;
            MonsterSpawnRequestInfo i = e.Info;

            if (c.MyCharacter != null && c.MyCharacter.Online)
            {
                MapEngine mapEngine = GetMapEngine(i.MapId);
                Monster m = mapEngine.GetMonsterByID(i.MonsterId, i.MapId);
                if (m != null)
                {
                    c.Send(PacketManager.SendSpawnMonster(m));
                    if (m.IsMoving)
                    {
                        MonsterMoveInfo info = new MonsterMoveInfo
                       {
                           CurrentPosition = m.Position,
                           Destination = m.Destination,
                           Rotation = m.Direction,
                           Status = MonsterMoveStatus.StartMoving
                       };

                        c.Send(PacketManager.SendMonsterStartMoving(m, info));
                    }
                }
            }
        }


        void c_ClientRequestDeath(object sender, ClientRequestDeath e)
        {
            Client c = (Client)sender;

            Portal p = new Portal
            {
                ToX = (short)c.MyCharacter.Map.SpawnX,
                ToY = (short)c.MyCharacter.Map.SpawnY,
                ToMapID = c.MyCharacter.MapId
            };

            if (c.MyCharacter.Position.X != p.ToX && c.MyCharacter.Position.Y != p.ToY)
            {
                c.MyCharacter.Position.X = p.ToX;
                c.MyCharacter.Position.Y = p.ToY;

                byte[] SendPortal = PacketManager.SendPortal(p);
                c.Send(SendPortal);

                byte[] SendPlayerDie1 = PacketManager.SendPlayerDie1(c.MyCharacter);
                c.Send(SendPlayerDie1);
            }
        }


        void c_ClientAttackInfo(object sender, ClientAttackEventArgs e)
        {
            Client c = (Client)sender;
            Client OtherPlayer = null;
            AttackInfo i = e.Info;
            BaseEntity Attacker = null;
            BaseEntity Target = null;
            MapEngine mapEngine = GetMapEngine(c.MyCharacter.MapId);
            Client[] clients = GetClientsForChars(mapEngine.GetCharactersInRange(c.MyCharacter.Position, 150).ToArray());

            if (i.AttackerID == c.MyCharacter.CharacterId)
                Attacker = c.MyCharacter;
            if (i.TargetID == c.MyCharacter.CharacterId)
                Target = c.MyCharacter;
            if (c.MyCharacter.tempPet != null && i.AttackerID == c.MyCharacter.tempPet.PetID)
                Attacker = c.MyCharacter.tempPet;
            if (c.MyCharacter.tempPet != null && i.TargetID == c.MyCharacter.tempPet.PetID)
                Target = c.MyCharacter.tempPet;
            if (c.MyCharacter.Pet != null && i.AttackerID == c.MyCharacter.Pet.PetID)
                Attacker = c.MyCharacter.Pet;
            if (c.MyCharacter.Pet != null && i.TargetID == c.MyCharacter.Pet.PetID)
                Target = c.MyCharacter.Pet;
            if (i.AttackerType == 3)
                Target = mapEngine.GetMonsterByID(i.AttackerID, c.MyCharacter.MapId);
            if (i.TargetType == 3)
                Target = mapEngine.GetMonsterByID(i.TargetID, c.MyCharacter.MapId);
            if (i.TargetType == 1)
            {
                OtherPlayer = GetClientByCharacterID(i.TargetID);
                if (OtherPlayer.MyCharacter.MapId == 10 && c.MyCharacter.MapId == 10)
                {
                    Target = OtherPlayer.MyCharacter;
                }
            }

            if (i.PacketID == 1)
            {
                if (Target is Character)
                {
                    Character ch = Target as Character;
                    if (ch.Alive)
                    {
                        byte[] SendAttack1 = PacketManager.SendAttack1(i);
                        SendToClients(SendAttack1, clients);
                    }
                }
                if (Target is Monster)
                {
                    Character ch = null;
                    if (Attacker is Character)
                        ch = Attacker as Character;
                    Monster m = Target as Monster;

                    if (m.Alive)
                    {
                        if (ch != null)
                        {
                            float distance = ch.Position.GetDistance(m.Position);
                            if(distance > ch.AttackRange * 2)
                                SendChatToAll(string.Format("{0} Rangehacking, THIS IS JUST A TEST", ch.Name));
                        }
                        byte[] SendAttack1 = PacketManager.SendAttack1(i);
                        SendToClients(SendAttack1, clients);
                    }
                }
            }
            else
            {
                AttackInfo info = null;

                if (Attacker is Pet && Target is Monster)
                {
                    Pet pet = Attacker as Pet;
                    Monster[] monsters = null;

                    if (pet.PetType >= 250)
                    {
                        if (pet.PetType == 250) // summon
                            monsters = mapEngine.GetMonstersByRange(pet, 28);
                        else if (pet.PetType == 251 || pet.PetType == 252 || pet.PetType == 253 || pet.PetType == 254)
                            monsters = mapEngine.GetMonstersByRange(pet, 15);
                        foreach (Monster m in monsters)
                        {
                            Target = m;
                            info = m.OnAttack(pet);

                            if (info == null)
                                return;

                            byte[] SendAttack2 = PacketManager.SendAttack2(info, Target);
                            SendToClients(SendAttack2, clients);

                            if (info.Dead)
                            {
                                CharacterGainExperience(c, m, c.MyCharacter, false);

                                MonsterDropItem(m, c.MyCharacter);

                                byte[] SendMonsterDie = PacketManager.SendMonsterDie(m);
                                SendToClients(SendMonsterDie, clients);
                            }
                        }
                    }
                    else // normal pet we want to level up 
                    {
                        info = Target.OnAttack(pet);

                        if (info == null)
                            return;

                        byte[] SendAttack2 = PacketManager.SendAttack2(info, Target);
                        SendToClients(SendAttack2, clients);

                        if (info.Dead)
                        {
                            PetGainExperience(c, Target, pet);

                            MonsterDropItem(Target, c.MyCharacter);

                            byte[] SendMonsterDie = PacketManager.SendMonsterDie(Target);
                            SendToClients(SendMonsterDie, clients);
                        }
                    }
                }
                else
                {
                    //info = CreateAttackInfo(Attacker, Target);
                    if (Target is Monster && Attacker is Character)
                    {
                        Character ch = Attacker as Character;
                        Monster m = Target as Monster;
                        info = m.OnAttack(ch);

                        if (info == null)
                            return;

                        byte[] SendAttack2 = PacketManager.SendAttack2(info, Target);
                        SendToClients(SendAttack2, clients);

                        if (info.Dead)
                        {
                            CharacterGainExperience(c, Target, c.MyCharacter, false);

                            MonsterDropItem(Target, c.MyCharacter);

                            byte[] SendMonsterDie = PacketManager.SendMonsterDie(Target);
                            SendToClients(SendMonsterDie, clients);
                        }
                    }
                    if (Target is Character && Attacker is Pet)
                    {
                        Character targ = Target as Character;
                        Pet pet = Attacker as Pet;

                        info = targ.OnAttack(pet);

                        if (info == null)
                            return;

                        byte[] SendAttack2 = PacketManager.SendAttack2(info, Target);
                        SendToClients(SendAttack2, clients);

                        byte[] SendHpMana = PacketManager.SendHealMana(targ.MaxHp, targ.CurrentHp, targ.MaxMana, targ.CurrentMana, 0);
                        OtherPlayer.Send(SendHpMana);

                        if (info.PetDamaged)
                        {
                            if (targ.tempPet != null)
                            {
                                byte[] SendPetHealth = PacketManager.SendPetHealth(targ.tempPet);
                                OtherPlayer.Send(SendPetHealth);

                                if (info.PetDied)
                                {
                                    BaseSkill skill = new BaseSkill();
                                    if (targ.RemoveActiveSkill((int)HardSkills.Shadow_Strike))
                                    {
                                        skill.SkillID = (int)HardSkills.Shadow_Strike;
                                        skill.SkillLevel = 1;
                                        OtherPlayer.Send(PacketManager.SendEndSkill(targ, skill));
                                    }
                                    if (targ.RemoveActiveSkill((int)AbsorbSkills.Shadow_Focus))
                                    {
                                        skill.SkillID = (int)AbsorbSkills.Shadow_Focus;
                                        skill.SkillLevel = 1;
                                        OtherPlayer.Send(PacketManager.SendEndSkill(targ, skill));
                                    }
                                    if (targ.RemoveActiveSkill((int)RebirthSkills.Duplicate_Attack))
                                    {
                                        skill.SkillID = (int)RebirthSkills.Duplicate_Attack;
                                        skill.SkillLevel = 1;
                                        OtherPlayer.Send(PacketManager.SendEndSkill(targ, skill));
                                    }


                                    byte[] RemovePet = PacketManager.SendMonsterDie(targ.tempPet);
                                    SendToClients(RemovePet, clients);
                                    targ.tempPet = null;
                                }
                            }
                        }

                        if (info.Dead)
                        {
                            byte[] SendPlayerDie = PacketManager.SendPlayerDie(targ);
                            SendToClients(SendPlayerDie, clients);
                        }
                    }

                    if (Target is Character && Attacker is Character)
                    {
                        Character targ = Target as Character;
                        Character ch = Attacker as Character;

                        info = targ.OnAttack(ch);

                        if (info == null)
                            return;

                        byte[] SendAttack2 = PacketManager.SendAttack2(info, Target);
                        SendToClients(SendAttack2, clients);

                        byte[] SendHpMana = PacketManager.SendHealMana(targ.MaxHp, targ.CurrentHp, targ.MaxMana, targ.CurrentMana, 0);
                        OtherPlayer.Send(SendHpMana);

                        if (info.PetDamaged)
                        {
                            if (targ.tempPet != null)
                            {
                                byte[] SendPetHealth = PacketManager.SendPetHealth(targ.tempPet);
                                OtherPlayer.Send(SendPetHealth);

                                if (info.PetDied)
                                {
                                    BaseSkill skill = new BaseSkill();
                                    if (targ.RemoveActiveSkill((int)HardSkills.Shadow_Strike))
                                    {
                                        skill.SkillID = (int)HardSkills.Shadow_Strike;
                                        skill.SkillLevel = 1;
                                        OtherPlayer.Send(PacketManager.SendEndSkill(targ, skill));
                                    }
                                    if (targ.RemoveActiveSkill((int)AbsorbSkills.Shadow_Focus))
                                    {
                                        skill.SkillID = (int)AbsorbSkills.Shadow_Focus;
                                        skill.SkillLevel = 1;
                                        OtherPlayer.Send(PacketManager.SendEndSkill(targ, skill));
                                    }
                                    if (targ.RemoveActiveSkill((int)RebirthSkills.Duplicate_Attack))
                                    {
                                        skill.SkillID = (int)RebirthSkills.Duplicate_Attack;
                                        skill.SkillLevel = 1;
                                        OtherPlayer.Send(PacketManager.SendEndSkill(targ, skill));
                                    }


                                    byte[] RemovePet = PacketManager.SendMonsterDie(targ.tempPet);
                                    SendToClients(RemovePet, clients);
                                    targ.tempPet = null;
                                }
                            }
                        }

                        if (info.Dead)
                        {
                            byte[] SendPlayerDie = PacketManager.SendPlayerDie(targ);
                            SendToClients(SendPlayerDie, clients);
                        }
                    }
                }
            }
        }

        private void MonsterDropItem(BaseEntity deadEntity, BaseEntity killingEntity)
        {
            bool gotItem = false;
            BaseItem item = null;
            MapItem mapItem = null;
            Monster m = null;
            if (deadEntity is Monster)
                m = deadEntity as Monster;
            Character attacker = killingEntity as Character;
            MapEngine mapEngine = GetMapEngine(attacker.MapId);
            Client[] clients = GetClientsForChars(mapEngine.GetCharactersInRange(attacker.Position, 150).ToArray());

            if (m != null)
            {
                if (m.Level >= 60 && XiahRandom.PercentSuccess(PillDropChance))
                {
                    item = itemDataManager.GetRebirthPillDrop(m);
                    if (item != null)
                        gotItem = true;
                }
                if (XiahRandom.PercentSuccess(StoneDropChance))
                {
                    item = itemDataManager.GetImbueDropItem();
                    if (item != null)
                        gotItem = true;
                }
                if (!gotItem && XiahRandom.PercentSuccess(BookDropChance))
                {
                    item = itemDataManager.GetBookDropItem(m);
                    if (item != null)
                        gotItem = true;
                }
                if (!gotItem && XiahRandom.PercentSuccess(ItemDropChance))
                {
                    item = itemDataManager.GetMonsterDropItem(m);
                    if (item != null)
                        gotItem = true;
                }

                if (XiahRandom.PercentSuccess(MoneyDropChance))
                {
                    BaseItem gold = itemDataManager.GetItemByReferenceID(20042);
                    gold.Amount = (short)(XiahRandom.Next(15, 17) * (m.Level));

                    MapItem map = mapEngine.PlayerDropItem(attacker.CharacterId, gold, (short)m.Position.X, (short)m.Position.Y, 0, m.MapID);

                    byte[] SendItemOnGround = PacketManager.SendItemOnGround(map);
                    SendToClients(SendItemOnGround, clients);
                }

                if (gotItem && item != null)
                {
                    mapItem = mapEngine.PlayerDropItem(attacker.CharacterId, item, (short)m.Position.X, (short)m.Position.Y, 0, m.MapID);

                    byte[] SendItemOnGround = PacketManager.SendItemOnGround(mapItem);
                    SendToClients(SendItemOnGround, clients);
                }
            }
        }

        private void PetGainExperience(Client c, BaseEntity entity, Pet pet)
        {
            if (pet.Level < 200)
            {
                Monster m = null;
                if (entity is Monster)
                    m = entity as Monster;

                long petexp = Math.Abs(pet.CurrentExperience - pet.NegativeExperience);
                bool leveled = false;
                bool evolved = false;

                int experience = m.Experience - pet.Level;

                if (experience < 0)
                    experience = 1;

                if (pet.IsUsingEmpathy)
                {
                    float bonus = (float)(experience * pet.BonusExperience);
                    experience += (int)bonus;
                }

                experience *= ExperienceMultiper;

                if (petexp + experience > pet.ExperienceToLevel)
                {
                    int stat = XiahRandom.Next(0, 4);
                    pet.Level++;
                    int additional = 1;

                    if (pet.Evolution < 3)
                    {
                        if (pet.Level >= pet.EvolutionTable[pet.Evolution].Level)
                        {
                            evolved = true;
                            pet.PetType = pet.EvolutionTable[pet.Evolution].Type;
                            pet.Evolution++;
                            additional += 2 * pet.Evolution;
                        }
                    }

                    switch (stat)
                    {
                        case 0:
                            pet.MaxHealth += (pet.Level * 4) + additional;
                            break;

                        case 1:
                            pet.Damage += (pet.Level * 3) + additional;
                            break;

                        case 2:
                            pet.Defense += (pet.Level * 3) + additional;
                            break;

                        case 3:
                            pet.AttackRating += (pet.Level * 3) + additional;
                            break;
                    }

                    pet.ExperienceToLevel = petManager.GetExperienceToLevelByLevel(pet.Level);
                    pet.NegativeExperience = petManager.GetNegativeExpByLevel(pet.Level);
                    leveled = true;
                    pet.CurHealth = pet.MaxHealth;
                }

                pet.CurrentExperience += experience;

                petManager.UpdatePet(pet);

                byte[] SendPetGainedExperience = PacketManager.SendPetExperienceGained(pet, experience);
                c.Send(SendPetGainedExperience);

                if (leveled)
                {
                    byte[] SendPetLeveled = PacketManager.SendPetLeveled(pet);
                    c.Send(SendPetLeveled);
                }
                if (evolved)
                {
                    byte[] SendPetEvolution = PacketManager.SendPetEvolution(pet);
                    c.Send(SendPetEvolution);
                }
            }
        }

        private void CharacterGainExperience(Client c, BaseEntity entity, Character ch, bool gainedExp)
        {
            if (ch.Party != null && gainedExp == false)
            {
                foreach (Character m in ch.Party.Members)
                {
                    Client member = GetClientByCharacterID(m.CharacterId);
                    CharacterGainExperience(member, entity, m, true);
                }
                return;
            }
            else if (ch.Level < 200)
            {
                Monster m = null;
                if (entity is Monster)
                    m = entity as Monster;

                long charexp = Math.Abs(ch.CurrentExp - ch.NegativeExp);
                bool leveled = false;
                bool training = false;
                int experience = m.Experience - ch.Level;
                if (experience < 0)
                    experience = 1;
                experience = experience * ExperienceMultiper;

                if (ch.Party != null)
                {
                    float maxExp = 1.0f;
                    float divider = 0.1f;
                    divider = maxExp - (divider * ch.Party.MemberCount);

                    experience = (int)(experience * divider);

                    if (experience < 0)
                        experience = 1;
                }

                if (charexp + experience > ch.ExpToTraining)
                {
                    ch.TrainingPoint++;
                    ch.GainedTrainings++;
                    ch.TpLevel++;
                    ch.ExpToTraining = characterManager.GetExperienceToTpByTpLevel(ch.TpLevel);
                    training = true;

                    c.MyCharacter.CalculateTotalStats();
                    c.MyCharacter.CurrentHp = c.MyCharacter.MaxHp;
                    c.MyCharacter.CurrentMana = c.MyCharacter.MaxMana;
                }
                if (charexp + experience > ch.ExpToLevel)
                {
                    ch.StatPoint += (short)ch.GetStatPointsPerLevel();
                    ch.Level++;
                    ch.ExpToLevel = characterManager.GetExperienceToLevelByLevel(ch.Level);
                    ch.NegativeExp = characterManager.GetNegativeExpByLevel(ch.Level);
                    leveled = true;

                    c.MyCharacter.CalculateTotalStats();
                    c.MyCharacter.CurrentHp = c.MyCharacter.MaxHp;
                    c.MyCharacter.CurrentMana = c.MyCharacter.MaxMana;

                    if (ch.Party != null)
                    {
                        foreach (Character p in c.MyCharacter.Party.Members)
                        {
                            Client playerClient = GetClientByCharacterID(p.CharacterId);
                            playerClient.Send(PacketManager.SendParty(c.MyCharacter.Party));
                        }
                    }
                }

                ch.CurrentExp += experience;

                characterManager.UpdateCharacter(ch);

                byte[] SendExpEffectPacket = PacketManager.SendExpEffectPacket(m);
                c.Send(SendExpEffectPacket);

                byte[] SendExpMaybe = PacketManager.SendExperience(ch, experience, 0, leveled, training);
                c.Send(SendExpMaybe);

                byte[] bufferSendStats = PacketManager.SendStats(c.MyCharacter);
                c.Send(bufferSendStats);
            }
        }

        void c_ClientCastSkillInfo(object sender, ClientCastSkillEventArgs e)
        {
            Client c = (Client)sender;
            CastSkillInfo i = e.Info;
            BaseSkill skill = null;
            MapEngine mapEngine = GetMapEngine(c.MyCharacter.MapId);
            Client[] clients = GetClientsForChars(mapEngine.GetCharactersInRange(c.MyCharacter.Position, 150).ToArray());

            skill = c.MyCharacter.FindSkill(i.SkillID);

            if (c.MyCharacter.NotCastedYet(skill))
            {
                if (i.PacketID == 1)
                {
                    bool cast = false;
                    if (c.MyCharacter.CurrentMana >= skill.ManaCost)
                    {
                        if (skill.KeepUpTime > 0 && skill.SkillID != (int)HardSkills.Haze && skill.SkillID != (int)HardSkills.Power_Up_Pet && skill.SkillID != (int)HardSkills.Empathy && skill.SkillID != (int)RebirthSkills.Strengthen_Monster)
                            cast = true;
                        else if (skill.SkillID == (int)HardSkills.Haze)
                        {
                            if (i.TargetID != 0)
                            {
                                Monster m = mapEngine.GetMonsterByID(i.TargetID, c.MyCharacter.MapId);
                                Monster[] monsters = mapEngine.GetMonstersByRange(m, skill.Distance);

                                int distance = MapEngine.GetDistance(c.MyCharacter.Position.X, c.MyCharacter.Position.Y, m.Position.X, m.Position.Y);

                                if (distance <= skill.Distance)
                                {
                                    cast = true;

                                    byte[] SendHazeEffect = PacketManager.SendSkillEffect(m, skill);
                                    SendToClients(SendHazeEffect, clients);

                                    HazeSkill hazeSkill = new HazeSkill(DateTime.Now, skill, monsters, c.MyCharacter, m);

                                    hazeSkill.HazeDamageCasted += new EventHandler<HazeDamageCastedEventArgs>(hazeSkill_HazeDamageCasted);
                                    hazeSkill.HazeEnded += new EventHandler(hazeSkill_HazeEnded);
                                    hazeSkill.DoEffect();
                                }
                            }
                        }
                        else if (skill.SkillID == (int)HardSkills.Power_Up_Pet)
                        {
                            if (i.TargetID != 0)
                            {
                                if (c.MyCharacter.Pet != null && c.MyCharacter.Pet.PetID == i.TargetID && !c.MyCharacter.Pet.IsUsingBuff)
                                {
                                    Pet pet = c.MyCharacter.Pet;
                                    cast = true;

                                    //byte[] SendPutmEffect = PacketManager.SendSkillEffect(pet, skill);
                                    //SendSpecific(nearbyClients, SendPutmEffect);

                                    PUTM putmSkill = new PUTM(DateTime.Now, skill, pet, c.MyCharacter);
                                    putmSkill.PUTMCasted += new EventHandler(putmSkill_PUTMCasted);
                                    putmSkill.PUTMEnded += new EventHandler(putmSkill_PUTMEnded);
                                    putmSkill.DoEffect();
                                }
                            }
                        }
                        else if (skill.SkillID == (int)HardSkills.Empathy)
                        {
                            if (i.TargetID != 0)
                            {
                                if (c.MyCharacter.Pet != null && c.MyCharacter.Pet.PetID == i.TargetID && !c.MyCharacter.Pet.IsUsingEmpathy)
                                {
                                    Pet pet = c.MyCharacter.Pet;
                                    cast = true;

                                    EmpathySkill empathySkill = new EmpathySkill(DateTime.Now, skill, pet, c.MyCharacter);
                                    empathySkill.EmpathyCasted += new EventHandler(empathySkill_EmpathyCasted);
                                    empathySkill.EmpathyEnded += new EventHandler(empathySkill_EmpathyEnded);
                                    empathySkill.DoEffect();
                                }
                            }
                        }
                        else if (skill.SkillID == (int)RebirthSkills.Strengthen_Monster)
                        {
                            if (i.TargetID != 0)
                            {
                                if (c.MyCharacter.Pet != null && c.MyCharacter.Pet.PetID == i.TargetID && !c.MyCharacter.Pet.IsUsingBuff)
                                {
                                    Pet pet = c.MyCharacter.Pet;
                                    cast = true;

                                    StrengthenMonster strMon = new StrengthenMonster(DateTime.Now, skill, pet, c.MyCharacter);
                                    strMon.StrMonsterCasted += new EventHandler(strMons_strMonCasted);
                                    strMon.StrMonsterEnded += new EventHandler(strMon_strMonEnded);
                                    strMon.DoEffect();
                                }
                            }
                        }

                        if (skill.SkillID == (int)HardSkills.Energy_Bomb || skill.SkillID == (int)HardSkills.Crushing_Hands || skill.SkillID == (int)HardSkills.Death_Energy || skill.SkillID == (int)RebirthSkills.Rising_Fire_Dragon)
                            cast = true;
                        if (skill.SkillID == (int)HardSkills.Attack_Song || skill.SkillID == (int)HardSkills.Sword_Missle || skill.SkillID == (int)HardSkills.Poison_Shot || skill.SkillID == (int)HardSkills.Fist_Wind)
                        {
                            Monster m = mapEngine.GetMonsterByID(i.TargetID, c.MyCharacter.MapId);
                            if (m != null)
                            {
                                int distance = MapEngine.GetDistance(c.MyCharacter.Position.X, c.MyCharacter.Position.Y, m.Position.X, m.Position.Y);

                                if (distance <= skill.Distance)
                                    cast = true;
                            }
                        }

                        if (cast)
                        {
                            c.MyCharacter.CurrentMana -= skill.ManaCost;

                            byte[] SendCastSkill1 = PacketManager.SendCast1(skill, i, 0);
                            SendToClients(SendCastSkill1, clients);
                            // c.Send(SendCastSkill1);

                            byte[] SendStats = PacketManager.SendStats(c.MyCharacter);
                            c.Send(SendStats);
                        }

                        if (skill.KeepUpTime > 0 && skill.SkillID != (int)HardSkills.Haze && skill.SkillID != (int)HardSkills.Power_Up_Pet)
                        {

                            if (skill.SkillID == (int)HardSkills.Summon_Monster || skill.SkillID == (int)HardSkills.Shadow_Strike || skill.SkillID == (int)AbsorbSkills.Shadow_Focus || skill.SkillID == (int)RebirthSkills.Duplicate_Attack)
                            {
                                if (c.MyCharacter.tempPet == null)
                                {
                                    byte type = 250;
                                    if (skill.SkillID == (int)HardSkills.Summon_Monster)
                                        type = 250;
                                    if (skill.SkillID == (int)HardSkills.Shadow_Strike || skill.SkillID == (int)RebirthSkills.Duplicate_Attack)
                                        type = 251;
                                    if (skill.SkillID == (int)AbsorbSkills.Shadow_Focus)
                                    {
                                        switch (c.MyCharacter.Class)
                                        {
                                            case 1:
                                                type = 251;
                                                break;
                                            case 2:
                                                type = 252;
                                                break;
                                            case 3:
                                                type = 253;
                                                break;
                                            case 4:
                                                type = 254;
                                                break;
                                            default:
                                                type = 250;
                                                break;
                                        }
                                    }
                                    Pet tempPet = new Pet
                                    {
                                        PetType = type,
                                        CurrentExperience = 0,
                                        Name = "Summon",
                                        MapID = c.MyCharacter.MapId,
                                        OwnerID = c.MyCharacter.CharacterId,
                                        Position = new Position(c.MyCharacter.Position.X, c.MyCharacter.Position.Y, c.MyCharacter.Position.Z),
                                        CurHealth = c.MyCharacter.CurrentHp,
                                        MaxHealth = c.MyCharacter.MaxHp,
                                        Level = 100,
                                        Damage = c.MyCharacter.TotalDamage,
                                        Defense = c.MyCharacter.TotalDefence,
                                        AttackRating = c.MyCharacter.TotalAttackRating,
                                    };

                                    if (skill.IncDamagePerc != 0)
                                    {
                                        float DamageInc = skill.IncDamagePerc;
                                        float increase = (DamageInc / 100);
                                        tempPet.Damage = (int)(tempPet.Damage * increase);
                                    }
                                    if (skill.IncHpCurPerc != 0)
                                    {
                                        float HpInc = skill.IncHpCurPerc;
                                        float increase = (HpInc / 100);
                                        tempPet.MaxHealth = (int)(tempPet.MaxHealth * increase);
                                        tempPet.CurHealth = tempPet.MaxHealth;
                                    }

                                    if (skill.SkillID == (int)RebirthSkills.Duplicate_Attack)
                                    {
                                        tempPet.Damage *= 2;
                                        tempPet.Defense *= 2;
                                        tempPet.AttackRating *= 2;
                                        tempPet.MaxHealth *= 2;
                                        tempPet.CurHealth = tempPet.MaxHealth;
                                    }

                                    tempPet.PetID = petManager.InsertPet(tempPet, false);

                                    c.MyCharacter.tempPet = tempPet;
                                    byte[] SendSummon = PacketManager.SendSummonSpawn(tempPet);
                                    SendToClients(SendSummon, clients);
                                }
                                else
                                    return;
                            }
                            c.MyCharacter.ActiveSkills.Add(new ActiveSkill(DateTime.Now, skill, null, c.MyCharacter));

                            byte[] SendSkillEffect = PacketManager.SendSkillEffect(c.MyCharacter, skill);
                            SendToClients(SendSkillEffect, clients);

                            byte[] SendStats = PacketManager.SendStats(c.MyCharacter);
                            c.Send(SendStats);
                        }
                    }
                    else
                    {
                        byte[] SendNoMana = PacketManager.SendCast1(skill, i, 1);
                        c.Send(SendNoMana);
                    }
                }
                else
                {
                    if (skill.SkillID == (int)HardSkills.Energy_Bomb || skill.SkillID == (int)HardSkills.Crushing_Hands || skill.SkillID == (int)HardSkills.Death_Energy || skill.SkillID == (int)RebirthSkills.Rising_Fire_Dragon)
                    {
                        Monster[] monsters = mapEngine.GetMonstersByRange(c.MyCharacter, skill.Distance);

                        foreach (Monster m in monsters)
                        {
                            CastSkillInfo info = m.OnCast(c.MyCharacter, skill);

                            byte[] SendDamage = PacketManager.SendCast2(skill, info, m);
                            SendToClients(SendDamage, clients);

                            if (info.Dead)
                            {
                                CharacterGainExperience(c, m, c.MyCharacter, false);

                                MonsterDropItem(m, c.MyCharacter);

                                byte[] SendMonsterDie = PacketManager.SendMonsterDie(m);
                                SendToClients(SendMonsterDie, clients);
                            }
                        }
                    }

                    if (skill.SkillID == (int)HardSkills.Attack_Song || skill.SkillID == (int)HardSkills.Sword_Missle || skill.SkillID == (int)HardSkills.Poison_Shot || skill.SkillID == (int)HardSkills.Fist_Wind)
                    {
                        Monster m = mapEngine.GetMonsterByID(i.TargetID, c.MyCharacter.MapId);
                        int distance = MapEngine.GetDistance(c.MyCharacter.Position.X, c.MyCharacter.Position.Y, m.Position.X, m.Position.Y);
                        if (m != null && m.Alive && distance < skill.Distance && c.MyCharacter.Alive)
                        {
                            CastSkillInfo info = m.OnCast(c.MyCharacter, skill);

                            byte[] SendDamage = PacketManager.SendCast2(skill, info, m);
                            SendToClients(SendDamage, clients);

                            if (info.Dead)
                            {
                                CharacterGainExperience(c, m, c.MyCharacter, false);

                                MonsterDropItem(m, c.MyCharacter);

                                byte[] SendMonsterDie = PacketManager.SendMonsterDie(m);
                                SendToClients(SendMonsterDie, clients);
                            }
                        }
                    }
                    else
                    {
                        byte[] SendCastSkill2 = PacketManager.SendCast2(skill, i, null);
                        c.Send(SendCastSkill2);
                    }
                }
            }
        }

        void strMon_strMonEnded(object sender, EventArgs e)
        {
            StrengthenMonster str = sender as StrengthenMonster;
            Client c = GetClientByCharacterID(str.Caster.CharacterId);
            MapEngine mapEngine = GetMapEngine(c.MyCharacter.MapId);
            Client[] clients = GetClientsForChars(mapEngine.GetCharactersInRange(c.MyCharacter.Position, 150).ToArray());

            byte[] endSkillEffect = PacketManager.SendEndSkill(str.Target, str.Skill);
            SendToClients(endSkillEffect, clients);

            byte[] SendPetStats = PacketManager.SendPetStats(str.Target);
            c.Send(SendPetStats);
        }

        void strMons_strMonCasted(object sender, EventArgs e)
        {
            StrengthenMonster str = sender as StrengthenMonster;
            Client c = GetClientByCharacterID(str.Caster.CharacterId);
            MapEngine mapEngine = GetMapEngine(c.MyCharacter.MapId);
            Client[] clients = GetClientsForChars(mapEngine.GetCharactersInRange(c.MyCharacter.Position, 150).ToArray());

            byte[] SendSkillEffect = PacketManager.SendSkillEffect(str.Target, str.Skill);
            SendToClients(SendSkillEffect, clients);

            byte[] SendPetStats = PacketManager.SendPetStats(str.Target);
            c.Send(SendPetStats);
        }

        void empathySkill_EmpathyEnded(object sender, EventArgs e)
        {
            EmpathySkill emph = sender as EmpathySkill;
            Client c = GetClientByCharacterID(emph.Caster.CharacterId);
            MapEngine mapEngine = GetMapEngine(c.MyCharacter.MapId);
            Client[] clients = GetClientsForChars(mapEngine.GetCharactersInRange(c.MyCharacter.Position, 150).ToArray());

            byte[] endSkillEffect = PacketManager.SendEndSkill(emph.Target, emph.Skill);
            SendToClients(endSkillEffect, clients);

            byte[] SendPetStats = PacketManager.SendPetStats(emph.Target);
            c.Send(SendPetStats);
        }

        void empathySkill_EmpathyCasted(object sender, EventArgs e)
        {
            EmpathySkill emph = sender as EmpathySkill;
            Client c = GetClientByCharacterID(emph.Caster.CharacterId);
            MapEngine mapEngine = GetMapEngine(c.MyCharacter.MapId);
            Client[] clients = GetClientsForChars(mapEngine.GetCharactersInRange(c.MyCharacter.Position, 150).ToArray());

            byte[] SendEmpathyEffect = PacketManager.SendSkillEffect(emph.Target, emph.Skill);
            SendToClients(SendEmpathyEffect, clients);

            byte[] SendPetStats = PacketManager.SendPetStats(emph.Target);
            c.Send(SendPetStats);
        }

        void putmSkill_PUTMEnded(object sender, EventArgs e)
        {
            PUTM putm = sender as PUTM;
            Client c = GetClientByCharacterID(putm.Caster.CharacterId);
            MapEngine mapEngine = GetMapEngine(c.MyCharacter.MapId);
            Client[] clients = GetClientsForChars(mapEngine.GetCharactersInRange(c.MyCharacter.Position, 150).ToArray());

            byte[] endSkillEffect = PacketManager.SendEndSkill(putm.Target, putm.Skill);
            SendToClients(endSkillEffect, clients);

            byte[] SendPetStats = PacketManager.SendPetStats(putm.Target);
            c.Send(SendPetStats);
        }

        void putmSkill_PUTMCasted(object sender, EventArgs e)
        {
            PUTM putm = sender as PUTM;
            Client c = GetClientByCharacterID(putm.Caster.CharacterId);
            MapEngine mapEngine = GetMapEngine(c.MyCharacter.MapId);
            Client[] clients = GetClientsForChars(mapEngine.GetCharactersInRange(c.MyCharacter.Position, 150).ToArray());

            byte[] SendPutmEffect = PacketManager.SendSkillEffect(putm.Target, putm.Skill);
            SendToClients(SendPutmEffect, clients);

            byte[] SendPetStats = PacketManager.SendPetStats(putm.Target);
            c.Send(SendPetStats);
        }

        void hazeSkill_HazeEnded(object sender, EventArgs e)
        {
            HazeSkill haze = sender as HazeSkill;
            Client c = GetClientByCharacterID(haze.Caster.CharacterId);
            MapEngine mapEngine = GetMapEngine(c.MyCharacter.MapId);
            Client[] clients = GetClientsForChars(mapEngine.GetCharactersInRange(c.MyCharacter.Position, 150).ToArray());

            byte[] endSkillEffect = PacketManager.SendEndSkill(haze.HitTarget, haze.Skill);
            SendToClients(endSkillEffect, clients);
            // send spefic etc, need the nearbyclients and monster youve casted haze on
        }

        void hazeSkill_HazeDamageCasted(object sender, HazeDamageCastedEventArgs e)
        {
            Monster[] targets = e.HitTargets as Monster[];
            HazeSkill haze = sender as HazeSkill;
            Monster hitTarget = haze.HitTarget as Monster;
            Client[] nearbyClients = GetNearByClients(hitTarget);
            Client c = GetClientByCharacterID(haze.Caster.CharacterId);

            foreach (Monster m in targets)
            {
                CastSkillInfo info = m.OnCast(c.MyCharacter, haze.Skill);
 

                byte[] SendDamage = PacketManager.SendCast2(haze.Skill, info, m);
                SendSpecific(nearbyClients, SendDamage);

                if (info.Dead)
                {
                    CharacterGainExperience(c, m, c.MyCharacter, false);

                    MonsterDropItem(m, c.MyCharacter);

                    byte[] SendMonsterDie = PacketManager.SendMonsterDie(m);
                    SendSpecific(nearbyClients, SendMonsterDie);

                    if (m == hitTarget)
                        haze.EndHaze();
                }
            }
        }

        void c_ClientUseTeleporterInfo(object sender, ClientUseTeleporterEventArgs e)
        {
            Client c = (Client)sender;
            UseTeleporterInfo i = e.Info;

            Map tomap = null;

            if (i.ToMap != 0)
                tomap = GetMapEngine(i.ToMap).Map; // its real bead which has mapid etc
            else
                tomap = GetMapEngine(c.MyCharacter.MapId).Map; // its either town portal scroll or bugged so its win/win for us

            Portal tempPort = new Portal
            {
                ToMapID = tomap.MapID,
                ToX = (short)tomap.SpawnX,
                ToY = (short)tomap.SpawnY
            };

            MapEngine oldMap = GetMapEngine(c.MyCharacter.MapId);
            Client[] clients = GetClientsForChars(oldMap.GetCharactersInRange(c.MyCharacter.Position, 150).ToArray());


            byte[] SendRemoveCharacter = PacketManager.SendRemoveCharacter(c.MyCharacter, RemoveCharacterEffect.Bead);
            SendToClients(SendRemoveCharacter, clients);

            c.MyCharacter.Position.X = (short)tomap.SpawnX;
            c.MyCharacter.Position.Y = (short)tomap.SpawnY;
            c.MyCharacter.OldMapId = c.MyCharacter.MapId;
            c.MyCharacter.MapId = tomap.MapID;

            if (c.MyCharacter.Pet != null && !c.MyCharacter.Pet.IsSealed)
            {
                byte[] bufferRemovePet = PacketManager.SendRemovePet(c.MyCharacter.Pet);
                SendToClients(bufferRemovePet, clients);
                
                oldMap.ActiveClientEntities.Remove(c.MyCharacter.Pet);
                c.MyCharacter.Pet.Position.X = c.MyCharacter.Position.X;
                c.MyCharacter.Pet.Position.Y = c.MyCharacter.Position.Y;
                c.MyCharacter.Pet.MapID = c.MyCharacter.MapId;
            }
            MapEngine mapEngine = GetMapEngine(c.MyCharacter.MapId);


            c.MyCharacter.Map = mapEngine.Map;
            characterManager.UpdateCharacter(c.MyCharacter);

            byte[] SendPortal = PacketManager.SendPortal(tempPort);
            c.Send(SendPortal);
        }

        void c_ClientCombineItemsInfo(object sender, ClientCombineItemsEvenArgs e)
        {
            Client c = (Client)sender;
            CombineItemsInfo i = e.Info;
            BaseItem item = null;

            try
            {
                item = c.MyCharacter.Bags[i.Bag - 1].Items.Single(x => x.ItemID == i.ItemID);

                if (item is ImbueItem)
                {
                    ImbueItem combined = item as ImbueItem;
                    byte stat = 0;

                    if (XiahRandom.PercentSuccess(StoneCombineChance))
                    {
                        if (combined is Red)
                        {
                            stat = (byte)XiahRandom.Next(2, 6);
                        }
                        if (combined is White)
                        {
                            stat = (byte)XiahRandom.Next(2, 10);
                        }
                        if (combined is Black)
                        {
                            stat = (byte)XiahRandom.Next(2, 4);

                            if (combined.VisualID == 830) // best jewel
                            {
                                if (stat == 2)
                                    combined.ImbueChance = 35;
                                if (stat == 3)
                                {
                                    switch (combined.IncreaseValue)
                                    {
                                        case 15:
                                            combined.IncreaseValue = 40;
                                            break;
                                        case 40:
                                            combined.IncreaseValue = 70;
                                            break;
                                        case 70:
                                            combined.IncreaseValue = 100;
                                            break;
                                    }
                                }
                            }
                            if (combined.VisualID == 821) // jewel
                            {
                                if (stat == 2)
                                    combined.ImbueChance = 25;
                                if (stat == 3)
                                {
                                    switch (combined.IncreaseValue)
                                    {
                                        case 10:
                                            combined.IncreaseValue = 20;
                                            break;
                                        case 20:
                                            combined.IncreaseValue = 35;
                                            break;
                                        case 35:
                                            combined.IncreaseValue = 50;
                                            break;
                                    }
                                }
                            }
                            if (combined.VisualID == 801) // crystal
                            {
                                if (stat == 2)
                                    combined.ImbueChance = 10;
                                if (stat == 3)
                                    combined.IncreaseValue = 5;
                            }
                            if (combined.VisualID == 811) // stone
                            {
                                if (stat == 2)
                                    combined.ImbueChance = 2;
                                if (stat == 3)
                                    combined.IncreaseValue = 2;
                            }
                        }

                        combined.ImbueData = stat;

                        c.Send(PacketManager.SendCombineMessage(0));
                        c.Send(PacketManager.SendImbuedItem(combined));
                        itemDataManager.UpdateItem(combined);
                    }
                    else
                    {
                        byte[] SendFailed = PacketManager.SendCombineMessage(1);
                        c.Send(SendFailed);

                        byte[] SendRemoveItem = PacketManager.SendRemoveItem(combined.Bag, combined.Slot);
                        c.Send(SendRemoveItem);

                        itemDataManager.DeleteItem(combined.ItemID);
                        c.MyCharacter.Bags[combined.Bag - 1].RemoveItem(combined);
                    }
                }
                else
                {
                    c.Send(PacketManager.SendCombineMessage(0));
                }
            }
            catch
            {
                c.Send(PacketManager.SendCombineMessage(0));
            }
        }

        void c_ClientPartyRequestInfo(object sender, ClientPartyRequestEventArgs e)
        {
            Client c = (Client)sender;
            PartyRequestInfo i = e.Info;
            Client asker = null;
            Client target = null;
            MapEngine mapEngine = GetMapEngine(c.MyCharacter.MapId);
            Client[] clients = GetClientsForChars(mapEngine.GetCharactersInRange(c.MyCharacter.Position, 150).ToArray());

            target = GetClientByCharacterID(i.TargetID);
            asker = GetClientByCharacterID(i.AskerID);

            if (i.Type == PartyType.Normal)
            {
                if (i.Error == PartyError.None)
                {
                    if (target.MyCharacter.Party != null)
                    {
                        i.Error = PartyError.AlreadyInParty;
                        asker.Send(PacketManager.SendPartyAnswer(i));
                        return;
                    }

                    if (target.MyCharacter.Level >= asker.MyCharacter.Level - 10 && target.MyCharacter.Level <= asker.MyCharacter.Level + 10)
                    {
                        byte[] SendPartyRequest = PacketManager.SendPartyAnswer(i);
                        target.Send(SendPartyRequest);
                    }
                    else
                    {
                        i.Error = PartyError.LevelDifference;
                        byte[] SendLevelDifference = PacketManager.SendPartyAnswer(i);
                        asker.Send(SendLevelDifference);
                    }

                }
                if (i.Error == PartyError.Cancel)
                {
                    asker.Send(PacketManager.SendPartyAnswer(i));
                }
                if (i.Error == PartyError.Accept)
                {
                    asker.Send(PacketManager.SendPartyAnswer(i));
                    asker.Send(PacketManager.SendPartyLeader(i));
                    target.Send(PacketManager.SendPartyLeader(i));

                    Party p = new Party
                    {
                        PartyID = asker.MyCharacter.CharacterId,
                        Leader = asker.MyCharacter,
                        Members = new List<Character>(),
                        Type = i.Type
                    };
                    p.Members.Add(asker.MyCharacter);
                    p.Members.Add(target.MyCharacter);

                    asker.MyCharacter.Party = p;
                    target.MyCharacter.Party = p;

                    asker.Send(PacketManager.SendAddPartyMember(p, target.MyCharacter));

                    asker.Send(PacketManager.SendParty(p));
                    target.Send(PacketManager.SendParty(p));

                    SendToClients(PacketManager.SendSpawnPlayers(asker.MyCharacter), clients);
                    SendToClients(PacketManager.SendSpawnPlayers(target.MyCharacter), clients);
                }
            }
            if (i.Type == PartyType.Relation)
            {
                try
                {
                    Friend f = asker.MyCharacter.FriendList.Single(x => x.FriendID == i.TargetID);
                    if (f.RelationType == FriendAddTypes.Lover || f.RelationType == FriendAddTypes.TeacherStudent)
                    {
                        asker.Send(PacketManager.SendPartyAnswer(i));
                        asker.Send(PacketManager.SendPartyLeader(i));
                        target.Send(PacketManager.SendPartyLeader(i));

                        Party p = new Party
                        {
                            PartyID = asker.MyCharacter.CharacterId,
                            Leader = asker.MyCharacter,
                            Members = new List<Character>(),
                            Type = i.Type
                        };
                        p.Members.Add(asker.MyCharacter);
                        p.Members.Add(target.MyCharacter);

                        asker.MyCharacter.Party = p;
                        target.MyCharacter.Party = p;

                        asker.Send(PacketManager.SendAddPartyMember(p, target.MyCharacter));

                        asker.Send(PacketManager.SendParty(p));
                        target.Send(PacketManager.SendParty(p));

                        SendToClients(PacketManager.SendSpawnPlayers(asker.MyCharacter), clients);
                        SendToClients(PacketManager.SendSpawnPlayers(target.MyCharacter), clients);
                    }
                    else
                        throw new Exception();
                }
                catch
                {
                    i.Error = PartyError.LevelDifference;

                    byte[] SendError = PacketManager.SendPartyAnswer(i);
                    asker.Send(SendError);
                }
            }
        }

        void c_ClientAddToPartyRequestInfo(object sender, ClientAddToPartyRequestEventArgs e)
        {
            Client c = (Client)sender;
            AddToPartyRequestInfo i = e.Info;
            Client asker = null;
            Client target = null;
            MapEngine mapEngine = GetMapEngine(c.MyCharacter.MapId);
            Client[] clients = GetClientsForChars(mapEngine.GetCharactersInRange(c.MyCharacter.Position, 150).ToArray());

            target = GetClientByCharacterID(i.TargetID);
            asker = GetClientByCharacterID(i.AskerID);

            if (i.Type == PartyType.Normal)
            {
                if (i.Error == PartyError.None)
                {
                    if (target.MyCharacter.Party != null)
                    {
                        i.Error = PartyError.CannotCreate;
                        asker.Send(PacketManager.SendAddToPartyAnswer(i));
                        return;
                    }

                    if (target.MyCharacter.Level >= asker.MyCharacter.Level - 10 && target.MyCharacter.Level <= asker.MyCharacter.Level + 10)
                    {
                        byte[] SendPartyRequest = PacketManager.SendAddToPartyAnswer(i);
                        target.Send(SendPartyRequest);
                    }
                    else
                    {
                        i.Error = PartyError.CannotCreate;
                        byte[] SendLevelDifference = PacketManager.SendAddToPartyAnswer(i);
                        asker.Send(SendLevelDifference);
                    }

                }
                if (i.Error == PartyError.Cancel)
                {
                    asker.Send(PacketManager.SendAddToPartyAnswer(i));
                }
                if (i.Error == PartyError.Accept)
                {
                    asker.Send(PacketManager.SendAddToPartyAnswer(i));
                    asker.Send(PacketManager.SendAddToPartyAnswer(i));
                    target.Send(PacketManager.SendAddToPartyAnswer(i));

                    asker.MyCharacter.Party.Members.Add(target.MyCharacter);

                    foreach (Character ch in asker.MyCharacter.Party.Members)
                    {
                        Client member = GetClientByCharacterID(ch.CharacterId);
                        ch.Party = asker.MyCharacter.Party;
                        member.Send(PacketManager.SendAddPartyMember(ch.Party, target.MyCharacter));
                        member.Send(PacketManager.SendParty(ch.Party));

                    }

                    SendToClients(PacketManager.SendSpawnPlayers(asker.MyCharacter), clients);
                    SendToClients(PacketManager.SendSpawnPlayers(target.MyCharacter), clients);
                }
            }
            else
            {
                i.Error = PartyError.CannotCreate;
                asker.Send(PacketManager.SendAddToPartyAnswer(i));
            }
        }

        void c_ClientLeavePartyInfo(object sender, ClientLeavePartyEventArgs e)
        {
            Client c = (Client)sender;
            LeavePartyInfo i = e.Info;

            if (c.MyCharacter.Party != null)
                ClientLeaveParty(c);
        }

        private void ClientLeaveParty(Client c)
        {
            byte[] SendLeaveParty = PacketManager.SendLeaveParty(c.MyCharacter.Party, c.MyCharacter);

            if (c.MyCharacter.Party.MemberCount == 2)
            {
                byte[] SendKillParty = PacketManager.SendKillParty(c.MyCharacter.Party);
                foreach (Character ch in c.MyCharacter.Party.Members)
                {
                    Client member = GetClientByCharacterID(ch.CharacterId);
                    if (member != null)
                    {
                        member.Send(SendLeaveParty);
                        member.Send(SendKillParty);

                        ch.Party = null;
                    }
                }
            }
            else
            {
                c.MyCharacter.Party.Members.Remove(c.MyCharacter);
                c.Send(SendLeaveParty);

                if (c.MyCharacter == c.MyCharacter.Party.Leader)
                {
                    Character newLeader = null;
                    if (c.MyCharacter.Party.Members[1] != c.MyCharacter)
                        newLeader = c.MyCharacter.Party.Members[1];

                    c.MyCharacter.Party.Leader = newLeader;
                    byte[] SendChangeLeader = PacketManager.SendChangePartyLeader(c.MyCharacter.Party);

                    foreach (Character ch in c.MyCharacter.Party.Members)
                    {
                        Client member = GetClientByCharacterID(ch.CharacterId);
                        member.Send(SendLeaveParty);
                        member.Send(SendChangeLeader);

                        ch.Party = c.MyCharacter.Party;
                    }
                }
                else
                {
                    foreach (Character ch in c.MyCharacter.Party.Members)
                    {
                        Client member = GetClientByCharacterID(ch.CharacterId);
                        if (member != null)
                        {
                            member.Send(SendLeaveParty);

                            ch.Party = c.MyCharacter.Party;
                        }
                    }
                }
            }
        }



        void c_ClientChangePartyLeaderInfo(object sender, ClientChangePartyLeaderEventArgs e)
        {
            Client c = (Client)sender;
            ChangePartyLeaderInfo i = e.Info;
            Client newLeader = GetClientByCharacterID(i.NewLeader);

            c.MyCharacter.Party.Leader = newLeader.MyCharacter;
            byte[] changeLeader = PacketManager.SendChangePartyLeader(c.MyCharacter.Party);

            foreach (Character ch in c.MyCharacter.Party.Members)
            {
                Client member = GetClientByCharacterID(ch.CharacterId);
                member.Send(changeLeader);

                ch.Party.Leader = newLeader.MyCharacter;
            }
        }

        void c_ClientCreateGuildInfo(object sender, ClientCreateGuildInfoEventArgs e)
        {
            Client c = (Client)sender;
            CreateGuildInfo i = e.Info;

            // perform check if enought gold/fame and also check if guild name doesnt already exist

            Guild guild = guildManager.CreateGuild(i.GuildName, c.MyCharacter.CharacterId);

            if (guild != null)
            {
                c.MyCharacter.Guild = guild;

                c.Send(PacketManager.SendGuildCreate(c.MyCharacter.Guild));
                c.Send(PacketManager.SendGuildJoin(c.MyCharacter));
                c.Send(PacketManager.SendGuildMember(c.MyCharacter.Guild, c.MyCharacter.Guild.Members[0]));

                // send money left, send stats.
            }
            // send there was error ..

        }

        void c_ClientRequestGuildInfo(object sender, ClientRequestGuildEventArgs e)
        {
            Client c = (Client)sender;

            if (c.MyCharacter.Guild != null)
            {
                UpdateGuildMemberStatus(c.MyCharacter.Guild);
                c.Send(PacketManager.SendGuild(c.MyCharacter.Guild));
            }
        }


        void c_ClientRequestGuildInformation(object sender, ClientRequestGuildInfoEventArgs e)
        {
            Client c = (Client)sender;
            RequestGuildInfo i = e.Info;

            Guild g = c.MyCharacter.Guild;

            if (g != null)
            {
                UpdateGuildMemberStatus(g);
                GuildMember me = g.Members.Single(x => x.Character.CharacterId == c.MyCharacter.CharacterId);

                c.Send(PacketManager.SendGuildMember(g, me));

                if (c.MyCharacter.Guild.Notice.Length > 0)
                    c.Send(PacketManager.SendGuildNotice(c.MyCharacter.Guild));

                foreach(GuildMember m in g.Members)
                {
                    SendToGuild(c.MyCharacter.Guild, PacketManager.SendGuildMemberInfo(m));
                }

                c.Send(PacketManager.SendGuildStats(g));
            }
        }

        void c_ClientRequestGuildMemberStatsInfo(object sender, ClientRequestGuildMemberStatsEventArgs e)
        {
            Client c = (Client)sender;
            RequestGuildMemberStatsInfo i = e.Info;

            UpdateGuildMemberStatus(c.MyCharacter.Guild);
            GuildMember g = c.MyCharacter.Guild.Members.Single(x => x.Character.CharacterId == i.MemberID);

            c.Send(PacketManager.SendGuildMemberInfo(g));
        }


        void c_ClientRequestGuildMemberChangeRankInfo(object sender, ClientRequestGuildMemberRankChangeEventArgs e)
        {
            Client c = (Client)sender;
            RequestGuildMemberChangeRankInfo i = e.Info;

            GuildMember me = c.MyCharacter.Guild.Members.Single(x => x.Character.CharacterId == c.MyCharacter.CharacterId);

            if (me.Rank <= GuildOrder.Second_Master)
            {
                if (i.NewRank == GuildOrder.Guild_Master)
                    return; // NO MAKE GUILD MASTER ARE 

                GuildMember member = c.MyCharacter.Guild.Members.Single(x => x.Character.CharacterId == i.MemberID);
                member.Rank = i.NewRank;

                SendToGuild(c.MyCharacter.Guild, PacketManager.SendGuildChangeMemberRank(member, GuildMemberError.Success));
                //send in guild
                guildManager.UpdateGuildMember(member);
            }
            else
            {
                c.Send(PacketManager.SendGuildChangeMemberRank(null, GuildMemberError.NoRights));
            }
        }


        void c_ClientRequestGuildMemberChangeTitleInfo(object sender, ClientRequestGuildMemberChangeTitleEventArgs e)
        {
            Client c = (Client)sender;
            RequestGuildMemberChangeTitleInfo i = e.Info;

            GuildMember me = c.MyCharacter.Guild.Members.Single(x => x.Character.CharacterId == c.MyCharacter.CharacterId);

            if (me.Rank <= GuildOrder.Elder)
            {
                try
                {
                    GuildMember member = c.MyCharacter.Guild.Members.Single(x => x.Character.CharacterId == i.MemberID);
                    member.Title = i.NewTitle;

                    SendToGuild(c.MyCharacter.Guild, PacketManager.SendGuildChangeMemberTitle(c.MyCharacter.Guild, member, GuildMemberError.Success));

                    SendToGuild(c.MyCharacter.Guild, PacketManager.SendGuildMember(c.MyCharacter.Guild, member));
                    // send in guild, send new guild packet to everyone around member with the title

                    guildManager.UpdateGuildMember(member);
                }
                catch
                {
                    c.Send(PacketManager.SendGuildChangeMemberTitle(c.MyCharacter.Guild, null, GuildMemberError.NoMember));
                }
            }
            else
            {
                c.Send(PacketManager.SendGuildChangeMemberTitle(c.MyCharacter.Guild, null, GuildMemberError.NoRights));
            }
        }


        void c_ClientRequestGuildChangeNoticeInfo(object sender, ClientRequestGuildChangeNoticeEventArgs e)
        {
            Client c = (Client)sender;
            RequestGuildChangeNoticeInfo i = e.Info;

            if (c.MyCharacter.Guild.Leader.Character.CharacterId == c.MyCharacter.CharacterId)
            {
                c.MyCharacter.Guild.Notice = i.NewNotice;

                c.Send(PacketManager.SendGuildChangeNoticeError(GuildNoticeError.Success));
                SendToGuild(c.MyCharacter.Guild, PacketManager.SendGuildNotice(c.MyCharacter.Guild));
                // send in guild
                guildManager.UpdateGuild(c.MyCharacter.Guild);
            }
            else
            {
                // send NO BOSS ARE
                c.Send(PacketManager.SendGuildChangeNoticeError(GuildNoticeError.OnlyGuildMasterCanUse));
            }
        }

        void c_ClientRequestJoinGuildInfo(object sender, ClientRequestJoinGuildEventArgs e)
        {
            Client c = (Client)sender;
            RequestJoinGuildInfo i = e.Info;
            Client asker = null;
            Client target = null;
            MapEngine mapEngine = GetMapEngine(c.MyCharacter.MapId);
            Client[] clients = GetClientsForChars(mapEngine.GetCharactersInRange(c.MyCharacter.Position, 150).ToArray());

            target = GetClientByCharacterID(i.TargetID);
            asker = GetClientByCharacterID(i.AskerID);

            if (asker.MyCharacter.Guild != null)
            {
                i.Answer = GuildJoinAnswer.AlreadyInGuild;
                asker.Send(PacketManager.SendGuildJoinAnswer(i));
                return;
            }

            if (target.MyCharacter.Guild == null)
            {
                i.Answer = GuildJoinAnswer.NotInGuild;
                asker.Send(PacketManager.SendGuildJoinAnswer(i));
                return;
            }

            if (i.Answer == GuildJoinAnswer.Join)
            {
                if (target.MyCharacter.Guild != null)
                {
                    GuildMember member = target.MyCharacter.Guild.Members.Single(x => x.Character.CharacterId == target.MyCharacter.CharacterId);
                    if (member.Rank == GuildOrder.Second_Master || member.Rank == GuildOrder.Guild_Master)
                        target.Send(PacketManager.SendGuildJoinAnswer(i));
                    else
                    {
                        i.Answer = GuildJoinAnswer.Decline;
                        asker.Send(PacketManager.SendGuildJoinAnswer(i));
                        return;
                    }
                }
            }

            if (i.Answer == GuildJoinAnswer.Decline)
            {
                asker.Send(PacketManager.SendGuildJoinAnswer(i));
            }

            if (i.Answer == GuildJoinAnswer.Accept)
            {
                GuildMember newMember = new GuildMember("", GuildOrder.Member, asker.MyCharacter);

                target.MyCharacter.Guild.Members.Add(newMember);
                asker.MyCharacter.Guild = target.MyCharacter.Guild;

                SendToGuild(target.MyCharacter.Guild, PacketManager.SendGuildJoin(asker.MyCharacter));
                SendToGuild(target.MyCharacter.Guild, PacketManager.SendGuildStats(target.MyCharacter.Guild));
                SendToGuild(target.MyCharacter.Guild, PacketManager.SendGuildMember(target.MyCharacter.Guild, newMember));
                /*
                target.Send(PacketManager.SendGuildJoin(asker.MyCharacter));
                target.Send(PacketManager.SendGuildStats(target.MyCharacter.Guild));
                target.Send(PacketManager.SendGuildMember(target.MyCharacter.Guild, newMember));
                */
                asker.Send(PacketManager.SendGuildJoin(asker.MyCharacter));

                guildManager.InsertGuildMember(newMember, asker.MyCharacter.Guild);

                foreach (GuildMember m in c.MyCharacter.Guild.Members)
                {
                    if (m.Character.CharacterId != target.MyCharacter.CharacterId || m.Character.CharacterId != asker.MyCharacter.CharacterId)
                    {
                        Client member = GetClientByCharacterID(m.Character.CharacterId);
                        if (member != null)
                        {
                            member.MyCharacter.Guild = target.MyCharacter.Guild;
                        }
                    }
                }
                // update guild memberlist for each members
            }
        }

        void c_ClientRequestCloseGuildInfo(object sender, ClientRequestCloseGuildEventArgs e)
        {
            Client c = (Client)sender;
            Guild g = c.MyCharacter.Guild;

            if (g != null && g.Leader.Character.CharacterId == c.MyCharacter.CharacterId)
            {
                guildManager.DeleteGuild(g.ID);
                c.Send(PacketManager.SendCloseGuild()); // gotta send to all members in guild who online.
            }
        }


        void c_ClientRequestLeaveGuildInfo(object sender, ClientRequestLeaveGuildEventArgs e)
        {
            Client c = (Client)sender;
            RequestLeaveGuildInfo i = e.Info;
            Guild g = c.MyCharacter.Guild;
            GuildMember member = g.Members.Single(x => x.Character.CharacterId == i.PlayerID);
            if (member != null)
            {
                if (member.Rank != GuildOrder.Guild_Master)
                {
                    SendToGuild(g, PacketManager.SendLeaveGuild(i.PlayerID, g));
                    SendToGuild(g, PacketManager.SendGuildStats(g));

                    g.Members.Remove(member);
                    guildManager.DeleteGuildMember(member.Character.CharacterId);
                    member.Character.Guild = null;

                    foreach (GuildMember m in c.MyCharacter.Guild.Members)
                    {
                        if (m.Character.CharacterId != i.PlayerID)
                        {
                            Client memberClient = GetClientByCharacterID(m.Character.CharacterId);
                            if (memberClient != null)
                            {
                                memberClient.MyCharacter.Guild = g;
                            }
                        }
                    }
                    // send to all members whos online new guild stats
                }
                else
                {
                    // cannot kick master..
                }
            }
        }

        void c_ClientGuildChatInfo(object sender, ClientGuildChatEventArgs e)
        {
            Client c = (Client)sender;
            GuildChatInfo i = e.Info;

            SendToGuild(c.MyCharacter.Guild, PacketManager.SendGuildChat(c.MyCharacter, i.Message));
        }

        void c_ClientUseRebirthPillInfo(object sender, ClientUseRebirthPillEventArgs e)
        {
            Client c = (Client)sender;
            UseRebirthPillInfo i = e.Info;
            BaseItem item = null;
            MapEngine mapEngine = GetMapEngine(c.MyCharacter.MapId);
            Client[] clients = GetClientsForChars(mapEngine.GetCharactersInRange(c.MyCharacter.Position, 150).ToArray());

            try
            {
                item = c.MyCharacter.Bags[i.Bag - 1].Items.Single(x => x.ItemID == i.ItemID);

                if (item is RebirthPill)
                {
                    RebirthPill pill = item as RebirthPill;

                    if (c.MyCharacter.Level < pill.RequiredLevel)
                    {
                        c.Send(PacketManager.SendRebirth(RebirthError.LowLevel, null));
                        return;
                    }
                    if (c.MyCharacter.Rebirth != pill.RequiredRebirth)
                    {
                        c.Send(PacketManager.SendRebirth(RebirthError.LowRebirth, null));
                        // send too low rebirths
                        return;
                    }

                    // remove pill
                    byte[] SendRemoveItem = PacketManager.SendRemoveItem(pill.Bag, pill.Slot);
                    c.Send(SendRemoveItem);

                    itemDataManager.DeleteItem(pill.ItemID);
                    c.MyCharacter.Bags[pill.Bag - 1].RemoveItem(pill);

                    c.MyCharacter.Rebirth++; 
                    c.MyCharacter.StatPoint += pill.IncreaseSp; // need fiqure this one out xd
                    // increase max fe to corresponding value, once fe added.
                    // give the rebirth quest
                    characterManager.UpdateCharacter(c.MyCharacter);

                    byte[] SendStats = PacketManager.SendStats(c.MyCharacter);
                    c.Send(SendStats);

                    byte[] SendRebirth = PacketManager.SendRebirth(RebirthError.Success, c.MyCharacter);
                    SendSpecific(clients, SendRebirth);
                }
                else
                {
                    // send error
                }
            }
            catch
            {
                // send error no item?
            }
        }


        public void SendChatToAll(string message)
        {
            Character fakeChar = new Character { CharacterId = -1, Name = "CONSOLE" };

            ChatMessageInfo msg = new ChatMessageInfo { Message = message, Type = ChatType.Pipe3, TargetName = "" };
            byte[] packet = PacketManager.SendChatMessage(fakeChar, msg);
            SendToAll(packet);
        }

        public void SendCommandHelp(string message, Client c)
        {
            Character fakeChar = new Character { CharacterId = -1, Name = "CONSOLE" };

            ChatMessageInfo msg = new ChatMessageInfo { Message = message, Type = ChatType.Guild, TargetName = "" };
            byte[] packet = PacketManager.SendChatMessage(fakeChar, msg);
            c.Send(packet);
        }

        public void SendToGuild(Guild g, byte[] p)
        {
            foreach (GuildMember m in g.Members)
            {
                Client member = GetClientByCharacterID(m.Character.CharacterId);

                if (member != null)
                {
                    member.Send(p);
                }
            }
        }

        public void UpdateGuildMemberStatus(Guild g)
        {
            foreach (GuildMember m in g.Members)
            {
                Client member = GetClientByCharacterID(m.Character.CharacterId);

                if (member != null)
                {
                    m.Online = true;
                }
                else
                    m.Online = false;
            }
        }

        public void KickPlayer(string name)
        {
            try
            {
                Client c = clients.Where(x => x.MyCharacter.Name.ToLower() == name.ToLower()).First();
                c.Dispose();
            }
            catch
            {
            }
        }

        private MapEngine GetMapEngine(int mapId)
        {
            return mapEngines.FirstOrDefault(x => x.Map.MapID == mapId);
        }
        #endregion
    }
}
