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
using WoW_2D.Gfx.Gui;
using WoW_2D.Gfx.Gui.Ui;
using WoW_2D.Network;

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

        private BitmapFont font;

        public GameState(GraphicsDevice graphics) : base(graphics) { }

        public override void Initialize()
        {
            escapeMenu = new EscapeMenuUI(graphics);
            chatUi = new ChatUI(graphics);
            hotbar = new HotbarUI(graphics);
            Controls.Add(chatUi.TextField);

            InputHandler.AddKeyPressHandler(ID, delegate () { OnEscapePressed(); }, Keys.Escape);
            InputHandler.AddKeyPressHandler(ID, delegate () { OnEnterPressed(); }, Keys.Enter);
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            font = content.Load<BitmapFont>("System/Font/font_small");

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

            if (NetworkManager.State == NetworkManager.NetworkState.Disconnected)
            {
                chatUi.ClearChats();
                WorldofWarcraft.Players.Clear();
                GameStateManager.EnterState(1);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            WorldofWarcraft.Map.Draw(spriteBatch, gameTime);

            spriteBatch.Begin();
            if (WorldofWarcraft.ClientSettings.GetSection("Interface").GetBool("myname"))
                DrawMyName(spriteBatch);
            if (WorldofWarcraft.ClientSettings.GetSection("Interface").GetBool("players"))
                DrawPlayerNames(spriteBatch);
            spriteBatch.End();

            escapeMenu.Draw(spriteBatch);
            chatUi.Draw(spriteBatch);
            hotbar.Draw(spriteBatch);
        }

        private void DrawMyName(SpriteBatch spriteBatch)
        {
            var boundPos = WorldofWarcraft.Map.Player.GetCamera().WorldToScreen(WorldofWarcraft.Map.Player.BoundingBox.X, WorldofWarcraft.Map.Player.BoundingBox.Y);

            spriteBatch.DrawString(font, WorldofWarcraft.Map.Player.WorldData.Name, 
                new Vector2(boundPos.X + 16f - font.MeasureString(WorldofWarcraft.Map.Player.WorldData.Name).Width / 2 + 1f, boundPos.Y - font.LineHeight + 1f), Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(font, WorldofWarcraft.Map.Player.WorldData.Name,
                new Vector2(boundPos.X + 16f - font.MeasureString(WorldofWarcraft.Map.Player.WorldData.Name).Width / 2, boundPos.Y - font.LineHeight), WorldofWarcraft.DefaultYellow, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }

        private void DrawPlayerNames(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < WorldofWarcraft.Players.Count; i++)
            {
                var player = WorldofWarcraft.Players[i];
                var playerBoundingBox = player.BoundingBox;

                if (WorldofWarcraft.Map.Player.GetCamera().BoundingRectangle.Contains(playerBoundingBox.Position))
                {
                    var translatedBoundPosition = WorldofWarcraft.Map.Player.GetCamera().WorldToScreen(playerBoundingBox.X, playerBoundingBox.Y);

                    spriteBatch.DrawString(font, player.WorldData.Name,
                new Vector2(translatedBoundPosition.X + 16f - font.MeasureString(WorldofWarcraft.Map.Player.WorldData.Name).Width / 2 + 1f, translatedBoundPosition.Y - font.LineHeight + 1f), Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(font, player.WorldData.Name,
                        new Vector2(translatedBoundPosition.X + 16f - font.MeasureString(WorldofWarcraft.Map.Player.WorldData.Name).Width / 2, translatedBoundPosition.Y - font.LineHeight), WorldofWarcraft.DefaultYellow, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                }
            }
        }

        private void OnEscapePressed() => escapeMenu.Activator();
        private void OnEnterPressed() => chatUi.OnEnterPressed();
    }
}
