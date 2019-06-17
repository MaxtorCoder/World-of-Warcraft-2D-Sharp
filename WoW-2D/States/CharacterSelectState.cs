using DiscordRPC;
using Framework.Entity;
using Framework.Network.Packet.Client;
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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoW_2D.Gfx;
using WoW_2D.Gfx.Gui;
using WoW_2D.Network;
using WoW_2D.Utils;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace WoW_2D.States
{
    /// <summary>
    /// The character-select state.
    /// </summary>
    public class CharacterSelectState : IGameState
    {
        private RectangleF listRect;
        private RectangleF[] characterBoxes;
        private int characterIndex = -1;

        private GuiButton enterWorldButton;
        private GuiButton createCharacterButton;
        private GuiButton deleteCharacterButton;
        private GuiButton backButton;

        private RealmCharacter realmCharacter;
        private Texture2D humanTexture;

        public CharacterSelectState(GraphicsDevice graphics) : base(graphics) { }

        public override void Initialize()
        {
            listRect = new RectangleF(0, 0, 225, graphics.Viewport.Height - 145);
            listRect.Position = new Point2(graphics.Viewport.Width - listRect.Width - 25, 45);

            characterBoxes = new RectangleF[7];
            for (int i = 0; i < characterBoxes.Length; i++)
            {
                characterBoxes[i] = new RectangleF(0, 0, 175, 50);
                if (i == 0)
                    characterBoxes[0].Position = new Point2(listRect.Position.X + (listRect.Width / 2 - characterBoxes[0].Width / 2), listRect.Position.Y + 45);
                else
                {
                    var lastBox = characterBoxes[i-1];
                    characterBoxes[i].Position = new Point2(listRect.Position.X + (listRect.Width / 2 - lastBox.Width / 2), lastBox.Position.Y + (lastBox.Height + 20));
                }
            }

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

            InputHandler.AddKeyPressHandler(ID, delegate () { OnEnterPressed(); }, Keys.Enter);
            InputHandler.AddKeyPressHandler(ID, delegate () { OnDownPressed(); }, Keys.Down);
            InputHandler.AddKeyPressHandler(ID, delegate () { OnUpPressed(); }, Keys.Up);
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            humanTexture = Global.HumanSpritesheet.GetSprite(new Point(32, 0), new Point(32));

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
            createCharacterButton.IsEnabled = (WorldofWarcraft.RealmCharacters.Count != 7) ? true : false;
            deleteCharacterButton.IsEnabled = (characterIndex > -1) ? true : false;
            enterWorldButton.IsEnabled = (characterIndex > -1) ? true : false;

            enterWorldButton.Update();
            createCharacterButton.Update();
            deleteCharacterButton.Update();
            backButton.Update();

            if (WorldofWarcraft.RealmCharacters.Count > 0)
            {
                if (characterIndex == -1 || characterIndex < 0 || characterIndex > WorldofWarcraft.RealmCharacters.Count)
                    characterIndex = 0;
            }
            else
                characterIndex = -1;

            try
            {
                if (characterIndex > -1)
                    realmCharacter = WorldofWarcraft.RealmCharacters[characterIndex];
            }
            catch { characterIndex = 0; }

            for (int i = 0; i < characterBoxes.Length; i++)
            {
                if (characterBoxes[i].Contains(Mouse.GetState().Position))
                {
                    if (InputHandler.IsMouseButtonPressed(InputHandler.MouseButton.LeftButton))
                    {
                        try
                        {
                            realmCharacter = WorldofWarcraft.RealmCharacters[i];
                            if (realmCharacter != null)
                                characterIndex = i;
                        }
                        catch { }
                    }
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            graphics.Clear(Color.Black);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(GfxManager.GetTexture("logo"), Vector2.Zero, Color.White);
            spriteBatch.DrawRectangle(listRect, Color.Gray, 2f);
            spriteBatch.DrawString(GfxManager.GetFont("main_font"), WorldofWarcraft.Realm.Name, new Vector2(listRect.Position.X + (listRect.Width / 2 - GfxManager.GetFont("main_font").MeasureString(WorldofWarcraft.Realm.Name).Width / 2), listRect.Position.Y + 15), Color.LightGray);
            enterWorldButton.Draw(spriteBatch);
            createCharacterButton.Draw(spriteBatch);
            deleteCharacterButton.Draw(spriteBatch);
            backButton.Draw(spriteBatch);
            try
            {
                if (characterIndex > -1)
                {
                    switch (realmCharacter.Race)
                    {
                        case Race.Human:
                            spriteBatch.Draw(humanTexture, new Vector2(graphics.Viewport.Width / 2 - humanTexture.Width / 2, graphics.Viewport.Height / 2 - humanTexture.Height / 2), null, Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
                            break;
                    }
                }
            }
            catch { }
            spriteBatch.End();

            if (NetworkManager.State == NetworkManager.NetworkState.Waiting)
            {
                spriteBatch.Begin(blendState: BlendState.NonPremultiplied);
                if (characterIndex > -1)
                {
                    spriteBatch.FillRectangle(characterBoxes[characterIndex], new Color(223, 195, 15, 120));
                    spriteBatch.DrawRectangle(characterBoxes[characterIndex], new Color(223, 195, 15));
                }
                spriteBatch.End();

                spriteBatch.Begin();
                for (int i = 0; i < WorldofWarcraft.RealmCharacters.Count; i++)
                {
                    var realmCharacter = WorldofWarcraft.RealmCharacters[i];
                    var box = characterBoxes[i];

                    spriteBatch.DrawString(GfxManager.GetFont("main_font"), realmCharacter.Name, new Vector2(box.Position.X + 5, box.Position.Y), Color.Black);
                    spriteBatch.DrawString(GfxManager.GetFont("main_font"), realmCharacter.Name, new Vector2(box.Position.X + 6, box.Position.Y + 1), GfxManager.DefaultYellow);
                    spriteBatch.DrawString(GfxManager.GetFont("small_font"), $"Level {realmCharacter.Level} {realmCharacter.Class}", new Vector2(box.Position.X + 5, box.Position.Y + GfxManager.GetFont("small_font").LineHeight), Color.White);
                    spriteBatch.DrawString(GfxManager.GetFont("small_font"), $"{realmCharacter.Location}", new Vector2(box.Position.X + 5, box.Position.Y + (GfxManager.GetFont("small_font").LineHeight * 2)), Color.DarkGray);
                }
                spriteBatch.End();
            }

            DrawNetworkUpdates();
        }

        private void DrawNetworkUpdates()
        {
            switch (NetworkManager.State)
            {
                case NetworkManager.NetworkState.RetrievingCharacters:
                    GuiNotification.Draw(GfxManager.GetFont("main_font"), spriteBatch, "Retrieving characters...");
                    break;
                case NetworkManager.NetworkState.DeletingCharacter:
                    GuiNotification.Draw(GfxManager.GetFont("main_font"), spriteBatch, "Deleting character...");
                    break;
                case NetworkManager.NetworkState.EnteringWorld:
                    GuiNotification.Draw(GfxManager.GetFont("main_font"), spriteBatch, "Entering world...");
                    break;
                case NetworkManager.NetworkState.EnterWorld:
                    GameStateManager.EnterState(4);
                    break;
            }
        }

        private void OnEnterWorldPress()
        {
            if (characterIndex > -1)
            {
                NetworkManager.State = NetworkManager.NetworkState.EnteringWorld;
                NetworkManager.Send(new CMSG_World_Enter() { GUID = WorldofWarcraft.RealmCharacters[characterIndex].GUID }, NetworkManager.Direction.World);
            }
        }

        private void OnCreateCharacterPress() => GameStateManager.EnterState(3);

        private void OnDeleteCharacterPress()
        {
            NetworkManager.State = NetworkManager.NetworkState.DeletingCharacter;
            NetworkManager.Send(new CMSG_Character_Delete() { Name = realmCharacter.Name }, NetworkManager.Direction.World);
        }

        private void OnBackPress()
        {
            NetworkManager.Disconnect(NetworkManager.Direction.Both);
            GameStateManager.EnterState(1);
        }

        private void OnEnterPressed() => OnEnterWorldPress();

        private void OnDownPressed()
        {
            if (characterIndex != WorldofWarcraft.RealmCharacters.Count - 1)
                characterIndex++;
        }

        private void OnUpPressed()
        {
            if (characterIndex != 0)
                characterIndex--;
        }

        public override void OnStateEnter()
        {
            NetworkManager.State = NetworkManager.NetworkState.RetrievingCharacters;
            NetworkManager.Send(new CMSG_Character_List(), NetworkManager.Direction.World);

            WorldofWarcraft.Discord.SetPresence(new RichPresence()
            {
                State = "Selecting character...",
                Assets = new Assets()
                {
                    LargeImageKey = "wow_icon_discord",
                    LargeImageText = "World of Warcraft 2D"
                }
            });
        }
    }
}
