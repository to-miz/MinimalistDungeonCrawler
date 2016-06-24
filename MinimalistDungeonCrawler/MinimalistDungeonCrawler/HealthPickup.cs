using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MinimalisticDungeonCrawler
{
    public class HealthPickup : Item
    {
        int health = 1;

        public HealthPickup(Screens.ScreenManager screenManager, Vector2 position, Map.Room parentRoom, Texture2D spriteSheet)
            : base(screenManager, position, parentRoom, spriteSheet)
        {
            Random rand = new Random();
            if (rand.Next(0, 100) < 25)
                health = 2;

            if (health == 2)
            {
                sourceRect.X = 8 * 16;
                sourceRect.Y = 6 * 16;
            }
            else
            {
                sourceRect.X = 9 * 16;
                sourceRect.Y = 6 * 16;
            }
            
            sourceRect.Width = 16;
            sourceRect.Height = 16;

            collisionBox.x = (int)position.X;
            collisionBox.y = (int)position.Y;
            collisionBox.width = 32;
            collisionBox.height = 32;
        }

        public override void onCollection(Player player)
        {
            if (player.Health < player.MaxHealth)
            {
                player.Health += health;
                parentRoom.removeItem(this);
                screenManager.powerup.Play();
            }
        }

        public override void loadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
        }

        public override void handleInput(Input.InputState inputState)
        {
        }
    }
}
