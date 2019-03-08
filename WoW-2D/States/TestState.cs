using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using WoW_2D.GameObject;

namespace WoW_2D.States
{
    /// <summary>
    /// Testing purposes? :P
    /// </summary>
    public class TestState : IGameState
    {
        private PlayerObject player;

        public TestState(WorldofWarcraft wow, GraphicsDevice graphics) : base(wow, graphics) { }

        public override void Initialize()
        {
            player = new PlayerObject(Graphics);
        }

        public override void LoadContent(ContentManager Content)
        {
            player.LoadContent(Content);
        }

        public override void UnloadContent()
        {}

        public override void Update(GameTime gameTime)
        {
            player.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Graphics.Clear(Color.Black);

            player.Draw(spriteBatch);
        }
    }
}
