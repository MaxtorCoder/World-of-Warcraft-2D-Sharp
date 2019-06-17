using Framework;
using Framework.Network;
using Framework.Network.Connection;
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
    public class CommandAccount : AbstractCommand
    {
        public CommandAccount() : base("account", AccountSecurity.Administrator)
        {
            _subCommands.Add("create", GetType().GetMethod("HandleCreation", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance));
            _subCommands.Add("gmlevel", GetType().GetMethod("HandleSecurity", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance));
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
                    MainWindow.QueueLogMessage($"Account created: {username}.");
                    break;
                case DatabaseManager.Status.RowExists:
                    MainWindow.QueueLogMessage($"Unable to create account {username}; Already exists!");
                    break;
                case DatabaseManager.Status.Fatal:
                    MainWindow.QueueLogMessage($"A fatal error occurred while trying to create account {username}. Database error?");
                    break;
            }
        }

        /// <summary>
        /// Handle the gmlevel command.
        /// </summary>
        /// <param name="args"></param>
        private void HandleSecurity(string[] args)
        {
            string username = args[2].ToUpper();
            int level = int.Parse(args[3]);

            if (Enum.IsDefined(typeof(AccountSecurity), level))
            {
                AccountSecurity security = (AccountSecurity)Enum.ToObject(typeof(AccountSecurity), level);

                var status = DatabaseManager.SetSecurity(username, level);
                switch (status)
                {
                    case DatabaseManager.Status.OK:
                        MainWindow.QueueLogMessage($"Security of {username} set to {security}.");
                        break;
                    case DatabaseManager.Status.Fatal:
                        MainWindow.QueueLogMessage($"Unable to set the security of {username}.");
                        break;
                }
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < _subCommands.Count; i++)
            {
                var keys = _subCommands.Keys.ToList();
                if (i < _subCommands.Count - 1)
                    sb.Append(keys[i] + ", ");
                else
                    sb.Append(keys[i]);
            }
            return $"Available commands for 'account': {sb.ToString()}";
        }
    }
}
