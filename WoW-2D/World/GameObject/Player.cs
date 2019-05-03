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
        public bool CanMove { get; set; } = true;
        private bool SentStop = true;

        public Player() => WorldData = WorldofWarcraft.Character;

        public override void Initialize(GraphicsDevice graphics)
        {
            camera = new Camera2D(graphics);
            camera.ZoomIn(0.5f);
        }

        public override void Update(GameTime gameTime)
        {
            if (CanMove)
                UpdateKeyPress();
            UpdateAnimation(gameTime);
            UpdatePosition(gameTime);
            camera.LookAt(new Vector2(WorldData.Vector.X + 8, WorldData.Vector.Y + 8));

            if (!CanMove)
                if (IsMovingUp || IsMovingRight || IsMovingLeft || IsMovingDown)
                {
                    IsMovingUp = false;
                    IsMovingRight = false;
                    IsMovingDown = false;
                    IsMovingLeft = false;
                }

            NorthBounds.Position = new Point2(WorldData.Vector.X + ((16 * 0.6f) / 2), WorldData.Vector.Y);
            EastBounds.Position = new Point2(NorthBounds.Position.X + NorthBounds.Width, WorldData.Vector.Y + ((16 * 0.6f) / 2));
            SouthBounds.Position = new Point2(WorldData.Vector.X + ((16 * 0.6f) / 2), WorldData.Vector.Y + (16 * 0.6f) + SouthBounds.Height);
            WestBounds.Position = new Point2(WorldData.Vector.X, WorldData.Vector.Y + ((16 * 0.6f) / 2));
            ColliderRadius.Center = new Point2(WorldData.Vector.X, WorldData.Vector.Y);
            BoundingBox.Position = new Point2(WorldData.Vector.X, WorldData.Vector.Y);
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

            Animations.Find(x => x.IsActive).Update(gameTime);
        }

        private void UpdatePosition(GameTime gameTime)
        {
            var collisionObjects = WorldofWarcraft.Map.ZoneMap.ObjectLayers[1].Objects;

            if (IsMovingUp || IsMovingDown || IsMovingLeft || IsMovingRight)
            {
                float offsetX = WorldData.Vector.X;
                float offsetY = WorldData.Vector.Y;

                SentStop = false;
                switch (WorldData.Vector.Direction)
                {
                    case MoveDirection.North:
                        offsetY -= speed * (1f / gameTime.ElapsedGameTime.Milliseconds);
                        break;
                    case MoveDirection.North_East:
                        offsetY -= speed * (1f / gameTime.ElapsedGameTime.Milliseconds);
                        offsetX += speed * (1f / gameTime.ElapsedGameTime.Milliseconds);
                        break;
                    case MoveDirection.North_West:
                        offsetY -= speed * (1f / gameTime.ElapsedGameTime.Milliseconds);
                        offsetX -= speed * (1f / gameTime.ElapsedGameTime.Milliseconds);
                        break;
                    case MoveDirection.East:
                        offsetX += speed * (1f / gameTime.ElapsedGameTime.Milliseconds);
                        break;
                    case MoveDirection.South:
                        offsetY += speed * (1f / gameTime.ElapsedGameTime.Milliseconds);
                        break;
                    case MoveDirection.South_East:
                        offsetY += speed * (1f / gameTime.ElapsedGameTime.Milliseconds);
                        offsetX += speed * (1f / gameTime.ElapsedGameTime.Milliseconds);
                        break;
                    case MoveDirection.South_West:
                        offsetY += speed * (1f / gameTime.ElapsedGameTime.Milliseconds);
                        offsetX -= speed * (1f / gameTime.ElapsedGameTime.Milliseconds);
                        break;
                    case MoveDirection.West:
                        offsetX -= speed * (1f / gameTime.ElapsedGameTime.Milliseconds);
                        break;
                }

                foreach (var collider in collisionObjects)
                {
                    if (ColliderRadius.Contains(collider.Position.ToPoint()))
                    {
                        var colliderRect = new RectangleF(collider.Position.ToPoint(), collider.Size);
                        if (NorthBounds.Intersects(colliderRect))
                            offsetY += speed * (1f / gameTime.ElapsedGameTime.Milliseconds);

                        if (EastBounds.Intersects(colliderRect))
                            offsetX -= speed * (1f / gameTime.ElapsedGameTime.Milliseconds);

                        if (SouthBounds.Intersects(colliderRect))
                            offsetY -= speed * (1f / gameTime.ElapsedGameTime.Milliseconds);

                        if (WestBounds.Intersects(colliderRect))
                            offsetX += speed * (1f / gameTime.ElapsedGameTime.Milliseconds);
                    }
                }

                WorldData.Vector.X = offsetX;
                WorldData.Vector.Y = offsetY;

                NetworkManager.Send(new CMSG_Movement_Update()
                {
                    X = WorldData.Vector.X,
                    Y = WorldData.Vector.Y,
                    Direction = WorldData.Vector.Direction,
                    IsMoving = true
                }, NetworkManager.Direction.World);
            }
            else if (!IsMovingUp && !IsMovingDown && !IsMovingLeft && !IsMovingRight)
            {
                if (!SentStop)
                {
                    NetworkManager.Send(new CMSG_Movement_Update()
                    {
                        X = WorldData.Vector.X,
                        Y = WorldData.Vector.Y,
                        Direction = WorldData.Vector.Direction,
                        IsMoving = false
                    }, NetworkManager.Direction.World);
                    SentStop = true;
                }
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
