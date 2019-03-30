using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Entity
{
    /// <summary>
    /// Realm-character data.
    /// </summary>
    [Serializable]
    public struct RealmCharacter
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public Class Class { get; set; }
        public string Location { get; set; }
        public Race Race { get; set; }
    }
}
