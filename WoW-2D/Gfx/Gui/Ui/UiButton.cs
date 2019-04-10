using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoW_2D.Gfx.Gui.Ui
{
    /// <summary>
    /// A simplistic button for in-game UI.
    /// </summary>
    public class UiButton : UiControl
    {
        public string Text { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        private bool isHovering = false;
        private Color hoverColor;
        private BitmapFont font;

        public UiButton(GraphicsDevice graphics) : base(graphics) { }

        public override void LoadContent(ContentManager content)
        {}

        public override void Update()
        {
            Point mousePosition = Mouse.GetState().Position;
            isHovering = ((mousePosition.X >= Position.X && mousePosition.X <= Position.X + Width && mousePosition.Y >= Position.Y && mousePosition.Y <= Position.Y + Height) && IsEnabled) ? true : false;
            hoverColor = !isHovering ? Color.White : WorldofWarcraft.DefaultYellow;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.FillRectangle(Position, new Size2(Width, Height), Color.Black);
            spriteBatch.DrawRectangle(Position, new Size2(Width, Height), hoverColor);
            spriteBatch.DrawString(font, Text, new Vector2((Position.X + (Width / 2)), (Position.Y + (Height / 2))), Color.White, 0f, font.MeasureString(Text) / 2, 1f, SpriteEffects.None, 0f);
            spriteBatch.End();
        }

        public void Draw(BitmapFont font, SpriteBatch spriteBatch)
        {
            this.font = font;
            Draw(spriteBatch);
        }
    }
}
