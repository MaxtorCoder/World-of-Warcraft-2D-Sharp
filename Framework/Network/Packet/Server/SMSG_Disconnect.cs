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
    /// Used for various different types of disconnects.
    /// </summary>
    public class SMSG_Disconnect : IPacket
    {
        public SMSG_Disconnect() : base((byte)ServerOpcodes.Opcodes.SMSG_DISCONNECT) { }

        public override byte[] Serialize()
        {
            using (var memStr = new MemoryStream())
            {
                using (var writer = new BinaryWriter(memStr))
                {
                    writer.Write(_opcode);
                }
                return memStr.ToArray();
            }
        }

        public override IPacket Deserialize(byte[] data)
        {
            return null;
        }
    }
}
