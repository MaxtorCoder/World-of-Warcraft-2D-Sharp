using Framework.Entity;
using Framework.Network.Packet.OpCodes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Network.Packet.Client
{
    /// <summary>
    /// Client character creation packet.
    /// </summary>
    public class CMSG_Character_Create : IPacket
    {
        public string Name { get; set; }
        public Race Race { get; set; }

        public CMSG_Character_Create() : base((byte)ClientOpcodes.CMSG_CHARACTER_CREATE) { }

        public override byte[] Serialize()
        {
            using (var memStr = new MemoryStream())
            {
                using (var writer = new BinaryWriter(memStr))
                {
                    writer.Write(_opcode);
                    writer.Write(Name);
                    writer.Write((int)Race);
                }
                return memStr.ToArray();
            }
        }

        public override IPacket Deserialize(byte[] data)
        {
            var obj = new CMSG_Character_Create();
            using (var memStr = new MemoryStream(data))
            {
                using (var reader = new BinaryReader(memStr))
                {
                    reader.ReadByte();
                    obj.Name = reader.ReadString();
                    obj.Race = (Race)reader.ReadInt32(); // TODO: Catch the error that could happen here (for example: the client sending their own custom race).
                }
            }
            return obj;
        }
    }
}
