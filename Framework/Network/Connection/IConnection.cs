using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Network.Connection
{
    /// <summary>
    /// A base for all connection-types.
    /// </summary>
    public class IConnection
    {
        protected Socket _clientSocket;

        #region Event/Handlers
        public delegate void DataReceived(IAsyncResult asyncResult);
        public event DataReceived OnDataReceived;
        protected void RaiseDataReceived(IAsyncResult asyncResult) => OnDataReceived?.Invoke(asyncResult);
        #endregion

        public IConnection(Socket clientSocket) => _clientSocket = clientSocket;
        public int EndReceive(IAsyncResult asyncResult) => _clientSocket.EndReceive(asyncResult);
    }
}
