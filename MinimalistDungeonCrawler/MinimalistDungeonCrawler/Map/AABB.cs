﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MinimalisticDungeonCrawler.Map
{
    public struct AABB
    {
        public int x;
        public int y;
        public int width;
        public int height;

        /*public AABB()
        {
            x = 0;
            y = 0;
            width = 0;
            height = 0;
        }*/

        public AABB(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public bool intersects(AABB other)
        {
            if (x + width <= other.x)
                return false;

            if (other.x + other.width <= x)
                return false;

            if (y + height <= other.y)
                return false;

            if (other.y + other.height <= y)
                return false;

            return true;
        }

        public int getHorizontalIntersection(AABB other)
        {
            int halfSizeA = width / 2;
            int halfSizeB = other.width / 2;

            int centerA = x + halfSizeA;
            int centerB = other.x + halfSizeB;

            int distance = centerA - centerB;
            int minDistance = halfSizeA + halfSizeB;

            if (Math.Abs(distance) >= Math.Abs(minDistance))
                return 0;

            if (distance > 0)
                return -minDistance + distance;
            else
                return minDistance + distance;
        }

        public int getVerticalIntersection(AABB other)
        {
            int halfSizeA = height / 2;
            int halfSizeB = other.height / 2;

            int centerA = y + halfSizeA;
            int centerB = other.y + halfSizeB;

            int distance = centerA - centerB;
            int minDistance = halfSizeA + halfSizeB;

            if (Math.Abs(distance) >= Math.Abs(minDistance))
                return 0;

            if (distance > 0)
                return -minDistance + distance;
            else
                return minDistance + distance;
        }

        public Rectangle Rect
        {
            get { return new Rectangle(x, y, width, height); }
        }

        public Rectangle getScaled(Vector2 scale)
        {
            return new Rectangle((int)((float)x * scale.X), (int)((float)y * scale.Y), (int)((float)width * scale.X), (int)((float)height * scale.Y));
        }

        public bool isEmpty()
        {
            return width == 0 || height == 0;
        }
    }
}
