using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using WoW_2D.Gfx.Gui;

namespace WoW_2D.States
{
    /// <summary>
    /// An abstract state for game-state types.
    /// </summary>
    public abstract class IGameState
    {
        public int ID { get; set; }
        public bool IsActive { get; set; }
        public List<IGuiControl> Controls = new List<IGuiControl>();
        protected GraphicsDevice Graphics;
        protected WorldofWarcraft WorldofWarcraft;

        public IGameState(WorldofWarcraft wow, GraphicsDevice Graphics)
        {
            this.Graphics = Graphics;
            this.WorldofWarcraft = wow;
        }

        public abstract void Initialize();
        public abstract void LoadContent(ContentManager Content);
        public abstract void UnloadContent();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);

        /// <summary>
        /// Get all gui controls supporting the specified predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public List<IGuiControl> GetGUIControlsSupporting(Predicate<IGuiControl> predicate)
        {
            return Controls.FindAll(predicate);
        }
    }
}
