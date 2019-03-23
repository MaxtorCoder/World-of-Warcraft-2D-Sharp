using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Network.Packet
{
    /// <summary>
    /// A base for packet types.
    /// </summary>
    public abstract class IPacket
    {
        protected byte _opcode;

        public IPacket(byte opcode) => _opcode = opcode;

        public abstract byte[] Serialize();
        public abstract IPacket Deserialize(byte[] data);
    }
}
