using Framework.Utils.Behaviour;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Entity
{
    /// <summary>
    /// Holds data about a creature in the world; Strictly used for data-transmission.
    /// </summary>
    [Serializable]
    public class WorldCreature
    {
        public string GUID { get; set; }
        public int ID { get; set; }
        public int ModelID { get; set; }
        public string Name { get; set; }
        public string SubName { get; set; }
        public Stats Stats { get; set; }
        public Vector Vector { get; set; }

        [NonSerialized]
        public bool IsMoving;

        [NonSerialized]
        public int[] BehaviourIDs;

        [NonSerialized]
        public List<IBehaviour> Behaviours;

        public WorldCreature Clone()
        {
            return (WorldCreature)MemberwiseClone();
        }
    }
}
