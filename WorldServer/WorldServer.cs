using Framework.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WorldServer.Command;

namespace WorldServer
{
    /// <summary>
    /// The World of Warcraft 2D world server.
    /// </summary>
    public class WorldServer
    {
        private static readonly string Version = $"" +
                    $"{FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileMajorPart}." +
                    $"{FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileMinorPart}." +
                    $"{FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileBuildPart}";
        private readonly string VersionStr = $"WorldServer Version v{Version}";

        private readonly List<ICommand> commands = new List<ICommand>()
        {
            new CommandAccount()
        };

        public WorldServer()
        {
            Console.Title = "DemiCore - World";
            Logger.Write(Logger.LogType.Server, VersionStr);

            (new Thread(new ThreadStart(CoreThread))).Start();
        }

        private void CoreThread()
        {
            Console.Write($"DemiCore {Version} >> ");
            while (true)
            {
                string command = Console.ReadLine();
                ProcessInput(command);
                Console.Write($"DemiCore {Version} >> ");

                Thread.Sleep(10);
            }
        }

        private void ProcessInput(string command)
        {
            foreach (var cmd in commands)
            {
                if (command.StartsWith(cmd.GetPrefix()))
                    cmd.HandleCommand(command.Split(' '));
            }
        }

        static void Main(string[] args) => new WorldServer();
    }
}
