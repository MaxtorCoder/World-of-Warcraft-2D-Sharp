using Framework;
using Framework.Entity;
using Framework.Network.Connection;
using Framework.Network.Packet.Client;
using Framework.Network.Packet.Opcodes;
using Framework.Network.Packet.Server;
using Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldServer.Network.Handlers
{
    /// <summary>
    /// Handles everything related to player characters.
    /// </summary>
    public class CharHandler
    {
        public static void HandleCreation(IConnection connection, byte[] buffer)
        {
            var packet = (CMSG_Character_Create)new CMSG_Character_Create().Deserialize(buffer);

            var mapId = DatabaseManager.FetchMapIDForRace((int)packet.Race);
            var map = WorldManager.GetMapByID(mapId);
            var status = DatabaseManager.CreateCharacter(((WorldConnection)connection).Account.ID, packet.Name, packet.Race, packet.Class, map);
            if (map.ID == -1)
                status = DatabaseManager.Status.Fatal;
            switch (status)
            {
                case DatabaseManager.Status.RowExists:
                    connection.Send(new SMSG_Character_Create() { Magic = (byte)ServerOpcodes.Responses.SMSG_CHARACTER_EXISTS });
                    return;
                case DatabaseManager.Status.Fatal:
                    connection.Send(new SMSG_Character_Create() { Magic = (byte)ServerOpcodes.Responses.SMSG_CHARACTER_SERVER_ERROR });
                    return;
                case DatabaseManager.Status.OK:
                    connection.Send(new SMSG_Character_Create() { Magic = (byte)ServerOpcodes.Responses.SMSG_CHARACTER_SUCCESS });
                    return;
            }
        }

        public static void HandleList(IConnection connection, byte[] buffer)
        {
            var characters = DatabaseManager.FetchCharacters(((WorldConnection)connection).Account.ID);

            for (int i = 0; i < characters.Count; i++)
            {
                var mapId = DatabaseManager.FetchMapIDForCharacter(characters[i].GUID);
                var map = WorldManager.GetMapByID(mapId);
                characters[i].Location = map.Name;
            }

            ((WorldConnection)connection).Account.RealmCharacters = characters;
            connection.Send(new SMSG_Character_List() { Characters = characters });
        }

        public static void HandleDeletion(IConnection connection, byte[] buffer)
        {
            var packet = (CMSG_Character_Delete)new CMSG_Character_Delete().Deserialize(buffer);
            var status = DatabaseManager.DeleteCharacter(((WorldConnection)connection).Account.ID, packet.Name);

            if (status == DatabaseManager.Status.OK)
                connection.Send(new SMSG_Character_Delete());
        }
    }
}
