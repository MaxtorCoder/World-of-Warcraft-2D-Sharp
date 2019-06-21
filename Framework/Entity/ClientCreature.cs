using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Entity
{
    [Serializable]
    public class ClientCreature
    {
        public string GUID { get; set; }
        public int ID { get; set; }
        public int ModelID { get; set; }
        public string Name { get; set; }
        public string SubName { get; set; }
        public Stats Stats { get; set; }
        public Vector Vector { get; set; }
    }
}
