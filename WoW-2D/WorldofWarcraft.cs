using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using WoW_2D.Gfx.Gui;
using WoW_2D.States;
using WoW_2D.Utils;

namespace WoW_2D
{
    /// <summary>
    /// The main engine of WoW-2D.
    /// </summary>
    public class WorldofWarcraft : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private GameStateManager stateManager;

        public static Color DefaultYellow = new Color(223, 195, 15);

        public WorldofWarcraft()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            Window.Title = "World of Warcraft 2D";
            Window.TextInput += OnKeyTyped;
            IsMouseVisible = true;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            stateManager = GameStateManager.Instance;
            stateManager.SetContentManager(Content);
            stateManager.AddState(new MainMenuState(this, graphics.GraphicsDevice) { ID = 1 });
            stateManager.AddState(new TestState(this, graphics.GraphicsDevice) { ID = 2 });

            InputHandler.AddKeyPressHandler(delegate () { OnEscapePressed(); }, Keys.Escape);
            InputHandler.AddKeyPressHandler(delegate () { OnRightArrowPressed(); }, Keys.Right);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            InputHandler.Update();
            stateManager.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            stateManager.Draw(spriteBatch);
        }

        /// <summary>
        /// Send the typed-key to all controls in the active state supporting text input.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnKeyTyped(object sender, TextInputEventArgs e)
        {
            var currentlyActiveState = stateManager.GetActiveState();
            var controlsSupportingTextInput = currentlyActiveState.GetGUIControlsSupporting(x => x.IsAcceptingTextInput);
            if (controlsSupportingTextInput.Count > 0)
            {
                foreach (IGuiControl control in controlsSupportingTextInput)
                    control.OnKeyTyped(e.Key, e.Character);
            }
        }

        /// <summary>
        /// Fired when the Escape key has been pressed.
        /// </summary>
        private void OnEscapePressed()
        {
            Exit();
        }

        /// <summary>
        /// Fired when the right arrow has been pressed.
        /// </summary>
        private void OnRightArrowPressed()
        {
            stateManager.EnterState(2);
        }
    }
}
