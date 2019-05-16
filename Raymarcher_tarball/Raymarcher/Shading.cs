using System;
using System.Drawing;

namespace Raymarcher
{
    public class Shading
    {
        private static float Clamp(float value, float min, float max)
        {
            return Math.Max(min, Math.Min(value, max));
        }

        public static Vector3 CalculateNormal(Vector3 point, SDF sdf)
        {
            float epsilon = 0.01f;

            return new Vector3(
                sdf.Evaluate(new Vector3(point.x + epsilon, point.y, point.z))
                - sdf.Evaluate(new Vector3(point.x - epsilon, point.y, point.z)),
                sdf.Evaluate(new Vector3(point.x, point.y + epsilon, point.z))
                - sdf.Evaluate(new Vector3(point.x, point.y - epsilon, point.z)),
                sdf.Evaluate(new Vector3(point.x, point.y, point.z + epsilon))
                - sdf.Evaluate(new Vector3(point.x, point.y, point.z - epsilon))).Normalized();
        }

        private static float ambient = 0.2f;
        private static float lightIntensity = 0.8f;
        
        /*
         * Compute the color of the pixel at the given intersection point. Steps can be used for debuging
         */
        public static Color Shade(Vector3 lightDir, Vector3 point, SDF sdf, int steps)
        {
            
            float scal = Vector3.Dot(-CalculateNormal(point, sdf), lightDir);

            if (scal < 0)
                scal = 0;

            int grey = ((int) (255 * (ambient + lightIntensity * scal)));
            return Color.FromArgb(grey, grey, grey);
        }
    }
}