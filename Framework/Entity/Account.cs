using Framework.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Network.Entity
{
    /// <summary>
    /// Holds account data.
    /// </summary>
    public class Account
    {
        public enum LoginStatus
        {
            LoggedIn,
            AlreadyLoggedIn,
            Unknown,
            ServerError
        }

        public int ID { get; set; }
        public string Username { get; set; }
        public string Password_Hashed { get; set; }
        public AccountSecurity Security { get; set; }
        public Guid SessionID { get; set; }

        public List<RealmCharacter> RealmCharacters { get; set; }
        public WorldCharacter Character { get; set; }

        public LoginStatus Status { get; set; }
    }
}
