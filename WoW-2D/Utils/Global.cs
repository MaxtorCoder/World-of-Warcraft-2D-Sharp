using MonoGame.Extended.BitmapFonts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoW_2D.Gfx;

namespace WoW_2D.Utils
{
    /// <summary>
    /// Global constants and methods.
    /// </summary>
    public class Global
    {
        public static SpriteSheet HumanSpritesheet;

        public static SpriteSheet RaceSpritesheet;
        public static SpriteSheet ClassSpritesheet;

        public static bool ShouldHideUI { get; set; } = false;
        public static bool ShouldExit { get; set; } = false;
        public static bool ShouldDrawBreakingNews { get; set; } = false;
        public static bool IsRequestingLoadingData { get; set; } = false;
        public static int FullscreenFlag { get; set; } = 0;
        public static string BreakingNewsText = string.Empty;

        public static Queue<ChatMessage> Chats = new Queue<ChatMessage>();
        public static List<ClassType> Classes = new List<ClassType>();
    }
}
