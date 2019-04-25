using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoW_2D.World.GameObject;
using static Framework.Entity.Vector;

namespace WoW_2D.World
{
    /// <summary>
    /// Handles currently-loaded map data.
    /// </summary>
    public class ClientMap
    {
        public TiledMap ZoneMap { get; set; }
        public Player Player { get; set; }

        private GraphicsDevice graphics;
        private TiledMapRenderer mapRenderer;

        public void Initialize(GraphicsDevice graphics)
        {
            this.graphics = graphics;
            mapRenderer = new TiledMapRenderer(graphics);
            Player.Initialize(graphics);
        }

        public void Update(GameTime gameTime)
        {
            Player.Update(gameTime);
            foreach (var player in WorldofWarcraft.Players)
                player.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            graphics.Clear(Color.Black);

            spriteBatch.Begin(transformMatrix: Player.GetCamera().GetViewMatrix());
            foreach (var layer in ZoneMap.Layers)
            {
                switch (layer.Name.ToLower())
                {
                    case "background":
                        mapRenderer.Draw(layer, viewMatrix: Player.GetCamera().GetViewMatrix());
                        Player.Draw(spriteBatch, gameTime);
                        foreach (var player in WorldofWarcraft.Players)
                            player.Draw(spriteBatch, gameTime);
                        spriteBatch.End();
                        break;
                    case "foreground":
                        spriteBatch.Begin(transformMatrix: Player.GetCamera().GetViewMatrix(), samplerState: SamplerState.PointClamp);
                        mapRenderer.Draw(layer, viewMatrix: Player.GetCamera().GetViewMatrix());
                        spriteBatch.End();
                        break;
                }
            }
        }

        public void UpdatePlayerMP(string name, float x, float y, MoveDirection direction, bool isMoving)
        {
            var player = WorldofWarcraft.Players.Find(p => p.WorldData.Name.ToLower() == name.ToLower());
            if (player != null)
            {
                player.WorldData.Vector.X = x;
                player.WorldData.Vector.Y = y;
                player.WorldData.Vector.Direction = direction;
                player.IsMoving = isMoving;
            }
        }
    }
}
