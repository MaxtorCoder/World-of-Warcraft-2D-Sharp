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
    /// A class-type to render and handle.
    /// </summary>
    public class ClassType
    {
        public Texture2D Icon { get; set; }
        public Class Class { get; }
        public List<Race> Races { get; set; }
        public bool IsHovering { get; set; }
        public bool IsSelected { get; set; }

        public ClassType(Class @class)
        {
            Class = @class;
            switch (@class)
            {
                case Class.Warrior: Icon = Global.ClassSpritesheet.GetSprite(new Point(0), new Point(32)); break;
                case Class.Mage: Icon = Global.ClassSpritesheet.GetSprite(new Point(1 * 32, 0), new Point(32)); break;
                case Class.Rogue: Icon = Global.ClassSpritesheet.GetSprite(new Point(2 * 32, 0), new Point(32)); break;
                case Class.Druid: Icon = Global.ClassSpritesheet.GetSprite(new Point(3 * 32, 0), new Point(32)); break;
                case Class.Hunter: Icon = Global.ClassSpritesheet.GetSprite(new Point(0, 1 * 32), new Point(32)); break;
                case Class.Shaman: Icon = Global.ClassSpritesheet.GetSprite(new Point(1 * 32, 1 * 32), new Point(32)); break;
                case Class.Priest: Icon = Global.ClassSpritesheet.GetSprite(new Point(2 * 32, 1 * 32), new Point(32)); break;
                case Class.Warlock: Icon = Global.ClassSpritesheet.GetSprite(new Point(3 * 32, 1 * 32), new Point(32)); break;
                case Class.Paladin: Icon = Global.ClassSpritesheet.GetSprite(new Point(0, 2 * 32), new Point(32)); break;
            }
        }

        public void Draw(BitmapFont font, SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(Icon, position, Color.White);
            spriteBatch.End();
            if (IsHovering)
            {
                spriteBatch.Begin(blendState: BlendState.NonPremultiplied);
                spriteBatch.FillRectangle(new Vector2(position.X + (Icon.Width / 2 - font.MeasureString(Class.ToString()).Width / 2), position.Y + Icon.Height), new Size2(font.MeasureString(Class.ToString()).Width, font.LineHeight), new Color(0f, 0f, 0f, 0.75f));
                spriteBatch.End();
                spriteBatch.Begin();
                spriteBatch.DrawString(font, Class.ToString(), new Vector2(position.X + (Icon.Width / 2 - font.MeasureString(Class.ToString()).Width / 2), position.Y + Icon.Height), Color.White);
                spriteBatch.End();
            }
        }
    }
}
