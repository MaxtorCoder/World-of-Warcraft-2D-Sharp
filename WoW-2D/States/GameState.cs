using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGameHelper.GameState;
using MonoGameHelper.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoW_2D.Gfx.Gui;
using WoW_2D.Gfx.Gui.Ui;

namespace WoW_2D.States
{
    /// <summary>
    /// The game state.
    /// </summary>
    public class GameState : IGameState
    {
        private EscapeMenuUI escapeMenu;
        private ChatUI chatUi;

        public GameState(GraphicsDevice graphics) : base(graphics) { }

        public override void Initialize()
        {
            escapeMenu = new EscapeMenuUI(graphics);
            chatUi = new ChatUI(graphics);
            Controls.Add(chatUi.TextField);

            InputHandler.AddKeyPressHandler(ID, delegate () { OnEscapePressed(); }, Keys.Escape);
            InputHandler.AddKeyPressHandler(ID, delegate () { OnEnterPressed(); }, Keys.Enter);
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            escapeMenu.LoadContent(content);
            chatUi.LoadContent(content);
        }

        public override void UnloadContent()
        {}

        public override void Update(GameTime gameTime)
        {
            WorldofWarcraft.Map.Update(gameTime);

            escapeMenu.Update();
            chatUi.Update();
        }

        public override void Draw(GameTime gameTime)
        {
            WorldofWarcraft.Map.Draw(spriteBatch, gameTime);

            escapeMenu.Draw(spriteBatch);
            chatUi.Draw(spriteBatch);
        }

        private void OnEscapePressed() => escapeMenu.IsVisible = !escapeMenu.IsVisible;
        private void OnEnterPressed() => chatUi.OnEnterPressed();
    }
}
