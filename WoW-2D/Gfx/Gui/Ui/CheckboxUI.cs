using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using MonoGameHelper.Utils;

namespace WoW_2D.Gfx.Gui.Ui
{
    /// <summary>
    /// A checkbox used for various options and controls.
    /// </summary>
    public class CheckboxUI : UiControl
    {
        public string Text { get; set; }
        public bool DrawBackdrop { get; set; } = false;
        public bool IsChecked { get; set; } = false;
        private BitmapFont font;
        private RectangleF checkBox = new RectangleF(0, 0, 16, 16);
        private RectangleF bounds;

        private Point mousePos;

        public CheckboxUI(GraphicsDevice graphics) : base(graphics) {}

        public override void LoadContent(ContentManager content)
        {
            font = GfxManager.GetFont("small_font");
        }

        public override void Update()
        {
            if (checkBox.Position.X == 0 && checkBox.Position.Y == 0)
                checkBox.Position = Position.ToPoint();
            if (bounds.X == 0 && bounds.Y == 0)
                bounds = new RectangleF(checkBox.X, checkBox.Y, checkBox.Width + font.MeasureString(Text).Width + 5f, font.MeasureString(Text).Height);

            mousePos = Mouse.GetState().Position;
            if (bounds.Contains(mousePos))
                if (InputHandler.IsMouseButtonPressed(InputHandler.MouseButton.LeftButton))
                    IsChecked = !IsChecked;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(blendState: BlendState.NonPremultiplied);
            spriteBatch.FillRectangle(new RectangleF(checkBox.X, checkBox.Y, checkBox.Width, checkBox.Height), new Color(0f, 0f, 0f, 0.75f));
            spriteBatch.DrawRectangle(new RectangleF(checkBox.X, checkBox.Y, checkBox.Width, checkBox.Height), Color.Gray);
            if (IsChecked)
                spriteBatch.FillRectangle(new RectangleF(checkBox.X + (checkBox.Width / 2 - 4), checkBox.Y + (checkBox.Height / 2 - 4), 8, 8), GfxManager.DefaultYellow);
            spriteBatch.End();

            spriteBatch.Begin();
            if (DrawBackdrop)
                spriteBatch.DrawString(font, Text, new Vector2(checkBox.X + checkBox.Width + 5f + 1f, checkBox.Y + (checkBox.Height / 2 - font.MeasureString(Text).Height / 2) + 1f), Color.Black);
            spriteBatch.DrawString(font, Text, new Vector2(checkBox.X + checkBox.Width + 5f, checkBox.Y + (checkBox.Height / 2 - font.MeasureString(Text).Height / 2)), GfxManager.DefaultYellow);
            spriteBatch.End();
        }
    }
}
