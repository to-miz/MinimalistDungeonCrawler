using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MinimalisticDungeonCrawler.Map
{
    public class Tile : Entity
    {
        Texture2D spriteSheet;
        public Rectangle sourceRect;
        Room parentRoom;

        public bool Solid = true;
        public bool Destructible = false;

        public static readonly Rectangle LeftTop = new Rectangle(0, 0, 16, 16);
        public static readonly Rectangle Top = new Rectangle(16, 0, 16, 16);
        public static readonly Rectangle RightTop = new Rectangle(32, 0, 16, 16);
        public static readonly Rectangle Right = new Rectangle(32, 16, 16, 16);
        public static readonly Rectangle RightBottom = new Rectangle(32, 32, 16, 16);
        public static readonly Rectangle Bottom = new Rectangle(16, 32, 16, 16);
        public static readonly Rectangle LeftBottom = new Rectangle(0, 32, 16, 16);
        public static readonly Rectangle Left = new Rectangle(0, 16, 16, 16);
        public static readonly Rectangle Ground = new Rectangle(16, 16, 16, 16);
        public static readonly Rectangle DoorClosedUp = new Rectangle(48, 0, 16, 16);
        public static readonly Rectangle DoorClosedRight = new Rectangle(64, 0, 16, 16);
        public static readonly Rectangle DoorClosedDown = new Rectangle(80, 0, 16, 16);
        public static readonly Rectangle DoorClosedLeft = new Rectangle(96, 0, 16, 16);
        public static readonly Rectangle DoorOpenUp = new Rectangle(48, 16, 16, 16);
        public static readonly Rectangle DoorOpenRight = new Rectangle(64, 16, 16, 16);
        public static readonly Rectangle DoorOpenDown = new Rectangle(80, 16, 16, 16);
        public static readonly Rectangle DoorOpenLeft = new Rectangle(96, 16, 16, 16);
        public static readonly Rectangle BigBoulder = new Rectangle(6*16, 2*16, 16, 16);
        public static readonly Rectangle Exit = new Rectangle(8 * 16, 4 * 16, 16, 16);
        public static readonly Rectangle Empty = new Rectangle(9 * 16, 4 * 16, 16, 16);

        public Tile(Screens.ScreenManager screenManager, AABB collisionBox, Rectangle sourceRect, Room parentRoom)
            : base(screenManager)
        {
            this.collisionBox = collisionBox;
            this.sourceRect = sourceRect;
            this.parentRoom = parentRoom;
        }

        public override void loadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            spriteSheet = content.Load<Texture2D>(@"data\Spritesheet");
        }

        public override void updateHorizontal(float dt)
        {
        }

        public override void updateVertical(float dt)
        {
        }

        public override void draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            SpriteBatch spriteBatch = screenManager.SpriteBatch;

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, parentRoom.TransitionMatrix);
            spriteBatch.Draw(screenManager.SpriteSheet, collisionBox.getScaled(scale), sourceRect, Color.White);
            spriteBatch.End();
        }

        public override void onCollision(int pushX, int pushY)
        {
        }

        public override void handleInput(Input.InputState inputState)
        {
        }
    }
}
