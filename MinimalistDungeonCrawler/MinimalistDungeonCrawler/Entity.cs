using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MinimalisticDungeonCrawler.Map;


namespace MinimalisticDungeonCrawler
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public abstract class Entity
    {
        protected AABB collisionBox = new AABB();
        protected Screens.ScreenManager screenManager;
        public static Vector2 scale;

        public Rectangle CollisionRect
        {
            get { return collisionBox.Rect; }
        }

        public AABB CollisionBox
        {
            get { return collisionBox; }
        }

        public Entity(Screens.ScreenManager screenManager)
        {
            this.screenManager = screenManager;
        }

        public abstract void loadContent(ContentManager content);
        public abstract void updateHorizontal(float dt);
        public abstract void updateVertical(float dt);
        public abstract void draw(GameTime gameTime);
        public abstract void onCollision(int pushX, int pushY);
        public abstract void handleInput(Input.InputState inputState);
    }
}
