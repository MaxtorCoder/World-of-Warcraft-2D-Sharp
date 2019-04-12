using Framework.Network.Packet.OpCodes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Network.Packet.Client
{
    /// <summary>
    /// The chat packet.
    /// </summary>
    public class CMSG_Chat : IPacket
    {
        public string Message { get; set; } = string.Empty;

        public CMSG_Chat() : base((byte)ClientOpcodes.CMSG_CHAT) { }

        public override byte[] Serialize()
        {
            using (var memStr = new MemoryStream())
            {
                using (var writer = new BinaryWriter(memStr))
                {
                    writer.Write(_opcode);
                    writer.Write(Message);
                }
                return memStr.ToArray();
            }
        }

        public override IPacket Deserialize(byte[] data)
        {
            var obj = new CMSG_Chat();
            using (var memStr = new MemoryStream(data))
            {
                using (var reader = new BinaryReader(memStr))
                {
                    reader.ReadByte();
                    obj.Message = reader.ReadString();
                }
            }
            return obj;
        }
    }
}
