using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Network.Packet
{
    /// <summary>
    /// A constant list of the opcodes used in the game.
    /// </summary>
    public class OpCodes
    {
        public const byte CMSG_LOGON = 0x01;
        public const byte SMSG_LOGON = 0x02;
    }
}
