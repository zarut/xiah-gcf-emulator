using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using System.Timers;

namespace ServerEngine
{
    class MonsterAI
    {
        public event EventHandler<MonsterMoveInfoEventArgs> MonsterMove;
        public event EventHandler<MonsterAttackInfoEventArgs> MonsterAttack;

        private Timer aiTimer;
        private List<BaseEntity> surroundings;
        private MonsterAIState state;

        public MonsterAI(Monster monster, List<BaseEntity> surroundings)
        {
            aiTimer = new Timer();
            aiTimer.Elapsed += new ElapsedEventHandler(aiTimer_Elapsed);
            aiTimer.Disposed += new EventHandler(aiTimer_Disposed);
            aiTimer.Enabled = true;
            aiTimer.AutoReset = true;
            aiTimer.Interval = 1000; //1000 for movemetn update, or 100 

            this.surroundings = surroundings;

            this.Monster = monster;
            this.state = MonsterAIState.Wander;
        }

        void aiTimer_Disposed(object sender, EventArgs e)
        {
           
        }

        void aiTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (Monster.Alive)
            {
                switch (state)
                {
                    case MonsterAIState.Wander:
                        if (!Monster.IsMoving && DateTime.Now >= Monster.LastMoveTime.AddSeconds(3))
                            DoWandering();
                        break;

                    case MonsterAIState.Chase:
                        if (!Monster.IsMoving && Monster.Target != null)
                            ChaseEntity(Monster.Target);
                        else
                            state = MonsterAIState.Wander;
                        break;

                    case MonsterAIState.Attack:
                        if (Monster.Target != null && DateTime.Now >= Monster.LastAttack.AddSeconds(2))
                            AttackEntity(Monster.Target);
                        else
                            state = MonsterAIState.Wander;
                        break;

                    default:
                        state = MonsterAIState.Wander;
                        break;
                }

                if (Monster.IsMoving)
                    UpdateMovement();

                if (HasTarget())
                {
                    if (state != MonsterAIState.Attack)
                        state = MonsterAIState.Chase;
                }
                else
                    state = MonsterAIState.Wander;
            }
            else
            {
                TrySpawn();
            }

            //// add logics, 3sec no mobs -> idle, if player around rape him etc
            //if (Monster.Alive)
            //{
            //    // add check if monster is too far away from spawning point, then walk back there
            //    if (!Monster.IsMoving)
            //    {
            //        if (Monster.Target != null)
            //        {
            //            UpdateTarget();

            //            if (TargetOnAttackRange())
            //                Monster.Attacking = true;
            //            else
            //                Monster.Attacking = false;
            //        }

            //        if (!Monster.Attacking && DateTime.Now >= Monster.LastMoveTime.AddSeconds(3)) // && 3 secs passed since last action
            //            DoWandering();
            //        else if (Monster.Attacking && DateTime.Now >= Monster.LastAttack.AddSeconds(2))
            //        {
            //            // do attacking stuff etc
            //            MonsterAttackInfo info = GetAttackInfo(false);
            //            if (MonsterAttack != null)
            //                MonsterAttack(this, new MonsterAttackInfoEventArgs(info));
            //        }
            //    }
            //    else if (Monster.IsMoving)
            //    {
            //        UpdateMovement();
            //    }
            //}
            //else
            //{
            //    TrySpawn();
            //}
        }

        private void TrySpawn()
        {
            int respawnTime = 15;
            if (Monster.LeaderID == 1)
                respawnTime *= 2;
            if (Monster.MonsterType == 112 || Monster.MonsterType == 187 || Monster.MonsterType == 188) // king, phx, gb
                respawnTime = 3600;
            if (Monster.MonsterType == 113) // emp
                respawnTime = 7200;

            if (DateTime.Now >= Monster.DeathTime.AddSeconds(respawnTime))
            {
                Monster.Position.X = Monster.SpawnX;
                Monster.Position.Y = Monster.SpawnY;
                Monster.CurHealth = Monster.MaxHealth;
                Monster.Target = null;
                Monster.Alive = true;
                Monster.Attacking = false;
                Monster.IsMoving = false;
                state = MonsterAIState.Wander;
            }
        }

        private void UpdateMovement()
        {
            if (Monster.Position != Monster.Destination)
            {
                byte state = Monster.Movement.Update();
                switch (state)
                {
                    case 0:
                        MonsterMoveInfo start = GetMovementInfo(MonsterMoveStatus.StartMoving);
                        if (MonsterMove != null)
                            MonsterMove(this, new MonsterMoveInfoEventArgs(start));
                        break;

                    case 1:
                        //if (state == MonsterAIState.Chase)
                        //{
                        //    if (Monster.Target.Position != Monster.Destination)
                        //    {
                        //        StopMoving();
                        //        MoveTo(Monster.Target.Position);
                        //    }
                        //}

                        MonsterMoveInfo keep = GetMovementInfo(MonsterMoveStatus.KeepMoving);
                        if (MonsterMove != null)
                            MonsterMove(this, new MonsterMoveInfoEventArgs(keep));
                        break;

                    case 2:
                        Monster.IsMoving = false;

                        MonsterMoveInfo stop = GetMovementInfo(MonsterMoveStatus.StopMoving);
                        if (MonsterMove != null)
                            MonsterMove(this, new MonsterMoveInfoEventArgs(stop));

                        Monster.LastMoveTime = DateTime.Now;
                        break;
                }

                //if (Monster.Movement.Update())
                //{
                //    Monster.IsMoving = false;

                //    MonsterMoveInfo stop = GetMovementInfo(MonsterMoveStatus.StopMoving);
                //    if (MonsterMove != null)
                //        MonsterMove(this, new MonsterMoveInfoEventArgs(stop));

                //    Monster.LastMoveTime = DateTime.Now;
                //}
                //else
                //{
                //    //if (state == MonsterAIState.Chase)
                //    //{
                //    //    if (Monster.Target.Position != Monster.Destination)
                //    //    {
                //    //        StopMoving();
                //    //        MoveTo(Monster.Target.Position);
                //    //    }
                //    //}

                //    MonsterMoveInfo keep = GetMovementInfo(MonsterMoveStatus.KeepMoving);
                //    if (MonsterMove != null)
                //        MonsterMove(this, new MonsterMoveInfoEventArgs(keep));
                //}
            }
            else
            {
                Monster.IsMoving = false;

                MonsterMoveInfo stop = GetMovementInfo(MonsterMoveStatus.StopMoving);
                if (MonsterMove != null)
                    MonsterMove(this, new MonsterMoveInfoEventArgs(stop));

                Monster.LastMoveTime = DateTime.Now;
            }
        }

        private void DoWandering()
        {
            if (HasTarget())
            {
                state = MonsterAIState.Chase;
                return;
            }
            Position destination;

            // last action time set here
            short toX = (short)XiahRandom.Next(Monster.SpawnX - (Monster.WanderRange), Monster.SpawnX + (Monster.WanderRange));
            short toY = (short)XiahRandom.Next(Monster.SpawnY - (Monster.WanderRange), Monster.SpawnY + (Monster.WanderRange));

            destination = new Position(toX, toY, 0);


            MoveTo(destination);
        }

        private void AttackEntity(BaseEntity ent)
        {
            UpdateTarget();

            if (Monster.Target != null)
            {
                if (!TargetOnAttackRange())
                    state = MonsterAIState.Chase;
                else
                {
                    Monster.Target = ent;
                    Monster.Attacking = true;

                    MonsterAttackInfo info = GetAttackInfo(false);
                    if (MonsterAttack != null)
                        MonsterAttack(this, new MonsterAttackInfoEventArgs(info));
                }
            }
        }

        private void ChaseEntity(BaseEntity ent)
        {
            UpdateTarget();

            if (Monster.Target != null)
            {
                if (TargetOnAttackRange())
                    state = MonsterAIState.Attack;
                else
                {
                    Position entPos = new Position(ent.Position.X, ent.Position.Y, 0);
                    MoveTo(entPos);
                }
            }
        }

        private void MoveTo(Position pos)
        {
            Monster.Destination = pos;

            Monster.Movement.MoveTo(Monster.Destination);

            Monster.IsMoving = true;
        }

        //private void StopMoving()
        //{
        //    Monster.Movement.Stop();
        //    Monster.IsMoving = false;

        //    MonsterMoveInfo stop = GetMovementInfo(MonsterMoveStatus.StopMoving);
        //    if (MonsterMove != null)
        //        MonsterMove(this, new MonsterMoveInfoEventArgs(stop));
        //}

        private double GetRotation(Position destination)
        {
            double rotation = Math.Atan2(destination.X - Monster.Position.X, destination.Y - Monster.Position.Y);
            rotation = Math.Acos(rotation) * (180 / Math.PI);

            return rotation;
        }

        private MonsterMoveInfo GetMovementInfo(MonsterMoveStatus type)
        {
            Monster.Direction = (short)GetRotation(Monster.Destination);
            MonsterMoveInfo i = new MonsterMoveInfo
            {
                CurrentPosition = Monster.Position,
                Destination = Monster.Destination,
                Rotation = Monster.Direction,
                Status = type
            };

            return i;
        }

        private MonsterAttackInfo GetAttackInfo(bool usingSkill)
        {
            MonsterAttackInfo i = new MonsterAttackInfo
            {
                Target = Monster.Target,
                UseSkill = usingSkill
            };

            return i;
        }

        private BaseEntity GetTarget()
        {
            BaseEntity target = null;
            if(surroundings.Count > 0)
            target = surroundings.FirstOrDefault(x => (Monster.GetDistance(Monster.Position.X, Monster.Position.Y, x.Position.X, x.Position.Y) <= (Monster.SightRange + 20)));
            return target;
        }

        private bool HasTarget()
        {
            BaseEntity target = GetTarget();
            if (target != null)
            {
                Monster.Target = target;
                return true;
            }

            return false;
        }

        private void UpdateTarget()
        {
            Character ch = null;
            Pet pet = null;

            if (Monster.Target is Character)
                ch = Monster.Target as Character;
            if (Monster.Target is Pet)
                pet = Monster.Target as Pet;

            int distance = Monster.GetDistance(Monster.SpawnX, Monster.SpawnY, Monster.Target.Position.X, Monster.Target.Position.Y);
            if (distance >= 150)
                ReturnToSpawnLocation();

            if (ch != null)
            {
                if (ch == null || !ch.Alive || !ch.Online)
                    ReturnToSpawnLocation();

                if (ch.MapId != Monster.MapID)
                    ReturnToSpawnLocation();
            }
            else if (pet != null)
            {
                if (pet == null || !pet.Alive)
                    ReturnToSpawnLocation();
                if (pet.MapID != Monster.MapID)
                    ReturnToSpawnLocation();
            }
            else
                ReturnToSpawnLocation();
        }

        private bool TargetOnAttackRange()
        {
            if (Monster.Target != null)
            {
                int distance = Monster.GetDistance(Monster.Position.X, Monster.Position.Y, Monster.Target.Position.X, Monster.Target.Position.Y);
                if (distance <= Monster.AttackRange + 5)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        private void ReturnToSpawnLocation()
        {
            Monster.Target = null;
            Monster.Destination = new Position(Monster.SpawnX, Monster.SpawnY, 0);
            Monster.Movement.MoveTo(Monster.Destination);
            Monster.IsMoving = true;
            Monster.Attacking = false;

            MonsterMoveInfo start = GetMovementInfo(MonsterMoveStatus.StartMoving);
            if (MonsterMove != null)
                MonsterMove(this, new MonsterMoveInfoEventArgs(start));
        }

        public Monster Monster
        {
            get;
            private set;
        }

        public void StartAI()
        {
            aiTimer.Start();
        }

        public void StopAI()
        {
            aiTimer.AutoReset = false;
            aiTimer.Enabled = false;
            aiTimer.Stop();
        }
    }
}
