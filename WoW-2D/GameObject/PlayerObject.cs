using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WoW_2D.Gfx;
using WoW_2D.Gfx.Animation;

namespace WoW_2D.GameObject
{
    /// <summary>
    /// The local player class.
    /// </summary>
    public class PlayerObject : IPlayerObject
    {
        public PlayerObject(GraphicsDevice Graphics) : base(Graphics) { }

        public override void Initialize()
        {
            SpriteSheet = new SpriteSheet(Graphics);
            AnimationManager = new AnimationManager();
        }

        public override void LoadContent(ContentManager content)
        {
            SpriteSheet.SetTexture(content.Load<Texture2D>("Sprites/Player/Human"));

            /** Create and add animations. **/
            Animation northAnimation = new Animation() { Name = "north_animation" };
            Animation eastAnimation = new Animation() { Name = "east_animation" };
            Animation southAnimation = new Animation() { Name = "south_animation" };
            Animation westAnimation = new Animation() { Name = "west_animation" };

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

            AnimationManager.AddAnimation(northAnimation);
            AnimationManager.AddAnimation(eastAnimation);
            AnimationManager.AddAnimation(southAnimation);
            AnimationManager.AddAnimation(westAnimation);
            AnimationManager.SetActiveAnimation(x => x.Name == "south_animation");
        }

        public override void Update(GameTime gameTime)
        {
            UpdateKeyPress();
            UpdateAnimation(gameTime);
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
            // Debug: This is more or less boiler-plate code. Not really sure how I want to handle it yet.
            // Copy/pasted from the Java edition.
            if (IsMovingUp)
                Direction = MovementDirection.North;

            if (IsMovingRight)
            {
                Direction = MovementDirection.East;
                AnimationManager.SetActiveAnimation(x => x.Name == "east_animation");
            }

            if (IsMovingUp && IsMovingRight)
                Direction = MovementDirection.North_East;

            if (IsMovingDown)
                Direction = MovementDirection.South;

            if (IsMovingDown && IsMovingRight)
                Direction = MovementDirection.South_East;

            if (IsMovingLeft)
            {
                Direction = MovementDirection.West;
                AnimationManager.SetActiveAnimation(x => x.Name == "west_animation");
            }

            if (IsMovingUp && IsMovingLeft)
                Direction = MovementDirection.North_West;

            if (IsMovingDown && IsMovingLeft)
                Direction = MovementDirection.South_West;

            if (IsMovingDown || IsMovingDown && IsMovingRight || IsMovingDown && IsMovingLeft)
                AnimationManager.SetActiveAnimation(x => x.Name == "south_animation");

            if (IsMovingUp || IsMovingUp && IsMovingRight || IsMovingUp && IsMovingLeft)
                AnimationManager.SetActiveAnimation(x => x.Name == "north_animation");

            AnimationManager.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            bool idle = (!IsMovingUp && !IsMovingDown && !IsMovingLeft && !IsMovingRight) ? true : false;
            AnimationManager.SetIdle(idle);
            AnimationManager.Draw(spriteBatch, Position);
        }
    }
}
