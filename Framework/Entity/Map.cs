using Framework.Network.Connection;
using Framework.Network.Packet.Server;
using Framework.Utils;
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
        public List<WorldCreature> Creatures = new List<WorldCreature>();

        public void Initialize()
        {
            // TODO: This should be done below when spawning clusters of a given type.
            Creatures.AddRange(CreatureManager.GetListOfID(1));

            // Spawn mobs
            var rand = new Random();
            foreach (var tmxObject in Spawns.Objects)
            {
                if (tmxObject.Name.ToLower() == "mobs")
                {
                    var x = tmxObject.X;
                    var y = tmxObject.Y;
                    var width = tmxObject.Width;
                    var height = tmxObject.Height;

                    for (int i = 0; i < Creatures.Count; i++)
                    {
                        var creature = Creatures[i];

                        var randX = rand.Next((int)x, (int)(x + width));
                        var randY = rand.Next((int)y, (int)(y + height));

                        creature.GUID = Guid.NewGuid().ToString();
                        creature.Vector = new Vector()
                        {
                            Direction = Vector.MoveDirection.South,
                            X = randX,
                            Y = randY
                        };
                    }
                }
            }
        }

        public void Update()
        {
        }
    }
}
