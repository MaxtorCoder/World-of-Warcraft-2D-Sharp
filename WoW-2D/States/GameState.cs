using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using MonoGameHelper.GameState;
using MonoGameHelper.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoW_2D.Gfx;
using WoW_2D.Gfx.Gui;
using WoW_2D.Gfx.Gui.Ui;
using WoW_2D.Network;
using WoW_2D.Utils;

namespace WoW_2D.States
{
    /// <summary>
    /// The game state.
    /// </summary>
    public class GameState : IGameState
    {
        private EscapeMenuUI escapeMenu;
        private ChatUI chatUi;
        private HotbarUI hotbar;
        private UnitFrameUI localFrame;

        public GameState(GraphicsDevice graphics) : base(graphics) { }

        public override void Initialize()
        {
            escapeMenu = new EscapeMenuUI(graphics);
            chatUi = new ChatUI(graphics);
            hotbar = new HotbarUI(graphics);
            localFrame = new UnitFrameUI(graphics, false);

            Controls.Add(chatUi.TextField);

            InputHandler.AddKeyPressHandler(ID, delegate () { OnEscapePressed(); }, Keys.Escape);
            InputHandler.AddKeyPressHandler(ID, delegate () { OnEnterPressed(); }, Keys.Enter);
            InputHandler.AddKeyPressHandler(ID, delegate () { OnTabPress(); }, Keys.Tab);
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
            WorldofWarcraft.World.Update(gameTime);
            escapeMenu.Update();
            chatUi.Update();
            localFrame.Update();
        }

        public override void Draw(GameTime gameTime)
        {
            graphics.Clear(Color.Black);

            WorldofWarcraft.World.Draw(spriteBatch, gameTime);
            escapeMenu.Draw(spriteBatch);
            chatUi.Draw(spriteBatch);
            hotbar.Draw(spriteBatch);
            localFrame.Draw(spriteBatch);
        }

        private void OnEscapePressed() => escapeMenu.Activator();
        private void OnEnterPressed() => chatUi.OnEnterPressed();
        private void OnTabPress() => Global.FullscreenFlag = 1;
    }
}
