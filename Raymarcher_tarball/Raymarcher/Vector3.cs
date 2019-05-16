using System;
using System.Text.RegularExpressions;

namespace Raymarcher
{
    public class Vector3
    {
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }

        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static Vector3 Cross(Vector3 a, Vector3 b)
        {
            return new Vector3(a.y *  b.z - a.z * b.y, a.z * b.x - a.x * b.z, a.x * b.y - a.y * b.x);
        }

        public static float Dot(Vector3 a, Vector3 b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }

        public float Magnitude()
        {
            return (float) Math.Sqrt(x * x + y * y + z * z);
        }

        public Vector3 Abs()
        {
            return new Vector3(Math.Abs(x), Math.Abs(y), Math.Abs(z));
        }

        public Vector3 Normalized()
        {
            float mag = Magnitude();
            if (mag == 0)
                return new Vector3(0, 0, 0);
            return new Vector3(x / mag, y / mag, z / mag);
        }

        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static Vector3 operator *(Vector3 a, float b)
        {
            return new Vector3(b * a.x, b * a.y, b * a.z);
        }
        
        public static Vector3 operator *(float a, Vector3 b)
        {
            return new Vector3(a * b.x, a * b.y, a * b.z);
        }

        /*
         * Alias for a cross product (returns Cross(a, b))
         */
        public static Vector3 operator *(Vector3 a, Vector3 b)
        {
            return Cross(a, b);
        }

        /*
         * returns a - b
         */
        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return a + (-b);
        }

        /*
         * Returns the vector -a
         */
        public static Vector3 operator -(Vector3 a)
        {
            return (-1) * a;
        }

        public static Vector3 Max(Vector3 a, float b)
        {
            return new Vector3(Math.Max(a.x, b), Math.Max(a.y, b), Math.Max(a.z, b));
        }

        public static Vector3 Up()
        {
            return new Vector3(0f, 1f, 0f);
        }
    }
}