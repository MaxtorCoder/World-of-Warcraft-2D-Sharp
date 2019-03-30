using Framework.Entity;
using Framework.Network.Packet.Client;
using Framework.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using MonoGameHelper.GameState;
using MonoGameHelper.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoW_2D.Gfx.Gui;
using WoW_2D.Network;

namespace WoW_2D.States
{
    /// <summary>
    /// The character creation state.
    /// </summary>
    public class CharacterCreateState : IGameState
    {
        private BitmapFont font;

        private GuiButton acceptButton;
        private GuiButton backButton;
        private GuiButton randomizeButton;
        private GuiEntryText nameText;

        private RaceSelection raceSelection;

        private Texture2D humanTexture, dwarfTexture, nightelfTexture, gnomeTexture;
        private Texture2D selectedTexture;

        public CharacterCreateState(GraphicsDevice graphics) : base(graphics) { }

        public override void Initialize()
        {
            acceptButton = new GuiButton(graphics) { Text = "Accept" };
            acceptButton.OnClicked += OnAcceptButtonPressed;
            backButton = new GuiButton(graphics) { Text = "Back" };
            backButton.OnClicked += OnBackButtonPressed;
            randomizeButton = new GuiButton(graphics) { Text = "Randomize" };

            nameText = new GuiEntryText(graphics)
            {
                Width = 210,
                Height = 25,
                BackgroundColor = new Color(0f, 0f, 0f, 0.75f),
                MaxCharacterLength = 12,
                IsActive = true
            };

            Controls.Add(acceptButton);
            Controls.Add(backButton);
            Controls.Add(randomizeButton);
            Controls.Add(nameText);

            raceSelection = new RaceSelection(graphics);
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            font = content.Load<BitmapFont>("System/Font/font");

            acceptButton.LoadContent(content);
            acceptButton.Position = new Vector2(graphics.Viewport.Width - acceptButton.BaseTexture.Width - 15, graphics.Viewport.Height - acceptButton.BaseTexture.Height - 75);

            backButton.LoadContent(content);
            backButton.Position = new Vector2(acceptButton.Position.X, acceptButton.Position.Y + backButton.BaseTexture.Height + 25);

            randomizeButton.LoadContent(content);
            randomizeButton.Position = new Vector2(graphics.Viewport.Width / 2 - randomizeButton.BaseTexture.Width / 2, backButton.Position.Y);

            nameText.Position = new Vector2(graphics.Viewport.Width / 2 - nameText.Width / 2, randomizeButton.Position.Y - nameText.Height - 20);
            nameText.LoadContent(content);

            humanTexture = content.Load<Texture2D>("Sprites/Human/Human");

            raceSelection.LoadContent(content);
        }

        public override void UnloadContent()
        {
        }

        public override void Update(GameTime gameTime)
        {
            acceptButton.IsEnabled = (nameText.Text.Length >= 3 && NetworkManager.State == NetworkManager.NetworkState.Waiting) ? true : false;
            nameText.IsEnabled = (NetworkManager.State == NetworkManager.NetworkState.Waiting) ? true : false;

            raceSelection.Update();
            var race = raceSelection.GetSelectedRace();
            switch (race)
            {
                case Race.Human:
                    selectedTexture = humanTexture;
                    break;
            }
            acceptButton.Update();
            backButton.Update();
            randomizeButton.Update();
            nameText.Update();
        }

        public override void Draw(GameTime gameTime)
        {
            graphics.Clear(Color.Black);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            if (selectedTexture != null)
                spriteBatch.Draw(selectedTexture, new Vector2(graphics.Viewport.Width / 2 - selectedTexture.Width / 2, graphics.Viewport.Height / 2 - selectedTexture.Height / 2), null, Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.FlipVertically, 0f);
            raceSelection.Draw(font, spriteBatch);
            acceptButton.Draw(spriteBatch);
            backButton.Draw(spriteBatch);
            randomizeButton.Draw(spriteBatch);
            spriteBatch.DrawString(font, "Name", new Vector2(nameText.Position.X + (nameText.Width / 2 - font.MeasureString("Name").Width / 2), nameText.Position.Y - font.LineHeight), WorldofWarcraft.DefaultYellow);
            spriteBatch.End();
            nameText.Draw(spriteBatch);

            DrawNetworkUpdates();
        }

        private void DrawNetworkUpdates()
        {
            switch (NetworkManager.State)
            {
                case NetworkManager.NetworkState.CreatingCharacter:
                    GuiNotification.Draw(font, spriteBatch, "Creating character...");
                    break;
                case NetworkManager.NetworkState.CharacterExists:
                    GuiNotification.Draw(font, spriteBatch, "Name unavailable. Please try again.", true);
                    break;
                case NetworkManager.NetworkState.CreateCharacterError:
                    GuiNotification.Draw(font, spriteBatch, "Server error.", true);
                    break;
                case NetworkManager.NetworkState.CharacterCreated:
                    GameStateManager.EnterState(2);
                    break;
                case NetworkManager.NetworkState.ServerError:
                    GuiNotification.Draw(font, spriteBatch, "Server error.", true);
                    break;
            }
        }

        private void OnAcceptButtonPressed()
        {
            NetworkManager.State = NetworkManager.NetworkState.CreatingCharacter;
            NetworkManager.Send(new CMSG_Character_Create()
            {
                Name = nameText.Text.Trim(),
                Race = raceSelection.GetSelectedRace()
            }, NetworkManager.Direction.Auth);
        }

        private void OnBackButtonPressed()
        {
            GameStateManager.EnterState(2);
        }

        private void OnRandomizeButtonPressed()
        {

        }
    }

    /// <summary>
    /// Handles race selection.
    /// </summary>
    public class RaceSelection
    {
        protected GraphicsDevice graphics;
        protected Texture2D raceIcons;
        protected Color[] colorData;

        protected Rectangle humanSubRect, dwarfSubRect, nightelfSubRect, gnomeSubRect;
        protected RectangleF humanRect, dwarfRect, nightelfRect, gnomeRect;

        protected Rectangle orcSubRect, undeadSubRect, taurenSubRect, trollSubRect;
        protected RectangleF orcRect, undeadRect, taurenRect, trollRect;

        protected Texture2D humanIcon, dwarfIcon, nightelfIcon, gnomeIcon;
        protected Texture2D orcIcon, undeadIcon, taurenIcon, trollIcon;

        protected RectangleF racePanel;

        protected Texture2D allyBanner, hordeBanner;
        protected Vector2 allyBanner_Position, hordeBanner_Position;

        protected float iconScale = 0.5f;

        protected Race selectedRace = Race.Human;

        public RaceSelection(GraphicsDevice graphics) => this.graphics = graphics;

        public void LoadContent(ContentManager content)
        {
            raceIcons = content.Load<Texture2D>("Sprites/UI/RaceIcons");
            allyBanner = content.Load<Texture2D>("Sprites/UI/Ally_Banner");
            hordeBanner = content.Load<Texture2D>("Sprites/UI/Horde_Banner");

            racePanel = new RectangleF(0, 0, 225, 512);
            humanRect = new RectangleF(0, 0, 32, 32);
            dwarfRect = new RectangleF(0, 0, 32, 32);
            nightelfRect = new RectangleF(0, 0, 32, 32);
            gnomeRect = new RectangleF(0, 0, 32, 32);

            orcRect = new RectangleF(0, 0, 32, 32);
            undeadRect = new RectangleF(0, 0, 32, 32);
            taurenRect = new RectangleF(0, 0, 32, 32);
            trollRect = new RectangleF(0, 0, 32, 32);

            LoadIcons();
            SetPositions();
        }

        protected void LoadIcons()
        {
            LoadAllyIcons();
            LoadHordeIcons();
        }

        protected void LoadAllyIcons()
        {
            humanSubRect = new Rectangle(0, 0, 64, 64);
            humanIcon = new Texture2D(graphics, humanSubRect.Width, humanSubRect.Height);
            colorData = new Color[humanSubRect.Width * humanSubRect.Height];
            raceIcons.GetData(0, humanSubRect, colorData, 0, colorData.Length);
            humanIcon.SetData(colorData);

            dwarfSubRect = new Rectangle(2 * 64, 0, 64, 64);
            dwarfIcon = new Texture2D(graphics, dwarfSubRect.Width, dwarfSubRect.Height);
            colorData = new Color[dwarfSubRect.Width * dwarfSubRect.Height];
            raceIcons.GetData(0, dwarfSubRect, colorData, 0, colorData.Length);
            dwarfIcon.SetData(colorData);

            nightelfSubRect = new Rectangle(1 * 64, 0, 64, 64);
            nightelfIcon = new Texture2D(graphics, nightelfSubRect.Width, nightelfSubRect.Height);
            colorData = new Color[nightelfSubRect.Width * nightelfSubRect.Height];
            raceIcons.GetData(0, nightelfSubRect, colorData, 0, colorData.Length);
            nightelfIcon.SetData(colorData);

            gnomeSubRect = new Rectangle(3 * 64, 0, 64, 64);
            gnomeIcon = new Texture2D(graphics, gnomeSubRect.Width, gnomeSubRect.Height);
            colorData = new Color[gnomeSubRect.Width * gnomeSubRect.Height];
            raceIcons.GetData(0, gnomeSubRect, colorData, 0, colorData.Length);
            gnomeIcon.SetData(colorData);
        }

        protected void LoadHordeIcons()
        {
            orcSubRect = new Rectangle(3 * 64, 1 * 64, 64, 64);
            orcIcon = new Texture2D(graphics, orcSubRect.Width, orcSubRect.Height);
            colorData = new Color[orcSubRect.Width * orcSubRect.Height];
            raceIcons.GetData(0, orcSubRect, colorData, 0, colorData.Length);
            orcIcon.SetData(colorData);

            undeadSubRect = new Rectangle(1 * 64, 1 * 64, 64, 64);
            undeadIcon = new Texture2D(graphics, undeadSubRect.Width, undeadSubRect.Height);
            colorData = new Color[undeadSubRect.Width * undeadSubRect.Height];
            raceIcons.GetData(0, undeadSubRect, colorData, 0, colorData.Length);
            undeadIcon.SetData(colorData);

            taurenSubRect = new Rectangle(0, 1 * 64, 64, 64);
            taurenIcon = new Texture2D(graphics, taurenSubRect.Width, taurenSubRect.Height);
            colorData = new Color[taurenSubRect.Width * taurenSubRect.Height];
            raceIcons.GetData(0, taurenSubRect, colorData, 0, colorData.Length);
            taurenIcon.SetData(colorData);

            trollSubRect = new Rectangle(2 * 64, 1 * 64, 64, 64);
            trollIcon = new Texture2D(graphics, trollSubRect.Width, trollSubRect.Height);
            colorData = new Color[trollSubRect.Width * trollSubRect.Height];
            raceIcons.GetData(0, trollSubRect, colorData, 0, colorData.Length);
            trollIcon.SetData(colorData);
        }

        protected void SetPositions()
        {
            racePanel.Position = new Point2(50, graphics.Viewport.Height / 2 - racePanel.Height / 2);
            allyBanner_Position = new Vector2(racePanel.Position.X + 25, racePanel.Position.Y + (racePanel.Height / 2 - allyBanner.Height / 2) - 75);
            hordeBanner_Position = new Vector2(racePanel.Position.X + (racePanel.Width - hordeBanner.Width - 25), racePanel.Position.Y + (racePanel.Height / 2 - hordeBanner.Height / 2) - 75);

            humanRect.Position = new Point2(allyBanner_Position.X + (allyBanner.Width / 2 - humanRect.Width / 2), allyBanner_Position.Y + 20);
            dwarfRect.Position = new Point2(humanRect.Position.X, humanRect.Position.Y + (dwarfRect.Height + 10));
            nightelfRect.Position = new Point2(dwarfRect.Position.X, dwarfRect.Position.Y + (nightelfRect.Height + 10));
            gnomeRect.Position = new Point2(nightelfRect.Position.X, nightelfRect.Position.Y + (gnomeRect.Height + 10));

            orcRect.Position = new Point2(hordeBanner_Position.X + (hordeBanner.Width / 2 - orcRect.Width / 2), hordeBanner_Position.Y + 20);
            undeadRect.Position = new Point2(orcRect.Position.X, orcRect.Position.Y + (undeadRect.Height + 10));
            taurenRect.Position = new Point2(undeadRect.Position.X, undeadRect.Position.Y + (taurenRect.Height + 10));
            trollRect.Position = new Point2(taurenRect.Position.X, taurenRect.Position.Y + (trollRect.Height + 10));
        }

        public void Update()
        {
            if (humanRect.Contains(Mouse.GetState().Position))
                if (InputHandler.IsMouseButtonPressed(InputHandler.MouseButton.LeftButton))
                    selectedRace = Race.Human;
        }

        public void Draw(BitmapFont font, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawRectangle(racePanel, Color.Gray, 2f);
            spriteBatch.DrawString(font, "Alliance", new Vector2(allyBanner_Position.X + (allyBanner.Width / 2 - font.MeasureString("Alliance").Width / 2), allyBanner_Position.Y - font.LineHeight), WorldofWarcraft.DefaultYellow);
            spriteBatch.DrawString(font, "Horde", new Vector2(hordeBanner_Position.X + (hordeBanner.Width / 2 - font.MeasureString("Horde").Width / 2), hordeBanner_Position.Y - font.LineHeight), WorldofWarcraft.DefaultYellow);
            spriteBatch.Draw(allyBanner, allyBanner_Position, Color.White);
            spriteBatch.Draw(hordeBanner, hordeBanner_Position, Color.White);

            spriteBatch.Draw(humanIcon, humanRect.Position, null, Color.White, 0f, Vector2.Zero, iconScale, SpriteEffects.None, 0f);
            spriteBatch.Draw(dwarfIcon, dwarfRect.Position, null, Color.White, 0f, Vector2.Zero, iconScale, SpriteEffects.None, 0f);
            spriteBatch.Draw(nightelfIcon, nightelfRect.Position, null, Color.White, 0f, Vector2.Zero, iconScale, SpriteEffects.None, 0f);
            spriteBatch.Draw(gnomeIcon, gnomeRect.Position, null, Color.White, 0f, Vector2.Zero, iconScale, SpriteEffects.None, 0f);

            spriteBatch.Draw(orcIcon, orcRect.Position, null, Color.White, 0f, Vector2.Zero, iconScale, SpriteEffects.None, 0f);
            spriteBatch.Draw(undeadIcon, undeadRect.Position, null, Color.White, 0f, Vector2.Zero, iconScale, SpriteEffects.None, 0f);
            spriteBatch.Draw(taurenIcon, taurenRect.Position, null, Color.White, 0f, Vector2.Zero, iconScale, SpriteEffects.None, 0f);
            spriteBatch.Draw(trollIcon, trollRect.Position, null, Color.White, 0f, Vector2.Zero, iconScale, SpriteEffects.None, 0f);
        }

        public Race GetSelectedRace()
        {
            return selectedRace;
        }
    }
}
