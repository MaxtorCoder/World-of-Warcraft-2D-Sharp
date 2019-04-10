using Framework.Entity;
using Framework.Network.Packet.Client;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGameHelper.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoW_2D.Gfx;
using WoW_2D.Network;
using WoW_2D.Utils;
using static Framework.Entity.Vector;

namespace WoW_2D.World.GameObject
{
    /// <summary>
    /// The local player object.
    /// </summary>
    public class Player : IPlayer
    {
        private Camera2D camera;

        public Player() => WorldData = WorldofWarcraft.Character;

        public override void Initialize(GraphicsDevice graphics)
        {
            camera = new Camera2D(graphics);
            camera.ZoomIn(0.5f);
        }

        public override void LoadContent(ContentManager content)
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
        }

        public override void Update(GameTime gameTime)
        {
            UpdateKeyPress();
            UpdateAnimation(gameTime);
            UpdatePosition(gameTime);
            camera.LookAt(new Vector2(WorldData.Vector.X + 8, WorldData.Vector.Y + 8));
        }

        private void UpdateKeyPress()
        {
            KeyboardState keyboardState = Keyboard.GetState();

            IsMovingUp = (keyboardState.IsKeyDown(Keys.W) && !IsMovingDown) ? true : false;
            IsMovingRight = (keyboardState.IsKeyDown(Keys.D) && !IsMovingLeft) ? true : false;
            IsMovingDown = (keyboardState.IsKeyDown(Keys.S) && !IsMovingUp) ? true : false;
            IsMovingLeft = (keyboardState.IsKeyDown(Keys.A) && !IsMovingRight) ? true : false;
        }

        private void UpdateAnimation(GameTime gameTime)
        {
            base.Update(gameTime);

            Animations.Find(x => x.IsActive).Update(gameTime);
        }

        private void UpdatePosition(GameTime gameTime)
        {
            if (IsMovingUp || IsMovingDown || IsMovingLeft || IsMovingRight)
            {
                switch (WorldData.Vector.Direction)
                {
                    case MoveDirection.North:
                        WorldData.Vector.Y -= speed * (1f / gameTime.ElapsedGameTime.Milliseconds);
                        break;
                    case MoveDirection.North_East:
                        WorldData.Vector.Y -= speed * (1f / gameTime.ElapsedGameTime.Milliseconds);
                        WorldData.Vector.X += speed * (1f / gameTime.ElapsedGameTime.Milliseconds);
                        break;
                    case MoveDirection.North_West:
                        WorldData.Vector.Y -= speed * (1f / gameTime.ElapsedGameTime.Milliseconds);
                        WorldData.Vector.X -= speed * (1f / gameTime.ElapsedGameTime.Milliseconds);
                        break;
                    case MoveDirection.East:
                        WorldData.Vector.X += speed * (1f / gameTime.ElapsedGameTime.Milliseconds);
                        break;
                    case MoveDirection.South:
                        WorldData.Vector.Y += speed * (1f / gameTime.ElapsedGameTime.Milliseconds);
                        break;
                    case MoveDirection.South_East:
                        WorldData.Vector.Y += speed * (1f / gameTime.ElapsedGameTime.Milliseconds);
                        WorldData.Vector.X += speed * (1f / gameTime.ElapsedGameTime.Milliseconds);
                        break;
                    case MoveDirection.South_West:
                        WorldData.Vector.Y += speed * (1f / gameTime.ElapsedGameTime.Milliseconds);
                        WorldData.Vector.X -= speed * (1f / gameTime.ElapsedGameTime.Milliseconds);
                        break;
                    case MoveDirection.West:
                        WorldData.Vector.X -= speed * (1f / gameTime.ElapsedGameTime.Milliseconds);
                        break;
                }

                NetworkManager.Send(new CMSG_Movement_Update()
                {
                    X = WorldData.Vector.X,
                    Y = WorldData.Vector.Y,
                    Direction = WorldData.Vector.Direction
                }, NetworkManager.Direction.World);
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            bool idle = (!IsMovingUp && !IsMovingDown && !IsMovingLeft && !IsMovingRight) ? true : false;
            SetAnimationIdle(idle);
            Animations.Find(x => x.IsActive).Draw(spriteBatch, new Vector2(WorldData.Vector.X, WorldData.Vector.Y));
        }

        public Camera2D GetCamera()
        {
            return camera;
        }
    }
}
