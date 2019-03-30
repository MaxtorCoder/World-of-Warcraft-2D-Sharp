using Framework;
using Framework.Entity;
using Framework.Network.Connection;
using Framework.Network.Packet.Client;
using Framework.Network.Packet.OpCodes;
using Framework.Network.Packet.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Network.Handlers
{
    /// <summary>
    /// Handles everything related to player characters.
    /// </summary>
    public class CharHandler
    {
        public static void HandleCreation(IConnection connection, byte[] buffer)
        {
            var packet = (CMSG_Character_Create)new CMSG_Character_Create().Deserialize(buffer);

            var status = DatabaseManager.CreateCharacter(((AuthConnection)connection).Account.ID, packet.Name, packet.Race);
            switch (status)
            {
                case DatabaseManager.Status.RowExists:
                    connection.Send(new SMSG_Character_Create() { Magic = (byte)ServerOpcodes.SMSG_CHARACTER_EXISTS });
                    return;
                case DatabaseManager.Status.Fatal:
                    connection.Send(new SMSG_Character_Create() { Magic = (byte)ServerOpcodes.SMSG_CHARACTER_SERVER_ERROR });
                    return;
                case DatabaseManager.Status.OK:
                    connection.Send(new SMSG_Character_Create() { Magic = (byte)ServerOpcodes.SMSG_CHARACTER_SUCCESS });
                    return;
            }
        }

        public static void HandleList(IConnection connection, byte[] buffer)
        {
            var characters = DatabaseManager.FetchCharacters(((AuthConnection)connection).Account.ID);

            connection.Send(new SMSG_Character_List() { Characters = characters });
        }

        public static void HandleDeletion(IConnection connection, byte[] buffer)
        {
            var packet = (CMSG_Character_Delete)new CMSG_Character_Delete().Deserialize(buffer);
            var status = DatabaseManager.DeleteCharacter(((AuthConnection)connection).Account.ID, packet.Name);

            if (status == DatabaseManager.Status.OK)
                connection.Send(new SMSG_Character_Delete());

            // TODO: Handle a fatal return.
        }
    }
}
