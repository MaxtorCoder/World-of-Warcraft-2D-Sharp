using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Graphics;
using MonoGameHelper.Gfx;
using MonoGameHelper.Utils;
using System.Diagnostics;

namespace WoW_2D.Gfx.Gui
{
    /// <summary>
    /// A basic button class.
    /// </summary>
    public class GuiButton : IGuiControl
    {
        public string Text { get; set; }

        private bool isHovering = false;
        private Color hoverColor;

        public GuiButton(GraphicsDevice graphics) : base(graphics) { }

        public override void LoadContent(ContentManager content)
        {
            BaseTexture = GfxManager.GetTexture("button_enabled"); // Set for positioning since both textures are the same size.
        }

        public override void Update()
        {
            Point mousePosition = Mouse.GetState().Position;
            isHovering = ((mousePosition.X >= Position.X && mousePosition.X <= Position.X + BaseTexture.Width && mousePosition.Y >= Position.Y && mousePosition.Y <= Position.Y + BaseTexture.Height) && IsEnabled) ? true : false;
            hoverColor = isHovering ? Color.White : WorldofWarcraft.DefaultYellow;
            BaseTexture = IsEnabled ? GfxManager.GetTexture("button_enabled") : GfxManager.GetTexture("button_disabled");

            UpdateMousePress();
        }

        private void UpdateMousePress()
        {
            if (isHovering)
            {
                if (InputHandler.IsMouseButtonPressed(InputHandler.MouseButton.LeftButton)) 
                {
                    base.OnClickedEvent();
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(BaseTexture, Position, Color.White);
            spriteBatch.DrawString(GfxManager.GetFont("main_font"), Text, new Vector2((Position.X + (BaseTexture.Width / 2)), (Position.Y + (BaseTexture.Height / 2))), hoverColor, 0f, GfxManager.GetFont("main_font").MeasureString(Text) / 2, 1f, SpriteEffects.None, 0f);
        }
    }
}
