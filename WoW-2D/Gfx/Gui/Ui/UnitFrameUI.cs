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
        public RectangleF UnitDisplay;
        public RectangleF HealthBar;
        public RectangleF PowerBar;

        private float healthPercent = 0.0f;

        public UnitFrameUI(GraphicsDevice graphics) : base(graphics) { }

        public override void LoadContent(ContentManager content) { }

        public override void Update()
        {
            healthPercent = WorldofWarcraft.Map.Player.WorldData.Stats.CurrentHP / WorldofWarcraft.Map.Player.WorldData.Stats.MaxHP;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var texture = WorldofWarcraft.Map.Player.GetAnimation().GetFrame(0);
            if (!Global.ShouldHideUI)
            {
                spriteBatch.Begin(blendState: BlendState.NonPremultiplied);
                spriteBatch.FillRectangle(UnitDisplay, Color.Black);
                spriteBatch.DrawRectangle(UnitDisplay, Color.Gray);
                spriteBatch.Draw(texture, new Vector2(UnitDisplay.Position.X + UnitDisplay.Width / 2, UnitDisplay.Position.Y + UnitDisplay.Height / 2), null, Color.White, 0f, new Vector2(texture.Width / 2, texture.Height / 2), 1f, SpriteEffects.None, 1f);
                spriteBatch.DrawString(GfxManager.GetFont("small_font"), WorldofWarcraft.Map.Player.WorldData.Name, new Vector2((UnitDisplay.Position.X + UnitDisplay.Width + 4f) + 1f, UnitDisplay.Position.Y + 1f), Color.Black);
                spriteBatch.DrawString(GfxManager.GetFont("small_font"), WorldofWarcraft.Map.Player.WorldData.Name, new Vector2((UnitDisplay.Position.X + UnitDisplay.Width + 4f), UnitDisplay.Position.Y), WorldofWarcraft.DefaultYellow);
                spriteBatch.FillRectangle(HealthBar, Color.Black);
                spriteBatch.FillRectangle(new RectangleF(HealthBar.Position, new Size2(HealthBar.Width * healthPercent, HealthBar.Height)), new Color(120, 173, 34));
                spriteBatch.DrawRectangle(HealthBar, Color.Gray);

                spriteBatch.FillRectangle(PowerBar, Color.Black);
                spriteBatch.DrawRectangle(PowerBar, Color.Gray);
                spriteBatch.End();
            }
        }
    }
}
