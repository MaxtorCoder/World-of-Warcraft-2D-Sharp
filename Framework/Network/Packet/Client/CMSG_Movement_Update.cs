using Framework.Network.Packet.Opcodes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Framework.Entity.Vector;

namespace Framework.Network.Packet.Client
{
    /// <summary>
    /// The client->server movement update packet.
    /// </summary>
    public class CMSG_Movement_Update : IPacket
    {
        public float X { get; set; }
        public float Y { get; set; }
        public MoveDirection Direction { get; set; }
        public bool IsMoving { get; set; }

        public CMSG_Movement_Update() : base((byte)ClientOpcodes.CMSG_MOVEMENT_UPDATE) { }

        public override byte[] Serialize()
        {
            using (var memStr = new MemoryStream())
            {
                using (var writer = new BinaryWriter(memStr))
                {
                    writer.Write(_opcode);
                    writer.Write(X);
                    writer.Write(Y);
                    writer.Write((int)Direction);
                    writer.Write(IsMoving);
                }
                return memStr.ToArray();
            }
        }

        public override IPacket Deserialize(byte[] data)
        {
            var obj = new CMSG_Movement_Update();
            using (var memStr = new MemoryStream(data))
            {
                using (var reader = new BinaryReader(memStr))
                {
                    reader.ReadByte();
                    obj.X = reader.ReadSingle();
                    obj.Y = reader.ReadSingle();
                    obj.Direction = (MoveDirection)reader.ReadInt32();
                    obj.IsMoving = reader.ReadBoolean();
                }
            }
            return obj;
        }
    }
}
