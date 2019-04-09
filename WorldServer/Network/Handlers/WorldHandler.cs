using Framework;
using Framework.Entity;
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
using static Framework.Entity.Vector;

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

        public static void HandleWorldEnter(IConnection connection, byte[] buffer)
        {
            var worldConnection = (WorldConnection)connection;
            var packet = (CMSG_World_Enter)new CMSG_World_Enter().Deserialize(buffer);

            foreach (var character in worldConnection.Account.RealmCharacters)
            {
                if (character.GUID == packet.GUID)
                {
                    worldConnection.Account.Character = new WorldCharacter()
                    {
                        GUID = character.GUID,
                        Name = character.Name,
                        Level = character.Level,
                        Class = character.Class,
                        Race = character.Race,
                        Vector = character.Vector
                    };
                }
            }
            DatabaseManager.UpdateOnlineCharacter(worldConnection.Account.ID, worldConnection.Account.Character.GUID);
            MainWindow.QueueLogMessage($"{worldConnection.Account.Character.Name} has joined our world!");

            worldConnection.Send(new SMSG_World_Enter()
            {
                WorldCharacter = worldConnection.Account.Character
            });
        }

        public static void HandleMoveUpdate(IConnection connection, byte[] buffer)
        {
            var worldConnection = (WorldConnection)connection;
            var packet = (CMSG_Movement_Update)new CMSG_Movement_Update().Deserialize(buffer);

            worldConnection.Account.Character.Vector.X = packet.X;
            worldConnection.Account.Character.Vector.Y = packet.Y;
            worldConnection.Account.Character.Vector.Direction = packet.Direction;
        }
    }
}
