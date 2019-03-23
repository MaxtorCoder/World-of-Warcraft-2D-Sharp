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
            } catch (MySqlException) { status = Status.Fatal; }

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
            } catch (MySqlException) { status = Status.Fatal; }

            return status;
        }

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

            return account;
        }
    }
}
