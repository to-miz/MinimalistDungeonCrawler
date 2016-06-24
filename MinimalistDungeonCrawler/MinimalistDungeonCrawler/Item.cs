using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MinimalisticDungeonCrawler
{
    public abstract class Item : CollidableEntity
    {
        protected Texture2D spriteSheet;
        protected Map.Room parentRoom;
        protected Map.AABB spriteRect;
        protected Rectangle sourceRect;

        public Item(Screens.ScreenManager screenManager, Vector2 position, Map.Room parentRoom, Texture2D spriteSheet)
            : base(screenManager)
        {
            this.parentRoom = parentRoom;
            this.spriteSheet = spriteSheet;
            this.position = position;
            spriteRect.width = 32;
            spriteRect.height = 32;
        }

        public abstract void onCollection(Player player);

        public override void draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            SpriteBatch spriteBatch = screenManager.SpriteBatch;
            spriteRect.x = (int)position.X;
            spriteRect.y = (int)position.Y;
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, parentRoom.TransitionMatrix);
            spriteBatch.Draw(screenManager.SpriteSheet, spriteRect.getScaled(scale), sourceRect, Color.White);
            spriteBatch.End();
        }
    }
}
