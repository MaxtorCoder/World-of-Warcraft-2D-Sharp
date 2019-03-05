using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using WoW_2D.Gfx;
using WoW_2D.Gfx.Animation;

namespace WoW_2D.States
{
    /// <summary>
    /// Testing purposes? :P
    /// </summary>
    public class TestState : IGameState
    {
        private SpriteSheet spritesheet;
        private AnimationManager animationManager;

        private Animation animation;

        public TestState(GraphicsDevice graphics) : base(graphics) { }

        public override void Initialize()
        {
            animationManager = new AnimationManager();
            animation = new Animation() { Name = "down_movement" };
        }

        public override void LoadContent(ContentManager Content)
        {
            spritesheet = new SpriteSheet(Graphics);
            spritesheet.SetTexture(Content.Load<Texture2D>("Sprites/Player/Human"));

            /** We multiply the position x or y by 32 to move indices in the spritesheet. **/
            animation.AddFrame(spritesheet.GetSprite(new Point(1 * 32, 0), new Point(32)), 200);
            animation.AddFrame(spritesheet.GetSprite(new Point(2 * 32, 0), new Point(32)), 200);
            animation.AddFrame(spritesheet.GetSprite(new Point(1 * 32, 0), new Point(32)), 200);
            animation.AddFrame(spritesheet.GetSprite(new Point(0 * 32, 0), new Point(32)), 200);
            animationManager.AddAnimation(animation);
            animationManager.SetActive(x => x.Name == "down_movement");
        }

        public override void UnloadContent()
        {}

        public override void Update(GameTime gameTime)
        {
            animationManager.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Graphics.Clear(Color.Black);

            animationManager.Draw(spriteBatch);
        }

        public override string ToString()
        {
            return null;
        }
    }
}
