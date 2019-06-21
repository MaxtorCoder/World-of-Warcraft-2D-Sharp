using Framework.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Utils.Behaviour
{
    /// <summary>
    /// A base for all behaviour types.
    /// </summary>
    public abstract class IBehaviour
    {
        public enum Behaviours
        {
            Patrol,
            Attack,
            Defend,
            Random
        }

        public int ID;
        public Behaviours BehaviourType;
        public string Name;

        public IBehaviour(Behaviours _behaviourType)
        {
            BehaviourType = _behaviourType;
        }

        public abstract void Start();
        public abstract void Update(ServerCreature creature);

        public IBehaviour Clone()
        {
            return (IBehaviour)MemberwiseClone();
        }
    }
}
