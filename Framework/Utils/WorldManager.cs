using Framework.Entity;
using Framework.Network.Connection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

namespace Framework.Utils
{
    /// <summary>
    /// Manages general map data.
    /// </summary>
    public class WorldManager
    {
        private static List<Map> _maps = new List<Map>();
        private const string _mapDirectory = "Data/Maps";

        public static void Initialize()
        {
            foreach (var mapFile in Directory.EnumerateFiles(_mapDirectory, "*.tmx"))
            {
                var tmx = new TmxMap(mapFile);
                var map = new Map()
                {
                    ID = int.Parse(tmx.Properties["MapID"]),
                    Name = tmx.Properties["MapName"],
                    Spawns = tmx.ObjectGroups["SpawnObjects"]
                };
                map.Initialize();

                _maps.Add(map);
            }
        }

        public static void AddCharacterToMap(WorldConnection connection)
        {
            foreach (var map in _maps)
            {
                if (map.ID == connection.Account.Character.Vector.MapID)
                    map.Characters.Add(connection);
            }
        }

        public static void RemoveCharacterFromMap(WorldConnection connection)
        {
            foreach (var map in _maps)
            {
                if (map.ID == connection.Account.Character.Vector.MapID)
                    map.Characters.Remove(connection);
            }
        }

        public static List<WorldConnection> GetAllPlayers()
        {
            List<IConnection> generics = new List<IConnection>();
            List<WorldConnection> worldConnections = new List<WorldConnection>();
            foreach (var map in _maps)
                generics.AddRange(map.Characters);

            if (generics != null)
            {
                foreach (var connection in generics)
                    worldConnections.Add((WorldConnection)connection);
                return worldConnections;
            }
            return null;
        }

        public static List<WorldConnection> GetPlayersWithinMap(int id)
        {
            List<IConnection> generics = null;
            List<WorldConnection> worldConnections = new List<WorldConnection>();
            foreach (var map in _maps)
            {
                if (map.ID == id)
                    generics = map.Characters;
            }

            if (generics != null)
            {
                foreach (var connection in generics)
                    worldConnections.Add((WorldConnection)connection);
                return worldConnections;
            }
            return null;
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
