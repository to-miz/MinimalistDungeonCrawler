﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MinimalisticDungeonCrawler
{
    public class BossProjectile : EnemyEntity
    {
        static readonly Rectangle[] BossProjectileRect = new Rectangle[] { new Rectangle(4 * 16, 4 * 16, 16, 16), new Rectangle(5 * 16, 4 * 16, 16, 16) };
        
        public BossProjectile(Screens.ScreenManager screenManager, Vector2 position, Map.Room parentRoom, Player player, Texture2D spriteSheet)
            : base(screenManager, position, parentRoom, player)
        {
            collisionOffsetX = 0;
            collisionOffsetY = 0;
            collisionBox.x = (int)position.X + collisionOffsetX;
            collisionBox.y = (int)position.Y + collisionOffsetY;
            collisionBox.width = 7 * 2;
            collisionBox.height = 8 * 2;
            currentSourceRect = BossProjectileRect;

            animCount = 2;
            animSpeed = 0.2f;

            velocity = player.Position - position;
            velocity.Normalize();
            velocity *= 2f;

            currentState = State.Attacking;
            attackRect.width = collisionBox.width;
            attackRect.height = collisionBox.height;

            this.spriteSheet = screenManager.SpriteSheet;
            countTowardsEnemyCount = false;
            health = 2;
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
        }

        public override void onCollision(int pushX, int pushY)
        {
            base.onCollision(pushX, pushY);

            if (pushX != 0)
                velocity.X = -velocity.X;

            if (pushY != 0)
                velocity.Y = -velocity.Y;
        }

        public override void draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.draw(gameTime);
        }

        public override void handleInput(Input.InputState inputState)
        {
        }
    }
}
