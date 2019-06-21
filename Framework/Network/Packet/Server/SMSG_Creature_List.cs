using Framework.Entity;
using Framework.Network.Packet.OpCodes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Network.Packet.Server
{
    /// <summary>
    /// Sends the active list of creatures upon a player spawn.
    /// </summary>
    public class SMSG_Creature_List : IPacket
    {
        public List<ClientCreature> Creatures;

        public SMSG_Creature_List() : base((byte)ServerOpcodes.Opcodes.SMSG_CREATURE_LIST) { }

        public override byte[] Serialize()
        {
            var formatter = new BinaryFormatter();
            using (var memStr = new MemoryStream())
            {
                using (var writer = new BinaryWriter(memStr))
                {
                    writer.Write(_opcode);
                    formatter.Serialize(memStr, Creatures);
                }
                return memStr.ToArray();
            }
        }

        public override IPacket Deserialize(byte[] data)
        {
            var obj = new SMSG_Creature_List();
            var formatter = new BinaryFormatter();
            using (var memStr = new MemoryStream(data))
            {
                using (var reader = new BinaryReader(memStr))
                {
                    reader.ReadByte();
                    obj.Creatures = (List<ClientCreature>)formatter.Deserialize(memStr);
                }
            }
            return obj;
        }
    }
}
