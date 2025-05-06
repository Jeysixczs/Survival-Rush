using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shooting_Game.Presenter
{
    public struct Vector2
    {
        public float X;
        public float Y;

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public void Normalize()
        {
            float length = (float)Math.Sqrt(X * X + Y * Y);
            if (length > 0)
            {
                X /= length;
                Y /= length;
            }
        }

        public Vector2 Normalized()
        {
            float length = (float)Math.Sqrt(X * X + Y * Y);
            return length > 0 ? new Vector2(X / length, Y / length) : new Vector2(0, 0);
        }

        public static Vector2 operator *(Vector2 v, float scalar)
        {
            return new Vector2(v.X * scalar, v.Y * scalar);
        }
    }
}
