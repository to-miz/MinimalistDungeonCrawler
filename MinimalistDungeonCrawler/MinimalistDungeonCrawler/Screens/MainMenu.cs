using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace MinimalisticDungeonCrawler.Screens
{
    public class MainMenu : GameScreen
    {
        SoundEffect select;

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 10.0f;

            if (!fadeOut)
            {
                if (fadeInTimer < 1f)
                {
                    fadeInTimer += fadeInSpeed * dt;
                }
                else
                    fadeInTimer = 1f;
            }
            else
            {
                if (fadeInTimer > 0f)
                {
                    fadeInTimer -= fadeInSpeed * dt;
                }
                else
                    fadeInTimer = 0f;
            }

            if (fadeInTimer <= 0f && fadeOut)
            {
                Done = true;
                Screens.Ingame ingameScreen = new Screens.Ingame();

                screenManager.addScreen(ingameScreen);
            }            
        }

        float fadeInTimer = 0.0f;
        static readonly float fadeInSpeed = 0.01f;
        bool fadeOut = false;

        public override void HandleInput(Input.InputState input)
        {
            if (input.IsNewButtonPress(Buttons.Start) || input.IsNewKeyPress(Keys.Space) || input.IsNewKeyPress(Keys.Enter))
            {
                if (fadeInTimer < 1f && !fadeOut)
                {
                    fadeInTimer = 1f;
                }
                else if (!fadeOut)
                {
                    fadeOut = true;
                    select.Play();
                }
                else
                {
                    fadeInTimer = 0.0f;
                }
            }
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            SpriteBatch spriteBatch = screenManager.SpriteBatch;
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
            Color color = Color.Black;
            color.A = (byte)(255f * fadeInTimer);
            spriteBatch.DrawString(screenManager.Font, "Minimalist", new Vector2(310, 0), color, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0);
            spriteBatch.DrawString(screenManager.Font, "Dungeon Crawler", new Vector2(250, 32), color, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0);
            spriteBatch.DrawString(screenManager.Font, "By Tolga \"Cranky\" Mizrak", new Vector2(200, 64), color, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0);

            if (fadeInTimer >= 1f && !fadeOut)
            {
                spriteBatch.DrawString(screenManager.Font, "Press start.", new Vector2(300, 200), Color.Black, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0);
            }
            
            spriteBatch.End();
        }

        public override void LoadContent()
        {
            base.LoadContent();

            ContentManager content = screenManager.Game.Content;

            select = content.Load<SoundEffect>(@"sounds\Select");
        }
    }
}
