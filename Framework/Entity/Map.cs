using Framework.Network.Connection;
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
    public class Map
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public TmxObjectGroup Spawns { get; set; }
        public List<IConnection> Characters = new List<IConnection>();
    }
}
