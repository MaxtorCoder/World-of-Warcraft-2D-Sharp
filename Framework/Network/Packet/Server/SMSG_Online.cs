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
    /// An online-list packet.
    /// </summary>
    public class SMSG_Online : IPacket
    {
        public List<WorldCharacter> Characters;

        public SMSG_Online() : base((byte)ServerOpcodes.Opcodes.SMSG_ONLINE) { }

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
            var obj = new SMSG_Online();
            var formatter = new BinaryFormatter();
            using (var memStr = new MemoryStream(data))
            {
                using (var reader = new BinaryReader(memStr))
                {
                    reader.ReadByte();
                    obj.Characters = (List<WorldCharacter>)formatter.Deserialize(memStr);
                }
            }
            return obj;
        }
    }
}
