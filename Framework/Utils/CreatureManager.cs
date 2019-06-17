using Framework;
using Framework.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Utils
{
    /// <summary>
    /// Contains and handles data about the creatures.
    /// </summary>
    public class CreatureManager
    {
        private static List<WorldCreature> CreaturesDB;

        public static void Initialize()
        {
            CreaturesDB = DatabaseManager.FetchCreatures();
        }

        public static void InitializeBehaviours()
        {
            foreach (var creature in CreaturesDB)
            {
                foreach (int id in creature.BehaviourIDs)
                {
                    creature.Behaviours.Add(BehaviourManager.GetBehaviour(id)?.Clone());
                }
            }
        }

        public static WorldCreature GetCreatureWithEntryID(int id)
        {
            WorldCreature creatureCopy = null;
            foreach (var creature in CreaturesDB)
            {
                if (creature.ID == id)
                    creatureCopy = creature.Clone();
                if (creatureCopy != null)
                    break;
            }
            return creatureCopy;
        }
        
        /// <summary>
        /// Test method.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<WorldCreature> GetListOfID(int id)
        {
            int count = 3;
            var creatures = new List<WorldCreature>();

            for (int i = 0; i < count; i++)
            {
                foreach (var creature in CreaturesDB)
                {
                    if (creature.ID == id)
                    {
                        creatures.Add(creature.Clone());
                    }
                }
            }

            return creatures;
        }
    }
}
