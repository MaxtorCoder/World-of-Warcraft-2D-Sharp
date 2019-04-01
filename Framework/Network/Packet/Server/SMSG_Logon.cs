using Framework.Network.Packet.OpCodes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Network.Packet.Server
{
    /// <summary>
    /// The server logon packet.
    /// </summary>
    public class SMSG_Logon : IPacket
    {
        public byte Magic { get; set; }
        public string SessionID { get; set; }

        public SMSG_Logon() : base((byte)ServerOpcodes.Opcodes.SMSG_LOGON) { }

        public override byte[] Serialize()
        {
            using (var memStr = new MemoryStream())
            {
                using (var writer = new BinaryWriter(memStr))
                {
                    writer.Write(_opcode);
                    writer.Write(Magic);
                    writer.Write(SessionID);
                }
                return memStr.ToArray();
            }
        }

        public override IPacket Deserialize(byte[] data)
        {
            var obj = new SMSG_Logon();
            using (var memStr = new MemoryStream(data))
            {
                using (var reader = new BinaryReader(memStr))
                {
                    reader.ReadByte();
                    obj.Magic = reader.ReadByte();
                    obj.SessionID = reader.ReadString();
                }
            }
            return obj;
        }
    }
}
