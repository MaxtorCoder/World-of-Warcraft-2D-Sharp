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
        private static Dictionary<int, Map> _maps = new Dictionary<int, Map>();
        private const string _mapDirectory = "Data/Maps";

        public static void Initialize()
        {
            foreach (var mapFile in Directory.EnumerateFiles(_mapDirectory, "*.tmx"))
            {
                var tmx = new TmxMap(mapFile);
                _maps.Add(int.Parse(tmx.Properties["map_id"]), new Map()
                {
                    Name = tmx.Properties["map_name"]
                });
            }
        }

        public static Map GetMapByID(int mapId)
        {
            foreach (var map in _maps)
            {
                if (map.Key == mapId)
                    return map.Value;
            }
            return null;
        }

        public static int GetCount()
        {
            return _maps.Count;
        }
    }
}
