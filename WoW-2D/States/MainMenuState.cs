using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WoW_2D.States
{
    /// <summary>
    /// A state for the main menu of the game.
    /// </summary>
    public class MainMenuState : IGameState
    {
        private Texture2D background;
        private SpriteFont font;

        private const string blizzardCopyright = "Copyright 2004-2019 Blizzard Entertainment. All Rights Reserved.";
        private const string teamCopyright = "Copyright 2018-2019 SolitudeDevelopment.";

        public MainMenuState(GraphicsDevice graphics) : base(graphics)
        {}

        public override void Initialize()
        {}

        public override void LoadContent(ContentManager content)
        {
            background = content.Load<Texture2D>("bg_0");
            font = content.Load<SpriteFont>("System/font");
            /** Drawback #2! Can't load custom fonts with the default importers and processors! **/
        }

        public override void UnloadContent()
        {}

        public override void Update(GameTime gameTime)
        {}

        public override void Draw(SpriteBatch spriteBatch)
        {
            Graphics.Clear(Color.Black);

            spriteBatch.Begin();
            spriteBatch.Draw(background, new Vector2(0f, 0f), Color.White);
            DrawUIStrings(spriteBatch);
            spriteBatch.End();
        }

        private void DrawUIStrings(SpriteBatch spriteBatch)
        {
            /** For some reason, we have to halve the font-size set in the font.spritefont file.. *Sigh* Drawback #1, Maxtor, told ya! **/
            spriteBatch.DrawString(font, "March 3 2019", new Vector2(0f, Graphics.Viewport.Height - font.LineSpacing / 2), new Color(223, 195, 15), 0f, new Vector2(0f, 0f), 0.5f, SpriteEffects.None, 0);
            spriteBatch.DrawString(font, "v0.1.0a", new Vector2(0f, Graphics.Viewport.Height - font.LineSpacing), new Color(223, 195, 15), 0f, new Vector2(0f, 0f), 0.5f, SpriteEffects.None, 0);
            spriteBatch.DrawString(font, blizzardCopyright, new Vector2(Graphics.Viewport.Width / 2 - font.MeasureString(blizzardCopyright).X / 4, Graphics.Viewport.Height - font.LineSpacing / 2), new Color(223, 195, 15), 0f, new Vector2(0f, 0f), 0.5f, SpriteEffects.None, 0);
            spriteBatch.DrawString(font, teamCopyright, new Vector2(Graphics.Viewport.Width / 2 - font.MeasureString(teamCopyright).X / 4, Graphics.Viewport.Height - font.LineSpacing), new Color(223, 195, 15), 0f, new Vector2(0f, 0f), 0.5f, SpriteEffects.None, 0);
        }

        public override string ToString()
        {
            return null;
        }
    }
}
