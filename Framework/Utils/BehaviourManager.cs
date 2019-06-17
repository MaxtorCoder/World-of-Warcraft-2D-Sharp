using Framework;
using Framework.Utils.Behaviour;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Utils
{
    /// <summary>
    /// Loads all behaviours from the database.
    /// </summary>
    public class BehaviourManager
    {
        private static List<IBehaviour> BehavioursDB;

        public static void Initialize()
        {
            BehavioursDB = DatabaseManager.FetchBehaviours();
        }

        public static IBehaviour GetBehaviour(int id)
        {
            return BehavioursDB.Find(b => b.ID == id)?.Clone();
        }
    }
}
