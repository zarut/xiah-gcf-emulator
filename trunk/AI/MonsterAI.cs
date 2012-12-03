using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Entities;
using AI.Desire;
using ServerEngine;
using ServerEngine.PacketEngine;

namespace AI
{
    public class MonsterAI : BaseAI
    {
        private ChannelServer Server;
        private Random Random;

        public MonsterAI(Monster monster, ChannelServer server)
            : base(monster, 1000)
        {
            this.Server = server;
            AITimer.Elapsed += new System.Timers.ElapsedEventHandler(AITimer_Elapsed);
            AITimer.Enabled = false;
            Random = new Random();
        }

        void AITimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (CurrentDesire.GetType() == typeof(Desire_Idle))
            {
                if (Server.GetNearClientsByMonster(Monster).Length > 0)
                {
                    AttackInfo atk = Server.GetNearClientsByMonster(Monster)[0].MyCharacter.OnAttack(Monster);

                    Server.SendInRange(Monster.X, Monster.Y, Monster.MapID, PacketManager.SendAttack1(atk));
                }
            }
        }
    }
}
