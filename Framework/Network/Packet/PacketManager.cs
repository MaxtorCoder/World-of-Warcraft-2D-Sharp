using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Network.Packet
{
    /// <summary>
    /// Manages the packet registry.
    /// </summary>
    public class PacketManager
    {
        private static Dictionary<byte, Handler> OpcodeHandlers = new Dictionary<byte, Handler>();
        public delegate void Handler(string obj);

        public static void DefineOpcode(byte opcode, Handler handler) => OpcodeHandlers[opcode] = handler;
        public static void InvokeHandler(byte opcode, string obj) => OpcodeHandlers[opcode]?.Invoke(obj);
    }
}
