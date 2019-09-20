using Framework.Entity;
using Framework.Network.Connection;
using Framework.Network.Packet;
using Framework.Network.Packet.Client;
using Framework.Network.Packet.Opcodes;
using Framework.Network.Packet.Server;
using Framework.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoW_2D.Network.Handler
{
    /// <summary>
    /// Handles most things related to authentication.
    /// </summary>
    public class AuthHandler
    {
        public static void HandleLogin(IConnection connection, byte[] buffer)
        {
            var packet = (SMSG_Logon)new SMSG_Logon().Deserialize(buffer);

            if (Enum.IsDefined(typeof(ServerOpcodes.Responses), (int)packet.Magic))
            {
                switch (packet.Magic)
                {
                    case (byte)ServerOpcodes.Responses.SMSG_LOGON_FAILED:
                        NetworkManager.State = NetworkManager.NetworkState.AuthenticatingFailed;
                        break;
                    case (byte)ServerOpcodes.Responses.SMSG_LOGON_UNK:
                        NetworkManager.State = NetworkManager.NetworkState.AuthenticatingUnk;
                        break;
                    case (byte)ServerOpcodes.Responses.SMSG_LOGON_SERVER_ERROR:
                        NetworkManager.State = NetworkManager.NetworkState.ServerError;
                        break;
                    case (byte)ServerOpcodes.Responses.SMSG_LOGON_ALREADY_LOGGED_IN:
                        NetworkManager.State = NetworkManager.NetworkState.AlreadyLoggedIn;
                        break;
                    case (byte)ServerOpcodes.Responses.SMSG_LOGON_SUCCESS:
                        NetworkManager.State = NetworkManager.NetworkState.RetrievingRealmlist;
                        NetworkManager.SessionID = Guid.Parse(packet.SessionID);
                        connection.Send(new CMSG_Realmlist());
                        break;
                }
            }
        }

        public static void HandleRealmlist(IConnection connection, byte[] buffer)
        {
            var packet = (SMSG_Realmlist)new SMSG_Realmlist().Deserialize(buffer);

            WorldofWarcraft.Realm = new Realm()
            {
                ID = packet.ID,
                Name = packet.Name,
                Port = packet.Port
            };

            NetworkManager.LogIntoGameServer();
        }
    }
}
