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
    /// A base class for server-types.
    /// </summary>
    public class IServer
    {
        protected Socket _serverSocket;
        public int ExitCode { get; set; } = 0;
        public bool IsDisposed { get; set; } = false;

        #region Event/Handlers
        public delegate void ClientConnected(IAsyncResult asyncResult);
        public event ClientConnected OnClientConnected;
        protected void RaiseClientConnected(IAsyncResult asyncResult)
        {
            OnClientConnected?.Invoke(asyncResult);
            _serverSocket.BeginAccept(new AsyncCallback(RaiseClientConnected), null);
        }
        #endregion

        public Socket EndAccept(IAsyncResult asyncResult)
        {
            return _serverSocket.EndAccept(asyncResult);
        }

        public IPEndPoint GetLocalEP()
        {
            return (IPEndPoint)_serverSocket.LocalEndPoint;
        }
    }
}
