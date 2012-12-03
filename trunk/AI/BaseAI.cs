using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading;
using AI.Desire;
using ServerEngine;
using Entities;

namespace AI
{
    public class BaseAI
    {
        public Monster Monster;
        public BaseDesire CurrentDesire;
        public object[] CurrentDesireParams;
        public System.Timers.Timer AITimer;
        public int IdleTimer;

        public BaseAI(Monster monster, int TimerInterval)
        {
            this.Monster = monster;
            this.AITimer = new System.Timers.Timer(TimerInterval);
            RaiseDesire(new Desire_Idle(), new object[] { });
        }

        public void RaiseDesire(BaseDesire desire, object[] Params)
        {
            if (desire != null && Params != null)
            {
                CurrentDesire = desire;
                CurrentDesireParams = Params;
                CurrentDesire.AI = this;
                CurrentDesire.ProcessDesire(Params);
            }
        }
    }
}
