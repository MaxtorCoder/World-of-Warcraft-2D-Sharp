using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Entity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
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

        public override void Initialize(GraphicsDevice graphics)
        {
            switch (WorldData.Race)
            {
                case Race.Human:
                    SpriteSheet = Global.HumanSpritesheet;
                    break;
            }

            var northAnimation = new Animation() { Name = "north_anim" };
            var eastAnimation = new Animation() { Name = "east_anim" };
            var southAnimation = new Animation() { Name = "south_anim" };
            var westAnimation = new Animation() { Name = "west_anim" };

            northAnimation.AddFrame(SpriteSheet.GetSprite(new Point(1 * 32, 3 * 32), new Point(32)), 175);
            northAnimation.AddFrame(SpriteSheet.GetSprite(new Point(2 * 32, 3 * 32), new Point(32)), 175);
            northAnimation.AddFrame(SpriteSheet.GetSprite(new Point(1 * 32, 3 * 32), new Point(32)), 175);
            northAnimation.AddFrame(SpriteSheet.GetSprite(new Point(0 * 32, 3 * 32), new Point(32)), 175);

            eastAnimation.AddFrame(SpriteSheet.GetSprite(new Point(1 * 32, 2 * 32), new Point(32)), 175);
            eastAnimation.AddFrame(SpriteSheet.GetSprite(new Point(2 * 32, 2 * 32), new Point(32)), 175);
            eastAnimation.AddFrame(SpriteSheet.GetSprite(new Point(1 * 32, 2 * 32), new Point(32)), 175);
            eastAnimation.AddFrame(SpriteSheet.GetSprite(new Point(0 * 32, 2 * 32), new Point(32)), 175);

            southAnimation.AddFrame(SpriteSheet.GetSprite(new Point(1 * 32, 0 * 32), new Point(32)), 175);
            southAnimation.AddFrame(SpriteSheet.GetSprite(new Point(2 * 32, 0 * 32), new Point(32)), 175);
            southAnimation.AddFrame(SpriteSheet.GetSprite(new Point(1 * 32, 0 * 32), new Point(32)), 175);
            southAnimation.AddFrame(SpriteSheet.GetSprite(new Point(0 * 32, 0 * 32), new Point(32)), 175);

            westAnimation.AddFrame(SpriteSheet.GetSprite(new Point(1 * 32, 1 * 32), new Point(32)), 175);
            westAnimation.AddFrame(SpriteSheet.GetSprite(new Point(2 * 32, 1 * 32), new Point(32)), 175);
            westAnimation.AddFrame(SpriteSheet.GetSprite(new Point(1 * 32, 1 * 32), new Point(32)), 175);
            westAnimation.AddFrame(SpriteSheet.GetSprite(new Point(0 * 32, 1 * 32), new Point(32)), 175);

            northAnimation.SetIdleFrame(0);
            eastAnimation.SetIdleFrame(0);
            southAnimation.SetIdleFrame(0);
            westAnimation.SetIdleFrame(0);

            Animations.AddRange(new[] { northAnimation, eastAnimation, southAnimation, westAnimation });
            switch (WorldData.Vector.Direction)
            {
                case MoveDirection.North:
                case MoveDirection.North_East:
                case MoveDirection.North_West:
                    Animations[0].IsActive = true;
                    break;
                case MoveDirection.South:
                case MoveDirection.South_East:
                case MoveDirection.South_West:
                    Animations[2].IsActive = true;
                    break;
                case MoveDirection.East:
                    Animations[1].IsActive = true;
                    break;
                case MoveDirection.West:
                    Animations[3].IsActive = true;
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
