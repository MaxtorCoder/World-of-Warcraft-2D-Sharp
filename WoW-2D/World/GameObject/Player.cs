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
        public bool CanMove { get; set; } = true;
        private bool SentStop = true;

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
            if (CanMove)
                UpdateKeyPress();
            UpdateAnimation(gameTime);
            UpdatePosition(gameTime);
            if (!CanMove)
            {
                if (IsMovingUp || IsMovingRight || IsMovingLeft || IsMovingDown)
                {
                    IsMovingUp = false;
                    IsMovingRight = false;
                    IsMovingDown = false;
                    IsMovingLeft = false;
                }
            }
            NorthBounds.Position = new Point2(Info.Vector.X + ((16 * 0.6f) / 2), Info.Vector.Y);
            EastBounds.Position = new Point2(NorthBounds.Position.X + NorthBounds.Width, Info.Vector.Y + ((16 * 0.6f) / 2));
            SouthBounds.Position = new Point2(Info.Vector.X + ((16 * 0.6f) / 2), Info.Vector.Y + (16 * 0.6f) + SouthBounds.Height);
            WestBounds.Position = new Point2(Info.Vector.X, Info.Vector.Y + ((16 * 0.6f) / 2));
            ColliderRadius.Center = new Point2(Info.Vector.X, Info.Vector.Y);
            BoundingBox.Position = new Point2(Info.Vector.X, Info.Vector.Y);
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
                Info.Vector.Direction = MoveDirection.North;

            if (IsMovingRight)
            {
                Info.Vector.Direction = MoveDirection.East;
                SetAnimation(x => x.Name == "east_anim");
            }

            if (IsMovingUp && IsMovingRight)
                Info.Vector.Direction = MoveDirection.North_East;

            if (IsMovingDown)
                Info.Vector.Direction = MoveDirection.South;

            if (IsMovingDown && IsMovingRight)
                Info.Vector.Direction = MoveDirection.South_East;

            if (IsMovingLeft)
            {
                Info.Vector.Direction = MoveDirection.West;
                SetAnimation(x => x.Name == "west_anim");
            }

            if (IsMovingUp && IsMovingLeft)
                Info.Vector.Direction = MoveDirection.North_West;

            if (IsMovingDown && IsMovingLeft)
                Info.Vector.Direction = MoveDirection.South_West;

            if (IsMovingDown || IsMovingDown && IsMovingRight || IsMovingDown && IsMovingLeft)
                SetAnimation(x => x.Name == "south_anim");

            if (IsMovingUp || IsMovingUp && IsMovingRight || IsMovingUp && IsMovingLeft)
                SetAnimation(x => x.Name == "north_anim");

            GetAnimation().Update(gameTime);
        }

        private void UpdatePosition(GameTime gameTime)
        {
            var collisionObjects = WorldofWarcraft.World.ZoneMap.ObjectLayers[1].Objects;

            if (IsMovingUp || IsMovingDown || IsMovingLeft || IsMovingRight)
            {
                float offsetX = Info.Vector.X;
                float offsetY = Info.Vector.Y;

                SentStop = false;
                switch (Info.Vector.Direction)
                {
                    case MoveDirection.North:
                        offsetY -= Info.Stats.WalkSpeed * (1f / gameTime.ElapsedGameTime.Milliseconds);
                        break;
                    case MoveDirection.North_East:
                        offsetY -= Info.Stats.WalkSpeed * (1f / gameTime.ElapsedGameTime.Milliseconds);
                        offsetX += Info.Stats.WalkSpeed * (1f / gameTime.ElapsedGameTime.Milliseconds);
                        break;
                    case MoveDirection.North_West:
                        offsetY -= Info.Stats.WalkSpeed * (1f / gameTime.ElapsedGameTime.Milliseconds);
                        offsetX -= Info.Stats.WalkSpeed * (1f / gameTime.ElapsedGameTime.Milliseconds);
                        break;
                    case MoveDirection.East:
                        offsetX += Info.Stats.WalkSpeed * (1f / gameTime.ElapsedGameTime.Milliseconds);
                        break;
                    case MoveDirection.South:
                        offsetY += Info.Stats.WalkSpeed * (1f / gameTime.ElapsedGameTime.Milliseconds);
                        break;
                    case MoveDirection.South_East:
                        offsetY += Info.Stats.WalkSpeed * (1f / gameTime.ElapsedGameTime.Milliseconds);
                        offsetX += Info.Stats.WalkSpeed * (1f / gameTime.ElapsedGameTime.Milliseconds);
                        break;
                    case MoveDirection.South_West:
                        offsetY += Info.Stats.WalkSpeed * (1f / gameTime.ElapsedGameTime.Milliseconds);
                        offsetX -= Info.Stats.WalkSpeed * (1f / gameTime.ElapsedGameTime.Milliseconds);
                        break;
                    case MoveDirection.West:
                        offsetX -= Info.Stats.WalkSpeed * (1f / gameTime.ElapsedGameTime.Milliseconds);
                        break;
                }

                foreach (var collider in collisionObjects)
                {
                    if (ColliderRadius.Contains(collider.Position.ToPoint()))
                    {
                        var colliderRect = new RectangleF(collider.Position.ToPoint(), collider.Size);
                        if (NorthBounds.Intersects(colliderRect))
                            offsetY += Info.Stats.WalkSpeed * (1f / gameTime.ElapsedGameTime.Milliseconds);

                        if (EastBounds.Intersects(colliderRect))
                            offsetX -= Info.Stats.WalkSpeed * (1f / gameTime.ElapsedGameTime.Milliseconds);

                        if (SouthBounds.Intersects(colliderRect))
                            offsetY -= Info.Stats.WalkSpeed * (1f / gameTime.ElapsedGameTime.Milliseconds);

                        if (WestBounds.Intersects(colliderRect))
                            offsetX += Info.Stats.WalkSpeed * (1f / gameTime.ElapsedGameTime.Milliseconds);
                    }
                }

                Info.Vector.X = offsetX;
                Info.Vector.Y = offsetY;

                NetworkManager.Send(new CMSG_Movement_Update()
                {
                    X = Info.Vector.X,
                    Y = Info.Vector.Y,
                    Direction = Info.Vector.Direction,
                    IsMoving = true
                }, NetworkManager.Direction.World);
            }
            else if (!IsMovingUp && !IsMovingDown && !IsMovingLeft && !IsMovingRight)
            {
                if (!SentStop)
                {
                    NetworkManager.Send(new CMSG_Movement_Update()
                    {
                        X = Info.Vector.X,
                        Y = Info.Vector.Y,
                        Direction = Info.Vector.Direction,
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
            GetAnimation().Draw(spriteBatch, new Vector2(Info.Vector.X, Info.Vector.Y));
        }
    }
}
