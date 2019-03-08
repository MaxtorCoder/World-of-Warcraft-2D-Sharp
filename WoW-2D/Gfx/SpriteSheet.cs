using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WoW_2D.Gfx
{
    /// <summary>
    /// Handles spritesheet related data.
    /// Any game-object that should use an animation would contain an instance of SpriteSheet in it's class.
    /// </summary>
    public class SpriteSheet
    {
        private GraphicsDevice Graphics;
        private Texture2D baseTexture;

        public SpriteSheet(GraphicsDevice Graphics) => this.Graphics = Graphics;

        /// <summary>
        /// Set a baseTexture for this spritesheet.
        /// </summary>
        /// <param name="baseTexture"></param>
        public void SetTexture(Texture2D baseTexture)
        {
            this.baseTexture = baseTexture;
        }

        /// <summary>
        /// Get a sprite at the specified position with the specified size.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public Texture2D GetSprite(Point position, Point size)
        {
            Rectangle srcRectangle = new Rectangle(position, size);
            Texture2D sprite = new Texture2D(Graphics, srcRectangle.Width, srcRectangle.Height);
            Color[] data = new Color[srcRectangle.Width * srcRectangle.Height];
            baseTexture.GetData(0, srcRectangle, data, 0, data.Length);
            sprite.SetData(data);
            return sprite;
        }
    }
}
