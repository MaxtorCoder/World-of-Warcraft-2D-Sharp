using Framework.Entity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoW_2D.Utils;

namespace WoW_2D.Gfx
{
    /// <summary>
    /// A race-type to render and handle.
    /// </summary>
    public class RaceType
    {
        public Texture2D Icon { get; set; }
        public Race Race { get; }
        public bool IsHovering { get; set; }
        public bool IsSelected { get; set; } = false;
        public bool IsEnabled { get; set; } = false;
        public List<ClassType> Classes { get; set; }

        public RaceType(Race race)
        {
            Race = race;
            Classes = Global.Classes.FindAll(x => x.Races.Contains(Race));
        }

        public void Draw(BitmapFont font, SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(Icon, position, Color.White);
            spriteBatch.End();
            if (IsEnabled)
            {
                if (IsHovering)
                {
                    spriteBatch.Begin(blendState: BlendState.NonPremultiplied);
                    spriteBatch.FillRectangle(new Vector2(position.X + (Icon.Width / 2 - font.MeasureString(Race.ToString()).Width / 2), position.Y + Icon.Height), new Size2(font.MeasureString(Race.ToString()).Width, font.LineHeight), new Color(0f, 0f, 0f, 0.75f));
                    spriteBatch.End();
                    spriteBatch.Begin();
                    spriteBatch.DrawString(font, Race.ToString(), new Vector2(position.X + (Icon.Width / 2 - font.MeasureString(Race.ToString()).Width / 2), position.Y + Icon.Height), Color.White);
                    spriteBatch.End();
                }
            }
            else
            {
                spriteBatch.Begin(blendState: BlendState.NonPremultiplied);
                spriteBatch.FillRectangle(new RectangleF(position.ToPoint(), Icon.Bounds.Size), new Color(0f, 0f, 0f, 0.75f));
                spriteBatch.End();
            }
        }
    }
}
