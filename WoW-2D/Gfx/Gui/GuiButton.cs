using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Graphics;
using MonoGameHelper.Gfx;
using MonoGameHelper.Utils;

namespace WoW_2D.Gfx.Gui
{
    /// <summary>
    /// A basic button class.
    /// </summary>
    public class GuiButton : IGuiControl
    {
        public string Text { get; set; }

        private Texture2D button_enabled;
        private Texture2D button_disabled;
        private BitmapFont font;

        private bool isHovering = false;
        private Color hoverColor;

        public GuiButton(GraphicsDevice graphics) : base(graphics) { }

        public override void LoadContent(ContentManager content)
        {
            button_enabled = content.Load<Texture2D>("Sprites/UI/button_enabled");
            button_disabled = content.Load<Texture2D>("Sprites/UI/button_disabled");
            font = content.Load<BitmapFont>("System/Font/font");
            BaseTexture = button_enabled; // Set for positioning since both textures are the same size.
        }

        public override void Update()
        {
            Point mousePosition = Mouse.GetState().Position;
            isHovering = ((mousePosition.X >= Position.X && mousePosition.X <= Position.X + BaseTexture.Width && mousePosition.Y >= Position.Y && mousePosition.Y <= Position.Y + BaseTexture.Height) && IsEnabled) ? true : false;
            hoverColor = isHovering ? Color.White : WorldofWarcraft.DefaultYellow;
            BaseTexture = IsEnabled ? button_enabled : button_disabled;

            UpdateMousePress();
        }

        private void UpdateMousePress()
        {
            if (InputHandler.IsMouseButtonPressed(InputHandler.MouseButton.LeftButton))
            {
                if (isHovering)
                    base.OnClickedEvent();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(BaseTexture, Position, Color.White);
            spriteBatch.DrawString(font, Text, new Vector2((Position.X + (BaseTexture.Width / 2)), (Position.Y + (BaseTexture.Height / 2))), hoverColor, 0f, font.MeasureString(Text) / 2, 1f, SpriteEffects.None, 0f);
        }
    }
}
