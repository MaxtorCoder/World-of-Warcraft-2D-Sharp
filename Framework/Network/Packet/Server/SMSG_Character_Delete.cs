using Framework.Network.Packet.Opcodes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Network.Packet.Server
{
    /// <summary>
    /// The server signal for deletion.
    /// Need to possibly implement status errors.
    /// </summary>
    public class SMSG_Character_Delete : IPacket
    {
        public SMSG_Character_Delete() : base((byte)ServerOpcodes.Opcodes.SMSG_CHARACTER_DELETE) { }

        public override byte[] Serialize()
        {
            using (var memStr = new MemoryStream())
            {
                using (var writer = new BinaryWriter(memStr))
                {
                    writer.Write(_opcode);
                }
                return memStr.ToArray();
            }
        }

        public override IPacket Deserialize(byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}
