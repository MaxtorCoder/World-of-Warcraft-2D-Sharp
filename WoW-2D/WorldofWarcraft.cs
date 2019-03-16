using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameHelper.GameState;
using MonoGameHelper.Gfx;
using MonoGameHelper.Utils;
using System;
using System.Diagnostics;
using System.Reflection;
using WoW_2D.Gfx.Gui;
using WoW_2D.States;

namespace WoW_2D
{
    /// <summary>
    /// The main engine of WoW-2D.
    /// </summary>
    public class WorldofWarcraft : Game
    {
        private GraphicsDeviceManager graphics;

        private static readonly string Version = $"{FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion}";
        public static string VersionStr = $"v{Version}";
        public static Color DefaultYellow = new Color(223, 195, 15);

        public WorldofWarcraft()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;

            Content.RootDirectory = "Content";
            Window.Title = "World of Warcraft 2D";
            Window.TextInput += OnKeyTyped;
            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            GameStateManager.SetContentManager(Content);
            GameStateManager.AddState(new MainMenuState(GraphicsDevice) { ID = 1 });
            GameStateManager.AddState(new TestState(GraphicsDevice) { ID = 2 });

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {}

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {}

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            InputHandler.Update();
            GameStateManager.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GameStateManager.Draw(gameTime);
        }

        /// <summary>
        /// Send the typed-key to all controls in the active state supporting text input.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnKeyTyped(object sender, TextInputEventArgs e)
        {
            var currentlyActiveState = GameStateManager.GetActiveState();
            var controlsSupportingTextInput = currentlyActiveState.GetGUIControlsSupporting(x => x.IsAcceptingTextInput);
            if (controlsSupportingTextInput.Count > 0)
            {
                foreach (IGuiControl control in controlsSupportingTextInput)
                    control.OnKeyTyped(e.Key, e.Character);
            }
        }
    }
}
