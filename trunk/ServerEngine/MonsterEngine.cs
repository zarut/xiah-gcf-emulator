using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using System.Threading;

namespace ServerEngine
{
    class MonsterEngine
    {
        //to do - add ai events
        public event EventHandler<MonsterMoveInfoEventArgs> MonsterMove;
        public event EventHandler<MonsterAttackInfoEventArgs> MonsterAttack;

        private Thread activeThread;

        public MonsterEngine(List<Monster> monsters, List<BaseEntity> surroundings)
        {
            List<MonsterAI> monsterAIs = monsters.Select(x => new MonsterAI(x, surroundings)).ToList();
            this.ActiveMonsters = monsterAIs;
            RegisterAIEvents();
            activeThread = new Thread(AIOperation);
            activeThread.IsBackground = true;
        }

        private void RegisterAIEvents()
        {
            for (int i = 0; i < ActiveMonsters.Count; i++)
            {
                ActiveMonsters[i].MonsterMove += new EventHandler<MonsterMoveInfoEventArgs>(MonsterEngine_MonsterMove);
                ActiveMonsters[i].MonsterAttack += new EventHandler<MonsterAttackInfoEventArgs>(MonsterEngine_MonsterAttack);
            }
        }

        void MonsterEngine_MonsterAttack(object sender, MonsterAttackInfoEventArgs e)
        {
            if (MonsterAttack != null)
                MonsterAttack(sender, e);
        }

        void MonsterEngine_MonsterMove(object sender, MonsterMoveInfoEventArgs e)
        {
            if (MonsterMove != null)
                MonsterMove(sender, e);
        }


        public List<MonsterAI> ActiveMonsters
        {
            get;
            private set;
        }

        private void AttackMonster(int monsterID, BaseEntity entity)
        {
            
        }

        private void SkillMonster(int monsterID, BaseEntity entity)
        {
        }

        public void StartEngine()
        {
            activeThread.Start();
        }

        public void StopEngine()
        {
            for (int i = 0; i < ActiveMonsters.Count; i++)
            {
                MonsterAI monster = ActiveMonsters[i];
                monster.StopAI();
            }

            activeThread.Abort();
        }

        private void AIOperation()
        {
            for (int i = 0; i < ActiveMonsters.Count; i++)
            {
                ActiveMonsters[i].StartAI();
                //to do - register ai events
            }

            while (true)
            {
                Thread.Sleep(100);
            }
        }
    }
}
