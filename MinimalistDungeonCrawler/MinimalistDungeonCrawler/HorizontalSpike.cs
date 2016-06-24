using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MinimalisticDungeonCrawler
{
    public class HorizontalSpike : EnemyEntity
    {
        static readonly Rectangle[] sourceRect = new Rectangle[] { new Rectangle(9 * 16, 5 * 16, 16, 16) };

        public HorizontalSpike(Screens.ScreenManager screenManager, Vector2 position, Map.Room parentRoom, Player player)
            : base(screenManager, position, parentRoom, player)
        {
            collisionOffsetX = 0;
            collisionOffsetY = 0;
            collisionBox.x = (int)position.X + collisionOffsetX;
            collisionBox.y = (int)position.Y + collisionOffsetY;
            collisionBox.width = 32;
            collisionBox.height = 32;
            currentSourceRect = sourceRect;
            countTowardsEnemyCount = false;

            animCount = 1;
            animSpeed = 0.1f;
            isHurtingOnContact = true;
            health = 3;
            maxHealth = 3;
            //invincibleCountdownSpeed = -0.02f;

            if (screenManager.RNG.Next(100) < 50)
                velocity.X = maxSpeed;
            else
                velocity.X = -maxSpeed;

            isHittable = false;
        }

        public override void updateHorizontal(float dt)
        {
            base.updateHorizontal(dt);

            animTimer += animSpeed * dt;
            if (animTimer >= 1f)
            {
                animTimer = 0f;
                currentAnim = (currentAnim + 1) % animCount;
            }
        }

        public override void draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.draw(gameTime);
        }

        public override void handleInput(Input.InputState inputState)
        {
        }

        public override void setKnockback(Vector2 velocity)
        {
            currentState = State.Neutral;
            if (health <= 0)
            {
                onDie();
            }
        }

        public override void onCollision(int pushX, int pushY)
        {
            base.onCollision(pushX, pushY);

            if (pushX != 0)
            {
                velocity.X = -velocity.X;
            }
        }

        public override void onDie()
        {
            base.onDie();
            parentRoom.addEnemy(new EnemyExplosion(screenManager, position, parentRoom, player, spriteSheet));
        }
    }
}
