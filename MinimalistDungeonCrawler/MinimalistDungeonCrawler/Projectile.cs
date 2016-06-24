﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MinimalisticDungeonCrawler
{
    public class Projectile : EnemyEntity
    {
        static readonly Rectangle[] SlimeProjectileRect = new Rectangle[] { new Rectangle(4 * 16, 4 * 16, 16, 16), new Rectangle(5 * 16, 4 * 16, 16, 16) };
        static readonly Rectangle[] SlimeProjectileShadowRect = new Rectangle[] { new Rectangle(6 * 16, 4 * 16, 16, 16), new Rectangle(7 * 16, 4 * 16, 16, 16) };

        float yOffset = 0;
        float yOffsetSpeed = -0.9f;
        float yOffsetGravity = 0.01f;

        public Projectile(Screens.ScreenManager screenManager, Vector2 position, Map.Room parentRoom, Player player, Texture2D spriteSheet)
            : base(screenManager, position, parentRoom, player)
        {
            collisionOffsetX = 5 * 2;
            collisionOffsetY = 6 * 2;
            collisionBox.x = (int)position.X + collisionOffsetX;
            collisionBox.y = (int)position.Y + collisionOffsetY;
            collisionBox.width = 6 * 2;
            collisionBox.height = 5 * 2;
            currentSourceRect = SlimeProjectileShadowRect;

            animCount = 2;
            animSpeed = 0.2f;

            velocity = player.Position - position;
            velocity.Normalize();
            velocity *= 1.5f;

            currentState = State.Attacking;
            attackRect.width = collisionBox.width;
            attackRect.height = collisionBox.height;

            this.spriteSheet = spriteSheet;
            countTowardsEnemyCount = false;
            health = 1;
        }

        public override void updateHorizontal(float dt)
        {
            base.updateHorizontal(dt);
            attackRect.x = collisionBox.x;

            if (Math.Abs(velocity.X * velocity.X + velocity.Y * velocity.Y) <= 0.000001f)
                return;

            animTimer += animSpeed * dt;
            if (animTimer >= 1f)
            {
                animTimer = 0f;
                currentAnim = (currentAnim + 1) % animCount;
            }
        }
        public override void updateVertical(float dt)
        {
            base.updateVertical(dt);
            attackRect.y = collisionBox.y;

            yOffsetSpeed += yOffsetGravity * dt;
            yOffset += yOffsetSpeed * dt;

            if (yOffset > 0f)
            {
                parentRoom.removeEnemy(this);
            }
        }

        public override void onCollision(int pushX, int pushY)
        {
            parentRoom.removeEnemy(this);
        }

        public override void draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.draw(gameTime);

            SpriteBatch spriteBatch = screenManager.SpriteBatch;
            spriteRect.x = (int)position.X;
            spriteRect.y = (int)(position.Y + yOffset);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, parentRoom.TransitionMatrix);
            spriteBatch.Draw(spriteSheet, spriteRect.getScaled(scale), SlimeProjectileRect[currentAnim], Color.White);
            spriteBatch.End();
        }

        public override void handleInput(Input.InputState inputState)
        {
        }
    }
}
