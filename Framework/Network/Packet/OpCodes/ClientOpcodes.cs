using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Network.Packet.Opcodes
{
    /// <summary>
    /// Opcodes used by the client.
    /// </summary>
    public enum ClientOpcodes
    {
        CMSG_LOGON              =           0x01,
        CMSG_CHARACTER_LIST     =           0x02,
        CMSG_CHARACTER_CREATE   =           0x03,
        CMSG_CHARACTER_DELETE   =           0x04,
        CMSG_REALMLIST          =           0x10,
        CMSG_WORLD              =           0x05,
        CMSG_WORLD_ENTER        =           0x06,
        CMSG_MOVEMENT_UPDATE    =           0x07,
        CMSG_CHAT               =           0x08,
        CMSG_GENERIC_REQUEST    =           0xF0
    }

    public enum Requests
    {
        MOTD                    =           0x01,
        OnlineList              =           0x02,
        CreatureList            =           0x03
    }
}
