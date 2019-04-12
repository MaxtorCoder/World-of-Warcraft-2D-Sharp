using Framework.Entity;
using Framework.Network.Connection;
using Framework.Network.Packet.OpCodes;
using Framework.Network.Packet.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoW_2D.Utils;

namespace WoW_2D.Network.Handler
{
    /// <summary>
    /// Handles world related packets.
    /// </summary>
    public class WorldHandler
    {
        public static void HandleWorldLogin(IConnection connection, byte[] buffer)
        {
            var packet = (SMSG_World)new SMSG_World().Deserialize(buffer);
            
            if (Enum.IsDefined(typeof(ServerOpcodes.Responses), (int)packet.Magic))
            {
                switch (packet.Magic)
                {
                    case (byte)ServerOpcodes.Responses.SMSG_WORLD_SERVER_ERROR:
                        NetworkManager.State = NetworkManager.NetworkState.ServerError;
                        break;
                    case (byte)ServerOpcodes.Responses.SMSG_WORLD_SUCCESS:
                        NetworkManager.State = NetworkManager.NetworkState.GameServer;
                        break;
                }
            }
        }

        public static void HandleWorldEnter(IConnection connection, byte[] buffer)
        {
            var packet = (SMSG_World_Enter)new SMSG_World_Enter().Deserialize(buffer);

            if (packet.WorldCharacter.Vector.MapID != -1) // -1 is unused at the moment.
            {
                WorldofWarcraft.Character = packet.WorldCharacter;
                NetworkManager.State = NetworkManager.NetworkState.EnterWorld;
            }
        }

        public static void HandleChatMessage(IConnection connection, byte[] buffer)
        {
            var packet = (SMSG_Chat)new SMSG_Chat().Deserialize(buffer);

            var flagByte = packet.Flag;
            var sender = packet.Sender;
            var message = packet.Message;
            
            if (Enum.IsDefined(typeof(ChatFlag), (int)flagByte))
            {
                Global.Chats.Enqueue(new ChatMessage()
                {
                    Flag = (ChatFlag)Enum.ToObject(typeof(ChatFlag), flagByte),
                    Sender = sender,
                    Message = message
                });
            }
        }
    }
}
