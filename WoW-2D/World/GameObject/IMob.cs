using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoW_2D.Gfx;

namespace WoW_2D.World.GameObject
{
    /// <summary>
    /// An abstract mob class.
    /// </summary>
    public abstract class IMob
    {
        protected SpriteSheet SpriteSheet;
        protected List<Animation> Animations = new List<Animation>();

        public virtual void Initialize(GraphicsDevice graphics) { }
        public abstract void LoadContent(ContentManager content);
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);

        protected void SetAnimation(Predicate<Animation> predicate)
        {
            var animation = Animations.Find(predicate);
            if (animation != null)
            {
                if (!animation.IsActive)
                {
                    var currentlyActiveAnimation = Animations.Find(x => x.IsActive);
                    if (currentlyActiveAnimation != null)
                        currentlyActiveAnimation.IsActive = false;
                    animation.IsActive = true;
                }
            }
        }

        protected void SetAnimationIdle(bool isIdle)
        {
            var animation = Animations.Find(x => x.IsActive);
            if (animation != null)
            {
                animation.IsIdle = isIdle;
                if (isIdle)
                    animation.Reset();
            }
        }
    }
}
