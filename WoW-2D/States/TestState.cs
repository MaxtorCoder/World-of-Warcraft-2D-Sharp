using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameHelper.GameState;
using MonoGameHelper.Utils;
using WoW_2D.GameObject;

namespace WoW_2D.States
{
    /// <summary>
    /// Testing purposes? :P
    /// </summary>
    public class TestState : IGameState
    {
        private PlayerObject player;

        public TestState(GraphicsDevice graphics) : base(graphics) { }

        public override void Initialize()
        {
            player = new PlayerObject(graphics);

            InputHandler.AddKeyPressHandler(ID, delegate () { OnRightArrowPress(); }, Keys.Right);
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            player.LoadContent(content);
        }

        public override void UnloadContent()
        {}

        public override void Update(GameTime gameTime)
        {
            player.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            graphics.Clear(Color.Black);

            player.Draw(spriteBatch);
        }

        #region Event/Key Handlers
        private void OnRightArrowPress()
        {
            GameStateManager.EnterState(1);
        }
        #endregion
    }
}
