using Framework.Entity;
using Framework.Network.Connection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;
using WorldServer.Network;

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

        public static void AddCharacterToMap(WorldConnection character)
        {
            foreach (var map in _maps)
            {
                if (map.ID == character.Account.Character.Vector.MapID)
                    map.Characters.Add(character);
            }
        }

        public static void RemoveCharacterFromMap(WorldConnection character)
        {
            foreach (var map in _maps)
            {
                if (map.ID == character.Account.Character.Vector.MapID)
                    map.Characters.Remove(character);
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
