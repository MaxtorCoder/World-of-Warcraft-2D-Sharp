using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Network.Server
{
    /// <summary>
    /// The TCP socket-server.
    /// </summary>
    public class TCPSocketServer : IServer
    {
        public TCPSocketServer(string ip, int port)
        {
            _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _serverSocket.Bind(new IPEndPoint(IPAddress.Parse(ip), port));
            _serverSocket.Listen(5);
            _serverSocket.BeginAccept(new AsyncCallback(RaiseClientConnected), null);
        }
    }
}
