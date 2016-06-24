using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MinimalisticDungeonCrawler
{
    public class RandomEffectItem : Item
    {
        string effectString = "";
        bool fadeOut = false;
        float fadeOutTimer = 1.0f;
        static readonly float fadeOutSpeed = -0.01f;

        public RandomEffectItem(Screens.ScreenManager screenManager, Vector2 position, Map.Room parentRoom, Texture2D spriteSheet)
            : base(screenManager, position, parentRoom, spriteSheet)
        {
            sourceRect.X = 5 * 16;
            sourceRect.Y = 7 * 16;
            sourceRect.Width = 16;
            sourceRect.Height = 16;

            collisionBox.x = (int)position.X;
            collisionBox.y = (int)position.Y;
            collisionBox.width = 32;
            collisionBox.height = 32;
        }

        public override void updateHorizontal(float dt)
        {
            base.updateHorizontal(dt);

            if (fadeOut)
            {
                if (fadeOutTimer > 0f)
                {
                    fadeOutTimer += fadeOutSpeed * dt;
                }
                else
                    parentRoom.removeItem(this);
            }            
        }

        public override void onCollection(Player player)
        {
            if (fadeOut)
                return;

            Random rand = new Random();
            int randomNumber = rand.Next(100);
            if (randomNumber < 25)
            {
                player.Attack += 1;
                effectString = "Attack power increased!";
            }
            else if (randomNumber < 50)
            {
                player.MaxHealth += 2;
                effectString = "Health increased!";
            }
            else if (randomNumber < 75)
            {
                player.Speed += 0.1f;
                effectString = "Speed increased!";
            }
            else
            {
                player.MaxHealth += 4;
                effectString = "Health increased twice!";
            }

            fadeOut = true;
            screenManager.powerup.Play();
        }

        public override void loadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
        }

        public override void handleInput(Input.InputState inputState)
        {
        }

        public override void draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (fadeOut)
            {
                SpriteBatch spriteBatch = screenManager.SpriteBatch;
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
                Color color = Color.Black;
                color.A = (byte)(255f * fadeOutTimer);
                spriteBatch.DrawString(screenManager.Font, effectString, new Vector2(200, 64), color, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0);

                spriteBatch.End();
            }
            else
            {
                base.draw(gameTime);
            }
        }
    }
}
