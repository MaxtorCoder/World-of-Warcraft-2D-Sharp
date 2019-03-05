using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoW_2D.Gfx.Animation
{
    /// <summary>
    /// Manages animation data.
    /// Any game-object that should use an animation would contain an instance of AnimationManager in it's class.
    /// </summary>
    public class AnimationManager
    {
        private List<Animation> animations = new List<Animation>();

        /// <summary>
        /// Add an animation to this manager.
        /// </summary>
        /// <param name="animation"></param>
        public void AddAnimation(Animation animation)
        {
            animations.Add(animation);
        }

        /// <summary>
        /// Attempt to set an animation to the currently active one based on a predicate.
        /// </summary>
        /// <param name="predicate"></param>
        public void SetActive(Predicate<Animation> predicate)
        {
            var animation = animations.Find(predicate);
            if (animation != null)
            {
                var currentlyActiveAnimation = animations.Find(x => x.IsActive);
                if (currentlyActiveAnimation != null)
                    currentlyActiveAnimation.IsActive = false;
                animation.IsActive = true;
            }
        }

        public void Update(GameTime gameTime)
        {
            var currentlyActiveAnimation = animations.Find(x => x.IsActive);
            if (currentlyActiveAnimation != null)
                currentlyActiveAnimation.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var currentlyActiveAnimation = animations.Find(x => x.IsActive);
            if (currentlyActiveAnimation != null)
                currentlyActiveAnimation.Draw(spriteBatch);
        }
    }
}
