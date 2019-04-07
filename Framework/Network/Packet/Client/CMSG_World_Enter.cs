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
    /// The world enter request packet.
    /// </summary>
    public class CMSG_World_Enter : IPacket
    {
        public string GUID { get; set; }

        public CMSG_World_Enter() : base((byte)ClientOpcodes.CMSG_WORLD_ENTER) { }

        public override byte[] Serialize()
        {
            using (var memStr = new MemoryStream())
            {
                using (var writer = new BinaryWriter(memStr))
                {
                    writer.Write(_opcode);
                    writer.Write(GUID);
                }
                return memStr.ToArray();
            }
        }

        public override IPacket Deserialize(byte[] data)
        {
            var obj = new CMSG_World_Enter();
            using (var memStr = new MemoryStream(data))
            {
                using (var reader = new BinaryReader(memStr))
                {
                    reader.ReadByte();
                    obj.GUID = reader.ReadString();
                }
            }
            return obj;
        }
    }
}
