using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    public class Position
    {
        public float X;
        public float Y;
        public float Z;

        public Position()
        {
        }

        public Position(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public static Position operator -(Position a, Position b)
        {
            Position vector = new Position();
            vector.X = (float)(a.X - b.X);
            vector.Y = (float)(a.Y - b.Y);
            vector.Z = (float)(a.Z - b.Z);

            return vector;
        }

        public Position NormalizedCopy()
        {
            var length = ((X * X) + (Y * Y)) + (Z * Z);
            var normFactor = 1f / ((float)Math.Sqrt(length));

           return new Position(
                (float)(X * normFactor),
                (float)(Y * normFactor),
                (float)(Z * normFactor));
        }

        public static Position operator *(Position a, float scaleFactor)
        {
            Position vector = new Position();
            vector.X = (float)(a.X * scaleFactor);
            vector.Y = (float)(a.Y * scaleFactor);
            vector.Z = (float)(a.Z * scaleFactor);

            return vector;
        }

        public static Position operator +(Position a, Position b)
        {
            Position vector = new Position();
            vector.X = (float)(a.X + b.X);
            vector.Y = (float)(a.Y + b.Y);
            vector.Z = (float)(a.Z + b.Z);

            return vector;
        }

        public float GetDistance(Position point)
        {
            float x = point.X - X;
            float y = point.Y - Y;
            float z = point.Z - Z;
            float dist = ((x * x) + (y * y)) + (z * z);

            return (float)Math.Sqrt(dist);
        }
    }
}
