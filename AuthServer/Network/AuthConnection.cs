using AuthServer.Utils;
using Framework.Network.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Network
{
    /// <summary>
    /// Auth-type connection.
    /// </summary>
    public class AuthConnection : IConnection
    {
        public AuthConnection(Socket clientSocket) : base(clientSocket)
        {
            _clientSocket.BeginReceive(Global.TemporaryDataBuffer, 0, Global.TemporaryDataBuffer.Length, SocketFlags.None, new AsyncCallback(RaiseDataReceived), this);
        }
    }
}
