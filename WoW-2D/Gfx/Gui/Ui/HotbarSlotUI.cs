using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace WoW_2D.Gfx.Gui.Ui
{
    /// <summary>
    /// The hotbar slot.
    /// </summary>
    public class HotbarSlotUI : UiControl
    {
        private RectangleF bounds;

        public HotbarSlotUI(GraphicsDevice graphics) : base(graphics)
        {
            bounds = new RectangleF(0, 0, 32, 32);
        }

        public override void LoadContent(ContentManager content)
        {}

        public override void Update()
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.FillRectangle(Position.X, Position.Y, bounds.Width, bounds.Height, new Color(0f, 0f, 0f, 0.75f));
            spriteBatch.DrawRectangle(new Vector2(Position.X, Position.Y), new Size2(bounds.Width, bounds.Height), Color.Gray);
        }

        public Size2 GetSize()
        {
            return bounds.Size;
        }
    }
}
