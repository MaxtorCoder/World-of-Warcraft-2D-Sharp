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
    /// The logout "request" packet.
    /// </summary>
    public class CMSG_Logout : IPacket
    {
        public CMSG_Logout() : base((byte)ClientOpcodes.CMSG_LOGOUT) { }

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
