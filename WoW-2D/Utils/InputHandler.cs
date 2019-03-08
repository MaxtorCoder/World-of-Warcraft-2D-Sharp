using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace WoW_2D.Utils
{
    /// <summary>
    /// Simplfies input handling.
    /// </summary>
    public class InputHandler
    {
        private static ButtonState oldLeftButtonState;
        private static ButtonState oldRightButtonState;
        private static KeyboardState oldKeyboardState;

        private static Dictionary<Keys, List<Action>> OnKeyPress = new Dictionary<Keys, List<Action>>();
        
        public enum MouseButton
        {
            LeftButton,
            RightButton
        }

        /// <summary>
        /// Add a handler to a specific key.
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="key"></param>
        public static void AddKeyPressHandler(Action handler, Keys key)
        {
            if (!OnKeyPress.ContainsKey(key))
                OnKeyPress.Add(key, new List<Action>());
            OnKeyPress[key].Add(handler);
        }

        /// <summary>
        /// Check for a key-press every frame every frame.
        /// </summary>
        public static void Update()
        {
            var currentKeyState = Keyboard.GetState();

            foreach (Keys key in Enum.GetValues(typeof(Keys)))
            {
                if (currentKeyState.IsKeyDown(key) && oldKeyboardState.IsKeyUp(key) && OnKeyPress.ContainsKey(key))
                {
                    foreach (var handler in OnKeyPress[key])
                        handler();
                }
            }
            oldKeyboardState = currentKeyState;
        }

        /// <summary>
        /// Checks to see if the given mouse-button has been clicked a single time.
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public static bool isMouseButtonPressed(MouseButton button)
        {
            ButtonState currentState;
            bool pressed = false;
            switch (button)
            {
                case MouseButton.LeftButton:
                    currentState = Mouse.GetState().LeftButton;
                    if (currentState == ButtonState.Pressed && oldLeftButtonState == ButtonState.Released)
                        pressed = true;
                    oldLeftButtonState = currentState;
                    return pressed;
                case MouseButton.RightButton:
                    currentState = Mouse.GetState().RightButton;
                    if (currentState == ButtonState.Pressed && oldRightButtonState == ButtonState.Released)
                        pressed = true;
                    oldRightButtonState = currentState;
                    return pressed;
            }
            return pressed;
        }
    }
}
