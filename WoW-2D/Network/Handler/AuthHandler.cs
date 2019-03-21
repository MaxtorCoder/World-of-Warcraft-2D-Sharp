using Framework.Network.Connection;
using Framework.Network.Packet;
using Framework.Network.Packet.Server;
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
            var packet = (SMSG_Logon)new SMSG_Logon().DeserializePacket(buffer);

            if (packet.Magic == OpCodes.SMSG_LOGON_FAILED)
                NetworkManager.State = NetworkManager.NetworkState.AuthenticatingFailed;
            else if (packet.Magic == OpCodes.SMSG_LOGON_UNK)
                NetworkManager.State = NetworkManager.NetworkState.AuthenticatingUnk;
            else if (packet.Magic == OpCodes.SMSG_LOGON_SERVER_ERROR)
                NetworkManager.State = NetworkManager.NetworkState.ServerError;
            else if (packet.Magic == OpCodes.SMSG_LOGON_SUCCESS)
            {
                NetworkManager.State = NetworkManager.NetworkState.RetrievingRealmlist;
                // TODO: send realmlist packet.
            }
        }
    }
}
