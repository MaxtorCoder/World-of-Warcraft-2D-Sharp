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
    /// The server character create packet.
    /// </summary>
    public class SMSG_Character_Create : IPacket
    {
        public byte Magic { get; set; }

        public SMSG_Character_Create() : base((byte)ServerOpcodes.SMSG_CHARACTER_CREATE) { }

        public override byte[] Serialize()
        {
            using (var memStr = new MemoryStream())
            {
                using (var writer = new BinaryWriter(memStr))
                {
                    writer.Write(_opcode);
                    writer.Write(Magic);
                }
                return memStr.ToArray();
            }
        }

        public override IPacket Deserialize(byte[] data)
        {
            var obj = new SMSG_Character_Create();
            using (var memStr = new MemoryStream(data))
            {
                using (var reader = new BinaryReader(memStr))
                {
                    reader.ReadByte();
                    obj.Magic = reader.ReadByte();
                }
            }
            return obj;
        }
    }
}
