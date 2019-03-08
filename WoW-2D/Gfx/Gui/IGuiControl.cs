using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WoW_2D.Gfx.Gui
{
    /// <summary>
    /// All gui types should extend this.
    /// </summary>
    public abstract class IGuiControl
    {
        protected GraphicsDevice Graphics;
        public Texture2D BaseTexture { get; set; }
        public Vector2 Position { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsAcceptingTextInput { get; set; }

        #region Handlers
        public delegate void OnClickedHandler();
        public event OnClickedHandler OnClicked;

        protected virtual void OnClickedEvent() => OnClicked?.Invoke();
        public virtual void OnKeyTyped(Keys key, char character) {}
        #endregion

        public IGuiControl(GraphicsDevice graphics) => Graphics = graphics;

        public abstract void LoadContent(ContentManager content);
        public abstract void Update();
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
