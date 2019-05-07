using Framework.Network.Connection;
using Framework.Network.Entity;
using Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Network.Connection
{
    /// <summary>
    /// World-type connection.
    /// </summary>
    public class WorldConnection : IConnection
    {
        public Account Account { get; set; }

        public WorldConnection(Socket clientSocket) : base(clientSocket) => Receive();
    }
}
