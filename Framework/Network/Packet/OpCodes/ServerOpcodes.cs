using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Network.Packet.OpCodes
{
    /// <summary>
    /// The server opcodes and their responses.
    /// </summary>
    public class ServerOpcodes
    {
        public enum Opcodes
        {
            SMSG_LOGON                      =          0x01,
            SMSG_CHARACTER_LIST             =          0x02,
            SMSG_CHARACTER_CREATE           =          0x03,
            SMSG_CHARACTER_DELETE           =          0x04,
            SMSG_REALMLIST                  =          0x10,
            SMSG_WORLD                      =          0x05,
            SMSG_WORLD_ENTER                =          0x06,
            SMSG_CHAT                       =          0x07,
            SMSG_ONLINE                     =          0x08,
            SMSG_CREATURE                   =          0x09,
            SMSG_CREATURE_LIST              =          0x11,
            SMSG_CONNECTION_ADD             =          0xFA,
            SMSG_CONNECTION_REMOVE          =          0xFB,
            SMSG_CONNECTION_MOVE            =          0xFC,
            SMSG_DISCONNECT                 =          0xFF
        }

        public enum Responses
        {
            SMSG_LOGON_SUCCESS              =          0x00,
            SMSG_LOGON_UNK                  =          0x10,
            SMSG_LOGON_FAILED               =          0x11,
            SMSG_LOGON_SERVER_ERROR         =          0x12,
            SMSG_LOGON_ALREADY_LOGGED_IN    =          0x13,

            SMSG_CHARACTER_CREATE           =          0x03,
            SMSG_CHARACTER_SUCCESS          =          0x00,
            SMSG_CHARACTER_EXISTS           =          0x01,
            SMSG_CHARACTER_SERVER_ERROR     =          0x02,

            SMSG_WORLD_SUCCESS              =          0x00,
            SMSG_WORLD_SERVER_ERROR         =          0x01
        }
    }
}
