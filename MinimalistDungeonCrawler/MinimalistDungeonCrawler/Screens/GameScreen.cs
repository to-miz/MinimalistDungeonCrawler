using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MinimalisticDungeonCrawler.Screens
{
    public abstract class GameScreen
    {
        public ScreenManager screenManager;
        protected bool isInitialized = false;
        public bool Done = false;

        public virtual void LoadContent()
        {
            isInitialized = true;
        }

        public abstract void Update(GameTime gameTime);
        public abstract void HandleInput(Input.InputState input);
        public abstract void Draw(GameTime gameTime);
    }
}
