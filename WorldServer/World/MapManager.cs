using Framework.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

namespace WorldServer.World
{
    /// <summary>
    /// Manages general map data.
    /// </summary>
    public class MapManager
    {
        private static List<Map> _maps = new List<Map>();
        private const string _mapDirectory = "Data/Maps";

        public static void Initialize()
        {
            foreach (var mapFile in Directory.EnumerateFiles(_mapDirectory, "*.tmx"))
            {
                var tmx = new TmxMap(mapFile);
                _maps.Add(new Map()
                {
                    ID = int.Parse(tmx.Properties["MapID"]),
                    Name = tmx.Properties["MapName"],
                    Spawns = tmx.ObjectGroups["SpawnObjects"]
                });
            }
        }

        public static Map GetMapByID(int mapId)
        {
            foreach (var map in _maps)
            {
                if (map.ID == mapId)
                    return map;
            }
            return new Map() { ID = -1 };
        }

        public static int GetCount()
        {
            return _maps.Count;
        }
    }
}
