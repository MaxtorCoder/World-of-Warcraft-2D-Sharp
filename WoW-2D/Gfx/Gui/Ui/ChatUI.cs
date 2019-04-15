using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Network.Packet.Client;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using WoW_2D.Network;
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

        private List<ChatMessage> chats;
        private BitmapFont font;

        public ChatUI(GraphicsDevice graphics) : base(graphics)
        {
            chatBox = new RectangleF(0, 0, 400, 175);
            chatBox.Position = new Point2(0, graphics.Viewport.Height - chatBox.Height);

            TextField = new GuiChatText(graphics)
            {
                Width = (int)chatBox.Width,
                Height = 25,
                BackgroundColor = new Color(0f, 0f, 0f, 0.75f),
                MaxCharacterLength = 150,
            };
            TextField.Position = new Vector2(0, chatBox.Position.Y - TextField.Height - 2f);

            chats = new List<ChatMessage>();
        }

        public override void LoadContent(ContentManager content)
        {
            TextField.LoadContent(content);

            font = content.Load<BitmapFont>("System/Font/font_small");
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

            if (Global.Chats.Count > 0)
            {
                var chat = Global.Chats.Dequeue();
                var message = $"[{chat.Sender}] says: {chat.Message}";
                var wrapped = Global.WrapText(font, message, chatBox.Width);
                chat.Message = wrapped;
                
                if (chats.Count == 0)
                    chat.Bounds = new RectangleF(chatBox.X + 1f, chatBox.Y, font.MeasureString(chat.Message).Width, font.MeasureString(chat.Message).Height);
                else
                {
                    var lastMessage = chats[chats.Count - 1];
                    chat.Bounds = new RectangleF(chatBox.X + 1f, lastMessage.Bounds.Y + lastMessage.Bounds.Height, font.MeasureString(chat.Message).Width, font.MeasureString(chat.Message).Height);
                }
                chats.Add(chat);
            }

            if (chats.Count > 0)
            {
                var lastChat = chats[chats.Count - 1];
                if (lastChat.Bounds.Y + lastChat.Bounds.Height > chatBox.Y + chatBox.Height)
                {
                    for (int i = 0; i < chats.Count; i++)
                    {
                        var chat = chats[i];
                        var chatPos = chat.Bounds;
                        chatPos.Y -= chats[0].Bounds.Height;
                        chat.Bounds = chatPos;
                    }
                    chats.RemoveAt(0);
                }
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

                spriteBatch.Begin();
                for (int i = 0; i < chats.Count; i++)
                {
                    var chat = chats[i];
                    spriteBatch.DrawString(font, chat.Message, chat.Bounds.Position, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                }
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
                    var message = TextField.GetText();
                    if (!string.IsNullOrWhiteSpace(message))
                    {
                        NetworkManager.Send(new CMSG_Chat()
                        {
                            Message = message
                        }, NetworkManager.Direction.World);
                    }
                    TextField.ResetText();
                }
            }
        }
    }
}
