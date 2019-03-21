using Framework.Network.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Utils
{
    /// <summary>
    /// Global variables; Used on either project (auth+world).
    /// </summary>
    public class Global
    {
        private static readonly byte[] _temporaryDataBuffer = new byte[1024];
        private static readonly List<IConnection> _connections = new List<IConnection>();

        public static byte[] GetTempBuffer()
        {
            return _temporaryDataBuffer;
        }

        public static void AddConnection(IConnection connection)
        {
            _connections.Add(connection);
        }
    }
}
