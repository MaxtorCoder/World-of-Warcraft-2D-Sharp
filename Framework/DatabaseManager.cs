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

        private static string AuthenticationConnectionStr =
            $"Server={SqlServer}; Database=demi_auth; User Id={SqlUsername}; Password={SqlPassword};";

        private static async Task<Status> InitializeAsync(MySqlConnection connection)
        {
            var status = Status.OK;

            try { await connection.OpenAsync(); }
            catch (MySqlException ex) { status = Status.Fatal; }

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
            } catch (MySqlException ex) { status = Status.Fatal; }

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
            } catch (MySqlException ex) { status = Status.Fatal; }

            return status;
        }

        /// <summary>
        /// Does this user exist?
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        private static Status UserExists(string username)
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

            var password_hash = Argon2.Hash(password);
            var query = "INSERT INTO account (username, password_hash, security) VALUES (@username, @password, @security)";
            var status = Status.OK;

            using (var connection = new MySqlConnection(AuthenticationConnectionStr))
            {
                using (var command = new MySqlCommand(query))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password_hash);
                    command.Parameters.AddWithValue("@security", 0);
                    command.Connection = connection;

                    status = ExecuteCommand(connection, command).Result;
                }
            }
            return status;
        }
    }
}
