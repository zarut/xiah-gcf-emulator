using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI.Desire
{
    public abstract class BaseDesire
    {
        public BaseAI AI { get; set; }
        public abstract void ProcessDesire(object[] Params);
    }
}
