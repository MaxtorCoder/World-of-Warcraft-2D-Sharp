using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
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
        private static Dictionary<string, Song> Songs = new Dictionary<string, Song>();
        private static Dictionary<string, Texture2D> Models = new Dictionary<string, Texture2D>();

        public static Color DefaultYellow = new Color(223, 195, 15);

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

            Songs.Add("main_theme", content.Load<Song>("System/Music/wow_main_theme"));

            Models.Add("model_zomboid_1", content.Load<Texture2D>("Sprites/NPC/1"));
            Models.Add("model_human", content.Load<Texture2D>("Sprites/Human/Human"));
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

        public static Song GetSong(string name)
        {
            foreach (var keyvalues in Songs)
                if (string.Equals(keyvalues.Key, name))
                    return keyvalues.Value;
            return null;
        }

        public static Dictionary<string, Texture2D> GetModels()
        {
            return Models;
        }

        public static string Wrap(BitmapFont font, string text, float maxWidth)
        {
            string line = string.Empty;
            string returnString = string.Empty;
            string[] wordArray = text.Split(' ');

            foreach (string word in wordArray)
            {
                if (font.MeasureString(line + word).Width > maxWidth)
                {
                    returnString = returnString + line + '\n';
                    line = string.Empty;
                }

                line = line + word + ' ';
            }

            return returnString + line;
        }
    }
}
