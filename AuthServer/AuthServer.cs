using AuthServer.Network;
using AuthServer.Network.Handlers;
using AuthServer.Utils;
using Framework;
using Framework.Network.Connection;
using Framework.Network.Packet;
using Framework.Network.Server;
using Framework.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AuthServer
{
    /// <summary>
    /// The World of Warcraft 2D authentication server.
    /// </summary>
    public class AuthServer
    {
        private static readonly string Version = $"{FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion}";
        private string VersionStr = $"AuthServer Version v{Version}";
        private TCPSocketServer tcpServer;

        public AuthServer()
        {
            Console.Title = "DemiCore - Authentication";
            Logger.Write(Logger.LogType.Server, VersionStr);

            tcpServer = new TCPSocketServer("127.0.0.1", 1337);
            tcpServer.OnClientConnected += OnClientConnected;

            if (DatabaseManager.Initialize() == DatabaseManager.Status.Fatal)
                tcpServer.ExitCode = ExitCode.Exit_Code_Database;

            Logger.Write(Logger.LogType.Server, $"Initialized on {tcpServer.GetLocalEP().Port}");
            (new Thread(new ThreadStart(CoreThread))).Start();
        }

        private void CoreThread()
        {
            PacketManager.DefineOpcode(OpCodes.CMSG_LOGON, AccountHandler.Login);

            // TODO: Ping every client x/sec here; Handle disconnects.
            while (!tcpServer.IsDisposed)
            {
                Thread.Sleep(10);

                if (tcpServer.ExitCode > 0 || tcpServer.ExitCode < 0)
                    tcpServer.IsDisposed = true;
            }
            DisplayErrorCode();
        }

        private void DisplayErrorCode()
        {
            switch (tcpServer.ExitCode)
            {
                case ExitCode.Exit_Code_Database:
                    Logger.Write(Logger.LogType.Error, "A database error has occured. Please check your connection settings.");
                    Console.ReadLine();
                    break;
            }
        }

        private void OnClientConnected(IAsyncResult asyncResult)
        {
            var authConnection = new AuthConnection(tcpServer.EndAccept(asyncResult));
            authConnection.OnDataReceived += OnDataReceived;
            Global.Connections.Add(authConnection);
        }

        // TODO: Handle disconnects.
        private void OnDataReceived(IAsyncResult asyncResult)
        {
            var authConnection = (AuthConnection)asyncResult.AsyncState;
            var len = authConnection.EndReceive(asyncResult);
            var dataBuffer = new byte[len];
            Array.Copy(Global.TemporaryDataBuffer, dataBuffer, len);
            
            using (var reader = new BinaryReader(new MemoryStream(dataBuffer)))
            {
                var opcode = reader.ReadByte();
                var data = reader.ReadString();

                PacketManager.InvokeHandler(opcode, data);
            }
        }

        static void Main(string[] args) => new AuthServer();
    }
}
