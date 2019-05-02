using Framework;
using Framework.Network.Packet;
using Framework.Network.Packet.OpCodes;
using Framework.Network.Packet.Server;
using Framework.Network.Server;
using Framework.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using WorldServer.Command;
using WorldServer.Network;
using WorldServer.Network.Handlers;
using WorldServer.World;

namespace WorldServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly string Version = $"" +
                    $"{FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileMajorPart}." +
                    $"{FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileMinorPart}." +
                    $"{FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileBuildPart}";
        private readonly string VersionStr = $"WorldServer Version v{Version}";
        public static string MOTD = $"Welcome to v{Version} of World of Warcraft 2D!";
        private TCPSocketServer tcpServer;
        private Thread coreThread;
        private int port = 1338;
        private static Queue<string> LogMessageQueue = new Queue<string>();

        private readonly List<AbstractCommand> commands = new List<AbstractCommand>()
        {
            new CommandAccount()
        };

        public MainWindow()
        {
            InitializeComponent();
            InitializeWorld();

            _logInput.KeyUp += OnKeyPressed;
            Closing += OnClosing;
        }

        private void InitializeWorld()
        {
            QueueLogMessage(VersionStr);

            tcpServer = new TCPSocketServer("127.0.0.1", port);
            tcpServer.OnClientConnected += OnClientConnected;

            if (DatabaseManager.Initialize() == DatabaseManager.Status.Fatal)
                tcpServer.ExitCode = ExitCode.Exit_Code_Database;

            if (tcpServer.ExitCode == 0)
            {
                QueueLogMessage("Resetting sessions...");
                DatabaseManager.ResetSessions();

                QueueLogMessage("Resetting online characters...");
                DatabaseManager.ResetOnlineCharacters();

                QueueLogMessage("Initializing map data...");
                MapManager.Initialize();

                QueueLogMessage($"Initialized on {port}");
            }

            coreThread = new Thread(new ThreadStart(CoreThread));
            coreThread.Start();
        }

        private void CoreThread()
        {
            PacketRegistry.DefineHandler((byte)ClientOpcodes.CMSG_WORLD, WorldHandler.HandleWorldLogin);
            PacketRegistry.DefineHandler((byte)ClientOpcodes.CMSG_CHARACTER_LIST, CharHandler.HandleList);
            PacketRegistry.DefineHandler((byte)ClientOpcodes.CMSG_CHARACTER_CREATE, CharHandler.HandleCreation);
            PacketRegistry.DefineHandler((byte)ClientOpcodes.CMSG_CHARACTER_DELETE, CharHandler.HandleDeletion);
            PacketRegistry.DefineHandler((byte)ClientOpcodes.CMSG_WORLD_ENTER, WorldHandler.HandleWorldEnter);
            PacketRegistry.DefineHandler((byte)ClientOpcodes.CMSG_MOVEMENT_UPDATE, WorldHandler.HandleMoveUpdate);
            PacketRegistry.DefineHandler((byte)ClientOpcodes.CMSG_CHAT, WorldHandler.HandleChatMessage);

            while (true)
            {
                if (tcpServer.ExitCode > 0 || tcpServer.ExitCode < 0)
                    tcpServer.IsDisposed = true;
                if (tcpServer.IsDisposed)
                    break;

                HandleQueuedLog();

                var connections = Global.GetConnections();
                for (int i = 0; i < connections.Count; i++)
                {
                    var connection = connections[i];
                    if (connection.ShouldDisconnect)
                    {
                        connection.OnDataReceived -= OnDataReceived;
                        connection.Close();
                        Global.RemoveConnection(connection);

                        try
                        {
                            var worldConnection = (WorldConnection)connection;
                            var character = worldConnection.Account.Character;
                            if (character != null)
                            {
                                DatabaseManager.UpdateOnlineCharacter(worldConnection.Account.ID, string.Empty);
                                DatabaseManager.SaveCharacter(character);
                                MapManager.RemoveCharacterFromMap(worldConnection);
                                QueueLogMessage($"{worldConnection.Account.Character.Name} has left our world!");
                            }

                            var connectionsInMap = MapManager.GetCharactersWithinMap(worldConnection.Account.Character.Vector.MapID);
                            if (connectionsInMap.Count > 0)
                            {
                                foreach (var c in connectionsInMap)
                                    c.Send(new SMSG_Connection_Remove()
                                    {
                                        Name = worldConnection.Account.Character.Name
                                    });
                            }
                        }
                        catch { }
                    }
                }

                Thread.Sleep(10);
            }
            if (tcpServer.ExitCode == 0)
                Environment.Exit(tcpServer.ExitCode);
            DisplayErrorCode();
        }

        private void HandleQueuedLog()
        {
            if (LogMessageQueue.Count > 0)
            {
                var message = LogMessageQueue.Dequeue();
                AppendToLog(message);
            }
        }

        private void OnClientConnected(IAsyncResult asyncResult)
        {
            var worldConnection = new WorldConnection(tcpServer.EndAccept(asyncResult));
            worldConnection.OnDataReceived += OnDataReceived;
            Global.AddConnection(worldConnection);
        }

        private void OnDataReceived(IAsyncResult asyncResult)
        {
            var worldConnecion = asyncResult.AsyncState as WorldConnection;
            var len = worldConnecion.EndReceive(asyncResult);
            if (len < 0)
            {
                worldConnecion.ShouldDisconnect = true;
                return;
            }

            try
            {
                var dataBuffer = new byte[len];
                Array.Copy(Global.GetTempBuffer(), dataBuffer, len);

                var opcode = dataBuffer[0];
                if (Enum.IsDefined(typeof(ClientOpcodes), (int)opcode))
                    PacketRegistry.Invoke(opcode, worldConnecion, dataBuffer);

                worldConnecion.Receive();
            }
            catch
            {
                worldConnecion.ShouldDisconnect = true;
            }
        }

        private void OnClosing(object sender, EventArgs e) => tcpServer.IsDisposed = true;

        private void OnKeyPressed(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Return:
                    if (tcpServer.IsDisposed)
                        Environment.Exit(tcpServer.ExitCode);

                    foreach (var cmd in commands)
                    {
                        var command = _logInput.Text;
                        if (command.StartsWith(cmd.GetPrefix()))
                            cmd.HandleCommand(command.Split(' '));
                    }
                    ClearInput();
                    break;
            }
        }

        public static void QueueLogMessage(string message) => LogMessageQueue.Enqueue($"→ {message} {Environment.NewLine}");
        private void AppendToLog(string message)
        {
            Dispatcher.Invoke(() =>
            {
                _logOuput.Text += message;
            });
        }

        private void ClearOutput()
        {
            Dispatcher.Invoke(() =>
            {
                _logOuput.Text = string.Empty;
            });
        }
        private void ClearInput() => _logInput.Text = string.Empty;

        private void DisplayErrorCode()
        {
            ClearOutput();
            switch (tcpServer.ExitCode)
            {
                case ExitCode.Exit_Code_Database:
                    AppendToLog("A database error has occured. Please check your connection settings.");
                    break;
            }
        }
    }
}
