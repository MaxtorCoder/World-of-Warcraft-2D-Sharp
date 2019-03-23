using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Network.Packet.OpCodes
{
    /// <summary>
    /// Opcodes used by the client.
    /// </summary>
    public enum ClientOpcodes
    {
        CMSG_LOGON          =           0x01,
        CMSG_REALMLIST      =           0x10,
        CMSG_LOGOUT         =           0xFF,
    }
}
