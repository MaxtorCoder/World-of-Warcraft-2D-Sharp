using Framework.Entity;
using Framework.Network.Packet.Client;
using Framework.Network.Packet.OpCodes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using MonoGameHelper.GameState;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoW_2D.Gfx;
using WoW_2D.Gfx.Gui;
using WoW_2D.Network;
using WoW_2D.Utils;
using WoW_2D.World;

namespace WoW_2D.States
{
    /// <summary>
    /// Loads the needed map, local player information, etc,.
    /// </summary>
    public class LoadingState : IGameState
    {
        private ContentManager _content;
        private bool isLoading = false;
        private bool hasStarted = false;
        private bool shouldRequestData = false;

        public LoadingState(GraphicsDevice graphics) : base(graphics) { }

        public override void Initialize()
        {
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);
            _content = content;
        }

        public override void UnloadContent()
        {
        }

        public override void Update(GameTime gameTime)
        {
            if (!isLoading)
                isLoading = true;

            if (isLoading)
            {
                if (!hasStarted)
                {
                    hasStarted = true;
                    MapManager.LoadMap(_content);
                }
                else
                {
                    if (WorldofWarcraft.Map.Player != null)
                    {
                        WorldofWarcraft.Map.Initialize(graphics);
                        WorldofWarcraft.Map.Player.LoadContent(_content);
                        if (!shouldRequestData)
                        {
                            NetworkManager.State = NetworkManager.NetworkState.RetrievingMOTD;
                            shouldRequestData = true;
                        }
                    }
                }
            }

            HandleNetUpdates();
        }

        private void HandleNetUpdates()
        {
            if (Global.IsRequestingLoadingData)
                return;
            switch (NetworkManager.State)
            {
                case NetworkManager.NetworkState.RetrievingMOTD:
                    NetworkManager.Send(new CMSG_Generic() { Type = (byte)Requests.MOTD }, NetworkManager.Direction.World);
                    Global.IsRequestingLoadingData = true;
                    break;
                case NetworkManager.NetworkState.RetrievingPlayers:
                    NetworkManager.Send(new CMSG_Generic() { Type = (byte)Requests.OnlineList }, NetworkManager.Direction.World);
                    Global.IsRequestingLoadingData = true;
                    break;
                case NetworkManager.NetworkState.Play:
                    GameStateManager.EnterState(5);
                    break;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            graphics.Clear(Color.Black);

            GuiNotification.Draw(GfxManager.GetFont("main_font"), spriteBatch, "Loading content...");
        }
    }
}
