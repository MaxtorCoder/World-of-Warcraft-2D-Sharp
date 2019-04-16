using Framework.Network.Packet.OpCodes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Network.Packet.Server
{
    /// <summary>
    /// The remove-player packet.
    /// </summary>
    public class SMSG_Connection_Remove : IPacket
    {
        public string Name { get; set; }

        public SMSG_Connection_Remove() : base((byte)ServerOpcodes.Opcodes.SMSG_CONNECTION_REMOVE) { }

        public override byte[] Serialize()
        {
            using (var memStr = new MemoryStream())
            {
                using (var writer = new BinaryWriter(memStr))
                {
                    writer.Write(_opcode);
                    writer.Write(Name);
                }
                return memStr.ToArray();
            }
        }

        public override IPacket Deserialize(byte[] data)
        {
            var obj = new SMSG_Connection_Remove();
            using (var memStr = new MemoryStream(data))
            {
                using (var reader = new BinaryReader(memStr))
                {
                    reader.ReadByte();
                    obj.Name = reader.ReadString();
                }
            }
            return obj;
        }
    }
}
