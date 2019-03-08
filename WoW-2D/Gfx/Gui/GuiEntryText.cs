using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WoW_2D.Gfx.Gui
{
    /// <summary>
    /// Used for basic textfield types such as username, password, etc,.
    /// </summary>
    public class GuiEntryText : IGuiTextField
    {
        public GuiEntryText(GraphicsDevice graphics) : base(graphics) { }

        public override void LoadContent(ContentManager content)
        {
            Color[] bgColor = new Color[Width * Height];
            for (int i = 0; i < bgColor.Length; i++) bgColor[i] = BackgroundColor;

            backgroundTexture = new Texture2D(Graphics, Width, Height);
            backgroundTexture.SetData(bgColor);
            clippingRectangle = new Rectangle(Position.ToPoint(), new Point(Width, Height));

            textFont = content.Load<SpriteFont>("System/font");
            Text = "";
        }

        public override void Update()
        {
            localCurosrPosition = (int)textFont.MeasureString(Text.Substring(0, cursorPosition)).X / 2;
            if (localCurosrPosition > Width - 1)
                matrix = Matrix.CreateTranslation(Width - localCurosrPosition - textFont.MeasureString(Cursor).X / 2, 0f, 0f);
            else
                matrix = Matrix.Identity;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            /** Draw the background of the textfield first with no modifiers. **/
            spriteBatch.Begin();
            spriteBatch.Draw(backgroundTexture, Position, Color.White);
            spriteBatch.End();

            /** Draw the textfield text with a modified matrix and clip. **/
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, rasterizer, null, matrix);
            Rectangle oldClip = spriteBatch.GraphicsDevice.ScissorRectangle; // Save the old clip.

            spriteBatch.GraphicsDevice.ScissorRectangle = clippingRectangle;
            spriteBatch.DrawString(textFont, Text, new Vector2(Position.X, Position.Y + 7), ForegroundColor, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);
            if (IsActive)
                spriteBatch.DrawString(textFont, Cursor, new Vector2(Position.X+localCurosrPosition, Position.Y + 6), ForegroundColor, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);
            spriteBatch.GraphicsDevice.ScissorRectangle = oldClip;
            spriteBatch.End();
        }
        
        public override void OnKeyTyped(Keys key, char character)
        {
            if (IsActive)
            {
                if (!filteredKeys.Contains(key))
                {
                    switch (key)
                    {
                        case Keys.Back:
                            if (Text.Length > 0)
                            {
                                if (MaskCharacter > 0)
                                    PasswordText = PasswordText.Substring(0, PasswordText.Length - 1);
                                Text = Text.Substring(0, Text.Length - 1);
                                cursorPosition--;
                            }
                            break;
                        default:
                            if (MaskCharacter > 0)
                            {
                                Text = Text + MaskCharacter;
                                PasswordText = PasswordText + character;
                            }
                            else
                                Text = Text + character;
                            cursorPosition++;
                            break;
                    }
                }
            }
        }
    }
}
