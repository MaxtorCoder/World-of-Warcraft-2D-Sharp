using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameHelper.GameState;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoW_2D.States
{
    /// <summary>
    /// The game state.
    /// </summary>
    public class GameState : IGameState
    {
        public GameState(GraphicsDevice graphics) : base(graphics) { }

        public override void Initialize()
        {
        }

        public override void UnloadContent()
        {
        }

        public override void Update(GameTime gameTime)
        {
            WorldofWarcraft.Map.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            WorldofWarcraft.Map.Draw(spriteBatch, gameTime);
        }
    }
}
