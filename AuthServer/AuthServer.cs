using AuthServer.Network;
using AuthServer.Network.Handlers;
using Framework;
using Framework.Entity;
using Framework.Network.Connection;
using Framework.Network.Packet;
using Framework.Network.Packet.OpCodes;
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
        private static readonly string Version = $"" +
                    $"{FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileMajorPart}." +
                    $"{FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileMinorPart}." +
                    $"{FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileBuildPart}";
        private readonly string VersionStr = $"AuthServer Version v{Version}";
        private TCPSocketServer tcpServer;

        private static List<Realm> Realmlist;

        public AuthServer()
        {
            Console.Title = "DemiCore - Authentication";
            Logger.Write(Logger.LogType.Server, VersionStr);

            tcpServer = new TCPSocketServer("127.0.0.1", 1337);
            tcpServer.OnClientConnected += OnClientConnected;

            if (DatabaseManager.Initialize() == DatabaseManager.Status.Fatal)
                tcpServer.ExitCode = ExitCode.Exit_Code_Database;

            if (tcpServer.ExitCode == 0)
            {
                Realmlist = DatabaseManager.FetchRealms();
                Logger.Write(Logger.LogType.Server, $"Loaded {Realmlist.Count} realm(s)");
                Logger.Write(Logger.LogType.Server, $"Initialized on {tcpServer.GetLocalEP().Port}");
            }
            (new Thread(new ThreadStart(CoreThread))).Start();
        }

        private void CoreThread()
        {
            PacketRegistry.DefineHandler((byte)ClientOpcodes.CMSG_LOGON, AuthHandler.HandleLogin);
            PacketRegistry.DefineHandler((byte)ClientOpcodes.CMSG_REALMLIST, AuthHandler.HandleRealmlist);
            PacketRegistry.DefineHandler((byte)ClientOpcodes.CMSG_CHARACTER_CREATE, CharHandler.HandleCreation);
            PacketRegistry.DefineHandler((byte)ClientOpcodes.CMSG_CHARACTER_LIST, CharHandler.HandleList);
            PacketRegistry.DefineHandler((byte)ClientOpcodes.CMSG_CHARACTER_DELETE, CharHandler.HandleDeletion);

            while (!tcpServer.IsDisposed)
            {
                Thread.Sleep(10);

                if (tcpServer.ExitCode > 0 || tcpServer.ExitCode < 0)
                    tcpServer.IsDisposed = true;

                var connections = Global.GetConnections();
                for (int i = 0; i < connections.Count; i++)
                {
                    var connection = connections[i];
                    if (connection.ShouldDisconnect)
                    {
                        connection.OnDataReceived -= OnDataReceived;
                        connection.Close();
                        Global.RemoveConnection(connection);

                        var authConnection = (AuthConnection)connection;
                        try
                        {
                            if (!string.IsNullOrWhiteSpace(authConnection.Account.Username))
                                Logger.Write(Logger.LogType.Server, $"{authConnection.Account.Username} has disconnected.");
                        }
                        catch { }
                    }
                }
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
            Global.AddConnection(authConnection);
        }

        private void OnDataReceived(IAsyncResult asyncResult)
        {
            var authConnection = asyncResult.AsyncState as AuthConnection;
            var len = authConnection.EndReceive(asyncResult);
            if (len < 0)
            {
                authConnection.ShouldDisconnect = true;
                return;
            }

            try
            {
                var dataBuffer = new byte[len];
                Array.Copy(Global.GetTempBuffer(), dataBuffer, len);

                var opcode = dataBuffer[0];
                if (Enum.IsDefined(typeof(ClientOpcodes), (int)opcode))
                    PacketRegistry.Invoke(opcode, authConnection, dataBuffer);
                else
                    Logger.WriteDebug($"[Demi Auth] Received unknown opcode: {opcode.ToString("x2")}");

                authConnection.Receive();
            }
            catch
            {
                authConnection.ShouldDisconnect = true;
            }
        }

        public static List<Realm> GetRealmlist()
        {
            return Realmlist;
        }

        static void Main(string[] args) => new AuthServer();
    }
}
