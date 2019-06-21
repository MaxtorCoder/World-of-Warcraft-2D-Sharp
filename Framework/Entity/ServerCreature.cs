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
    public class ServerCreature
    {
        public string GUID { get; set; }
        public int ID { get; set; }
        public int ModelID { get; set; }
        public string Name { get; set; }
        public string SubName { get; set; }
        public Stats Stats { get; set; }
        public Vector Vector { get; set; }

        public int[] BehaviourIDs;
        public List<IBehaviour> Behaviours;

        public ServerCreature Clone()
        {
            return (ServerCreature)MemberwiseClone();
        }
    }
}
