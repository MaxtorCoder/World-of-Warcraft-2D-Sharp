using Framework.Entity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using MonoGameHelper.GameState;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoW_2D.Gfx.Gui;
using WoW_2D.World;

namespace WoW_2D.States
{
    /// <summary>
    /// Loads the needed map, local player information, etc,.
    /// </summary>
    public class LoadingState : IGameState
    {
        private ContentManager _content;
        private bool isLoading = false;
        private bool hasStarted = false;
        private BitmapFont font;

        public LoadingState(GraphicsDevice graphics) : base(graphics) { }

        public override void Initialize()
        {
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);
            font = content.Load<BitmapFont>("System/Font/font");
            _content = content;
        }

        public override void UnloadContent()
        {
        }

        public override void Update(GameTime gameTime)
        {
            if (!isLoading)
                isLoading = true;

            if (isLoading)
            {
                if (!hasStarted)
                {
                    hasStarted = true;
                    MapManager.LoadMap(_content);
                }
                else
                {
                    if (WorldofWarcraft.Map.Player != null)
                    {
                        WorldofWarcraft.Map.Initialize(graphics);
                        WorldofWarcraft.Map.Player.LoadContent(_content);
                        GameStateManager.EnterState(5);
                    }
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            graphics.Clear(Color.Black);

            GuiNotification.Draw(font, spriteBatch, "Loading content...");
        }
    }
}
