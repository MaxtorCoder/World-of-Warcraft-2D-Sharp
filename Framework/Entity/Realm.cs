using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Entity
{
    /// <summary>
    /// Holds realm data.
    /// </summary>
    public class Realm
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Port { get; set; }
    }
}
