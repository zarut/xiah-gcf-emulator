using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI.Desire
{
    public class Desire_Idle : BaseDesire
    {
        public Desire_Idle() { }

        public override void ProcessDesire(object[] Params)
        {
            // fighting = false
            AI.CurrentDesire = this;
            AI.CurrentDesireParams = Params;
        }
    }
}
