using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace WoW_2D.Gfx.Animation
{
    /// <summary>
    /// Handles animation data.
    /// </summary>
    public class Animation
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsIdle { get; set; }

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

        /// <summary>
        /// Sets an idle frame for this animation.
        /// </summary>
        /// <param name="index"></param>
        public void SetIdleFrame(int index)
        {
            var frame = frames[index];
            if (frame != null)
                frame.IsFrameIdle = true;
        }

        /// <summary>
        /// Resets the animation back to the first index.
        /// </summary>
        public void Reset()
        {
            frame = frames[0];
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

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Begin();
            if (IsIdle)
                spriteBatch.Draw(frames.Find(x => x.IsFrameIdle).Texture, position, Color.White);
            else
                spriteBatch.Draw(frame.Texture, position, Color.White);
            spriteBatch.End();
        }

        /// <summary>
        /// Handles animation-frame data.
        /// </summary>
        protected class Frame
        {
            public Texture2D Texture { get; set; }
            public int Duration { get; set; }
            public bool IsFrameIdle;

            public Frame(Texture2D texture, int duration)
            {
                Texture = texture;
                Duration = duration;
            }
        }
    }
}
