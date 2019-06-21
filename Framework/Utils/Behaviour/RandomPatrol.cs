using Framework.Entity;
using Framework.Network.Packet.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Framework.Entity.Vector;

namespace Framework.Utils.Behaviour
{
    /// <summary>
    /// The random-movement/patrol behaviour.
    /// </summary>
    public class RandomPatrol : IPatrol
    {
        public int WaitTimer, MoveTimer;

        private bool isRunning = false;
        private bool shouldMove = false;
        private bool shouldIdle = true;
        private bool isRandomDirectionSet = false;
        private bool hasSentStop = true;

        private long lastTime;
        private int i = 0;
        private MoveDirection movementDirection;

        private Random random;

        public override void Start()
        {
            if (isRunning)
                return;

            isRunning = true;
            lastTime = Environment.TickCount;
            random = new Random();
        }

        public override void Update(ServerCreature creature)
        {
            if (isRunning)
            {
                if ((Environment.TickCount - lastTime) >= 1000)
                {
                    lastTime += 1000;
                    i += 1000;
                }

                if (shouldIdle)
                {
                    if (!hasSentStop)
                    {
                        var players = WorldManager.GetAllPlayers();
                        if (players != null)
                        {
                            foreach (var player in players)
                            {
                                player.Send(new SMSG_Creature()
                                {
                                    Creature = new ClientCreature()
                                    {
                                        GUID = creature.GUID,
                                        Vector = creature.Vector
                                    },
                                    State = SMSG_Creature.CreatureState.MoveStop
                                });
                            }
                            hasSentStop = true;
                        }
                    }
                    if (isRandomDirectionSet) isRandomDirectionSet = false;
                    if (i >= WaitTimer * 1000)
                    {
                        i = 0;
                        shouldIdle = false;
                        shouldMove = true;
                    }
                }

                if (shouldMove)
                {
                    if (hasSentStop)
                        hasSentStop = false;
                    if (!isRandomDirectionSet)
                    {
                        movementDirection = (MoveDirection)random.Next(8);
                        isRandomDirectionSet = true;
                    }

                    MoveCreature(ref creature);
                    var players = WorldManager.GetAllPlayers();
                    if (players != null)
                    {
                        foreach (var player in players)
                        {
                            player.Send(new SMSG_Creature()
                            {
                                Creature = new ClientCreature()
                                {
                                    GUID = creature.GUID,
                                    Vector = creature.Vector
                                },
                                State = SMSG_Creature.CreatureState.Move
                            });
                        }
                    }

                    if (i >= MoveTimer * 1000)
                    {
                        i = 0;
                        shouldMove = false;
                        shouldIdle = true;
                    }
                }
            }
        }

        private void MoveCreature(ref ServerCreature creature)
        {
            switch (movementDirection)
            {
                case MoveDirection.East:
                    creature.Vector.X += 0.05f;
                    break;
                case MoveDirection.West:
                    creature.Vector.X -= 0.05f;
                    break;
                case MoveDirection.North:
                    creature.Vector.Y -= 0.05f;
                    break;
                case MoveDirection.North_East:
                    creature.Vector.Y -= 0.05f;
                    creature.Vector.X += 0.05f;
                    break;
                case MoveDirection.North_West:
                    creature.Vector.Y -= 0.05f;
                    creature.Vector.X -= 0.05f;
                    break;
                case MoveDirection.South:
                    creature.Vector.Y += 0.05f;
                    break;
                case MoveDirection.South_East:
                    creature.Vector.Y += 0.05f;
                    creature.Vector.X += 0.05f;
                    break;
                case MoveDirection.South_West:
                    creature.Vector.Y += 0.05f;
                    creature.Vector.X -= 0.05f;
                    break;
            }
            if (creature.Vector.Direction != movementDirection)
                creature.Vector.Direction = movementDirection;
        }
    }
}
