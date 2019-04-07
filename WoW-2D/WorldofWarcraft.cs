using Framework.Entity;
using Framework.Network.Packet;
using Framework.Network.Packet.OpCodes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameHelper.GameState;
using MonoGameHelper.Gfx;
using MonoGameHelper.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using WoW_2D.Gfx;
using WoW_2D.Gfx.Gui;
using WoW_2D.Network;
using WoW_2D.Network.Handler;
using WoW_2D.States;
using WoW_2D.Utils;
using WoW_2D.World;

namespace WoW_2D
{
    /// <summary>
    /// The main engine of WoW-2D.
    /// </summary>
    public class WorldofWarcraft : Game
    {
        private GraphicsDeviceManager graphics;

        private static readonly string Version = $"" +
            $"{FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileMajorPart}." +
            $"{FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileMinorPart}." +
            $"{FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileBuildPart}";
        public static string VersionStr = $"v{Version}";
        public static Color DefaultYellow = new Color(223, 195, 15);
        public readonly static IPEndPoint Realmlist = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1337);
        public static Realm Realm;
        public static List<RealmCharacter> RealmCharacters = new List<RealmCharacter>(7);
        public static WorldCharacter Character;
        public static ClientMap Map { get; set; }

        public WorldofWarcraft()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1280,
                PreferredBackBufferHeight = 720
            };

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
            Global.HumanSpritesheet = new SpriteSheet(GraphicsDevice);
            Global.HumanSpritesheet.SetTexture(Content.Load<Texture2D>("Sprites/Human/Human"));

            GameStateManager.SetContentManager(Content);
            GameStateManager.AddState(new MainMenuState(GraphicsDevice));
            GameStateManager.AddState(new CharacterSelectState(GraphicsDevice));
            GameStateManager.AddState(new CharacterCreateState(GraphicsDevice));
            GameStateManager.AddState(new LoadingState(GraphicsDevice));
            GameStateManager.AddState(new GameState(GraphicsDevice));

            GuiNotification.Initialize(GraphicsDevice, Content);

            PacketRegistry.DefineHandler((byte)ServerOpcodes.Opcodes.SMSG_LOGON, AuthHandler.HandleLogin);
            PacketRegistry.DefineHandler((byte)ServerOpcodes.Opcodes.SMSG_REALMLIST, AuthHandler.HandleRealmlist);
            PacketRegistry.DefineHandler((byte)ServerOpcodes.Opcodes.SMSG_CHARACTER_CREATE, CharHandler.HandleCreation);
            PacketRegistry.DefineHandler((byte)ServerOpcodes.Opcodes.SMSG_CHARACTER_LIST, CharHandler.HandleList);
            PacketRegistry.DefineHandler((byte)ServerOpcodes.Opcodes.SMSG_CHARACTER_DELETE, CharHandler.HandleDeletion);
            PacketRegistry.DefineHandler((byte)ServerOpcodes.Opcodes.SMSG_WORLD, WorldHandler.HandleWorldLogin);
            PacketRegistry.DefineHandler((byte)ServerOpcodes.Opcodes.SMSG_WORLD_ENTER, WorldHandler.HandleWorldEnter);

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
