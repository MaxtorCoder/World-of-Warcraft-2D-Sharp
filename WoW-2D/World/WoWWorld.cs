using Framework.Entity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoW_2D.World.GameObject;
using static Framework.Entity.Vector;
using static Framework.Network.Packet.Server.SMSG_Creature;

namespace WoW_2D.World
{
    /// <summary>
    /// Handles currently-loaded map data.
    /// </summary>
    public class WoWWorld
    {
        // TODO: Change to only render/update players within view of the camera.

        public TiledMap ZoneMap { get; set; } = null;
        public Player Player { get; set; } = null;
        public List<PlayerMP> Players = new List<PlayerMP>();
        public List<NPC> Creatures = new List<NPC>();
        public Camera Camera = null;

        public bool HasInitialized { get; set; } = false;

        private TiledMapRenderer mapRenderer;

        public void Initialize(GraphicsDevice graphics)
        {
            Player.Initialize();
            mapRenderer = new TiledMapRenderer(graphics);
            Camera = new Camera(graphics)
            {
                ClampBounds = new RectangleF(Point2.Zero, new Size2(WorldofWarcraft.World.ZoneMap.WidthInPixels, WorldofWarcraft.World.ZoneMap.HeightInPixels))
            };

            HasInitialized = true;
        }

        public void Update(GameTime gameTime)
        {
            Player.Update(gameTime);
            foreach (var player in Players)
                player.Update(gameTime);
            foreach (var npc in Creatures)
                npc.Update(gameTime);
            Camera.Update(new Vector2(Player.Info.Vector.X, Player.Info.Vector.Y));
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, Camera.GetViewMatrix());
            foreach (var layer in ZoneMap.Layers)
            {
                switch (layer.Name.ToLower())
                {
                    case "background":
                        mapRenderer.Draw(layer, viewMatrix: Camera.GetViewMatrix());
                        Player.Draw(spriteBatch, gameTime);
                        foreach (var player in Players)
                            player.Draw(spriteBatch, gameTime);
                        foreach (var npc in Creatures)
                            npc.Draw(spriteBatch, gameTime);
                        spriteBatch.End();
                        break;
                    case "foreground":
                        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, Camera.GetViewMatrix());
                        mapRenderer.Draw(layer, viewMatrix: Camera.GetViewMatrix());
                        spriteBatch.End();
                        break;
                }
            }

            if (WorldofWarcraft.ClientSettings.GetSection("Interface").GetBool("players"))
                DrawPlayerNames(spriteBatch);
        }

        private void DrawPlayerNames(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            for (int i = 0; i < Players.Count; i++)
            {
                var player = Players[i];
                var playerBoundingBox = player.BoundingBox;

                /*
                if (WorldofWarcraft.Map.Player.GetCamera().BoundingRectangle.Contains(playerBoundingBox.Position))
                {
                    var translatedBoundPosition = WorldofWarcraft.Map.Player.GetCamera().WorldToScreen(playerBoundingBox.X, playerBoundingBox.Y);

                    spriteBatch.DrawString(GfxManager.GetFont("small_font"), player.WorldData.Name,
                new Vector2(translatedBoundPosition.X + 16f - GfxManager.GetFont("small_font").MeasureString(WorldofWarcraft.Map.Player.WorldData.Name).Width / 2 + 1f, translatedBoundPosition.Y - GfxManager.GetFont("small_font").LineHeight + 1f), Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(GfxManager.GetFont("small_font"), player.WorldData.Name,
                        new Vector2(translatedBoundPosition.X + 16f - GfxManager.GetFont("small_font").MeasureString(WorldofWarcraft.Map.Player.WorldData.Name).Width / 2, translatedBoundPosition.Y - GfxManager.GetFont("small_font").LineHeight), WorldofWarcraft.DefaultYellow, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                }
                */
            }
            spriteBatch.End();
        }

        public void UpdatePlayerMP(string name, float x, float y, MoveDirection direction, bool isMoving)
        {
            var player = Players.Find(p => p.Info.Name.ToLower() == name.ToLower());
            if (player != null)
            {
                player.Info.Vector.X = x;
                player.Info.Vector.Y = y;
                player.Info.Vector.Direction = direction;
                player.IsMoving = isMoving;
            }
        }

        /// <summary>
        /// Removes a player from the world.
        /// </summary>
        /// <param name="name"></param>
        public void RemovePlayer(string name)
        {
            var player = Players.Find(x => x.Info.Name.ToLower() == name.ToLower());
            if (player != null)
                Players.Remove(player);
        }

        public void UpdateCreature(ClientCreature creature, CreatureState state)
        {
            NPC npc = null;
            switch (state)
            {
                case CreatureState.Add:
                    var newNpc = new NPC()
                    {
                        GUID = creature.GUID,
                        ID = creature.ID,
                        ModelID = creature.ModelID,
                        Name = creature.Name,
                        SubName = creature.SubName,
                        Stats = creature.Stats,
                        Vector = creature.Vector
                    };
                    newNpc.Initialize();
                    Creatures.Add(newNpc);
                    break;
                case CreatureState.Move:
                    npc = Creatures.Find(c => c.GUID == creature.GUID);
                    if (npc != null)
                    {
                        npc.IsMoving = true;
                        npc.Vector = creature.Vector;
                    }
                    break;
                case CreatureState.MoveStop:
                    npc = Creatures.Find(c => c.GUID == creature.GUID);
                    if (npc != null)
                    {
                        npc.IsMoving = false;
                        npc.Vector = creature.Vector;
                    }
                    break;
            }
        }
    }
}
