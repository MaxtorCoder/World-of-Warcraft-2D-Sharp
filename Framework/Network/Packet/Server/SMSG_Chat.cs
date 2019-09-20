using Framework.Network.Packet.Opcodes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Network.Packet.Server
{
    /// <summary>
    /// The server chat message.
    /// </summary>
    public class SMSG_Chat : IPacket
    {
        public byte Flag { get; set; }
        public string Sender { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

        public SMSG_Chat() : base((byte)ServerOpcodes.Opcodes.SMSG_CHAT) { }

        public override byte[] Serialize()
        {
            using (var memStr = new MemoryStream())
            {
                using (var writer = new BinaryWriter(memStr))
                {
                    writer.Write(_opcode);
                    writer.Write(Flag);
                    writer.Write(Sender);
                    writer.Write(Message);
                }
                return memStr.ToArray();
            }
        }

        public override IPacket Deserialize(byte[] data)
        {
            var obj = new SMSG_Chat();
            using (var memStr = new MemoryStream(data))
            {
                using (var reader = new BinaryReader(memStr))
                {
                    reader.ReadByte();
                    obj.Flag = reader.ReadByte();
                    obj.Sender = reader.ReadString();
                    obj.Message = reader.ReadString();
                }
            }
            return obj;
        }
    }
}
