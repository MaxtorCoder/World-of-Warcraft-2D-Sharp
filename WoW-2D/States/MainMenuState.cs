using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WoW_2D.Gfx.Gui;
using WoW_2D.Utils;

namespace WoW_2D.States
{
    /// <summary>
    /// A state for the main menu of the game.
    /// </summary>
    public class MainMenuState : IGameState
    {
        private Texture2D background;
        private SpriteFont font;

        private GuiButton loginButton;
        private GuiEntryText usernameText;
        private GuiEntryText passwordText;

        private const string accountLabel = "Account Name";
        private const string passwordLabel = "Account Password";

        private const string blizzardCopyright = "Copyright 2004-2019 Blizzard Entertainment. All Rights Reserved.";
        private const string teamCopyright = "Copyright 2018-2019 SolitudeDevelopment.";

        public MainMenuState(WorldofWarcraft wow, GraphicsDevice graphics) : base(wow, graphics)
        {}

        public override void Initialize()
        {
            loginButton = new GuiButton(Graphics) { Text = "Login" };
            loginButton.OnClicked += OnLoginPress;

            usernameText = new GuiEntryText(Graphics)
            {
                Width = 210,
                Height = 25,
                BackgroundColor = new Color(0f, 0f, 0f, 0.75f),
                IsActive = true
            };

            passwordText = new GuiEntryText(Graphics)
            {
                Width = 210,
                Height = 25,
                BackgroundColor = new Color(0, 0f, 0f, 0.75f),
                MaskCharacter = '*'
            };

            Controls.Add(loginButton);
            Controls.Add(usernameText);
            Controls.Add(passwordText);

            InputHandler.AddKeyPressHandler(delegate () { OnTabPress(); }, Keys.Tab);
        }

        public override void LoadContent(ContentManager content)
        {
            background = content.Load<Texture2D>("bg_0");
            font = content.Load<SpriteFont>("System/font");

            loginButton.LoadContent(content);
            loginButton.Position = new Vector2(WorldofWarcraft.Window.ClientBounds.Width / 2 - loginButton.BaseTexture.Width / 2, WorldofWarcraft.Window.ClientBounds.Height / 2 + 150);
            loginButton.IsEnabled = true;

            usernameText.Position = new Vector2(WorldofWarcraft.Window.ClientBounds.Width / 2 - usernameText.Width / 2, WorldofWarcraft.Window.ClientBounds.Height / 2 - usernameText.Height / 2);
            passwordText.Position = new Vector2(WorldofWarcraft.Window.ClientBounds.Width / 2 - passwordText.Width / 2, usernameText.Position.Y + 95);

            usernameText.LoadContent(content);
            passwordText.LoadContent(content);
        }

        public override void UnloadContent()
        {}

        public override void Update(GameTime gameTime)
        {
            loginButton.Update();
            usernameText.Update();
            passwordText.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Graphics.Clear(Color.Black);

            spriteBatch.Begin();
            spriteBatch.Draw(background, new Vector2(0f), Color.White);
            DrawUIStrings(spriteBatch);
            loginButton.Draw(spriteBatch);
            spriteBatch.End();

            // Render the textfield outside of the last spritebatch call since we begin a new one.
            usernameText.Draw(spriteBatch);
            passwordText.Draw(spriteBatch);
        }

        private void DrawUIStrings(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, "March 3 2019", new Vector2(0f, WorldofWarcraft.Window.ClientBounds.Height - font.LineSpacing / 2), WorldofWarcraft.DefaultYellow, 0f, new Vector2(0f, 0f), 0.5f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(font, "v0.1.0a", new Vector2(0f, WorldofWarcraft.Window.ClientBounds.Height - font.LineSpacing), WorldofWarcraft.DefaultYellow, 0f, new Vector2(0f, 0f), 0.5f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(font, blizzardCopyright, new Vector2(WorldofWarcraft.Window.ClientBounds.Width / 2, WorldofWarcraft.Window.ClientBounds.Height - (font.LineSpacing * 0.5f) / 2), WorldofWarcraft.DefaultYellow, 0f, font.MeasureString(blizzardCopyright) / 2, 0.5f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(font, teamCopyright, new Vector2(WorldofWarcraft.Window.ClientBounds.Width / 2, WorldofWarcraft.Window.ClientBounds.Height - (font.LineSpacing * 0.5f) * 1.5f), WorldofWarcraft.DefaultYellow, 0f, font.MeasureString(teamCopyright) / 2, 0.5f, SpriteEffects.None, 0f);

            spriteBatch.DrawString(font, accountLabel, new Vector2(WorldofWarcraft.Window.ClientBounds.Width / 2 + 1, (usernameText.Position.Y + 10) + 1), Color.Black, 0f, new Vector2(font.MeasureString(accountLabel).X / 2), 0.5f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(font, accountLabel, new Vector2(WorldofWarcraft.Window.ClientBounds.Width / 2, usernameText.Position.Y + 10), WorldofWarcraft.DefaultYellow, 0f, new Vector2(font.MeasureString(accountLabel).X / 2), 0.5f, SpriteEffects.None, 0f);

            spriteBatch.DrawString(font, passwordLabel, new Vector2(WorldofWarcraft.Window.ClientBounds.Width / 2 + 1, (passwordText.Position.Y + 20) + 1), Color.Black, 0f, new Vector2(font.MeasureString(passwordLabel).X / 2), 0.5f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(font, passwordLabel, new Vector2(WorldofWarcraft.Window.ClientBounds.Width / 2, passwordText.Position.Y + 20), WorldofWarcraft.DefaultYellow, 0f, new Vector2(font.MeasureString(passwordLabel).X / 2), 0.5f, SpriteEffects.None, 0f);
        }

        /// <summary>
        /// Fired when the login button has been pressed.
        /// </summary>
        private void OnLoginPress()
        {
            Debug.WriteLine($"Username: {usernameText.GetText()}");
            Debug.WriteLine($"Password: {passwordText.GetText()}");
            loginButton.IsEnabled = false;
        }

        /// <summary>
        /// Used to switch between the textfields by pressing 'Tab'.
        /// </summary>
        private void OnTabPress()
        {
            if (usernameText.IsActive)
            {
                usernameText.IsActive = false;
                passwordText.IsActive = true;
            }
            else if (passwordText.IsActive)
            {
                passwordText.IsActive = false;
                usernameText.IsActive = true;
            }
        }
    }
}
