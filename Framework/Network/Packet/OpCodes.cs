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
        /** Client->server opcodes. **/
        public const byte CMSG_LOGON = 0x01;

        /** Server->client opcodes. **/
        public const byte SMSG_LOGON = 0x02;
        public const byte SMSG_LOGON_SUCCESS = 0x00;
        public const byte SMSG_LOGON_UNK = 0x10;                    /* Unknown account. */
        public const byte SMSG_LOGON_FAILED = 0x11;                 /* Incorrect username/password. */
        public const byte SMSG_LOGON_SERVER_ERROR = 0x12;           /* Database error. */
    }
}
