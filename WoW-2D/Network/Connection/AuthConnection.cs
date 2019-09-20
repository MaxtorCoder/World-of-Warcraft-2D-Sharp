using Framework.Network.Connection;
using Framework.Network.Packet;
using Framework.Network.Packet.Client;
using Framework.Network.Packet.Opcodes;
using Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WoW_2D.Network.Connection
{
    /// <summary>
    /// The client-side authentication connection.
    /// </summary>
    public class AuthConnection : IConnection
    {
        private string username, password;

        public AuthConnection(Socket clientSocket, string username, string password) : base(clientSocket)
        {
            OnDataReceived += ProcessRecvd;

            this.username = username;
            this.password = password;

            Initialize();
        }

        private void Initialize()
        {
            NetworkManager.State = NetworkManager.NetworkState.Connecting;
            _clientSocket.BeginConnect(WorldofWarcraft.Realmlist, new AsyncCallback(Connect), null);
        }

        private void Connect(IAsyncResult asyncResult)
        {
            try
            {
                _clientSocket.EndConnect(asyncResult);
                NetworkManager.State = NetworkManager.NetworkState.Authenticating;

                Receive();
                Send(new CMSG_Logon() { Username = username, Password = password });
            }
            catch
            {
                NetworkManager.State = NetworkManager.NetworkState.ConnectingFailed;
            }
        }

        public override void Close()
        {
            OnDataReceived -= ProcessRecvd;
            base.Close();
        }

        // TODO: Handle server close.
        private void ProcessRecvd(IAsyncResult asyncResult)
        {
            var authConnection = asyncResult.AsyncState as AuthConnection;
            var len = authConnection.EndReceive(asyncResult);
            var buffer = new byte[len];
            Array.Copy(Global.GetTempBuffer(), buffer, len);

            try
            {
                var opcode = buffer[0];
                if (Enum.IsDefined(typeof(ServerOpcodes.Opcodes), (int)opcode))
                    PacketRegistry.Invoke(opcode, this, buffer);

                Receive();
            }
            catch
            {
                authConnection.OnDataReceived -= ProcessRecvd;
                Close();
            }
        }
    }
}
