using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace WoW_2D.Gfx.Gui
{
    /// <summary>
    /// A base class for textfield types.
    /// </summary>
    public abstract class IGuiTextField : IGuiControl
    {
        #region Modifiable TextField Values
        public Color BorderColor { get; set; } = WorldofWarcraft.DefaultYellow; // Unused as of right now.
        public Color BackgroundColor { get; set; } = Color.Black;
        public Color ForegroundColor { get; set; } = Color.White;

        public int Width { get; set; }
        public int Height { get; set; }
        public string Text { get; set; }
        public string PasswordText { get; set; }
        public char MaskCharacter { get; set; }
        public string Cursor { get; set; } = "|";
        public bool IsActive { get; set; }
        #endregion

        protected Texture2D backgroundTexture, borderTexture;
        protected Rectangle clippingRectangle;
        protected SpriteFont textFont;
        protected int cursorPosition = 0;
        protected int localCurosrPosition = 0;
        protected RasterizerState rasterizer = new RasterizerState() { ScissorTestEnable = true };
        protected Matrix matrix = Matrix.Identity;
        protected IReadOnlyCollection<Keys> filteredKeys = new List<Keys>()
        {
            Keys.Tab,
            Keys.Escape
        };

        public IGuiTextField(GraphicsDevice graphics) : base(graphics) { IsAcceptingTextInput = true; }

        /// <summary>
        /// Get the text of this textfield.
        /// </summary>
        /// <returns></returns>
        public string GetText()
        {
            if (MaskCharacter > 0)
                return PasswordText;
            return Text;
        }
    }
}
