using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Entity
{
    /// <summary>
    /// The current character in the world.
    /// </summary>
    [Serializable]
    public class WorldCharacter
    {
        public string GUID { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public Class Class { get; set; }
        public Race Race { get; set; }
        public Vector Vector { get; set; }
    }
}
