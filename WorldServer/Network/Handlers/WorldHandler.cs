using Framework;
using Framework.Network.Connection;
using Framework.Network.Entity;
using Framework.Network.Packet.Client;
using Framework.Network.Packet.OpCodes;
using Framework.Network.Packet.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldServer.Network.Handlers
{
    /// <summary>
    /// Handles packets relating to world requests.
    /// </summary>
    public class WorldHandler
    {
        public static void HandleWorldLogin(IConnection connection, byte[] buffer)
        {
            var packet = (CMSG_World)new CMSG_World().Deserialize(buffer);
            var account = DatabaseManager.FetchAccount(packet.SessionID);

            if (account.Status == Account.LoginStatus.ServerError)
            {
                connection.Send(new SMSG_World()
                {
                    Magic = (byte)ServerOpcodes.Responses.SMSG_WORLD_SERVER_ERROR
                });
                connection.ShouldDisconnect = true;
                return;
            }

            ((WorldConnection)connection).Account = account;
            connection.Send(new SMSG_World()
            {
                Magic = (byte)ServerOpcodes.Responses.SMSG_WORLD_SUCCESS
            });
        }
    }
}
