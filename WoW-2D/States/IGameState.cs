using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoW_2D.States
{
    /// <summary>
    /// An abstract state for game-state types.
    /// </summary>
    public abstract class IGameState
    {
        public int ID { get; set; }
        public bool IsActive { get; set; }
        protected GraphicsDevice Graphics;

        public IGameState(GraphicsDevice Graphics)
        {
            this.Graphics = Graphics;
        }

        public abstract void Initialize();
        public abstract void LoadContent(ContentManager Content);
        public abstract void UnloadContent();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract override string ToString();
    }
}
