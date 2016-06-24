using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MinimalisticDungeonCrawler.Screens
{
    public class Ingame : GameScreen
    {
        ContentManager content;
        Player player;
        Map.Map map;

        static readonly Rectangle[] hearts = new Rectangle[] { new Rectangle(5 * 16, 5 * 16, 16, 16), new Rectangle(6 * 16, 5 * 16, 16, 16) };
        static readonly Rectangle emptyheart = new Rectangle(7 * 16, 5 * 16, 16, 16);

        public Ingame()
        {
        }

        int floor = 1;

        public override void LoadContent()
        {
            if (isInitialized)
                return;

            if (content == null)
                content = new ContentManager(screenManager.Game.Services, "Content");

            player = new Player(screenManager);
            map = new Map.Map(screenManager, player, floor);
            map.loadContent(content);
            player.loadContent(content);
            player.Position = map.CurrentRoom.PlayerPos;
            player.ParentRoom = map.CurrentRoom;

            isInitialized = true;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 10.0f;
            map.update(dt);

            if (player.Health <= 0)
            {
                Done = true;
                Screens.GameOver gameOverScreen = new Screens.GameOver();

                screenManager.addScreen(gameOverScreen);
            }

            if (map.GoToNextFloor)
            {
                floor++;
                map = new Map.Map(screenManager, player, floor);
                map.loadContent(content);
                player.Position = map.CurrentRoom.PlayerPos;
                player.ParentRoom = map.CurrentRoom;
            }
        }

        public override void HandleInput(Input.InputState input)
        {
            if (map.CurrentRoom.IsTransitioning)
            {
                return;
            }

            player.handleInput(input);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            map.draw(gameTime);
            player.draw(gameTime);

            //draw gui
            SpriteBatch spriteBatch = screenManager.SpriteBatch;
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
            Rectangle spriteRect = new Rectangle();
            spriteRect.X = 19 * 32;
            spriteRect.Y = 0;
            spriteRect.Width = 192;
            spriteRect.Height = 480;

            spriteBatch.Draw(screenManager.BlankTexture, spriteRect, Color.White);

            spriteRect.Width = 32;
            spriteRect.Height = 32;

            int x = 0;
            int y = 0;
            for (int i = 0; i < player.MaxHealth; i += 2, ++x)
            {
                if (x > 6)
                {
                    ++y;
                    x = 0;
                }

                spriteRect.X = 19 * 32 + 16 + 18 * x;
                spriteRect.Y = 18 * y;

                spriteBatch.Draw(screenManager.SpriteSheet, spriteRect, emptyheart, Color.White);
            }

            x = 0;
            y = 0;
            for (int i = 0; i < player.Health; i += 2, ++x)
            {
                if (x > 6)
                {
                    ++y;
                    x = 0;
                }

                spriteRect.X = 19 * 32 + 16 + 18 * x;
                spriteRect.Y = 18 * y;

                spriteBatch.Draw(screenManager.SpriteSheet, spriteRect, hearts[0], Color.White);
                if (i + 2 < player.Health || player.Health % 2 != 1)
                    spriteBatch.Draw(screenManager.SpriteSheet, spriteRect, hearts[1], Color.White);
            }

            /*if (player.Health % 2 == 1 && player.Health != player.MaxHealth)
            {
                spriteRect.X = 19 * 32 + 16 + 18 * x;
                spriteRect.Y = 18 * y;
                spriteBatch.Draw(screenManager.SpriteSheet, spriteRect, hearts[0], Color.White);
            }*/

            spriteBatch.End();

            map.drawMinimap();
        }
    }
}
