using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoW_2D.Network;

namespace WoW_2D.Gfx.Gui
{
    /// <summary>
    /// Used for drawing network updates.
    /// </summary>
    public class GuiNotification
    {
        private static RectangleF rectangle;
        private static GuiButton button;

        public static void Initialize(GraphicsDevice graphics, ContentManager content)
        {
            rectangle = new Rectangle(0, 0, 460, 145);
            rectangle.Position = new Point2(graphics.Viewport.Width / 2 - rectangle.Width / 2, graphics.Viewport.Height / 2 - rectangle.Height / 2);

            button = new GuiButton(graphics) { Text = "Okay" };
            button.OnClicked += OnButtonClicked;
            button.IsEnabled = true;
            button.LoadContent(content);
            button.Position = new Vector2(rectangle.Position.X + (rectangle.Width / 2 - button.BaseTexture.Width / 2), rectangle.Position.Y + (rectangle.Height - button.BaseTexture.Height - 5));
        }

        public static void Draw(BitmapFont font, SpriteBatch spriteBatch, string text, bool drawButton = false)
        {
            var wrapped = WrapText(font, text);
            spriteBatch.Begin();
            spriteBatch.FillRectangle(rectangle, new Color(0f, 0f, 0f, 0.85f));
            spriteBatch.DrawRectangle(rectangle, WorldofWarcraft.DefaultYellow);
            spriteBatch.DrawString(font, wrapped, new Vector2(rectangle.X + (rectangle.Width / 2 - font.MeasureString(wrapped).Width / 2), rectangle.Y + (rectangle.Height / 2 - font.MeasureString(wrapped).Height / 2)), Color.White);
            if (drawButton)
                button.Draw(spriteBatch);
            spriteBatch.End();

            button.Update();
        }

        private static string WrapText(BitmapFont font, string text)
        {
            string line = string.Empty;
            string returnString = string.Empty;
            string[] wordArray = text.Split(' ');

            foreach (string word in wordArray)
            {
                if (font.MeasureString(line + word).Width > rectangle.Width)
                {
                    returnString = returnString + line + '\n';
                    line = string.Empty;
                }

                line = line + word + ' ';
            }

            return returnString + line;
        }

        private static void OnButtonClicked()
        {
            NetworkManager.State = NetworkManager.NetworkState.Waiting;
        }
    }
}
