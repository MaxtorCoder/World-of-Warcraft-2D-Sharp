using Framework.Network.Packet;
using Framework.Utils;
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
        public bool ShouldDisconnect { get; set; }

        #region Event/Handlers
        public delegate void DataReceived(IAsyncResult asyncResult);
        public event DataReceived OnDataReceived;
        protected void RaiseDataReceived(IAsyncResult asyncResult) => OnDataReceived?.Invoke(asyncResult);
        #endregion

        public IConnection(Socket clientSocket) => _clientSocket = clientSocket;
        public int EndReceive(IAsyncResult asyncResult)
        {
            int len = -1;

            try { len = _clientSocket.EndReceive(asyncResult); }
            catch { Close(); }

            return len;
        }

        public void Receive()
        {
            _clientSocket.BeginReceive(Global.GetTempBuffer(), 0, Global.GetTempBuffer().Length, SocketFlags.None, new AsyncCallback(RaiseDataReceived), this);
        }

        public void Send(IPacket packet)
        {
            byte[] packetData = packet.Serialize();
            _clientSocket.BeginSend(packetData, 0, packetData.Length, SocketFlags.None, new AsyncCallback(SendCallback), _clientSocket);
        }

        private void SendCallback(IAsyncResult asyncResult)
        {
            var socket = asyncResult.AsyncState as Socket;
            try { socket.EndSend(asyncResult); }
            catch { Close(); }
        }

        public virtual void Close()
        {
            try
            { _clientSocket.Shutdown(SocketShutdown.Both); }
            catch { }
            finally { _clientSocket.Close(); }
        }
    }
}
