using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Utils.Behaviour
{
    /// <summary>
    /// The base for patrol behaviours.
    /// </summary>
    public abstract class IPatrol : IBehaviour
    {
        public enum PatrolTypes
        {
            Random,
            Waypoint
        }

        public IPatrol() : base(Behaviours.Patrol) { }
    }
}
