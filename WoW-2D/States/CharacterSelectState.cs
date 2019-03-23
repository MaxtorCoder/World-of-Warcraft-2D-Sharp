using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using MonoGameHelper.GameState;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoW_2D.Gfx.Gui;
using WoW_2D.Network;

namespace WoW_2D.States
{
    /// <summary>
    /// The character-select state.
    /// </summary>
    public class CharacterSelectState : IGameState
    {
        private BitmapFont font;
        private RectangleF listRect;

        private GuiButton enterWorldButton;
        private GuiButton createCharacterButton;
        private GuiButton deleteCharacterButton;
        private GuiButton backButton;

        public CharacterSelectState(GraphicsDevice graphics) : base(graphics) { }

        public override void Initialize()
        {
            listRect = new RectangleF(0, 0, 225, graphics.Viewport.Height - 145);
            listRect.Position = new Point2(graphics.Viewport.Width - listRect.Width - 25, 45);

            enterWorldButton = new GuiButton(graphics) { Text = "Enter World" };
            enterWorldButton.OnClicked += OnEnterWorldPress;

            createCharacterButton = new GuiButton(graphics) { Text = "Create Character" };
            createCharacterButton.OnClicked += OnCreateCharacterPress;

            deleteCharacterButton = new GuiButton(graphics) { Text = "Delete Character" };
            deleteCharacterButton.OnClicked += OnDeleteCharacterPress;

            backButton = new GuiButton(graphics) { Text = "Back" };
            backButton.OnClicked += OnBackPress;

            Controls.Add(enterWorldButton);
            Controls.Add(createCharacterButton);
            Controls.Add(deleteCharacterButton);
            Controls.Add(backButton);
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            font = content.Load<BitmapFont>("System/Font/font");

            enterWorldButton.LoadContent(content);
            enterWorldButton.Position = new Vector2(graphics.Viewport.Width / 2 - enterWorldButton.BaseTexture.Width / 2, graphics.Viewport.Height - enterWorldButton.BaseTexture.Height - 15);
            enterWorldButton.IsEnabled = false;

            createCharacterButton.LoadContent(content);
            createCharacterButton.Position = new Vector2(listRect.Position.X + (listRect.Width / 2 - createCharacterButton.BaseTexture.Width / 2), listRect.Position.Y + listRect.Height - createCharacterButton.BaseTexture.Height - 15);

            deleteCharacterButton.LoadContent(content);
            deleteCharacterButton.Position = new Vector2(createCharacterButton.Position.X, listRect.Y + (listRect.Height + 15));
            deleteCharacterButton.IsEnabled = false;

            backButton.LoadContent(content);
            backButton.Position = new Vector2(deleteCharacterButton.Position.X, enterWorldButton.Position.Y);
        }

        public override void UnloadContent()
        {
        }

        public override void Update(GameTime gameTime)
        {
            enterWorldButton.Update();
            createCharacterButton.Update();
            deleteCharacterButton.Update();
            backButton.Update();
        }

        public override void Draw(GameTime gameTime)
        {
            graphics.Clear(Color.Black);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.DrawRectangle(listRect, Color.Gray, 2f);
            spriteBatch.DrawString(font, WorldofWarcraft.ConnectedRealm.Name, new Vector2(listRect.Position.X + (listRect.Width / 2 - font.MeasureString(WorldofWarcraft.ConnectedRealm.Name).Width / 2), listRect.Position.Y + 15), Color.LightGray);
            enterWorldButton.Draw(spriteBatch);
            createCharacterButton.Draw(spriteBatch);
            deleteCharacterButton.Draw(spriteBatch);
            backButton.Draw(spriteBatch);
            spriteBatch.End();
        }

        private void OnEnterWorldPress()
        {

        }

        private void OnCreateCharacterPress()
        {
            GameStateManager.EnterState(3);
        }

        private void OnDeleteCharacterPress()
        {

        }

        private void OnBackPress()
        {
            NetworkManager.Disconnect(NetworkManager.Direction.Auth);
            GameStateManager.EnterState(1);
        }
    }
}
