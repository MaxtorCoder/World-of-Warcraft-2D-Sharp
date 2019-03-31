using System;
using System.Diagnostics;
using Framework.Network.Packet.Client;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.BitmapFonts;
using MonoGameHelper.GameState;
using MonoGameHelper.Utils;
using WoW_2D.Gfx.Gui;
using WoW_2D.Network;

namespace WoW_2D.States
{
    /// <summary>
    /// A state for the main menu of the game.
    /// </summary>
    public class MainMenuState : IGameState
    {
        private Texture2D background;
        private BitmapFont font;

        private GuiButton loginButton;
        private GuiEntryText usernameText;
        private GuiEntryText passwordText;

        private const string accountLabel = "Account Name";
        private const string passwordLabel = "Account Password";

        private const string blizzardCopyright = "Copyright 2004-2019 Blizzard Entertainment. All Rights Reserved.";
        private const string teamCopyright = "Copyright 2018-2019 SolitudeDevelopment.";

        public MainMenuState(GraphicsDevice graphics) : base(graphics)
        {}

        public override void Initialize()
        {
            loginButton = new GuiButton(graphics) { Text = "Login" };
            loginButton.OnClicked += OnLoginPress;

            usernameText = new GuiEntryText(graphics)
            {
                Width = 210,
                Height = 25,
                BackgroundColor = new Color(0f, 0f, 0f, 0.75f),
                IsActive = true
            };

            passwordText = new GuiEntryText(graphics)
            {
                Width = 210,
                Height = 25,
                BackgroundColor = new Color(0, 0f, 0f, 0.75f),
                MaskCharacter = '*'
            };

            Controls.Add(loginButton);
            Controls.Add(usernameText);
            Controls.Add(passwordText);

            InputHandler.AddKeyPressHandler(ID, delegate () { OnTabPress(); }, Keys.Tab);
            InputHandler.AddKeyPressHandler(ID, delegate () { OnEnterPress(); }, Keys.Enter);
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            background = content.Load<Texture2D>("bg_0");
            font = content.Load<BitmapFont>("System/Font/font");

            loginButton.LoadContent(content);
            loginButton.Position = new Vector2(graphics.Viewport.Width / 2 - loginButton.BaseTexture.Width / 2, graphics.Viewport.Height / 2 + 150);

            usernameText.Position = new Vector2(graphics.Viewport.Width / 2 - usernameText.Width / 2, graphics.Viewport.Height / 2 - usernameText.Height / 2 - 50);
            passwordText.Position = new Vector2(graphics.Viewport.Width / 2 - passwordText.Width / 2, usernameText.Position.Y + 120);

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

            UpdateUsability();
        }

        private void UpdateUsability()
        {
            loginButton.IsEnabled = NetworkManager.State == NetworkManager.NetworkState.Waiting;
            usernameText.IsEnabled = NetworkManager.State == NetworkManager.NetworkState.Waiting;
            passwordText.IsEnabled = NetworkManager.State == NetworkManager.NetworkState.Waiting;
        }

        public override void Draw(GameTime gameTime)
        {
            graphics.Clear(Color.Black);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(background, new Vector2(0f), Color.White);
            DrawUIStrings();
            loginButton.Draw(spriteBatch);
            spriteBatch.End();

            // Render the textfield outside of the last spritebatch call since we begin a new one.
            usernameText.Draw(spriteBatch);
            passwordText.Draw(spriteBatch);

            DrawNetworkUpdates();
        }

        private void DrawUIStrings()
        {
            spriteBatch.DrawString(font, "March 3 2019", new Vector2(0f, graphics.Viewport.Height - font.LineHeight), WorldofWarcraft.DefaultYellow, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(font, WorldofWarcraft.VersionStr, new Vector2(0f, graphics.Viewport.Height - font.LineHeight * 2), WorldofWarcraft.DefaultYellow, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            spriteBatch.DrawString(font, blizzardCopyright, new Vector2(graphics.Viewport.Width / 2, graphics.Viewport.Height - font.LineHeight), WorldofWarcraft.DefaultYellow, 0f, new Vector2(font.MeasureString(blizzardCopyright).Width / 2, 0f), 1f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(font, teamCopyright, new Vector2(graphics.Viewport.Width / 2, graphics.Viewport.Height - (font.LineHeight * 2)), WorldofWarcraft.DefaultYellow, 0f, new Vector2(font.MeasureString(teamCopyright).Width / 2, 0f), 1f, SpriteEffects.None, 0f);

            spriteBatch.DrawString(font, accountLabel, new Vector2(usernameText.Position.X + (usernameText.BaseTexture.Width / 2) + 1, usernameText.Position.Y - (font.LineHeight * 2) + 1), Color.Black, 0f, new Vector2(font.MeasureString(accountLabel).Width / 2, 0f), 1f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(font, accountLabel, new Vector2(usernameText.Position.X + (usernameText.BaseTexture.Width / 2), usernameText.Position.Y - (font.LineHeight * 2)), WorldofWarcraft.DefaultYellow, 0f, new Vector2(font.MeasureString(accountLabel).Width / 2, 0f), 1f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(font, passwordLabel, new Vector2(passwordText.Position.X + (passwordText.BaseTexture.Width / 2) + 1, passwordText.Position.Y - (font.LineHeight * 2) + 1), Color.Black, 0f, new Vector2(font.MeasureString(passwordLabel).Width / 2, 0f), 1f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(font, passwordLabel, new Vector2(passwordText.Position.X + (passwordText.BaseTexture.Width / 2), passwordText.Position.Y - (font.LineHeight * 2)), WorldofWarcraft.DefaultYellow, 0f, new Vector2(font.MeasureString(passwordLabel).Width / 2, 0f), 1f, SpriteEffects.None, 0f);
        }

        private void DrawNetworkUpdates()
        {
            switch (NetworkManager.State)
            {
                case NetworkManager.NetworkState.Connecting:
                    GuiNotification.Draw(font, spriteBatch, "Connecting...");
                    break;
                case NetworkManager.NetworkState.ConnectingFailed:
                    GuiNotification.Draw(font, spriteBatch, "Unable to connect. Please check your realmlist file or contact a developer.", true);
                    break;
                case NetworkManager.NetworkState.Authenticating:
                    GuiNotification.Draw(font, spriteBatch, "Authenticating...");
                    break;
                case NetworkManager.NetworkState.AuthenticatingFailed:
                    GuiNotification.Draw(font, spriteBatch, "Incorrect username/password was entered. Please try again or contact a developer.", true);
                    break;
                case NetworkManager.NetworkState.AuthenticatingUnk:
                    GuiNotification.Draw(font, spriteBatch, "This account could not be found. Please try again or contact a developer.", true);
                    break;
                case NetworkManager.NetworkState.AlreadyLoggedIn:
                    GuiNotification.Draw(font, spriteBatch, "This account is already logged in.", true);
                    break;
                case NetworkManager.NetworkState.RetrievingRealmlist:
                    GuiNotification.Draw(font, spriteBatch, "Retrieving realmlist...");
                    break;
                case NetworkManager.NetworkState.Realmlist:
                    GameStateManager.EnterState(2);
                    break;
                case NetworkManager.NetworkState.ServerError:
                    GuiNotification.Draw(font, spriteBatch, "Server error.", true);
                    break;
            }
        }

        #region Event/Key Handlers
        private void OnLoginPress()
        {
            if (loginButton.IsEnabled)
                NetworkManager.Initialize(usernameText.GetText(), passwordText.GetText());

            usernameText.ResetText();
            passwordText.ResetText();

            usernameText.IsActive = true;
            passwordText.IsActive = false;
        }
        
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
        
        private void OnEnterPress()
        {
            OnLoginPress();
        }
        #endregion
    }
}
