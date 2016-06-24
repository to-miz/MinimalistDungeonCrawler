using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MinimalisticDungeonCrawler
{
    public abstract class AnimatableEntity : CollidableEntity
    {
        protected Map.AABB spriteRect;

        protected int currentAnim = 0;
        protected int animCount = 2;
        protected float animTimer = 0f;
        protected float animSpeed = 0.1f;

        public AnimatableEntity(Screens.ScreenManager screenManager)
            : base(screenManager)
        {
        }
    }
}
