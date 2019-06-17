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
    /// The frame for units.
    /// </summary>
    public class UnitFrameUI : UiControl
    {
        private RectangleF frameBounds;
        private RectangleF unitDisplay;
        private RectangleF healthBar;
        private RectangleF powerBar;

        private float healthPercent = 0.0f;

        public UnitFrameUI(GraphicsDevice graphics, bool isTargetFrame) : base(graphics)
        {
            if (!isTargetFrame)
                frameBounds = new RectangleF(Point2.Zero, new Size2(150f, 46f));
            else
                frameBounds = new RectangleF(new Point2(152f, 0f), new Size2(150f, 46f));

            unitDisplay = new RectangleF(new Point2(frameBounds.X + 2f, frameBounds.Y + 2f), new Size2(42f, 42f));
            healthBar = new RectangleF(new Point2(unitDisplay.X + unitDisplay.Width + 4f, unitDisplay.Y + GfxManager.GetFont("small_font").LineHeight), new Size2(100f, 12f));
            powerBar = new RectangleF(new Point2(healthBar.X, healthBar.Y + 12f), healthBar.Size);
        }

        public override void LoadContent(ContentManager content) { }

        public override void Update()
        {
            healthPercent = WorldofWarcraft.World.Player.Info.Stats.CurrentHP / WorldofWarcraft.World.Player.Info.Stats.MaxHP;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var texture = WorldofWarcraft.World.Player.GetAnimation().GetFrame(0);
            if (!Global.ShouldHideUI)
            {
                spriteBatch.Begin(blendState: BlendState.NonPremultiplied);
                spriteBatch.FillRectangle(unitDisplay, Color.Black);
                spriteBatch.DrawRectangle(unitDisplay, Color.Gray);
                spriteBatch.Draw(texture, new Vector2(unitDisplay.Position.X + unitDisplay.Width / 2, unitDisplay.Position.Y + unitDisplay.Height / 2), null, Color.White, 0f, new Vector2(texture.Width / 2, texture.Height / 2), 1f, SpriteEffects.None, 1f);
                spriteBatch.DrawString(GfxManager.GetFont("small_font"), WorldofWarcraft.World.Player.Info.Name, new Vector2((unitDisplay.Position.X + unitDisplay.Width + 4f) + 1f, unitDisplay.Position.Y + 1f), Color.Black);
                spriteBatch.DrawString(GfxManager.GetFont("small_font"), WorldofWarcraft.World.Player.Info.Name, new Vector2((unitDisplay.Position.X + unitDisplay.Width + 4f), unitDisplay.Position.Y), GfxManager.DefaultYellow);
                spriteBatch.FillRectangle(healthBar, Color.Black);
                spriteBatch.FillRectangle(new RectangleF(healthBar.Position, new Size2(healthBar.Width * healthPercent, healthBar.Height)), new Color(120, 173, 34));
                spriteBatch.DrawRectangle(healthBar, Color.Gray);

                spriteBatch.FillRectangle(powerBar, Color.Black);
                spriteBatch.DrawRectangle(powerBar, Color.Gray);
                spriteBatch.End();
            }
        }
    }
}
