using AuthServer.Network;
using Framework.Network.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Utils
{
    /// <summary>
    /// Global variables.
    /// </summary>
    public class Global
    {
        public static byte[] TemporaryDataBuffer = new byte[1024];
        public static List<AuthConnection> Connections = new List<AuthConnection>();
    }
}
