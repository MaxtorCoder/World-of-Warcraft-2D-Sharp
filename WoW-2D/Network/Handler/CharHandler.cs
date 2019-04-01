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

namespace WoW_2D.Network.Handler
{
    /// <summary>
    /// Handles everything related to player characters.
    /// </summary>
    public class CharHandler
    {
        public static void HandleCreation(IConnection connection, byte[] buffer)
        {
            var packet = (SMSG_Character_Create)new SMSG_Character_Create().Deserialize(buffer);
            if (Enum.IsDefined(typeof(ServerOpcodes.Responses), (int)packet.Magic))
            {
                switch (packet.Magic)
                {
                    case (byte)ServerOpcodes.Responses.SMSG_CHARACTER_EXISTS:
                        NetworkManager.State = NetworkManager.NetworkState.CharacterExists;
                        break;
                    case (byte)ServerOpcodes.Responses.SMSG_CHARACTER_SERVER_ERROR:
                        NetworkManager.State = NetworkManager.NetworkState.CreateCharacterError;
                        break;
                    case (byte)ServerOpcodes.Responses.SMSG_CHARACTER_SUCCESS:
                        NetworkManager.State = NetworkManager.NetworkState.CharacterCreated;
                        break;
                }
            }
        }

        public static void HandleList(IConnection connection, byte[] buffer)
        {
            var packet = (SMSG_Character_List)new SMSG_Character_List().Deserialize(buffer);

            WorldofWarcraft.RealmCharacters = packet.Characters;
            NetworkManager.State = NetworkManager.NetworkState.Waiting;
        }

        public static void HandleListComplete(IConnection connection, byte[] buffer) => NetworkManager.State = NetworkManager.NetworkState.Waiting;

        public static void HandleDeletion(IConnection connection, byte[] buffer)
        {
            NetworkManager.State = NetworkManager.NetworkState.RetrievingCharacters;
            NetworkManager.Send(new CMSG_Character_List(), NetworkManager.Direction.World);
        }
    }
}
