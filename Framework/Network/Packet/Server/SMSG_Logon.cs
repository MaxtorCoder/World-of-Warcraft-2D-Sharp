using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Network.Packet.Server
{
    /// <summary>
    /// The server logon packet.
    /// </summary>
    public class SMSG_Logon : IPacket
    {
        public byte Magic { get; set; }

        public SMSG_Logon() : base(OpCodes.SMSG_LOGON) { }

        public override byte[] SerializePacket()
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

        public override IPacket DeserializePacket(byte[] data)
        {
            var obj = new SMSG_Logon();
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
