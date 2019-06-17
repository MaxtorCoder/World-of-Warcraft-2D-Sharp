using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Entity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGameHelper.Utils;
using WoW_2D.Gfx;
using WoW_2D.Utils;
using static Framework.Entity.Vector;

namespace WoW_2D.World.GameObject
{
    /// <summary>
    /// The net-player object.
    /// </summary>
    public class PlayerMP : IPlayer
    {
        public bool IsMoving { get; set; }

        public override void Initialize()
        {
            base.Initialize();

            switch (Info.Vector.Direction)
            {
                case MoveDirection.North:
                case MoveDirection.North_East:
                case MoveDirection.North_West:
                    Model.Animations[0].IsActive = true;
                    break;
                case MoveDirection.South:
                case MoveDirection.South_East:
                case MoveDirection.South_West:
                    Model.Animations[2].IsActive = true;
                    break;
                case MoveDirection.East:
                    Model.Animations[1].IsActive = true;
                    break;
                case MoveDirection.West:
                    Model.Animations[3].IsActive = true;
                    break;
            }

            NorthBounds = new RectangleF(0, 0, 16 * 0.6f, 6);
            EastBounds = new RectangleF(0, 0, 6, (16 * 0.6f));
            SouthBounds = new RectangleF(0, 0, 16 * 0.6f, 6);
            WestBounds = new RectangleF(0, 0, 6, (16 * 0.6f));

            BoundingBox = new RectangleF(0, 0, 32 * 0.6f, 32 * 0.6f);
            ColliderRadius = new CircleF(Point2.Zero, 150f);
        }

        public override void Update(GameTime gameTime)
        {
            switch (Info.Vector.Direction)
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
            GetAnimation().Update(gameTime);

            BoundingBox.Position = new Point2(Info.Vector.X, Info.Vector.Y);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            bool idle = (!IsMoving) ? true : false;
            SetAnimationIdle(idle);
            GetAnimation().Draw(spriteBatch, new Vector2(Info.Vector.X, Info.Vector.Y));
        }
    }
}
