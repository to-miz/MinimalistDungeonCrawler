using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MinimalisticDungeonCrawler
{
    public class Bat : EnemyEntity
    {
        static readonly Rectangle[] BatRect = new Rectangle[] { new Rectangle(0 * 16, 4 * 16, 16, 16), new Rectangle(1 * 16, 4 * 16, 16, 16) };

        public Bat(Screens.ScreenManager screenManager, Vector2 position, Map.Room parentRoom, Player player)
            : base(screenManager, position, parentRoom, player)
        {
            collisionOffsetX = 4 * 2;
            collisionOffsetY = 5 * 2;
            collisionBox.x = (int)position.X + collisionOffsetX;
            collisionBox.y = (int)position.Y + collisionOffsetY;
            collisionBox.width = 16;
            collisionBox.height = 12;
            currentSourceRect = BatRect;

            animCount = 2;
            animSpeed = 0.1f;
            isHurtingOnContact = true;
            health = 3;
            maxSpeed = 1.5f;
            invincibleCountdownSpeed = -0.02f;
        }

        public override void updateHorizontal(float dt)
        {
            base.updateHorizontal(dt);

            //do some ai stuff
            if (currentState == State.Neutral)
            {
                velocity = player.Position - position;
                velocity.Normalize();
                velocity *= maxSpeed;
            }           

            /*if (Math.Abs(velocity.X * velocity.X + velocity.Y * velocity.Y) <= 0.000001f)
                return;*/

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
            currentState = State.Knockback;
            velocity.Normalize();
            velocity *= 6f;
            this.velocity = velocity;
            knockbackFriction = -velocity;
            knockbackFriction.Normalize();
            knockbackFriction *= knockbackFrictionSpeed;
            knockbackTimer = 0f;
        }

        public override void onDie()
        {
            base.onDie();
            parentRoom.addEnemy(new EnemyExplosion(screenManager, position, parentRoom, player, spriteSheet));
        }
    }
}
