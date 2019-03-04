using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WoW_2D.States
{
    public class TestState : IGameState
    {
        public TestState(GraphicsDevice graphics) : base(graphics) { }

        public override void Initialize()
        {
        }

        public override void LoadContent(ContentManager Content)
        {
        }

        public override void UnloadContent()
        {
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Graphics.Clear(Color.AntiqueWhite);
        }

        public override string ToString()
        {
            return null;
        }
    }
}
