using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Network.Packet
{
    public abstract class GamePacket
    {
        public const int HeaderSize = 6;

        /// <summary>
        /// GamePacket structure.
        /// </summary>
        public PacketHeader Header;
        public byte[] Data;
    }
}
