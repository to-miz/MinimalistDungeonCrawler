using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MinimalisticDungeonCrawler
{
    public class EnemyExplosion : EnemyEntity
    {
        static readonly Rectangle[] ExplosionRect = new Rectangle[] { new Rectangle(2 * 16, 3 * 16, 16, 16), new Rectangle(3 * 16, 3 * 16, 16, 16) };

        public EnemyExplosion(Screens.ScreenManager screenManager, Vector2 position, Map.Room parentRoom, Player player, Texture2D spriteSheet)
            : base(screenManager, position, parentRoom, player)
        {
            currentSourceRect = ExplosionRect;

            animCount = 2;
            animSpeed = 0.06f;
            isHittable = false;
            this.spriteSheet = spriteSheet;
        }

        bool animationEnded = false;

        public override void updateHorizontal(float dt)
        {
            base.updateHorizontal(dt);

            animTimer += animSpeed * dt;
            if (animTimer >= 1f)
            {
                animTimer = 0f;
                currentAnim = (currentAnim + 1) % animCount;
            }

            if (currentAnim == 1)
                animationEnded = true;

            if (currentAnim == 0 && animationEnded)
            {
                Random random = new Random();
                int randomNumber = random.Next(0, 100);
                if( randomNumber < 10 )
                    parentRoom.addItem( new HealthPickup( screenManager, position, parentRoom, spriteSheet ) );

                parentRoom.removeEnemy(this);
            }
        }

        public override void handleInput(Input.InputState inputState)
        {
        }
    }
}
