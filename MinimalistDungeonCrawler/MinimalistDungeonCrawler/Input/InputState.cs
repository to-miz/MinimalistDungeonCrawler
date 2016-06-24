using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace MinimalisticDungeonCrawler.Input
{
    public class InputState
    {
        public KeyboardState currentKeyboardState = new KeyboardState();
        public GamePadState currentGamePadState = new GamePadState();

        public KeyboardState lastKeyboardState = new KeyboardState();
        public GamePadState lastGamePadState = new GamePadState();

        public bool gamePadWasConnected = false;

        public void update()
        {
            lastKeyboardState = currentKeyboardState;
            lastGamePadState = currentGamePadState;

            currentKeyboardState = Keyboard.GetState();
            currentGamePadState = GamePad.GetState(PlayerIndex.One);

            if (currentGamePadState.IsConnected)
            {
                gamePadWasConnected = true;
            }
        }

        public bool IsNewKeyPress(Keys key)
        {
            return (currentKeyboardState.IsKeyDown(key) && lastKeyboardState.IsKeyUp(key));
        }

        public bool IsNewButtonPress(Buttons button)
        {
            return (currentGamePadState.IsButtonDown(button) && lastGamePadState.IsButtonUp(button));
        }
    }
}
