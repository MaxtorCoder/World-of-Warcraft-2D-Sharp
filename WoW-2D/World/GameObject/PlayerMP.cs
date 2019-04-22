using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using static Framework.Entity.Vector;

namespace WoW_2D.World.GameObject
{
    /// <summary>
    /// The net-player object.
    /// </summary>
    public class PlayerMP : IPlayer
    {
        public bool IsMoving { get; set; }

        public override void Update(GameTime gameTime)
        {
            switch (WorldData.Vector.Direction)
            {
                case MoveDirection.East:
                    SetAnimation(x => x.Name == "east_anim");
                    break;
                case MoveDirection.West:
                    SetAnimation(x => x.Name == "west_anim");
                    break;
                case MoveDirection.North:
                case MoveDirection.North_East:
                case MoveDirection.North_West:
                    SetAnimation(x => x.Name == "north_anim");
                    break;
                case MoveDirection.South:
                case MoveDirection.South_East:
                case MoveDirection.South_West:
                    SetAnimation(x => x.Name == "south_anim");
                    break;
            }
            Animations.Find(x => x.IsActive).Update(gameTime);

            BoundingBox.Position = new Point2(WorldData.Vector.X, WorldData.Vector.Y);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            bool idle = (!IsMoving) ? true : false;
            SetAnimationIdle(idle);
            Animations.Find(x => x.IsActive).Draw(spriteBatch, new Vector2(WorldData.Vector.X, WorldData.Vector.Y));
        }
    }
}
