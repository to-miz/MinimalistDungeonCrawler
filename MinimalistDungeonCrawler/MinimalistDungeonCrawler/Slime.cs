using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MinimalisticDungeonCrawler
{
    public class Slime : EnemyEntity
    {
        static readonly Rectangle[] SlimeRect = new Rectangle[] { new Rectangle(2 * 16, 4 * 16, 16, 16), new Rectangle(3 * 16, 4 * 16, 16, 16) };

        float timer = 0f;
        float timerSpeed = 0.01f;
        bool shoot = true;

        public Slime(Screens.ScreenManager screenManager, Vector2 position, Map.Room parentRoom, Player player )
            : base(screenManager, position, parentRoom, player)
        {
            collisionOffsetX = 4 * 2;
            collisionOffsetY = 5 * 2;
            collisionBox.x = (int)position.X + collisionOffsetX;
            collisionBox.y = (int)position.Y + collisionOffsetY;
            collisionBox.width = 16;
            collisionBox.height = 12;
            currentSourceRect = SlimeRect;

            animCount = 2;
            animSpeed = 0.02f;
            isHurtingOnContact = true;

            timer = (float)screenManager.RNG.NextDouble();
        }

        public override void updateHorizontal(float dt)
        {
            base.updateHorizontal(dt);

            //do some ai stuff
            if (currentState == State.Neutral)
            {
                if (!shoot)
                {
                    velocity = player.Position - position;
                    velocity.Normalize();
                    velocity *= 0.3f;
                }
                else
                    velocity = Vector2.Zero;

                timer += timerSpeed * dt;
                if ((timer >= 1f && !shoot) || (timer >= 0.3f && shoot))
                {
                    timer = 0f;
                    if (shoot)
                    {
                        Projectile proj = new Projectile(screenManager, position, parentRoom, player, spriteSheet);
                        parentRoom.addEnemy(proj);
                    }
                    shoot = !shoot;
                }
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

        public override void onDie()
        {
            base.onDie();
            parentRoom.addEnemy(new EnemyExplosion(screenManager, position, parentRoom, player, spriteSheet));
        }
    }
}
