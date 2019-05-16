using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Entity
{
    /// <summary>
    /// Holds a character's stat-data.
    /// </summary>
    [Serializable]
    public class Stats
    {
        public int Level { get; set; }
        public float CurrentHP { get; set; } = 68f;
        public float MaxHP { get; set; } = 68f;
        public float WalkSpeed { get; set; } = 8.5f;
    }
}
