﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using MonoGameHelper.Gfx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoW_2D.Utils;

namespace WoW_2D.Gfx.Gui
{
    /// <summary>
    /// The chat textfield.
    /// </summary>
    public class GuiChatText : IGuiTextField
    {
        public bool IsVisible { get; set; } = false;

        public GuiChatText(GraphicsDevice graphics) : base(graphics) { }

        public override void LoadContent(ContentManager content)
        {
            Color[] bgColor = new Color[Width * Height];
            for (int i = 0; i < bgColor.Length; i++) bgColor[i] = BackgroundColor;

            BaseTexture = new Texture2D(graphics, Width, Height);
            BaseTexture.SetData(bgColor);
            clippingRectangle = new Rectangle(Position.ToPoint(), new Point(Width, Height));

            textFont = GfxManager.GetFont("main_font");
            Text = "";

            FilteredKeys.Remove(Keys.Space);
        }

        public override void Update()
        {
            base.Update();

            IsActive = IsVisible;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsVisible && !Global.ShouldHideUI)
            {
                /** Draw the background of the textfield first with no modifiers. **/
                spriteBatch.Begin();
                spriteBatch.DrawRectangle(new Vector2(Position.X - 1f, Position.Y - 1f), new Size2(BaseTexture.Width + 2, BaseTexture.Height + 2), Color.DarkGray, 2.5f);
                spriteBatch.Draw(BaseTexture, Position, Color.White);
                spriteBatch.End();

                /** Draw the textfield text with a modified matrix and clip. **/
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, rasterizer, null, matrix);
                Rectangle oldClip = spriteBatch.GraphicsDevice.ScissorRectangle; // Save the old clip.

                spriteBatch.GraphicsDevice.ScissorRectangle = clippingRectangle;
                spriteBatch.DrawString(textFont, Text, new Vector2(Position.X, Position.Y + (BaseTexture.Height / 2)), ForegroundColor, 0f, new Vector2(0f, textFont.MeasureString(Text).Height / 2), 1f, SpriteEffects.None, 0f);
                if (IsActive)
                    spriteBatch.DrawString(textFont, Cursor, new Vector2(Position.X + localCurosrPosition, Position.Y + (BaseTexture.Height / 2)), ForegroundColor, 0f, new Vector2(0f, textFont.MeasureString(Cursor).Height / 2), 1f, SpriteEffects.None, 0f);
                spriteBatch.GraphicsDevice.ScissorRectangle = oldClip;
                spriteBatch.End();
            }
        }
    }
}
