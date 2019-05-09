using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using WoW_2D.Utils;

namespace WoW_2D.Gfx.Gui.Ui
{
    /// <summary>
    /// The login-screen "Breaking News" box.
    /// </summary>
    public class BreakingNewsUI : UiControl
    {
        private string breakingNews = "Breaking News";

        private RectangleF breakingNewsBox;

        public BreakingNewsUI(GraphicsDevice graphics) : base(graphics)
        {
            breakingNewsBox = new RectangleF(new Point2(), new Size2(275, 400));
            breakingNewsBox.Position = new Point2(15f, graphics.Viewport.Height / 2 - breakingNewsBox.Height / 2);
        }

        public override void LoadContent(ContentManager content)
        {}

        public override void Update()
        {}

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Global.ShouldDrawBreakingNews)
            {
                var text = Global.WrapText(GfxManager.GetFont("small_font"), Global.BreakingNewsText, breakingNewsBox.Width);

                spriteBatch.Begin(blendState: BlendState.NonPremultiplied);
                spriteBatch.FillRectangle(breakingNewsBox, new Color(0f, 0f, 0f, 0.75f));
                spriteBatch.DrawRectangle(breakingNewsBox, Color.Gray);
                spriteBatch.End();

                spriteBatch.Begin();
                spriteBatch.DrawString(GfxManager.GetFont("main_font"), breakingNews, new Vector2(breakingNewsBox.Position.X + (breakingNewsBox.Width / 2) + 1, breakingNewsBox.Position.Y + 10f + 1), Color.Black, 0f, new Vector2(GfxManager.GetFont("main_font").MeasureString(breakingNews).Width / 2, 0f), 1f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(GfxManager.GetFont("main_font"), breakingNews, new Vector2(breakingNewsBox.Position.X + (breakingNewsBox.Width / 2), breakingNewsBox.Position.Y + 10f), WorldofWarcraft.DefaultYellow, 0f, new Vector2(GfxManager.GetFont("main_font").MeasureString(breakingNews).Width / 2, 0f), 1f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(GfxManager.GetFont("small_font"), text, new Vector2(breakingNewsBox.X + 5f, breakingNewsBox.Position.Y + 40f), Color.White);
                spriteBatch.End();
            }
        }
    }
}
