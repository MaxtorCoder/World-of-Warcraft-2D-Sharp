using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace WoW_2D.Gfx.Gui.Ui
{
    /// <summary>
    /// Game interface options ui.
    /// </summary>
    public class InterfaceOptionsUI : UiControl
    {
        public BitmapFont Font { get; set; }

        private RectangleF bounds;
        private RectangleF renderPanel;
        
        private CheckboxUI renderPlayerNames;
        private CheckboxUI renderNPCNames;
        private CheckboxUI renderMobNames;

        private ButtonUI applyButton;

        public InterfaceOptionsUI(GraphicsDevice graphics) : base(graphics)
        {
            bounds = new RectangleF(0, 0, 400, 500);
            bounds.Position = new Point2(graphics.Viewport.Width / 2 - bounds.Width / 2, graphics.Viewport.Height / 2 - bounds.Height / 2);

            renderPanel = new RectangleF(0, 0, bounds.Width / 3, 250);
            renderPanel.Position = new Point2(bounds.Position.X + 15f, bounds.Position.Y + 25f);

            renderPlayerNames = new CheckboxUI(graphics)
            {
                Text = "Players",
                DrawBackdrop = true,
                Position = new Vector2(renderPanel.X + 4f, renderPanel.Y + 4f),
                IsChecked = WorldofWarcraft.ClientSettings.GetSection("Interface").GetBool("players")
            };

            renderNPCNames = new CheckboxUI(graphics)
            {
                Text = "NPC's",
                DrawBackdrop = true,
                Position = new Vector2(renderPanel.X + 4f, renderPlayerNames.Position.Y + 20f)
            };

            renderMobNames = new CheckboxUI(graphics)
            {
                Text = "Mobs",
                DrawBackdrop = true,
                Position = new Vector2(renderPanel.X + 4f, renderNPCNames.Position.Y + 20f)
            };

            applyButton = new ButtonUI(graphics)
            {
                Text = "Apply",
                Width = 50,
                Height = 25
            };
            applyButton.Position = new Vector2(bounds.X + (bounds.Width - applyButton.Width - 10f), bounds.Y + (bounds.Height - applyButton.Height - 10f));
            applyButton.OnClicked += OnApplyClicked;
        }

        public override void LoadContent(ContentManager content)
        {
            renderPlayerNames.LoadContent(content);
            renderNPCNames.LoadContent(content);
            renderMobNames.LoadContent(content);
        }

        public override void Update()
        {
            renderPlayerNames.Update();
            renderNPCNames.Update();
            renderMobNames.Update();
            applyButton.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsVisible)
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(Font, "Interface", new Vector2(bounds.X + (bounds.Width / 2), bounds.Y - Font.LineHeight / 2), Color.Black, 0f, new Vector2(Font.MeasureString("Interface").Width / 2, Font.MeasureString("Interface").Height / 2), 1f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(Font, "Interface", new Vector2(bounds.X + (bounds.Width / 2) + 1, bounds.Y - Font.LineHeight / 2 + 1), WorldofWarcraft.DefaultYellow, 0f, new Vector2(Font.MeasureString("Interface").Width / 2, Font.MeasureString("Interface").Height / 2), 1f, SpriteEffects.None, 0f);
                spriteBatch.End();

                spriteBatch.Begin(blendState: BlendState.NonPremultiplied);
                spriteBatch.FillRectangle(bounds, new Color(0f, 0f, 0f, 0.75f));
                spriteBatch.DrawRectangle(bounds, Color.Gray);
                spriteBatch.End();

                spriteBatch.Begin();
                spriteBatch.DrawString(Font, "Names", new Vector2(renderPanel.X + (renderPanel.Width / 2 - Font.MeasureString("Names").Width / 2), renderPanel.Y - Font.MeasureString("Names").Height), WorldofWarcraft.DefaultYellow);
                spriteBatch.DrawRectangle(renderPanel, Color.Gray);
                spriteBatch.End();

                renderPlayerNames.Draw(spriteBatch);
                renderNPCNames.Draw(spriteBatch);
                renderMobNames.Draw(spriteBatch);

                applyButton.Draw(Font, spriteBatch);
            }
        }

        public override void Activator()
        {
            renderPlayerNames.IsChecked = WorldofWarcraft.ClientSettings.GetSection("Interface").GetBool("players");
        }

        private void OnApplyClicked()
        {
            WorldofWarcraft.ClientSettings.GetSection("Interface").SetValueOfKey("players", renderPlayerNames.IsChecked.ToString());
            WorldofWarcraft.ClientSettings.Save();
            IsVisible = false;
        }
    }
}
