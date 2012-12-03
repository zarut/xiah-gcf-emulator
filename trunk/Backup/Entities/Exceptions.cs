using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    public class RequiredClassException : Exception
    {
        public RequiredClassException(string message)
            : base(message)
        {

        }
    }

    public class BagIsFullException : Exception
    {
        public BagIsFullException(string message)
            : base(message)
        {
        }
    }

    public class InvalidItemSlotException : Exception
    {
        public InvalidItemSlotException(string message)
            : base(message)
        {
        }
    }
}
