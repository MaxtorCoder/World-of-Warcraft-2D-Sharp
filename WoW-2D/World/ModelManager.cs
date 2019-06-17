using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameHelper.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoW_2D.Gfx;

namespace WoW_2D.World
{
    /// <summary>
    /// Creates all models in the game.
    /// </summary>
    public class ModelManager
    {
        private static List<Model> Models = new List<Model>();

        public static void Load(GraphicsDevice graphics)
        {
            var modelCount = 0;
            var models = GfxManager.GetModels();
            foreach (var modelInfo in models)
            {
                modelCount++;
                var newModel = new Model()
                {
                    ID = modelCount,
                    Name = modelInfo.Key,
                    Spritesheet = new SpriteSheet(graphics).SetTexture(modelInfo.Value)
                };
                Models.Add(newModel);
            }
        }

        public static Model GetModel(Predicate<Model> predicate)
        {
            var model = Models.Find(predicate);
            var modelCopy = model.Copy();
            {
                var northAnimation = new Animation() { Name = "north_anim", Scale = 0.6f };
                var eastAnimation = new Animation() { Name = "east_anim", Scale = 0.6f };
                var southAnimation = new Animation() { Name = "south_anim", Scale = 0.6f };
                var westAnimation = new Animation() { Name = "west_anim", Scale = 0.6f };

                northAnimation.AddFrame(modelCopy.Spritesheet.GetSprite(new Point(1 * 32, 3 * 32), new Point(32)), 175);
                northAnimation.AddFrame(modelCopy.Spritesheet.GetSprite(new Point(2 * 32, 3 * 32), new Point(32)), 175);
                northAnimation.AddFrame(modelCopy.Spritesheet.GetSprite(new Point(1 * 32, 3 * 32), new Point(32)), 175);
                northAnimation.AddFrame(modelCopy.Spritesheet.GetSprite(new Point(0 * 32, 3 * 32), new Point(32)), 175);

                eastAnimation.AddFrame(modelCopy.Spritesheet.GetSprite(new Point(1 * 32, 2 * 32), new Point(32)), 175);
                eastAnimation.AddFrame(modelCopy.Spritesheet.GetSprite(new Point(2 * 32, 2 * 32), new Point(32)), 175);
                eastAnimation.AddFrame(modelCopy.Spritesheet.GetSprite(new Point(1 * 32, 2 * 32), new Point(32)), 175);
                eastAnimation.AddFrame(modelCopy.Spritesheet.GetSprite(new Point(0 * 32, 2 * 32), new Point(32)), 175);

                southAnimation.AddFrame(modelCopy.Spritesheet.GetSprite(new Point(1 * 32, 0 * 32), new Point(32)), 175);
                southAnimation.AddFrame(modelCopy.Spritesheet.GetSprite(new Point(2 * 32, 0 * 32), new Point(32)), 175);
                southAnimation.AddFrame(modelCopy.Spritesheet.GetSprite(new Point(1 * 32, 0 * 32), new Point(32)), 175);
                southAnimation.AddFrame(modelCopy.Spritesheet.GetSprite(new Point(0 * 32, 0 * 32), new Point(32)), 175);

                westAnimation.AddFrame(modelCopy.Spritesheet.GetSprite(new Point(1 * 32, 1 * 32), new Point(32)), 175);
                westAnimation.AddFrame(modelCopy.Spritesheet.GetSprite(new Point(2 * 32, 1 * 32), new Point(32)), 175);
                westAnimation.AddFrame(modelCopy.Spritesheet.GetSprite(new Point(1 * 32, 1 * 32), new Point(32)), 175);
                westAnimation.AddFrame(modelCopy.Spritesheet.GetSprite(new Point(0 * 32, 1 * 32), new Point(32)), 175);

                northAnimation.SetIdleFrame(0);
                eastAnimation.SetIdleFrame(0);
                southAnimation.SetIdleFrame(0);
                westAnimation.SetIdleFrame(0);

                modelCopy.Animations = new List<Animation>();
                modelCopy.Animations.AddRange(new[] { northAnimation, eastAnimation, southAnimation, westAnimation });
            }
            return modelCopy;
        }

        /// <summary>
        /// Holds model data.
        /// </summary>
        public class Model
        {
            public int ID { get; set; } = 0;
            public string Name { get; set; } = string.Empty;
            public SpriteSheet Spritesheet { get; set; } = null;
            public List<Animation> Animations { get; set; } = null;

            public Model Copy()
            {
                return (Model)MemberwiseClone();
            }
        }
    }
}
