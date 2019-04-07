using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Entity
{
    /// <summary>
    /// Represents positional-data in the game world.
    /// </summary>
    [Serializable]
    public class Vector
    {
        public enum MoveDirection
        {
            North = 0,
            South = 1,
            East = 2,
            West = 3,
            North_East = 4,
            North_West = 5,
            South_East = 6,
            South_West = 7
        }

        public int MapID { get; set; }
        public int CellID { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public MoveDirection Direction { get; set; }
    }
}
