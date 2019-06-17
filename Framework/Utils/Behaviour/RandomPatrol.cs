using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Utils.Behaviour
{
    /// <summary>
    /// The random-movement/patrol behaviour.
    /// </summary>
    public class RandomPatrol : IPatrol
    {
        public int WaitTimer, MoveTimer;
    }
}
