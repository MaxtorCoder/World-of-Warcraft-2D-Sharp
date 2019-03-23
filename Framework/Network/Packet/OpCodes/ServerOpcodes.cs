using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Network.Packet.OpCodes
{
    /// <summary>
    /// Opcodes used by the server.
    /// </summary>
    public enum ServerOpcodes
    {
        SMSG_LOGON                      =           0x01,
        SMSG_LOGON_SUCCESS              =           0x00,
        SMSG_LOGON_UNK                  =           0x10,
        SMSG_LOGON_FAILED               =           0x11,
        SMSG_LOGON_SERVER_ERROR         =           0x12,
        SMSG_LOGON_ALREADY_LOGGED_IN    =           0x13,

        SMSG_REALMLIST                  =           0x10,
    }
}
