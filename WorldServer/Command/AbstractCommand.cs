using Framework.Entity;
using Framework.Network;
using Framework.Network.Connection;
using Framework.Network.Packet.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WorldServer.Command
{
    /// <summary>
    /// A base class for command-types.
    /// </summary>
    public abstract class AbstractCommand
    {
        private string _prefix;
        private AccountSecurity _security;
        protected Dictionary<string, MethodInfo> _subCommands;

        public AbstractCommand(string prefix, AccountSecurity security)
        {
            _prefix = prefix;
            _security = security;
            _subCommands = new Dictionary<string, MethodInfo>();
        }

        /// <summary>
        /// Handle the command input.
        /// </summary>
        /// <param name="args"></param>
        public virtual void HandleCommand(IConnection connection, string[] args)
        {
            if (connection != null)
            {
                WorldConnection worldConnection = (WorldConnection)connection;
                if (worldConnection.Account.Security < _security)
                {
                    worldConnection.Send(new SMSG_Chat()
                    {
                        Flag = (byte)ChatFlag.Server,
                        Message = "Insufficient security."
                    });
                    return;
                }
            }

            if (args.Length > 1)
            {
                var subCommand = args[1].ToLower();
                foreach (var keyvalue in _subCommands)
                {
                    if (keyvalue.Key == subCommand)
                    {
                        var method = keyvalue.Value;
                        method.Invoke(this, new object[] { connection, args });
                    }
                }
            }
            else
                MainWindow.QueueLogMessage(ToString());
        }

        public string GetPrefix()
        {
            return _prefix;
        }

        public AccountSecurity GetSecurity()
        {
            return _security;
        }

        public abstract override string ToString();
    }
}
