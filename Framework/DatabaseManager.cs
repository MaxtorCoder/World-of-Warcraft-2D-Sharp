using Framework.Entity;
using Framework.Network;
using Framework.Network.Cryptography;
using Framework.Network.Entity;
using Isopoh.Cryptography.Argon2;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    /// <summary>
    /// Handles everything related to the database.
    /// </summary>
    public class DatabaseManager
    {
        public enum Status
        {
            OK,
            RowExists,
            Fatal
        }

        private const string SqlServer = "localhost";
        private const string SqlUsername = "root";
        private const string SqlPassword = "admin";

        private static readonly string AuthenticationConnectionStr =
            $"Server={SqlServer}; Database=demi_auth; User Id={SqlUsername}; Password={SqlPassword};";

        private static readonly string CharacterConnectionStr =
            $"Server={SqlServer}; Database=demi_character; User Id={SqlUsername}; Password={SqlPassword};";

        private static async Task<Status> InitializeAsync(MySqlConnection connection)
        {
            var status = Status.OK;

            try { await connection.OpenAsync(); }
            catch (MySqlException) { status = Status.Fatal; }

            return status;
        }

        /// <summary>
        /// Test database connections.
        /// </summary>
        public static Status Initialize()
        {
            List<Status> statuses = new List<Status>();

            using (var connection = new MySqlConnection(AuthenticationConnectionStr))
            {
                Status status = InitializeAsync(connection).Result;
                statuses.Add(status);
            }

            using (var connection = new MySqlConnection(CharacterConnectionStr))
            {
                Status status = InitializeAsync(connection).Result;
                statuses.Add(status);
            }

            foreach (var status in statuses)
                if (status == Status.Fatal)
                    return Status.Fatal;
            return Status.OK;
        }

        /// <summary>
        /// Execute an asynchronous command.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        private static async Task<Status> ExecuteCommand(MySqlConnection connection, MySqlCommand command)
        {
            var status = Status.OK;

            try
            {
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            } catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                status = Status.Fatal;
            }

            return status;
        }

        /// <summary>
        /// Execute an asynchronous count.
        /// Currently only checks for a single existing row rather then a count.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        private static async Task<Status> ExecuteCount(MySqlConnection connection, MySqlCommand command)
        {
            var status = Status.OK;

            try
            {
                await connection.OpenAsync();
                int count = Convert.ToInt32(await command.ExecuteScalarAsync());
                if (count > 0)
                    status = Status.RowExists;
            } catch (MySqlException ex)
            {
                status = Status.Fatal;
                Console.WriteLine(ex.Message);
            }

            return status;
        }

        /// <summary>
        /// Execute an account reader.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        private static async Task<Account> ExecuteAccountReader(MySqlConnection connection, MySqlCommand command)
        {
            var account = new Account();

            try
            {
                await connection.OpenAsync();
                var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    account.ID = reader.GetInt32(0);
                    account.Username = reader.GetString(1);
                    account.Password_Hashed = reader.GetString(2);
                    account.Security = (AccountSecurity)reader.GetInt32(3);
                }
            }
            catch (MySqlException) { account.Status = Account.LoginStatus.ServerError; }

            return account;
        }

        /// <summary>
        /// Execute a realm reader.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        private static async Task<List<Realm>> ExecuteRealmReader(MySqlConnection connection, MySqlCommand command)
        {
            var realmlist = new List<Realm>();

            try
            {
                await connection.OpenAsync();
                var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var realm = new Realm()
                    {
                        ID = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Port = reader.GetInt32(2)
                    };
                    realmlist.Add(realm);
                }
            }
            catch (MySqlException) { }

            return realmlist;
        }

        /// <summary>
        /// Execute a character reader.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        private static async Task<List<RealmCharacter>> ExecuteCharacterReader(MySqlConnection connection, MySqlCommand command)
        {
            var characters = new List<RealmCharacter>();

            try
            {
                await connection.OpenAsync();
                var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    characters.Add(new RealmCharacter()
                    {
                        Name = reader["Username"].ToString(),
                        Level = (int)reader["Level"],
                        Class = (Class)reader["Class"],
                        Race = (Race)reader["Race"],
                        Location = "Elwynn Forest" // TODO: Read from database.
                    });
                }
            }
            catch (MySqlException) { }

            return characters;
        }

        /// <summary>
        /// Fetch a character guid.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        private static async Task<string> FetchCharacterGUID(MySqlConnection connection, MySqlCommand command)
        {
            var guid = string.Empty;

            try
            {
                await connection.OpenAsync();
                var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                    guid = reader["character_id"].ToString();
            }
            catch (MySqlException) { }

            return guid;
        }

        /// <summary>
        /// Fetch a map id.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        private static async Task<int> FetchMapID(MySqlConnection connection, MySqlCommand command)
        {
            var mapId = -1;

            try
            {
                await connection.OpenAsync();
                var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                    mapId = int.Parse(reader["map_id"].ToString());
            }
            catch (MySqlException) { }

            return mapId;
        }

        /// <summary>
        /// Fetch all realms for the realmlist.
        /// </summary>
        /// <returns></returns>
        public static List<Realm> FetchRealms()
        {
            var realmlist = new List<Realm>();
            var query = "SELECT * FROM realmlist";

            using (var connection = new MySqlConnection(AuthenticationConnectionStr))
            {
                using (var command = new MySqlCommand(query))
                {
                    command.Connection = connection;
                    realmlist = ExecuteRealmReader(connection, command).Result;
                }
            }
            return realmlist;
        }

        /// <summary>
        /// Fetch the map id belonging to the specified race id.
        /// </summary>
        /// <param name="raceId"></param>
        /// <returns></returns>
        public static int FetchMapIDForRace(int raceId)
        {
            var mapId = -1;
            var query = "SELECT map_id FROM character_spawns WHERE race_id=@raceId";

            using (var connection = new MySqlConnection(CharacterConnectionStr))
            {
                using (var command = new MySqlCommand(query))
                {
                    command.Connection = connection;
                    command.Parameters.AddWithValue("@raceId", raceId);
                    mapId = FetchMapID(connection, command).Result;
                }
            }

            return mapId;
        }

        /// <summary>
        /// Does this user exist?
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static Status UserExists(string username)
        {
            var status = Status.OK;
            var query = "SELECT COUNT(*) FROM account WHERE username=@username";

            using (var connection = new MySqlConnection(AuthenticationConnectionStr))
            {
                using (var command = new MySqlCommand(query))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Connection = connection;

                    status = ExecuteCount(connection, command).Result;
                }
            }

            return status;
        }

        /// <summary>
        /// Create a new user.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static Status CreateUser(string username, string password)
        {
            var existStatus = UserExists(username);
            if (existStatus == Status.RowExists || existStatus == Status.Fatal)
                return existStatus;

            var password_hash = CryptoHelper.ArgonHash(CryptoHelper.ComputeSHA256(password));
            var query = "INSERT INTO account (username, password_hash, security) VALUES (@username, @password, @security)";
            var status = Status.OK;

            using (var connection = new MySqlConnection(AuthenticationConnectionStr))
            {
                using (var command = new MySqlCommand(query))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password_hash);
                    command.Parameters.AddWithValue("@security", AccountSecurity.Player);
                    command.Connection = connection;

                    status = ExecuteCommand(connection, command).Result;
                }
            }
            return status;
        }

        /// <summary>
        /// Does this character exist?
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static Status CharacterExists(string name)
        {
            var status = Status.OK;
            var query = "SELECT COUNT(*) FROM user_character WHERE name=@name";

            using (var connection = new MySqlConnection(CharacterConnectionStr))
            {
                using (var command = new MySqlCommand(query))
                {
                    command.Parameters.AddWithValue("@name", name);
                    command.Connection = connection;

                    status = ExecuteCount(connection, command).Result;
                }
            }

            return status;
        }

        /// <summary>
        /// Attempts to create a character.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="name"></param>
        /// <param name="race"></param>
        /// <returns></returns>
        public static Status CreateCharacter(int userId, string name, Race race, int mapId)
        {
            name = name.Substring(0, 1).ToUpper() + name.Substring(1).ToLower();
            var existStatus = CharacterExists(name);
            if (existStatus == Status.RowExists || existStatus == Status.Fatal)
                return existStatus;

            var guid = Guid.NewGuid();
            var query = "INSERT INTO user_character (user_id, character_id, name) VALUES (@userId, @characterId, @name)";
            var status = Status.OK;

            using (var connection = new MySqlConnection(CharacterConnectionStr))
            {
                using (var command = new MySqlCommand(query))
                {
                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.AddWithValue("@characterId", guid);
                    command.Parameters.AddWithValue("@name", name);
                    command.Connection = connection;

                    status = ExecuteCommand(connection, command).Result;
                }
            }

            if (status != Status.OK)
                return status;

            query = "INSERT INTO character_data (user_id, character_id, level, class_id, race_id) VALUES (@userId, @characterId, @level, @classId, @raceId)";
            using (var connection = new MySqlConnection(CharacterConnectionStr))
            {
                using (var command = new MySqlCommand(query))
                {
                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.AddWithValue("@characterId", guid);
                    command.Parameters.AddWithValue("@level", 1);
                    command.Parameters.AddWithValue("@classId", (int)Class.Warrior);
                    command.Parameters.AddWithValue("@raceId", (int)race);
                    command.Connection = connection;

                    status = ExecuteCommand(connection, command).Result;
                }
            }

            query = "INSERT INTO character_location_data (character_id, map_id, cell_id, x, y) VALUES (@characterId, @mapId, -1, 0.0, 0.0)";
            using (var connection = new MySqlConnection(CharacterConnectionStr))
            {
                using (var command = new MySqlCommand(query))
                {
                    command.Parameters.AddWithValue("@characterId", guid);
                    command.Parameters.AddWithValue("@mapId", mapId);
                    command.Connection = connection;

                    status = ExecuteCommand(connection, command).Result;
                }
            }

            return status;
        }

        /// <summary>
        /// Get the characters for the specified user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static List<RealmCharacter> FetchCharacters(int userId)
        {
            var characters = new List<RealmCharacter>();
            var query = "SELECT DISTINCT user_character.name as Username, character_data.level as Level, character_data.class_id as Class, character_data.race_id as Race " +
                "FROM user_character, character_data " +
                "WHERE user_character.user_id=@userId AND character_data.user_id=@userId";

            using (var connection = new MySqlConnection(CharacterConnectionStr))
            {
                using (var command = new MySqlCommand(query))
                {
                    command.Parameters.AddWithValue("@userId", userId);
                    command.Connection = connection;
                    characters = ExecuteCharacterReader(connection, command).Result;
                }
            }

            return characters;
        }

        /// <summary>
        /// Attempt to delete the given character for the specified user.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Status DeleteCharacter(int userId, string name)
        {
            var guidQuery = "SELECT character_id FROM user_character WHERE user_id=@userId AND name=@name";
            var guid = string.Empty;

            using (var connection = new MySqlConnection(CharacterConnectionStr))
            {
                using (var command = new MySqlCommand(guidQuery))
                {
                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.AddWithValue("@name", name);
                    command.Connection = connection;
                    guid = FetchCharacterGUID(connection, command).Result;
                }
            }

            /** I have no real way of testing this at the moment. **/
            if (guid == string.Empty)
                return Status.Fatal;

            var tables = new string[]
            {
                "user_character",
                "character_data",
                "character_location_data"
            };
            
            foreach (var table in tables)
            {
                var status = Status.OK;

                var deleteQuery = "DELETE FROM "+table+" WHERE character_id=@characterId";
                using (var connection = new MySqlConnection(CharacterConnectionStr))
                {
                    using (var command = new MySqlCommand(deleteQuery))
                    {
                        command.Parameters.AddWithValue("@characterId", guid);
                        command.Connection = connection;
                        status = ExecuteCommand(connection, command).Result;
                    }
                }

                if (status == Status.Fatal)
                    return status;
            }

            return Status.OK;
        }

        /// <summary>
        /// This method assumes the user does in-fact exist.
        /// Attempt to log the player in with the given password.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static Account TryLogin(string username, string password)
        {
            var account = new Account();
            var query = "SELECT * FROM account WHERE username=@username";

            using (var connection = new MySqlConnection(AuthenticationConnectionStr))
            {
                using (var command = new MySqlCommand(query))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Connection = connection;
                    account = ExecuteAccountReader(connection, command).Result;
                }
            }

            if (account.Status == Account.LoginStatus.ServerError)
                return account;

            bool isPasswordCorrect = CryptoHelper.VerifyPassword(account.Password_Hashed, password);
            if (!isPasswordCorrect)
            {
                account.Status = Account.LoginStatus.Unknown;
                return account;
            }
            account.Status = Account.LoginStatus.LoggedIn;

            if (account.Status == Account.LoginStatus.LoggedIn)
            {
                var updateQuery = "UPDATE account SET session_id=@sessionId WHERE user_id=@userId";
                var sessionId = Guid.NewGuid();
                var sessionUpdate = Status.OK;
                using (var connection = new MySqlConnection(AuthenticationConnectionStr))
                {
                    using (var command = new MySqlCommand(updateQuery))
                    {
                        command.Parameters.AddWithValue("@sessionId", sessionId);
                        command.Parameters.AddWithValue("@userId", account.ID);
                        command.Connection = connection;
                        sessionUpdate = ExecuteCommand(connection, command).Result;
                    }
                }

                if (sessionUpdate != Status.OK)
                    account.Status = Account.LoginStatus.ServerError;
                else
                    account.SessionID = sessionId;
            }

            return account;
        }

        /// <summary>
        /// Fetch the account that the given sessionId belongs to.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public static Account FetchAccount(string sessionId)
        {
            var account = new Account();
            var query = "SELECT * FROM account WHERE session_id=@sessionId";

            using (var connection = new MySqlConnection(AuthenticationConnectionStr))
            {
                using (var command = new MySqlCommand(query))
                {
                    command.Parameters.AddWithValue("@sessionId", sessionId);
                    command.Connection = connection;
                    account = ExecuteAccountReader(connection, command).Result;
                }
            }

            if (account.Status == Account.LoginStatus.ServerError)
                return account;

            return account;
        }
    }
}
