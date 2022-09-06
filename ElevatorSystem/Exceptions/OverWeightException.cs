using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSystem.Exceptions
{
    public class OverWeightException : Exception
    {
        public OverWeightException()
        {
        }

        public OverWeightException(string message)
            : base(message)
        {
        }

        public OverWeightException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
