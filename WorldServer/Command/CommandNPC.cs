using Framework.Entity;
using Framework.Network;
using Framework.Network.Connection;
using Framework.Network.Packet.Server;
using Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldServer.World;

namespace WorldServer.Command
{
    /// <summary>
    /// Commands for handling NPCs in the world.
    /// </summary>
    public class CommandNPC : AbstractCommand
    {
        public CommandNPC() : base("npc", AccountSecurity.Gamemaster)
        {
            _subCommands.Add("add", GetType().GetMethod("HandleAdd", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance));
        }

        private void HandleAdd(IConnection connection, string[] args)
        {
            var worldConnection = (WorldConnection)connection;
            int id = int.Parse(args[2]);

            var creature = CreatureManager.GetCreatureWithEntryID(id);
            if (creature != null)
            {
                /** Set creature's info. **/
                creature.GUID = Guid.NewGuid().ToString();
                creature.Vector = new Vector()
                {
                    X = worldConnection.Account.Character.Vector.X,
                    Y = worldConnection.Account.Character.Vector.Y
                };
                WorldManager.GetMapByID(worldConnection.Account.Character.Vector.MapID).Creatures.Add(creature);

                connection.Send(new SMSG_Creature()
                {
                    Creature = creature,
                    State = SMSG_Creature.CreatureState.Add
                });
            }
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }
}
