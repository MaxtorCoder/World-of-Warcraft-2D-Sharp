using Framework.Network.Connection;
using Framework.Network.Packet;
using Framework.Network.Packet.Client;
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

                _clientSocket.BeginReceive(Global.GetTempBuffer(), 0, Global.GetTempBuffer().Length, SocketFlags.None, new AsyncCallback(RaiseDataReceived), this);
                Send(new CMSG_Logon() { Username = username, Password = password });
            }
            catch
            {
                NetworkManager.State = NetworkManager.NetworkState.ConnectingFailed;
            }
        }

        private void ProcessRecvd(IAsyncResult asyncResult)
        {
            var authConnection = (AuthConnection)asyncResult.AsyncState;
            var len = authConnection.EndReceive(asyncResult);
            var buffer = new byte[len];
            Array.Copy(Global.GetTempBuffer(), buffer, len);

            PacketRegistry.Invoke(buffer[0], this, buffer);
        }
    }
}
