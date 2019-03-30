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
            Realmlist,
            CreatingCharacter,
            CreateCharacterError,
            CharacterExists,
            CharacterCreated,
            RetrievingCharacters,
            DeletingCharacter,
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

        public static void Send(IPacket packet, Direction direction)
        {
            switch (direction)
            {
                case Direction.Auth:
                    authConnection.Send(packet);
                    break;
            }
        }

        public static void Disconnect(Direction direction)
        {
            State = NetworkState.Waiting;
            switch (direction)
            {
                case Direction.Auth:
                    authConnection.Close();
                    break;
            }
        }
    }
}
