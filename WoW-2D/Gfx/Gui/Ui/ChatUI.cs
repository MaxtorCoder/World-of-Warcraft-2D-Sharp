using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using WoW_2D.Utils;

namespace WoW_2D.Gfx.Gui.Ui
{
    /// <summary>
    /// The chat UI.
    /// </summary>
    public class ChatUI : UiControl
    {
        public GuiChatText TextField { get; set; }
        private RectangleF chatBox;
        private const float alphaMin = 0.50f;
        private const float alphaMax = 0.75f;
        private float alpha = alphaMin;

        public ChatUI(GraphicsDevice graphics) : base(graphics)
        {
            TextField = new GuiChatText(graphics)
            {
                Width = 365,
                Height = 25,
                BackgroundColor = new Color(0f, 0f, 0f, 0.75f),
                MaxCharacterLength = 256,
            };
            TextField.Position = new Vector2(graphics.Viewport.Width / 2 - TextField.Width / 2, graphics.Viewport.Height - 100);

            chatBox = new RectangleF(0, 0, 400, 175);
            chatBox.Position = new Point2(0, graphics.Viewport.Height - chatBox.Height);
        }

        public override void LoadContent(ContentManager content)
        {
            TextField.LoadContent(content);
        }

        public override void Update()
        {
            TextField.Update();
            WorldofWarcraft.Map.Player.CanMove = !TextField.IsVisible;

            var mousePosition = Mouse.GetState().Position;
            if (chatBox.Contains(mousePosition))
            {
                alpha += 0.02f;
                if (alpha > alphaMax)
                    alpha = alphaMax;
            }
            else
            {
                alpha -= 0.02f;
                if (alpha < alphaMin)
                    alpha = alphaMin;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            TextField.Draw(spriteBatch);

            if (!Global.ShouldHideUI)
            {
                spriteBatch.Begin(blendState: BlendState.NonPremultiplied);
                spriteBatch.FillRectangle(chatBox, new Color(0f, 0f, 0f, alpha));
                spriteBatch.DrawRectangle(chatBox, Color.Gray);
                spriteBatch.End();
            }
        }

        public void OnEnterPressed()
        {
            if (!Global.ShouldHideUI)
            {
                TextField.IsVisible = !TextField.IsVisible;
                if (!TextField.IsVisible)
                {
                    TextField.ResetText();

                }
            }
        }
    }
}
