using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Network
{
    /// <summary>
    /// Security level enum.
    /// </summary>
    public enum AccountSecurity : int
    {
        Player = 0,
        Gamemaster = 1,
        Administrator = 2,
    }
}
