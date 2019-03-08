using WoW_2D.Gfx;
using WoW_2D.Gfx.Animation;

namespace WoW_2D.GameObject
{
    /// <summary>
    /// All object types that should have an animation would extend this class.
    /// </summary>
    public class AnimatedObject
    {
        protected SpriteSheet SpriteSheet { get; set; }
        protected AnimationManager AnimationManager { get; set; }
    }
}
