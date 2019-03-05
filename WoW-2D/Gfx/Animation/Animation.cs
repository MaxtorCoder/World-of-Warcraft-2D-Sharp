using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoW_2D.Gfx.Animation
{
    /// <summary>
    /// Handles animation data.
    /// </summary>
    public class Animation
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }

        private Frame frame;
        private int frameIndex = 0;
        private int tickCounter = 0;

        private List<Frame> frames = new List<Frame>();

        /// <summary>
        /// Adds a frame to this animation with the specified texture and duration (in ms).
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="duration"></param>
        public void AddFrame(Texture2D texture, int duration)
        {
            frames.Add(new Frame(texture, duration));
        }

        public void Update(GameTime gameTime)
        {
            if (frame == null)
                frame = frames[0];
            tickCounter += 1 * gameTime.ElapsedGameTime.Milliseconds;
            if (tickCounter >= frame.Duration)
            {
                if (frameIndex >= frames.Count - 1)
                    frameIndex = 0;
                else
                    frameIndex++;
                tickCounter = 0;
                frame = frames[frameIndex];
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(frame.Texture, new Vector2(), Color.White);
            spriteBatch.End();
        }

        /// <summary>
        /// Handles animation-frame data.
        /// </summary>
        protected class Frame
        {
            public Texture2D Texture { get; set; }
            public int Duration { get; set; }

            public Frame(Texture2D texture, int duration)
            {
                Texture = texture;
                Duration = duration;
            }
        }
    }
}
