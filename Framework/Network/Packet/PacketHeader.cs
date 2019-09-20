using Framework.Network.Packet.Opcodes;

namespace Framework.Network.Packet
{
    public class PacketHeader
    {
        public uint PacketSize;
        public GameOpcodes Opcode;
    }
}
