using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MinimalisticDungeonCrawler
{
    public class BossArmPiece : EnemyEntity
    {
        static readonly Rectangle[] BossArmPieceSource = new Rectangle[] { new Rectangle(5 * 16, 8 * 16, 16, 16) };

        public BossArmPiece(Screens.ScreenManager screenManager, Vector2 position, Map.Room parentRoom, Player player)
            : base(screenManager, position, parentRoom, player)
        {
            collisionOffsetX = 0;
            collisionOffsetY = 0;
            collisionBox.x = (int)position.X + collisionOffsetX;
            collisionBox.y = (int)position.Y + collisionOffsetY;
            collisionBox.width = 12;
            collisionBox.height = 12;
            currentSourceRect = BossArmPieceSource;
            countTowardsEnemyCount = false;

            animCount = 1;
            animSpeed = 0.1f;
            isHurtingOnContact = true;
            isHittable = false;
            health = 3;
            maxHealth = 3;
            maxSpeed = 1.1f;
        }

        int level = 0;

        public override void updateHorizontal(float dt)
        {
            base.updateHorizontal(dt);
        }

        public override void draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            
        }

        public void specialDraw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.draw(gameTime);
        }

        public override void handleInput(Input.InputState inputState)
        {
        }

        public override void setKnockback(Vector2 velocity)
        {
        }

        public override void onCollision(int pushX, int pushY)
        {
        }

        public override void onDie()
        {
            base.onDie();
            parentRoom.addEnemy(new EnemyExplosion(screenManager, position, parentRoom, player, spriteSheet));
        }
    }
}
