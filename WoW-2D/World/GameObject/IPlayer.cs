using Framework.Entity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Framework.Entity.Vector;

namespace WoW_2D.World.GameObject
{
    /// <summary>
    /// An abstract class for player-types.
    /// </summary>
    public abstract class IPlayer : IMob
    {
        protected WorldCharacter WorldData;
        public bool IsMovingUp, IsMovingDown, IsMovingLeft, IsMovingRight;
        protected float speed = 8.5f;

        public override void Update(GameTime gameTime)
        {
            if (IsMovingUp)
                WorldData.Vector.Direction = MoveDirection.North;

            if (IsMovingRight)
            {
                WorldData.Vector.Direction = MoveDirection.East;
                SetAnimation(x => x.Name == "east_anim");
            }

            if (IsMovingUp && IsMovingRight)
                WorldData.Vector.Direction = MoveDirection.North_East;

            if (IsMovingDown)
                WorldData.Vector.Direction = MoveDirection.South;

            if (IsMovingDown && IsMovingRight)
                WorldData.Vector.Direction = MoveDirection.South_East;

            if (IsMovingLeft)
            {
                WorldData.Vector.Direction = MoveDirection.West;
                SetAnimation(x => x.Name == "west_anim");
            }

            if (IsMovingUp && IsMovingLeft)
                WorldData.Vector.Direction = MoveDirection.North_West;

            if (IsMovingDown && IsMovingLeft)
                WorldData.Vector.Direction = MoveDirection.South_West;

            if (IsMovingDown || IsMovingDown && IsMovingRight || IsMovingDown && IsMovingLeft)
                SetAnimation(x => x.Name == "south_anim");

            if (IsMovingUp || IsMovingUp && IsMovingRight || IsMovingUp && IsMovingLeft)
                SetAnimation(x => x.Name == "north_anim");
        }
    }
}
