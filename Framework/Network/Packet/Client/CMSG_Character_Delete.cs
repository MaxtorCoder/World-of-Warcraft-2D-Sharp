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
    /// The client character-delete packet.
    /// </summary>
    public class CMSG_Character_Delete : IPacket
    {
        public string Name { get; set; }

        public CMSG_Character_Delete() : base((byte)ClientOpcodes.CMSG_CHARACTER_DELETE) { }

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
            var obj = new CMSG_Character_Delete();
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
