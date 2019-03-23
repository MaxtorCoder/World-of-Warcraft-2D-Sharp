using Framework;
using Framework.Network.Entity;
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
using Framework.Network.Packet.OpCodes;

namespace AuthServer.Network.Handlers
{
    /// <summary>
    /// Handles everything related to player accounts.
    /// </summary>
    public class AuthHandler
    {
        public static void HandleLogin(IConnection connection, byte[] buffer)
        {
            var packet = (CMSG_Logon)new CMSG_Logon().Deserialize(buffer);

            var status = DatabaseManager.UserExists(packet.Username);
            if (status != DatabaseManager.Status.RowExists)
            {
                connection.Send(new SMSG_Logon() { Magic = (byte)ServerOpcodes.SMSG_LOGON_UNK });
                connection.ShouldDisconnect = true;
                return;
            }

            var connections = Global.GetConnections();
            if (connections.Count > 0)
            {
                foreach (var c in connections)
                {
                    var authConnection = (AuthConnection)c;
                    if (authConnection.Account != null)
                    {
                        if (packet.Username.ToLower() == authConnection.Account.Username.ToLower())
                        {
                            connection.Send(new SMSG_Logon() { Magic = (byte)ServerOpcodes.SMSG_LOGON_ALREADY_LOGGED_IN });
                            connection.ShouldDisconnect = true;
                            return;
                        }
                    }
                }
            }

            var account = DatabaseManager.TryLogin(packet.Username, packet.Password);
            if (account.Status == Account.LoginStatus.Unknown)
            {
                connection.Send(new SMSG_Logon() { Magic = (byte)ServerOpcodes.SMSG_LOGON_FAILED });
                connection.ShouldDisconnect = true;
                return;
            }

            if (account.Status == Account.LoginStatus.ServerError)
            {
                connection.Send(new SMSG_Logon() { Magic = (byte)ServerOpcodes.SMSG_LOGON_SERVER_ERROR });
                connection.ShouldDisconnect = true;
                return;
            }

            ((AuthConnection)connection).Account = account;
            Logger.Write(Logger.LogType.Server, $"{account.Username} ({account.Security}) has logged in.");

            connection.Send(new SMSG_Logon() { Magic = (byte)ServerOpcodes.SMSG_LOGON_SUCCESS });
        }

        public static void HandleRealmlist(IConnection connection, byte[] buffer)
        {
            var realm = AuthServer.GetRealmlist()[0];

            connection.Send(new SMSG_Realmlist()
            {
                ID = realm.ID,
                Name = realm.Name,
                Port = realm.Port
            });
        }
    }
}
