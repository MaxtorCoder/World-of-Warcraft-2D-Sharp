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
using WoW_2D.Gfx;
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
        private BitmapFont font_small;

        private GuiButton acceptButton;
        private GuiButton backButton;
        private GuiButton randomizeButton;
        private GuiEntryText nameText;

        private Texture2D humanTexture;

        private RectangleF racePanel;
        private Texture2D allyBanner, hordeBanner;
        private Vector2 allyBanner_Position, hordeBanner_Position;

        private List<RaceType> alliance = new List<RaceType>();
        private List<RaceType> horde = new List<RaceType>();
        private List<RaceType> allRaces = new List<RaceType>();

        private RectangleF[] allianceBounds = new RectangleF[4];
        private RectangleF[] hordeBounds = new RectangleF[4];
        private RectangleF[] classBounds = new RectangleF[9];

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

            racePanel = new RectangleF(0, 0, 225, 512);
            racePanel.Position = new Point2(50, graphics.Viewport.Height / 2 - racePanel.Height / 2);

            InputHandler.AddKeyPressHandler(ID, delegate () { OnEnterPress(); }, Keys.Enter);
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            font = content.Load<BitmapFont>("System/Font/font");
            font_small = content.Load<BitmapFont>("System/Font/font_small");

            acceptButton.LoadContent(content);
            acceptButton.Position = new Vector2(graphics.Viewport.Width - acceptButton.BaseTexture.Width - 15, graphics.Viewport.Height - acceptButton.BaseTexture.Height - 75);

            backButton.LoadContent(content);
            backButton.Position = new Vector2(acceptButton.Position.X, acceptButton.Position.Y + backButton.BaseTexture.Height + 25);

            randomizeButton.LoadContent(content);
            randomizeButton.Position = new Vector2(graphics.Viewport.Width / 2 - randomizeButton.BaseTexture.Width / 2, backButton.Position.Y);

            nameText.Position = new Vector2(graphics.Viewport.Width / 2 - nameText.Width / 2, randomizeButton.Position.Y - nameText.Height - 20);
            nameText.LoadContent(content);
            
            LoadRaceContent(content);
            LoadClassContent();

            /** Set a 'default' enabled. **/
            allRaces[0].IsSelected = true;
            allRaces[0].Classes[0].IsSelected = true;

            humanTexture = Utils.Global.HumanSpritesheet.GetSprite(new Point(32, 0), new Point(32));
        }

        private void LoadRaceContent(ContentManager content)
        {
            allyBanner = content.Load<Texture2D>("Sprites/UI/Ally_Banner");
            allyBanner_Position = new Vector2(racePanel.Position.X + 25, racePanel.Position.Y + (racePanel.Height / 2 - allyBanner.Height / 2) - 75);

            hordeBanner = content.Load<Texture2D>("Sprites/UI/Horde_Banner");
            hordeBanner_Position = new Vector2(racePanel.Position.X + (racePanel.Width - hordeBanner.Width - 25), racePanel.Position.Y + (racePanel.Height / 2 - hordeBanner.Height / 2) - 75);

            alliance.AddRange(new[]
            {
                new RaceType(Race.Human) { Icon = Utils.Global.RaceSpritesheet.GetSprite(new Point(0), new Point(32)), IsEnabled = true },
                new RaceType(Race.Dwarf) { Icon = Utils.Global.RaceSpritesheet.GetSprite(new Point(2 * 32, 0), new Point(32)) },
                new RaceType(Race.NightElf) { Icon = Utils.Global.RaceSpritesheet.GetSprite(new Point(1 * 32, 0), new Point(32)) },
                new RaceType(Race.Gnome) { Icon = Utils.Global.RaceSpritesheet.GetSprite(new Point(3 * 32, 0), new Point(32)) }
            });

            horde.AddRange(new[]
            {
                new RaceType(Race.Orc) { Icon = Utils.Global.RaceSpritesheet.GetSprite(new Point(3 * 32, 1 * 32), new Point(32)) },
                new RaceType(Race.Undead) { Icon = Utils.Global.RaceSpritesheet.GetSprite(new Point(1 * 32, 1 * 32), new Point(32)) },
                new RaceType(Race.Tauren) { Icon = Utils.Global.RaceSpritesheet.GetSprite(new Point(0, 1 * 32), new Point(32)) },
                new RaceType(Race.Troll) { Icon = Utils.Global.RaceSpritesheet.GetSprite(new Point(2 * 32, 1 * 32), new Point(32)) },
            });

            for (int i = 0; i < allianceBounds.Length; i++)
            {
                allianceBounds[i] = new RectangleF(0, 0, 32, 32);

                if (i == 0)
                    allianceBounds[i].Position = new Point2((allyBanner_Position.X + (allyBanner.Width / 2 - allianceBounds[i].Width / 2)), allyBanner_Position.Y + 15);
                else
                {
                    var lastBound = allianceBounds[i - 1];
                    allianceBounds[i].Position = new Point2((allyBanner_Position.X + (allyBanner.Width / 2 - allianceBounds[i].Width / 2)), lastBound.Y + (lastBound.Height + 15));
                }
            }

            for (int i = 0; i < hordeBounds.Length; i++)
            {
                hordeBounds[i] = new RectangleF(0, 0, 32, 32);

                if (i == 0)
                    hordeBounds[i].Position = new Point2((hordeBanner_Position.X + (hordeBanner.Width / 2 - hordeBounds[i].Width / 2)), hordeBanner_Position.Y + 15);
                else
                {
                    var lastBound = hordeBounds[i - 1];
                    hordeBounds[i].Position = new Point2((hordeBanner_Position.X + (hordeBanner.Width / 2 - hordeBounds[i].Width / 2)), lastBound.Y + (lastBound.Height + 15));
                }
            }

            allRaces.AddRange(alliance);
            allRaces.AddRange(horde);
        }

        private void LoadClassContent()
        {
            for (int i = 0; i < classBounds.Length; i++)
            {
                classBounds[i] = new RectangleF(0, 0, 32, 32);

                if (i == 0)
                    classBounds[i].Position = new Point2(allyBanner_Position.X + (allyBanner.Width / 2 - classBounds[i].Width / 2), allyBanner_Position.Y + allyBanner.Height + classBounds[i].Height);
                else if (i > 4)
                {
                    if (i == 5)
                    {
                        var firstBound = classBounds[0];
                        classBounds[i].Position = new Point2(firstBound.X, firstBound.Y + classBounds[i].Height + font.LineHeight);
                    }
                    else
                    {
                        var lastBound = classBounds[i - 1];
                        classBounds[i].Position = new Point2(lastBound.X + classBounds[i].Width, lastBound.Y);
                    }
                }
                else
                {
                    var lastBound = classBounds[i - 1];
                    classBounds[i].Position = new Point2(lastBound.Position.X + classBounds[i].Width + 4f, lastBound.Position.Y);
                }
            }
        }

        public override void UnloadContent()
        {
        }

        public override void Update(GameTime gameTime)
        {
            var mousePos = Mouse.GetState().Position;

            UpdateRaceTypes(mousePos);
            UpdateClassTypes(mousePos);

            acceptButton.IsEnabled = (nameText.Text.Length >= 3 && NetworkManager.State == NetworkManager.NetworkState.Waiting) ? true : false;
            nameText.IsEnabled = (NetworkManager.State == NetworkManager.NetworkState.Waiting) ? true : false;

            acceptButton.Update();
            backButton.Update();
            randomizeButton.Update();
            nameText.Update();
        }

        private void UpdateRaceTypes(Point mousePos)
        {
            for (int i = 0; i < allianceBounds.Length; i++)
            {
                if (allianceBounds[i].Contains(mousePos))
                {
                    var allyRace = alliance[i];
                    allyRace.IsHovering = true;
                    if (InputHandler.IsMouseButtonPressed(InputHandler.MouseButton.LeftButton))
                        DeselectCurrentRace(ref allyRace);
                }
                else
                    alliance[i].IsHovering = false;
            }

            for (int i = 0; i < hordeBounds.Length; i++)
            {
                if (hordeBounds[i].Contains(mousePos))
                {
                    var hordeRace = horde[i];
                    hordeRace.IsHovering = true;
                    if (InputHandler.IsMouseButtonPressed(InputHandler.MouseButton.LeftButton))
                        DeselectCurrentRace(ref hordeRace);
                }
                else
                    horde[i].IsHovering = false;
            }
        }

        private void UpdateClassTypes(Point mousePos)
        {
            // TODO: Handle class selecting.
            var race = GetSelectedRace();
            for (int i = 0; i < race.Classes.Count; i++)
            {
                var @class = race.Classes[i];
                if (classBounds[i].Contains(mousePos))
                    @class.IsHovering = true;
                else
                    @class.IsHovering = false;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            graphics.Clear(Color.Black);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            acceptButton.Draw(spriteBatch);
            backButton.Draw(spriteBatch);
            randomizeButton.Draw(spriteBatch);
            spriteBatch.DrawString(font, "Name", new Vector2(nameText.Position.X + (nameText.Width / 2 - font.MeasureString("Name").Width / 2), nameText.Position.Y - font.LineHeight), WorldofWarcraft.DefaultYellow);
            spriteBatch.End();

            spriteBatch.Begin(blendState: BlendState.NonPremultiplied);
            spriteBatch.FillRectangle(racePanel, new Color(0f, 0f, 0f, 0.75f));
            spriteBatch.DrawRectangle(racePanel, Color.Gray);
            spriteBatch.Draw(WorldofWarcraft.Logo, new Vector2(racePanel.X + (racePanel.Width / 2 - WorldofWarcraft.Logo.Width / 2), racePanel.Y - (WorldofWarcraft.Logo.Height / 2) - 15), Color.White);
            spriteBatch.End();

            spriteBatch.Begin();
            spriteBatch.DrawString(font, "Alliance", new Vector2(allyBanner_Position.X + (allyBanner.Width / 2 - font.MeasureString("Alliance").Width / 2), allyBanner_Position.Y - font.LineHeight), WorldofWarcraft.DefaultYellow);
            spriteBatch.DrawString(font, "Horde", new Vector2(hordeBanner_Position.X + (hordeBanner.Width / 2 - font.MeasureString("Horde").Width / 2), hordeBanner_Position.Y - font.LineHeight), WorldofWarcraft.DefaultYellow);
            spriteBatch.Draw(allyBanner, allyBanner_Position, Color.White);
            spriteBatch.Draw(hordeBanner, hordeBanner_Position, Color.White);

            var race = GetSelectedRace();
            switch (race.Race)
            {
                case Race.Human:
                    spriteBatch.Draw(humanTexture, new Vector2(graphics.Viewport.Width / 2 - humanTexture.Width / 2, graphics.Viewport.Height / 2 - humanTexture.Height / 2), Color.White);
                    break;
            }
            spriteBatch.End();

            for (int i = 0; i < race.Classes.Count; i++)
                race.Classes[i].Draw(font_small, spriteBatch, classBounds[i].Position);

            for (int i = 0; i < alliance.Count; i++)
                alliance[i].Draw(font_small, spriteBatch, allianceBounds[i].Position);

            for (int i = 0; i < horde.Count; i++)
                horde[i].Draw(font_small, spriteBatch, hordeBounds[i].Position);

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

        private void DeselectCurrentRace(ref RaceType newlySelectedRace)
        {
            var race = GetSelectedRace();
            race.IsSelected = false;

            newlySelectedRace.IsSelected = true;
        }

        private RaceType GetSelectedRace()
        {
            var race = allRaces.Find(x => x.IsSelected);
            return race;
        }

        private void OnAcceptButtonPressed()
        {
            var race = GetSelectedRace();
            var @class = race.Classes.Find(x => x.IsSelected).Class;
            if (acceptButton.IsEnabled)
            {
                NetworkManager.State = NetworkManager.NetworkState.CreatingCharacter;
                NetworkManager.Send(new CMSG_Character_Create()
                {
                    Name = nameText.Text.Trim(),
                    Race = race.Race,
                    Class = @class
                }, NetworkManager.Direction.World);
            }
        }

        private void OnBackButtonPressed() => GameStateManager.EnterState(2);
        private void OnRandomizeButtonPressed()
        {}

        private void OnEnterPress() => OnAcceptButtonPressed();
        public override void OnStateEnter() => nameText.ResetText();
    }
}
