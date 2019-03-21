using Framework.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static WorldServer.WorldServer;

namespace WorldServer.Command
{
    /// <summary>
    /// A base class for command-types.
    /// </summary>
    public abstract class ICommand
    {
        private string _prefix;
        private AccountSecurity _security;
        protected Dictionary<string, MethodInfo> _subCommands;

        public ICommand(string prefix, AccountSecurity security)
        {
            _prefix = prefix;
            _security = security;
            _subCommands = new Dictionary<string, MethodInfo>();
        }

        /// <summary>
        /// Handle the command input.
        /// </summary>
        /// <param name="args"></param>
        public abstract void HandleCommand(string[] args);

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
