using Framework.Entity;
using Framework.Network.Packet.Opcodes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Framework.Entity.Vector;

namespace Framework.Network.Packet.Server
{
    /// <summary>
    /// Sent to update a player's movement in the world.
    /// </summary>
    public class SMSG_Connection_Movement : IPacket
    {
        public string Name { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public MoveDirection Direction { get; set; }
        public bool IsMoving { get; set; }

        public SMSG_Connection_Movement() : base((byte)ServerOpcodes.Opcodes.SMSG_CONNECTION_MOVE) { }

        public override byte[] Serialize()
        {
            using (var memStr = new MemoryStream())
            {
                using (var writer = new BinaryWriter(memStr))
                {
                    writer.Write(_opcode);
                    writer.Write(Name);
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
            var obj = new SMSG_Connection_Movement();
            using (var memStr = new MemoryStream(data))
            {
                using (var reader = new BinaryReader(memStr))
                {
                    reader.ReadByte();
                    obj.Name = reader.ReadString();
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
