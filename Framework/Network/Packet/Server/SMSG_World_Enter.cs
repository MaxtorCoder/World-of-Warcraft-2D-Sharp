using Framework.Entity;
using Framework.Network.Packet.OpCodes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using static Framework.Entity.Vector;

namespace Framework.Network.Packet.Server
{
    /// <summary>
    /// The server's world-enter response packet.
    /// </summary>
    public class SMSG_World_Enter : IPacket
    {
        public WorldCharacter WorldCharacter { get; set; }

        public SMSG_World_Enter() : base((byte)ServerOpcodes.Opcodes.SMSG_WORLD_ENTER) { }

        public override byte[] Serialize()
        {
            using (var memStr = new MemoryStream())
            {
                using (var writer = new BinaryWriter(memStr))
                {
                    writer.Write(_opcode);
                    writer.Write(WorldCharacter.Name);
                    writer.Write(WorldCharacter.Level);
                    writer.Write((int)WorldCharacter.Class);
                    writer.Write((int)WorldCharacter.Race);
                    writer.Write(WorldCharacter.Vector.MapID);
                    writer.Write(WorldCharacter.Vector.CellID);
                    writer.Write(WorldCharacter.Vector.X);
                    writer.Write(WorldCharacter.Vector.Y);
                    writer.Write((int)WorldCharacter.Vector.Direction);
                }
                return memStr.ToArray();
            }
        }

        public override IPacket Deserialize(byte[] data)
        {
            var obj = new SMSG_World_Enter();
            var worldCharacter = new WorldCharacter();
            using (var memStr = new MemoryStream(data))
            {
                using (var reader = new BinaryReader(memStr))
                {
                    reader.ReadByte();
                    worldCharacter.Name = reader.ReadString();
                    worldCharacter.Level = reader.ReadInt32();
                    worldCharacter.Class = (Class)reader.ReadInt32();
                    worldCharacter.Race = (Race)reader.ReadInt32();
                    worldCharacter.Vector = new Vector()
                    {
                        MapID = reader.ReadInt32(),
                        CellID = reader.ReadInt32(),
                        X = reader.ReadSingle(),
                        Y = reader.ReadSingle(),
                        Direction = (MoveDirection)reader.ReadInt32()
                    };
                }
            }
            obj.WorldCharacter = worldCharacter;
            return obj;
        }
    }
}
