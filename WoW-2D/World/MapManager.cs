using Microsoft.Xna.Framework.Content;
using MonoGame.Extended.Tiled;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoW_2D.World.GameObject;

namespace WoW_2D.World
{
    /// <summary>
    /// Handles loading maps, unloading, etc,.
    /// </summary>
    public class MapManager
    {
        private static IReadOnlyDictionary<int, string> Maps = new Dictionary<int, string>()
        {
            { 1, "Maps/map1" },
        };

        public static void LoadMap(ContentManager content)
        {
            string mapDirectory = string.Empty;

            foreach (var entry in Maps)
            {
                if (entry.Key == WorldofWarcraft.Character.Vector.MapID)
                    mapDirectory = entry.Value;
            }

            if (mapDirectory != string.Empty)
            {
                WorldofWarcraft.Map = new ClientMap()
                {
                    ZoneMap = content.Load<TiledMap>(mapDirectory),
                    Player = new Player()
                };
            }
        }
    }
}
