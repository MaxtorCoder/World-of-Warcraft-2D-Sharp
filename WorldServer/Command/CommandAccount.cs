using Framework;
using Framework.Network;
using Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldServer.Command
{
    /// <summary>
    /// Command for handling account data.
    /// </summary>
    public class CommandAccount : ICommand
    {
        public CommandAccount() : base("account", AccountSecurity.Administrator)
        {
            _subCommands.Add("create", GetType().GetMethod("HandleCreation", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance));
        }

        public override void HandleCommand(string[] args)
        {
            if (args.Length > 1)
            {
                var subCommand = args[1].ToLower();
                foreach (var keyvalue in _subCommands)
                {
                    if (keyvalue.Key == subCommand)
                    {
                        var method = keyvalue.Value;
                        method.Invoke(this, new object[] { args });
                    }
                }
            }
        }

        /// <summary>
        /// Handle the creation command.
        /// </summary>
        /// <param name="args"></param>
        private void HandleCreation(string[] args)
        {
            string username = args[2].ToUpper();
            string password = args[3];

            var status = DatabaseManager.CreateUser(username, password);
            switch (status)
            {
                case DatabaseManager.Status.OK:
                    Logger.Write(Logger.LogType.Server, $"Account created: {username}.");
                    break;
                case DatabaseManager.Status.RowExists:
                    Logger.Write(Logger.LogType.Server, $"Unable to create account {username}; Already exists!");
                    break;
                case DatabaseManager.Status.Fatal:
                    Logger.Write(Logger.LogType.Error, $"A fatal error occurred while trying to create account {username}. Database error?");
                    break;
            }
        }

        public override string ToString()
        {
            return null;
        }
    }
}
