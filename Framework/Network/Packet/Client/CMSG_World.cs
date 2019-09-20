using Framework.Network.Packet.Opcodes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Network.Packet.Client
{
    /// <summary>
    /// The world initialization packet.
    /// </summary>
    public class CMSG_World : IPacket
    {
        public string SessionID { get; set; }

        public CMSG_World() : base((byte)ClientOpcodes.CMSG_WORLD) { }

        public override byte[] Serialize()
        {
            using (var memStr = new MemoryStream())
            {
                using (var writer = new BinaryWriter(memStr))
                {
                    writer.Write(_opcode);
                    writer.Write(SessionID);
                }
                return memStr.ToArray();
            }
        }

        public override IPacket Deserialize(byte[] data)
        {
            var obj = new CMSG_World();
            using (var memStr = new MemoryStream(data))
            {
                using (var reader = new BinaryReader(memStr))
                {
                    reader.ReadByte();
                    obj.SessionID = reader.ReadString();
                }
            }
            return obj;
        }
    }
}
