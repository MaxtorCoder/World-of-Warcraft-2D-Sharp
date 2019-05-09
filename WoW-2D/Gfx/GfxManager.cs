using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoW_2D.Gfx
{
    /// <summary>
    /// Handles resource loading and use.
    /// </summary>
    public class GfxManager
    {
        private static Dictionary<string, Texture2D> Textures = new Dictionary<string, Texture2D>();
        private static Dictionary<string, BitmapFont> Fonts = new Dictionary<string, BitmapFont>();

        public static void Load(ContentManager content)
        {
            Textures.Add("human_spritesheet", content.Load<Texture2D>("Sprites/Human/Human"));
            Textures.Add("race_spritesheet", content.Load<Texture2D>("Sprites/UI/RaceIcons"));
            Textures.Add("class_spritesheet", content.Load<Texture2D>("Sprites/UI/ClassIcons"));
            Textures.Add("logo", content.Load<Texture2D>("Logo"));
            Textures.Add("background1", content.Load<Texture2D>("bg_0"));
            Textures.Add("ally_banner", content.Load<Texture2D>("Sprites/UI/Ally_Banner"));
            Textures.Add("horde_banner", content.Load<Texture2D>("Sprites/UI/Horde_Banner"));
            Textures.Add("button_enabled", content.Load<Texture2D>("Sprites/UI/button_enabled"));
            Textures.Add("button_disabled", content.Load<Texture2D>("Sprites/UI/button_disabled"));

            Fonts.Add("main_font", content.Load<BitmapFont>("System/Font/font"));
            Fonts.Add("small_font", content.Load<BitmapFont>("System/Font/font_small"));
        }

        public static Texture2D GetTexture(string name)
        {
            foreach (var keyvalues in Textures)
                if (string.Equals(keyvalues.Key, name))
                    return keyvalues.Value;
            return null;
        }

        public static BitmapFont GetFont(string name)
        {
            foreach (var keyvalues in Fonts)
                if (string.Equals(keyvalues.Key, name))
                    return keyvalues.Value;
            return null;
        }
    }
}
