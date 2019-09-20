using Framework.Network.Connection;
using Framework.Network.Packet;
using Framework.Network.Packet.Client;
using Framework.Network.Packet.Opcodes;
using Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WoW_2D.Network.Connection
{
    /// <summary>
    /// The client-side world connection.
    /// </summary>
    public class WorldConnection : IConnection
    {
        public WorldConnection() : base(new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
        {
            OnDataReceived += ProcessRecvd;

            Initialize();
        }

        private void Initialize()
        {
            NetworkManager.State = NetworkManager.NetworkState.LoggingIntoGameServer;

            _clientSocket.BeginConnect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), WorldofWarcraft.Realm.Port), new AsyncCallback(Connect), null);
        }

        private void Connect(IAsyncResult asyncResult)
        {
            try
            {
                _clientSocket.EndConnect(asyncResult);
                Send(new CMSG_World()
                {
                    SessionID = NetworkManager.SessionID.ToString()
                });

                Receive();
            }
            catch
            {
                NetworkManager.State = NetworkManager.NetworkState.GameServerConnectionFailed;
            }
        }

        private void ProcessRecvd(IAsyncResult asyncResult)
        {
            var worldConnection = asyncResult.AsyncState as WorldConnection;
            var len = worldConnection.EndReceive(asyncResult);

            try
            {
                var buffer = new byte[len];
                Array.Copy(Global.GetTempBuffer(), buffer, len);
                var opcode = buffer[0];
                if (Enum.IsDefined(typeof(ServerOpcodes.Opcodes), (int)opcode))
                    PacketRegistry.Invoke(opcode, this, buffer);

                Receive();
            }
            catch
            {
                worldConnection.OnDataReceived -= ProcessRecvd;
                Close();
            }
        }
    }
}
