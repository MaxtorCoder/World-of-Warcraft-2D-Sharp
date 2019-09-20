using Framework.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Network.Packet
{
    public class PacketReader : BinaryReader
    {
        private static MemoryStream stream;

        public PacketReader(byte[] data) 
            : base(stream = new MemoryStream(data))
        {

        }

        public uint BytesRemaining => stream?.Remaining() ?? 0u;
    }
}
