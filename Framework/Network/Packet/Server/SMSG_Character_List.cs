using Framework.Entity;
using Framework.Network.Packet.Opcodes;
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
    /// The server character-response packet.
    /// Serializes the list and sends as a whole to the client.
    /// </summary>
    public class SMSG_Character_List : IPacket
    {
        public List<RealmCharacter> Characters { get; set; }

        public SMSG_Character_List() : base((byte)ServerOpcodes.Opcodes.SMSG_CHARACTER_LIST) { }

        public override byte[] Serialize()
        {
            var formatter = new BinaryFormatter();
            using (var memStr = new MemoryStream())
            {
                using (var writer = new BinaryWriter(memStr))
                {
                    writer.Write(_opcode);
                    formatter.Serialize(memStr, Characters);
                }
                return memStr.ToArray();
            }
        }

        public override IPacket Deserialize(byte[] data)
        {
            var newData = data.Skip(1).ToArray(); // Remove the opcode.
            var obj = new SMSG_Character_List();
            var formatter = new BinaryFormatter();
            var stream = new MemoryStream(newData);
            obj.Characters = (List<RealmCharacter>)formatter.Deserialize(stream);
            return obj;
        }
    }
}
