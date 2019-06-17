using DiscordRPC;
using Framework.Entity;
using Framework.Network.Packet;
using Framework.Network.Packet.OpCodes;
using Framework.Utils;
using INI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameHelper.GameState;
using MonoGameHelper.Gfx;
using MonoGameHelper.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using WoW_2D.Gfx;
using WoW_2D.Gfx.Gui;
using WoW_2D.Network;
using WoW_2D.Network.Handler;
using WoW_2D.States;
using WoW_2D.Utils;
using WoW_2D.World;
using WoW_2D.World.GameObject;

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
        public static DiscordRpcClient Discord;
        public static INIFile ClientSettings;

        public static IPEndPoint Realmlist;
        public static Realm Realm;
        public static List<RealmCharacter> RealmCharacters = new List<RealmCharacter>(7);

        public static WoWWorld World { get; set; }
        public static Queue<WorldCharacter> PlayerQueue = new Queue<WorldCharacter>();

        public WorldofWarcraft()
        {
            ClientSettings = INIFile.Instance;
            ClientSettings.Load("Data/client.ini");

            Realmlist = new IPEndPoint(IPAddress.Parse(
                ClientSettings.GetSection("Network").GetString("ip")),
                ClientSettings.GetSection("Network").GetInt("port"));

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
            GfxManager.Load(Content);
            ModelManager.Load(GraphicsDevice);
            
            Utils.Global.HumanSpritesheet = new SpriteSheet(GraphicsDevice).SetTexture(GfxManager.GetTexture("human_spritesheet"));
            Utils.Global.RaceSpritesheet = new SpriteSheet(GraphicsDevice).SetTexture(GfxManager.GetTexture("race_spritesheet"));
            Utils.Global.ClassSpritesheet = new SpriteSheet(GraphicsDevice).SetTexture(GfxManager.GetTexture("class_spritesheet"));

            Utils.Global.Classes.Add(new ClassType(Class.Warrior) { Races = new List<Race>() { Race.Human, Race.NightElf, Race.Dwarf, Race.Gnome, Race.Tauren, Race.Undead, Race.Troll, Race.Orc } });
            Utils.Global.Classes.Add(new ClassType(Class.Paladin) { Races = new List<Race>() { Race.Human, Race.Dwarf } });
            Utils.Global.Classes.Add(new ClassType(Class.Rogue) { Races = new List<Race>() { Race.Human, Race.NightElf, Race.Dwarf, Race.Gnome, Race.Undead, Race.Troll, Race.Orc } });
            Utils.Global.Classes.Add(new ClassType(Class.Priest) { Races = new List<Race>() { Race.Human, Race.NightElf, Race.Dwarf, Race.Gnome, Race.Undead, Race.Troll } });
            Utils.Global.Classes.Add(new ClassType(Class.Mage) { Races = new List<Race>() { Race.Human, Race.Gnome, Race.Undead, Race.Troll } });
            Utils.Global.Classes.Add(new ClassType(Class.Warlock) { Races = new List<Race>() { Race.Human, Race.Gnome, Race.Orc, Race.Undead } });
            Utils.Global.Classes.Add(new ClassType(Class.Hunter) { Races = new List<Race>() { Race.NightElf, Race.Dwarf, Race.Gnome, Race.Tauren, Race.Troll, Race.Orc } });
            Utils.Global.Classes.Add(new ClassType(Class.Druid) { Races = new List<Race>() { Race.NightElf, Race.Tauren } });
            Utils.Global.Classes.Add(new ClassType(Class.Shaman) { Races = new List<Race>() { Race.Orc, Race.Tauren, Race.Troll } });

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
            PacketRegistry.DefineHandler((byte)ServerOpcodes.Opcodes.SMSG_CHAT, WorldHandler.HandleChatMessage);
            PacketRegistry.DefineHandler((byte)ServerOpcodes.Opcodes.SMSG_CONNECTION_ADD, WorldHandler.HandleConnectionAdd);
            PacketRegistry.DefineHandler((byte)ServerOpcodes.Opcodes.SMSG_CONNECTION_MOVE, WorldHandler.HandleConnectionMovement);
            PacketRegistry.DefineHandler((byte)ServerOpcodes.Opcodes.SMSG_CONNECTION_REMOVE, WorldHandler.HandleConnectionRemove);
            PacketRegistry.DefineHandler((byte)ServerOpcodes.Opcodes.SMSG_DISCONNECT, WorldHandler.HandleDisconnect);
            PacketRegistry.DefineHandler((byte)ServerOpcodes.Opcodes.SMSG_ONLINE, WorldHandler.HandleOnlineList);
            PacketRegistry.DefineHandler((byte)ServerOpcodes.Opcodes.SMSG_CREATURE, WorldHandler.HandleCreature);
            PacketRegistry.DefineHandler((byte)ServerOpcodes.Opcodes.SMSG_CREATURE_LIST, WorldHandler.HandleCreatureList);

            Discord = new DiscordRpcClient("572201528799264770");
            Discord.Initialize();

            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead(new Uri("http://localhost/wow2d/alert.txt")))
                using (var reader = new StreamReader(stream, Encoding.UTF8, true))
                {
                    Utils.Global.BreakingNewsText = reader.ReadToEnd();
                    Utils.Global.ShouldDrawBreakingNews = true;
                }
            } catch { Utils.Global.ShouldDrawBreakingNews = false; }

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

            if (PlayerQueue.Count > 0)
            {
                var playerData = PlayerQueue.Dequeue();
                var playerToAdd = new PlayerMP() { Info = playerData };
                playerToAdd.Initialize();
                World.Players.Add(playerToAdd);
            }

            if (Utils.Global.FullscreenFlag == 1)
            {
                Utils.Global.FullscreenFlag = 0;
                graphics.ToggleFullScreen();
            }

            if (Utils.Global.ShouldExit)
                Exit();
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
