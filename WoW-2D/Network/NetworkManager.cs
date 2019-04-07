using Framework.Network.Packet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using WoW_2D.Network.Connection;

namespace WoW_2D.Network
{
    /// <summary>
    /// Handles most things network related.
    /// </summary>
    public class NetworkManager
    {
        public enum NetworkState
        {
            Waiting,
            Connecting,
            ConnectingFailed,
            Authenticating,
            AuthenticatingFailed,
            AuthenticatingUnk,
            AlreadyLoggedIn,
            RetrievingRealmlist,
            LoggingIntoGameServer,
            GameServerConnectionFailed,
            GameServer,
            CreatingCharacter,
            CreateCharacterError,
            CharacterExists,
            CharacterCreated,
            RetrievingCharacters,
            DeletingCharacter,
            EnteringWorld,
            EnterWorld,
            ServerError
        }

        public enum Direction
        {
            Auth,
            World,
            Both
        }

        public static NetworkState State { get; set; } = NetworkState.Waiting;
        private static AuthConnection authConnection;
        private static WorldConnection worldConnection;

        public static Guid SessionID { get; set; }

        /// <summary>
        /// Initialize a connection to the authentication server.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public static void Initialize(string username, string password)
        {
            authConnection = new AuthConnection(
                new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp),
                username, password);
        }

        public static void LogIntoGameServer()
        {
            worldConnection = new WorldConnection();
        }

        public static void Send(IPacket packet, Direction direction)
        {
            switch (direction)
            {
                case Direction.Auth:
                    authConnection.Send(packet);
                    break;
                case Direction.World:
                    worldConnection.Send(packet);
                    break;
            }
        }

        public static void Disconnect(Direction direction)
        {
            switch (direction)
            {
                case Direction.Auth:
                    authConnection.Close();
                    break;
                case Direction.World:
                    worldConnection.Close();
                    break;
                case Direction.Both:
                    authConnection.Close();
                    worldConnection.Close();
                    break;
            }
        }
    }
}
