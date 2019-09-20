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
    /// The server realmlist packet.
    /// Only allows for a single realm to be written.
    /// </summary>
    public class SMSG_Realmlist : IPacket
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Port { get; set; }

        public SMSG_Realmlist() : base((byte)ServerOpcodes.Opcodes.SMSG_REALMLIST) { }

        public override byte[] Serialize()
        {
            using (var memStr = new MemoryStream())
            {
                using (var writer = new BinaryWriter(memStr))
                {
                    writer.Write(_opcode);
                    writer.Write(ID);
                    writer.Write(Name);
                    writer.Write(Port);
                }
                return memStr.ToArray();
            }
        }

        public override IPacket Deserialize(byte[] data)
        {
            var obj = new SMSG_Realmlist();
            using (var memStr = new MemoryStream(data))
            {
                using (var reader = new BinaryReader(memStr))
                {
                    reader.ReadByte();
                    obj.ID = reader.ReadInt32();
                    obj.Name = reader.ReadString();
                    obj.Port = reader.ReadInt32();
                }
            }
            return obj;
        }
    }
}
