using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoW_2D.States
{
    /// <summary>
    /// Handles anything related to game-states.
    /// </summary>
    public class GameStateManager
    {
        private static GameStateManager instance;
        private ContentManager content;

        private List<IGameState> states = new List<IGameState>();

        public static GameStateManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new GameStateManager();
                return instance;
            }
        }

        /// <summary>
        /// Set the manager's content manager.
        /// </summary>
        /// <param name="content"></param>
        public void SetContentManager(ContentManager content)
        {
            this.content = content;
        }

        /// <summary>
        /// Add a state to the manager.
        /// </summary>
        /// <param name="state"></param>
        public void AddState(IGameState state)
        {
            var exists = states.Exists(x => x.ID == state.ID);
            if (!exists)
            {
                state.Initialize();
                states.Add(state);
                if (content != null)
                    state.LoadContent(content);
            }
        }

        /// <summary>
        /// Begin drawing and updating a specific scene.
        /// </summary>
        /// <param name="id"></param>
        public void EnterState(int id)
        {
            var state = states.Find(x => x.ID == id);
            if (state != null)
            {
                var currentlyActiveState = states.Find(x => x.IsActive);
                if (currentlyActiveState != null)
                    currentlyActiveState.IsActive = false;
                state.IsActive = true;
            }
        }

        /// <summary>
        /// Unload the content in all states.
        /// </summary>
        public void UnloadContent()
        {
            foreach (IGameState state in states)
                state.UnloadContent();
        }
        
        /// <summary>
        /// Update the active state.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            var state = states.Find(x => x.IsActive);
            if (state != null)
                state.Update(gameTime);
            else
                EnterState(1);
        }

        /// <summary>
        /// Draw the active state.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            var state = states.Find(x => x.IsActive);
            if (state != null)
                state.Draw(spriteBatch);
        }
    }
}
