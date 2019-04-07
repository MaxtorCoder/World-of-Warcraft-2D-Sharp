using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

namespace Framework.Entity
{
    /// <summary>
    /// Manages specific map data for the server.
    /// </summary>
    public struct Map
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public TmxObjectGroup Spawns { get; set; }
    }
}
