using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WoW_2D.Gfx;

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
        }

        public override void LoadContent(ContentManager content)
        {
        }

        public override void Update(GameTime gameTime)
        {
            UpdateKeyPress();
        }

        private void UpdateKeyPress()
        {
            KeyboardState keyboardState = Keyboard.GetState();

            IsMovingUp = (keyboardState.IsKeyDown(Keys.W) && !IsMovingDown) ? true : false;
            IsMovingRight = (keyboardState.IsKeyDown(Keys.D) && !IsMovingLeft) ? true : false;
            IsMovingDown = (keyboardState.IsKeyDown(Keys.S) && !IsMovingUp) ? true : false;
            IsMovingLeft = (keyboardState.IsKeyDown(Keys.A) && !IsMovingRight) ? true : false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}
