using Framework;
using Framework.Network.Packet;
using Framework.Network.Packet.OpCodes;
using Framework.Network.Server;
using Framework.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WorldServer.Command;
using WorldServer.Network;
using WorldServer.Network.Handlers;

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
            PrintIntro();
            InitializeWorld();

            _logInput.KeyUp += OnKeyPressed;
            Closing += OnClosing;
        }

        private void PrintIntro()
        {
            QueueLogMessage("My first XAML project <3");
            QueueLogMessage(VersionStr);
        }

        private void InitializeWorld()
        {
            tcpServer = new TCPSocketServer("127.0.0.1", port);
            tcpServer.OnClientConnected += OnClientConnected;

            if (DatabaseManager.Initialize() == DatabaseManager.Status.Fatal)
                tcpServer.ExitCode = ExitCode.Exit_Code_Database;

            if (tcpServer.ExitCode == 0)
            {
                QueueLogMessage($"Initialized on {port}");
            }

            // TODO: Load maps and such.

            coreThread = new Thread(new ThreadStart(CoreThread));
            coreThread.Start();
        }

        private void CoreThread()
        {
            PacketRegistry.DefineHandler((byte)ClientOpcodes.CMSG_WORLD, WorldHandler.HandleWorldLogin);
            PacketRegistry.DefineHandler((byte)ClientOpcodes.CMSG_CHARACTER_LIST, CharHandler.HandleList);
            PacketRegistry.DefineHandler((byte)ClientOpcodes.CMSG_CHARACTER_CREATE, CharHandler.HandleCreation);
            PacketRegistry.DefineHandler((byte)ClientOpcodes.CMSG_CHARACTER_DELETE, CharHandler.HandleDeletion);

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

                        var worldConnection = (WorldConnection)connection;
                        try
                        {
                            // TODO: Print character name if exists.
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

        private void ClearOutput() => _logOuput.Text = string.Empty;
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
