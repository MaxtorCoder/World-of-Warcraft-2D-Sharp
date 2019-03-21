using Framework.Network;
using Framework.Network.Connection;
using Framework.Utils;
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
        public Account Account { get; set; }

        public AuthConnection(Socket clientSocket) : base(clientSocket)
        {
            _clientSocket.BeginReceive(Global.GetTempBuffer(), 0, Global.GetTempBuffer().Length, SocketFlags.None, new AsyncCallback(RaiseDataReceived), this);
        }
    }
}
