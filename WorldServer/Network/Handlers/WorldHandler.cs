using Framework;
using Framework.Entity;
using Framework.Network;
using Framework.Network.Connection;
using Framework.Network.Entity;
using Framework.Network.Packet.Client;
using Framework.Network.Packet.OpCodes;
using Framework.Network.Packet.Server;
using Framework.Utils;
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
                        Stats = new Stats()
                        {
                            Level = character.Level
                        },
                        Class = character.Class,
                        Race = character.Race,
                        Vector = character.Vector
                    };
                }
            }

            /** Send this new player to all connected players.**/
            var onlinePlayers = WorldManager.GetAllPlayers();
            if (onlinePlayers.Count > 0)
            {
                foreach (var player in onlinePlayers)
                {
                    player.Send(new SMSG_Connection_Add()
                    {
                        WorldCharacter = worldConnection.Account.Character
                    });
                }
            }

            worldConnection.Send(new SMSG_World_Enter()
            {
                WorldCharacter = worldConnection.Account.Character
            });

            WorldManager.AddCharacterToMap(worldConnection);
            DatabaseManager.UpdateOnlineCharacter(worldConnection.Account.ID, worldConnection.Account.Character.GUID);
            MainWindow.QueueLogMessage($"{worldConnection.Account.Character.Name} has joined our world!");
        }

        public static void HandleGenericRequest(IConnection connection, byte[] buffer)
        {
            var packet = (CMSG_Generic)new CMSG_Generic().Deserialize(buffer);
            var worldConnection = (WorldConnection)connection;
            switch (packet.Type)
            {
                case (byte)Requests.MOTD:
                    // TODO: Fix lowercase text.
                    connection.Send(new SMSG_Chat()
                    {
                        Flag = (byte)ChatFlag.Server,
                        Message = string.Format(MainWindow.WorldSettings.GetSection("Data").GetString("motd"), MainWindow.Version)
                    });
                    break;
                case (byte)Requests.OnlineList:
                    var onlinePlayers = WorldManager.GetAllPlayers();
                    var onlineCharacters = new List<WorldCharacter>();
                    foreach (var player in onlinePlayers)
                        if (worldConnection.Account.ID != player.Account.ID)
                            onlineCharacters.Add(player.Account.Character);
                    connection.Send(new SMSG_Online() { Characters = onlineCharacters });
                    break;
                case (byte)Requests.CreatureList:
                    var creatures = WorldManager.GetMapByID(worldConnection.Account.Character.Vector.MapID).Creatures;
                    var clientCreatures = new List<ClientCreature>();
                    foreach (var c in creatures)
                    {
                        clientCreatures.Add(new ClientCreature()
                        {
                            GUID = c.GUID,
                            ID = c.ID,
                            ModelID = c.ModelID,
                            Name = c.Name,
                            SubName = c.SubName,
                            Stats = c.Stats,
                            Vector = c.Vector
                        });
                    }
                    connection.Send(new SMSG_Creature_List() { Creatures = clientCreatures });
                    break;
            }
        }

        public static void HandleMoveUpdate(IConnection connection, byte[] buffer)
        {
            var worldConnection = (WorldConnection)connection;
            var packet = (CMSG_Movement_Update)new CMSG_Movement_Update().Deserialize(buffer);

            worldConnection.Account.Character.Vector.X = packet.X;
            worldConnection.Account.Character.Vector.Y = packet.Y;
            worldConnection.Account.Character.Vector.Direction = packet.Direction;

            var onlinePlayers = WorldManager.GetPlayersWithinMap(worldConnection.Account.Character.Vector.MapID);
            if (onlinePlayers.Count > 0)
            {
                foreach (var player in onlinePlayers)
                {
                    if (player.Account.ID != worldConnection.Account.ID)
                        player.Send(new SMSG_Connection_Movement()
                        {
                            Name = worldConnection.Account.Character.Name,
                            X = worldConnection.Account.Character.Vector.X,
                            Y = worldConnection.Account.Character.Vector.Y,
                            Direction = worldConnection.Account.Character.Vector.Direction,
                            IsMoving = packet.IsMoving
                        });
                }
            }
        }

        public static void HandleChatMessage(IConnection connection, byte[] buffer)
        {
            var worldConnection = (WorldConnection)connection;
            var packet = (CMSG_Chat)new CMSG_Chat().Deserialize(buffer);
            var flag = (byte)ChatFlag.Player;

            if (worldConnection.Account.Security >= AccountSecurity.Gamemaster)
                flag = (byte)ChatFlag.GM;

            var packetMessage = packet.Message;
            if (packetMessage.StartsWith(MainWindow.WorldSettings.GetSection("Data").GetString("commandkey")))
            {
                packetMessage = packetMessage.Remove(0, 1);
                MainWindow.HandleCommandInput(worldConnection, packetMessage);
            }
            else
            {
                var players = WorldManager.GetPlayersWithinMap(worldConnection.Account.Character.Vector.MapID);
                if (players != null)
                {
                    foreach (var player in players)
                    {
                        player.Send(new SMSG_Chat()
                        {
                            Flag = flag,
                            Sender = worldConnection.Account.Character.Name,
                            Message = packet.Message
                        });
                    }
                }
            }
        }
    }
}
