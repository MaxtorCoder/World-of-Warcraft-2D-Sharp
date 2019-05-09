using Framework.Network.Packet.Client;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using MonoGameHelper.Gfx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoW_2D.Network;
using WoW_2D.Utils;

namespace WoW_2D.Gfx.Gui.Ui
{
    /// <summary>
    /// The "Esc" menu.
    /// </summary>
    public class EscapeMenuUI : UiControl
    {
        private BitmapFont font;

        private RectangleF menuBox;

        private ButtonUI interfaceButton;
        private ButtonUI exitButton;

        private InterfaceOptionsUI interfaceOptionsUI;

        public EscapeMenuUI(GraphicsDevice graphics) : base(graphics)
        {
            menuBox = new RectangleF(0, 0, 175, 256);
            menuBox.Position = new Point2(graphics.Viewport.Width / 2 - menuBox.Width / 2, graphics.Viewport.Height / 2 - menuBox.Height / 2);

            interfaceButton = new ButtonUI(graphics)
            {
                Text = "Interface",
                Width = menuBox.Width - 15,
                Height = 25
            };
            interfaceButton.Position = new Vector2(menuBox.Position.X + (menuBox.Width / 2 - interfaceButton.Width / 2), menuBox.Position.Y + interfaceButton.Height);
            interfaceButton.OnClicked += OnInterfaceClicked;

            exitButton = new ButtonUI(graphics)
            {
                Text = "Exit Game",
                Width = menuBox.Width - 15,
                Height = 25
            };
            exitButton.Position = new Vector2(menuBox.Position.X + (menuBox.Width / 2 - exitButton.Width / 2), menuBox.Position.Y + (menuBox.Height - exitButton.Height - 5));
            exitButton.OnClicked += OnLogoutClicked;

            interfaceOptionsUI = new InterfaceOptionsUI(graphics);
        }

        public override void LoadContent(ContentManager content)
        {
            font = GfxManager.GetFont("small_font");

            interfaceOptionsUI.Font = font;
            interfaceOptionsUI.LoadContent(content);
        }

        public override void Update()
        {
            if (IsVisible)
            {
                interfaceButton.Update();
                exitButton.Update();
            }

            interfaceOptionsUI.Update();

            if (IsVisible || interfaceOptionsUI.IsVisible)
                Global.ShouldHideUI = true;
            else
                Global.ShouldHideUI = false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsVisible)
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(font, "Options", new Vector2(menuBox.X + (menuBox.Width / 2), menuBox.Y - font.LineHeight / 2), Color.Black, 0f, new Vector2(font.MeasureString("Options").Width / 2, font.MeasureString("Options").Height / 2), 1f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(font, "Options", new Vector2(menuBox.X + (menuBox.Width / 2) + 1, menuBox.Y - font.LineHeight / 2 + 1), WorldofWarcraft.DefaultYellow, 0f, new Vector2(font.MeasureString("Options").Width / 2, font.MeasureString("Options").Height / 2), 1f, SpriteEffects.None, 0f);
                spriteBatch.End();

                spriteBatch.Begin(blendState: BlendState.NonPremultiplied);
                spriteBatch.FillRectangle(menuBox, new Color(0f, 0f, 0f, 0.45f));
                spriteBatch.DrawRectangle(menuBox, Color.DarkGray, 1f);
                spriteBatch.End();

                interfaceButton.Draw(font, spriteBatch);
                exitButton.Draw(font, spriteBatch);
            }

            interfaceOptionsUI.Draw(spriteBatch);
        }

        private void OnInterfaceClicked()
        {
            IsVisible = false;
            interfaceOptionsUI.IsVisible = true;
        }

        private void OnLogoutClicked() => Global.ShouldExit = true;

        public override void Activator()
        {
            if (interfaceOptionsUI.IsVisible)
            {
                interfaceOptionsUI.IsVisible = false;
                interfaceOptionsUI.Activator();
            }
            IsVisible = !IsVisible;
        }
    }
}
