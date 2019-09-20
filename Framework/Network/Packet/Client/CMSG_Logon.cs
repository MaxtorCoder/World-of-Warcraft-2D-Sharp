using Framework.Network.Cryptography;
using Framework.Network.Packet.Opcodes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Network.Packet.Client
{
    /// <summary>
    /// CLient logon packet.
    /// </summary>
    public class CMSG_Logon : IPacket
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public CMSG_Logon() : base((byte)ClientOpcodes.CMSG_LOGON) { }

        public override byte[] Serialize()
        {
            using (var memStr = new MemoryStream())
            {
                using (var writer = new BinaryWriter(memStr))
                {
                    writer.Write(_opcode);
                    writer.Write(Username);
                    writer.Write(CryptoHelper.ComputeSHA256(Password));
                }
                return memStr.ToArray();
            }
        }

        public override IPacket Deserialize(byte[] data)
        {
            var obj = new CMSG_Logon();
            using (var memStr = new MemoryStream(data))
            {
                using (var reader = new BinaryReader(memStr))
                {
                    reader.ReadByte();
                    obj.Username = reader.ReadString();
                    obj.Password = reader.ReadString();
                }
            }
            return obj;
        }
    }
}
