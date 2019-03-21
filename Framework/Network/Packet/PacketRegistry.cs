using Framework.Network.Connection;
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
    public class PacketRegistry
    {
        private static Dictionary<byte, Handler> Handlers = new Dictionary<byte, Handler>();
        public delegate void Handler(IConnection connection, byte[] buffer);

        public static void DefineHandler(byte opcode, Handler handler) => Handlers[opcode] = handler;
        public static void Invoke(byte opcode, IConnection connection, byte[] buffer)
        {
            if (Handlers.ContainsKey(opcode)) Handlers[opcode].Invoke(connection, buffer);
        }
    }
}
