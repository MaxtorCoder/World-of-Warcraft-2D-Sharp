using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WoW_2D.GameObject
{
    /// <summary>
    /// An extendable player-object for player-types.
    /// </summary>
    public abstract class IPlayerObject : AnimatedObject
    {
        public enum MovementDirection : int
        {
            North = 0,
            South = 1,
            East = 2,
            West = 3,
            North_East = 4,
            North_West = 5,
            South_East = 6,
            South_West = 7
        }

        public GraphicsDevice Graphics;
        public Vector2 Position;
        public MovementDirection Direction { get; set; }
        public bool IsMovingUp, IsMovingDown, IsMovingLeft, IsMovingRight;

        public IPlayerObject(GraphicsDevice Graphics)
        {
            this.Graphics = Graphics;
            Initialize();
        }

        public abstract void Initialize();
        public abstract void LoadContent(ContentManager content);
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
