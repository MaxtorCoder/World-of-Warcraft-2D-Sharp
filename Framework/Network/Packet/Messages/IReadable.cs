using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Network.Packet.Messages
{
    public interface IReadable
    {
        void Read(PacketReader reader);
    }
}
