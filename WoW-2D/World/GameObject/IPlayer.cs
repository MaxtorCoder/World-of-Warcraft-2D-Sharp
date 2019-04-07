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
    }
}
