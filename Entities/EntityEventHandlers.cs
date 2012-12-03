using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    public class HazeDamageCastedEventArgs : EventArgs
    {
        private BaseEntity[] targets;
        private Character caster;

        public HazeDamageCastedEventArgs(BaseEntity[] targets, Character caster)
        {
            this.targets = targets;
            this.caster = caster;
        }

        public BaseEntity[] HitTargets { get { return targets; } }
        public Character Caster { get { return caster; } }
    }
}
