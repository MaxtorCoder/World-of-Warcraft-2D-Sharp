using Framework;
using Framework.Network;
using Framework.Network.Connection;
using Framework.Network.Packet;
using Framework.Network.Packet.Client;
using Framework.Network.Packet.Server;
using Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Network.Handlers
{
    /// <summary>
    /// Handles everything related to player accounts.
    /// </summary>
    public class AuthHandler
    {
        public static void HandleLogin(IConnection connection, byte[] buffer)
        {
            var packet = (CMSG_Logon)new CMSG_Logon().DeserializePacket(buffer);

            var status = DatabaseManager.UserExists(packet.Username);
            if (status != DatabaseManager.Status.RowExists)
            {
                connection.Send(new SMSG_Logon() { Magic = OpCodes.SMSG_LOGON_UNK });
                connection.Close();
                return;
            }

            var account = DatabaseManager.TryLogin(packet.Username, packet.Password);
            if (account.Status == Account.LoginStatus.Unknown)
            {
                connection.Send(new SMSG_Logon() { Magic = OpCodes.SMSG_LOGON_FAILED });
                connection.Close();
                return;
            }

            if (account.Status == Account.LoginStatus.ServerError)
            {
                connection.Send(new SMSG_Logon() { Magic = OpCodes.SMSG_LOGON_SERVER_ERROR });
                connection.Close();
                return;
            }

            ((AuthConnection)connection).Account = account;
            Logger.Write(Logger.LogType.Server, $"{account.Username} ({account.Security}) has logged in.");

            connection.Send(new SMSG_Logon() { Magic = OpCodes.SMSG_LOGON_SUCCESS });
        }
    }
}
